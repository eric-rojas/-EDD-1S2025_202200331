using System;
using System.Text;

namespace AutoGestPro.Core
{
    // Definición de la clase Vehículo
    public class Vehiculo
    {
        public int ID { get; set; }
        public int ID_Usuario { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Placa { get; set; }

        public Vehiculo(int id, int idUsuario, string marca, string modelo, string placa)
        {
            ID = id;
            ID_Usuario = idUsuario;
            Marca = marca;
            Modelo = modelo;
            Placa = placa;
        }

        public override string ToString()
        {
            return $"ID: {ID}, ID_Usuario: {ID_Usuario}, Marca: {Marca}, Modelo: {Modelo}, Placa: {Placa}";
        }
    }

    // Clase Nodo para la lista doblemente enlazada
    public class NodoVehiculo
    {
        public Vehiculo Vehiculo { get; set; }
        public NodoVehiculo Siguiente { get; set; }
        public NodoVehiculo Anterior { get; set; }

        public NodoVehiculo(Vehiculo vehiculo)
        {
            Vehiculo = vehiculo;
            Siguiente = null;
            Anterior = null;
        }
    }

    // Lista Doblemente Enlazada
    public class ListaVehiculos
    {
        private NodoVehiculo cabeza;
        private NodoVehiculo cola;
        private ListaUsuarios listaUsuarios; // Referencia a la lista de usuarios para validación

        public ListaVehiculos(ListaUsuarios listaUsuarios)
        {
            cabeza = null;
            cola = null;
            this.listaUsuarios = listaUsuarios;
        }

        // Sobrecarga del constructor para usar sin lista de usuarios (para compatibilidad)
        public ListaVehiculos()
        {
            cabeza = null;
            cola = null;
            this.listaUsuarios = null;
        }

        // Método para verificar si existe un ID de vehículo
        public bool ExisteID(int id)
        {
            NodoVehiculo actual = cabeza;
            while (actual != null)
            {
                if (actual.Vehiculo.ID == id)
                    return true;
                actual = actual.Siguiente;
            }
            return false;
        }

        // Método para insertar con validaciones
        public bool Insertar(Vehiculo vehiculo)
        {
            // Validar que el ID del vehículo sea único
            if (ExisteID(vehiculo.ID))
            {
                Console.WriteLine($"Error: Ya existe un vehículo con el ID {vehiculo.ID}.");
                return false;
            }

            // Validar que el usuario exista
            if (listaUsuarios != null)
            {
                Usuario usuario = listaUsuarios.Buscar(vehiculo.ID_Usuario);
                if (usuario == null)
                {
                    Console.WriteLine($"Error: El usuario con ID {vehiculo.ID_Usuario} no existe en el sistema.");
                    return false;
                }
            }

            // Crear el nuevo nodo
            NodoVehiculo nuevoNodo = new NodoVehiculo(vehiculo);

            // Si la lista está vacía
            if (cabeza == null)
            {
                cabeza = nuevoNodo;
                cola = nuevoNodo;
            }
            else
            {
                // Añadir al final de la lista
                cola.Siguiente = nuevoNodo;
                nuevoNodo.Anterior = cola;
                cola = nuevoNodo;
            }
            return true;
        }

        // Sobrecarga para compatibilidad con el código anterior
        public bool Insertar(int id, int idUsuario, string marca, string modelo, string placa)
        {
            Vehiculo vehiculo = new Vehiculo(id, idUsuario, marca, modelo, placa);
            return Insertar(vehiculo);
        }

        // Buscar un vehículo por ID
        public Vehiculo Buscar(int id)
        {
            NodoVehiculo actual = cabeza;
            while (actual != null)
            {
                if (actual.Vehiculo.ID == id)
                    return actual.Vehiculo;
                actual = actual.Siguiente;
            }
            return null;
        }

        // Buscar vehículos por ID_Usuario
        public List<Vehiculo> BuscarPorUsuario(int idUsuario)
        {
            List<Vehiculo> vehiculosUsuario = new List<Vehiculo>();
            NodoVehiculo actual = cabeza;
            
            while (actual != null)
            {
                if (actual.Vehiculo.ID_Usuario == idUsuario)
                    vehiculosUsuario.Add(actual.Vehiculo);  // Agregamos el objeto Vehiculo, no el ID
                
                actual = actual.Siguiente;
            }
            
            return vehiculosUsuario;  // Retornamos una lista de Vehiculo, no de int
        }

