using System;
using System.IO;

namespace AutoGestPro.Core
{
    // definición del usuario
    public class Usuario
    {   
        // Añadimos Edad a las características del usuario
        public int ID { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Correo { get; set; }
        public int Edad { get; set; }  // Nueva propiedad Edad
        public string Contrasenia { get; set; }  // Renombrado para mantener convención

        public Usuario(int id, string nombres, string apellidos, string correo, int edad, string contrasenia)
        {
            ID = id;
            Nombres = nombres;
            Apellidos = apellidos;
            Correo = correo;
            Edad = edad;  // Inicializamos la nueva propiedad
            Contrasenia = contrasenia;
        }

        public override string ToString()
        {
            return $"ID: {ID}, Nombre: {Nombres} {Apellidos}, Correo: {Correo}, Edad: {Edad}";
        }
    }


    // La clase Nodo
    public class Nodo
    {
        public Usuario Usuario { get; set; }
        public Nodo Siguiente { get; set; }

        public Nodo(Usuario usuario)
        {
            Usuario = usuario;
            Siguiente = null;
        }
    }

    // Iniciamos la Lista Simple Enlazada
    public class ListaUsuarios
    {
        // iniciamos con su cabeza
        private Nodo cabeza;
        // en nuestra lista tenemos a la cabeza como null para inicializar
        public ListaUsuarios()
        {
            cabeza = null;
        }

        // Método para validar que el ID sea único
        public bool ExisteID(int id)
        {
            Nodo actual = cabeza;
            while (actual != null)
            {
                if (actual.Usuario.ID == id)
                    return true;
                actual = actual.Siguiente;
            }
            return false;
        }

        // Método para validar que el correo sea único
        public bool ExisteCorreo(string correo)
        {
            Nodo actual = cabeza;
            while (actual != null)
            {
                if (actual.Usuario.Correo.Equals(correo, StringComparison.OrdinalIgnoreCase))
                    return true;
                actual = actual.Siguiente;
            }
            return false;
        }

        // Insertar un nuevo usuario al final de la lista con validaciones
        public bool Insertar(Usuario usuario)
        {
            // Validar que el ID y correo sean únicos
            if (ExisteID(usuario.ID))
            {
                Console.WriteLine($"Error: Ya existe un usuario con el ID {usuario.ID}.");
                return false;
            }

            if (ExisteCorreo(usuario.Correo))
            {
                Console.WriteLine($"Error: Ya existe un usuario con el correo {usuario.Correo}.");
                return false;
            }

            // creamos un nodo con el usuario proporcionado
            Nodo nuevoNodo = new Nodo(usuario);
            // si nuestra cabeza es nula incertamos en ella un nodo nuevo
            if (cabeza == null)
            {
                cabeza = nuevoNodo;
            }
            // si no esta vacia (nula) entonces nos vamos hasta el final y lo añadimos
            else
            {
                Nodo actual = cabeza;
                while (actual.Siguiente != null)
                {
                    actual = actual.Siguiente;
                }
                actual.Siguiente = nuevoNodo;
            }
            return true;
        }

        // Buscar un usuario por ID
        public Usuario Buscar(int id)
        {
            Nodo actual = cabeza;
            while (actual != null)
            {
                if (actual.Usuario.ID == id)
                    return actual.Usuario;
                actual = actual.Siguiente;
            }
            return null; // Si no se encuentra el usuario
        }

        // Buscar un usuario por correo
        public Usuario BuscarPorCorreo(string correo)
        {
            Nodo actual = cabeza;
            while (actual != null)
            {
                if (actual.Usuario.Correo.Equals(correo, StringComparison.OrdinalIgnoreCase))
                    return actual.Usuario;
                actual = actual.Siguiente;
            }
            return null; // Si no se encuentra el usuario
        }

        // Editar los datos de un usuario por ID con validación de correo único
        public bool Editar(int id, string nuevosNombres, string nuevosApellidos, string nuevoCorreo, int nuevaEdad, string nuevaContrasenia)
        {
            Usuario usuario = Buscar(id);
            if (usuario != null)
            {
                // Verificar si el nuevo correo ya existe (exceptuando el propio usuario)
                Nodo actual = cabeza;
                while (actual != null)
                {
                    if (actual.Usuario.ID != id && 
                        actual.Usuario.Correo.Equals(nuevoCorreo, StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine($"Error: Ya existe otro usuario con el correo {nuevoCorreo}.");
                        return false;
                    }
                    actual = actual.Siguiente;
                }

                usuario.Nombres = nuevosNombres;
                usuario.Apellidos = nuevosApellidos;
                usuario.Correo = nuevoCorreo;
                usuario.Edad = nuevaEdad;
                usuario.Contrasenia = nuevaContrasenia;
                return true;
            }
            return false; // No existe ese id
        }

        // Eliminar un usuario por ID
        public bool Eliminar(int id)
        {
            if (cabeza == null) return false;
            
            if (cabeza.Usuario.ID == id) 
            {
                cabeza = cabeza.Siguiente;
                return true;
            }

            Nodo actual = cabeza;
            while (actual.Siguiente != null && actual.Siguiente.Usuario.ID != id)
            {
                actual = actual.Siguiente;
            }

            if (actual.Siguiente == null) return false; // No encontrado

            actual.Siguiente = actual.Siguiente.Siguiente;
            return true;
        }

        // Mostrar todos los usuarios
        public void Mostrar()
        {
            Nodo actual = cabeza;
            while (actual != null)
            {
                Console.WriteLine(actual.Usuario);
                actual = actual.Siguiente;
            }
        }

        // Generar visualización Graphviz
        public string GenerarGraphviz()
        {
            if (cabeza == null)
            {
                return "digraph G {\n    node [shape=record];\n    NULL [label = \"{NULL}\"];\n}\n";
            }

            var graphviz = "digraph G {\n";
            graphviz += "    node [shape=record];\n";
            graphviz += "    rankdir=LR;\n";
            graphviz += "    subgraph cluster_0 {\n";
            graphviz += "        label = \"Lista Simple de Usuarios\";\n";

            Nodo actual = cabeza;
            int index = 0;
            while (actual != null)
            {
                graphviz += $"        n{index} [label = \"{{<data> ID: {actual.Usuario.ID} \\n" +
                        $"Nombres: {actual.Usuario.Nombres} \\n" +
                        $"Apellidos: {actual.Usuario.Apellidos} \\n" +
                        $"Correo: {actual.Usuario.Correo} \\n" +
                        $"Edad: {actual.Usuario.Edad} \\n" +
                        $"Contrasenia: {actual.Usuario.Contrasenia} \\n" +
                        $"Siguiente: }}\"];\n";
                actual = actual.Siguiente;
                index++;
            }

            actual = cabeza;
            for (int i = 0; actual != null && actual.Siguiente != null; i++)
            {
                graphviz += $"        n{i} -> n{i + 1};\n";
                actual = actual.Siguiente;
            }

            graphviz += "    }\n";
            graphviz += "}\n";
            return graphviz;
        }
    }
}