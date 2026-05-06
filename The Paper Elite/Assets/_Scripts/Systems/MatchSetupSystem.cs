using System.Collections.Generic;
using UnityEngine;

public class MatchSetupSystem : MonoBehaviour
{
    private int defaultDrawAmount = 5;

    [SerializeField] private HeroData heroData;
    [SerializeField] private List<EnemyData> enemyDatas;
    private void Start()
    {
        HeroSystem.Instance.Setup(heroData);
        EnemySystem.Instance.Setup(enemyDatas);
        CardSystem.Instance.Setup(heroData.Deck);

        RefillManaGA refillManaGA = new();
        ActionSystem.Instance.Perform(refillManaGA, () =>
        {
            DrawCardsGA drawCardsGA = new(defaultDrawAmount);
            ActionSystem.Instance.Perform(drawCardsGA);
        });
    }
}
