using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition_Door : MonoBehaviour
{
    public string loadLevel;
    public GameObject Door;
 
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetButtonDown("Action"))
            {
                SceneManager.LoadScene(loadLevel);
            }
        }
    }
}
