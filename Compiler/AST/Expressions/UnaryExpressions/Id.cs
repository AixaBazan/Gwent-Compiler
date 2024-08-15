class Variable: UnaryExpression
{
    public Variable(Token variable, CodeLocation location) : base(location)
    {
        this.variable = variable;
    }
    public override object? Value{get; set;}
    public Token variable { get; set;}
    public override ExpressionType Type{get;set;}
    Scope scupi {get; set;}
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        this.scupi = scope;
        if(scope.Get(variable) == null) 
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "La varible a la que desea acceder no se le ha asignado un valor aun"));
            Type = ExpressionType.ErrorType;
            return false;
        }
        Value = scope.Get(variable);
        if(Value is string) this.Type = ExpressionType.Text;
        else if(Value is double) this.Type = ExpressionType.Number;
        else if(Value is bool) this.Type = ExpressionType.Boolean;
        return true;
    }
    public override void Evaluate()
    {
        this.Value = scupi.Get(variable);
    }
    public override string ToString()
    {
        return String.Format("{0}",variable.Value);
    }
}