        /*public List<Vehiculo> BuscarPorUsuario(int idUsuario)
        {
            List<Vehiculo> vehiculosUsuario = new List<Vehiculo>();
            NodoVehiculo actual = cabeza;
            
            while (actual != null)
            {
                if (actual.Vehiculo.ID_Usuario == idUsuario)
                    vehiculosUsuario.Add(actual.Vehiculo);
                actual = actual.Siguiente;
            }
            
            return vehiculosUsuario;
        }*/

        // Editar un vehículo
        public bool Editar(int id, int idUsuario, string marca, string modelo, string placa)
        {
            // Validar que el usuario exista
            if (listaUsuarios != null)
            {
                Usuario usuario = listaUsuarios.Buscar(idUsuario);
                if (usuario == null)
                {
                    Console.WriteLine($"Error: El usuario con ID {idUsuario} no existe en el sistema.");
                    return false;
                }
            }

            NodoVehiculo actual = cabeza;
            while (actual != null)
            {
                if (actual.Vehiculo.ID == id)
                {
                    actual.Vehiculo.ID_Usuario = idUsuario;
                    actual.Vehiculo.Marca = marca;
                    actual.Vehiculo.Modelo = modelo;
                    actual.Vehiculo.Placa = placa;
                    return true;
                }
                actual = actual.Siguiente;
            }
            return false;
        }

        // Eliminar un vehículo por ID
        public bool Eliminar(int id)
        {
            if (cabeza == null)
                return false;

            // Si es el primer nodo
            if (cabeza.Vehiculo.ID == id)
            {
                NodoVehiculo temp = cabeza;
                cabeza = cabeza.Siguiente;
                
                if (cabeza != null)
                    cabeza.Anterior = null;
                else
                    cola = null; // Si era el único nodo, la cola también es null
                
                return true;
            }

            // Si es el último nodo
            if (cola.Vehiculo.ID == id)
            {
                cola = cola.Anterior;
                cola.Siguiente = null;
                return true;
            }

            // Si es un nodo intermedio
            NodoVehiculo actual = cabeza;
            while (actual != null && actual.Vehiculo.ID != id)
            {
                actual = actual.Siguiente;
            }

            if (actual == null)
                return false; // No se encontró el vehículo

            // Reconectar los nodos adyacentes
            actual.Anterior.Siguiente = actual.Siguiente;
            actual.Siguiente.Anterior = actual.Anterior;
            
            return true;
        }

        // Mostrar todos los vehículos
        public void Mostrar()
        {
            NodoVehiculo actual = cabeza;
            while (actual != null)
            {
                Console.WriteLine(actual.Vehiculo);
                actual = actual.Siguiente;
            }
        }

        // Mostrar vehículos en orden inverso
        public void MostrarReversa()
        {
            NodoVehiculo actual = cola;
            while (actual != null)
            {
                Console.WriteLine(actual.Vehiculo);
                actual = actual.Anterior;
            }
        }

        // Generar representación gráfica con Graphviz
        public string GenerarGraphviz()
        {
            if (cabeza == null)
            {
                return "digraph G {\n    node [shape=record];\n    NULL [label = \"{NULL}\"];\n}\n";
            }

            StringBuilder dot = new StringBuilder();
            dot.AppendLine("digraph G {");
            dot.AppendLine("    rankdir=LR;");
            dot.AppendLine("    node [shape=record];");
            dot.AppendLine("    subgraph cluster_0 {");
            dot.AppendLine("        label = \"Lista Doblemente Enlazada de Vehículos\";");
            dot.AppendLine("        style=filled;");
            dot.AppendLine("        color=lightgrey;");

            // Crear los nodos
            NodoVehiculo actual = cabeza;
            while (actual != null)
            {
                dot.AppendLine($"        node{actual.Vehiculo.ID} [label=\"{{" +
                    $"ID: {actual.Vehiculo.ID} | " +
                    $"ID Usuario: {actual.Vehiculo.ID_Usuario} | " +
                    $"Marca: {actual.Vehiculo.Marca} | " +
                    $"Modelo: {actual.Vehiculo.Modelo} | " +
                    $"Placa: {actual.Vehiculo.Placa}" +
                    $"}}\"];");
                actual = actual.Siguiente;
            }

            // Crear las conexiones bidireccionales
            actual = cabeza;
            while (actual.Siguiente != null)
            {
                dot.AppendLine($"        node{actual.Vehiculo.ID} -> node{actual.Siguiente.Vehiculo.ID} [dir=both, color=\"blue:red\"];");
                actual = actual.Siguiente;
            }

            dot.AppendLine("    }");
            dot.AppendLine("}");
            return dot.ToString();
        }
    }
}