
using Card_package;
using Location_package;

namespace Player_package
{
    public class Player
    {
        public string name { get; }
        public int id { get; }
        // public Deck deck { get; }
        public int energy { get; set; }
        public List<ICard> displayedCards { get; set; }

        public ICard[] cards { get; private set; }
        public ICard selectedCard { get; set; }
        public Location selectedLocation { get; set; }
        public Player(int id, string name, bool hasDeck, ICard[] cards)
        {

            this.id = id;
            this.name = name;
            // deck = new Deck();
            displayedCards = new List<ICard>();
            this.cards = cards;
            if (hasDeck == false)
            {
                // deck.setDefaultDeck();
            }
            energy = 3;
        }
        public ICard[] GetCards()
        {
            return this.cards;
        }
        string HandPrint()
        {
            if (displayedCards != null)
            {
                string result = "";
                foreach (ICard card in displayedCards)
                {
                    result += card.ToString() + "\n ";
                }
                // Remove the last comma and space
                if (displayedCards.Count > 0)
                {
                    result = result.Substring(0, result.Length - 2);
                }
                return result;
            }
            return " no Cards,";
        }
        public override string ToString()
        {
            return $"name: {this.name} HandCards: {this.HandPrint()} energy:{this.energy}";
        }


    }
    // public class Deck
    // {
    //     private CardService cardService = CardService.getInstance();

    //     static int numOfMasterCards = 1;
    //     static int numOfAllCards = 3;
    //     public ICard[] cards { get;private set; }
    //     public Deck(){
    //         this.cards = new ICard[numOfAllCards];
    //     }
    //     // public void setDefaultDeck()
    //     // {
    //     //     this.cards = this.cardService.getRandomCards(1, 1, numOfMasterCards);

    //     // }

    // }
}
