using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;
 

public class ClickToMove : MonoBehaviour
{
    [Header("Stats")]
    public float attackDistance;
    public float attackRate;
    private float nextAttack;

    //NAV MESH
    private NavMeshAgent navMeshAgent;
    private Animator anim;

    //ENEMY
    private Transform targetedEnemy;
    private bool enemyClicked;
    private bool walking;
    private bool crouching = false;
    private bool crouchWalke;
    private bool standing = true; 

    //INTERACTABLES
    private Transform clickedObject;
    private bool isObjectClicked;
    private bool isChestOpen; //Funktioniert für eine Chest aber nicht für mehrere

    //DOUBLE CLICK
    private bool oneClick;
    private bool doubleClick;
    private float timerForDoubleClick;
    private float delay = 0.25f;

    void Awake()
    {
        anim = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (standing) {

        
        CheckDoubleClick();

        if (Input.GetButtonDown("Fire2"))
        {
            navMeshAgent.ResetPath();
            if (Physics.Raycast(ray, out hit, 1000))
            {
                if(hit.collider.tag == "Enemy")
                {
                    targetedEnemy = hit.transform;
                    enemyClicked = true;
                    print("Enemy HIT!");
                }
                else if(hit.collider.tag == "Chest")
                {
                    isObjectClicked = true;
                    clickedObject = hit.transform;
                }
                else if (hit.collider.tag == "Info")
                {
                    isObjectClicked = true;
                    clickedObject = hit.transform;
                }
                else
                {
                    walking = true;
                    enemyClicked = false;
                    navMeshAgent.isStopped = false;
                    navMeshAgent.destination = hit.point;
                }
            }
        }

        if(enemyClicked)
        {
            MoveAndAttack();
        }
        else if (enemyClicked) //Change this to if Left click
        {
            //Select enemy
        }
        else if(isObjectClicked && clickedObject.gameObject.tag == "Info")
        {
            ReadInfos(clickedObject);
        }
        else if (isObjectClicked && clickedObject.gameObject.tag == "Chest" && !isChestOpen)
        {
            OpenChest(clickedObject);
        }
        else
        {
            if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                walking = false;
            }
            else if(!navMeshAgent.pathPending && navMeshAgent.remainingDistance >= navMeshAgent.stoppingDistance)
            {
                walking = true;
            }

        }

        anim.SetBool("isWalking", walking);
    }
        
        if (Input.GetKeyDown("c"))
        {
            standing = !standing;
            crouching = !crouching;
            anim.SetBool("isCrouching", crouching);
        }

        if (crouching && !standing)
        {
            if (Input.GetButtonDown("Fire2"))
            {
                navMeshAgent.ResetPath();
                if (Physics.Raycast(ray, out hit, 1000))
                {
                    if (hit.collider.tag == "Enemy")
                    {
                        targetedEnemy = hit.transform;
                        enemyClicked = true;
                        print("Enemy HIT!");
                    }
                    else if (hit.collider.tag == "Chest")
                    {
                        isObjectClicked = true;
                        clickedObject = hit.transform;
                    }
                    else if (hit.collider.tag == "Info")
                    {
                        isObjectClicked = true;
                        clickedObject = hit.transform;
                    }
                    else
                    {
                        crouchWalke = true;
                        enemyClicked = false;
                        navMeshAgent.isStopped = false;
                        navMeshAgent.destination = hit.point;
                    }
                }
            }


            if (enemyClicked)
            {
                MoveAndAttack();
            }
            else if (enemyClicked) //Change this to if Left click
            {
                //Select enemy
            }
            else if (isObjectClicked && clickedObject.gameObject.tag == "Info")
            {
                ReadInfos(clickedObject);
            }
            else if (isObjectClicked && clickedObject.gameObject.tag == "Chest" && !isChestOpen)
            {
                OpenChest(clickedObject);
            }
            else
            {
                if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
                {
                    crouchWalke  = false;
                }
                else if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance >= navMeshAgent.stoppingDistance)
                {
                    
                    crouchWalke= true;
                }

            }
        }
        anim.SetBool("isCrouchWalking", crouchWalke);
    
    
    
    }


    void MoveAndAttack()
    {
        if(targetedEnemy == null)
        {
            return;
        }

        navMeshAgent.destination = targetedEnemy.position;

        if(!navMeshAgent.pathPending && navMeshAgent.remainingDistance > attackDistance)
        {
            navMeshAgent.isStopped = false;
            walking = true;
        }
        else if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= attackDistance)
        {
            anim.SetBool("isAttacking", false);
            transform.LookAt(targetedEnemy);
            Vector3 dirToAttack = targetedEnemy.transform.position - transform.position;

            if(Time.time > nextAttack)
            {
                targetedEnemy.GetComponent<Interactable>().Interact();

                nextAttack = Time.time + attackRate;
                anim.SetBool("isAttacking", true);
            }

            navMeshAgent.isStopped = true;
            walking = false;
        }
    }

    void ReadInfos(Transform target)
    {
        //set target
        navMeshAgent.destination = target.position;
        //go close
        if(!navMeshAgent.pathPending && navMeshAgent.remainingDistance > attackDistance)
        {
            navMeshAgent.isStopped = false;
            walking = true;
        }
        //then read
        else if(!navMeshAgent.pathPending && navMeshAgent.remainingDistance < attackDistance)
        {
            navMeshAgent.isStopped = true;
            transform.LookAt(target);
            walking = false;

            //print Info
            print(target.GetComponent<Infos>().info);

            isObjectClicked = false;
            navMeshAgent.ResetPath();
        }
        
    }

    void OpenChest(Transform target)
    {
        //set target
        navMeshAgent.destination = target.position;
        //go close
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance > attackDistance)
        {
            navMeshAgent.isStopped = false;
            walking = true;
        }
        //then read
        else if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < attackDistance)
        {
            navMeshAgent.isStopped = true;
            transform.LookAt(target);
            walking = false;

            //play Animation
            target.gameObject.GetComponentInChildren<Animator>().SetTrigger("Play");
            isChestOpen = true;

            isObjectClicked = false;
            navMeshAgent.ResetPath();
        }

    }

    void CheckDoubleClick()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            if (!oneClick)
            {
                oneClick = true;
                timerForDoubleClick = Time.time;
            }
            else
            {
                oneClick = false;
                doubleClick = true;
            }
        }

        if (oneClick)
        {
            if((Time.time - timerForDoubleClick) > delay)
            {
                oneClick = false;
                doubleClick = false;
            }
        }
    }
}
