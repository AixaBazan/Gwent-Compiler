class Parser
{
    private TokenStream Stream;
    public Parser(TokenStream stream) 
    {
        this.Stream = stream;
    }
    //Parseando expresiones
    public Expression parse() 
    {
        try 
        {
            return expression();
        } catch (CompilingError e) 
        {
            Console.WriteLine(e);
            return null;
        }
    }
    private Expression expression()
    {
        return Equality();
    }
    private Expression Equality()
    {
        Expression exp = Comparation();
        while(!Stream.End && Stream.Match(TokenValue.equal)) 
        {
            Token operador = Stream.Previous();
            Expression right = Comparation();
            exp = new Equal(Stream.Previous().Location, exp, right);
        }
        return exp;
    }
    private Expression Comparation() 
    {
        Expression expr = Term();
        while (!Stream.End && Stream.Match(TokenValue.elderly, TokenValue.minor, TokenValue.lessOrEqual, TokenValue.greaterOrEqual)) 
        {
            Token operatorToken = Stream.Previous();
            Expression right = Term();
            if(operatorToken.Value == TokenValue.elderly)
                {
                    expr = new Elderly(Stream.Previous().Location, expr, right);
                }
            else if(operatorToken.Value == TokenValue.minor)
                {
                    expr = new Minor(Stream.Previous().Location, expr, right);
                }
            else if(operatorToken.Value == TokenValue.lessOrEqual)
                {
                    expr = new LessOrEqual(Stream.Previous().Location, expr, right);
                }
            else if(operatorToken.Value == TokenValue.greaterOrEqual)
                {
                    expr = new GreaterOrEqual(Stream.Previous().Location, expr, right);
                }
        }
        return expr;
    }
    private Expression Term()
    {
        Expression expr = Factor();
        while (!Stream.End && Stream.Match(TokenValue.Add, TokenValue.Sub, TokenValue.ConcantenationWithoutSpace, TokenValue.ConcatenationWithEspace)) 
        {
            Token operatorToken = Stream.Previous();
            Expression right = Factor();
            if(operatorToken.Value == TokenValue.Add)
            {
                expr = new Add(Stream.Previous().Location, expr, right);
            }
            else if(operatorToken.Value == TokenValue.ConcantenationWithoutSpace)
            {
                expr = new ConcatWithoutSpace(Stream.Previous().Location, expr, right);
            }
            else if(operatorToken.Value == TokenValue.ConcatenationWithEspace)
            {
                expr = new Concat(Stream.Previous().Location, expr, right);
            }
            else if(operatorToken.Value == TokenValue.Sub)
            {
                expr = new Sub(Stream.Previous().Location, expr, right);
            }
        }
        return expr;
    }
    private Expression Factor() 
    {
        Expression expr = Unary();
        while (!Stream.End && Stream.Match(TokenValue.Mul, TokenValue.Div)) 
        {
            Token operatorToken = Stream.Previous();
            Expression right = Unary();
            if(operatorToken.Value == TokenValue.Mul)
                {
                    expr = new Mult(Stream.Previous().Location, expr, right);
                }
            else if(operatorToken.Value == TokenValue.Div)
                {
                    expr = new Div(Stream.Previous().Location, expr, right);
                }
        }
        return expr;
    }
    private Expression Unary() 
    {
       if (!Stream.End && Stream.Match(TokenValue.Sub))
        {
            Expression right = Unary();
            return new Number(-double.Parse(Stream.Previous().Value), Stream.Previous().Location);
        }
        return Primary();
    }
    private Expression Primary()
    {
        if (Stream.Match(TokenValue.False)) 
            return new Bool(false, Stream.Previous().Location);
        
        if (Stream.Match(TokenValue.True)) 
            return new Bool(true, Stream.Previous().Location);
        
        if (Stream.Match(TokenValue.OpenBracket)) 
        {
            Expression expr = expression();
            Stream.Consume(TokenValue.ClosedBracket, "Se esperaba ')' después de la expresión.");
            return new Grouping(expr, Stream.Previous().Location);
        }
        
        if (Stream.Match(TokenType.Number)) 
        {
            // Asegúrate de que el valor sea una cadena de número válida
            string numberStr = Stream.Previous().Value;
            if (double.TryParse(numberStr, out double number))
            {
                return new Number(number, Stream.Previous().Location);
            }
            else
            {
                throw new CompilingError(Stream.Previous().Location, ErrorCode.Expected, "Formato de número inválido.");
            }
        }
        
        if (Stream.Match(TokenType.Text)) 
            return new Text(Stream.Previous().Value, Stream.Previous().Location);
        
        throw new CompilingError(Stream.Peek().Location, ErrorCode.Expected, "Se esperaba una expresion");
    }
}
