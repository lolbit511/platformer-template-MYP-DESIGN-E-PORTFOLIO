using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyTrigger : MonoBehaviour
{
    Rigidbody2D trigger;

    /*
    private float turncountdown = 0.3f;

    private void Update()
    {
        turncountdown -= Time.deltaTime;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log(turncountdown);

        if (turncountdown < 0 && !collision.gameObject.CompareTag("Enemy"))
        {
            enemyGeneric.facingR = !enemyGeneric.facingR;
            turncountdown = enemyGeneric.turnCD;
        }
        
    }
    */
}
