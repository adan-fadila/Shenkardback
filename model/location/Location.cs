using System.Collections.Generic;
using Card_package;
using Game_package;
using Player_package;
namespace Location_package
{

    public class Location : ILocation
    {

        public int id { get; set; }
        public string Name { get; }
        public string Desc { get; }
        // public List<Zone> zones { get; }
        public Zone zone1 { get; private set; }
        public Zone zone2 { get; private set; }
        public bool revealed { get; set; }

        public ILocationBattleStrategy battleStrategy { get; }

        public Location(int id, string name, string desc, ILocationBattleStrategy locationBattleStrategy)
        {
            this.id = id;
            this.Name = name;
            this.Desc = desc;
            this.battleStrategy = locationBattleStrategy;
            this.revealed = false;

        }
        public void setPlayer(int p1, int p2)
        {
            this.zone1 = new Zone(p1);
            this.zone2 = new Zone(p2);
        }

        public void reveal()
        {
            this.revealed = true;
        }
        public void applyEffect(Game game)
        {
            if (this.revealed)
            {
                this.battleStrategy.activate(game, this);
            }
        }
        public void playCard(Player player, ICard card)
        {
            Zone playerZone = getPlayerZone(player.id);
            playerZone.setCard(card);
            player.displayedCards.Remove(card);
            player.energy -= card.Cost;


        }
        private Zone getPlayerZone(int player)
        {
            if (this.zone1.Player == player) { return zone1; }
            return zone2;
        }
         string ZonePrint(Zone zone)
        {
            if (zone != null)
            {
                string result = "";
                foreach (ICard card in zone.GetCards())
                {
                    result += card.ToString() + "\n ";
                }
                // Remove the last comma and space
                if (zone.GetCards().Count > 0)
                {
                    result = result.Substring(0, result.Length - 2);
                }
                result += " Total:" + zone.total;
                return result;
            }
            return " no Cards,";
        }
        public override string ToString()
        {
            return $"Location Name: {this.Name} LocationDesc: {this.Desc} LocationZone1: {this.ZonePrint(zone1)} LocationZone2: {this.ZonePrint(zone2)}";
        }

    }
    public class Zone
    {
        private const int cards_capacity = 4;
        private int cards_count = 0;
        private List<ICard> cards;
        public int total { get; set; }
        public int Player { get; }
        public Zone(int player)
        {

            cards = new List<ICard>();
            this.total = 0;
            this.Player = player;
        }
        public List<ICard> GetCards()
        {
            return this.cards;
        }
        public bool setCard(ICard card)
        {
            if (CanPlaceCardInZone())
            {
                this.cards.Add(card);
                if (card != null)
                {
                    updateTotal(card.Power);
                }
                return true;
            }
            return false;

        }
        public bool CanPlaceCardInZone()
        {
            return this.cards.Count < cards_capacity;
        }
        public void updateTotal(int cardPower)
        {
            this.total += cardPower;
        }
        public ICard GetLastCard()
        {
            cards_count--;

            return this.cards[cards_count];
        }

    }
}