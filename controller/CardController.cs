using Card_package;
class CardController
{
    private CardService cardService = CardService.getInstance();
    private ICard getCard(int id){
        return cardService.getCard(id);
    }
    public void updateCard(int id, int cost,int power){
        cardService.updateCard(id,cost,power);
    }
    public void deleteCard(int id){
        cardService.deleteCard(id);
    }
    public void createCard(string name, string desc, int cost, int power){
        cardService.createCard(name,desc,cost,power);
    }
    public List<ICard> getCards(){
       return cardService.getCards();
    }
}