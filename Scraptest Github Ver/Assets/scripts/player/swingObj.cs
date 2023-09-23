using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class swingObj : NetworkBehaviour
{
    public GameObject self;
    public float swingLifetime = 1.0f;
    

    private void Awake()
    {
        
    }

    private void Update()
    {
        //if (!IsOwner) return;

        swingLifetime -= Time.deltaTime;
        if (swingLifetime < 0)
        {
            Destroy(self);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("aaaaaa");
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("hit");
        }
        
    }
}
