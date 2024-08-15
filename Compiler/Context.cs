//Our context stores the declared effects and cards.

  /*This project doesn't collect and check the cards existence.
  That could be a fine exercise for you */
public class Context
{
    public List<string> effects;
    public List<string> cards;

    public Context()
    {
        effects = new List<string>();
        cards = new List<string>();
    }
}