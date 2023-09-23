using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;


public class playerBundle : NetworkBehaviour
{
    public GameObject player;
    public GameObject self;
    public GameObject cam;

    void Awake()
    {
        Instantiate(player);
        //Instantiate(cam);
        Destroy(self);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
