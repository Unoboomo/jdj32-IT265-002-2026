using System.Collections.Generic;
using UnityEngine;

public class Player
{

    public const int DEFAULT_MANA = 3;
    public const int DEFAULT_DRAW_AMOUNT = 5;
    private readonly HeroData HeroData;
    public Sprite Image => HeroData.Image;

    public int MaxHealth;
    public int CurrentHealth;
    public int CurrentMana;
    public bool Alive;

    public List<Card> drawPile = new();
    public List<Card> discardPile = new();
    public List<Card> hand = new();
    public Player(HeroData heroData)
    {
        HeroData = heroData;
        CurrentHealth = MaxHealth = heroData.Health;
        CurrentMana = DEFAULT_MANA;
        foreach (var cardData in heroData.Deck)
        {
            Card card = new(cardData);
            drawPile.Add(card);
        }
        Alive = heroData.Health > 0 ? true : false;
    }
}
