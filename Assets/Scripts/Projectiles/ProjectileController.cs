using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] string tagName = "Enemy";
    [SerializeField] float damagePoints = 1;
    [SerializeField] float autoDestroyDelay = 4f;
    private Rigidbody2D rigidbody2D;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals(tagName))
        {
            //buscamos si tiene la implementación de la interface
            var component = collision.gameObject.GetComponent<IDamageTarget>();
            // si no es nulo implementa la interface adecuadamente
            if (component != null)
            {
                component.TakeDamage(damagePoints);
                // destruimos el projectil
                Destroy(this.gameObject);
            }
        }
    }


    //definir el mismo método con el mismo nombre
    // la idea es que esté por separado el manejo de la velocidad
    public void Shoot(Vector2 force)
    {
        Destroy(this.gameObject, autoDestroyDelay);
        // si no es nulo lo recuperamos
        if (rigidbody2D == null)
        {
            rigidbody2D = GetComponent<Rigidbody2D>();
        }

        var angle = Mathf.Atan2(force.y, force.x) * Mathf.Rad2Deg;
        // rotar el proyectil deacuerdo a la fuerza
        this.transform.rotation = Quaternion.Euler(0, 0, angle);

        // le aplicamos una fuerza
        rigidbody2D.AddForce(force, ForceMode2D.Impulse);
    }
}
