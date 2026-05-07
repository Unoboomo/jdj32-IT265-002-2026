using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterSelectSystem : Singleton<CharacterSelectSystem>
{
    [SerializeField] private TMP_Text selectedHeroesText;
    [SerializeField] private int maxPlayers = 4;

    private List<HeroData> selectedHeroes = new();

    public void ResetSelection()
    {
        selectedHeroes.Clear();
        UpdateUI();
    }
    public void SelectHero(HeroData heroData)
    {
        if (selectedHeroes.Count >= maxPlayers)
        {
            Debug.Log("Max players reached");
            return;
        }

        selectedHeroes.Add(heroData);
        UpdateUI();
    }
    private void UpdateUI()
    {
        if (selectedHeroesText == null) return;

        selectedHeroesText.text = "Selected Players:\n";
        for (int i = 0; i < selectedHeroes.Count; i++)
        {
            selectedHeroesText.text += $"Player {i + 1}: {selectedHeroes[i].name}\n";
        }
    }
    public void ConfirmSelection()
    {
        if (selectedHeroes.Count == 0)
        {
            Debug.Log("Select at least one hero");
            return;
        }

        UISystem.Instance.OnClickBeginMatch(selectedHeroes);
    }
}
