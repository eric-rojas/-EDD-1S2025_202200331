using System;
using System.Collections.Generic;

namespace AutoGestPro.Core
{
    // Definición de la clase Servicio
    public class Servicio
    {
        public int ID { get; set; }
        public int ID_Repuesto { get; set; }
        public int ID_Vehiculo { get; set; }
        public string Detalles { get; set; }
        public double Costo { get; set; }

        public Servicio(int id, int idRepuesto, int idVehiculo, string detalles, double costo)
        {
            ID = id;
            ID_Repuesto = idRepuesto;
            ID_Vehiculo = idVehiculo;
            Detalles = detalles;
            Costo = costo;
        }

        public override string ToString()
        {
            return $"ID: {ID}, ID_Repuesto: {ID_Repuesto}, ID_Vehiculo: {ID_Vehiculo}, Detalles: {Detalles}, Costo: {Costo:C}";
        }
    }

    // Clase Nodo para el árbol binario
    public class NodoServicio
    {
        public Servicio Servicio { get; set; }
        public NodoServicio Izquierda { get; set; }
        public NodoServicio Derecha { get; set; }

        public NodoServicio(Servicio servicio)
        {
            Servicio = servicio;
            Izquierda = null;
            Derecha = null;
        }
    }

    // Implementación del árbol binario de servicios
    public class ArbolBinarioServicios
    {
        private NodoServicio raiz;
        private ArbolAVLRepuestos repuestos; // Referencia al árbol de repuestos
        private ListaVehiculos vehiculos;   // Referencia a la lista de vehículos

        public ArbolBinarioServicios(ArbolAVLRepuestos repuestos, ListaVehiculos vehiculos)
        {
            raiz = null;
            this.repuestos = repuestos;
            this.vehiculos = vehiculos;
        }

        // Sobrecarga del constructor para usar sin referencias (para compatibilidad)
        public ArbolBinarioServicios()
        {
            raiz = null;
            this.repuestos = null;
            this.vehiculos = null;
        }

        // Verificar si existe un ID en el árbol
        public bool ExisteID(int id)
        {
            return BuscarPorID(id) != null;
        }

        // Insertar un servicio con validaciones
        public bool Insertar(Servicio servicio)
        {
            // Validar que el ID sea único
            if (ExisteID(servicio.ID))
            {
                Console.WriteLine($"Error: Ya existe un servicio con el ID {servicio.ID}.");
                return false;
            }

            // Validar que el repuesto exista
            if (repuestos != null && !repuestos.ExisteID(servicio.ID_Repuesto))
            {
                Console.WriteLine($"Error: El repuesto con ID {servicio.ID_Repuesto} no existe en el sistema.");
                return false;
            }

            // Validar que el vehículo exista
            if (vehiculos != null && !vehiculos.ExisteID(servicio.ID_Vehiculo))
            {
                Console.WriteLine($"Error: El vehículo con ID {servicio.ID_Vehiculo} no existe en el sistema.");
                return false;
            }

            // Si pasa todas las validaciones, insertar el servicio
            raiz = InsertarRecursivo(raiz, servicio);
            return true;
        }

        // Método auxiliar para inserción recursiva
        private NodoServicio InsertarRecursivo(NodoServicio actual, Servicio servicio)
        {
            // Si el nodo actual es null, creamos un nuevo nodo
            if (actual == null)
            {
                return new NodoServicio(servicio);
            }

            // Si el ID es menor, vamos al subárbol izquierdo
            if (servicio.ID < actual.Servicio.ID)
            {
                actual.Izquierda = InsertarRecursivo(actual.Izquierda, servicio);
            }
            // Si el ID es mayor, vamos al subárbol derecho
            else if (servicio.ID > actual.Servicio.ID)
            {
                actual.Derecha = InsertarRecursivo(actual.Derecha, servicio);
            }
            // Si el ID ya existe, no hacemos nada (ya validamos antes)
            return actual;
        }

        // Buscar un servicio por ID
        public Servicio BuscarPorID(int id)
        {
            return BuscarRecursivo(raiz, id);
        }

        // Método auxiliar para búsqueda recursiva
        private Servicio BuscarRecursivo(NodoServicio nodo, int id)
        {
            // Si el nodo es null o encontramos el ID, terminamos
            if (nodo == null)
            {
                return null;
            }
            
            if (id == nodo.Servicio.ID)
            {
                return nodo.Servicio;
            }
            
            // Buscamos en el subárbol izquierdo o derecho según el ID
            return id < nodo.Servicio.ID
                ? BuscarRecursivo(nodo.Izquierda, id)
                : BuscarRecursivo(nodo.Derecha, id);
        }

        // Recorrido en Orden (Inorder)
        public void Mostrar()
        {
            List<Servicio> servicios = RecorridoInOrden();
            foreach (var servicio in servicios)
            {
                Console.WriteLine(servicio);
            }
        }

