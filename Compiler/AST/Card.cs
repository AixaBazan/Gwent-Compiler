public class Card : AST
{
    public Expression Name{get;private set;}
    public Expression Power{get;private set;}
    public Expression Faction{get;private set;}
    public List<Expression> Range{get;private set;}
    public Expression Type{get;private set;}
    public Card(Expression name, Expression type, Expression faction, Expression power, List<Expression> range ,CodeLocation location):base(location)
    {
        this.Name = name;
        this.Type = type;
        this.Faction = faction;
        this.Power = power;
        this.Range = range;
    }
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        bool checkPower = Power.CheckSemantic(context, scope, errors);
        bool checkType = Type.CheckSemantic(context, scope, errors);
        bool checkName = Name.CheckSemantic(context, scope, errors);
        bool checkFaction = Faction.CheckSemantic(context, scope, errors);
        if (Power.Type != ExpressionType.Number)
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "El poder de la carta debe ser una expresion de tipo numero"));
            return false;
        }
        if(Name.Type != ExpressionType.Text)
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "El nombre de la carta debe ser una expresion de tipo texto"));
            return false;
        }
        if(Type.Type != ExpressionType.Text)
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "El tipo de la carta debe ser una expresion de tipo texto"));
            return false;
        }
        if(Faction.Type != ExpressionType.Text)
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "La faccion de la carta debe ser una expresion de tipo texto"));
            return false;
        }        
        return checkPower && checkName && checkType && checkFaction;
    }
    public void Interprete()
    {
        Name.Evaluate();
        Faction.Evaluate();
        Type.Evaluate();
        Power.Evaluate();
    }
    public override string ToString()
    { 
        return String.Format("Card {0} \n\t Power: {1} \n\t Faction: {2} \n\t Type: {3}", Name, Power, Faction, Type);
    }
}