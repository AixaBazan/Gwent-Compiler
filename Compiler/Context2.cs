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
    field, //campo
    otherField,
    parent //solo permitida en el postAction
}