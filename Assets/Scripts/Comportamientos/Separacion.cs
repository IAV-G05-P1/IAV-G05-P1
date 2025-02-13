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

        private GameObject[] targets;

        float distance;

        [SerializeField]
        float maxAcceleration;


        public override ComportamientoDireccion GetComportamientoDireccion()
        {
            ComportamientoDireccion result = new ComportamientoDireccion();

            for (int i = 0; i < targets.Length; i++)
            {
                Vector3 direccion = targets[i].transform.position - miTransform.position;
                distance = direccion.magnitude;

                if (distance < umbral)
                {
                    float strength = float.MinValue(decayCoefficient / (distance * distance), maxAcceleration);
                }
            }
            // IMPLEMENTAR separación
            return new ComportamientoDireccion();
        }

        void Start()
        {
            miTransform = transform;
        }
    }
}