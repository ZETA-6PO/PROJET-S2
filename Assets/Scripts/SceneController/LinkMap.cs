using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LinkMap : MonoBehaviour
{
     //This is the corresponding interrior scene
        public string sceneName;
        
        //this is the exit point
        public Transform exitPoint;

        public int numQuest;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") && GameManager.Instance.quests[numQuest].Completed)
            {
                
                DataPersistenceManager.Instance.gameData.lastPosition = exitPoint.position;
                
                
                //set the last position on map
                SceneManager.LoadScene(sceneName);
                
            }
        }
}
