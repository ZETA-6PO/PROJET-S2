using UnityEngine;
using UnityEngine.SceneManagement;

public class FromMusicStoreToExt : MonoBehaviour
{
    public string sceneName;
    public string buildingExit;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SceneManager.LoadScene(sceneName);
            switch (buildingExit)
            {
                case "MusicStorePoor":
                    GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(-15,4,0);
                    Destroy(GameObject.FindGameObjectWithTag("Player"));
                    Destroy(GameObject.FindGameObjectWithTag("MainCamera"));
                    break;
                case "InteriorMapsStudioPoor":
                    GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(-14,32,0);
                    Destroy(GameObject.FindGameObjectWithTag("Player"));
                    Destroy(GameObject.FindGameObjectWithTag("MainCamera"));
                    break;
                case "InteriorMapFirstScene":
                    GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(-108,16,0);
                    Destroy(GameObject.FindGameObjectWithTag("Player"));
                    Destroy(GameObject.FindGameObjectWithTag("MainCamera"));
                    break;
                case "InteriorMapShopPoor":
                    GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(-130,-2,0);
                    Destroy(GameObject.FindGameObjectWithTag("Player"));
                    Destroy(GameObject.FindGameObjectWithTag("MainCamera"));
                    break;
                default:
                    GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(-15,4,0);
                    break;
            }
        }
    }
}
