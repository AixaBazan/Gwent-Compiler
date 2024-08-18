
class Var : Stmt
{
    public string Name { get; private set;}
    public Expression InitialValue { get; private set;}
    public Token Operator { get; private set;}
    public override Scope AssociatedScope {get; set;}
    public Var(string name, Expression initializer, Token op, CodeLocation location) : base(location)
    {
        Name = name;
        InitialValue = initializer;
        Operator = op;
    }
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        this.AssociatedScope = scope;
        bool valid = InitialValue.CheckSemantic(context, AssociatedScope, errors);
        if(Operator.Value == TokenValue.Assign)
        {
        AssociatedScope.DefineType(Name, InitialValue.Type);  
        return true;
        }
        if(Operator.Value == TokenValue.Increase || Operator.Value == TokenValue.Decrease)
        {
            if(AssociatedScope.GetType(Name) != ExpressionType.Number)
            {
                errors.Add(new CompilingError(Location, ErrorCode.Invalid, "No se pueden incrementar o decrementar el valor de una variable que no sea tipo Number o que no exista"));
                return false;
            }
            else return true;
        }
        return valid;
    }   
    // Arreglar
    public override void Interprete()
    {
        InitialValue.Evaluate();
        if(Operator.Value == TokenValue.Assign) 
        {
            AssociatedScope.Define(Name, InitialValue.Value);
            return;
        }
        double actualValue = (double)AssociatedScope.Get(Name);
        if (Operator.Value == TokenValue.Increase)
            AssociatedScope.Define(Name, actualValue + (double)InitialValue.Value);
        else if(Operator.Value == TokenValue.Decrease)
            AssociatedScope.Define(Name, actualValue - (double)InitialValue.Value);
    }
    public override string ToString()
    {
        if(InitialValue != null) return Name + "=" + InitialValue + ";";
        return Name;
    }
}