using DG.Tweening;
using TMPro;
using UnityEngine;

public class CombatantView : MonoBehaviour
{
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private SpriteRenderer spriteRenderer;

    public int MaxHealth {  get; private set; }
    public int CurrentHealth { get; private set; }

    protected void SetupBase(int maxHealth, int currentHealth, Sprite image)
    {
        MaxHealth = maxHealth;
        CurrentHealth = currentHealth;
        spriteRenderer.sprite = image;
        UpdateHealthText();
    }

    private void UpdateHealthText()
    {
        healthText.text = "HP: " + CurrentHealth;
    }

    public virtual void Damage(int damageAmount)
    {
        CurrentHealth -= damageAmount;
        if (CurrentHealth < 0) 
        { 
            CurrentHealth = 0; 
        }
        transform.DOShakePosition(0.2f, 0.5f);
        UpdateHealthText();
    }
}
