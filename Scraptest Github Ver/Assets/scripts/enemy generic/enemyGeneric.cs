using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyGeneric : MonoBehaviour
{
    public int health = 50;
    [SerializeField] private float iFrames = -1f;
    public LayerMask attackLayers;

    private float actionCooldowns;
    private float doActionProbability;
    private float checkTimer;
    public GameObject spikeProjRoarPrefab;
    public GameObject hurtSmokeBurst;
    public GameObject deathFragments;

    public float moveSpeed;
    private float currentSpeed;

    public Rigidbody2D rb;
    public Transform target = null;
    Vector2 moveDir;

    private int finalAngle = 180;

    public GameObject[] playerList;

    private bool targetIsPlayerOne;

    private void Awake()
    {
        actionCooldowns = 5.0f;
        //target = GetClosestPlayer();
        playerList = GameObject.FindGameObjectsWithTag("Player");
        target = GameObject.FindGameObjectWithTag("Player").transform;

    }

    void Update()
    {
        attackManager();
        Debug.Log(target);
        //target = GetClosestPlayer();
        findNearestPlayer();
        if (attacked() && iFrames < 0)
        {
            Debug.Log("hit! remaining hp: " + health);
            if (targetIsPlayerOne) player.currentEnergy++;
            if (!targetIsPlayerOne) player.currentEnergy_player2++;
            health--;
            iFrames = 0.35f;
            GameObject hurtParticles = (GameObject)Instantiate(hurtSmokeBurst, new Vector2(rb.position.x, rb.position.y), transform.rotation);
            Destroy(hurtParticles, 2.5f);
        }
        if (health <= 0)
        {
            GameObject deathfrags = (GameObject)Instantiate(deathFragments, new Vector2(rb.position.x, rb.position.y), transform.rotation);
            Destroy(deathfrags, 2.5f);
            Destroy(gameObject);
        }

        if (iFrames >= 0)
        {
            iFrames -= Time.deltaTime;
        }

        if (target)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;


            Debug.Log("angle: " + angle);
            if (angle > -90 && angle < 90)
            {
                finalAngle = 1;
            }
            else
            {
                finalAngle = -1;
            }
            gameObject.transform.localScale = new Vector2(finalAngle, 1);

            //rotation code

            moveDir = direction;
            rb.velocity = Vector2.zero;
        }


    }
    private void FixedUpdate()
    {
        if (target)
        {
            rb.velocity = new Vector2(moveDir.x, moveDir.y) * currentSpeed;
        }
    }

    private bool dist()
    {
        float minDist = 2;
        float maxDist = 10;
        float dist = Vector3.Distance(target.position, transform.position);
        if (maxDist < dist || dist < minDist)
        {
            return false; // active
        }
        return true; // player is still far
    }

    private void findNearestPlayer()
    {
        float distOne = Vector3.Distance(playerList[0].transform.position, transform.position);
        float distTwo = Vector3.Distance(playerList[1].transform.position, transform.position);
        if (distOne < distTwo) //player 1 is closer
        {
            target = playerList[0].transform;
            targetIsPlayerOne = true;
        }
        else //player 2 is closer
        {
            target = playerList[1].transform;
            targetIsPlayerOne = false;
        }

    }

    private bool attacked()
    {
        return Physics2D.OverlapArea(new Vector2(transform.position.x, transform.position.y - 0.5f),
            new Vector2(transform.position.x, transform.position.y - 0.5f), attackLayers);
    }

    private void attackManager()
    {
        if (!dist()) return;

        checkTimer -= Time.deltaTime;
        actionCooldowns -= Time.deltaTime;


    }


    private float killProjTimer = 3.0f;
    public GameObject spikeProjPrefab; // insert prefab of spike attack projectile


}


/*
public Transform patrolPoint0; 
public Transform patrolPoint1;
public float moveSpeed;
public int patrolDes;

private void Update()
{
    patrolAI();
    attackAI();

}

void attackAI()
{

}

void patrolAI()
{
    if (patrolDes == 0)
    {
        transform.position = Vector2.MoveTowards(transform.position, patrolPoint0.position, moveSpeed * Time.deltaTime);
        if (Vector2.Distance(transform.position, patrolPoint0.position) < 0.3f)
        {
            patrolDes = 1;
        }
    }

    if (patrolDes == 1)
    {
        transform.position = Vector2.MoveTowards(transform.position, patrolPoint1.position, moveSpeed * Time.deltaTime);
        if (Vector2.Distance(transform.position, patrolPoint1.position) < 0.3f)
        {
            patrolDes = 0;
        }
    }
}


[SerializeField] float moveSpeed = 1f;

Rigidbody2D selfBody;
public Transform triggerDetection;

static public bool facingR = true;
static public float turnCD = 0.3f;

void Awake()
{
    selfBody = GetComponent<Rigidbody2D>();
}

// Update is called once per frame
private void Update()
{
    if (facingR)
    {
        triggerDetection.localPosition = new Vector2(0,0);
        triggerDetection.localScale = new Vector3(-1, 1, 1);
        selfBody.velocity = new Vector2(-moveSpeed, selfBody.velocity.y); //new Vector2 (moveSpeed, 0f);
    }
    else
    {
        triggerDetection.localPosition = new Vector2(0, 0);
        triggerDetection.localScale = new Vector3(1, 1,1);
        selfBody.velocity = new Vector2(moveSpeed, selfBody.velocity.y);
    }
}

public bool facingRight()
{
    return transform.localScale.x > Mathf.Epsilon;
}
*/

