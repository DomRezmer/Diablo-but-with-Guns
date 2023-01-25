using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent (typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyBehaiviour : Interactable
{
    //NAV MESH AGENT
    private NavMeshAgent navMeshAgent;
    private Animator anim;
    private bool walking;

    //RADIUS
    public float lookRadius;
    public float attackRadius;

    //ATTACK
    public float attackDamage = 2;
    private float nextAttack;
    public float attackRate = 1f;
    public bool isDead;

    //TARGET
    Transform targetPlayer;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<EnemyHealth>().setCurrentHealth(100);
        anim = GetComponentInChildren<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        targetPlayer = PlayerManager.instance.ourPlayer.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {

            anim.SetBool("walking", walking);

            if (!walking)
            {
                anim.SetBool("idling", true);
            }
            else
            {
                anim.SetBool("idling", false);
            }

            float distance = Vector3.Distance(transform.position, targetPlayer.position);

            if (distance <= lookRadius)
            {
                //move to Target
                MoveAndAttack();
                //attack
            }
            else
            {
                walking = false;
            }
        }
        else if (anim.GetBool("isDead") == false)
        {
            anim.Play("Death");
         }

    }

    void MoveAndAttack()
    {
        navMeshAgent.destination = targetPlayer.position;

        if(!navMeshAgent.pathPending && navMeshAgent.remainingDistance > attackRadius)
        {
            navMeshAgent.isStopped = false;
            walking = true;
        }
        else if(!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= attackRadius)
        {
            anim.SetBool("fighting", false);
            transform.LookAt(targetPlayer);
            Debug.Log("Player Seen!!");

            if(Time.time > nextAttack)
            {
                nextAttack = Time.time + attackRate;

                anim.SetBool("fighting", true);
                targetPlayer.GetComponent<PlayerHealth>().TakeDamage(attackDamage);

                if (targetPlayer.GetComponent<PlayerHealth>().isDead)
                {
                    anim.SetBool("fighting", false);
                }
                
            }

            navMeshAgent.isStopped = true;
            walking = false;
        }
                    
    }
     void OnDrawGizmos()
    {
        Handles.color = Color.yellow;
        //Draw the lookRadius of the enemy
        Handles.DrawWireArc(transform.position+ new Vector3(0, 0.2f, 0), transform.up, transform.right, 360, lookRadius);

        Handles.color = Color.red;
        //Draw the attackRadius of the enemy
        Handles.DrawWireArc(transform.position + new Vector3(0, 0.2f, 0), transform.up, transform.right, 360, attackRadius);
    }

    public override void Interact()
    {
        Debug.Log("Enemy got damaged!");
        
        base.Interact();
    }
}
