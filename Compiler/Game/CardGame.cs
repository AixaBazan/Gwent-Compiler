public abstract class Card
{
    public abstract string Name { get; set; }
    public abstract double Power { get; set; }
    public abstract string Faction {get; set;}
    public abstract string Type { get; set;}
    public int Owner 
    {
        get
        {
            if(this.Faction == "Fairies") return 1;
            else return 2;
        }
        set{}
    }
    public abstract List<string> GameZone { get; set; } 
    //Falta el efecto
}
public class UnitCard : Card
{
    public override string Name { get; set; }
    public override double Power { get; set; }
    public override string Faction { get; set; }
    public override string Type { get; set;}
    public override List<string> GameZone { get; set; } 
    public UnitCard(string name, double power, string faction, string type, List<string> gameZone)
    {
        this.Name = name;
        this.Power = power;
        this.Faction = faction;
        this.Type = type;
        this.GameZone = gameZone;
    }
}
public class SpecialCard : Card
{
    public override string Name { get; set; }
    public override double Power { get {return 0;} set{} }
    public override string Faction { get; set; }
    public override string Type { get; set;}
    public override List<string> GameZone { get; set; } 

    public SpecialCard(string name, string faction, string type)
    {
        this.Name = name;
        this.Faction = faction;
        this.Type = type;
        this.GameZone = new List<string>();
        UpdateGameZone();
    }
    private void UpdateGameZone()
    {
        if(Type == "Clima")
        {
            GameZone.Add("ZonaClima");
        }
        //verificar las aumento
    }
}
public class LeaderCard : Card
{
    public override string Name { get; set; }
    public override double Power { get {return 0;} set{} }
    public override string Faction { get; set; }
    public override string Type { get {return "Leader";} set{}}
    public override List<string> GameZone { get; set; } //revisar
    public LeaderCard(string name, string faction, string type)
    {
        this.Name = name;
        this.Faction = faction;
        this.Type = type;
        this.GameZone = new List<string>();
    }
}