using System;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
    [SerializeField] private Image playerSprite;
    [SerializeField] private Image enemySprite;
    [SerializeField] private Text playerText;
    [SerializeField] private Text enemyText;
    [SerializeField] private Text dialogText;
    [SerializeField] private Slider appreciation;
    [SerializeField] private Slider resistance;
    [SerializeField] private Slider inspiration;
    [SerializeField] private Button refForfeitButton;

    private float appreciationToReach = 0.5f;
    private float currentAppreciationVelocity = 0;

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

    public void Update()
    {
        if (appreciation.value != appreciationToReach)
        {
            appreciation.value = Mathf.SmoothDamp(appreciation.value, appreciationToReach, ref currentAppreciationVelocity, 30 * Time.deltaTime);
        }
    }

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
        if (_appreciation > 1)
            appreciationToReach = _appreciation / 100;
        else
        {
            appreciationToReach = _appreciation;
        }
    }

    public void UpdateInspiration(float inspiration)
    {
        this.inspiration.value = inspiration / 10;
    }

    public void UpdateResistance(float resistance)
    {
        this.resistance.value = resistance / 10;
    }

    public void EnableButtonForfeit()
    {
        refForfeitButton.gameObject.SetActive(true);
    }

    public void DisableButtonForfeit()
    {
        refForfeitButton.gameObject.SetActive(false);
    }



}
