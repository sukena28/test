using AutoMapper;
using ConsoleApp1.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ConsoleApp1
{
    public class ChainService
    {
        private readonly IMapper _mapper;

        public ChainService(IMapper mapper)
        {
            _mapper = mapper;
        }

        
        public ChainDTO GetChainRequest()
        {
            var jsonData = GetJsonData();

            var result =  _mapper.Map<ChainDTO>(jsonData);

            return result;
        }

        private JObject GetJsonData()
        {
            var chain = new ChainDTO()
            {
                Id = 1,
                Name = "ChainDTO",
                B = 10,
                ComponentType = ComponentType.Chain,
                DTOLinks = new List<IComponentDTO> {

                    new FractureDto() { Id = 1, Name = "FractureDto", ComponentType = ComponentType.Fracture, B = 20},
                    new ChainDTO() { Id = 1, Name = "ChainDTO Nested", ComponentType = ComponentType.Chain, B = 20 , DTOLinks = new List<IComponentDTO>
                    {
                        new FractureDto() { Id = 1, Name = "FractureDto dd", ComponentType = ComponentType.Fracture, B = 57}
                    }
                    }
                }

            };

            return JObject.Parse(JsonConvert.SerializeObject(chain));
        }
    }
}
