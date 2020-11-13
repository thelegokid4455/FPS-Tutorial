using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class health : MonoBehaviour
{
    public int maxHealth;
    private int curHealth;

    public GameObject mainObject;
    public GameObject deadObject;

    public GameObject triggerObj;

    // Start is called before the first frame update
    void Start()
    {
        curHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(curHealth <= 0)
        {
            Die();
        }
    }

    void ApplyDamage (int dmg)
    {
        curHealth -= dmg;
    }

    void Die ()
    {
        if(triggerObj)
        triggerObj.GetComponent<EventTrigger>().doTrigger();
        Instantiate(deadObject, transform.position, transform.rotation);
        Destroy(mainObject);
    }
}
