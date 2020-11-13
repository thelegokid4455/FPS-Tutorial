using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grenade : MonoBehaviour
{

    public bool impact;

    public GameObject explodeObj;

    public float maxTime;
    private float curTime;

    // Start is called before the first frame update
    void Start()
    {
        curTime = maxTime;
    }

    // Update is called once per frame
    void Update()
    {
        curTime -= Time.deltaTime * 1;

        if (!impact && curTime <= 0)
        {
            explode();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag != "Player" && impact)
        {
            explode();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag != "Player" && impact)
        {
            explode();
        }
    }

    void explode ()
    {
        GameObject dom = Instantiate(explodeObj, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
