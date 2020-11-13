using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTrigger : MonoBehaviour
{   

    //START
    public bool TriggerEnter;
    public bool TriggerStay;

    //Do
    public bool destroyObject;
    public bool openDoor;
    public bool activateObject;

    //OBJECT
    public GameObject mainObject;
    public GameObject thisObject;

    //END
    public bool destroyThis;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void doTrigger()
    {

        if(openDoor)
        {
            mainObject.GetComponent<door>().canOpen = true;
        }

        if(activateObject)
        {
            if(mainObject.activeSelf == false)
                mainObject.SetActive(true);
            else
                mainObject.SetActive(false);
        }

        if(destroyThis)
        {
            Destroy(thisObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (TriggerEnter)
                doTrigger();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (TriggerStay)
                doTrigger();
        }
    }
}
