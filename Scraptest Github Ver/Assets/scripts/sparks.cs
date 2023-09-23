using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sparks : MonoBehaviour
{
    public GameObject self;
    public ParticleSystem circleSparksClose;
    public ParticleSystem circleSparksFar;
    private float timeTilDelete = 10.0f;

    // Start is called before the first frame update
    private void Awake()
    {
        circleSparksClose.Play();
        circleSparksFar.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (timeTilDelete > 0)
        {
            timeTilDelete -= Time.deltaTime;
        }
        else
        {
            if (self != null)
            {
                Destroy(self);
            } 
        }
    }
}
