using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Models
{
    public interface IComponentDTO
    {
        public string Name { get; set; }
        public ComponentType ComponentType { get; set; }
        public double B { get; set; }
    }
}
