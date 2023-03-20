using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    ScoreKeeper scoreKeeper;
    [SerializeField] float loadGameOverDelay = 2f;
    [SerializeField] float loadVictoryDelay = 1f;

    void Start(){
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
    }

    public void LoadMenu(){
        SceneManager.LoadScene("Main Menu");
    }

    public void LoadGame(){
        scoreKeeper.ResetScore();
        SceneManager.LoadScene("Level 1");
    }

    public void LoadNextLevel(){
        if(SceneManager.GetActiveScene().name == "Level 1"){
            SceneManager.LoadScene("Level 2");
        }
        if(SceneManager.GetActiveScene().name == "Level 2"){
            SceneManager.LoadScene("Level 3");
        }
        if(SceneManager.GetActiveScene().name == "Level 3"){
            LoadVictory();
        }
    }

    public void LoadGameOver(){
        StartCoroutine(WaitAndLoad("Game Over", loadGameOverDelay));
    }

    public void LoadVictory(){
        StartCoroutine(WaitAndLoad("Victory", loadVictoryDelay));
    }

    public void QuitGame(){
        Application.Quit();
    }

    IEnumerator WaitAndLoad(string sceneName, float delay){
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }
}
