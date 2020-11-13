using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class asu : MonoBehaviour
{

    public GameObject moveAnim;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.W))
        {
            moveAnim.GetComponent<Animation>().Play("Walk");
        }
        else if (Input.GetKey(KeyCode.S))
        {
            moveAnim.GetComponent<Animation>().Play("Back");
        }
        else
        {
            moveAnim.GetComponent<Animation>().Play("Idle");
        }
    }
}
