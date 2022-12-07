
namespace ConsoleApp1.Models
{
    public class GroupDTO : IComponentDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public ComponentType ComponentType { get; set; } = ComponentType.Group;
        public double B { get; set; }
        public List<IComponentDTO> DTOItems { get; set; } = new();


    }
}
