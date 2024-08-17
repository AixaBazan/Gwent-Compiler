using System.Linq.Expressions;
public class While : Stmt
{
    public Expression Condition { get; }
    public Stmt Body { get; }
    public override Scope AssociatedScope {get;set;}
    public While(Expression condition, Stmt body, CodeLocation location):base(location)
    {
        Condition = condition;
        Body = body;
    }
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        this.AssociatedScope = scope.CreateChild();
        
        bool cond = Condition.CheckSemantic(context, scope, errors);
        
        if(Condition.Type != ExpressionType.Boolean)
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "El while debe recibir una expresion booleana"));
            return false;
        }
        bool body = cond && Body.CheckSemantic(context, AssociatedScope, errors);
        return body;
    }
    public override void Interprete()
    {
        Condition.Evaluate();
        while ((bool)Condition.Value) 
        {
            Body.Interprete();
            Condition.Evaluate();
        }
    }
    public override string ToString()
    {
        return "(while(" + Condition + "){" + Body + "})";
    }
}