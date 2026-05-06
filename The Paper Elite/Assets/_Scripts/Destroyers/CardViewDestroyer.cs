using DG.Tweening;
using System.Collections;
using UnityEngine;

public class CardViewDestroyer : Singleton<CardViewDestroyer>
{
    public IEnumerator DestroyCardView(CardView cardView, Vector3 position)
    {
        cardView.transform.DOScale(Vector3.zero, 0.2f);
        Tween tween = cardView.transform.DOMove(position, 0.2f);
        yield return tween.WaitForCompletion();
        Destroy(cardView.gameObject);
    }
}