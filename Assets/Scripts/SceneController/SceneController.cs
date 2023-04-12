using System.Collections;
using UnityEngine;


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
        IEnumerator Start()
        {
            yield return new WaitUntil(() => DataPersistenceManager.Instance != null && QuestManager.Instance!=null);
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
            QuestManager.Instance.OnSceneLoad();
        }
        
        //set the variable prior the start
        public virtual void Init()
        {
            //entryPoint.position = Vector3.zero;
        }
    }