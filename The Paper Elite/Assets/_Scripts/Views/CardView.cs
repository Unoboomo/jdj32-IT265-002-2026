using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class CardView : MonoBehaviour
{
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text mana;
    [SerializeField] private TMP_Text type;

    [SerializeField] private SpriteRenderer imageSR;

    [SerializeField] private GameObject wrapper;

    [SerializeField] private LayerMask dropLayer;
    public bool IsAnimating { get; set; } = false;
    public Card Card {  get; private set; }

    private Vector3 dragStartPosition;
    private Quaternion dragStartRotation;
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
        if (IsAnimating || !Interactions.Instance.PlayerCanHover()) 
        {
            return;
        }
        wrapper.SetActive(false);
        Vector3 position = new(transform.position.x, -2, 0);
        CardViewHoverSystem.Instance.Show(Card, position);
    }

    public void OnMouseExit()
    {
        CardViewHoverSystem.Instance.Hide();
        if (IsAnimating || !Interactions.Instance.PlayerCanHover())
        {
            return;
        }
        wrapper.SetActive(true);
    }

    public void OnMouseDown()
    {
        if (!Interactions.Instance.PlayerCanInteract())
        {
            return;
        }
        Interactions.Instance.PlayerIsDragging = true;
        CardViewHoverSystem.Instance.Hide();
        wrapper.SetActive(true);
        dragStartPosition = transform.position;
        dragStartRotation = transform.rotation;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.position = MouseUtil.GetMousePositionInWorldSpace(-1);
    }

    public void OnMouseDrag()
    {
        if (!Interactions.Instance.PlayerCanInteract())
        {
            return;
        }
        transform.position = MouseUtil.GetMousePositionInWorldSpace(-1);
    }

    public void OnMouseUp()
    {
        if (!Interactions.Instance.PlayerCanInteract())
        {
            return;
        }
        if (Physics.Raycast(transform.position, Vector3.forward, out RaycastHit hit, 10f, dropLayer))
        {
            PlayCardGA playCardGA = new(Card);
            ActionSystem.Instance.Perform(playCardGA);
        }
        else
        {
            transform.position = dragStartPosition;
            transform.rotation = dragStartRotation;
        }
        Interactions.Instance.PlayerIsDragging = false;

    }
}
