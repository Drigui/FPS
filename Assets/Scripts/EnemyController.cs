using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy Data")]
    [SerializeField] private int currentLife;
    [SerializeField] private int maxLife;
    [SerializeField] private int enemyScorePoint;

    private NavMeshAgent agent;
    //[Header("Enemy Movement")]

    private Transform playerTransform;

    private WeaponController weaponController;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        SearchPlayer();
    }
    private void SearchPlayer() 
    {
        NavMeshHit hit;
        if (!agent.Raycast(playerTransform.position, out hit))
        {
            if (hit.distance <= 10f)
            {
                agent.SetDestination(playerTransform.position);
                agent.stoppingDistance = 2f;
                transform.LookAt(playerTransform.position);
            }
            if (hit.distance <= 7)
            {
                if (weaponController.CanShoot())
                {
                    weaponController.Shoot();
                }
            }
           
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
