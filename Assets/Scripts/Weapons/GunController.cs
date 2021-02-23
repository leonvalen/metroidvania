using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GunPosition
{
    Stand,
    Duck,
    Up
}


public class GunController : MonoBehaviour
{
    [SerializeField] GameObject projectilPrefab;

    [SerializeField] Transform standPivot;
    [SerializeField] Transform duckPivot;
    [SerializeField] Transform upPivot;

    public void SetPosition(GunPosition gunPosition)
    {
        if (gunPosition == GunPosition.Stand)
        {
            this.transform.position = standPivot.position;
        }
        if (gunPosition == GunPosition.Duck)
        {
            this.transform.position = duckPivot.position;
        }
        if (gunPosition == GunPosition.Up)
        {
            this.transform.position = upPivot.position;
        }
    }


    // se recibe un vector de fuerza
    public void Shoot(Vector2 force)
    {
        //variable local en la posición que tiene este GameObject y su Quaternion.identity
        var projectile = Instantiate(projectilPrefab, this.transform.position, Quaternion.identity);
        // recuperar el componente del GameObject que instanciamos
        projectile.GetComponent<ProjectileController>().Shoot(force);


    }

    //macro UNITY_EDITOR
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        //dibujar una pistola lazer
        Gizmos.DrawWireSphere(this.gameObject.transform.position, 0.3f);
        Gizmos.DrawWireCube(this.transform.position + this.transform.right * 0.45f, new Vector2(0.3f, 0.3f));
    }
#endif
}
