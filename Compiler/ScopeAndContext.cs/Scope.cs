public class Scope //Para variables
{
    private Dictionary<string, object> values = new Dictionary<string, object>(); //diccionario nombre de la variable - valor
    private Dictionary<string, ExpressionType> types = new Dictionary<string, ExpressionType>(); // diccionario nombre de la variable - tipo
    public Scope? Parent {get; private set;}
    public Scope()
    {
        Parent = null; 
    }
    public Scope CreateChild()
    {
        Scope child = new Scope();
        child.Parent = this;   
        return child;
    }
    public object? Get(Token name) => Get(name);
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
        else 
        {
            System.Console.WriteLine("No se encontro la variable");
            return null;
        }
    }
    public ExpressionType GetType(string name)
    {
        if (types.ContainsKey(name))
        {
            return types[name];
        }
        else if (Parent != null)
        {
            return Parent.GetType(name);
        }
        else
        {
            //retorno ErrorType si la variable no se encuentra en el scope
            return ExpressionType.ErrorType; 
        }
    }
    public void Define(string name, object value)
    {
        if (values.ContainsKey(name))
        {
            values[name] = value;
            return;
        }
        else if (Parent != null)
        {
            if(AssignValue(name, value))
            {
                return;
            }
            else values.Add(name,value);
        }
        else values.Add(name,value);
    }
    private bool AssignValue(string name, object value)
    {
        if(values.ContainsKey(name))
        {
            values[name] = value;
            return true;
        }
        if (Parent != null)
        {
            return Parent.AssignValue(name, value); // Llama al método assign en el entorno padre
        }
        return false;
    }
    
    public void DefineType(string name, ExpressionType type)
    {
        if(types.ContainsKey(name))
        {
            types[name] = type;
        }
        else if(Parent != null)
        {
            if(AssignType(name, type))
            {
                return;
            }
            else types.Add(name, type);
        }
        else types.Add(name,type);
    }
    private bool AssignType(string name, ExpressionType type)
    {
        if(types.ContainsKey(name))
        {
            types[name] = type;
            return true;
        }
        if (Parent != null)
        {
            return Parent.AssignType(name, type); // Llama al método assign en el entorno padre
        }
        return false;
    }
    
}