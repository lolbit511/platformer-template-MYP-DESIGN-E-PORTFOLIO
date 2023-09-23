using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class player : NetworkBehaviour
{
    [SerializeField] private float playerSpeed = 5.0f;
    [SerializeField] private float jumpPower = 5.0f;

    [Header("pos debug")]
    [SerializeField] private NetworkVariable<float> playerPositionX = new NetworkVariable<float>();
    [SerializeField] private NetworkVariable<float> playerPositionY = new NetworkVariable<float>();

    // scraped network code (didnt work)
    #region networking
    /*
    private float oldPlayerX;
    private float oldPlayerY;
    private void UpdateServer()
    {
        transform.position = new Vector2(playerPositionX.Value, playerPositionY.Value);
    }

    private void UpdateClient()
    {
        float playerX = _playerRigidbody.position.x;
        float playerY = _playerRigidbody.position.y;

        MovePlayer();
        jump();


        // sync values of player coord
        if (oldPlayerX != playerX || oldPlayerY != playerY)
        {
            oldPlayerX = playerX;
            oldPlayerY = playerY;
        }

        UpdateClientPositionServerRpc(playerX, playerY);
    }
    */
    [ServerRpc]
    public void UpdateClientPositionServerRpc(float playerPosX, float playerPosY)
    {
        playerPositionX.Value = playerPosX;
        playerPositionY.Value = playerPosY;
    }

    #endregion

    [Header("Health")]
    [SerializeField] private float iFrames = -1f;
    static public float maxHP = demoscript.maxHP;
    static public float currentHealth = 5;
    static public float currentHealth_player2 = 5;
    [SerializeField] public float currentHP = 5;
    //public Image HealthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<Image>();
    private bool alive = true;
    public GameObject deathParticlesSmoke;
    public GameObject deathParticlesSpark;
    public GameObject deathParticlesCorpse;
    private float respawnCountdown = 2.5f;
    public Transform respawnPoint;

    public SpriteRenderer spriteRenderer;
    public Sprite notDead;
    public Sprite ded;

    [Header("Energy")]
    static public float maxEnergy = demoscript.maxEnergy;
    static public float currentEnergy = 1;
    static public float currentEnergy_player2 = 1;
    [SerializeField] public float currentEN = 1;

    [Header("jump")]
    private float jumpTimeCounter;
    public float jumpTime;
    private bool isJumping;

    // check for ground
    [Header("layers")]
    public LayerMask groundLayers;
    public LayerMask attackLayers;

    //private float rollSpeed = 15;
    public static int weaponType = 0;

    // dash
    [Header("dash")]
    private float facing = 1;
    private float horizontal = 1;
    private float dashCD = 0.8f;
    public float dashSpeed = 50f;
    private bool dashing = false;
    private bool canDash = false;

    // coyoteTime
    private float coyoteTime = 0.1f;
    private float coyoteTimeCounter;

    // jumpBuffer
    private float jumpBufferTime = 1f;
    private float jumpBufferCounter;

    public Rigidbody2D _playerRigidbody;
    public GameObject cam;
    //public Transform playerPos;

    public static int camOffset = 1;

    [HideInInspector]
    static public bool grounded;

    public bool playerOne;

    private void Start()
    {
        //Instantiate(cam);
    }
    private void Awake()
    {
        //if (!IsOwner) return;
        //_playerRigidbody = GetComponent<Rigidbody2D>();
        if (_playerRigidbody == null)
        {
            Debug.LogError("Player is missing a Rigidbody2D component");
        }
    }



    private void Update()
    {
        
        if (IsServer)
        {
            //UpdateServer();
        }

        //if (!IsOwner) return;

        //if (HealthBar == null) HealthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<Image>();



        if (!alive && playerOne)
        {

            respawnCountdown -= Time.deltaTime;
            if (respawnCountdown < 0) // repspawn code . respawned code
            {
                transform.position = new Vector2(respawnPoint.position.x, respawnPoint.position.y);
                currentHealth = maxHP;
                currentEnergy = maxEnergy;
                alive = true;
            }
            return;
        }

        if (!alive && !playerOne)
        {

            respawnCountdown -= Time.deltaTime;
            if (respawnCountdown < 0) // repspawn code . respawned code
            {
                transform.position = new Vector2(respawnPoint.position.x, respawnPoint.position.y);
                currentHealth_player2 = maxHP;
                currentEnergy_player2 = maxEnergy;
                alive = true;
            }
            return;
        }

        if (Input.GetKeyDown(KeyCode.E) &&
            SceneManager.GetActiveScene() == SceneManager.GetSceneByName("openworld"))
        {
            SceneManager.LoadScene("inventory");
        }

        //maxHP = demoscript.maxHP;
        //HealthBar.fillAmount = currentHP / maxHP;

        //Debug.Log("maxHP:" + maxHP);
        //Debug.Log("currentHP:" + currentHP);
        //Debug.Log("currentHealth:" + currentHealth);

        // currentHP belongs to the healthbar class
        // currentHealth belongs to the healthbar class
        currentHP = currentHealth;
        currentEN = currentEnergy;

        if (currentHealth > maxHP)
        {
           currentHealth = maxHP;
        }
        if (currentEnergy > maxEnergy)
        {
            currentEnergy = maxEnergy;
        }


        /* debugging health (also health cheats)
        if (Input.GetKeyDown(KeyCode.O))
        {
            currentHealth++;
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            currentHealth--;
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            currentHealth--;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            currentEnergy--;
        }
        */
        if (playerOne)
        {
            if (currentEnergy == maxEnergy && currentHealth != maxHP)
            {
                currentHealth++;
                currentEnergy = 0;
            }

            if (attacked() && iFrames < 0)
            {
                currentHealth--;
                iFrames = 1.5f;
            }
            if (iFrames >= 0)
            {
                iFrames -= Time.deltaTime;
            }

            if (currentHealth <= 0) //death code
            {
                _playerRigidbody.velocity = Vector3.zero;
                _playerRigidbody.AddForce(Vector2.up * jumpPower * 5, ForceMode2D.Impulse);
                GameObject deathBurstSmoke = (GameObject)Instantiate(deathParticlesSmoke, new Vector2(_playerRigidbody.position.x, _playerRigidbody.position.y), transform.rotation);
                GameObject deathBurstSpark = (GameObject)Instantiate(deathParticlesSpark, new Vector2(_playerRigidbody.position.x, _playerRigidbody.position.y), transform.rotation);
                GameObject deathBurstCorpse = (GameObject)Instantiate(deathParticlesSpark, new Vector2(_playerRigidbody.position.x, _playerRigidbody.position.y), transform.rotation);
                Destroy(deathBurstSmoke, 2.5f);
                Destroy(deathBurstSpark, 2.5f);
                Destroy(deathBurstCorpse, 5.5f);
                respawnCountdown = 5.0f;
                alive = false;
            }
        }
        if (!playerOne)
        {
            if (currentEnergy_player2 == maxEnergy && currentHealth_player2 != maxHP)
            {
                currentHealth_player2++;
                currentEnergy_player2 = 0;
            }

            if (attacked() && iFrames < 0)
            {
                currentHealth_player2--;
                iFrames = 1.5f;
            }
            if (iFrames >= 0)
            {
                iFrames -= Time.deltaTime;
            }

            if (currentHealth_player2 <= 0) //death code
            {
                _playerRigidbody.velocity = Vector3.zero;
                _playerRigidbody.AddForce(Vector2.up * jumpPower * 5, ForceMode2D.Impulse);
                GameObject deathBurstSmoke = (GameObject)Instantiate(deathParticlesSmoke, new Vector2(_playerRigidbody.position.x, _playerRigidbody.position.y), transform.rotation);
                GameObject deathBurstSpark = (GameObject)Instantiate(deathParticlesSpark, new Vector2(_playerRigidbody.position.x, _playerRigidbody.position.y), transform.rotation);
                Destroy(deathBurstSmoke, 2.5f);
                Destroy(deathBurstSpark, 2.5f);
                respawnCountdown = 5.0f;
                alive = false;
            }
        }


    }

    private void LateUpdate()
    {
        //if (!IsOwner) return;

        if (!alive) return;

        if (IsGrounded())
        {

            coyoteTimeCounter = coyoteTime;
        }
        else
        {

            coyoteTimeCounter -= Time.deltaTime;
        }
        //UpdateClient();
        MovePlayer();
        jump();
        //look();
    }

    private void FixedUpdate()
    {
        //if (!IsOwner) return;
        // inventory

        if (!alive) return;

        grounded = IsGrounded();
        //Debug.Log("d");
        if (playerOne)
        {
            if (Input.GetKey(KeyCode.W) && isJumping == true) // higher jump when held
            {
                if (jumpTimeCounter > 0)
                {
                    _playerRigidbody.AddForce(Vector2.up * jumpPower * 15, ForceMode2D.Impulse);
                    jumpTimeCounter -= Time.deltaTime;
                }
                else
                {
                    isJumping = false;
                    coyoteTimeCounter = 0f;
                }
            }
        }
        if (!playerOne)
        {
            if (Input.GetKey(KeyCode.UpArrow) && isJumping == true) // higher jump when held
            {
                if (jumpTimeCounter > 0)
                {
                    _playerRigidbody.AddForce(Vector2.up * jumpPower * 15, ForceMode2D.Impulse);
                    jumpTimeCounter -= Time.deltaTime;
                }
                else
                {
                    isJumping = false;
                    coyoteTimeCounter = 0f;
                }
            }
        }
    
    }


    private void jump()
    {
        if (playerOne)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                jumpBufferCounter = jumpBufferTime;
            }
            else
            {
                jumpBufferCounter -= Time.deltaTime;
            }
            if (Input.GetKeyUp(KeyCode.W))
            {
                isJumping = false;
            }
        }
        if (!playerOne)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                jumpBufferCounter = jumpBufferTime;
            }
            else
            {
                jumpBufferCounter -= Time.deltaTime;
            }
            if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                isJumping = false;
            }
        }


        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            jumpBufferCounter = 0;
            _playerRigidbody.velocity = Vector2.zero;
            _playerRigidbody.angularVelocity = 0f;
            _playerRigidbody.AddForce(Vector2.up * (jumpPower + 8), ForceMode2D.Impulse);
        }

        
    }

    

    private void look()
    {
        var verticalInput = Input.GetAxisRaw("Vertical"); // unused, clashed with new inputs
        if (verticalInput == 1)
        {
            camOffset = 2;
        }
        else if (verticalInput == -1)
        {
            camOffset = -2;
        }
        else
        {
            camOffset = 0;
        }
    }


    //private float movementX;
    private void MovePlayer()
    {
        var horizontalInput = 0;
        if (playerOne)
        {
            if (Input.GetKey(KeyCode.A))
            {
                horizontalInput = -1;
                horizontal = -1;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                horizontalInput = 1;
                horizontal = 1;
            }
        }
        if(!playerOne)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                horizontalInput = -1;
                horizontal = -1;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                horizontalInput = 1;
                horizontal = 1;
            }
        }
        
        //_playerRigidbody.velocity = new Vector2(horizontalInput * playerSpeed, _playerRigidbody.velocity.y);

        //horizontalInput = Input.GetAxis("Horizontal");
        _playerRigidbody.velocity = new Vector2(horizontalInput * playerSpeed, _playerRigidbody.velocity.y);

        // dash

        //horizontal = Input.GetAxisRaw("Horizontal");
        if (horizontal != 0)
        {
            facing = horizontal;
            
        }

        if (facing == 1)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }

        //gameObject.transform.localScale = new Vector2(facing * 0.5f, 0.5f);

        if (playerOne)
        {
            if (Input.GetKeyDown(KeyCode.V) && dashing == false && canDash)
            {
                dashing = true;
                canDash = false;
                dashCD = 0.2f;
            }
        }
        if (!playerOne)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1) && dashing == false && canDash)
            {
                dashing = true;
                canDash = false;
                dashCD = 0.2f;
            }
        }
        

        if (dashing)
        {
            Debug.Log("dash triggered ");
            _playerRigidbody.velocity = new Vector2(transform.localScale.x * dashSpeed * facing, 0f);
        }

        if (dashCD > 0)
        {
            dashCD -= Time.deltaTime;
        }
        else
        {
            dashing = false;
            if (IsGrounded())
            {
                canDash = true;
            }
        }

    }


    private void Jump() => _playerRigidbody.velocity = new Vector2(0, jumpPower);

    private bool IsGrounded()
    {
        return Physics2D.OverlapArea(new Vector2(transform.position.x, transform.position.y - 0.5f),
            new Vector2(transform.position.x, transform.position.y - 0.5f), groundLayers);
    }

    private bool attacked()
    {
        return Physics2D.OverlapArea(new Vector2(transform.position.x, transform.position.y),
            new Vector2(transform.position.x, transform.position.y), attackLayers);
    }
}