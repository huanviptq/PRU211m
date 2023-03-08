using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    AudioPlayer audioPlayer;
    void Start(){
        audioPlayer = FindObjectOfType<AudioPlayer>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player"){
            audioPlayer.PlayCoinClip();
            Destroy(gameObject);
        }
    }
}
