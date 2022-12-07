
namespace ConsoleApp1.Models
{
    public class ChainDTO : IComponentDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public ComponentType ComponentType { get; set; } = ComponentType.Chain;
        public List<IComponentDTO> DTOLinks { get; set; } = new();
        public double B { get ; set ; }
    }
    public enum ComponentType
    {
        Fracture = 1, Tearing = 3, Fatigue = 2, EAC = 4, Creep = 5, Group = 7, Chain = 6, MaxFlawSize = 8, Recharacterize = 9
    }
}
