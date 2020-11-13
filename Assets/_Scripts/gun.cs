using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gun : MonoBehaviour
{
    public Transform Spawn;

    public float range;
    public float damage;
    public float force;
    public float kickMultiplier;
    public LayerMask layerMask;

    private float inAccuracy;

    public GameObject UntaggedHit;

    public int maxAmmo;
    private int curAmmo;
    public int mag;

    private bool isReloading;
    public float maxReloadTime;
    private float reloadTime;

    public float maxFireRate;
    private float curFireRate;

    public float normAcc;
    public float aimAcc;

    private GameObject mainCam;
    public float aimFOV;
    public float normFOV;

    public float aimSpeed;
    private Vector3 curPos;
    public Vector3 normPos;
    public Vector3 aimPos;

    public bool isAiming;

    public GameObject smokeEmit;
    public GameObject mFlash;

    public GameObject shellObj;
    public Transform shellSpawn;

    public GameObject dropMag;

    public GameObject wepAnim;

    public AudioClip shootSound;
    public AudioClip reloadSound;

    private GameObject Player;

    //Kick
    public Transform kickGO;
    public Transform kickWep;
    public float kickUp = 0.5f;
    public float kickSideways = 0.5f;

    public bool selected;
    public bool equipped;

    public GameObject gunMesh;
    public GameObject gunFPS;

    public GameObject wepPos;

    private bool drawing;

    public float maxDrawTime;
    private float drawTime;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        curAmmo = maxAmmo;
        mainCam = GameObject.FindGameObjectWithTag("MainCamera");
        wepPos = GameObject.FindGameObjectWithTag("WepPos");
    }

    public void Draw ()
    {
        selected = true;
        drawTime = maxDrawTime;
        wepAnim.GetComponent<Animation>().Stop();
        wepAnim.GetComponent<Animation>().CrossFade("draw");
    }

    // Update is called once per frame
    void Update()
    {
        if (equipped)
        {
            if (selected)
            {
                curFireRate -= Time.deltaTime * 1;
                reloadTime -= Time.deltaTime * 1;
                drawTime -= Time.deltaTime * 1;

                if (drawTime > 0)
                    drawing = true;
                else
                    drawing = false;

                if (reloadTime <= 0)
                    isReloading = false;
                else
                {
                    DeAim();
                }

                transform.localPosition = Vector3.MoveTowards(transform.localPosition, curPos, aimSpeed);

                if (Input.GetButton("Fire2") && !isReloading && !drawing)
                {
                    Aim();
                }
                else
                {
                    DeAim();
                }

                if (Input.GetButtonDown("Fire1") && !drawing)
                {
                    Fire();
                }

                if (Input.GetKeyDown(KeyCode.R) && !isReloading && mag > 0 && !drawing)
                {
                    if (curAmmo < maxAmmo)
                        Reload();
                    else
                        return;
                }

                gunFPS.SetActive(true);
                gunMesh.SetActive(false);

                GetComponent<Collider>().enabled = false;
                GetComponent<Rigidbody>().isKinematic = true;

                if (!isAiming)
                {
                    transform.localPosition = Vector3.zero;
                    transform.localRotation = new Quaternion(0, 0, 0, 0);
                }

                transform.parent = wepPos.transform;
            }
            else
            {
                gunFPS.SetActive(false);
            }
        }
        else
        {
            gunFPS.SetActive(false);
            gunMesh.SetActive(true);

            GetComponent<Collider>().enabled = true;
            GetComponent<Rigidbody>().isKinematic = false;

            transform.parent = null;
        }
    }

    void Aim ()
    {
        mainCam.GetComponent<Camera>().fieldOfView = aimFOV;
        curPos = aimPos;
        inAccuracy = aimAcc;
        isAiming = true;
    }

    void DeAim ()
    {
        mainCam.GetComponent<Camera>().fieldOfView = normFOV;
        curPos = normPos;
        inAccuracy = normAcc;
        isAiming = false;
    }

    private void LateUpdate()
    {
        
    }

    void Fire()
    {
        if (curFireRate <= 0 && curAmmo > 0)
        { 
            if(reloadTime <= 0)
                fireOneShot();
        }
        else
        {
            return;
        }
    }

    void fireOneShot ()
    {
        RaycastHit hit;

        Vector3 dir = Spawn.transform.TransformDirection(new Vector3(0, 0, 1));
        Vector3 pos = Spawn.transform.position;

        if (Physics.Raycast(pos, dir, out hit, range, layerMask))
        {
            Vector3 contact = hit.point;
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, hit.normal);
            float rScale = Random.Range(0.5f, 1.0f);

            if (hit.rigidbody)
                hit.rigidbody.AddForceAtPosition(force * dir, hit.point);

            if (hit.collider.tag == "Untagged")
            {
                GameObject def = Instantiate(UntaggedHit, contact, rot);
                def.transform.localPosition += .02f * hit.normal;
                def.transform.localScale = new Vector3(rScale, rScale, rScale);
                def.transform.parent = hit.transform;
                hit.collider.SendMessageUpwards("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);
            }
        }


        //EFFECTS
        Instantiate(mFlash, Spawn.position, Spawn.rotation);

        GameObject bull = Instantiate(shellObj, shellSpawn.position, shellSpawn.rotation);
        Physics.IgnoreCollision(bull.GetComponent<Collider>(), Player.GetComponent<Collider>());

        bull.GetComponent<Rigidbody>().AddForce(transform.up * Random.Range(1.0f, 2.0f) * 100);
        bull.GetComponent<Rigidbody>().AddForce(transform.right * Random.Range(1.0f, 2.0f) * 100);

        wepAnim.GetComponent<Animation>().Stop();
        wepAnim.GetComponent<Animation>().CrossFade("fire");

        GetComponent<AudioSource>().PlayOneShot(shootSound);
        curFireRate = maxFireRate;

        smokeEmit.GetComponent<ParticleSystem>().Play();

        kickWep.localRotation = Quaternion.Euler(kickWep.localRotation.eulerAngles - new Vector3((kickUp / 2 * inAccuracy), Random.Range(-(kickSideways / 2 * inAccuracy), (kickSideways / 2 * inAccuracy)), 0));
        kickGO.localRotation = Quaternion.Euler(kickGO.localRotation.eulerAngles - new Vector3(Random.Range(-(kickUp * kickMultiplier), (kickUp * kickMultiplier)), Random.Range(-(kickSideways), (kickSideways)), 0));

        curAmmo -= 1;
    }

    void Reload ()
    {
        isReloading = true;

        reloadTime = maxReloadTime;

        Instantiate(dropMag, Spawn.position, Spawn.rotation);

        GetComponent<AudioSource>().PlayOneShot(reloadSound);

        wepAnim.GetComponent<Animation>().Stop();
        wepAnim.GetComponent<Animation>().CrossFade("reload");

        curAmmo = maxAmmo;
        mag -= 1;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            equipped = true;
            Draw();
        }
    }
}
