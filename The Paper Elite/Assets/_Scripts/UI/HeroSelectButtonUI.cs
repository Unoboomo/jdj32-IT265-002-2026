using UnityEngine;
using UnityEngine.UI;

public class HeroSelectButtonUI : MonoBehaviour
{
    [SerializeField] private HeroData HeroData;
    [SerializeField] private Image portraitImage;

    private void Start()
    {
        if (HeroData != null)
        {
            portraitImage.sprite = HeroData.Image;
        }
    }

    public void OnClick()
    {
        CharacterSelectSystem.Instance.SelectHero(HeroData);
    }
}