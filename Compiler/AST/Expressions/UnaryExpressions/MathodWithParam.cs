class MethodWithParams : UnaryExpression
{
    public MethodWithParams(Expression exp, string method, string param, CodeLocation location): base(location)
    {
        this.expression = exp;
        this.method = method;
        this.param = param;
    }
    public Expression expression{ get; set;}
    public string method { get; set;}
    public string param {get; set;}
    public override object? Value {get;set;}
    public override ExpressionType Type {get; set;}
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        bool ValidExpression = expression.CheckSemantic(context, scope, errors);
        if(scope.GetType(param) == ExpressionType.ErrorType)
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "La varible a la que desea acceder no se ha definido"));
            Type = ExpressionType.ErrorType;
            return false;
        }
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
       this.Value = "Es un contexto o una lista";
    }
    public override string ToString()
    {
        return String.Format(expression + "." + method + "(" + param + ")");
    }
}