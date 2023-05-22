using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    
    public GameObject panel;
    public DescriptionSpace space;
    public GameObject questPrefab;
    
    public void UpdateQuests(List<Quest> quests)
    {
        if (quests.Count>0) space.Change(quests[0]);
        QuestCell[] list = panel.GetComponents<QuestCell>();
        foreach (QuestCell cell in list)
        {
            Destroy(cell);
        }
        foreach (Quest quest in quests)
        {
            GameObject cellObject = Instantiate(questPrefab, panel.transform);
            QuestCell cell = cellObject.GetComponent<QuestCell>();
            cell.Initialise(quest,this);
        }
    }
    public void AddOneQuest(Quest quest)
    {
        GameObject cellObject = Instantiate(questPrefab, panel.transform);
        QuestCell cell = cellObject.GetComponent<QuestCell>();
        cell.Initialise(quest,this);
    }
    
}
