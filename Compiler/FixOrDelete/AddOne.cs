class AddOne : UnaryExpression
{
    public AddOne(string name, Expression variable, Token op, CodeLocation location) : base(location)
    {
        this.Name = name;
        this.variable = variable;
        this.Operator = op;
    }
    string Name { get; set; }
    public override object? Value{get; set;}
    public Expression variable { get; set;}
    public Token Operator { get; private set;}
    public override ExpressionType Type{get;set;}
    Scope AssociatedScope {get; set;}
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        this.AssociatedScope = scope;
        bool fine = variable.CheckSemantic(context, scope, errors);
        if(variable.Type != ExpressionType.Number)
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "No se le puede aplicar ++ ni -- a una propiedad que no devuelva un numero"));
            Type = ExpressionType.ErrorType;
            return false;
        }
        Type = ExpressionType.Number;
        return true;
    }
    public override void Evaluate()
    {
        variable.Evaluate();
        if(Operator.Value == TokenValue.Increase)
        {
        if(variable is Property)
        {
            this.Value = (double)variable.Value + 1;
            return;
        }
        AssociatedScope.Define(Name, (double)variable.Value + 1);
        this.Value = AssociatedScope.Get(Name);
        }
        else
        {
        if(variable is Property)
        {
            this.Value = (double)variable.Value - 1;
            return;
        }
        AssociatedScope.Define(Name, (double)variable.Value - 1);
        this.Value = AssociatedScope.Get(Name);
        }
        System.Console.WriteLine(variable + " tipo: " + Type);
    }
    public override string ToString()
    {
        return String.Format("{0}",variable);
    }
}