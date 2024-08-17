using System.Text.RegularExpressions;

class Parser
{
    private TokenStream Stream;
    public Scope environment;
    public bool HadParseError {get; private set;}
    public Parser(TokenStream stream) 
    {
        this.Stream = stream;
        this.environment = new Scope();
        this.HadParseError = false;
    }
    //Parse
    public Card PARSE()
    {
        //Interpreter program = new Interpreter(new CodeLocation());
        Card card = null;
        while(!Stream.End)
        {
            CodeLocation loc = Stream.Peek().Location;
            if(Stream.Match(TokenValue.Card))
            {
                if(Stream.Match(TokenValue.OpenCurlyBracket))
                {
                    card = ParseCard(Stream.LookAhead(-2).Location);
                    //program.Cards.Add(card);
                }
                else
                {
                    throw new CompilingError(Stream.Previous().Location, ErrorCode.Expected, "Se esperaba {");
                }
            }
            else throw new CompilingError(loc, ErrorCode.Invalid, "Intento de declaracion invalida, se espera card o effect");
        }
        return card;
    }
    public Effect PARSEffect()
    {
        //Interpreter program = new Interpreter(new CodeLocation());
        Effect effect = null;
        while(!Stream.End)
        {
            CodeLocation loc = Stream.Peek().Location;
            if(Stream.Match(TokenValue.declareEffect))
            {
                if(Stream.Match(TokenValue.OpenCurlyBracket))
                {
                    effect = ParseEffect(Stream.LookAhead(-2).Location);
                    //program.Cards.Add(card);
                }
                else
                {
                    throw new CompilingError(Stream.Previous().Location, ErrorCode.Expected, "Se esperaba {");
                }
            }
            else throw new CompilingError(loc, ErrorCode.Invalid, "Intento de declaracion invalida, se espera card o effect");
        }
        return effect;
    }
    //Parseando la carta
    public Card ParseCard(CodeLocation location)
    {
        Expression Name = null;
        Expression Power = null;
        Expression Faction = null;
        Expression Type = null;
        List<Expression> Range = null;
        //List<string> cardEffects{get;private set;}
        do
        {
            if(Stream.Peek().Type == TokenType.End) throw new CompilingError(Stream.Previous().Location, ErrorCode.Expected, "Declaracion no terminada, se esperaba }");
            else if(Stream.Match(TokenValue.name)) Name = AssignProperty(Name is null, "Name");
            else if(Stream.Match(TokenValue.type)) Type = AssignProperty(Type is null, "Type");
            else if(Stream.Match(TokenValue.faction)) Faction = AssignProperty(Faction is null, "Faction");
            else if(Stream.Match(TokenValue.power)) Power = AssignProperty(Power is null, "Power");
            else if(Stream.Match(TokenValue.range)) Range = AssignRange(Range is null); // revisar esto
            else throw new CompilingError(Stream.Peek().Location, ErrorCode.Invalid, "Declaracion de carta invalida");
        }while(!Stream.Match(TokenValue.ClosedCurlyBracket));

        if(Name is null) throw new CompilingError(Stream.Peek().Location, ErrorCode.Expected, "No se declaro el nombre de la carta");
        if(Type is null) throw new CompilingError(Stream.Peek().Location, ErrorCode.Expected, "No se declaro el tipo de la carta");
        if(Faction is null) throw new CompilingError(Stream.Peek().Location, ErrorCode.Expected, "No se declaro la faccion de la carta");
        if(Power is null) throw new CompilingError(Stream.Peek().Location, ErrorCode.Expected, "No se declaro el poder de la carta");
        if(Range is null) throw new CompilingError(Stream.Peek().Location, ErrorCode.Expected, "No se declaro el range de la carta");
        return new Card(Name, Type, Faction, Power, Range, location);
    }
    private List<Expression> AssignRange(bool IsNotDefinided)
    {
        List<Expression> range = new List<Expression>();
        if(!IsNotDefinided)
        {
            throw new CompilingError(Stream.Peek().Location, ErrorCode.None, "Ya se definio la propiedad Range de la carta");
        }
        Stream.Consume(TokenValue.colon, "Se esperaba :");
        Stream.Consume(TokenValue.OpenSquareBracket, "Se esperaba [");
        do
        {
            Expression exp = expression();
            range.Add(exp);
            
            if(!Stream.Comma(TokenValue.ClosedSquareBracket))
            {
                throw new CompilingError(Stream.Peek().Location, ErrorCode.Expected, "Se esperaba ,");
            } 
        }while(!Stream.Match(TokenValue.ClosedSquareBracket));
        
        if(range.Count == 0 || range.Count > 3)
        {
            throw new CompilingError(Stream.Peek().Location, ErrorCode.Invalid, "Declaracion invalida de las posibles zonas de juego de la carta");
        }
        if(!Stream.Comma())
        {
            throw new CompilingError(Stream.Previous().Location, ErrorCode.Expected, "Se esperaba , ");
        }
        else return range;
    }
    private Expression AssignProperty(bool IsNotDefinided, string name)
    {
        if(!IsNotDefinided)
        {
            throw new CompilingError(Stream.Peek().Location, ErrorCode.None, "Ya se definio la propiedad " + name +" de la carta");
        }
        Expression exp;
        if(Stream.Match(TokenValue.colon))
        {
            exp = expression();
        }
        else throw new CompilingError(Stream.Peek().Location, ErrorCode.Expected, "Se esperaba : para definir la propiedad " + name + " de la carta");
        if(!Stream.Comma())
        {
            throw new CompilingError(Stream.Previous().Location, ErrorCode.Expected, "Se esperaba , ");
        }
        return exp;
    }
    //Parseando los efectos
    public Effect ParseEffect(CodeLocation loc)
    {
        Expression Name = null;
        Token targets = null;
        Token context = null;
        Stmt body = null;

        do
        {
            if(Stream.Peek().Type == TokenType.End) throw new CompilingError(Stream.Previous().Location, ErrorCode.Expected, "Declaracion no terminada, se esperaba }");
            else if(Stream.Match(TokenValue.name)) Name = AssignProperty(Name is null, "Name");
            else if(Stream.Match(TokenValue.action))
            {
                if(!(body is null)) throw new CompilingError(Stream.Previous().Location, ErrorCode.Invalid, "Ya declaro el campo Action del efecto");
                Stream.Consume(TokenValue.colon, "Se esperaba : ");
                Stream.Consume(TokenValue.OpenBracket, "Se esperaba (");
                if (Stream.Match(TokenType.Identifier))
                {
                    targets = Stream.Previous();
                }
                else throw new CompilingError(Stream.Previous().Location, ErrorCode.Expected, "Se esperaba un identificador");
                Stream.Consume(TokenValue.comma, "Se esperaba ,");
                if (Stream.Match(TokenType.Identifier))
                {
                    context = Stream.Previous();
                }
                else throw new CompilingError(Stream.Previous().Location, ErrorCode.Expected, "Se esperaba un identificador");
                Stream.Consume(TokenValue.ClosedBracket, "Se esperaba )");
                Stream.Consume(TokenValue.lambda, "Se esperaba =>");
                if(Stream.Match(TokenValue.OpenCurlyBracket)) body = ActionBody();
                else body = SingleStmt();
                if(!Stream.Comma())
            {
                throw new CompilingError(Stream.Previous().Location, ErrorCode.Expected, "Se esperaba , ");
            }
            }
            else 
            {
                throw new CompilingError(Stream.Peek().Location, ErrorCode.Invalid, "Declaracion de propiedad de efecto invalida");
            }

        }while(!Stream.Match(TokenValue.ClosedCurlyBracket));
        if (Name is null) throw new CompilingError(Stream.Peek().Location, ErrorCode.Expected, "No se declaro el nombre del efecto");
        if (body is null) throw new CompilingError(Stream.Peek().Location, ErrorCode.Expected, "No se declaro el Action del efecto");

        return new Effect(Name, targets, context, body,  loc);
    }
    //Sentencias
    public Stmt ActionBody()
    {
        List<Stmt> statements = new List<Stmt>();
        do
        {
            try
            {
                if (Stream.Peek().Type == TokenType.End)  throw new CompilingError(Stream.Peek().Location, ErrorCode.Expected, "Se esperaba }");
                //else if (Stream.Match(TokenValue.OpenCurlyBracket)) return new Block(Block());
                else if (Stream.Match(TokenValue.WhileCicle)) statements.Add(While());
                else if (Stream.Match(TokenValue.ForCicle)) statements.Add(For());
                else statements.Add(SingleStmt());
            }
            catch (CompilingError error)
            {
                System.Console.WriteLine(error);
                this.HadParseError = true;
            }
        } while (!Stream.Match(TokenValue.ClosedCurlyBracket));
        Stream.Match(TokenValue.semicolon); //no se pq
        return new Block(statements, Stream.Peek().Location);
    }
    private Stmt For()
    {
        CodeLocation loc = Stream.Previous().Location;
        Token token = null;
        if(Stream.Match(TokenType.Identifier))
        {
            token = Stream.Previous();
        }
        else throw new CompilingError(Stream.Previous().Location, ErrorCode.Expected, "Se espera un identificador despues de for");
        Stream.Consume(TokenValue.In, "Se esperaba la palabra reservada in");
        Expression collection = expression();
        Stmt body = null;
        if(Stream.Match(TokenValue.OpenBracket))
        {
            body = ActionBody();
        }
        else body = SingleStmt();

        return new For(token, collection, body, loc);
    }
    private Stmt While()
    {
        CodeLocation loc = Stream.Previous().Location;
        Stream.Consume(TokenValue.OpenBracket, "Expect '(' after 'while'.");
        Expression condition = expression();
        Stream.Consume(TokenValue.ClosedBracket, "Expect ')' after condition.");
        Stmt body = null;
        if(Stream.Match(TokenValue.OpenCurlyBracket))
        {
            body = ActionBody();
        }
        else body = SingleStmt();
        return new While(condition, body, loc);
    }
    //revisar
    private Stmt SingleStmt()
    {
        Stmt stmt = null;
        if (Stream.Match(TokenValue.Print))
        {
            try
            {
                stmt = new Print(expression(), Stream.Previous().Location);
            }
            catch(CompilingError error) 
            {
                Console.WriteLine(error);
            }
        }
        else if (Stream.Match(TokenType.Identifier))
        {
            if(Stream.Match(TokenValue.Assign))
            {
                stmt = VarDeclaration();
            }
            else 
            {
                HadParseError = true; 
                throw new CompilingError(Stream.Peek().Location, ErrorCode.Invalid, "Expression invalida despues del identificador " + Stream.Previous().Value);
            }
        }
        ///else throw new CompilingError(Stream.Peek().Location, ErrorCode.Invalid, "Statement vacio");
        if (!Stream.Match(TokenValue.semicolon))
        {
            throw new CompilingError(Stream.Previous().Location, ErrorCode.Expected, "Declaracion incompleta, se esperaba ;");
        }
        return stmt;
    }
    //revisar ;
    public Stmt VarDeclaration() 
    {
        string Id = Stream.LookAhead(-2).Value;
        CodeLocation loc = Stream.LookAhead(-2).Location;
        Expression initializer = expression();
        return new Var(Id ,initializer, loc);
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
        if (Stream.Match(TokenType.Identifier)) 
        {
            Token variable = Stream.Previous();
            Expression exp = new Variable(variable.Value, Stream.Previous().Location);
            while (Stream.Match(TokenValue.dot)) //chequear si se va a llamar a una propiedad o a una funcion
            {
                Token caller = null;
                if(Stream.Match(TokenType.KeyWord)) caller = Stream.Previous();
                else throw new CompilingError(Stream.Previous().Location, ErrorCode.Expected, "Se esperaba una propiedad o metodo");
                if (Stream.Match(TokenValue.OpenBracket))
                {
                    if (!Stream.Match(TokenValue.ClosedBracket))
                    {
                        string param = null;
                        if(Stream.Match(TokenType.Identifier))
                        {
                            param = Stream.Previous().Value;
                        }
                        else
                        {
                            throw new CompilingError(Stream.Previous().Location, ErrorCode.Expected, "Se esperaba el parametro del metodo");
                        }
                        Stream.Consume(TokenValue.ClosedBracket, "Se esperaba )");
                        exp = new MethodWithParams(exp, caller.Value, param, Stream.Previous().Location); //chequear loc
                    }
                    else exp = new Method(exp, caller.Value, Stream.Previous().Location); //chequear loc
                }
                //else expr = new Property(caller, expr);
                else exp = new Property(exp, caller.Value, Stream.Previous().Location); //Chequear loc
            }
            return exp;
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
