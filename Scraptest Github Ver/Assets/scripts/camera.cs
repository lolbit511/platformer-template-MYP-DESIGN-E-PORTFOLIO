using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using Cinemachine;

public class camera : NetworkBehaviour
{
    public int followingPlayer;
    public GameObject target;
    public Vector3 offset;
    public float damping;

    private Vector3 velocity = Vector3.zero;

    [SerializeField] GameObject playerCam;

    //public GameObject P1;
    //public GameObject P2;


    //[SerializeField] private CinemachineFreeLook freeLookCameraToZoom;



    public void Update()
    {
        
        if (target == null)
        {
            //if (followingPlayer == 1) target = P1;
            //if (followingPlayer == 2) target = P2;
        }
        

        Follow();

    }

    void Follow()
    {
        if (target == null) return;
        /*
        freeLookCameraToZoom = CinemachineFreeLook.FindObjectOfType<CinemachineFreeLook>();
        freeLookCameraToZoom.LookAt = this.gameObject.transform;
        freeLookCameraToZoom.Follow = this.gameObject.transform;
        */

        Vector3 targetPosition = target.transform.position + offset;
        targetPosition.y = targetPosition.y + 1;

        targetPosition.y = targetPosition.y + player.camOffset;

        

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, damping);
        

    }

    public override void OnNetworkSpawn()
    {
        GameObject camToDelete = GameObject.Find("startcam");
        Destroy(camToDelete);
        transform.parent = null;
    }
}
