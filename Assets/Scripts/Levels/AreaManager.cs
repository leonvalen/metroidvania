using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AreaManager : MonoBehaviour
{
    [SerializeField] Color areaColor;
    [SerializeField] RangeData boundaryRangeX;
    [SerializeField] RangeData boundaryRangeY;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //si el jugador entró en el área
        if (collision.gameObject.tag.Equals("Player"))
        {
            UpdateCameraBoundaries();
        }

    }
    void UpdateCameraBoundaries()
    {
        var cameraBoundaryX = new RangeData();
        var cameraBoundaryY = new RangeData();
        cameraBoundaryX.min = boundaryRangeX.min + this.transform.position.x;
        cameraBoundaryX.max = boundaryRangeX.max + this.transform.position.x;
        cameraBoundaryY.min = boundaryRangeY.min + this.transform.position.y;
        cameraBoundaryY.max = boundaryRangeY.max + this.transform.position.y;
        FindObjectOfType<CameraController>().UpdateBoundaries(cameraBoundaryX, cameraBoundaryY);
    }

    private void Awake()
    {
        UpdateColiider();
    }

    void UpdateColiider()
    {
        var boxCollider = GetComponent<BoxCollider2D>();
        var offSet = Vector2.zero;
        var deltaX = Mathf.Abs(Mathf.Abs(boundaryRangeX.min) - Mathf.Abs(boundaryRangeX.max)) * 0.5f;
        var deltaY = Mathf.Abs(Mathf.Abs(boundaryRangeY.min) - Mathf.Abs(boundaryRangeY.max)) * 0.5f;
        offSet.x = deltaX;
        offSet.y = deltaY;
        if (Mathf.Abs(boundaryRangeX.min) > Mathf.Abs(boundaryRangeX.max))
        {
            offSet.x *= -1;
        }
        if (Mathf.Abs(boundaryRangeY.min) > Mathf.Abs(boundaryRangeY.max))
        {
            offSet.y *= -1;
        }
        boxCollider.offset = offSet;
        //obtener el tamaño del boxCollider
        boxCollider.size = new Vector2(
            Mathf.Abs(boundaryRangeX.min) + Mathf.Abs(boundaryRangeX.max),
            Mathf.Abs(boundaryRangeY.min) + Mathf.Abs(boundaryRangeY.max)
        );
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = areaColor;


        var pointA = new Vector2(boundaryRangeX.min, boundaryRangeY.min) + (Vector2)this.transform.position;
        var pointB = new Vector2(boundaryRangeX.max, boundaryRangeY.min) + (Vector2)this.transform.position;


        Gizmos.DrawLine(pointA, pointB);
        pointA = new Vector2(boundaryRangeX.min, boundaryRangeY.max) + (Vector2)this.transform.position;
        pointB = new Vector2(boundaryRangeX.max, boundaryRangeY.max) + (Vector2)this.transform.position;
        Gizmos.DrawLine(pointA, pointB);
        pointA = new Vector2(boundaryRangeX.min, boundaryRangeY.min) + (Vector2)this.transform.position;
        pointB = new Vector2(boundaryRangeX.min, boundaryRangeY.max) + (Vector2)this.transform.position;
        Gizmos.DrawLine(pointA, pointB);
        pointA = new Vector2(boundaryRangeX.max, boundaryRangeY.min) + (Vector2)this.transform.position;
        pointB = new Vector2(boundaryRangeX.max, boundaryRangeY.max) + (Vector2)this.transform.position;
        Gizmos.DrawLine(pointA, pointB);
    }
#endif
}