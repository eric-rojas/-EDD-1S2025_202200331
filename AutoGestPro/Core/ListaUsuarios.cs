using System;
using System.IO;

namespace AutoGestPro.Core /* madres esto es importante, estaba haciendo pruebas basicas en consola y no me reconocía este archivo porque le faltaba el 
namespace y no jalaba nada, me decia q no habia nada en Core. entonces es importante decirle que el archivo pertenece a que carpetas y en el main poner una
importacion algo asi: using AutoGestPro.Core; jajaj bueno algo q si ayuda el chatsito. 
Bueno ahora sigan con la lectura :)
*/
{
    // definición del usuario
    public class Usuario
    {   
        // tenemos sus 5 diferentes caracteristicas
        public int ID { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Correo { get; set; }
        public string Contraseña { get; set; }

        public Usuario(int id, string nombres, string apellidos, string correo, string contraseña)
        {
            ID = id;
            Nombres = nombres;
            Apellidos = apellidos;
            Correo = correo;
            Contraseña = contraseña;
        }

        public override string ToString()
        {
            return $"ID: {ID}, Nombre: {Nombres} {Apellidos}, Correo: {Correo}";
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

        // Insertar un nuevo usuario al final de la lista
        public void Insertar(Usuario usuario)
        {
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
        }

    // estos los estaremos buscando por el ID de los usuarios
        // Buscar un usuario por ID
        public Usuario Buscar(int id)
        {
            // nuestro apuntador será la el nodo actual, Mientras este apuntador (actual = cabeza) no sea vacío entonces 
            Nodo actual = cabeza;
            while (actual != null)
            {
                // si el actual es identico al id que proporcionamos entonces retornamos su valor
                if (actual.Usuario.ID == id)
                    return actual.Usuario;
                actual = actual.Siguiente; // si no es identico nos vamos al siguiente
            }
            return null; // Si no se encuentra el usuario
        }

        // Editar los datos de un usuario por ID
        public bool Editar(int id, string nuevosNombres, string nuevosApellidos, string nuevoCorreo, string nuevaContraseña)
        {
            Usuario usuario = Buscar(id); // creamos a usuario en base al id que obtenemos de la funcion Buscar
            if (usuario != null) // si este usuario es diferente a null o vació entonces podemos ingresar un usuario.nombre este ultimo parte de la clase usuario antes creada
            {
                usuario.Nombres = nuevosNombres;
                usuario.Apellidos = nuevosApellidos;
                usuario.Correo = nuevoCorreo;
                usuario.Contraseña = nuevaContraseña;
                return true;
            }
            return false; // Sino entonces no existe ese id
        }

        // Eliminar un usuario por ID
        public bool Eliminar(int id)
        {
            // si la lista esta vacia no eliminamos nada... salimos de esta función 
            if (cabeza == null) return false;
            
            // Si es el primer nodo entonces movemos la cabeza al siguiente nodo y lo saltamos... asi es un nodo sin conección y no existe
            // entonces si se desconecta ese nodo no se puede acceder a el, eso se toma como eliminado 
            if (cabeza.Usuario.ID == id) 
            {
                cabeza = cabeza.Siguiente;
                return true;
            }

            // si el nodo no es el primero entonces lo buscamos
            Nodo actual = cabeza;
            while (actual.Siguiente != null && actual.Siguiente.Usuario.ID != id) // mientras el acual.siguiente no sea nulo y el id no sea el mismo entonces...
            {
                actual = actual.Siguiente; //... entonces pasamos al siguiente.
            } // asi hasta que lo encontremos y salimos del While

            if (actual.Siguiente == null) return false; // No encontrado

            // Si encontramos el nodo, lo "saltamos" asignando el nodo siguiente del nodo encontrado (actual.Siguiente.Siguiente) al nodo actual (actual.Siguiente). Esto desconecta el nodo con el ID que queremos eliminar.
            actual.Siguiente = actual.Siguiente.Siguiente; // En palabras mas simples, si lo encontramos entonces este lo saltamos y vale keso ese id, se queda sin siguiente y  se queda solito y pobechito se muere xd
            return true;
        }

        // Mostrar todos los usuarios
        public void Mostrar() // pos aqui mostramos todo asi q no explication, gracias!!
        {
            Nodo actual = cabeza;
            while (actual != null)
            {
                Console.WriteLine(actual.Usuario);
                actual = actual.Siguiente;
            }
        }

        /*
        public void GenerarDOT(string ruta)
        {

            using (StreamWriter writer = new StreamWriter(ruta))
            {
                writer.WriteLine("digraph G {");
                writer.WriteLine("rankdir=LR;");
                Nodo actual = cabeza;
                while (actual != null)
                {
                    if (actual.Siguiente != null)
                    {
                        writer.WriteLine($"    \"{actual.Usuario.ID}\" -> \"{actual.Siguiente.Usuario.ID}\";");
                    }
                    else
                    {
                        writer.WriteLine($"    \"{actual.Usuario.ID}\";");
                    }
                    actual = actual.Siguiente;
                }
                writer.WriteLine("}");
            }
        }
        */

        
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
                        $"Contraseña: {actual.Usuario.Contraseña} \\n" +
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