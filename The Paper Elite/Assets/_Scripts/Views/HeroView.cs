using TMPro;
using UnityEngine;

public class HeroView : CombatantView
{
    public void Setup(int maxHealth, int currentHealth, Sprite image)
    {
        SetupBase(maxHealth, currentHealth, image);
    }
    public override void Damage(int damageAmount)
    {
        base.Damage(damageAmount);

        // Sync the UI health back to the actual Player data!
        if (PlayerSystem.Instance != null && PlayerSystem.Instance.ActivePlayer != null)
        {
            PlayerSystem.Instance.ActivePlayer.CurrentHealth = CurrentHealth;
        }
    }
}
