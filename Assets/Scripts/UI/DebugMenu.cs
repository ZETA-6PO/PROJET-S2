
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugMenu : MonoBehaviour
{
    [SerializeField] PlayerController controller;
    public GameObject player;
    
    
    public void ChangeSpeed(string speed)
    {
        controller.moveSpeed = float.Parse(speed);
    }

    public void Change_X(string x)
    {
        controller.teleportPlayerAt(new Vector3(float.Parse(x),player.transform.position.y));
    }
    
    public void Change_Y(string y)
    {
        controller.teleportPlayerAt(new Vector3(player.transform.position.x,float.Parse(y)));
    }

    public void ChangeMap(string map)
    {
        SceneManager.LoadScene(map);
    }

    public void ChangeQuest(string quest_id)
    {
        foreach (Quest oldQuest in GameManager.Instance.quests)
        {
            oldQuest.Active = false;
        }
        GameManager.Instance.quests.First(quest => quest.QuestId == quest_id).Active = true;
    }
}
