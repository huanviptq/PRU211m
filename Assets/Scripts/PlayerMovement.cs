using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    /// <summary>
    /// run speed of the player
    /// </summary>
    [SerializeField] float runSpeed = 5f;
    /// <summary>
    /// jump power of the player
    /// </summary>
    [SerializeField] float jumpPower = 5f;
    /// <summary>
    /// collider to check if the player is on the ground
    /// </summary>
    [SerializeField] Transform groundCheckCollider;
    /// <summary>
    /// point to check if the player is in range of an enemy
    /// </summary>
    [SerializeField] Transform attackPoint;
    /// <summary>
    /// range of the attack (how far the player can attack)
    /// </summary>
    [SerializeField] float attackRange = 0.5f;
    /// <summary>
    /// rate of the attack (how many times per second)
    /// </summary>
    [SerializeField] float attackRate = 2f;
    /// <summary>
    /// damage of the attack (how much damage the player does)
    /// </summary>
    [SerializeField] int attackDamage = 50;
    /// <summary>
    /// damage of hazard to player
    /// </summary>
    [SerializeField] int hazardDamage = 20;
    /// <summary>
    /// slash prefab to spawn
    /// </summary>
    [SerializeField] GameObject slash;
    /// <summary>
    /// slash point to spawn slash prefab
    /// </summary>
    [SerializeField] Transform slashPoint;
    /// <summary>
    /// time until the next attack
    /// </summary>
    float nextAttackTime = 0f;
    /// <summary>
    /// audio player
    /// </summary>
    AudioPlayer audioPlayer;
    /// <summary>
    /// radius of the ground check collider
    /// </summary>
    const float groundCheckRadius = 0.2f;
    /// <summary>
    /// input of the player
    /// </summary>
    Vector2 moveInput;
    /// <summary>
    /// rigidbody of the player
    /// </summary>
    Rigidbody2D rb;
    /// <summary>
    /// animator of the player
    /// </summary>
    Animator animator;
    /// <summary>
    /// capsule collider of the player
    /// </summary>
    CapsuleCollider2D capsule;
    /// <summary>
    /// box collider of the player
    /// </summary>
    BoxCollider2D box;
    /// <summary>
    /// if the player is on the ground
    /// </summary>
    bool isGrounded = false;

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsule = GetComponent<CapsuleCollider2D>();
        box = GetComponent<BoxCollider2D>();
        audioPlayer = FindObjectOfType<AudioPlayer>();
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        Run();
        FlipSprite();
        animator.SetFloat("yVelocity", rb.velocity.y); 
        GroundCheck();
        DieByHazard();
    }

    /// <summary>
    /// Move the player
    /// </summary>
    /// <param name="value"></param>
    void OnMove(InputValue value)
    {
        if(!PauseMenu.isPaused){
            moveInput = value.Get<Vector2>();
        }
    }

    /// <summary>
    ///  Jump the player
    /// </summary>
    /// <param name="value"></param>
    void OnJump(InputValue value){
        if(!box.IsTouchingLayers(LayerMask.GetMask("Ground"))){
            return;
        }
        if(value.isPressed && !PauseMenu.isPaused){
            rb.velocity += new Vector2(0f, jumpPower);
            animator.SetBool("isJumping", true);
            audioPlayer.PlayJumpClip();
        }
    }

    /// <summary>
    /// Attack the player
    /// </summary>
    void OnAttack(){
        if(Time.time >= nextAttackTime && !PauseMenu.isPaused){
            animator.SetTrigger("Attack");
            StartCoroutine(SlashDelay());
            audioPlayer.PlayAttackClip();
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, LayerMask.GetMask("Enemy"));
            foreach(Collider2D enemy in hitEnemies){
                enemy.GetComponent<Health>().EnemyTakeDamage(attackDamage);
            }
            nextAttackTime = Time.time + 1f / attackRate;
        }
    }

    /// <summary>
    /// OnDrawGizmosSelected
    /// </summary>
    void OnDrawGizmosSelected() {
        if(attackPoint == null){
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    /// <summary>
    /// 
    /// </summary>
    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, rb.velocity.y);
        rb.velocity = playerVelocity;
        bool hasHorizontalSpeed = Mathf.Abs(rb.velocity.x) > 0;
        animator.SetBool("isRunning", hasHorizontalSpeed);
    }

    /// <summary>
    /// Flip the sprite
    /// </summary>
    void FlipSprite(){
            bool hasHorizontalSpeed = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;
            if(hasHorizontalSpeed){
                transform.localScale = new Vector2(Mathf.Sign(rb.velocity.x), 1f);
                slash.transform.localScale = new Vector2(Mathf.Sign(rb.velocity.x), 1f);
            }
    }

    /// <summary>
    /// Check if the player is on the ground
    /// </summary>
    void GroundCheck(){
        isGrounded = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckCollider.position, groundCheckRadius, LayerMask.GetMask("Ground"));
        if(colliders.Length > 0){
            isGrounded = true;
        }
        animator.SetBool("isJumping", !isGrounded);
    }
    /// <summary>
    /// Check if the player is touching a hazard and die
    /// </summary>
    void DieByHazard(){
        if(capsule.IsTouchingLayers(LayerMask.GetMask("Hazards"))){
            capsule.GetComponent<Health>().PlayerTakeDamage(hazardDamage);
        }
    }

    /// <summary>
    ///  Play splash sound when player touches water
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter2D(Collider2D other) {
        if(capsule.IsTouchingLayers(LayerMask.GetMask("Water"))){
            audioPlayer.PlaySplashClip();
        }
    }

    /// <summary>
    /// Delay for the slash
    /// </summary>
    /// <returns></returns>
    IEnumerator SlashDelay(){
        yield return new WaitForSeconds(0.15f);
        Instantiate(slash, slashPoint.position, Quaternion.identity);
    }
}