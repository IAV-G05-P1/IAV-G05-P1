/*    
   Copyright (C) 2020-2025 Federico Peinado
   http://www.federicopeinado.com

   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

   Autor: Federico Peinado 
   Contacto: email@federicopeinado.com
*/
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.TextCore.Text;

namespace UCM.IAV.Movimiento
{
    /// <summary>
    /// Clase para modelar el comportamiento de SEGUIR a otro agente
    /// </summary>
    public class Llegada : ComportamientoAgente
    {
        /// <summary>
        /// Obtiene la dirección
        /// </summary>
        /// <returns></returns>


        // El radio para llegar al objetivo
        public float rObjetivo;

        // El radio en el que se empieza a ralentizarse
        public float rRalentizado;

        public float fRalentizado;

        public int distancia = 7;

        // El tiempo en el que conseguir la aceleracion objetivo
        float timeToTarget = 0.1f;

        public override ComportamientoDireccion GetComportamientoDireccion()
        {
            ComportamientoDireccion result = new ComportamientoDireccion();
            Vector3 direccion = objetivo.transform.position - miTransform.position;

            if (direccion.magnitude < distancia)
            {
                return result;
            }

            float velObjetivo;

            if (direccion.magnitude > rRalentizado)
            {
                velObjetivo = agente.velocidadMax;
            } 
            else
            {
                velObjetivo = agente.velocidadMax * direccion.magnitude / rRalentizado;
            }

            direccion.Normalize();
            direccion *= velObjetivo;

            Vector3 aceleracion = direccion - agente.velocidad;
            direccion /= timeToTarget;

            if (aceleracion.magnitude > agente.aceleracionMax)
            {
                aceleracion.Normalize();
                aceleracion *= agente.aceleracionMax;
            }

            result.lineal = aceleracion;

            return result;
        }

        void Start()
        {
            miTransform = transform;
        }

    }
}
