using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flyingSentry : MonoBehaviour
{
    public int health = 5;
    [SerializeField] private float iFrames = -1f;
    public LayerMask attackLayers;

    private float actionCooldowns;
    private float doActionProbability;
    private float checkTimer;
    [SerializeField] private float spikeattackDelay = 1f;
    [SerializeField] private bool spikeShotCharging = false;
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
            if (angle > -90  && angle < 90)
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

            if (!dist() || spikeShotCharging)
            {
                currentSpeed = 0f;
            }
            else
            {
                
                currentSpeed = moveSpeed;
            }
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
        if (attacked()) // minus cd by 5.5 sec every hit taken
        {
            actionCooldowns -= 5.5f;
        }

        //every ti0.5s randomise a value for actionCD
        if (actionCooldowns < 0 && checkTimer < 0) 
        {
            doActionProbability = Random.Range(-10, actionCooldowns*-1);
            checkTimer = 0.2f;
        }

        if (doActionProbability > 0f && !spikeShotCharging) // do an action
        {
            Debug.Log("ATTACKING");
            doActionProbability = -1;
            checkTimer = 2f;
            actionCooldowns = 5.0f;

            spikeShotCharging = true;
            spikeattackDelay = .5f;
            GameObject spikeProjRoar = (GameObject)Instantiate(spikeProjRoarPrefab, new Vector2(rb.position.x, rb.position.y), transform.rotation);
            Destroy(spikeProjRoar, 2.5f);
        }

        if (killProjTimer > 0) killProjTimer -= Time.deltaTime;

        if (spikeattackDelay > 0f) spikeattackDelay -= Time.deltaTime;

        if (spikeattackDelay < 0f && spikeShotCharging)
        {
            shockAttack(8);

            spikeShotCharging = false;
        }

    }


    private float killProjTimer = 3.0f;
    public GameObject spikeProjPrefab; // insert prefab of spike attack projectile
    private void shockAttack(int projectileNumber)
    {
        //killProjTimer = 3.0f;
        float angleStep = 360f/ projectileNumber;
        float angle = 0f;
        float radius = 5f;

        for (int i = 0; i <= projectileNumber-1; i++)
        {
            float projectileDirXpostion = rb.position.x + Mathf.Sin((angle * Mathf.PI) / 180) * radius;
            float projectileDirYpostion = rb.position.y + Mathf.Cos((angle * Mathf.PI) / 180) * radius;

            Vector2 projectileVector = new Vector2 (projectileDirXpostion, projectileDirYpostion);
            Vector2 projectileMoveDirection = (projectileVector - rb.position).normalized * moveSpeed;

            GameObject attackProj = (GameObject)Instantiate(spikeProjPrefab, new Vector2(rb.position.x, rb.position.y + 0.2f), transform.rotation);
            Rigidbody2D attackProjRB = attackProj.GetComponent<Rigidbody2D>();
            //attackProjRB.AddForce(new Vector2(0, 1) * 1.5f * 15, ForceMode2D.Impulse);
            attackProjRB.velocity = new Vector2(projectileMoveDirection.x*3, projectileMoveDirection.y*3);
            Destroy(attackProj, 1.5f);

            angle += angleStep;
        }

        return;
    }

    private void spikeAttack()
    {
        
        return;
    }

}
