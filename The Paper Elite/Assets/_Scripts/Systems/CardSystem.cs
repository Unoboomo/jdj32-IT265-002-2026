using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class CardSystem : Singleton<CardSystem>
{
    [SerializeField] private HandView handView;
    [SerializeField] private Transform drawPilePoint;
    [SerializeField] private Transform discardPilePoint;

    //in addition to having their own deck, heros also have their own draw pile, discard pile, and hand, and heroes
    //are switched out after every enemy turn, moving down a list
    private List<Card> DrawPile;
    private List<Card> DiscardPile;
    private List<Card> Hand;

    void OnEnable()
    {
        ActionSystem.AttachPerformer<DrawCardsGA>(DrawCardsPerformer);
        ActionSystem.AttachPerformer<DiscardAllCardsGA>(DiscardAllCardsPerformer);
        ActionSystem.AttachPerformer<PlayCardGA>(PlayCardPerformer);
    }

    void OnDisable()
    {
        ActionSystem.DetachPerformer<DrawCardsGA>();
        ActionSystem.DetachPerformer<DiscardAllCardsGA>();
        ActionSystem.DetachPerformer<PlayCardGA>();
    }

    //Publics
    public void Setup(List<Card> drawPile, List<Card> discardPile, List<Card> hand)
    {
        DrawPile = drawPile;
        DiscardPile = discardPile;
        Hand = hand;
    }

    // Performers
    private IEnumerator DrawCardsPerformer(DrawCardsGA drawCardsGA)
    {
        int actualAmount = Mathf.Min(drawCardsGA.Amount, DrawPile.Count);
        int notDrawnAmount = drawCardsGA.Amount - actualAmount;
        for (int i = 0; i < actualAmount; i++)
        {
            yield return DrawCard();
        }
        if (notDrawnAmount > 0)
        {
            RefillDeck();
            notDrawnAmount = Mathf.Min(notDrawnAmount, DrawPile.Count);
            for (int i = 0; i < notDrawnAmount; i++)
            {
                yield return DrawCard();
            }
        }
    }

    private IEnumerator DiscardAllCardsPerformer(DiscardAllCardsGA discardAllCardsGA)
    {
        foreach(var card in Hand.ToList())
        {
            yield return DiscardCard(card);
        }
        Hand.Clear();
    }

    private IEnumerator PlayCardPerformer(PlayCardGA playCardGA)
    {
        yield return DiscardCard(playCardGA.Card);
        SpendManaGA spendManaGA = new(playCardGA.Card.Mana);
        ActionSystem.Instance.AddReaction(spendManaGA);
        foreach (var effect in playCardGA.Card.Effects)
        {
            PerformEffectGA performEffectGA = new(effect);
            ActionSystem.Instance.AddReaction(performEffectGA);
        }
    }

    // Helpers
    private IEnumerator DrawCard()
    {
        Card card = DrawPile.Draw();
        if (card == null)
        {
            Debug.Log("did not draw card from draw pile");
            yield break;
        }
        Hand.Add(card);
        CardView cardView = CardViewCreator.Instance.CreateCardView(card, drawPilePoint.position, drawPilePoint.rotation);
        yield return handView.AddCard(cardView);
    }

    //discards adds a card to discard pile, removes it from hand view, and destroyes the card view
    private IEnumerator DiscardCard(Card card)
    {
        if (!Hand.Remove(card))
        {
            Debug.Log("could not remove card from hand");
            yield break;
        }
        DiscardPile.Add(card);
        CardView cardView = handView.RemoveCard(card);
        
        yield return CardViewDestroyer.Instance.DestroyCardView(cardView, discardPilePoint.position);
    }
    
    // Shuffles the discard pile back into the draw pile
    private void RefillDeck()
    {
        DrawPile.AddRange(DiscardPile);
        DiscardPile.Clear();
    }
}
