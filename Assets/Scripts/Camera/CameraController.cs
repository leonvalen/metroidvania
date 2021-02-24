using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class RangeData
{
    public float min;
    public float max;
}

public class CameraController : MonoBehaviour
{
    [Header("Boundary Range")]
    [SerializeField]
    RangeData boundaryRangeX;
    [SerializeField]
    RangeData boundaryRangeY;

    // limitar el viewport de la camara
    RangeData cameraSizeX;
    RangeData cameraSizeY;

    [SerializeField] float offsetZ = -20;
    private Vector3 targetPosition;
    Vector3 vel;
    float lastPosGroundY = 0; // ultima posición en el suelo
    public float offsetX;

    // Update is called once per frame

    private void Awake()
    {
        GetCameraSize();
    }

    void GetCameraSize()
    {
        // obtenere cuanto mide la cámara
        cameraSizeX = new RangeData();
        cameraSizeY = new RangeData();
        cameraSizeX.min = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x - this.transform.position.x;
        cameraSizeX.max = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - this.transform.position.x;
        cameraSizeY.min = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y - this.transform.position.y;
        cameraSizeY.max = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - this.transform.position.y;
    }

    void Update()
    {
        // solo necesitamos el signo
        offsetX = Mathf.Sign(PlayerController.instance.nonZeroMovementX) * 3;

        if (PlayerController.instance.playerIsOnGround)
        {
            lastPosGroundY = PlayerController.instance.transform.position.y;
        }

        targetPosition = new Vector3(
            Mathf.Clamp(PlayerController.instance.transform.position.x + offsetX, boundaryRangeX.min - cameraSizeX.min, boundaryRangeX.max - cameraSizeX.max),
            Mathf.Clamp(lastPosGroundY, boundaryRangeY.min - cameraSizeY.min, boundaryRangeY.max - cameraSizeY.max),
            offsetZ
            );

        this.transform.position = Vector3.SmoothDamp(this.transform.position, targetPosition, ref vel, 0.3f);
    }



#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        var pointA = new Vector2(boundaryRangeX.min, boundaryRangeY.min);
        var pointB = new Vector2(boundaryRangeX.max, boundaryRangeY.min);
        Gizmos.DrawLine(pointA, pointB);
        pointA = new Vector2(boundaryRangeX.min, boundaryRangeY.max);
        pointB = new Vector2(boundaryRangeX.max, boundaryRangeY.max);
        Gizmos.DrawLine(pointA, pointB);
        pointA = new Vector2(boundaryRangeX.min, boundaryRangeY.min);
        pointB = new Vector2(boundaryRangeX.min, boundaryRangeY.max);
        Gizmos.DrawLine(pointA, pointB);
        pointA = new Vector2(boundaryRangeX.max, boundaryRangeY.min);
        pointB = new Vector2(boundaryRangeX.max, boundaryRangeY.max);
        Gizmos.DrawLine(pointA, pointB);

    }
#endif
}
