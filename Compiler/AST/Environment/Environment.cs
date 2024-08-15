public class Scope //Para variables
{
    private Dictionary<string, object> values = new Dictionary<string, object>();
    Scope? Parent;
    public Scope()
    {
        Parent = null; // Sin entorno padre
    }

    // Constructor que acepta un entorno padre
    public Scope(Scope parent)
    {
        this.Parent = parent; // Inicializa el entorno padre
    }
    public object? Get(string name) 
    {
        if (values.ContainsKey(name))
        { 
            return values[name];
        }
        else if(Parent != null)
        {
            return Parent.Get(name);
        }
        else return null;
        //else throw new CompilingError(name.Location, ErrorCode.VariableUndefined, "Variable no definida:" + name);
    }
    public object? Get(Token name) 
    {
        if (values.ContainsKey(name.Value))
        { 
            return values[name.Value];
        }
        else if(Parent != null)
        {
            return Parent.Get(name);
        }
        else return null;
        //else throw new CompilingError(name.Location, ErrorCode.VariableUndefined, "Variable no definida:" + name);
    }
    public void Define(string name, object value)
    {
        if (values.ContainsKey(name))
        {
            values[name] = value; // Actualiza el valor
            return; // Salir del método si la asignación fue exitosa
        }
        // Si no se encuentra en el entorno actual, intenta en el entorno padre
        if (Parent != null)
        {
            Parent.Define(name, value); // Llama al método assign en el entorno padre
            return; // Salir del método si la asignación fue exitosa
        }
        values.Add(name, value);
    }
}