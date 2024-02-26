using System;
using System.Collections;
using System.Collections.Generic;

namespace Card_package
{
    public class RegularCard : ICard
    {
        public string Name { get; }
        public string Desc { get; }
        public int Cost { get; set; }
        public int Power { get; set; }

        public int id { get; private set; }
        public string Image { get; set; }

        public RegularCard(int id, string Name, string Desc, int Cost, int Power, string Image)
        {
            this.id = id;
            this.Name = Name;
            this.Desc = Desc;
            this.Cost = Cost;
            this.Power = Power;
            this.Image = Image;
        }
        public override string ToString()
        {
            return $"CardId: {this.id}\n";
        }

    }
}
