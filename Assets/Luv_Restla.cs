using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Luv_Restla : MonoBehaviour
{
    public void TeleportAt(Vector3 teleport)
    {
        FindObjectOfType<Luv_Restla>().transform.position = teleport;
    }
}
