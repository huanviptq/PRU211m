using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] bool isAI;
    [SerializeField] int health = 100;
    Animator animator;
    void Start(){
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage){
        health -= damage;
        if(health > 0 && !isAI){
            animator.SetTrigger("Hit");
        }
        StartCoroutine(VisualIndicator(Color.red));
        StartCoroutine(DieDelay());
    }

    IEnumerator VisualIndicator(Color color){
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.15f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    IEnumerator DieDelay(){
        yield return new WaitForSeconds(0.15f);
        if(health <= 0 && isAI){
            Destroy(transform.parent.gameObject);
        }
        if(health <= 0 && !isAI){
            animator.SetTrigger("Die");
            GetComponent<PlayerMovement>().enabled = false;
        }
    }

    public int GetHealth(){
        return health;
    }
}
