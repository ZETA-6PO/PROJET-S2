using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MSlot : MonoBehaviour
{
    [SerializeField] private Image img;
    private MultiplayerFighter _fighter;
    private MSetter _launcher;

    public void Initialize(MultiplayerFighter fighter, MSetter launcher)
    {
        _fighter = fighter;
        img.sprite = _fighter.image;
        _launcher = launcher;
    }

    public void SlotClicked()
    {
        _launcher.ChangeCurrentFighter(_fighter);
    }
}
