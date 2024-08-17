class Effect : AST
{
    public Expression Name {get;private set;}
    //public Dictionary<Token, Token> Params {get;private set;}
    public Token Targets {get; private set;}
    public Token Context {get; private set;}
    public Stmt Body {get; private set;}
    public Scope AssociatedScope {get; private set;}
    public Effect(Expression name, Token targets, Token context, Stmt body, CodeLocation location) : base(location)
    {
        this.Name = name;
        this.Targets = targets;
        this.Context = context;
        this.Body = body;
        //this.Params = param;
    }
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        this.AssociatedScope = scope.CreateChild(); //Se crea el scope del efecto
        
        //Se verifica que la expresion del nombre sea de tipo texto
        bool ValidName = Name.CheckSemantic(context, AssociatedScope, errors);
        if(Name.Type != ExpressionType.Text)
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "El nombre del efecto debe ser de tipo Texto"));
            return false;
        }

        //Se annaden al scope las variables que se le pasan al action como parametro
        if(AssociatedScope.GetType(Targets.Value) != ExpressionType.ErrorType || AssociatedScope.GetType(Context.Value) != ExpressionType.ErrorType)
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "La variable ya ha sido declarada"));
            return false;
        }
        AssociatedScope.DefineType(Targets.Value, ExpressionType.List);
        AssociatedScope.DefineType(Context.Value, ExpressionType.Context);

        //Se chequea semanticamente el cuerpo del action
        bool ValidAction = Body.CheckSemantic(context, AssociatedScope, errors);
        return ValidName && ValidAction;
    }
    public void Interprete()
    {
        Body.Interprete();
    }
    public override string ToString()
    {
        // Obtener el nombre del efecto
        string effectName = Name.ToString(); // Suponiendo que Name tiene un método ToString() que devuelve el nombre

        // Obtener el cuerpo de la acción
        string actionBody = Body.ToString(); // Suponiendo que Body tiene un método ToString() que devuelve el cuerpo de la acción

        // Formatear la salida
        return $"effect\n{{\n  Name: \"{effectName}\",\n  Action: {actionBody}\n}}";
    }
}