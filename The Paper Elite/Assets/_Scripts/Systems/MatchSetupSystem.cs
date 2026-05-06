using System.Collections.Generic;
using UnityEngine;

public class MatchSetupSystem : MonoBehaviour
{
    private int defaultDrawAmount = 5;

    [SerializeField] private List<HeroData> heroDatas;
    [SerializeField] private List<EnemyData> enemyDatas;
    private void Start()
    {
        PlayerSystem.Instance.Setup(heroDatas);

        EnemySystem.Instance.Setup(enemyDatas);

        RefillManaGA refillManaGA = new();
        ActionSystem.Instance.Perform(refillManaGA, () =>
        {
            DrawCardsGA drawCardsGA = new(defaultDrawAmount);
            ActionSystem.Instance.Perform(drawCardsGA);
        });
    }
}
