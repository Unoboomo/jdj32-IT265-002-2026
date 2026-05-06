using Unity.VisualScripting;
using UnityEngine;
using System.Collections;


public class EnemyViewDestroyer : Singleton<EnemyViewDestroyer>
{
    public IEnumerator DestroyEnemyView(EnemyView enemyView)
    {
        Destroy(enemyView.gameObject);
        yield return null;
    }
} 
