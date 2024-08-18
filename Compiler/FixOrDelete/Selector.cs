class Selector : Stmt
{
    public Expression Source { get; private set;}
    public Expression Single {get; private set;}
    //public Expression Predicate {get; private set;}
    public Selector(Expression source, Expression single, CodeLocation location) : base(location)
    {
        this.Source = source;
        this.Single = single;
    }
    public override Scope AssociatedScope {get;set;}
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        this.AssociatedScope = scope;

        bool ValidSource = Source.CheckSemantic(context, AssociatedScope, errors);
        if(Source.Type != ExpressionType.Text)
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "El Source del Selector debe ser una expresion de tipo texto"));
            return false;
        }
        Source.Evaluate();
        if(!context.ValidRange.Contains(Source.Value))
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "El Source declarado es invalido"));
            return false;
        }
        if(Single is null)
        {
            Single.Type  = ExpressionType.Boolean;
            Single.Value = false;
        }
        else
        {
            bool ValidSingle = Single.CheckSemantic(context, scope, errors);
            if(Single.Type != ExpressionType.Boolean)
            {
                errors.Add(new CompilingError(Location, ErrorCode.Invalid, "El Single del Selector debe ser una expresion booleana"));
                return false;
            }
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