using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bruiserSpawn : MonoBehaviour
{

    public GameObject bruiserObj;
    public GameObject extraSpawnPoints;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("aaaaaa");
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("loaded bruiser");
            bruiserObj.SetActive(true);
            extraSpawnPoints.SetActive(true);
        }

    }
}
