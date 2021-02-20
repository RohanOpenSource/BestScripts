using EZCameraShake;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{

    public Rigidbody player;
    private Animator anim;
    public Camera cam;
    public float damage;
    public float firerate=15f;
    public float kb = 100f;
    public float recoil = 100f;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    private float nexttimetofire = 0f;
    public float shotsBeforeReload;
    public float shots;
    private bool isReloading = false;
    public AudioSource shootsound;
    public Image crosshair;
    // Update is called once per frame
    private void Start()
    {
        anim=gameObject.GetComponent<Animator>();
    }
    void Update()
    {
        if (shots >= shotsBeforeReload || (Input.GetKeyDown("r")&&shots>=1))
        {
            reload();
        }
        if (Input.GetMouseButton(0)&& Time.time>=nexttimetofire && !isReloading)
        {
            shootsound.Play();
            shots += 1;
            cam.GetComponent<CameraShaker>().ShakeOnce(10f, 3f,0.1f,0.3f);
            nexttimetofire = Time.time + 1f / firerate;
            muzzleFlash.Play();
            player.AddRelativeForce(-cam.transform.forward * recoil);



            //anim.SetTrigger("shoot");
            RaycastHit hit;

            if (Physics.Raycast(cam.transform.position,cam.transform.forward, out hit, 100f)){

                GameObject e=Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(e, 0.5f);
                e.GetComponent<ParticleSystem>().Play();
                Target target = hit.transform.GetComponent<Target>();
                
                if (target != null)
                {
                    StartCoroutine(chair());
                    target.TakeDamage(damage);
                }
                if (hit.rigidbody!=null)
                {
                    hit.rigidbody.AddForce(-hit.normal*kb);
                }

            }
        }

    }
    void reload()
    {
        shots = 0;
        isReloading = true;
        anim.SetTrigger("Reload");
        StartCoroutine(Reloader());
    }

    private IEnumerator Reloader()
    {
        yield return new WaitForSeconds(2f);
        isReloading = false;
    }
    private IEnumerator chair()
    {
        yield return new WaitForSeconds(0.2f);
    }
}
