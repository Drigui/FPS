using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy Data")]
    [SerializeField] private int currentLife;
    [SerializeField] private int maxLife;
    [SerializeField] private int enemyScorePoint;

    [Header("Patrol")]
    [SerializeField] private GameObject patrolPointsContainer;
    private List<Transform> patrolPoints = new List<Transform>();
    private NavMeshAgent agent;
    private int destinationPoint;
    private bool isChasing;


    //player
    private Transform playerTransform;

    private WeaponController weaponController;



    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        weaponController = GetComponent<WeaponController>();

        //take all children of patrolpoints & add to array
        foreach (Transform child in patrolPointsContainer.transform)
        {
            patrolPoints.Add(child);
        }
        GotoNextPatrolPoint();
    }

    private void Update()
    {
        //search player with raycast
        SearchPlayer();

        //TODO choose next destination point
        if (!isChasing && !agent.pathPending && agent.remainingDistance < 3)
        {
            GotoNextPatrolPoint();
        }
    }

    /// <summary>
    /// go to next point
    /// </summary>
    private void GotoNextPatrolPoint()
    {
        if (agent.remainingDistance <= 0.5f)
        {
            //choose next point in the list
            //cycling to start point if need
            destinationPoint = (destinationPoint + 1) % patrolPoints.Count;

        }
        //restart stoping distance to 0
        agent.stoppingDistance = 0;
        //set agent to the current destination
        agent.SetDestination(patrolPoints[destinationPoint].position);

        
        

    }


    private void SearchPlayer() 
    {
        NavMeshHit hit;
        if (!agent.Raycast(playerTransform.position, out hit))
        {
            if (hit.distance <= 10f)
            {
                agent.isStopped = false;
                agent.SetDestination(playerTransform.position);
                agent.stoppingDistance = 3f;
                transform.LookAt(playerTransform.position);


                if(hit.distance < 5f)
                {
                    agent.isStopped = true;
                }
                else
                {
                    agent.isStopped = false;

                }

                if (hit.distance <= 7)
                {
                    if (weaponController.CanShoot())
                    {
                        weaponController.Shoot();
                    }
                }
            }
            
            else
            {
                agent.isStopped = false;
                isChasing = false;
            }
        }
        else
        {
            agent.isStopped = false;
            isChasing = false;
        }
        
    }

    /// <summary>
    /// enemy receive bullet
    /// </summary>
    /// <param name="quantity">Damage quantity</param>
    public void DamageEnemy(int quantity)
    {
        currentLife -= quantity;
        if (currentLife <= 0)
        {
            Destroy(gameObject);
        }
    }


}
