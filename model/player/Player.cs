
using Card_package;
using Location_package;

namespace Player_package
{
    public class Player
{
    public string name { get; }
    public int id { get; }
    // public Deck deck { get; }
    public int energy;
    public List<ICard> displayedCards {get; set;}

    public ICard[] cards {get; private set;}
    public ICard selectedCard {get; set;}
    public Location selectedLocation {get;set;}
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
        energy = 0;
    }
    public ICard[] GetCards(){
        return this.cards;
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
