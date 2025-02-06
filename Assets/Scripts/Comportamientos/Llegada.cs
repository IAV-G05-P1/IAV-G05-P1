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
            result.lineal = objetivo.transform.position - miTransform.position;

            if (result.lineal.magnitude < distancia)
            {
                result = new ComportamientoDireccion();
                return result;
            }
            result.lineal /= timeToTarget;

            if (result.lineal.magnitude < agente.velocidadMax)
            {
                result.lineal.Normalize();
                result.lineal *= agente.velocidadMax;
            }

            //Quaternion paco = Quaternion.Euler(result.lineal.x, result.lineal.y, result.lineal.z);
            miTransform.rotation = Quaternion.LookRotation(result.lineal, Vector3.up);
                //Quaternion.Lerp(miTransform.rotation, paco, 10 * Time.deltaTime);

            return result;
        }

        void Start()
        {
            miTransform = transform;
        }

    }
}
