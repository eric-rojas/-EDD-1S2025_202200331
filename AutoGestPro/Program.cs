/*
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

            
            listaVehiculos.Insertar(1, 23, "Toyota", "Corolla", "ABC123");
            listaVehiculos.Insertar(2, 40, "Ford", "Focus", "XYZ456");

            
            Console.WriteLine("Vehículos registrados:");
            listaVehiculos.Mostrar();

            
            Console.WriteLine("\nVehículos registrados (en reversa):");
            listaVehiculos.MostrarReversa();

            
            listaVehiculos.Eliminar(1);

            
            Console.WriteLine("\nVehículos después de eliminar el ID 1:");
            listaVehiculos.Mostrar();


 
            Console.WriteLine("\n ------------------------------------"); 
 
            ListaRepuestos lista = new ListaRepuestos();
            
            lista.Insertar(1, "Filtro de aceite", "Filtro para motor 1.6L", 15.99);
            lista.Insertar(2, "Bujía", "Bujía de iridio para alto rendimiento", 9.50);
            lista.Insertar(3, "Pastillas de freno", "Pastillas cerámicas delanteras", 45.75);
            
            Console.WriteLine("Lista de repuestos:");
            lista.Mostrar();
            
            Console.WriteLine("\nEliminando el repuesto con ID 2...");
            lista.Eliminar(2);
            
            Console.WriteLine("\nLista después de la eliminación:");
            lista.Mostrar();

            Console.WriteLine("\n ------------------------------------"); 
            ColaServicios cola = new ColaServicios();

            cola.Encolar(2, 21, 20, "respuesto", 20);
            cola.Encolar(5, 25, 3, "respuesto2", 60);
            cola.Encolar(52, 45, 52, "respuesto3", 50);
            cola.Encolar(23, 32, 234, "respuesto4", 230);

            Console.WriteLine("\n cola de Servicios");

            Console.WriteLine("\n encolar");
            cola.Mostrar();
            
            Console.WriteLine("\n desencolar1");
            cola.Desencolar();
            cola.Mostrar();
            
            Console.WriteLine("\n desencolar2");
            cola.Desencolar();
            cola.Mostrar();
            
            Console.WriteLine("\n desencolar3");
            cola.Desencolar();
            cola.Mostrar();

            Console.WriteLine("\n desencolar4");
            cola.Desencolar();
            cola.Mostrar();

            Console.WriteLine("\n ------------------------------------"); 
            Console.WriteLine("\n pila de Facturas");

            PilaFacturas pila = new PilaFacturas();

            pila.Apilar(1, 100);
            pila.Apilar(2, 200);
            pila.Apilar(3, 300);
            pila.Apilar(4, 400);

            pila.Mostrar();

            Console.WriteLine("\n desapilar1");
            pila.Desapilar();
            pila.Mostrar();

            Console.WriteLine("\n desapilar2");
            pila.Desapilar();
            pila.Mostrar();
            
           
            
        }
    }
}
*/

using System;
using Gtk;
using AutoGestPro.Core;
using AutoGestPro.UI;
namespace AutoGestPro
{
    class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            MatrizBitacora matrizBitacora = null;
            try
            {
                Application.Init();

                // Crear las instancias de las estructuras de datos
                var listaUsuarios = new ListaUsuarios();
                var listaVehiculos = new ListaVehiculos();
                var listaRepuestos = new ListaRepuestos();
                var colaServicios = new ColaServicios();
                var pilaFacturas = new PilaFacturas();
                matrizBitacora = new MatrizBitacora();

                // Crear el generador de servicios
                var generadorServicio = new GeneradorServicio(
                    listaVehiculos,
                    listaRepuestos,
                    colaServicios,
                    pilaFacturas,
                    matrizBitacora
                );

                // Crear ventana de inicio de sesión
                var inicioWindow = new Inicio();
                
                // Configurar el evento de cierre de la ventana de inicio
                inicioWindow.DeleteEvent += (o, args) => 
                {
                    matrizBitacora?.Dispose(); // Liberar recursos antes de cerrar
                    Application.Quit();
                    args.RetVal = true;
                };

                // Configurar el evento de login exitoso
                inicioWindow.LoginExitoso += () =>
                {
                    Console.WriteLine("Login exitoso");
                    inicioWindow.Hide();

                    var menu = new Menu1(
                        listaUsuarios, 
                        listaVehiculos, 
                        listaRepuestos, 
                        colaServicios, 
                        pilaFacturas,
                        generadorServicio
                    );
                    
                    menu.DeleteEvent += (o, args) =>
                    {
                        matrizBitacora?.Dispose(); // Liberar recursos antes de cerrar
                        Application.Quit();
                        args.RetVal = true;
                    };
                    menu.ShowAll();
                };

                inicioWindow.ShowAll();
                Console.WriteLine("Iniciando aplicación...");
                
                Application.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en la aplicación: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
            finally
            {
                matrizBitacora?.Dispose(); // Asegurar que los recursos se liberen incluso si hay una excepción
            }
        }
    }
}