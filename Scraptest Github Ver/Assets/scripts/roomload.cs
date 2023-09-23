using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roomload : MonoBehaviour
{
    public GameObject roomToLoad; //insert prefab here

    //public GameObject player;

    [SerializeField]
    private bool roomLoaded = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("aaaaaa");
        if (roomLoaded == false && collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("loaded");
            Instantiate(roomToLoad, new Vector3(27, 19, 0), Quaternion.identity);
            roomLoaded = true;
        }
        
    }
}
