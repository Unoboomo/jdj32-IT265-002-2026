using UnityEngine;

public class HeroSystem : Singleton<HeroSystem>
{
    [field: SerializeField] public HeroView HeroView {  get; private set; }

    public void Setup(int maxHealth, int currentHealth, Sprite image)
    {
        HeroView.Setup(maxHealth, currentHealth, image);
    }
}
