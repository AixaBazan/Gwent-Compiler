
class Pow : BinaryExpression
{
    public override object? Value{get;set;}
    public override ExpressionType Type { get; set; }
    public Pow(CodeLocation location, Expression left, Expression right) : base(location)
    {
        this.Right = right;
        this.Left = left;
    }
    public override void Evaluate()
    {
        Right.Evaluate();
        Left.Evaluate();

        Value = Math.Pow((double)Left.Value,(double)Right.Value);
    }
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        bool right = Right.CheckSemantic(context, scope, errors);
        bool left = Left.CheckSemantic(context, scope, errors);
        if(Right.Type != ExpressionType.Number || Left.Type != ExpressionType.Number)
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "We don't do that here... "));
            Type = ExpressionType.ErrorType;
            return false;
        }
        Type = ExpressionType.Number;
        return right && left;
    }
    public override string ToString()
    {
        if (Value == null)
        {
            return String.Format("({0} ^ {1})", Left, Right);
        }
        return Value.ToString();
    }
}