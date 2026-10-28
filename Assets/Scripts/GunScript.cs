using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public int damage;
    public float range = 100f;
    public int ammo;
    private int ammoCapacity;
    bool canShoot = true;
    public float fireCooldown,reloadCooldown;
    public ParticleSystem gunFlash,gunHit;

    void Start(){
        ammoCapacity = ammo;
    }
    void Update()
    {
        if(Input.GetMouseButton(0) && canShoot){
            StartCoroutine(Shoot());
        }
    }

    IEnumerator Shoot(){
        ammo-=1;
        RaycastHit hit;
        Camera cam = PlayerMovement.Instance.playerCamera;
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out
        hit,range)){
        IDamagable target = hit.transform.GetComponent<IDamagable>();
        Debug.Log(target.ToString());
        if(target != null){target.TakeDamage(damage);}
        ParticleSystem particle = Instantiate(gunHit,
        hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(particle.gameObject,2);}
        gunFlash.Play();
        canShoot = false;
        yield return new WaitForSeconds(fireCooldown);
        if(ammo<=0){StartCoroutine(Reload());
        }else{canShoot=true;}
    }

    IEnumerator Reload(){
        yield return new WaitForSeconds(reloadCooldown);
        ammo = ammoCapacity;
        canShoot = true;
    }
 /*
    void OnCollisionEnter(Collision collision)
    {
        if (collision.GetType() == Zombie)
        {
            
        }
    }
    */
}
