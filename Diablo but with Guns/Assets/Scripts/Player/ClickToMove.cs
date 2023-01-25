using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;


public class ClickToMove : MonoBehaviour
{
    public Transform player;

    [Header("Stats")]
    public float damageAmount;
    private float baseDamage = 20;
    public float attackDistance;
    public float attackRate;
    private float nextAttack;

    //GAMECONTROLLER FOR LEVEL UP
    public GameObject gameController;

    //NAV MESH
    private NavMeshAgent navMeshAgent;
    private Animator anim;

    //ENEMY
    private Transform targetedEnemy;
    private bool enemyClicked;
    private bool walking;

    //PLAYER ANIMATIONS
    private bool crouching = false;
    private bool crouchWalke;
    private bool standing = true;

    //INTERACTABLES
    private Transform clickedObject;
    private bool isObjectClicked;
    private bool isChestOpen; //Funktioniert für eine Chest aber nicht für mehrere

    public GameObject deathParticle;

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
        if (standing)
        {

            if (Input.GetButtonDown("Fire2") && !player.GetComponent<PlayerHealth>().isDead)
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
                    else if (hit.collider.tag == "Boss")
                    {
                        targetedEnemy = hit.transform;
                        enemyClicked = true;
                        print("Boss HIT!");
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
                        walking = true;
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
                    walking = false;
                }
                else if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance >= navMeshAgent.stoppingDistance)
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
                    else if (hit.collider.tag == "Boss")
                    {
                        targetedEnemy = hit.transform;
                        enemyClicked = true;
                        print("Boss HIT!");
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
                    crouchWalke = false;
                }
                else if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance >= navMeshAgent.stoppingDistance)
                {

                    crouchWalke = true;
                }

            }
        }
        anim.SetBool("isCrouchWalking", crouchWalke);

    }

    void MoveAndAttack()
    {
        if (targetedEnemy == null)
        {
            return;
        }

        navMeshAgent.destination = targetedEnemy.position;

        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance > attackDistance)
        {
            navMeshAgent.isStopped = false;
            walking = true;
        }
        else if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= attackDistance)
        {
            anim.SetBool("Hit1", false);
            transform.LookAt(targetedEnemy);
            Vector3 dirToAttack = targetedEnemy.transform.position - transform.position;

            if (Time.time > nextAttack)
            {
                targetedEnemy.GetComponent<Interactable>().Interact();

                nextAttack = Time.time + attackRate;
                anim.SetBool("Hit1", true);

                if (targetedEnemy.GetComponent<EnemyHealth>().currentHealth > 0 && targetedEnemy.tag == "Enemy")
                {
                    targetedEnemy.GetComponent<EnemyHealth>().TakeDamage(damageAmount);
                }

                else if (targetedEnemy.GetComponent<EnemyHealth>().currentHealth <= 0 && targetedEnemy.tag == "Enemy")
                {
                    Destroy(targetedEnemy.gameObject);
                    if (!targetedEnemy.GetComponent<EnemyBehaiviour>().isDead)
                    {
                        Instantiate(deathParticle, targetedEnemy.transform.position, targetedEnemy.transform.rotation);
                        targetedEnemy.GetComponent<EnemyBehaiviour>().isDead = true;
                    }
                    anim.SetBool("Hit1", false);

                    gameController.GetComponent<LevelUpSystem>().AddEXP();
                    LevelUp();
                }

                else if (targetedEnemy.GetComponent<EnemyHealth>().currentHealth > 0 && targetedEnemy.tag == "Boss")
                {
                    targetedEnemy.GetComponent<EnemyHealth>().TakeDamage(damageAmount);
                }

                else if (targetedEnemy.GetComponent<EnemyHealth>().currentHealth <= 0 && targetedEnemy.tag == "Boss")
                {
                    targetedEnemy.GetComponent<BossBehaiviour>().isDead = true;
                    anim.SetBool("Hit1", false);

                    gameController.GetComponent<LevelUpSystem>().AddBossEXP();
                    LevelUp();
                }
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

    void LevelUp()
    {
        if (gameController.GetComponent<LevelUpSystem>().currentLevel > 0)
        {
            damageAmount = baseDamage * gameController.GetComponent<LevelUpSystem>().currentLevel;
        }
    }
}
