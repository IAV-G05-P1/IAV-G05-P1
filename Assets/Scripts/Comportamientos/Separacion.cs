/*    
   Copyright (C) 2020-2025 Federico Peinado
   http://www.federicopeinado.com

   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

   Autor: Federico Peinado 
   Contacto: email@federicopeinado.com
*/
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

namespace UCM.IAV.Movimiento
{
    public class Separacion : ComportamientoAgente
    {
        /// <summary>
        /// Separa al agente
        /// </summary>
        /// <returns></returns>

        // Entidades potenciales de las que huir
        public GameObject targEmpty;

        // Umbral en el que se activa
        [SerializeField]
        float umbral;

        // Coeficiente de reducción de la fuerza de repulsión
        [SerializeField]
        float decayCoefficient;

        public List<GameObject> targets = new List<GameObject>();

        float distance;

        float maxAcceleration;


        public float radio = 2f;

        private SphereCollider trigger;


        public override ComportamientoDireccion GetComportamientoDireccion()
        {
            ComportamientoDireccion result = new ComportamientoDireccion();

            foreach (GameObject rat in targets)
            {
                Vector3 direccion = miTransform.position - rat.transform.position;
                distance = direccion.magnitude;

                if (distance < umbral)
                {
                    float strength = Mathf.Min(decayCoefficient / (distance * distance), maxAcceleration);

                    direccion.Normalize();
                    result.lineal += strength * direccion;
                }
            }

            return result;
        }
        private void OnTriggerEnter(Collider ratColl)
        {
            // Se activa el seguimiento de las ratas al contacto con el trigger
            Merodear ratComp = ratColl.gameObject.GetComponent<Merodear>();
            if (ratComp != null && !targets.Contains(ratColl.gameObject))
            {
                targets.Add(ratColl.gameObject);
            }
        }

        private void OnTriggerExit(Collider ratColl)
        {
            // Si las ratas salen del trigger, se reactivan sus comportamientos por defecto
            Merodear ratComp = ratColl.gameObject.GetComponent<Merodear>();
            if (ratComp != null && targets.Contains(ratColl.gameObject))
            {
                targets.Remove(ratColl.gameObject);
            }
        }


        void Start()
        {
            miTransform = transform;
            maxAcceleration = miTransform.gameObject.GetComponent<Agente>().aceleracionMax;

            trigger = transform.gameObject.AddComponent<SphereCollider>();
            trigger.isTrigger = true;
            trigger.enabled = true;
            trigger.radius = radio;
        }
    }
}