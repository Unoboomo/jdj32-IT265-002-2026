using System.Collections;
using UnityEngine;

public class ManaSystem : Singleton<ManaSystem>
{
    [SerializeField] private ManaUI manaUI;

    private Player ActivePlayer => PlayerSystem.Instance.ActivePlayer;

    void OnEnable()
    {
        ActionSystem.AttachPerformer<SpendManaGA>(SpendManaPerformer);
        ActionSystem.AttachPerformer<RefillManaGA>(RefillManaPerformer);
    }
    public bool HasEnoughMana(int mana)
    {
        if (ActivePlayer == null) return false;
        return ActivePlayer.CurrentMana >= mana;
    }
    void OnDisable()
    {
        ActionSystem.DetachPerformer<SpendManaGA>();
        ActionSystem.DetachPerformer<RefillManaGA>();
    }
    private IEnumerator SpendManaPerformer(SpendManaGA spendManaGA)
    {
        ActivePlayer.CurrentMana -= spendManaGA.Amount;
        manaUI.UpdateManaText(ActivePlayer.CurrentMana);
        yield return null;
    }

    private IEnumerator RefillManaPerformer(RefillManaGA refillManaGA)
    {
        ActivePlayer.CurrentMana = Player.DEFAULT_MANA;
        manaUI.UpdateManaText(ActivePlayer.CurrentMana);
        yield return null;
    }
    public void UpdateManaUI()
    {
        manaUI.UpdateManaText(ActivePlayer.CurrentMana);
    }
}
