using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExit : MonoBehaviour
{
    LevelManager levelManager;

    void Start(){
        levelManager = FindObjectOfType<LevelManager>();
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Player"){
            levelManager.LoadNextLevel();
        }
    }
}
