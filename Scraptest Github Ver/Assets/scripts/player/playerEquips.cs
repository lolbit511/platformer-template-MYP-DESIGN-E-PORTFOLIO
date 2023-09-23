using UnityEngine;

public class playerEquips : MonoBehaviour
{
    public Transform player;

    void Update()
    {
        Vector3 targetPosition = player.position;
        Vector3 smoothPos = Vector3.Lerp(transform.position, targetPosition, 2f * Time.fixedDeltaTime);
        transform.position = targetPosition;
    }
}
