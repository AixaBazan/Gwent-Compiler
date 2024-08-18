class MethodWithParams : UnaryExpression
{
    public MethodWithParams(Expression exp, string method, Expression param, CodeLocation location): base(location)
    {
        this.expression = exp;
        this.method = method;
        this.param = param;
    }
    public Expression expression{ get; set;}
    public string method { get; set;}
    public Expression param {get; set;}
    public override object? Value {get;set;}
    public override ExpressionType Type {get; set;}
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        bool ValidExpression = expression.CheckSemantic(context, scope, errors);
        //Ver si el parametro que m pasaron es correcto
       

       
        //revisar q este bien semanticamente y asignar el tipo d retorno
        if(expression.Type == ExpressionType.List)
        {
            if(context.ListMethodsWithParams.ContainsKey(method))
            {
                Type = context.ListMethodsWithParams[method];
                return true;
            }
            else
            {
                errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Las listas no presentan el metodo " + method));
                Type = ExpressionType.ErrorType;
                return false;
            }
        }
        else if(expression.Type == ExpressionType.Context)
        {
            if(context.ContextMethods.ContainsKey(method))
            {
                Type = context.ContextMethods[method];
                return true;
            }
            else
            {
                errors.Add(new CompilingError(Location, ErrorCode.Invalid, "El contexto no contiene el metodo " + method));
                Type = ExpressionType.ErrorType;
                return false;
            }
        }
        else
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "La expresion que declaro no presenta metodos para acceder"));
            Type = ExpressionType.ErrorType;
            return false;
        }
    }
    public override void Evaluate()
    {
       this.Value = Type;
    }
    public override string ToString()
    {
        return String.Format(expression + "." + method + "(" + param + ")");
    }
}