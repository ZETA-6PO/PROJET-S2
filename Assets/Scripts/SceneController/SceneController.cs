using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneController : MonoBehaviour
    {
        
        public PlayerController playerController;
        public GameObject playerPrefab;
        public GameObject cameraPrefab;
        public Transform entryPoint;
        public GameObject dialogPrefab;
        public GameObject playerInstanciated;
        public GameObject cameraInstanciated;
        public GameObject dialogInstanciated;
        public DialogManager dialogManager;

        public bool canGoOut;
        IEnumerator Start()
        {
            yield return new WaitUntil(() => DataPersistenceManager.Instance != null && GameManager.Instance!=null);
            Init();
            PreStart();
            OnStart();
        }

        public virtual void OnStart()
        {
            
        }

        //This function instanciate player, camera, dialogbox
        void PreStart()
        {
            cameraPrefab.SetActive(false);
            playerInstanciated = Instantiate(playerPrefab, entryPoint.position, Quaternion.identity);
            cameraInstanciated = Instantiate(cameraPrefab, entryPoint.position, Quaternion.identity);
            CameraFollow cf = cameraInstanciated.GetComponent<CameraFollow>();
            cf.player = playerInstanciated;
            cameraInstanciated.SetActive(true);
            dialogInstanciated = Instantiate(dialogPrefab, Vector3.zero, Quaternion.identity);
            dialogManager = dialogInstanciated.GetComponent<DialogManager>();
            dialogManager.player = playerInstanciated;
            playerController = playerInstanciated.GetComponent<PlayerController>();
            playerController.canMove = true;
            GameManager.Instance.OnSceneLoad();
            
            
            //Set if the MiniMap appear or not if we are in Exterior or in Interior
            
            if (SceneManager.GetActiveScene().name.StartsWith("Ext"))
            {
                cf.MiniMapAnim.MiniMapAnimator.SetBool("IsOpen", true);
            }
            else
            {
                cf.MiniMapAnim.MiniMapAnimator.SetBool("IsOpen", false);
            }
        }
        
        //set the variable prior the start
        public virtual void Init()
        {
            entryPoint.position = DataPersistenceManager.Instance.gameData.lastPosition;
        }
    }