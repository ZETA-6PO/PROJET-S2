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
        Debug.Log($"Entering a linker.");
        if (collision.CompareTag("Player"))
        {
            Debug.Log($"Set the exit position at : {DataPersistenceManager.Instance.gameData.lastPosition}");
            DataPersistenceManager.Instance.gameData.lastPosition = exitPoint.position;
            
            
            //set the last position on map
            
            
            SceneManager.LoadScene(sceneName);
            
        }
    }
}