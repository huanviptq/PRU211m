using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    /// <summary>
    /// Define is ai or not
    /// </summary>
    [SerializeField] bool isAI; 
    /// <summary>
    /// Current health
    /// </summary>
    [SerializeField] int health = 100;

    /// <summary>
    /// Score earned
    /// </summary>
    [SerializeField] int score = 50;

    Animator animator;
    
    /// <summary>
    /// Audio control
    /// </summary>
    AudioPlayer audioPlayer;
    
    /// <summary>
    /// Define object is vulnerable or not. If fasle player won't take damage.
    /// </summary>
    bool isVulnerable = true;

    /// <summary>
    /// Define object is dead or not
    /// </summary>
    [HideInInspector] public bool isDead = false;

    /// <summary>
    /// Define object to hold score
    /// </summary>
    ScoreKeeper scoreKeeper;
    LevelManager levelManager;

    /// <summary>
    /// Max health of object
    /// </summary>
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

    /// <summary>
    /// Control health of player when take damage. If player is vulnerable, they won't take damage.
    /// </summary>
    /// <param name="damage">Damage taken</param>
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

    /// <summary>
    /// Control health of ai when take damage.
    /// </summary>
    /// <param name="damage">Damage taken</param>
    public void EnemyTakeDamage(int damage){
        health -= damage;
        StartCoroutine(DieDelay());
        StartCoroutine(VisualIndicator(Color.red));
    }

    /// <summary>
    /// Control the color of object when taking damage
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    IEnumerator VisualIndicator(Color color){
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.15f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    /// <summary>
    /// Control is player is vunerable.
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Control after object dead. 
    /// </summary>
    /// <returns></returns>
    IEnumerator DieDelay(){
        if(health <= 0 && isAI){
            yield return new WaitForSeconds(0.15f);
            if(dropChance >= 80 && !isBoss){
                Instantiate(heart, dropPoint.transform.position, Quaternion.identity);
            }
            if(isBoss){
                Instantiate(exitPortal, exitDropPoint.transform.position, Quaternion.identity);
            }
            Destroy(transform.parent.gameObject); // Xoa game object
            scoreKeeper.ModifyScore(score);         // Tang diem
        }
        if(health <= 0 && !isAI){
            isDead = true;
            yield return new WaitForSeconds(0.15f);
            animator.SetTrigger("Die");
            GetComponent<PlayerMovement>().enabled = false; // Khong cho di chuyen
            levelManager.LoadGameOver();  // load man gameover
        }
    }

    /// <summary>
    /// Control player heal.
    /// </summary>
    /// <param name="value"></param>
    public void Heal(int value){
        if(health >= MAX_HEALTH){
            this.health = MAX_HEALTH;
        }
        else{
            health += value;
        }
    }

    /// <summary>
    /// Get object health
    /// </summary>
    /// <returns></returns>
    public int GetHealth(){
        return health;
    }
}
