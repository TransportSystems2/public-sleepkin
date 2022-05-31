using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pillow.ApplicationCore.Constants;
using Pillow.ApplicationCore.Entities.BookAggregate;
using Pillow.ApplicationCore.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace Pillow.PublicApi.TrackEndpoints
{
    [Authorize (Roles=AuthorizationConstants.Roles.Moderator)]
    public class Add : BaseAsyncEndpoint<AddTrackRequest, AddTrackResponse>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        private readonly IUriComposer _uriComposer;
        private readonly IAudioFileGuide _audioFileGuide;
        private readonly ILogger<Add> _logger;

        private const string AudioPrefixMimeType = "audio/";
        
        public Add(IBookRepository bookRepository,
            IMapper mapper,
            IUriComposer uriComposer,
            IAudioFileGuide audioFileGuide,
            ILogger<Add> logger)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
            _uriComposer = uriComposer;
            _audioFileGuide = audioFileGuide;
            _logger = logger;
        }

        [HttpPost("api/books/{BookCode}/Tracks")]
        [SwaggerOperation(
            Summary = "Add a track file to book",
            Description = "Add a track file to book",
            OperationId = "tracks.Add",
            Tags = new[] { "TrackEndpoints" })
        ]
        [ProducesResponseType(typeof(AddTrackRequest), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [RequestSizeLimit(50_000_000)]
        public override async Task<ActionResult<AddTrackResponse>> HandleAsync([FromForm]AddTrackRequest request,
            CancellationToken cancellationToken)
        {
            AddTrackResponse response = new AddTrackResponse(request.CorrelationId());

            if (!request.UploadedTrack.ContentType.StartsWith(AudioPrefixMimeType))
            {
                return BadRequest($"UploadedTrack has invalid MimeType: {request.UploadedTrack.ContentType}");
            }

            Book book = await _bookRepository.GetByCodeAsync(request.BookCode);
            if (book is null)
            {
                return NotFound(request.BookCode);
            }

            IFormFile uploadTrack = request.UploadedTrack;

            string uploadTrackExtenstion = Path.GetExtension(uploadTrack.FileName).Trim('.');
            string trackGuid = Guid.NewGuid().ToString();
            string tempTrackPath = _uriComposer.ComposeTempTrackFilePath(trackGuid, uploadTrackExtenstion);
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(tempTrackPath));
                _logger.LogInformation("Created temporary track path: {TempTrackPath}", tempTrackPath);

                await using FileStream fileStream = new FileStream(tempTrackPath, FileMode.Create);
                _logger.LogInformation("Open file stream: {TempTrackPath}, lenght:{Lenght}", 
                    tempTrackPath, fileStream.Length);

                await uploadTrack.CopyToAsync(fileStream, cancellationToken);
                _logger.LogInformation("Saved stream to temporary file: {TempTrackPath}", tempTrackPath);

                IAudioFile fileInfo = _audioFileGuide.TryToCreate(tempTrackPath);
                Track newTrack = new Track(
                    trackGuid,
                    request.BookCode,
                    uploadTrack.FileName,
                    request.Title,
                    uploadTrackExtenstion,
                    uploadTrack.Length,
                    fileInfo?.Duration ?? TimeSpan.FromSeconds(request.DefaultDurationTimeInSeconds),
                    request.IsTrailer,
                    request.HasBackgroundMusic);

                book.AddTrack(newTrack);
                await _bookRepository.UpdateAsync(book);
                
                _logger.LogInformation("Saved track entity Guid: {TrackGuid}", trackGuid);
                
                string trackPath = _uriComposer.ComposeTrackFilePath(trackGuid, uploadTrackExtenstion);
                Directory.CreateDirectory(Path.GetDirectoryName(trackPath));
                _logger.LogInformation("Created track path: {TrackPath}", trackPath);

                System.IO.File.Move(
                    tempTrackPath,
                    trackPath);
                _logger.LogInformation("Track has been moved from tempTrackPath:{TempTrackPath} to permanent one: {TrackPath}",
                    tempTrackPath, trackPath);

                response.Track = _mapper.Map<TrackDto>(newTrack);
                response.Track.Path = _uriComposer.ComposeTrackUri(
                    newTrack.BookCode,
                    newTrack.Code,
                    newTrack.Format);
            }
            finally
            {
                if (System.IO.File.Exists(tempTrackPath))
                {
                    System.IO.File.Delete(tempTrackPath);
                    _logger.LogInformation("Removed tempTrack:{TempTrackPath}", tempTrackPath);
                }
            }

            return Ok(response);
        }
    }
}