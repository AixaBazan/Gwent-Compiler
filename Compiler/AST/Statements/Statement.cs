public abstract class Stmt : AST
{
    public Stmt(CodeLocation location) : base(location){ }
    public abstract void Interprete();
}