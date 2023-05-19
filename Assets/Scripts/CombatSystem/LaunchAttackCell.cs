using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LaunchAttackCell : MonoBehaviour
{
    //UI objects
    public Image icon;
    public TMP_Text title;
    public TMP_Text countText;
    
    //
    private AttackObject _attack;
    private AttackMenu _launcher;
    private int _number;
    private int _index;

    public void Initialise(AttackObject a, AttackMenu m,int use,int i)
    {
        _index = i;
        _attack = a;
        _launcher = m;
        icon.sprite = _attack.image;
        title.text = _attack.name;
        _number = _attack.MaxUse-use;
        if (_number < 0) _number = 0;
        countText.text = _number.ToString();
        enabled = true;
        ActiveCell();
    }
    
    private void ActiveCell()
    {
        icon.gameObject.SetActive(true);
        title.gameObject.SetActive(true);
        countText.gameObject.SetActive(true);
    }


    public void CellClicked()
    {
        if (_number>0)
        {
            _number -= 1;
            countText.text = _number.ToString();
            Debug.Log("attack"+_index+"clicked");
            _launcher.OnclickAttack(_index);
            
        }
    }

}
