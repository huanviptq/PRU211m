using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
    Rigidbody2D slashRigid;
    [SerializeField] float slashSpeed = 20f;
    [SerializeField] int slashDamage = 20;
    PlayerMovement player;
    float horizontalSlashSpeed;

    void Start()
    {
        slashRigid = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerMovement>();
        horizontalSlashSpeed = player.transform.localScale.x * slashSpeed;
    }

    void Update()
    {
        slashRigid.velocity = new Vector2(horizontalSlashSpeed, 0f);
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Enemy"){
            other.GetComponent<Health>().EnemyTakeDamage(slashDamage);
        }
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D other) {
        Destroy(gameObject);
    }
}
