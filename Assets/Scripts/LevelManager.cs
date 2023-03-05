using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public void LoadGame(){
        SceneManager.LoadScene("Level 1");
    }

    public void QuitGame(){
        Application.Quit();
    }
}
