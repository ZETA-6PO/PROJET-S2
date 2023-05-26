
using TMPro;
using UnityEngine;

public class MStatCell : MonoBehaviour
{
    [SerializeField] private TMP_Text _statName;
    [SerializeField] private TMP_Text _variable;

    public void Initialize(string stat, string var)
    {
        _statName.text = stat;
        _variable.text = var;
    }
}
