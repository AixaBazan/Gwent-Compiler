/* This node represents a program. The program has an errors list and some cards
 and elements represented by dictionaries. Every Card and Element is acceced by his id */
public class GwentProgram : Stmt
{
    public List<Effect> Effects {get; set;}
    public List<Card> Cards {get; set;}
    public override Scope AssociatedScope {get; set;}
    public GwentProgram(CodeLocation location) : base (location)
    {
        Effects = new List<Effect>();
        Cards = new List<Card>();
    }
    
    /* To check a program semantic we sould first collect all the existing elements and store them in the context.
    Then, we check semantics of elements and cards */
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        this.AssociatedScope = scope;
        bool checkEffects = true;
        foreach (Effect effect in Effects)
        {
            //cada efecto tiene su propio scope
            checkEffects = checkEffects && effect.CheckSemantic(context, AssociatedScope, errors);
        }

        bool checkCards = true;
        foreach (Card card in Cards)
        {
            //cada carta tiene su propio scope
            checkCards = checkCards && card.CheckSemantic(context, AssociatedScope, errors);
        }

        return checkCards && checkEffects;
    }

    public override void Interprete()
    {
        foreach (Effect card in Effects)
        {
            card.Interprete();
        }
    }

    public override string ToString()
    {
        string s = "";
        foreach (Effect effect in Effects)
        {
            s = s + "\n" + effect.ToString();
        }
        foreach (Card card in Cards)
        {
            s += "\n" + card.ToString();
        }
        return s;
    }
}