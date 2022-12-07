using AutoMapper;
using ConsoleApp1.Models;
using Newtonsoft.Json.Linq;

namespace ConsoleApp1
{
    public class ChainProfile : Profile
    {
        public ChainProfile()
        {
            CreateMap<JObject, ChainDTO>()
                        .ConvertUsing<JObjectChainConverter>();



        }
        public class JObjectChainConverter : ITypeConverter<JObject, ChainDTO>
        {
            public ChainDTO Convert(JObject jsonChain, ChainDTO destination, ResolutionContext context)
            {
                var linksDtoKey = $"{nameof(ChainDTO.DTOLinks)}";

                var linksJson = jsonChain[linksDtoKey];

                jsonChain[linksDtoKey]?.Parent?.Remove();

                var chainDto = jsonChain?.ToObject<ChainDTO>() ?? new ChainDTO();

                chainDto.DTOLinks = GetLinkedList(linksJson, destination, context);

                return chainDto;
            }
            private static List<IComponentDTO> GetLinkedList(JToken? linksDto, ChainDTO destination, ResolutionContext context)
            {

                if (linksDto == null)
                    return new List<IComponentDTO>();

                var componenteTypeClass = typeof(IComponentDTO)
                                .Assembly.GetTypes()
                                .Where(p => typeof(IComponentDTO).IsAssignableFrom(p) && p.IsClass && p != null).ToList()
                                .Select(t => (IComponentDTO)Activator.CreateInstance(t)).ToList();


                if(componenteTypeClass?.Count == 0)
                    throw new NotSupportedException($"Component Type is empty");


                var componentList = linksDto.Select(item =>
                {
                    var componentType = item[$"{nameof(IComponentDTO.ComponentType)}"]?.ToObject<ComponentType>();

                    Type classType = componenteTypeClass!.FirstOrDefault(x => x!.ComponentType == componentType).GetType();

                    if (classType == null) { throw new NotSupportedException($"Component Type is empty"); }

                   var component =  (IComponentDTO)context.Mapper.Map(item?.ToObject<JObject>(), typeof(JObject), classType);

                    return component;

                }).ToList();

                return componentList;

            }
        }
    }
}
