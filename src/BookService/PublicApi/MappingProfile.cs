using System.Linq;
using AutoMapper;
using Pillow.ApplicationCore.Entities.BookAggregate;
using Pillow.ApplicationCore.Entities.TagAggregate;
using Pillow.PublicApi.BookEndpoints;
using Pillow.PublicApi.TagEndpoints;
using Pillow.PublicApi.TrackEndpoints;
using Pillow.PublicApi.UsersEndpoints.FavoriteBooks;

namespace Pillow.PublicApi
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Track, TrackDto>();
            CreateMap<Tag, TagDto>();
            CreateMap<Book, BookDto>()
                .ForMember(
                    dto => dto.Tags,
                    opt => opt.MapFrom(book => book.BookTag.Select(bt => bt.Tag)))
                .ForMember(
                    dto => dto.Tracks,
                    opt => opt.MapFrom(book => book.Tracks.Where(track => !track.IsTrailer))
                )
                .ForMember(
                    dto => dto.Trailer,
                    opt => opt.MapFrom(book => book.Tracks.FirstOrDefault(track => track.IsTrailer))
                );

            CreateMap<Book, FavoriteBookDto>()
                .ForMember(
                    dto => dto.Tags,
                    opt => opt.MapFrom(book => book.BookTag.Select(bt => bt.Tag)))
                .ForMember(
                    dto => dto.Tracks,
                    opt => opt.MapFrom(book => book.Tracks.Where(track => !track.IsTrailer))
                )
                .ForMember(
                    dto => dto.Trailer,
                    opt => opt.MapFrom(book => book.Tracks.FirstOrDefault(track => track.IsTrailer))
                );
        }
    }
}
