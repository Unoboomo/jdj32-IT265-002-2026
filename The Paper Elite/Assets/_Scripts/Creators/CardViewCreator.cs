using DG.Tweening;
using UnityEngine;

public class CardViewCreator : Singleton<CardViewCreator>
{
    [SerializeField] private CardView cardViewPrefab;

    public CardView CreateCardView(Card card, Vector3 position, Quaternion rotation)
    {
        CardView cardView = Instantiate(cardViewPrefab, position, rotation);
        Vector3 targetScale = cardView.transform.localScale;
        cardView.transform.localScale = Vector3.zero;
        cardView.transform.DOScale(targetScale, 0.15f);
        cardView.Setup(card);
        return cardView;
    }
}
