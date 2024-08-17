//La clase context contiene las propiedades a la q se puede acceder cada tipo
public class Context
{
    public Dictionary<string, ExpressionType> contextProperties = new Dictionary<string, ExpressionType>
    {
        {"Hand" , ExpressionType.List}, {"Field", ExpressionType.List}, {"Graveyard", ExpressionType.List},
        {"TriggerPlayer", ExpressionType.Number}, {"Board", ExpressionType.List}, {"Deck", ExpressionType.List}
    };
    public Dictionary<string, ExpressionType> ContextMethods = new Dictionary<string, ExpressionType>
    {
        {"HandOfPlayer", ExpressionType.List}, {"FieldOfPlayer", ExpressionType.List}, {"GraveyardOfPlayer", ExpressionType.List},
        {"DeckOfPlayer", ExpressionType.List}
    };
    public Dictionary<string, ExpressionType> cardProperties = new Dictionary<string, ExpressionType>
    {
        {"Name", ExpressionType.Text}, {"Faction", ExpressionType.Text}, {"Type", ExpressionType.Text},
        {"Range", ExpressionType.List}, {"Owner", ExpressionType.Number}, {"Power", ExpressionType.Number}
    };
    public Dictionary<string, ExpressionType> ListMethodsWithParams = new Dictionary<string, ExpressionType>
    {
        {"Find", ExpressionType.List}, {"Push", ExpressionType.Function}, {"SendBottom", ExpressionType.Function},
        {"Remove", ExpressionType.Function}
    };
    public Dictionary<string, ExpressionType> ListMethodsWithoutParams  = new Dictionary<string, ExpressionType>
    {
        {"Pop", ExpressionType.Card}, //tambien quita la carta de la lista
        {"Shuffle", ExpressionType.Function}
    };
    
    // public List<string> effects;
    // public List<string> cards;

    // public Context()
    // {
    //     effects = new List<string>();
    //     cards = new List<string>();
    // }
}