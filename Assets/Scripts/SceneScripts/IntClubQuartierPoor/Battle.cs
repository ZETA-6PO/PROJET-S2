using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Battle : MonoBehaviour
{

    public Fighter[] fighters;
    
    public void BattleTime()
    {
        if (!GameManager.Instance.quests[3].Active && !GameManager.Instance.quests[6].Active 
                                                   && GameManager.Instance.quests[1].Completed
                                                   && GameManager.Instance.quests[2].Completed)
        {
            int rand = Random.Range(0, 10);
            StartBattle(fighters[rand]);
        }
    }
    
    
    private void StartBattle(Fighter ennemy)
    {
        // Start Combat
        FindObjectOfType<GameManager>().StartACombat(ennemy, isWin =>
        {
            if (isWin)
            {
                FindObjectOfType<DialogManager>().StartDialogue(
                    new Dialogue(new[]
                    {
                        new SingleDialogue("KoBalaD", new[]
                        {
                            "I will never get over such humiliation. You are truly a great artist."
                        }),
                        new SingleDialogue("Studio Boss", new[]
                        {
                            "You might have been lucky but hey. Here you go as promised."
                        }),
                        new SingleDialogue("", new[]
                        {
                            "Good job ! Here are 1000 HypeCoins offered by the Studio Boss to thank you for this performance.",
                            "Great, you now have a new instrument available during battles. Don’t hesitate to use it in order to win more often.",
                            "Furthermore, there are some new instruments available in the music store,check them out. They can really help you in the future."
                            
                        })
                    }),
                    Array.Empty<string>(),
                    i => { });

                int randReward = Random.Range(2, 10);
                FindObjectOfType<GameManager>().AddCoins(randReward*10);

            }
            else
            {
                FindObjectOfType<DialogManager>().StartDialogue(
                    new Dialogue(new[]
                    {
                        new SingleDialogue("KoBalaD", new[]
                        {
                            "Honestly easy I didn't even have to force it."
                        })
                    }),
                    new string[]{"I will beat you this time!", "No I prefer to stop the massacre..."},
                    i =>
                    {
                        if (i == 1)
                        {
                            StartBattle(ennemy);
                        }
                        else
                        {
                            SceneManager.LoadScene("ExtFirstScene");   
                        }
                        
                    });
            }
        } );
        
    }
}
