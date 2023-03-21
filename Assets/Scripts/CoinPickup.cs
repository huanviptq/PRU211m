using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    AudioPlayer audioPlayer; //Khoi tao audio
    ScoreKeeper scoreKeeper; //Khoi tao score
    [SerializeField] int score = 50; //Default an 1 dong coin 50 score
    bool wasCollected = false;
    void Start(){
        audioPlayer = FindObjectOfType<AudioPlayer>(); //Gan audio hien tai
        scoreKeeper = FindObjectOfType<ScoreKeeper>(); //Gan score hien tai
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player" && !wasCollected){ // check neu collider la player
            wasCollected = true; 
            audioPlayer.PlayCoinClip();
            gameObject.SetActive(false);
            Destroy(gameObject);      //destroy coin was collected
            scoreKeeper.ModifyScore(score); //tang diem
        }
    }
}
