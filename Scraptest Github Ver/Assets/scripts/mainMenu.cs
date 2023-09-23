using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainMenu : MonoBehaviour
{
    public GameObject player1;
    public GameObject cam1;

    public GameObject player2;
    public GameObject cam2;

    public Transform spawn;
    public GameObject camToDelete;
    public GameObject MainMenu;

    private Camera playerCamera1;
    private Camera playerCamera2;

    private Camera cam;

    public GameObject player2UI;
    void Start()
    {
        playerCamera1 = cam1.GetComponent<Camera>();
        playerCamera2 = cam2.GetComponent<Camera>();
        player2UI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startGame_OnePlayer()
    {

        Destroy(camToDelete);
        MainMenu.SetActive(false);

        cam1.SetActive(true);
        player1.transform.position = new Vector2(spawn.position.x, spawn.position.y);
    }
    public void startGame_TwoPlayer()
    {
        Destroy(camToDelete);
        MainMenu.SetActive(false);
        player2UI.SetActive(true);

        cam1.SetActive(true);
        player1.transform.position = new Vector2(spawn.position.x, spawn.position.y);

        cam2.SetActive(true);
        player2.transform.position = new Vector2(spawn.position.x, spawn.position.y);

        playerCamera1.rect = new Rect(0,0.5f,1,0.5f);
        playerCamera2.rect = new Rect(0,0,1,0.5f);

    }
}
