using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door : MonoBehaviour
{

    public GameObject doorObj;

    private bool isOpen;

    private Vector3 curPos;
    public Vector3 openPos;
    public Vector3 closePos;

    public float openSpeed;

    public AudioClip useAudio;

    public bool canOpen;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        doorObj.transform.localPosition = Vector3.MoveTowards(doorObj.transform.localPosition, curPos, openSpeed);

        if (isOpen)
        {
            curPos = openPos;
        }
        else
        {
            curPos = closePos;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if(Input.GetButtonDown("Use") && canOpen)
            {
                GetComponent<AudioSource>().PlayOneShot(useAudio);
                if (isOpen)
                    isOpen = false;
                else
                    isOpen = true;
            }
        }
    }
}
