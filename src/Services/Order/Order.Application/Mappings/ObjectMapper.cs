using AutoMapper;

namespace Order.Application.Mappings
{
    public class ObjectMapper
    {
        private static readonly Lazy<IMapper> _lazyMapper = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CustomMapping>();
            });

            return config.CreateMapper();
        });

        public static IMapper Mapper = _lazyMapper.Value;
    }
}