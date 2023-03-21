using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
    Rigidbody2D slashRigid; //Khoi tao rigid body 2D
    [SerializeField] float slashSpeed = 20f; //Toc do bay cua slash
    [SerializeField] int slashDamage = 20; //Dame moi phat chem
    PlayerMovement player; //Khoi tao player movement
    float horizontalSlashSpeed;

    void Start()
    {
        slashRigid = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerMovement>();
        horizontalSlashSpeed = player.transform.localScale.x * slashSpeed; //Khoang cach bay cua slash
    }

    void Update()
    {
        slashRigid.velocity = new Vector2(horizontalSlashSpeed, 0f); //van toc bay cua slash
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Enemy"){
            other.GetComponent<Health>().EnemyTakeDamage(slashDamage); //tru mau cua enemy
        }
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D other) {
        Destroy(gameObject);  //va cham nhau thi destroy slash
    }
}
