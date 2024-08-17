class Method : UnaryExpression
{
    public Method(Expression exp, string method, CodeLocation location): base(location)
    {
        this.expression = exp;
        this.method = method;
    }
    public Expression expression{ get; set; }
    public string method { get; set; }
    public override object? Value {get;set;}
    public override ExpressionType Type {get; set;}
    public override bool CheckSemantic(Context context, Scope table, List<CompilingError> errors)
    {
        bool ValidExpression = expression.CheckSemantic(context, table, errors);
        if(expression.Type == ExpressionType.List)
        {
            if(context.ListMethodsWithoutParams.ContainsKey(method))
            {
                Type = context.ListMethodsWithoutParams[method];
                return true;
            }
            else
            {
                errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Las listas no contienen el metodo " + method));
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
       this.Value = "Es una carta o una lista";
    }
    public override string ToString()
    {
        return String.Format(expression + "." + method + "()");
    }
}