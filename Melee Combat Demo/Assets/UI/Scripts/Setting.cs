using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Setting : MonoBehaviour
{
    private Animator m_animator;
    private bool isPaused = false;

    void Start()
    {
        GameObject pauseButton = GameObject.FindGameObjectWithTag("pauseButton");
        m_animator = pauseButton.GetComponent<Animator>();
    }

    public void Restart() {
        SceneManager.LoadScene("GamePlay");
    }

    public void Pause() {
        isPaused = !isPaused;
        m_animator.SetBool("isPause", isPaused);

        if (isPaused == true)
        {
            Time.timeScale = 0;
        }
        else {
            Time.timeScale = 1;
        }
    }




} // class
