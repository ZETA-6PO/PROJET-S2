
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugMenu : MonoBehaviour
{
    [SerializeField] PlayerController controller;
    public GameObject player;

    public void Start()
    {
        controller = FindObjectOfType<PlayerController>();
        player = controller.gameObject;
    }

    public void ChangeSpeed(string speed)
    {
        controller.moveSpeed = float.Parse(speed);
    }

    public void Change_X(string x)
    {
        Debug.Log(x);
        player.transform.position = new Vector3(Convert.ToSingle(x),player.transform.position.y);
    }
    
    public void Change_Y(string y)
    {
        player.transform.position = (new Vector3(player.transform.position.x,float.Parse(y)));
    }

    public void ChangeMap(string map)
    {
        SceneManager.LoadScene(map);
    }

    public void ChangeQuest(string quest_id)
    {
        Debug.Log(quest_id);
        foreach (Quest oldQuest in GameManager.Instance.quests)
        {
            oldQuest.Active = false;
            oldQuest.Completed = true;
        }
        GameManager.Instance.quests.First(quest => quest.QuestId == quest_id).Active = true;
        GameManager.Instance.quests.First(quest => quest.QuestId == quest_id).Completed = false;
    }

    public void ChangeCoins(string coins)
    {
        Debug.Log(coins);
        GameManager.Instance.AddCoins(Convert.ToInt32(coins));
    }
}
