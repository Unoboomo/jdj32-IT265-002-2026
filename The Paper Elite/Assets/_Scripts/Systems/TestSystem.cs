using UnityEngine;
using System.Collections.Generic;

public class TestSystem : MonoBehaviour
{
    [SerializeField] private List<CardData> deckData;

    private void Start()
    {
        CardSystem.Instance.Setup(deckData);
    }
}
