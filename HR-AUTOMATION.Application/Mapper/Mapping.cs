using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace HR_AUTOMATION.Application.Mapper
{
    /// <summary>
    /// A static class for mapping configuration using AutoMapper. 
    /// This class sets up a lazy-loaded instance of the IMapper.
    /// </summary>
    public static class Mapping
    {
        /// <summary>
        /// For deferred initialization of the IMapper.
        /// </summary>
        private static readonly Lazy<IMapper> Lazy = new(() =>
        {
            ILoggerFactory loggerFactory = NullLoggerFactory.Instance;

            MapperConfiguration config = new(cfg =>
            {
                /// This line ensures that internal properties are also mapped over.
                cfg.ShouldMapProperty =
                    property => property.GetMethod != null && (property.GetMethod.IsPublic || property.GetMethod.IsAssembly);

                cfg.AddProfile<MappingProfile>();
            }, loggerFactory);

            config.AssertConfigurationIsValid();

            IMapper mapper = config.CreateMapper();

            return mapper;
        });

        /// <summary>
        /// Gets the singleton instance of IMapper, initialized lazily.
        /// The IMapper instance is created when accessed for the first time.
        /// </summary>
        public static IMapper Mapper => Lazy.Value;
    }
}