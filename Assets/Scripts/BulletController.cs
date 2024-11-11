using System.Collections;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [Header("Bullet Info")]
    [SerializeField] private float activeTime;
    [Header("Particles")]
    [SerializeField] private GameObject damageParticle;
    [SerializeField] private GameObject Particle;
    [SerializeField] private GameObject impactParticle;

    private int damage;

    public int Damage { get => damage; set => damage = value; }

    public void OnEnable()
    {
        StartCoroutine(DeactiveAfterTime());
    }
    public void OnDisable()
    {
        
    }
    private IEnumerator DeactiveAfterTime()
    {
        yield return new WaitForSeconds(activeTime);
        gameObject.SetActive(false);
    }

    //when bullet collide smth
    private void OnTriggerEnter(Collider other)
    {
        gameObject.SetActive(false);

        //TODO collision enemy or player
        if (other.CompareTag("Enemy"))
        {
            // instantiate damage particle
            GameObject particles = Instantiate(damageParticle, transform.position, Quaternion.identity);
            //create damage on enemy
            other.GetComponent<EnemyController>().DamageEnemy(damage);
        }
        else if (other.CompareTag("Player"))
        {
            GameObject particles = Instantiate(damageParticle, transform.position, Quaternion.identity);
            //TODO reduce life player
        }
        else
        {

        }
    }
}
