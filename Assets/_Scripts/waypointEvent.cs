using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waypointEvent : MonoBehaviour
{

    public GameObject curWaypoint;
    public GameObject nextWaypoint;

    public bool distance;
    public float minDist;

    private Transform Player;

    public GameObject TextObj;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        nextWaypoint.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        var dist = Vector3.Distance(Player.transform.position, gameObject.transform.position);
        if (TextObj)
        {
            TextObj.transform.parent.LookAt(Player);
            TextObj.GetComponent<TextMesh>().text = "Waypoint\n" + dist.ToString("F2") + " Meters";
        }
        if (distance)
        {
            if (dist<= minDist)
            {
                doNext();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (!distance)
            {
                doNext();
            }
        }
       
    }

    void doNext()
    {
        curWaypoint.SetActive(false);
        if(nextWaypoint)
            nextWaypoint.SetActive(true);
        Destroy(gameObject);
    }
}
