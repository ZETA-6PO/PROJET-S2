using System;
using UnityEngine;

[Serializable]
public class QuestData
{
    public string questId;
    public bool active;
    public bool completed;
    public QuestProperty[] questProperties;
    
    /// <summary>
    /// A quest properties is a storable property that can be
    /// saved.
    /// </summary>
    [Serializable]
    public struct QuestProperty
    {
        public string name;
        public string value;
    }

    public QuestData(string questId, bool active, bool completed, QuestProperty[] questProperties)
    {
        this.questId = questId;
        this.active = active;
        this.completed = completed;
        this.questProperties = questProperties;
    }
    
}