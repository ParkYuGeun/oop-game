using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class is responsible for handling the Play Game button functionality.
using UnityEngine.SceneManagement;

public class PlayExitGameButton : MonoBehaviour
{
    public void OnExitGameButtonClicked()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void OnPlayGameButtonClicked()
    {
        // Load the main game scene when the Play Game button is clicked.
        SceneManager.LoadScene("SampleScene");
    }
}
