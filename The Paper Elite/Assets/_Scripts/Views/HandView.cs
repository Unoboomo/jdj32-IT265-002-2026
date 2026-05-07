using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using DG.Tweening;
using System.Linq;

public class HandView : MonoBehaviour
{
    [SerializeField] private SplineContainer splineContainer;
    private readonly List<CardView> cards = new();

    public IEnumerator AddCard(CardView cardView) 
    {
        cards.Add(cardView);
        yield return UpdateCardPositions(0.2f);
    }
    public CardView RemoveCard(Card card)
    {
        CardView cardView = GetCardView(card);
        if (cardView == null )
        {
            return null;
        }
        cards.Remove(cardView);
        StartCoroutine(UpdateCardPositions(0.15f));
        return cardView;
    }
    private CardView GetCardView(Card card)
    {
        return cards.Where(cardView => cardView.Card == card).FirstOrDefault();
    }
    private IEnumerator UpdateCardPositions(float duration)
    {
        if (cards.Count == 0)
        {
            yield break;
        }
        float cardSpacing = 1f / Mathf.Max(cards.Count, 10);
        float firstCardPosition = 0.5f - (cards.Count - 1) * cardSpacing / 2;
        Spline spline = splineContainer.Spline;
        for (int i = 0; i < cards.Count; i++)
        {
            var card = cards[i];
            string layoutId = "HandLayout_" + card.GetEntityId();
            DOTween.Kill(layoutId);
            card.IsAnimating = false;
            float p = firstCardPosition + i * cardSpacing;
            Vector3 splinePosition = spline.EvaluatePosition(p);
            Vector3 forward = spline.EvaluateTangent(p);
            Vector3 up = spline.EvaluateUpVector(p);
            Quaternion rotation =  Quaternion.LookRotation(-up, Vector3.Cross(-up, forward).normalized);

            var move = card.transform.DOMove(splinePosition + transform.position + 0.01f * i * Vector3.back, duration);
            var rot = card.transform.DORotate(rotation.eulerAngles, duration);
            card.IsAnimating = true;
            DOTween.Sequence()
                .SetId(layoutId)
                .Join(move)
                .Join(rot)
                .OnComplete(() =>
                {
                    card.IsAnimating = false;
                });
        }
        yield return new WaitForSeconds(duration);
    }
    public void ClearHand()
    {
        foreach (var card in cards)
        {
            if (card != null && card.gameObject != null)
            {
                card.transform.DOKill();
                Destroy(card.gameObject);
            }
        }
        cards.Clear();
    }
}
