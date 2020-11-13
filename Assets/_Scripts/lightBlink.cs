using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightBlink : MonoBehaviour
{
    public Light bLight;

    public float minChangeTime;
    public float maxChangeTime;
    private float curTime;

    public float minIntensity;
    public float maxIntensity;

    // Start is called before the first frame update
    void Start()
    {
        if (!bLight)
            bLight = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        curTime -= Time.deltaTime * 1;

        if (curTime <= 0)
            changeIntensity();
    }

    void changeIntensity ()
    {
        bLight.intensity = Random.Range(minIntensity, maxIntensity);

        curTime = Random.Range(minChangeTime, maxChangeTime);
    }
}
