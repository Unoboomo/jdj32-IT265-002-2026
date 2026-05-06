using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerSystem : Singleton<PlayerSystem>
{
    public List<Player> PlayerList = new();
    
    public int PlayerCount => PlayerList.Count;
    public Player ActivePlayer { get; private set; }
    private int activePlayerIndex = 0;
    private int deadPlayers = 0;

    void OnEnable()
    {
        ActionSystem.AttachPerformer<KillPlayerGA>(KillPlayerPerformer);
        ActionSystem.AttachPerformer<SwitchActivePlayerGA>(SwitchPlayerPerformer);
        ActionSystem.AttachPerformer<EndPlayerTurnGA>(EndPlayerTurnPerformer);

    }

    void OnDisable()
    {
        ActionSystem.DetachPerformer<KillPlayerGA>();
        ActionSystem.DetachPerformer<SwitchActivePlayerGA>();
        ActionSystem.DetachPerformer<EndPlayerTurnGA>();
    }

    public void Setup(List<HeroData> heroDatas)
    {
        PlayerList.Clear();
        foreach (var heroData in heroDatas)
        {
            Player player = new(heroData);
            PlayerList.Add(player);
        }
        activePlayerIndex = 0;
        SetActivePlayer(PlayerList[activePlayerIndex]);
    }

    private void SetActivePlayer(Player player)
    {

        ActivePlayer = player;
        HeroSystem.Instance.Setup(ActivePlayer.MaxHealth, ActivePlayer.CurrentHealth, ActivePlayer.Image);
        CardSystem.Instance.Setup(ActivePlayer.drawPile, ActivePlayer.discardPile, ActivePlayer.hand);
        ManaSystem.Instance.UpdateManaUI();
    }
    
    public bool MoveToNextPlayer()
    {
        if (deadPlayers >= PlayerCount)
        {
            //game over
            Debug.Log("All players are dead. Game Over.");
            return false;
        }

        do
        {
            activePlayerIndex = (activePlayerIndex + 1) % PlayerCount;
        } while (!PlayerList[activePlayerIndex].Alive);
        SetActivePlayer(PlayerList[activePlayerIndex]);
        return true;
    }
    private IEnumerator EndPlayerTurnPerformer(EndPlayerTurnGA endPlayerTurnGA)
    {
        ActionSystem.Instance.AddReaction(new DiscardAllCardsGA());

        yield return null;
    }
    private IEnumerator SwitchPlayerPerformer(SwitchActivePlayerGA switchActivePlayerGA)
    {
        if (!MoveToNextPlayer())
        {
            //end of game
            yield break;
        }

        ActionSystem.Instance.AddReaction(new RefillManaGA());
        ActionSystem.Instance.AddReaction(new DrawCardsGA(Player.DEFAULT_DRAW_AMOUNT));

        yield return null;
    }

    private IEnumerator KillPlayerPerformer(KillPlayerGA killPlayerGA)
    {
        if (ActivePlayer.Alive)
        {
            ActivePlayer.Alive = false;
            deadPlayers++;
            if (deadPlayers >= PlayerCount)
            {
                //game over
                Debug.Log("All players are dead. Game Over.");
            }
        }
        yield return null;
    }
}
