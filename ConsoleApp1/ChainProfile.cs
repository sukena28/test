using AutoMapper;
using ConsoleApp1.Models;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace ConsoleApp1
{
    public class ChainProfile : Profile
    {
        public ChainProfile()
        {
            CreateMap<JObject, ChainDTO>()
                        .ConvertUsing<JObjectChainDTOConverter>();


            CreateMap<JObject, GroupDTO>()
              .ConvertUsing<JObjectGroupConverter>();

        }
        public class JObjectGroupConverter : ITypeConverter<JObject, GroupDTO>
        {
            public GroupDTO Convert(JObject json, GroupDTO destination, ResolutionContext context)
            {
                return LinkedComponentTypeConverter.Convert(json, destination, context);
            }
        }
        public class JObjectChainDTOConverter : ITypeConverter<JObject, ChainDTO>
        {
            public ChainDTO Convert(JObject json, ChainDTO destination, ResolutionContext context)
            {
                return LinkedComponentTypeConverter.Convert(json, destination, context);
            }
        }
        public static class LinkedComponentTypeConverter
        {
            public static DestinationType Convert<DestinationType>(JObject jsonChain, DestinationType destination, ResolutionContext context)
            {
                var componentListPropertyName  = GetPropertyName(typeof(DestinationType), typeof(List<IComponentDTO>));

                var componentListJson = jsonChain[componentListPropertyName];

                jsonChain[componentListPropertyName]?.Parent?.Remove();

                var component = jsonChain.ToObject<DestinationType>();

                var componentList = GetComponentList(componentListJson, destination, context);

                SetPropertyByName(component, componentListPropertyName, componentList);

                return component;
            }
            private static List<IComponentDTO> GetComponentList<DestinationType>(JToken? linksDto, DestinationType destination, ResolutionContext context)
            {

                if (linksDto == null)
                    return new List<IComponentDTO>();

                var componentTypePropertyName = GetPropertyName(typeof(DestinationType), typeof(List<ComponentType>));


                var componenteTypeClass = typeof(IComponentDTO)
                                .Assembly.GetTypes()
                                .Where(p => typeof(IComponentDTO).IsAssignableFrom(p) && p.IsClass && p != null).ToList()
                                .Select(t => (IComponentDTO)Activator.CreateInstance(t)).ToList();


                if (componenteTypeClass?.Count == 0)
                    throw new NotSupportedException($"Component Type is empty");


                var componentList = linksDto.Select(item =>
                {

                    var componentType = item[$"{componentTypePropertyName}"]?.ToObject<ComponentType>();

                    Type classType = componenteTypeClass.FirstOrDefault(x => x.ComponentType == componentType).GetType();

                    if (classType == null) { throw new NotSupportedException($"Component Type is empty"); }

                    var component = (IComponentDTO)context.Mapper.Map(item?.ToObject<JObject>(), typeof(JObject), classType);

                    return component;

                }).ToList();

                return componentList;

            }
            private static string GetPropertyName(Type objectType, Type propType)
            {
                return objectType.GetProperties()
                                  .FirstOrDefault(property => property.PropertyType
                                  .Equals(propType))?.Name ?? "";
            }
            public static void SetPropertyByName(Object obj, string name, Object value)
            {
                var property = obj.GetType().GetProperty(name, BindingFlags.Public | BindingFlags.Instance);

                if (property != null && property.CanWrite)
                    property.SetValue(obj, value, null);
            }

        }
    }
}
