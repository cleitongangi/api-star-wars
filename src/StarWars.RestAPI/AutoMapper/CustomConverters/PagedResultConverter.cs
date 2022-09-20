using AutoMapper;
using StarWars.Domain.Core.Pagination;

namespace StarWars.RestAPI.AutoMapper.CustomConverters
{
    public class PagedResultConverter<TSource, TDestination> : ITypeConverter<PagedResult<TSource>, PagedResult<TDestination>>
        where TSource : class
        where TDestination : class
    {
        public PagedResult<TDestination> Convert(PagedResult<TSource> source, PagedResult<TDestination> destination, ResolutionContext context)
        {
            var destinationResult = context.Mapper.Map<IList<TSource>, IList<TDestination>>(source.Results);
            return new PagedResult<TDestination>((IList<TDestination>)destinationResult, source.CurrentPage, source.PageSize);
        }
    }
}
