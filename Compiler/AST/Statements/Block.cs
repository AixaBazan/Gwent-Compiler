using System.Text;
public class Block : Stmt
{
    public List<Stmt> Statements { get; } // Lista de declaraciones en el bloque

    public Block(List<Stmt> statements, CodeLocation location): base(location)
    {
        Statements = statements; // Inicializa la lista de declaraciones
    }
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        foreach(Stmt item in Statements)
        {
            if(item.CheckSemantic(context, scope, errors) == false)
            {
                return false;
            }
        }
        return true;
    }
    public override void Interprete()
    {
        foreach(Stmt item in Statements)
        {
            item.Interprete();
        }
    }
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("{");

        foreach (Stmt item in Statements)
        {
            sb.AppendLine("  " + item.ToString());
        }

        sb.AppendLine("}");

        return sb.ToString();
    }
}