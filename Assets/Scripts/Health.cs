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
    static int MAX_HEALTH = 100;
    [SerializeField] GameObject heart;
    [SerializeField] Transform dropPoint;
    int dropChance;
    [SerializeField] bool isBoss;
    [SerializeField] GameObject exitPortal;
    [SerializeField] Transform exitDropPoint;

    void Start(){
        animator = GetComponent<Animator>();
        audioPlayer = FindObjectOfType<AudioPlayer>();
        scoreKeeper = FindAnyObjectByType<ScoreKeeper>();
        levelManager = FindObjectOfType<LevelManager>();
        dropChance = Random.Range(1, 101);
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
            if(dropChance >= 80 && !isBoss){
                Instantiate(heart, dropPoint.transform.position, Quaternion.identity);
            }
            if(isBoss){
                Instantiate(exitPortal, exitDropPoint.transform.position, Quaternion.identity);
            }
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

    public void Heal(int value){
        if(health >= MAX_HEALTH){
            this.health = MAX_HEALTH;
        }
        else{
            health += value;
        }
    }

    public int GetHealth(){
        return health;
    }
}
