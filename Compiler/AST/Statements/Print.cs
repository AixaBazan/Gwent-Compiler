
public class Print : Stmt
{
    public Expression expression { get; }

    public Print(Expression exp, CodeLocation location) : base(location)
    {
        this.expression = exp;
    }
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        bool check = expression.CheckSemantic(context, scope, errors);
        return check;
    }
    public override void Interprete()
    {
        expression.Evaluate();
        Console.WriteLine(expression.Value);
    }
    public override string ToString()
    {
        return $"print {expression.ToString()}";
    }
}