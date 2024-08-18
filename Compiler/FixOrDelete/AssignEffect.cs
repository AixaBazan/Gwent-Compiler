
class AssignEffect : Stmt
{
    public Expression Name {get; private set;}

    //faltan los params
    public Selector selector {get; private set;}    
    //public AssignEffect PostAction {get; private set;}
    public AssignEffect(Expression name, Selector sel, CodeLocation location) : base(location)
    {

    }
    public override Scope AssociatedScope {get;set;}
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        throw new NotImplementedException();
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