using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawntrigger : MonoBehaviour
{
    public GameObject[] enemyToSpawn;
    public Transform spawnLoc;


    [SerializeField]
    private bool enemySpawned = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("aaaaaa");
        if (enemySpawned == false && collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("loaded");

            for (int i = 0; i < enemyToSpawn.Length; i++)
            {
                Instantiate(enemyToSpawn[i], new Vector2(spawnLoc.position.x, spawnLoc.position.y), Quaternion.identity);
            }
            

            enemySpawned = true;
        }

    }
}
