class ContextGame
{
    public int TriggerPlayer{ get; set; } // retorna el Id del jugador q esta en su turno
    public List<Card> Board{ get; set; } //retorna todas las listas del campo, hacer metodo 
    public List<Card> HandOfPlayer(Player player)
    {
        return player.Hand;
    }
    public List<Card> FieldOfPlayer(Player player)
    {
        return player.Field;
    }
    public List<Card> GraveyardOfPlayer(Player player)
    {
        return player.Cementery;
    }
}
enum ValidCardType
{
    Oro,
    Plata,
    Clima,
    Aumento,
    Lider
}
enum ValidRange
{
    Melee,
    Siege,
    Ranged
}
enum ValidSource //fuente de donde se sacan las cartas
{
    hand,
    otherHand,
    deck,
    otherDeck,
    field, 
    otherField,
    parent //solo permitida en el postAction
}