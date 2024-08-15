
class For : Stmt
{
    public Token IterationVar {get;}
    public Variable Collection {get;}
    public Stmt Body {get;}
    public For(Token exp1, Variable exp2, Stmt body, CodeLocation location) : base(location)
    {
        this.IterationVar = exp1;;
        this.Collection = exp2;
        this.Body = body;
    }
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