using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDemo : MonoBehaviour, IDamageTarget
{
    [SerializeField] float healt = 3;
    public void TakeDamage(float damagePoints)
    {
        healt -= damagePoints;
        if (healt <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
