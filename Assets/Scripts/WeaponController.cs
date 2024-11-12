using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private Transform barrel;

    [Header("Ammo")]
    [SerializeField] private int currentAmmo;
    [SerializeField] private int maxAmmo;
    [SerializeField] private bool infiniteAmmo;

    [Header("Performance")]
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float shootRate;
    [SerializeField] private int damage;

    private ObjectPool objectPool;
    private float lastShotTime;

    private bool isPlayer;

    private void Awake()
    {
        //checks if player
        isPlayer = GetComponent<PlayerMovement>() != null ;
        

        objectPool = GetComponent<ObjectPool>();
    }

    /// <summary>
    /// check if can shoot
    /// </summary>
    public bool CanShoot()
    {
        //check shootRate
        if (Time.time - lastShotTime >= shootRate)
        {
            if (currentAmmo > 0 || infiniteAmmo)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// handle weapon shoot
    /// </summary>
    public void Shoot()
    {
        
        //update last shoot time
        lastShotTime = Time.time;

        //reduce ammo
        if (!infiniteAmmo)
        {
            currentAmmo--;
        }

        GameObject bullet = objectPool.GetGameObject();

        //locate ball at pos
        bullet.transform.position = barrel.position;
        bullet.transform.rotation = barrel.rotation;

        Debug.Log("Barrel position: " + barrel.position);

        //assign damage to bullet
        bullet.GetComponent<BulletController>().Damage = damage;

        if (isPlayer)
        {
            //create ray from camera to middle of screen
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;
            Vector3 targerPoint;

            //check if point smth & adjust direction
            if (Physics.Raycast(ray, out hit))
            {
                targerPoint = hit.point;

            }
            else
            {
                targerPoint =ray.GetPoint(5);
            }
            bullet.GetComponent<Rigidbody>().linearVelocity = (targerPoint - barrel.forward).normalized * bulletSpeed;

        }
        else
        {
            //bullet rb velocity
            bullet.GetComponent<Rigidbody>().linearVelocity = barrel.forward * bulletSpeed;
        }
        
    }
}
