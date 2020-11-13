using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target;

    public Transform spotter;

    public float minDis; //chase
    public float attackDis; //attack
    public float rotateSpeed;

    public float damage;
    public float moveSpeed;

    public GameObject Bullet;
    public Transform spawn;

    public float fireRate;
    private float curFireRate;

    public AudioClip fireSound;

    public bool seePlayer;
    private RaycastHit hit;

    void Start()
    {
        curFireRate = fireRate;
    }

    void Update()
    {
        if (target == null)
            target = GameObject.FindGameObjectWithTag("Player").transform;



        curFireRate -= Time.deltaTime * 1;

        spotter.transform.LookAt(target);

        var targetPos = new Vector3(target.position.x, transform.position.y, target.position.z);
        var distance = Vector3.Distance(target.position, transform.position);


        transform.LookAt(targetPos);


        if (distance <= minDis && curFireRate <= 0)
        {
            if (seePlayer)
            {
                transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
            }
            else
            {

            }
        }

        if (distance <= attackDis && curFireRate <= 0)
        {
            if (seePlayer)
                Fire();
        }

        Vector3 dir = spotter.transform.TransformDirection(new Vector3(0, 0, 1));
        Vector3 pos = spotter.transform.position;

        if (Physics.Raycast(pos, dir, out hit, 10000))
        {
            if (hit.collider.tag == "Player")
            {
                seePlayer = true;
            }
            else
            {
                seePlayer = false;
            }
        }

    }

    void Fire()
    {

        var shotBullet = Instantiate(Bullet, spawn.transform.position, spawn.transform.rotation);
        //Instantiate(MFlash, spawn.transform.position, spawn.transform.rotation);

        //shotBullet.GetComponent(bullet).damage = damage;
        //Bullet.transform.Translate(Vector3.forward * Time.deltaTime * bulletSpeed);

        GetComponent<AudioSource>().PlayOneShot(fireSound);
        curFireRate = fireRate;
    }
}
