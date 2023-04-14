using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestCell : MonoBehaviour
{
    // class for the quest cell prefab
    public TMP_Text Name;
    public Image doneToggle;
    public Quest quest;
    public QuestManager Manager;
    public void Initialise(Quest q, QuestManager manager)
    {
        quest = q;
        Manager = manager;
        Name.text = quest.information.name;
        doneToggle.gameObject.SetActive(quest.Completed);
    }

    public void Done()
    {
        doneToggle.gameObject.SetActive(true);
    }

    public void Clicked()
    {
        Manager.space.Change(quest);
    }
}
