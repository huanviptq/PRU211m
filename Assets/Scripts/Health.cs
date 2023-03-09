using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] bool isAI;
    [SerializeField] int health = 100;
    [SerializeField] int score = 50;
    Animator animator;
    AudioPlayer audioPlayer;
    bool isVulnerable = true;
    [HideInInspector] public bool isDead = false;
    ScoreKeeper scoreKeeper;
    LevelManager levelManager;

    void Start(){
        animator = GetComponent<Animator>();
        audioPlayer = FindObjectOfType<AudioPlayer>();
        scoreKeeper = FindAnyObjectByType<ScoreKeeper>();
        levelManager = FindObjectOfType<LevelManager>();
    }

    public void PlayerTakeDamage(int damage){
        if(isVulnerable){
            health -= damage;
            isVulnerable = false;
            animator.SetTrigger("Hit");
            audioPlayer.PlayHitClip();
            StartCoroutine(DieDelay());
            StartCoroutine(Invunerable());
            StartCoroutine(VisualIndicator(Color.red));
        }
    }

    public void EnemyTakeDamage(int damage){
        health -= damage;
        StartCoroutine(DieDelay());
        StartCoroutine(VisualIndicator(Color.red));
    }

    IEnumerator VisualIndicator(Color color){
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.15f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    IEnumerator Invunerable(){
        if(!isDead){
            float blinkDelay = 0.0836f;
            for(int i = 0; i < 10; i++){
                GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
                yield return new WaitForSeconds(blinkDelay);
                GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                yield return new WaitForSeconds(blinkDelay);
            }
            isVulnerable = true;
        }
    }

    IEnumerator DieDelay(){
        if(health <= 0 && isAI){
            yield return new WaitForSeconds(0.15f);
            Destroy(transform.parent.gameObject);
            scoreKeeper.ModifyScore(score);
        }
        if(health <= 0 && !isAI){
            isDead = true;
            yield return new WaitForSeconds(0.15f);
            animator.SetTrigger("Die");
            GetComponent<PlayerMovement>().enabled = false;
            levelManager.LoadGameOver();
        }
    }

    public int GetHealth(){
        return health;
    }
}
