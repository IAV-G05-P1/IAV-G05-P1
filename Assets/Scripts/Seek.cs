using System.Collections;
using System.Collections.Generic;
using UCM.IAV.Movimiento;
using UnityEngine;

public class Seek : ComportamientoAgente
{
    Transform character;
    [SerializeField] Transform target;

    #region parameters
    [SerializeField] float maxSpeed;
    [SerializeField] float satRadius;
    [SerializeField] float timeToTarget = 0.25f;
    #endregion

    #region methods
    /*public ComportamientoDireccion SeekBehaviour()
    {
        ComportamientoDireccion result = new ComportamientoDireccion();
        result.lineal = target.position - character.position;

        if (result.lineal.magnitude < satRadius)
        {
            return null;
        }
        result.lineal /= timeToTarget;

        if (result.lineal.magnitude < maxSpeed)
        {
            result.lineal.Normalize();
            result.lineal *= maxSpeed;
        }

        character.rotation = Quaternion.Euler(result.lineal);
    }*/
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        character = transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
