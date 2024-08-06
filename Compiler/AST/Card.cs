public class Card : AST
{
    public string Name{get;private set;}
    public Expression? Power{get;private set;}
    public string Faction{get;private set;}
    public List<string> PlayZone{get;private set;}
    public string Type{get;private set;}
    public List<string> cardEffects{get;private set;}
    public Card(string name, string faction, string type, CodeLocation location):base(location)
    {
        this.Name = name;
        this.Faction = faction;
        this.Type = type;
        this.cardEffects = new List<string>();
        this.PlayZone = new List<string>();
    }
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        // We check the Power's Expression semantic 
        bool checkPower = Power.CheckSemantic(context, scope, errors);
        if (Power.Type != ExpressionType.Number)
        {
            // The Power's type can only be numerical, otherwise we send an error
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "The Power must be numerical"));
        }

        /* Then we check each card effect and send error if this effect doesn't exixts or
        was already used. Finally, we add the effect to the scope to say that it has been used */
        bool checkEffects = true;
        foreach (string effect in cardEffects)
        {
            if (!context.effects.Contains(effect))
            {
                errors.Add(new CompilingError(Location, ErrorCode.Invalid, String.Format("{0} effect Does not exists", effect)));
                checkEffects = false;
            }
            if (scope.effects.Contains(effect))
            {
                errors.Add(new CompilingError(Location, ErrorCode.Invalid, String.Format("{0} element already in use", effect)));
                checkEffects = false;
            }
            else
            {
                scope.effects.Add(effect);
            }
        }
        bool CheckGameZone = true;
        
        return checkPower && checkEffects;
    }
}