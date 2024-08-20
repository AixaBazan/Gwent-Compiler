
class AssignEffect : Stmt
{
    public Expression Name {get; private set;}

    public List<(string, Expression)> Params {get; private set;}
    public Selector selector {get; private set;}    
    //public AssignEffect PostAction {get; private set;}
    public AssignEffect(Expression name, List<(string, Expression)> param, Selector sel, CodeLocation location) : base(location)
    {

    }
    public override Scope AssociatedScope {get;set;}
    public bool PostActionCheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        return true;
    } 
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        this.AssociatedScope = scope;

        //Se verifica que la expresion del nombre sea de tipo texto
        Name.CheckSemantic(context, AssociatedScope, errors);
        if(Name.Type != ExpressionType.Text)
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "El nombre del efecto debe ser de tipo Texto"));
            return false;
        }
        Name.Evaluate();
        if(!context.effects.Contains(Name.Value))
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "El efecto que le desea asignar a la carta no existe, debe declararlo"));
            return false;
        }
        return true;
    }
    public override void Interprete()
    {
        throw new NotImplementedException();
    }
    public override string ToString()
    {
        return base.ToString();
    }
}