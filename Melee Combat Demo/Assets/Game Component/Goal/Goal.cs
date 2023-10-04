using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other) {

        if (other.gameObject.CompareTag("Player")) {
            Debug.Log("reached");
            SceneManager.LoadScene("Ending");
        }
    }


} // class
