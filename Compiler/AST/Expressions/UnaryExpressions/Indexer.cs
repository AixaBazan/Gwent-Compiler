public class Indexer : UnaryExpression
{
    public Indexer(Expression exp, double index, CodeLocation location): base(location)
    {
        this.expression = exp;
        this.Index = index;
    }
    public Expression expression{ get; set; }
    public double Index { get; set; }
    public override object? Value {get;set;}
    public override ExpressionType Type {get; set;}
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        bool validExp = expression.CheckSemantic(context, scope, errors);
        if(expression.Type != ExpressionType.List)
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Solo se permite indexar en listas"));
            Type = ExpressionType.ErrorType;
            return false;
        }
        Type = ExpressionType.Card;
        return true;
    }
    public override void Evaluate()
    {
        this.Value = Type;
        System.Console.WriteLine("Estoy indexando en una lista");
    }
    public override string ToString()
    {
        return String.Format(expression + "[" + Index + "]");
    }
}