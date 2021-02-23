using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// crea un inspector custom
[CustomEditor(typeof(GunController))]
public class GunControllerInspector : Editor
{
    private GunController instance;

    private void OnEnable()
    {
        instance = target as GunController;
    }

    public override void OnInspectorGUI()
    {
        //ejecutar la funcion del padre
        base.OnInspectorGUI();

        if (GUILayout.Button("Fire1"))
        {
            instance.Shoot(instance.transform.right * 20);
        }
    }
}