        // Recorrido InOrden (Izquierda-Raíz-Derecha)
        public List<Servicio> RecorridoInOrden()
        {
            List<Servicio> resultado = new List<Servicio>();
            InOrdenRecursivo(raiz, resultado);
            return resultado;
        }

        private void InOrdenRecursivo(NodoServicio nodo, List<Servicio> resultado)
        {
            if (nodo != null)
            {
                InOrdenRecursivo(nodo.Izquierda, resultado);
                resultado.Add(nodo.Servicio);
                InOrdenRecursivo(nodo.Derecha, resultado);
            }
        }

        // Editar un servicio
        public bool Editar(int id, int idRepuesto, int idVehiculo, string detalles, double costo)
        {
            // Validar que el servicio exista
            if (!ExisteID(id))
            {
                Console.WriteLine($"Error: No existe un servicio con el ID {id}.");
                return false;
            }

            // Validar que el repuesto exista
            if (repuestos != null && !repuestos.ExisteID(idRepuesto))
            {
                Console.WriteLine($"Error: El repuesto con ID {idRepuesto} no existe en el sistema.");
                return false;
            }

            // Validar que el vehículo exista
            if (vehiculos != null && !vehiculos.ExisteID(idVehiculo))
            {
                Console.WriteLine($"Error: El vehículo con ID {idVehiculo} no existe en el sistema.");
                return false;
            }

            // Buscar el servicio y actualizar sus datos
            Servicio servicio = BuscarPorID(id);
            if (servicio != null)
            {
                servicio.ID_Repuesto = idRepuesto;
                servicio.ID_Vehiculo = idVehiculo;
                servicio.Detalles = detalles;
                servicio.Costo = costo;
                return true;
            }
            
            return false;
        }

        // Buscar servicios por ID de vehículo
        public List<Servicio> BuscarPorVehiculo(int idVehiculo)
        {
            List<Servicio> resultado = new List<Servicio>();
            BuscarPorVehiculoRecursivo(raiz, idVehiculo, resultado);
            return resultado;
        }

        private void BuscarPorVehiculoRecursivo(NodoServicio nodo, int idVehiculo, List<Servicio> resultado)
        {
            if (nodo == null)
                return;

            if (nodo.Servicio.ID_Vehiculo == idVehiculo)
                resultado.Add(nodo.Servicio);

            BuscarPorVehiculoRecursivo(nodo.Izquierda, idVehiculo, resultado);
            BuscarPorVehiculoRecursivo(nodo.Derecha, idVehiculo, resultado);
        }

        // Buscar servicios por ID de repuesto
        public List<Servicio> BuscarPorRepuesto(int idRepuesto)
        {
            List<Servicio> resultado = new List<Servicio>();
            BuscarPorRepuestoRecursivo(raiz, idRepuesto, resultado);
            return resultado;
        }

        private void BuscarPorRepuestoRecursivo(NodoServicio nodo, int idRepuesto, List<Servicio> resultado)
        {
            if (nodo == null)
                return;

            if (nodo.Servicio.ID_Repuesto == idRepuesto)
                resultado.Add(nodo.Servicio);

            BuscarPorRepuestoRecursivo(nodo.Izquierda, idRepuesto, resultado);
            BuscarPorRepuestoRecursivo(nodo.Derecha, idRepuesto, resultado);
        }

        // Eliminar un servicio por ID
        public bool Eliminar(int id)
        {
            // Validar que exista
            if (!ExisteID(id))
            {
                Console.WriteLine($"Error: No existe un servicio con el ID {id}.");
                return false;
            }

            raiz = EliminarRecursivo(raiz, id);
            return true;
        }

        // Método auxiliar para eliminación recursiva
        private NodoServicio EliminarRecursivo(NodoServicio nodo, int id)
        {
            if (nodo == null)
                return null;

            // Buscar el nodo a eliminar
            if (id < nodo.Servicio.ID)
            {
                nodo.Izquierda = EliminarRecursivo(nodo.Izquierda, id);
            }
            else if (id > nodo.Servicio.ID)
            {
                nodo.Derecha = EliminarRecursivo(nodo.Derecha, id);
            }
            else
            {
                // Nodo encontrado, procedemos a eliminarlo
                
                // Caso 1: Nodo hoja (sin hijos)
                if (nodo.Izquierda == null && nodo.Derecha == null)
                    return null;

                // Caso 2: Nodo con un solo hijo
                if (nodo.Izquierda == null)
                    return nodo.Derecha;
                if (nodo.Derecha == null)
                    return nodo.Izquierda;

                // Caso 3: Nodo con dos hijos
                // Encontrar el sucesor inorden (mínimo del subárbol derecho)
                NodoServicio sucesor = EncontrarMinimo(nodo.Derecha);
                // Copiar los datos del sucesor al nodo actual
                nodo.Servicio = sucesor.Servicio;
                // Eliminar el sucesor
                nodo.Derecha = EliminarRecursivo(nodo.Derecha, sucesor.Servicio.ID);
            }
            return nodo;
        }

