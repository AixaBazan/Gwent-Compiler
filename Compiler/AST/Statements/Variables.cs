
class Var : Stmt
{
    public Token Name { get; private set;}
    public Token Operation {get; private set;}
    public Expression Initializer { get; private set;}

    Scope environment{get; set;}
    public Var(Token name, Token operation, Expression initializer, CodeLocation location) : base(location)
    {
        Name = name;
        Initializer = initializer;
        Operation = operation;
        //environment = env;
    }
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        this.environment = scope;
        if (Initializer != null) 
        {
            Initializer.CheckSemantic(context, scope, errors);
            Initializer.Evaluate();
            scope.Define(Name.Value, Initializer.Value); 
        }
        else  scope.Define(Name.Value ,null);
        return true;
    }   
    public object? GiveValue() 
    {
        Interprete();
        return environment.Get(Name);
    }
    public override void Interprete()
    {
        Initializer.Evaluate();
        environment.Define(Name.Value, Initializer.Value);
        System.Console.WriteLine("Lo busque " + environment.Get(Name));
    }
    public override string ToString()
    {
        if(Initializer != null) return Name.Value + "=" + Initializer;
        return Name.Value;
    }
}