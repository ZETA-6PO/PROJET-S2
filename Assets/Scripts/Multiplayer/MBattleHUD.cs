using System;
using UnityEngine;
using UnityEngine.UI;

public class MBattleHUD: MonoBehaviour
{
    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private Transform playerParent;
    [SerializeField]
    private Transform enemyParent;
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

    public void Initialize(MultiplayerFighter player, MultiplayerFighter enemy)
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

    private void InitializePlayer(MultiplayerFighter fighter)
    {
        playerText.text = fighter.nickName;
        GameObject go = Instantiate(playerPrefab, playerParent, false);
    }
        
    private void InitializeEnemy(MultiplayerFighter fighter)
    {
        enemyText.text = fighter.nickName;
            
        var go = Instantiate(enemyPrefab, enemyParent, false);
    }

    private void InitializeBar(MultiplayerFighter player)
    {
        inspiration.value = player.inspiration/10;
        resistance.value = player.resistance / 10;
        appreciation.value = 0.5f;
    }

    public void UpdateAppreciation(float appreciation)
    {
        this.appreciation.value = appreciation;
    }
    
    public void UpdateInspiration(float inspiration)
    {
        this.inspiration.value = inspiration;
    }
    
    public void UpdateResistance(float resistance)
    {
        this.resistance.value = resistance;
    }
}
