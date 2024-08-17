class For : Stmt
{
    public Token IterationVar {get;}
    public Expression Collection {get;}
    public Stmt Body {get;}
    public override Scope AssociatedScope{get; set;}
    public For(Token token, Expression collection, Stmt body, CodeLocation location) : base(location)
    {
        this.IterationVar = token;
        this.Collection = collection;
        this.Body = body;
    }
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        this.AssociatedScope = scope.CreateChild();
        bool body = Body.CheckSemantic(context, AssociatedScope, errors);
        bool collection = Collection.CheckSemantic(context, scope, errors);
        return body;
    }
    public override void Interprete()
    {
        
    }
    public override string ToString()
    {
        return base.ToString();
    }
}