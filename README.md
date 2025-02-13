# Hamelin - Base
Proyecto de videojuego actualizado a **Unity 2022.3.57f1** diseñador para servir como punto de partida en algunas prácticas.

Consiste en un entorno virtual 3D que representa el pueblo de Hamelín, un personaje controlable por el jugador que es el flautista de Hamelín, un perro compañero y un montón de ratas preparadas para controlarse con IA.

## Licencia
Federico Peinado, autor de la documentación, código y recursos de este trabajo, concedo permiso permanente a los alumnos de la Facultad de Informática de la Universidad Complutense de Madrid para utilizar este material, con sus comentarios y evaluaciones, con fines educativos o de investigación; ya sea para obtener datos agregados de forma anónima como para utilizarlo total o parcialmente reconociendo expresamente mi autoría.

## Referencias
Los recursos de terceros utilizados son de uso público.
* *AI for Games*, Ian Millington.
* [Kaykit Medieval Builder Pack](https://kaylousberg.itch.io/kaykit-medieval-builder-pack)
* [Kaykit Dungeon](https://kaylousberg.itch.io/kaykit-dungeon)
* [Kaykit Animations](https://kaylousberg.itch.io/kaykit-animations)

## Pseudocódigo (diseño y desarrollo de software)

El problema que se nos presenta principalmente a solucionar, es crear diferentes comportamientos para los agentes, ya sea Huir de un agente, Merodear, Separarse unos de otros o Seguir a un agente, que son lo único que hemos visto que haya que implementar realmente en la plantilla en cuestión a la IA.

También es necesario implementar la Percepción de uno de los agentes, en este caso el Perro, que deberá de seguir al agente del Jugador o huir de las Ratas.

Para ello hemos empezado a desarrollar diferentes ideas, en forma de Pseudocódigo que luego se implementarán en el códico final con posibles cambios. Todas las clases heredarán de ComportamientoAgente, puesto que ahí se gestiona el cómo se están moviendo los agentes y varios parámetros y cosas de la clase los vamos a necesitar.

### Seguimiento al jugador
 #Clase para modelar el comportamiento de SEGUIR a otro agente. En el caso del Perro, este seguirá al jugador si no encuentra a una rata cerca de él, y en el caso de las ratas, cuando oigan la flauta, estas irán tras el jugador hasta que deje de tocar o salgan del rando de audición del sonido de la flauta.
 La gestión de cuando las ratas oyen o no la flauta viene dentro de la clase TocarFlauta que ya venía implementada.

    class Llegada : extends ComportamientoAgente:

        #Declaracfion de variables publicas
        rObjetivo:float
        rRalentizado:float
        fRalentizado:float
        distancia:int
        timeToTarget:float
        miTransform:Transform #el transform del personaje que use Llegada

        #Metodos publicos de override
        function GetComportamientoDireccion() -> ComportamientoDireccion:

            result:ComportamientoDireccion = new ComportamientoDireccion()
            direccion:Vector3 = objetivo.transform.position - miTransform.position

            if direccion.magnitude < distancia:
                return result

            velObjetivo:float

            if direccion.magnitude > rRalentizado:
                velObjetivo = agente.velocidadMax
            else:
                velObjetivo = agente.velocidadMax * direccion.magnitude / rRalentizado
            
            direccion.Normalize()
            direccion = direccion * velObjetivo

            aceleracion:Vector3 = direccion - agente.velocidad
            direccion = direccion / timeToTarget

            if aceleracion.magnitude > agente.aceleracionMax:
                aceleracion.Normalize()
                aceleracion = aceleracion * agente.aceleracionMax

            result.lineal = aceleracion

            return result

        #void Start()
        function Start():
            miTransform = transform


### Comportamiendo del merodeo de las ratas

 #Clase para modelar el comportamiento de DEAMBULAR a otro agente. Pensado para que las ratas, cuando no estén escuchando la música de la flauta, se pongan a deambular por el escenario hasta que oigan la flauta.
 Como se ha mencionado antes, la gestión de cuando las ratas oyen o no la flauta viene dentro de la clase TocarFlauta que ya venía implementada.

    class Merodear : extends ComportamientoAgente
    
        #variables que se van a editar desde el editor con SerializeField
            tiempoMaximo:float
            tiempoMinimo:float

        #Variables publicas
            t:float
            actualT:float

            lastDir:ComportamientoDireccion = new ComportamientoDireccion()

        #Metodos publicos de override
        function GetComportamientoDireccion() -> ComportamientoDireccion:
            
            #IMPLEMENTAR merodear
            CompDir:ComportamientoDireccion = new ComportamientoDireccion()
            CompDir.lineal = agente.velocidad


            if actualT >= t:
                CompDir.lineal = new Vector3(Random.RandomRange(-2,2), Random.RandomRange(-2, 2), Random.RandomRange(-2, 2)).normalized
                Debug.Log(CompDir.lineal.ToString())
                t = Random.RandomRange(tiempoMinimo,tiempoMaximo)   
                actualT = 0

            #FINAL DE LA IMPLEMENTAION
            actualT = actualT + Time.deltaTime 
            return CompDir


### Comportamiento de huida
 #Clase para modelar el comportamiento de HUIR a otro agente. Pensado para que cuando el Pero esté cerca de alguna Rata, este deje de seguir al jugador y salga corriendo en dirección contraria.

    class Huir : extends ComportamientoAgente

        #Metodos publicos de override
        function GetComportamientoDireccion() -> ComportamientoDireccion:
            resultado:ComportamientoDireccion = new ComportamientoDireccion()

            resultado.lineal = miTransform.position - objetivo.transform.position

            resultado.lineal.Normalize()
            resultado.lineal = resultado.lineal * agente.aceleracionMax

            return resultado

        #void Start()
        function Start():
            miTransform = transform

### Separación entre agentes en grupo
 #Clase para modelar el comportamiento de SEPARACIÓN en un grupo de agentes para que haya un cierto espacio entre estos y no estén pegados. Cuando las ratas están en grupo, estas deben de dejar un espacio entre ellas, para eso sirve esta clase.

    class Separacion : extends ComportamientoAgente

        #Variables públicas
        targEmpty:GameObject
        targets:List<GameObject> = new List<GameObject>()
        distance:float
        maxAcceleration:float
        radio:float

        #Variables privadas
        trigger:SphereCollider

        #variables que se van a editar desde el editor con SerializeField
        umbral:float
        decayCoefficient:float

        #Metodos publicos de override
        function GetComportamientoDireccion() -> ComportamientoDireccion:
        
            result:ComportamientoDireccion = new ComportamientoDireccion()

            foreach rat:GameObject in targets:
                direccion:Vector3 = miTransform.position - rat.transform.position
                distance = direccion.magnitude

                if distance < umbral:
                    strength:float = Mathf.Min(decayCoefficient / (distance * distance), maxAcceleration)
                    direccion.Normalize()
                    result.lineal = result.lineal + strength * direccion
            
            return result
        

        #Metodos privados
        function OnTriggerEnter(ratColl:Collider):
            ratComp:Merodear = ratColl.gameObject.GetComponent<Merodear>()
            if ratComp != null && !targets.Contains(ratColl.gameObject):
                targets.Add(ratColl.gameObject)

        function OnTriggerExit(ratColl:Collider):
            ratComp:Merodear = ratColl.gameObject.GetComponent<Merodear>()
            if ratComp != null && targets.Contains(ratColl.gameObject):
                targets.Remove(ratColl.gameObject)

        #void Start()
        function Start():
            miTransform = transform
            maxAcceleration = miTransform.gameObject.GetComponent<Agente>().aceleracionMax

            trigger = transform.gameObject.AddComponent<SphereCollider>()
            trigger.isTrigger = true
            trigger.enabled = true
            trigger.radius = radio