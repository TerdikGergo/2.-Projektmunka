using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace szamitogepboltprojekt
{
    internal class Component
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Specs { get; set; }
        public double Price { get; set; }

        public Component(string type, string name, string specs, double price)
        { 
            Type = type;
            Name = name;
            Specs = specs;
            Price = price;
        }
    }
}
