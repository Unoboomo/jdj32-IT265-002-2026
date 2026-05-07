using UnityEngine;
using SerializeReferenceEditor;
using System.Collections.Generic;

public enum CardType
{
    Attack,
    Block,
    Special,
}

[CreateAssetMenu(menuName = "Data/Card")]
public class CardData : ScriptableObject
{
    [field: SerializeField] public string Description {  get; private set; }
    [field: SerializeField] public int Mana { get; private set; }
    [field: SerializeField] public CardType Type { get; private set; }
    [field: SerializeField] public Sprite Image { get; private set; }

    [field: SerializeReference,SR] public List<Effect> Effects { get; private set; }

}


