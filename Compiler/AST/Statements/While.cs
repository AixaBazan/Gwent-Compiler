using System.Linq.Expressions;
public class While : Stmt
{
    public Expression Condition { get; }
    public Stmt Body { get; }
    Scope scupi{get;set;}
    public While(Expression condition, Stmt body, CodeLocation location):base(location)
    {
        Condition = condition;
        Body = body;
    }
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        this.scupi = new Scope(scope);
        bool cond = Condition.CheckSemantic(context, scope, errors);
        bool body = Body.CheckSemantic(context, scupi, errors);
        if(Condition.Type != ExpressionType.Boolean)
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "El while debe recibir una expresion booleana"));
            return false;
        }
        return cond;
    }
    public override void Interprete()
    {
        Condition.Evaluate();
        System.Console.WriteLine(Condition.Value);
        while ((bool)Condition.Value) 
        {

            Body.Interprete();
            Condition.Evaluate();
            
            // Mensajes de depuración
            //System.Console.WriteLine("Valor de i: " + context.Get("i")); // Asegúrate de que i se esté actualizando
            System.Console.WriteLine("Valor de Condition.Value: " + Condition.Value);
        }
    }
    public override string ToString()
    {
        return "(while(" + Condition + "){" + Body + "})";
    }
}