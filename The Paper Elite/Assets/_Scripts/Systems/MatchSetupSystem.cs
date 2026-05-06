using System.Collections.Generic;
using UnityEngine;

public class MatchSetupSystem : MonoBehaviour
{
    private int defaultDrawAmount = 5;

    [SerializeField] private List<CardData> deckData;

    private void Start()
    {
        CardSystem.Instance.Setup(deckData);
        RefillManaGA refillManaGA = new();
        ActionSystem.Instance.Perform(refillManaGA, () =>
        {
            DrawCardsGA drawCardsGA = new(defaultDrawAmount);
            ActionSystem.Instance.Perform(drawCardsGA);
        });
    }
}
