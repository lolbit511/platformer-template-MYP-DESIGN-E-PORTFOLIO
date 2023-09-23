using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public class bruiser : MonoBehaviour
{
    public int health = 30;
    [SerializeField] private float iFrames = -1f;
    [HideInInspector] public LayerMask attackLayers;

    [SerializeField] private float actionCooldowns;
    bool attacking = false;
    bool chooseAttack = false;
    int attackType = 0;
    bool shieldRaised = false;
    float untilShieldDown = 5f;

    bool aboutToParry = false;
    float parryDelay = 0f;

    bool aboutToCharge = false;
    float chargeDelay = 0f;
    bool charging = false;
    float untilChargeEnd = 1f;

    bool chargingLeft = true;

    [HideInInspector] public GameObject attackCharge;
    [HideInInspector] public GameObject drillAttackCharge;
    [HideInInspector] public GameObject hurtSmokeBurst;
    [HideInInspector] public GameObject deathFragments;
    public GameObject spawnSmoke;
    public GameObject spawnObject;
    public GameObject spawnObject2;

    //public GameObject placeholderParry;

    [HideInInspector] public float moveSpeed;
    private float currentSpeed;

    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Transform target = null;
    Vector2 moveDir;

    private int finalAngle = 180;

    [HideInInspector] public GameObject[] playerList;

    private bool targetIsPlayerOne;

    SpriteRenderer sprite;
    [HideInInspector] public float redness = 0;

    //public Image bruiserImage;
    public Sprite idle;
    public Sprite shieldUp;




    //health milestones
    bool hp_25 = false;
    bool hp_20 = false;
    bool hp_15 = false;
    bool hp_10 = false;
    bool hp_5 = false;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        //bruiserImage = GetComponent<Image>();
    }
    private void Awake()
    {
        actionCooldowns = 3.5f;
        //target = GetClosestPlayer();
        playerList = GameObject.FindGameObjectsWithTag("Player");
        target = GameObject.FindGameObjectWithTag("Player").transform;

        
        
    }

    void Update()
    {
        //debugstuff();

        outOfBounds();
        attackManager();
        //Debug.Log(target);
        //target = GetClosestPlayer();
        findNearestPlayer();

        if (shieldRaised)
        {
            sprite.sprite = shieldUp;
        }
        if (!shieldRaised)
        {
            sprite.sprite = idle;
        } 

        if (attacked() && iFrames < 0)
        {
            if (shieldRaised)
            {
                // run code for successful parry //////////////////////////////// undone
                GameObject groundSlam = (GameObject)Instantiate(earthShatterProj, new Vector2(1.38f, -3.76f), transform.rotation);
                Destroy(groundSlam, 3f);
                shieldRaised = false;
                attacking = false;
                shockAttack(4, 4,false);
                shockAttack(8, 6,false);
            }
            else
            {
                //Debug.Log("hit! remaining hp: " + health);
                if (targetIsPlayerOne) player.currentEnergy++;
                if (!targetIsPlayerOne) player.currentEnergy_player2++; 
                health--;
                iFrames = 0.35f;
                GameObject hurtParticles = (GameObject)Instantiate(hurtSmokeBurst, new Vector2(rb.position.x, rb.position.y-0.15f), transform.rotation);
                Destroy(hurtParticles, 2.5f);

                
            }
            
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
            sprite.color = new Color(1, 0,0);
        }
        else
        {
            // colors
            redness = (30f - health) / 30f;
            //Debug.Log(redness);
            sprite.color = new Color(1, 1 - redness, 1 - redness);
        }

        if (target)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;


            //Debug.Log("angle: " + angle);
            if (angle > -90 && angle < 90 && !attacking) // turn to player
            {
                finalAngle = 1;
            }
            else
            {
                finalAngle = -1;
            }
            gameObject.transform.localScale = new Vector2(finalAngle* 0.48f, 0.48f);

            
        }


    }

    private void turnToPlayer()
    {

    }
    private void FixedUpdate()
    {
        if (target)
        {
            rb.velocity = new Vector2(moveDir.x, moveDir.y) * currentSpeed;
        }
    }

    private void outOfBounds()
    {

        if (transform.position.x > 3.33)
        {
            transform.position = new Vector2(3.33f, transform.position.y);
        }
        if (transform.position.x < -5.43)
        {
            transform.position = new Vector2(-5.43f, transform.position.y);
        }
    }

    private bool dist()
    {
        float minDist = 0;
        float maxDist = 100;
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
        if (!attacking) // important ///// remember to turn attacking off after attack
        {
            actionCooldowns -= Time.deltaTime;
        }


        if (actionCooldowns < 0)
        {
            attacking = true;
            chooseAttack = true;
            if (health < 10) // health below 10, phase 2
            {
                attackType = Random.Range(1, 3);
            }
            else
            {
                
                attackType = 2;
            }
            
            Debug.Log("current attack index: " + attackType);
        }

        parryUpdate();

        if (chooseAttack && attackType == 1) //parry attack
        {
            GameObject attackCharging = (GameObject)Instantiate(attackCharge, new Vector2(rb.position.x, rb.position.y-0.3f), transform.rotation);
            Destroy(attackCharging, 2.5f);

            actionCooldowns = 3f;
            chooseAttack = false;

            aboutToParry = true;
            parryDelay = 0.5f;

            
        }
        if (aboutToParry) parryDelay -= Time.deltaTime;
        if (parryDelay < 0)
        {
            parry();
        }


        if (chooseAttack && attackType == 2) //charge attack
        {
            GameObject attackCharging2 = (GameObject)Instantiate(drillAttackCharge, new Vector2(rb.position.x, rb.position.y-0.3f), transform.rotation);
            Destroy(attackCharging2, 4.5f);

            actionCooldowns = 3.5f;
            chooseAttack = false;

            aboutToCharge = true;
            chargeDelay = 1.5f;
        }
        if (aboutToCharge) chargeDelay -= Time.deltaTime;
        if (chargeDelay < 0)
        {
            //Debug.Log("charged");
            charge();
        }
        if (charging)
        {
            chargeUpdate();
        }
        

        #region healthspawns
        if (health == 25 && !hp_25)
        {
            hp_25 = true;
            GameObject minionSpawn = (GameObject)Instantiate(spawnSmoke, new Vector2(rb.position.x, rb.position.y + 2.5f), transform.rotation);
            Destroy(minionSpawn, 2.5f);

            Instantiate(spawnObject, new Vector2(rb.position.x, rb.position.y + 2.5f), transform.rotation);

            parry();
        }
        if (health == 20 && !hp_20)
        {
            hp_20 = true;
            GameObject minionSpawn = (GameObject)Instantiate(spawnSmoke, new Vector2(rb.position.x, rb.position.y + 2.5f), transform.rotation);
            Destroy(minionSpawn, 2.5f);

            Instantiate(spawnObject, new Vector2(rb.position.x, rb.position.y + 2.5f), transform.rotation);

            parry();
        }
        if (health == 15 && !hp_15)
        {
            hp_15 = true;
            GameObject minionSpawn = (GameObject)Instantiate(spawnSmoke, new Vector2(rb.position.x, rb.position.y + 2.5f), transform.rotation);
            Destroy(minionSpawn, 2.5f);

            Instantiate(spawnObject, new Vector2(rb.position.x, rb.position.y + 2.5f), transform.rotation);
            Instantiate(spawnObject, new Vector2(rb.position.x, rb.position.y + 3f), transform.rotation);

            parry();
        }
        if (health == 10 && !hp_10)
        {
            hp_10 = true;
            GameObject minionSpawn = (GameObject)Instantiate(spawnSmoke, new Vector2(rb.position.x, rb.position.y + 2.5f), transform.rotation);
            Destroy(minionSpawn, 2.5f);

            Instantiate(spawnObject, new Vector2(rb.position.x, rb.position.y + 2.5f), transform.rotation);
            Instantiate(spawnObject, new Vector2(rb.position.x+1, rb.position.y + 2.5f), transform.rotation);
            Instantiate(spawnObject, new Vector2(rb.position.x-1, rb.position.y + 2.5f), transform.rotation);



            parry();
        }
        if (health == 5 && !hp_5)
        {
            hp_5 = true;
            GameObject minionSpawn = (GameObject)Instantiate(spawnSmoke, new Vector2(rb.position.x, rb.position.y + 2.5f), transform.rotation);
            Destroy(minionSpawn, 2.5f);

            Instantiate(spawnObject2, new Vector2(rb.position.x, rb.position.y + 2.5f), transform.rotation);


            parry();
        }
        #endregion
    }

    private void parry() // parry activation code
    {
        untilShieldDown = 5f;
        shieldRaised = true;
        aboutToParry = false;
        parryDelay = 5f;

        //GameObject parry = (GameObject)Instantiate(placeholderParry, new Vector2(rb.position.x, rb.position.y - 0.15f), transform.rotation);
        //Destroy(parry, 2.5f);
    }

    private void charge()
    {
        aboutToCharge = false;
        chargeDelay = 5f;
        untilChargeEnd = 3f;
        charging = true;
        shieldRaised = false;
        shockAttack(4,2, false);
    }
    private void chargeUpdate()
    {
        if (chargingLeft) transform.position = Vector2.MoveTowards(transform.position, leftpos.position, 5.5f * Time.deltaTime);
        if (!chargingLeft) transform.position = Vector2.MoveTowards(transform.position, rightpos.position, 5.5f * Time.deltaTime);
        untilChargeEnd -= Time.deltaTime;
        if (untilChargeEnd < 0)
        {
            shockAttack(10, 2.5f, false);
            shockAttack(8, 2f, false);
            attacking = false;
            charging = false;
            chargingLeft = !chargingLeft;
        }
    }


    private void parryUpdate() // parry code that runs every frame
    {
        if (shieldRaised)
        {
            // set frame to hand raised ///////////////////////////////////////////////// undone
            untilShieldDown -= Time.deltaTime;
        }

        if (untilShieldDown <= 0)
        {
            shieldRaised = false;
            attacking = false;
        }

    }


    public GameObject earthShatterProj; // insert prefab of ground attack projectile

    public GameObject spikeProjPrefab; // insert prefab of spike attack projectile
    public GameObject spikeProjPrefaDim;
    private void shockAttack(int projectileNumber,float projSpeed,bool Dimmed)
    {
        //killProjTimer = 3.0f;
        float angleStep = 360f / projectileNumber;
        float angle = 0f;
        float radius = 5f;

        for (int i = 0; i <= projectileNumber - 1; i++)
        {
            float projectileDirXpostion = rb.position.x + Mathf.Sin((angle * Mathf.PI) / 180) * radius;
            float projectileDirYpostion = rb.position.y + Mathf.Cos((angle * Mathf.PI) / 180) * radius;

            Vector2 projectileVector = new Vector2(projectileDirXpostion, projectileDirYpostion);
            Vector2 projectileMoveDirection = (projectileVector - rb.position).normalized * projSpeed;

            if (!Dimmed)
            {
                GameObject attackProj = (GameObject)Instantiate(spikeProjPrefab, new Vector2(rb.position.x, rb.position.y + 0.2f), transform.rotation);
                Rigidbody2D attackProjRB = attackProj.GetComponent<Rigidbody2D>();
                //attackProjRB.AddForce(new Vector2(0, 1) * 1.5f * 15, ForceMode2D.Impulse);
                attackProjRB.velocity = new Vector2(projectileMoveDirection.x * 3, projectileMoveDirection.y * 3);
                Destroy(attackProj, 1.5f);
            }
            else
            {
                GameObject attackProj = (GameObject)Instantiate(spikeProjPrefaDim, new Vector2(rb.position.x, rb.position.y + 0.2f), transform.rotation);
                Rigidbody2D attackProjRB = attackProj.GetComponent<Rigidbody2D>();
                //attackProjRB.AddForce(new Vector2(0, 1) * 1.5f * 15, ForceMode2D.Impulse);
                attackProjRB.velocity = new Vector2(projectileMoveDirection.x * 3, projectileMoveDirection.y * 3);
                Destroy(attackProj, 1.5f);
            }
            

            angle += angleStep;
        }

        return;
    }
            


    public Transform leftpos;
    public Transform rightpos;
    private void debugstuff()
    {
        
        if (Input.GetKeyDown(KeyCode.P))
        {
            //transform.position = Vector2.MoveTowards(transform.position, leftpos.position, 5 * Time.deltaTime);
            
        }
        
    }

}
