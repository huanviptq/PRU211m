using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    static ScoreKeeper instance; 
    int score;

    void Awake(){
        ManageSingleton();
    }

    /// <summary>
    /// Keep score won't reset when navigate between screen.
    /// </summary>
    void ManageSingleton(){
        if(instance != null){              
            gameObject.SetActive(false);   
            Destroy(gameObject);
        }
        else{
            instance = this;
            DontDestroyOnLoad(gameObject); 
        }
    }

    /// <summary>
    /// Get current score
    /// </summary>
    /// <returns></returns>
    public int GetScore(){
        return score;
    }

    /// <summary>
    /// Modify player score
    /// </summary>
    /// <param name="value"></param>
    public void ModifyScore(int value){
        score += value;
        Mathf.Clamp(score, 0, int.MaxValue);
    }

    /// <summary>
    /// Reset player score
    /// </summary>
    public void ResetScore(){
        score = 0;
    }
}
