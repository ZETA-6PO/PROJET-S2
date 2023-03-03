
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log(transform.position.ToString());
        GameObject.FindGameObjectWithTag("Player").transform.position = transform.position;
    }
}
