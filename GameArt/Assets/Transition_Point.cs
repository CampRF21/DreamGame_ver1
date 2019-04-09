using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Transition_Point : MonoBehaviour
{
    public string loadLevel;

    public void frontGate()
    {
        SceneManager.LoadScene(loadLevel);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            frontGate();
        }
    }


}
