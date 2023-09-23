using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class attack : NetworkBehaviour
{
    //[SerializeField] private float attackSpeed = 5.0f;
    private float horizontal = 1;
    private float attackDir = 1; // left is -1, right is 1

    [Header("Attack Settings")]  
    public float attackCountdown = 0.0f; // changing value
    static public float attackCD = 0.3f; // what countdown is set to
    //[SerializeField] private float swingmovementTime = 0f;

    [Header("prefab Settings")]
    public GameObject swingObj;
    public Rigidbody2D swingObjBody;
    public Transform playerPos;
    public GameObject sparkParticles;


    private bool IsGrounded;
    public bool playerOne;

    void Update()
    {
        //if (!IsOwner) return;
        IsGrounded = player.grounded;
        facing();
        attacking();
    }

    private void facing()
    {
        if (playerOne)
        {
            if (Input.GetKey(KeyCode.A))
            {
                horizontal = -1;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                horizontal = 1;
            }
        }
        if (!playerOne)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                horizontal = -1;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                horizontal = 1;
            }
        }
        //////////
        if (horizontal != 0)
        {
            attackDir = horizontal;
        }
        if (attackDir == 1)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        if (attackCountdown > 0)
        {
            attackCountdown -= Time.deltaTime;
        }
    }

    private void attacking()
    {
        if (playerOne)
        {
            if (Input.GetKeyDown(KeyCode.C) && attackCountdown < 0)
            {

                GameObject attackSpark = (GameObject)Instantiate(sparkParticles, new Vector2(playerPos.position.x + (attackDir * 0.5f), playerPos.position.y + 0.2f), transform.rotation);
                attackSpark.GetComponent<Transform>().localScale = new Vector2(attackDir * 0.8f, 0.4f);

                GameObject dashSwing = (GameObject)Instantiate(swingObj, new Vector2(playerPos.position.x + (attackDir * 1.2f), playerPos.position.y + 0.2f), transform.rotation);
                dashSwing.GetComponent<Transform>().localScale = new Vector2(attackDir * 0.8f, 0.4f);
                if (IsGrounded) // grounded attack
                {
                    //Instantiate(swingObj, new Vector2(playerPos.position.x + (attackDir * 1.5f), playerPos.position.y-0.25f), Quaternion.identity);
                    attackCountdown = attackCD;
                }
                else // dash attack
                {

                    dashSwing.GetComponent<Rigidbody2D>().velocity = new Vector2(transform.localScale.x * 35f * attackDir, -0.25f);
                    //swingmovementTime = attackCD;
                    attackCountdown = attackCD;
                }

            }
        }
        if (!playerOne)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && attackCountdown < 0)
            {

                GameObject attackSpark = (GameObject)Instantiate(sparkParticles, new Vector2(playerPos.position.x + (attackDir * 0.5f), playerPos.position.y + 0.2f), transform.rotation);
                attackSpark.GetComponent<Transform>().localScale = new Vector2(attackDir * 0.8f, 0.4f);

                GameObject dashSwing = (GameObject)Instantiate(swingObj, new Vector2(playerPos.position.x + (attackDir * 1.2f), playerPos.position.y + 0.2f), transform.rotation);
                dashSwing.GetComponent<Transform>().localScale = new Vector2(attackDir * 0.8f, 0.4f);
                if (IsGrounded) // grounded attack
                {
                    //Instantiate(swingObj, new Vector2(playerPos.position.x + (attackDir * 1.5f), playerPos.position.y-0.25f), Quaternion.identity);
                    attackCountdown = attackCD;
                }
                else // dash attack
                {

                    dashSwing.GetComponent<Rigidbody2D>().velocity = new Vector2(transform.localScale.x * 35f * attackDir, -0.25f);
                    //swingmovementTime = attackCD;
                    attackCountdown = attackCD;
                }

            }
        }
        
        /*
        if (swingmovementTime > 0)
        {
            dashSwing.AddForce(Vector2.up * 2 * 15, ForceMode2D.Impulse);
            swingmovementTime -= Time.deltaTime;
        }
        */
    }

}
