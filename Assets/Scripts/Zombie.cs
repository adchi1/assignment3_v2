using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour, IDamagable
{
    public int health,damage;
    private int maxHealth;
    private enum state{attack,chase,idle,dead};
    public float visionRange,attackRange;
    private NavMeshAgent navMeshAgent;
    private float speed;
    state currentState;
    Animator animator;
    bool isAttacking =false;
    public AnimationClip attackAnimation,deathAnimation;
    private IEnumerator routine;

    void Start(){
        maxHealth = health;
        currentState = state.idle;
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        speed = navMeshAgent.speed;
    }
    void Update()
    {
        switch(currentState){
            case state.idle:
            navMeshAgent.SetDestination(transform.position);
                animator.SetTrigger("Idle");
                if(Vector3.Distance(transform.position,PlayerMovement.Instance.transform.position)<visionRange){
                    currentState = state.chase;
                }
                break;
            case state.chase:
                navMeshAgent.SetDestination(PlayerMovement.Instance.transform.position);
                if(health<maxHealth/2){
                    navMeshAgent.speed = speed * 5;
                    animator.SetTrigger("Run");
                }else{
                    animator.SetTrigger("Walk");
                    navMeshAgent.speed= speed * 1;
                }
                if(Vector3.Distance(transform.position,PlayerMovement.Instance.transform.position)>visionRange*1.5f){
                    currentState = state.idle;
                }else if(Vector3.Distance(transform.position,PlayerMovement.Instance.transform.position)<=attackRange){
                    currentState = state.attack;
                }
                break;
            case state.attack:
                animator.SetTrigger("Attack");
                Quaternion quaternion = Quaternion.LookRotation(PlayerMovement.Instance.transform.position-transform.position);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, quaternion,1);
                if(!isAttacking){
                    isAttacking = true;
                    routine = Attack();
                    StartCoroutine(routine);
                }
                break;
            case state.dead:
                break;
        }
    }

    IEnumerator Attack(){
        navMeshAgent.SetDestination(transform.position);
        yield return new WaitForSeconds(attackAnimation.length);
        isAttacking = false;
        if(Vector3.Distance(transform.position,PlayerMovement.Instance.transform.position)<=attackRange+.1f){
            PlayerMovement.Instance.TakeDamage(damage);
        }else{
            currentState = state.chase;
        }
    }

    IEnumerator Die()
    {
        currentState = state.dead;
        navMeshAgent.SetDestination(transform.position);
        Destroy(this.GetComponent<Collider>());
        animator.SetTrigger("Die");
        yield return new WaitForSeconds(deathAnimation.length);
        Destroy(this.gameObject);
    }

    public void TakeDamage(int damage)
    {
        health-=damage;
        Debug.Log("I got hit!");
        if(health<=0){
            if(routine!=null){StopCoroutine(routine);}
            routine = Die();
            StartCoroutine(routine);
        }
    }
}