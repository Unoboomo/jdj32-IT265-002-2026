using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class UISystem : Singleton<UISystem>
{
    [Header("UI Panels")]
    [SerializeField] private GameObject titleScreenPanel;
    [SerializeField] private GameObject characterSelectPanel;
    [SerializeField] private GameObject matchUIPanel;
    [SerializeField] private GameObject winScreenPanel;
    [SerializeField] private GameObject loseScreenPanel;
    [SerializeField] private GameObject turnTransitionPanel;
    [Header("Turn Transition UI")]
    [SerializeField] private TMP_Text turnTransitionText;
    [Header("World Views")]
    [SerializeField] private GameObject combatEnvironmentRoot; 
    public bool IsWaitingForPlayerReady { get; private set; }

    private void Start()
    {
        ShowTitleScreen();
    }
    private void HideAllPanels()
    {
        titleScreenPanel.SetActive(false);
        characterSelectPanel.SetActive(false);
        matchUIPanel.SetActive(false);
        winScreenPanel.SetActive(false);
        loseScreenPanel.SetActive(false);
        turnTransitionPanel.SetActive(false);
        if (combatEnvironmentRoot != null)
        {
            combatEnvironmentRoot.SetActive(false);
        }
    }

    public void ShowTitleScreen()
    {
        HideAllPanels();
        titleScreenPanel.SetActive(true);
    }
    public void ShowCharacterSelect()
    {
        HideAllPanels();
        characterSelectPanel.SetActive(true);
        if (CharacterSelectSystem.Instance != null)
        {
            CharacterSelectSystem.Instance.ResetSelection();
        }
    }
    public void OnClickBeginMatch(List<HeroData> selectedHeroes)
    {
        HideAllPanels();
        matchUIPanel.SetActive(true);
        if (combatEnvironmentRoot != null)
        {
            combatEnvironmentRoot.SetActive(true);
        }
        MatchSetupSystem.Instance.StartCombat(selectedHeroes);
    }
    public void ShowWinScreen()
    {
        HideAllPanels();
        winScreenPanel.SetActive(true);
    }
    public void ShowLoseScreen()
    {
        HideAllPanels();
        loseScreenPanel.SetActive(true);
    }
    public void ShowTurnTransition(string playerName)
    {
        HideAllPanels();
        turnTransitionPanel.SetActive(true);
        IsWaitingForPlayerReady = true;

        if (turnTransitionText != null)
        {
            turnTransitionText.text = $"Pass the device to\n{playerName}";
        }
    }
    public void ConfirmPlayerReady()
    {
        IsWaitingForPlayerReady = false;
        turnTransitionPanel.SetActive(false);
        matchUIPanel.SetActive(true);

        if (combatEnvironmentRoot != null)
        {
            combatEnvironmentRoot.SetActive(true);
        }
    }

    public void ReturnToTitleScreen()
    {
        if (MatchSetupSystem.Instance != null)
        {
            MatchSetupSystem.Instance.CleanupMatch();
        }
        ShowTitleScreen();
    }
}
