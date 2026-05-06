using TMPro;
using UnityEngine;

public class EnemyView : CombatantView
{
    [SerializeField] private TMP_Text attackText;

    public int AttackPower {  get; set; }

    public void Setup(EnemyData enemyData)
    {
        AttackPower = enemyData.AttackPower;
        updateAttackText();
        SetupBase(enemyData.Health, enemyData.Image);
    }

    private void updateAttackText()
    {
        attackText.text = "AKT: " + AttackPower;
    }
}
