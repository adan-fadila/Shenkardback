using System;
using System.Diagnostics;


namespace Card_package
{
    public class CardFactory : ICardFactory
    {
        private readonly cardDataAccess cardDataAccess = cardDataAccess.getInstance();
        public CardFactory()
        {

        }

        public ICard generate(int id)
        {
            string CardType = cardDataAccess.getCardType(id).ToLower();
            string CardDesc = cardDataAccess.getCardDesc(id);
            int CardCost = cardDataAccess.getCardCost(id);
            int CardPower = cardDataAccess.getCardPower(id);
            string CardName = cardDataAccess.getCardName(id);

            string Ability = null;
            if (CardType.Equals("master") || CardType.Equals("ability"))
            {
                Ability = cardDataAccess.getCardAbility(id);
            }

            switch (CardType)
            {
                case "master":
                    return new MasterCard(id, CardName, CardDesc, CardPower, Ability);
                case "ability":
                    return new AbilityCard(id, CardName, CardDesc, CardCost, CardPower, Ability);

                case "regular":
                    return new RegularCard(id, CardName, CardDesc, CardCost, CardPower);
                default:
                    throw new ArgumentException($"Invalid card type {id}");
            }
        }
    }
}
