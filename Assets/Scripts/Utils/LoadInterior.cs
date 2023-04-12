using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// This class is responsible of loading interrior.
/// It must be placed beside a collidebox.
/// </summary>
public class LoadInterior : MonoBehaviour
{
    //This is the corresponding interrior scene
    public string sceneName;
    
    //this is the exit point
    public Transform exitPoint;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //set the last position on map
            DataPersistenceManager.Instance.gameData.lastPosition = exitPoint.position;
            
            
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
            
        }
    }
}