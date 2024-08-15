class Assign : Expression
{
    public Token Name { get; }
    public Expression Value_ { get; }
    public Assign(Token name, Expression value, CodeLocation location): base(location)
    {
        Name = name;
        Value_ = value;
    }
    public override ExpressionType Type{get;set;}
    public override object? Value{get;set;}
    public override void Evaluate()
    { 
        Value_.Evaluate();
        this.Value = Value_.Value;
    }
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        return true;
    }
}