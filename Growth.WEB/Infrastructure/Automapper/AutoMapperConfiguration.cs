using AutoMapper;
using Growth.BLL.Infrastructure.Automapper;

namespace Growth.WEB.Infrastructure.Automapper
{
    /// <summary>
    /// Class for attaching AutoMapper profiles
    /// </summary>
    public class AutoMapperConfiguration
    {
        /// <summary>
        /// Configures an AutoMapper instance
        /// </summary>
        /// <returns></returns>
        public MapperConfiguration Configure()
        {
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<DtoToApiModelProfile>();
                cfg.AddProfile<ApiModelToDtoProfile>();

                ServiceAutoMapperConfiguration.Initialize(cfg);
            });

            return mapperConfiguration;
        }
    }
}