using Gtk;
using System;
using AutoGestPro.Core; 

namespace AutoGestPro
{
    class Program
    {
        static void Main(string[] args)
        {
            // Crear la lista de usuarios
            ListaUsuarios listaUsuarios = new ListaUsuarios();

            // Crear usuarios de prueba
            Usuario usuario1 = new Usuario(1, "Juan", "Pérez", "juan@gmail.com", "pass123");
            Usuario usuario2 = new Usuario(2, "Ana", "Gómez", "ana@gmail.com", "pass456");
            Usuario usuario3 = new Usuario(3, "Carlos", "López", "carlos@gmail.com", "pass789");

            // Insertar usuarios en la lista
            listaUsuarios.Insertar(usuario1);
            listaUsuarios.Insertar(usuario2);
            listaUsuarios.Insertar(usuario3);

            // Mostrar todos los usuarios
            Console.WriteLine("Usuarios después de la inserción:");
            listaUsuarios.Mostrar();

            // Buscar un usuario por ID
            Console.WriteLine("\nBuscando usuario con ID 2:");
            Usuario usuarioEncontrado = listaUsuarios.Buscar(2);
            Console.WriteLine(usuarioEncontrado != null ? usuarioEncontrado : "Usuario no encontrado.");

            // Editar un usuario
            Console.WriteLine("\nEditando usuario con ID 2...");
            bool editado = listaUsuarios.Editar(2, "Ana María", "Gómez", "ana_maria@gmail.com", "newpass123");
            Console.WriteLine(editado ? "Usuario editado exitosamente." : "No se encontró el usuario.");

            // Mostrar todos los usuarios después de la edición
            Console.WriteLine("\nUsuarios después de la edición:");
            listaUsuarios.Mostrar();

            // Eliminar un usuario
            Console.WriteLine("\nEliminando usuario con ID 1...");
            bool eliminado = listaUsuarios.Eliminar(1);
            Console.WriteLine(eliminado ? "Usuario eliminado exitosamente." : "No se encontró el usuario.");

            // Mostrar todos los usuarios después de la eliminación
            Console.WriteLine("\nUsuarios después de la eliminación:");
            listaUsuarios.Mostrar();




            Console.WriteLine("\n ------------------------------------");
            ListaVehiculos listaVehiculos = new ListaVehiculos();

            // Insertar vehículos
            listaVehiculos.Insertar(1, "Toyota", "Corolla", "ABC123");
            listaVehiculos.Insertar(2, "Ford", "Focus", "XYZ456");

            // Mostrar lista de vehículos
            Console.WriteLine("Vehículos registrados:");
            listaVehiculos.Mostrar();

            // Mostrar lista en reversa
            Console.WriteLine("\nVehículos registrados (en reversa):");
            listaVehiculos.MostrarReversa();

            // Eliminar un vehículo
            listaVehiculos.Eliminar(1);

            // Mostrar lista de vehículos después de eliminar
            Console.WriteLine("\nVehículos después de eliminar el ID 1:");
            listaVehiculos.Mostrar();
            
        }
    }
}
