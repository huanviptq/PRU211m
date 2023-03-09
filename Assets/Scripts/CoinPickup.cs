using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    AudioPlayer audioPlayer;
    ScoreKeeper scoreKeeper;
    [SerializeField] int score = 50;
    bool wasCollected = false;
    void Start(){
        audioPlayer = FindObjectOfType<AudioPlayer>();
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player" && !wasCollected){
            wasCollected = true;
            audioPlayer.PlayCoinClip();
            gameObject.SetActive(false);
            Destroy(gameObject);
            scoreKeeper.ModifyScore(score);
        }
    }
}
