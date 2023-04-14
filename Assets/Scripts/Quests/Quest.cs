using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public abstract class Quest : ScriptableObject
{
    public string QuestId;

    [SerializeField] public List<string> dialogues;
    [SerializeField] public List<GameObject> gameObjects;
    
    [System.Serializable]
    public struct Info 
    {
        public string name;
        public string description;
    }

    [Header("Info")] public Info Information;
    
    public abstract void onLoadScene(string sceneName); //called whenever a scene is loaded

    public abstract IEnumerator onStartQuest(); //called when the quest start 
    
    
}