/*    
   Copyright (C) 2020-2025 Federico Peinado
   http://www.federicopeinado.com

   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

   Autor: Federico Peinado 
   Contacto: email@federicopeinado.com
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UCM.IAV.Movimiento
{

    /// <summary>
    /// Clase para modelar el comportamiento de HUIR a otro agente
    /// </summary>
    public class Huir : ComportamientoAgente
    {
        public float radio = 5f;

        public List<GameObject> rats = new List<GameObject>();

        private SphereCollider trigger;

        //BING CHILLING

        /// <summary>
        /// Obtiene la dirección
        /// </summary>
        /// <returns></returns>
        public override ComportamientoDireccion GetComportamientoDireccion()
        {
            ComportamientoDireccion resultado = new ComportamientoDireccion();

            foreach (GameObject rat in rats)
            {
                ComportamientoDireccion aux = new ComportamientoDireccion();
                if (rat != null)
                {
                    aux.lineal = miTransform.position - rat.transform.position;

                    aux.lineal.Normalize();
                    aux.lineal *= agente.aceleracionMax;

                    resultado.lineal += aux.lineal;
                }
            }        

            

            return resultado;
        }

        private void OnTriggerEnter(Collider ratColl)
        {
            // Se activa el seguimiento de las ratas al contacto con el trigger
            Merodear ratComp = ratColl.gameObject.GetComponent<Merodear>();
            if (ratComp != null && !rats.Contains(ratColl.gameObject))
            {
                rats.Add(ratColl.gameObject);
            }
        }

        private void OnTriggerExit(Collider ratColl)
        {
            // Si las ratas salen del trigger, se reactivan sus comportamientos por defecto
            Merodear ratComp = ratColl.gameObject.GetComponent<Merodear>();
            if (ratComp != null && rats.Contains(ratColl.gameObject))
            {
                rats.Remove(ratColl.gameObject);
            }
        }

        void Start()
        {
            miTransform = transform;

            trigger = transform.gameObject.AddComponent<SphereCollider>();
            trigger.isTrigger = true;
            trigger.enabled = false;
            trigger.radius = radio;
        }

    }
}
