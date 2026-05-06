using System.Collections;
using UnityEngine;

public class EnemySystem : MonoBehaviour
{

    void OnEnable()
    {
        ActionSystem.AttachPerformer<EnemyTurnGA>(EnemyTurnPerformer);
    }

    void OnDisable()
    {
        ActionSystem.DetachPerformer<EnemyTurnGA>();
    }
    // Performers

    private IEnumerator EnemyTurnPerformer(EnemyTurnGA enemyTurnGA)
    {
        Debug.Log("enemy turn");
        yield return new WaitForSeconds(2f);
        Debug.Log("End enemy turn");
    }
}
