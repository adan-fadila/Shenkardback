using Card_package;
class CardController
{
    private CardService cardService = CardService.getInstance();
    public ICard getCard(int id){
        return cardService.getCard(id);
    }
    public void updateCardCost(int id, int cost){
        cardService.updateCardCost(id,cost);
    }
    public void updateCardPower(int id, int power){
        cardService.updateCardPower(id,power);
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