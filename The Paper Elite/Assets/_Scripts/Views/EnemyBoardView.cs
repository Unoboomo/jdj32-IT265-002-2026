using System.Collections.Generic;
using UnityEngine;

public class EnemyBoardView : MonoBehaviour
{
    [SerializeField] private List<Transform> slots;
    public List<EnemyView> EnemyViews { get; private set; } = new();
    public void AddEnemy(EnemyData enemyData)
    {
        if (EnemyViews.Count >= slots.Count)
        {
            Debug.LogWarning("Tried to add an enemy, but there are no empty slots left on the board!");
            return;
        }
        Transform slot = slots[EnemyViews.Count];
        EnemyView enemyView = EnemyViewCreator.Instance.CreateEnemyView(enemyData, slot.position, slot.rotation);
        enemyView.transform.parent = slot;
        EnemyViews.Add(enemyView);
    }
}
