using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunManager : MonoBehaviour
{

    public GameObject curWeapon;

    public bool selected;

    public Transform kickGO;
    public float kickUp = 0.5f;
    public float kickSideways = 0.5f;

    public GameObject knifeObj;
    public Transform Spawn;
    public GameObject slashEffect;
    public float damage;
    public float range;
    public float force;
    public LayerMask layerMask;

    public float maxKnifeTime;
    private float knifeTime;

    private bool knifing;

    public AudioClip knifeSound;

    public GameObject grenadeObj;
    public GameObject grenObj;

    public float throwForce;
    private bool grenade;

    public float maxGrenTime;
    private float grenTime;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        knifeTime -= Time.deltaTime * 1;
        grenTime -= Time.deltaTime * 1;

        if (knifeTime <= 0)
        {
            knifeObj.SetActive(false);
            knifing = false;
            
        }
        else
        {
            knifeObj.SetActive(true);
        }
        

        if (grenTime <= 0)
        {
            grenadeObj.SetActive(false);
            grenade = false;

        }
        else
        {
            grenadeObj.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.V) && !knifing && !grenade)
        {
            Knife();
        }

        if (Input.GetKeyDown(KeyCode.G) && !knifing && !grenade)
        {
            Throw();
        }
    }

    void Throw ()
    {
        GameObject gr = Instantiate(grenObj, Spawn.transform.position, Spawn.transform.rotation);
        gr.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0,0, throwForce));

        StartCoroutine(grenTimer());

        grenTime = maxGrenTime;
        grenadeObj.transform.GetChild(0).GetComponent<Animation>().Stop();
        grenadeObj.transform.GetChild(0).GetComponent<Animation>().Play();

        kickGO.localRotation = Quaternion.Euler(kickGO.localRotation.eulerAngles - new Vector3(Random.Range(-(kickUp), (kickUp)), Random.Range(-(kickSideways), (kickSideways)), 0));
        GetComponent<AudioSource>().PlayOneShot(knifeSound);
        grenade = true;
    }

    void Knife()
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
                GameObject def = Instantiate(slashEffect, contact, rot);
                def.transform.localPosition += .02f * hit.normal;
                def.transform.localScale = new Vector3(rScale, rScale, rScale);
                def.transform.parent = hit.transform;
                hit.collider.SendMessageUpwards("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);
            }
        }

        
        StartCoroutine(knifeTimer());

        knifeTime = maxKnifeTime;
        knifeObj.transform.GetChild(0).GetComponent<Animation>().Stop();
        knifeObj.transform.GetChild(0).GetComponent<Animation>().Play();

        kickGO.localRotation = Quaternion.Euler(kickGO.localRotation.eulerAngles - new Vector3(Random.Range(-(kickUp), (kickUp)), Random.Range(-(kickSideways), (kickSideways)), 0));
        GetComponent<AudioSource>().PlayOneShot(knifeSound);
        knifing = true;
    }

    IEnumerator knifeTimer()
    {
        curWeapon.GetComponent<gun>().selected = false;
        yield return new WaitForSeconds(maxKnifeTime);
        curWeapon.GetComponent<gun>().selected = true;
        curWeapon.GetComponent<gun>().Draw();
    }

    IEnumerator grenTimer()
    {
        curWeapon.GetComponent<gun>().selected = false;
        yield return new WaitForSeconds(maxGrenTime);
        curWeapon.GetComponent<gun>().selected = true;
        curWeapon.GetComponent<gun>().Draw();
    }


}
