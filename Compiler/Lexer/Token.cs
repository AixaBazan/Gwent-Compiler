public class Token
{
    public string Value { get; private set; }
    public TokenType Type { get; private set; }
    public CodeLocation Location { get; private set; }
    public Token(TokenType type, string value, CodeLocation location)
    {
        this.Type = type;
        this.Value = value;
        this.Location = location;
    }

    public override string ToString()
    {
        return string.Format("{0} [{1}] ,line: {2}" , Type, Value, Location.Line);
    }
}
public struct CodeLocation
{
    public int Line;
    public int Column;
}
public enum TokenType
{
    KeyWord,
    Identifier,
    Operator,
    Number,
    Text,
    Unknwon,
    End
}
public class TokenValue
{
    protected TokenValue(){}
    //Operators:
    //Aritmetic Operators
    public const string Add = "Addition"; // +
    public const string Sub = "Subtract"; // -
    public const string Mul = "Multiplication"; // *
    public const string Div = "Division"; // /
    public const string addOne = "AddOne"; // ++
    public const string pow = "Pow"; // ^
    //Assignment Operators
    public const string Assign = "Assign"; // =
    //OtherOperators
    public const string comma = "ValueSeparator"; // ,
    public const string semicolon = "StatementSeparator"; // ;
    public const string colon = "Colon"; // :
    public const string point = "Point"; //.
    public const string OpenBracket = "OpenBracket"; // (
    public const string ClosedBracket = "ClosedBracket"; // )
    public const string OpenCurlyBracket = "OpenCurlyBracket"; // {
    public const string ClosedCurlyBracket = "ClosedCurlyBracket"; // }
    public const string OpenSquareBracket = "OpenSquareBracket"; // [
    public const string ClosedSquareBracket = "ClosedSquareBracket"; // ]
    public const string lambda = "LambdaOperator"; // =>
    //String Concatenation
    public const string ConcantenationWithoutSpace = "ConcatenationWithoutSpace"; // @
    public const string ConcatenationWithEspace = "ConcatenationWithEspace "; // @@
    //Logic Operators
    public const string and = "And"; // &&
    public const string or = "Or"; // ||
    //Comparison Operators
    public const string minor = "Minor"; // <
    public const string elderly = "Elderly"; // >
    public const string equal = "Equal"; // ==
    public const string lessOrEqual = "LessOrEqual"; // <=
    public const string greaterOrEqual = "greaterOrEqual"; // >=
    
