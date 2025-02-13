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

El problema que se nos presenta a solucionar, es crear diferentes comportamientos para los agentes, ya sea Huir, Merodear, Separarse unos de otros o Seguir, que son lo único que hemos visto que haya que implementar realmente en la plantilla.
También es necesario implementar la Percepción de uno de los agentes, en este caso el Perro, que deberá de seguir al agente del Jugador o huir de las Ratas.

Para ello hemos empezado a desarrollar diferentes ideas, en forma de Pseudocódigo que luego se implementarán en el códico final con posibles cambios.

### Seguimiento al jugador
 #Clase para modelar el comportamiento de SEGUIR a otro agente

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

 #Clase para modelar el comportamiento de DEAMBULAR a otro agente

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
 #Clase para modelar el comportamiento de HUIR a otro agente

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
