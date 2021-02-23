using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float offsetZ = -20;
    private Vector3 targetPosition;
    Vector3 vel;
    float lastPosGroundY = 0; // ultima posición en el suelo

    // Update is called once per frame
    void Update()
    {
        // solo necesitamos el signo
        var offsetX = Mathf.Sign(PlayerController.instance.rigidbody2D.velocity.x) * 3;

        if (PlayerController.instance.playerIsOnGround)
        {
            lastPosGroundY = PlayerController.instance.transform.position.y;
        }

        targetPosition = new Vector3(
            PlayerController.instance.transform.position.x + offsetX,
            lastPosGroundY,
            offsetZ
            );

        this.transform.position = Vector3.SmoothDamp(this.transform.position, targetPosition, ref vel, 0.3f);
    }
}
