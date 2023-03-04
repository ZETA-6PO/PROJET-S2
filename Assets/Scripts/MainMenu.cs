using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	
    // Start is called before the first frame update
	public void StartNewGame()
	{
		Debug.Log("StartNewGame");
		//SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		
	}
	
	public void ChargeGame()
    {
	    Debug.Log("ChargeGame");
    }
	
    public void QuitGame()
    {
	    Debug.Log("QUIT!");
	    Application.Quit();
    }
    
}
