class Program
{
    public static void Main(string[] args)
    { 
        // Ruta del archivo de texto
        string filePath = @"D:\Aixa Study\2do Proyecto\Prueba.txt";
        // Leer el contenido del archivo y asignarlo a una variable string
        string text = File.ReadAllText(filePath);
        //Console.WriteLine(text);
        LexicalAnalyzer lex = Compiling.Lexical;
        List<CompilingError> errores = new List<CompilingError>();
        IEnumerable<Token> tokens = lex.GetTokens("Prueba", text, errores);
        foreach (var error in errores)
        {
            Console.WriteLine(error);
        }
        System.Console.WriteLine();
        foreach(Token token in tokens)
        {
            System.Console.WriteLine(token);
        }



        System.Console.WriteLine("Se supone que aqui se parsea");
        TokenStream read = new TokenStream(tokens);
        Parser parser = new Parser(read);
        List<CompilingError> Error = new List<CompilingError>();
        GwentProgram result = parser.ParseProgram(Error);
        if (result == null)
        {
            Console.WriteLine("No se pudo parsear la expresión.");
            return;
        }
        if(parser.errors.Count == 0)
        {
        System.Console.WriteLine("Resultado del parseo:");
        System.Console.WriteLine(result);

        
        List<CompilingError> errors = new List<CompilingError>();
        
            Context context = new Context();
            Scope scope = new Scope();
            

            result.CheckSemantic(context, scope, errors);

            if (errors.Count > 0)
            {
                foreach (CompilingError error in errors)
                {
                    Console.WriteLine("{0}, {1}, {2}", error.Location.Line, error.Code, error.Argument);
                }
            }
            else
            {
                System.Console.WriteLine("Todo bien semanticamente");
                System.Console.WriteLine(context.effects.Count);
                System.Console.WriteLine(context.cards.Count);
                //result.Interprete();

                 //result.Interprete();

                 //Console.WriteLine("Valor: " + result);
            }
        }
        else
        {
            foreach(var item in parser.errors)
            {
                System.Console.WriteLine(item);
            }
            System.Console.WriteLine("Hubo errores en el parse, no se pudo continuar la evaluacion");
        }


        // // //Expresiones:
        // // System.Console.WriteLine("Se supone que aqui se parsea");
        // // TokenStream read = new TokenStream(tokens);
        // // Parser parser = new Parser(read);
        // // List<CompilingError> Error = new List<CompilingError>();
        // // Expression result = parser.expression();
        // // if (result == null)
        // // {
        // //     Console.WriteLine("No se pudo parsear la expresión.");
        // //     return;
        // // }
        // // if(parser.errors.Count == 0)
        // // {
        // // System.Console.WriteLine("Resultado del parseo:");
        // // System.Console.WriteLine(result);
             
        // // List<CompilingError> errors = new List<CompilingError>();
        
        // //     Context context = new Context();
        // //     Scope scope = new Scope();
            

        // //     result.CheckSemantic(context, scope, errors);

        // //     if (errors.Count > 0)
        // //     {
        // //         foreach (CompilingError error in errors)
        // //         {
        // //             Console.WriteLine("{0}, {1}, {2}", error.Location.Line, error.Code, error.Argument);
        // //         }
        // //     }
        // //     else
        // //     {
        // //         //System.Console.WriteLine("Carta bien semanticamente");
        // //         result.Evaluate();

        // //          //result.Interprete();

        // //          Console.WriteLine("Valor: " + result);
        // //     }
        // // }
        // // else
        // // {
        // //     foreach(var item in parser.errors)
        // //     {
        // //         System.Console.WriteLine(item);
        // //     }
        // //     System.Console.WriteLine("Hubo errores en el parse, no se pudo continuar la evaluacion");
        // // }
    }   
    
}

