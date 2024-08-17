
class Var : Stmt
{
    public string Name { get; private set;}
    public Expression InitialValue { get; private set;}
    public override Scope AssociatedScope {get; set;}
    public Var(string name, Expression initializer, CodeLocation location) : base(location)
    {
        Name = name;
        InitialValue = initializer;
    }
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        this.AssociatedScope = scope;
        InitialValue.CheckSemantic(context, AssociatedScope, errors);
        AssociatedScope.DefineType(Name, InitialValue.Type);  
        return true;
    }   
    public override void Interprete()
    {
        InitialValue.Evaluate();
        AssociatedScope.Define(Name, InitialValue.Value);
    }
    public override string ToString()
    {
        if(InitialValue != null) return Name + "=" + InitialValue + ";";
        return Name;
    }
}