public class CardComp : Stmt
{
    public Expression Name{get;private set;}
    public Expression Power{get;private set;}
    public Expression Faction{get;private set;}
    public List<Expression> Range{get;private set;}
    public Expression Type{get;private set;}
    public OnActivation OnActv {get; private set;}
    public override Scope AssociatedScope { get; set;}
    public CardComp(Expression name, Expression type, Expression faction, Expression power, List<Expression> range, OnActivation activation, CodeLocation location):base(location)
    {
        this.Name = name;
        this.Type = type;
        this.Faction = faction;
        this.Power = power;
        this.Range = range;
        this.OnActv = activation;
    }
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        //Cada carta tiene su propio scope
        this.AssociatedScope = scope.CreateChild();

        //Se chequean las distintas propiedades de la carta y q sean del tipo valido
        Power.CheckSemantic(context, AssociatedScope, errors);
        if (Power.Type != ExpressionType.Number)
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "El poder de la carta debe ser una expresion de tipo numero"));
            return false;
        }

        Type.CheckSemantic(context, AssociatedScope, errors);
        if(Type.Type != ExpressionType.Text)
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "El tipo de la carta debe ser una expresion de tipo texto"));
            return false;
        }

        Name.CheckSemantic(context, AssociatedScope, errors);
        if(Name.Type != ExpressionType.Text)
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "El nombre de la carta debe ser una expresion de tipo texto"));
            return false;
        }

        Faction.CheckSemantic(context, AssociatedScope, errors);
        if(Faction.Type != ExpressionType.Text)
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "La faccion de la carta debe ser una expresion de tipo texto"));
            return false;
        } 

        //OnActivation 
        bool ValidAct = OnActv.CheckSemantic(context, AssociatedScope, errors);  
        if(ValidAct)
        {
            context.cards.Add((string)Name.Value);
        }
        return ValidAct;
    }
    //Falta interpretar selector y efectos
    public override void Interprete()
    {
        Name.Evaluate();
        Faction.Evaluate();
        Type.Evaluate();
        Power.Evaluate();
    }
    public override string ToString()
    { 
        return String.Format("Card {0} \n\t Power: {1} \n\t Faction: {2} \n\t Type: {3} \n\t OnActivation: {4}", Name, Power, Faction, Type, OnActv);
    }
}