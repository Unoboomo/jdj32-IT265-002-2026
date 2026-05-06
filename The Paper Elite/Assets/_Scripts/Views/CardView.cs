using TMPro;
using UnityEngine;

public class CardView : MonoBehaviour
{
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text mana;
    [SerializeField] private TMP_Text type;

    [SerializeField] private SpriteRenderer imageSR;

    [SerializeField] private GameObject wrapper;
    public bool IsInteractive { get; set; } = true;
    public Card Card {  get; private set; }

    public void Setup(Card card)
    {
        Card = card;
        title.text = card.Title;
        description.text = card.Description;
        mana.text = card.Mana.ToString();
        type.text = card.Type;
        imageSR.sprite = card.Image;
    }

    public void OnMouseEnter()
    {
        if (!IsInteractive) 
        {
            return;
        }
        wrapper.SetActive(false);
        Vector3 position = new(transform.position.x, -2, 0);
        CardViewHoverSystem.Instance.Show(Card, position);
    }

    public void OnMouseExit()
    {
        if (!IsInteractive)
        {
            return;
        }
        CardViewHoverSystem.Instance.Hide();
        wrapper.SetActive(true);
    }
}
