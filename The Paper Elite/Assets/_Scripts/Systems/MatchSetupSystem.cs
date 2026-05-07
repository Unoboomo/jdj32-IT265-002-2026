using System.Collections.Generic;
using UnityEngine;

public class MatchSetupSystem : Singleton<MatchSetupSystem>
{
    private const int DEFAULT_DRAW_AMOUNT = 5;

    [SerializeField] private List<EnemyData> enemyDatas;
    public void StartCombat(List<HeroData> selectedHeroes)
    {
        if (selectedHeroes == null || selectedHeroes.Count == 0)
        {
            return;
        }
        PlayerSystem.Instance.Setup(selectedHeroes);
        EnemySystem.Instance.Setup(enemyDatas);

        RefillManaGA refillManaGA = new();
        ActionSystem.Instance.Perform(refillManaGA, () =>
        {
            DrawCardsGA drawCardsGA = new(DEFAULT_DRAW_AMOUNT);
            ActionSystem.Instance.Perform(drawCardsGA);
        });
    }
    public void CleanupMatch()
    {
        CardSystem.Instance.Cleanup();
        EnemySystem.Instance.Cleanup();
    }
}
