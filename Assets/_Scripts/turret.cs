using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turret : MonoBehaviour
{
    public Transform Spawn;

    public float maxFireRate;
    private float curFireRate;

    public float damage;
    public float force;
    public float range;
    public LayerMask layerMask;

    public GameObject UntaggedHit;

    public GameObject mFlash;

    public int curAmmo;

    public GameObject shellObj;
    public Transform shellSpawn;

    public AudioClip shootSound;

    public GameObject wepAnim;

    private Transform Player;

    public float attackRange;

    public GameObject RotObj;
    public float rotSpeed;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        curFireRate -= Time.deltaTime * 1;

        float dist = Vector3.Distance(Player.transform.position, transform.position);

        Quaternion targetRot = Quaternion.LookRotation(Player.transform.position - transform.position);
        targetRot.x = 0;
        targetRot.z = 0;

        RotObj.transform.rotation = Quaternion.Slerp(RotObj.transform.rotation, targetRot, rotSpeed * Time.deltaTime);

        if (dist <= attackRange)
        {
            Fire();
        }
    }
    void Fire()
    {
        if (curFireRate <= 0 && curAmmo > 0)
        {
                fireOneShot();
        }
        else
        {
            return;
        }
    }


    void fireOneShot()
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

            else if (hit.collider.tag == "Player")
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

        bull.GetComponent<Rigidbody>().AddForce(transform.up * Random.Range(1.0f, 2.0f) * 100);
        bull.GetComponent<Rigidbody>().AddForce(transform.right * Random.Range(1.0f, 2.0f) * 100);

       // wepAnim.GetComponent<Animation>().Stop();
        //wepAnim.GetComponent<Animation>().CrossFade("fire");

        GetComponent<AudioSource>().PlayOneShot(shootSound);
        curFireRate = maxFireRate;

        curAmmo -= 1;
    }
}
