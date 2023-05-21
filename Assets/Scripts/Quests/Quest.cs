using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


/// <summary>
/// This class handle all the OOP for the Quests.
/// </summary>
public abstract class Quest : ScriptableObject
{
    /// <summary>
    /// This is the QuestId for referencing the Quest.
    /// </summary>
    public string QuestId;
    
    /// <summary>
    /// Stores the eventual dialogue.
    /// </summary>
    [SerializeField] public List<string> dialogues;
    /// <summary>
    /// Stores the eventual prefabs if the quest require to instance one.
    /// </summary>
    [SerializeField] public List<GameObject> gameObjects;
    /// <summary>
    /// Stores the information required for the UI.
    /// </summary>
    [Header("Info")] public Info information;


    [SerializeField] public List<Vector3> waypoints;

    public bool Active;
    public bool Completed;
    
    /// <summary>
    /// Struct containing all basic Quest information such as the name and description.
    /// </summary>
    [System.Serializable]
    public struct Info 
    {
        public string name;
        public string description;
    }
    /// <summary>
    /// This function is called by the GameManager whenever a Scene is load.
    /// </summary>
    /// <param name="sceneName">The loaded scene's name.</param>
    public abstract void OnLoadScene(string sceneName);
    
    /// <summary>
    /// Use this function to load the Quest's variable with the saved objects.
    /// </summary>
    /// <param name="questProperties"></param>
    public abstract void LoadQuestProperties(QuestData.QuestProperty[] questProperties);
    
    /// <summary>
    /// Use this function to store the Quest's variable as property.
    /// </summary>
    /// <returns></returns>
    public abstract QuestData.QuestProperty[] SaveQuestProperties();

}