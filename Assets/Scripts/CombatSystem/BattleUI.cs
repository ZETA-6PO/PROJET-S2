using System;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer playerSprite;
    [SerializeField]
    private SpriteRenderer enemySprite;
    [SerializeField]
    private Text playerText;
    [SerializeField]
    private Text enemyText;
    [SerializeField]
    private Text dialogText;
    [SerializeField]
    private Slider appreciation;
    [SerializeField]
    private Slider resistance;
    [SerializeField]
    private Slider inspiration;
    
    //private GameObject PauseScreen;

    public void Initialize(Fighter player, Fighter enemy)
    {
        InitializePlayer(player);
        InitializeEnemy(enemy);
        InitializeBar(player);
    }

    public void SetDialogText(string text)
    {
        dialogText.text = text;
    }

    /*public void ShowPauseMenu()
    {
        PauseScreen.SetActive(true);
    }*/
        
    /*public void HidePauseMenu()
    {
        PauseScreen.SetActive(false);
    }*/

    private void InitializePlayer(Fighter fighter)
    {
        playerText.text = fighter.unitName;
        playerSprite.sprite = fighter.sprite;
    }
        
    private void InitializeEnemy(Fighter fighter)
    {
        enemyText.text = fighter.unitName;
        enemySprite.sprite = fighter.sprite;
    }

    private void InitializeBar(Fighter player)
    {
        inspiration.value = player.inspiration / 10;
        resistance.value = player.resistance / 10;
        appreciation.value = 0.5f;
    }

    public void UpdateAppreciation(float _appreciation)
    {
        if (_appreciation>1)
            this.appreciation.value = _appreciation/100;
        else
        {
            this.appreciation.value = _appreciation;
        }
    }
    
    public void UpdateInspiration(float inspiration)
    {
        Debug.LogError($"updt inspi = {inspiration}");
        this.inspiration.value = inspiration/10;
    }
    
    public void UpdateResistance(float resistance)
    {
        Debug.LogError($"updt inspi = {resistance}");
        this.resistance.value = resistance/10;
    }
}
