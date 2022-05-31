using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pillow.ApplicationCore.Entities.TagAggregate;
using Pillow.ApplicationCore.Interfaces;
using Pillow.ApplicationCore.Specifications;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Pillow.PublicApi.TagEndpoints
{
    [Authorize]
    public class List : BaseAsyncEndpoint<ListTagRequest, ListTagResponse>
    {
        private readonly IAsyncRepository<Tag> _itemRepository;

        private readonly IMapper _mapper;

        public List(IAsyncRepository<Tag> itemRepository,
            IMapper mapper)
        {
            _itemRepository = itemRepository;
            _mapper = mapper;
        }

        [HttpGet("api/tags")]
        [SwaggerOperation(
            Summary = "List Tags",
            Description = "List Tags",
            OperationId = "tag.List",
            Tags = new[] { "TagEndpoints" })
        ]
        public override async Task<ActionResult<ListTagResponse>> HandleAsync([FromRoute]ListTagRequest request,
            CancellationToken cancellationToken)
        {
            var response = new ListTagResponse(request.CorrelationId());

            var filterSpec = new TagFilterSpecification();

            var items = await _itemRepository.ListAsync(filterSpec);

            response.Items.AddRange(items.Select(_mapper.Map<TagDto>));

            return Ok(response);
        }
    }
}