        // Método auxiliar para encontrar el nodo con valor mínimo
        private NodoServicio EncontrarMinimo(NodoServicio nodo)
        {
            while (nodo.Izquierda != null)
                nodo = nodo.Izquierda;
            return nodo;
        }

        // Calcular costo total de todos los servicios
        public double CalcularCostoTotal()
        {
            return CalcularCostoTotalRecursivo(raiz);
        }

        private double CalcularCostoTotalRecursivo(NodoServicio nodo)
        {
            if (nodo == null)
                return 0;

            return nodo.Servicio.Costo + 
                   CalcularCostoTotalRecursivo(nodo.Izquierda) + 
                   CalcularCostoTotalRecursivo(nodo.Derecha);
        }
        public string GenerarGraphviz()
        {
            using (StringWriter writer = new StringWriter())
            {
                writer.WriteLine("digraph ArbolServicios {");
                writer.WriteLine("node [shape=record];");
                
                if (raiz != null)
                {
                    GenerarGraphvizRecursivo(raiz, writer);
                }
                else
                {
                    writer.WriteLine("empty [label=\"Árbol vacío\", shape=plaintext];");
                }
                
                writer.WriteLine("}");
                return writer.ToString();
            }
        }

        // Recorrido PreOrden
        public List<Servicio> RecorridoPreOrden()
        {
            List<Servicio> resultado = new List<Servicio>();
            PreOrdenRecursivo(raiz, resultado);
            return resultado;
        }

        private void PreOrdenRecursivo(NodoServicio nodo, List<Servicio> resultado)
        {
            if (nodo != null)
            {
                resultado.Add(nodo.Servicio);
                PreOrdenRecursivo(nodo.Izquierda, resultado);
                PreOrdenRecursivo(nodo.Derecha, resultado);
            }
        }

        // Recorrido PostOrden
        public List<Servicio> RecorridoPostOrden()
        {
            List<Servicio> resultado = new List<Servicio>();
            PostOrdenRecursivo(raiz, resultado);
            return resultado;
        }

        private void PostOrdenRecursivo(NodoServicio nodo, List<Servicio> resultado)
        {
            if (nodo != null)
            {
                PostOrdenRecursivo(nodo.Izquierda, resultado);
                PostOrdenRecursivo(nodo.Derecha, resultado);
                resultado.Add(nodo.Servicio);
            }
        }

        // Obtener servicios de un usuario
        public List<Servicio> ObtenerServiciosPorUsuario(int idUsuario, ListaVehiculos vehiculos)
        {
            List<Vehiculo> vehiculosUsuario = vehiculos.BuscarPorUsuario(idUsuario);  // Obtener la lista de Vehiculo
            List<Servicio> resultado = new List<Servicio>();

            foreach (Vehiculo vehiculo in vehiculosUsuario)  // Iterar sobre la lista de objetos Vehiculo
            {
                resultado.AddRange(BuscarPorVehiculo(vehiculo.ID));  // Usar vehiculo.ID en lugar de un entero directo
            }

            return resultado;
        }



        private void GenerarGraphvizRecursivo(NodoServicio nodo, TextWriter writer)
        {
            if (nodo == null)
                return;

            // Crear etiqueta con información del servicio
            string nodeLabel = string.Format("{{ID: {0}|ID_Rep: {1}|ID_Veh: {2}|Det: {3}|Costo: {4:C}}}",
                nodo.Servicio.ID,
                nodo.Servicio.ID_Repuesto,
                nodo.Servicio.ID_Vehiculo,
                nodo.Servicio.Detalles,
                nodo.Servicio.Costo);
            
            // Escribir definición del nodo
            writer.WriteLine($"node{nodo.Servicio.ID} [label=\"{nodeLabel}\"];");
            
            // Agregar conexiones con hijos
            if (nodo.Izquierda != null)
            {
                writer.WriteLine($"node{nodo.Servicio.ID} -> node{nodo.Izquierda.Servicio.ID} [label=\"I\"];");
                GenerarGraphvizRecursivo(nodo.Izquierda, writer);
            }
            /*else
            {
                // Nodo nulo izquierdo (opcional para visualización)
                writer.WriteLine($"nullL{nodo.Servicio.ID} [label=\"NIL\", shape=point];");
                writer.WriteLine($"node{nodo.Servicio.ID} -> nullL{nodo.Servicio.ID} [label=\"I\"];");
            }*/
            
            if (nodo.Derecha != null)
            {
                writer.WriteLine($"node{nodo.Servicio.ID} -> node{nodo.Derecha.Servicio.ID} [label=\"D\"];");
                GenerarGraphvizRecursivo(nodo.Derecha, writer);
            }
            /*else
            {
                // Nodo nulo derecho (opcional para visualización)
                writer.WriteLine($"nullR{nodo.Servicio.ID} [label=\"NIL\", shape=point];");
                writer.WriteLine($"node{nodo.Servicio.ID} -> nullR{nodo.Servicio.ID} [label=\"D\"];");
            }*/
        }
    }

}