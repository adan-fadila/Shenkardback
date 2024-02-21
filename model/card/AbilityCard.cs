namespace Card_package
{
    public class AbilityCard : ICard
    {
        public string Name { get; }

        public string Desc { get; }

        public int Cost { get; set; }
        public int Power { get; set; }

        public int id { get; private set; }
        public string Image { get; set; }

        public IAbilityStrategy Ability;
        private AbilityFactory abilityFactory = new AbilityFactory();

        public AbilityCard(int id, string Name, string Desc, int Cost, int Power, string Ability,string Image)
        {
            this.Name = Name;
            this.Desc = Desc;
            this.Cost = Cost;
            this.Power = Power;
            this.id = id;
            this.Ability = abilityFactory.generate(Ability);
            this.Image = Image;
        }


        public void ActivateAbility()
        {
            // Call the ability strategy to activate the ability
            this.Ability.ActivateAbility();
        }
       public override string ToString()
        {
            return $"CardId: {this.id} CardName:{this.Name} CardDesc:{this.Desc} CardPower: {this.Power} CardCost:{this.Cost}";
        }
    }
}