    //KeyWords:
    //Palabras claves para definir una carta
    public const string Card = "card"; //card
    public const string type = "Type"; //Type
    public const string name = "Name"; //Name
    public const string faction = "Faction"; //Faction
    public const string power = "Power"; //Power
    public const string range = "Range"; //Range
    public const string onActivation = "OnActivation"; //OnActivation
    public const string assignEffect = "AssignEffect"; //Effect
    public const string amount = "Amount"; //Amount
    public const string selector = "Selector"; //Selector
    public const string Source = "Source"; //Source
    public const string single = "Single"; //Single
    public const string predicate = "Predicate"; //Predicate
    public const string postAction = "PostAction"; //PostAction
    // Palabras claves para definir un efecto
    public const string declareEffect = "DeclareEffect"; //effect
    public const string Params = "Params"; 
    public const string action = "Action"; 
    //Otras
    public const string ForCicle = "for"; 
    public const string In = "in";
    public const string WhileCicle = "while"; 
    public const string False = "false";
    public const string True = "true";
    public const string Print = "print"; 
}
class Compiling
{
    private static LexicalAnalyzer? LexicalProcess;
    public static LexicalAnalyzer Lexical
    {
        get
        {
            if (LexicalProcess == null)
            {
                LexicalProcess = new LexicalAnalyzer();

                //Operator
                LexicalProcess.RegisterOperator("+", TokenValue.Add);
                LexicalProcess.RegisterOperator("-", TokenValue.Sub);
                LexicalProcess.RegisterOperator("*", TokenValue.Mul);
                LexicalProcess.RegisterOperator("/", TokenValue.Div);
                LexicalProcess.RegisterOperator("++", TokenValue.addOne);
                LexicalProcess.RegisterOperator("^", TokenValue.pow);
                LexicalProcess.RegisterOperator("&&", TokenValue.and);
                LexicalProcess.RegisterOperator("||", TokenValue.or);
                LexicalProcess.RegisterOperator("<", TokenValue.minor);
                LexicalProcess.RegisterOperator(">", TokenValue.elderly);
                LexicalProcess.RegisterOperator("==", TokenValue.equal);
                LexicalProcess.RegisterOperator("<=", TokenValue.lessOrEqual);
                LexicalProcess.RegisterOperator(">=", TokenValue.greaterOrEqual);
                LexicalProcess.RegisterOperator("=", TokenValue.Assign);
                LexicalProcess.RegisterOperator("@", TokenValue.ConcantenationWithoutSpace);
                LexicalProcess.RegisterOperator("@@", TokenValue.ConcatenationWithEspace);
                LexicalProcess.RegisterOperator("(", TokenValue.OpenBracket);
                LexicalProcess.RegisterOperator(")", TokenValue.ClosedBracket);
                LexicalProcess.RegisterOperator("{", TokenValue.OpenCurlyBracket);
                LexicalProcess.RegisterOperator("}", TokenValue.ClosedCurlyBracket);
                LexicalProcess.RegisterOperator("[", TokenValue.OpenSquareBracket);
                LexicalProcess.RegisterOperator("]", TokenValue.ClosedSquareBracket);
                LexicalProcess.RegisterOperator(",", TokenValue.comma);
                LexicalProcess.RegisterOperator(";", TokenValue.semicolon);
                LexicalProcess.RegisterOperator(":", TokenValue.colon);
                LexicalProcess.RegisterOperator(".", TokenValue.point);
                LexicalProcess.RegisterOperator("=>", TokenValue.lambda);

                //Text
                LexicalProcess.RegisterText("\"", "\"");
                //KeyWords
                LexicalProcess.RegisterKeyword("card", TokenValue.Card);
                LexicalProcess.RegisterKeyword("Type", TokenValue.type);
                LexicalProcess.RegisterKeyword("Name", TokenValue.name);
                LexicalProcess.RegisterKeyword("Faction", TokenValue.faction);
                LexicalProcess.RegisterKeyword("Power", TokenValue.power);
                LexicalProcess.RegisterKeyword("Range", TokenValue.range);
                LexicalProcess.RegisterKeyword("OnActivation", TokenValue.onActivation);
                LexicalProcess.RegisterKeyword("Effect", TokenValue.assignEffect);
                LexicalProcess.RegisterKeyword("Amount", TokenValue.amount);
                LexicalProcess.RegisterKeyword("Selector", TokenValue.selector);
                LexicalProcess.RegisterKeyword("Source", TokenValue.Source);
                LexicalProcess.RegisterKeyword("Single", TokenValue.single);
                LexicalProcess.RegisterKeyword("Predicate", TokenValue.predicate);
                LexicalProcess.RegisterKeyword("PostAction", TokenValue.postAction);
                LexicalProcess.RegisterKeyword("effect", TokenValue.declareEffect);
                LexicalProcess.RegisterKeyword("Params", TokenValue.Params);
                LexicalProcess.RegisterKeyword("Action", TokenValue.action);
                LexicalProcess.RegisterKeyword("for", TokenValue.ForCicle);
                LexicalProcess.RegisterKeyword("in", TokenValue.In);
                LexicalProcess.RegisterKeyword("while", TokenValue.WhileCicle);
                LexicalProcess.RegisterKeyword("false", TokenValue.False);
                LexicalProcess.RegisterKeyword("true", TokenValue.True);
                LexicalProcess.RegisterKeyword("print", TokenValue.Print);
            }
         return LexicalProcess;
        }
    }
}