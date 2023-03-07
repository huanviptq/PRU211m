using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float attackCooldown;
    [SerializeField] float range;
    [SerializeField] float colliderDistance;
    [SerializeField] int damage;
    [SerializeField] BoxCollider2D boxCollider;
    float cooldownTimer = Mathf.Infinity;
    AudioPlayer audioPlayer;
    Animator animator;
    EnemyPatrol enemyPatrol;
    Health playerHealth;

    void Awake()
    {
        animator = GetComponent<Animator>();
        audioPlayer = FindObjectOfType<AudioPlayer>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
    }

    void Update()
    {
        cooldownTimer += Time.deltaTime;
        if(PlayerInSight() && !playerHealth.isDead){
            if(cooldownTimer >= attackCooldown){
                cooldownTimer = 0;
                animator.SetTrigger("Attack");
                audioPlayer.PlayAttackClip();
            }
        }

        if(enemyPatrol != null){
            enemyPatrol.enabled = !PlayerInSight();
        }
    }

    bool PlayerInSight(){
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
        new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z)
        , 0, Vector2.left, 0, LayerMask.GetMask("Player"));
        if(hit.collider != null){
            playerHealth = hit.transform.GetComponent<Health>();
        }
        return hit.collider != null;
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, 
        new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    void DamagePlayer(){
        if(PlayerInSight()){
            playerHealth.PlayerTakeDamage(damage);
        }
    }
}
