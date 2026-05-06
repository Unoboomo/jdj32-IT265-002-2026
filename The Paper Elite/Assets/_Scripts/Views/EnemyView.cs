using TMPro;
using UnityEngine;

public class EnemyView : CombatantView
{
    [SerializeField] private TMP_Text attackText;

    public int AttackPower {  get; set; }

    public void Setup()
    {
        AttackPower = 10;
        updateAttackText();
        SetupBase(30, null);
    }

    private void updateAttackText()
    {
        attackText.text = "AKT: " + AttackPower;
    }
}
