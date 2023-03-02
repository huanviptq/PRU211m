using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol Points")]
    [SerializeField] Transform leftEdge;
    [SerializeField] Transform rightEdge;

    [Header("Enemy")]
    [SerializeField] Transform enemy;

    [Header("Movement parameters")]
    [SerializeField] float speed;
    Vector3 initScale;
    bool movingLeft;

    void Awake(){
        initScale = enemy.localScale;
    }

    void Update() {
        if(enemy != null){
            if(movingLeft){
                if(enemy.position.x >= leftEdge.position.x){
                    MoveInDirection(-1);
                }        
                else{
                    DirectionChange();
                }
            }
            else{
                if(enemy.position.x <= rightEdge.position.x){
                    MoveInDirection(1);
                }   
                else{
                    DirectionChange();
                }     
            }
        } 
    }

    void DirectionChange(){
        movingLeft = !movingLeft;
    }
    
    void MoveInDirection(int direction){
        enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * direction, initScale.y, initScale.z);
        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * direction * speed, enemy.position.y, enemy.position.z);
    }
}
