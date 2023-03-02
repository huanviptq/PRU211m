using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] bool isAI;
    [SerializeField] int health = 100;

    void Start(){

    }

    public void TakeDamage(int damage){
        health -= damage;
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
    }
}
