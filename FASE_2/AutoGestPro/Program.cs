﻿using System;
using Gtk;
using AutoGestPro.Core;
using AutoGestPro.UI;
using AutoGestPro.Utils;

namespace AutoGestPro
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            Application.Init();

            // Inicializa todas las estructuras
            var listaUsuarios = new ListaUsuarios();
            var listaVehiculos = new ListaVehiculos();
            var arbolRepuestos = new ArbolAVLRepuestos();
            var arbolServicios = new ArbolBinarioServicios();
            var arbolFacturas = new ArbolBFacturas();

            var generadorServicio = new GeneradorServicio(listaVehiculos, arbolServicios, arbolRepuestos, arbolFacturas);

            Inicio login = new Inicio(listaUsuarios);
            login.LoginExitoso += (correoUsuario) => 
            {
                Console.WriteLine($"Usuario autenticado: {correoUsuario}");
                
                Window menuWindow;
                if (correoUsuario == "root")
                {
                    Console.WriteLine("Acceso concedido como root.");
                    menuWindow = new Menu1(
                        listaUsuarios,
                        listaVehiculos,
                        generadorServicio,
                        arbolRepuestos,
                        arbolServicios,
                        arbolFacturas
                    );
                }
                else
                {
                    menuWindow = new Menu2(
                        listaUsuarios,
                        listaVehiculos,
                        generadorServicio,
                        arbolRepuestos,
                        arbolServicios,
                        arbolFacturas
                    );
                }

                menuWindow.ShowAll();
                login.Hide();

                menuWindow.DeleteEvent += (o, args) =>
                {
                    menuWindow.Destroy();
                    login.Show();
                };
            };

            login.ShowAll();
            Application.Run();  // Esta línea es crucial
        } 
    }
}




        /*
namespace AutoGestPro
{
    class Program
    {
        [STAThread]
        static void Main()
        {        
        public static void Main(string[] args)
        {
            // Crear las instancias de las estructuras de datos
            var listaUsuarios = new ListaUsuarios();
            var listaVehiculos = new ListaVehiculos();
            var ArbolAVLRepuestos = new ArbolAVLRepuestos();
            var ArbolBFacturas = new ArbolBFacturas();
            var ArbolBinarioServicios = new ArbolBinarioServicios();

            // insertar datos de prueba
            Usuario usuario1 = new Usuario(1, "juan", "perez", "juan@gmail.com", 22, "65465");
            Usuario usuario2 = new Usuario(2, "maria", "gomez", "maria@gmail.com", 25, "65465");
            Usuario usuario3 = new Usuario(3, "pedro", "rodriguez", "marias@gmail.com", 30, "65465");

            // Insertar usuarios en la lista
            listaUsuarios.Insertar(usuario1);
            listaUsuarios.Insertar(usuario2);
            listaUsuarios.Insertar(usuario3);

            // mostrar los usuarios
            listaUsuarios.Mostrar();

            // insertar vehiculos de prueba
            Vehiculo vehiculo1 = new Vehiculo(1, 1, "Toyota", "2010", "ABC123");
            Vehiculo vehiculo2 = new Vehiculo(2, 2, "Nissan", "2015", "XYZ456");
            Vehiculo vehiculo3 = new Vehiculo(3, 3, "Chevrolet", "2018", "DEF789");
            Vehiculo vehiculo4 = new Vehiculo(4, 4, "Toyota", "2010", "ABC123");
            Vehiculo vehiculo5 = new Vehiculo(5, 5, "Nissan", "2015", "XYZ456");
            Vehiculo vehiculo6 = new Vehiculo(6, 6, "Chevrolet", "2018", "DEF789");
            Vehiculo vehiculo7 = new Vehiculo(7, 7, "Toyota", "2010", "ABC123");
            Vehiculo vehiculo8 = new Vehiculo(8, 8, "Nissan", "2015", "XYZ456");
            Vehiculo vehiculo9 = new Vehiculo(9, 9, "Chevrolet", "2018", "DEF789");
            Vehiculo vehiculo10 = new Vehiculo(10, 10, "Toyota", "2010", "ABC123");

            // insertar vehiculos en la lista
            listaVehiculos.Insertar(vehiculo1);
            listaVehiculos.Insertar(vehiculo2);
            listaVehiculos.Insertar(vehiculo3);
            listaVehiculos.Insertar(vehiculo4);
            listaVehiculos.Insertar(vehiculo5);
            listaVehiculos.Insertar(vehiculo6);
            listaVehiculos.Insertar(vehiculo7);
            listaVehiculos.Insertar(vehiculo8);
            listaVehiculos.Insertar(vehiculo9);
            listaVehiculos.Insertar(vehiculo10);

            // mostrar los vehiculos
            listaVehiculos.Mostrar();

            // insertar repuestos de prueba
            Repuesto repuesto1 = new Repuesto(1, "Filtro de aceite", "10", 100);
            Repuesto repuesto2 = new Repuesto(2, "Filtro de aire", "20", 20);
            Repuesto repuesto3 = new Repuesto(6, "Bujias", "30", 300);
            Repuesto repuesto4 = new Repuesto(9, "Pastillas de freno", "40", 400);
            Repuesto repuesto5 = new Repuesto(5, "Llantas", "50", 8000); 
            Repuesto repuesto6 = new Repuesto(3, "Filtro de aceite", "60", 600);
            Repuesto repuesto7 = new Repuesto(7, "Filtro de aire", "70", 70);
            Repuesto repuesto8 = new Repuesto(8, "Bujias", "80", 800);
            Repuesto repuesto9 = new Repuesto(4, "Pastillas de freno", "90", 90);
            Repuesto repuesto10 = new Repuesto(10, "Llantas", "100", 1000);

            // insertar repuestos en el arbol
            ArbolAVLRepuestos.Insertar(repuesto1);
            ArbolAVLRepuestos.Insertar(repuesto2);  
            ArbolAVLRepuestos.Insertar(repuesto3);
            ArbolAVLRepuestos.Insertar(repuesto4);
            ArbolAVLRepuestos.Insertar(repuesto5);
            ArbolAVLRepuestos.Insertar(repuesto6);
            ArbolAVLRepuestos.Insertar(repuesto7);
            ArbolAVLRepuestos.Insertar(repuesto8);
            ArbolAVLRepuestos.Insertar(repuesto9);
            ArbolAVLRepuestos.Insertar(repuesto10);

            // mostrar los repuestos
            ArbolAVLRepuestos.Mostrar();

            // insertar servicios de prueba
            Servicio servicio1 = new Servicio(1, 1, 1, "Cambio de aceite", 1000);
            Servicio servicio2 = new Servicio(2, 2, 2, "Cambio de filtro de aire", 2000);
            Servicio servicio3 = new Servicio(3, 3, 3, "Cambio de bujias", 3000);
            Servicio servicio4 = new Servicio(4, 4, 4, "Cambio de frenos", 4000);
            Servicio servicio5 = new Servicio(5, 5, 5, "Cambio de llantas", 5000);
            Servicio servicio6 = new Servicio(6, 6, 6, "Cambio de aceite", 6000);   
            Servicio servicio7 = new Servicio(7, 7, 7, "Cambio de filtro de aire", 7000);
            Servicio servicio8 = new Servicio(8, 8, 8, "Cambio de bujias", 8000);
            Servicio servicio9 = new Servicio(9, 9, 9, "Cambio de frenos", 9000);
            Servicio servicio10 = new Servicio(10, 10, 10, "Cambio de llantas", 10000);
            Servicio servicio11 = new Servicio(11, 11, 11, "Cambio de aceite", 11000);
            Servicio servicio12 = new Servicio(12, 12, 12, "Cambio de filtro de aire", 12000);
            Servicio servicio13 = new Servicio(13, 13, 13, "Cambio de bujias", 13000);
            Servicio servicio14 = new Servicio(14, 14, 14, "Cambio de frenos", 14000);
            Servicio servicio15 = new Servicio(15, 15, 15, "Cambio de llantas", 15000);
            Servicio servicio16 = new Servicio(16, 16, 16, "Cambio de aceite", 16000);
            Servicio servicio17 = new Servicio(17, 17, 17, "Cambio de filtro de aire", 17000);
            Servicio servicio18 = new Servicio(18, 18, 18, "Cambio de bujias", 18000);
            Servicio servicio19 = new Servicio(19, 19, 19, "Cambio de frenos", 19000);
            Servicio servicio20 = new Servicio(20, 20, 20, "Cambio de llantas", 20000);
            Servicio servicio21 = new Servicio(21, 21, 21, "Cambio de aceite", 21000);
            Servicio servicio22 = new Servicio(22, 22, 22, "Cambio de filtro de aire", 22000);
            Servicio servicio23 = new Servicio(23, 23, 23, "Cambio de bujias", 23000);
            Servicio servicio24 = new Servicio(24, 24, 24, "Cambio de frenos", 24000);
            Servicio servicio25 = new Servicio(25, 25, 25, "Cambio de llantas", 25000);
            // insertar servicios en el arbol
            ArbolBinarioServicios.Insertar(servicio15);
            ArbolBinarioServicios.Insertar(servicio7);
            ArbolBinarioServicios.Insertar(servicio23);
            ArbolBinarioServicios.Insertar(servicio4);
            ArbolBinarioServicios.Insertar(servicio19);
            ArbolBinarioServicios.Insertar(servicio11);
            ArbolBinarioServicios.Insertar(servicio25);
            ArbolBinarioServicios.Insertar(servicio2);
            ArbolBinarioServicios.Insertar(servicio16);
            ArbolBinarioServicios.Insertar(servicio9);
            ArbolBinarioServicios.Insertar(servicio21);
            ArbolBinarioServicios.Insertar(servicio13);
            ArbolBinarioServicios.Insertar(servicio6);
            ArbolBinarioServicios.Insertar(servicio24);
            ArbolBinarioServicios.Insertar(servicio1);
            ArbolBinarioServicios.Insertar(servicio18);
            ArbolBinarioServicios.Insertar(servicio8);
            ArbolBinarioServicios.Insertar(servicio22);
            ArbolBinarioServicios.Insertar(servicio5);
            ArbolBinarioServicios.Insertar(servicio17);
            ArbolBinarioServicios.Insertar(servicio10);
            ArbolBinarioServicios.Insertar(servicio20);
            ArbolBinarioServicios.Insertar(servicio3);
            ArbolBinarioServicios.Insertar(servicio14);
            ArbolBinarioServicios.Insertar(servicio12);
            // mostrar los servicios
            ArbolBinarioServicios.Mostrar();

            // insertar facturas de prueba
            Factura factura1 = new Factura(1, 1, 1000);
            Factura factura2 = new Factura(2, 2, 2000);
            Factura factura3 = new Factura(3, 3, 3000);
            Factura factura4 = new Factura(4, 4, 4000);
            Factura factura5 = new Factura(5, 5, 5000);
            Factura factura6 = new Factura(6, 6, 6000);
            Factura factura7 = new Factura(7, 7, 7000);
            Factura factura8 = new Factura(8, 8, 8000);
            Factura factura9 = new Factura(9, 9, 9000);
            Factura factura10 = new Factura(10, 10, 10000);
            Factura factura11 = new Factura(11, 11, 11000);
            Factura factura12 = new Factura(12, 12, 12000);
            Factura factura13 = new Factura(13, 13, 13000);
            Factura factura14 = new Factura(14, 14, 14000);
            Factura factura15 = new Factura(15, 15, 15000);
            Factura factura16 = new Factura(16, 16, 16000);
            Factura factura17 = new Factura(17, 17, 17000);
            Factura factura18 = new Factura(18, 18, 18000);
            Factura factura19 = new Factura(19, 19, 19000);
            Factura factura20 = new Factura(20, 20, 20000);

            // insertar facturas en el arbol
            ArbolBFacturas.Insertar(factura1);
            ArbolBFacturas.Insertar(factura2);
            ArbolBFacturas.Insertar(factura3);
            ArbolBFacturas.Insertar(factura4);
            ArbolBFacturas.Insertar(factura5);
            ArbolBFacturas.Insertar(factura6);
            ArbolBFacturas.Insertar(factura7);
            ArbolBFacturas.Insertar(factura8);
            ArbolBFacturas.Insertar(factura9);
            ArbolBFacturas.Insertar(factura10);
            ArbolBFacturas.Insertar(factura11);
            ArbolBFacturas.Insertar(factura12);
            ArbolBFacturas.Insertar(factura13);
            ArbolBFacturas.Insertar(factura14);
            ArbolBFacturas.Insertar(factura15);
            ArbolBFacturas.Insertar(factura16);
            ArbolBFacturas.Insertar(factura17);
            ArbolBFacturas.Insertar(factura18);
            ArbolBFacturas.Insertar(factura19);
            ArbolBFacturas.Insertar(factura20);

            // mostrar las facturas
            ArbolBFacturas.Mostrar();

            

            string dotFileUsuarios = "usuarios.dot";
            // Generar el archivo DOT para usuarios
            string contenidoDotUsuarios = listaUsuarios.GenerarGraphviz();
            GraphvizExporter.GenerarArchivoDot(dotFileUsuarios, contenidoDotUsuarios);
            // Convertir el archivo DOT a PNG para usuarios
            GraphvizExporter.ConvertirDotAPng(dotFileUsuarios);

            string dotfileVehiculos = "vehiculos.dot";
            //Generar el archivo Dot para Vehiculos
            string contenidoDotVehiculos = listaVehiculos.GenerarGraphviz();
            GraphvizExporter.GenerarArchivoDot(dotfileVehiculos, contenidoDotVehiculos);
            // Convertir el archivo DOT a PNG para vehiculos
            GraphvizExporter.ConvertirDotAPng(dotfileVehiculos);

            string dotFileRepuestos = "repuestos.dot";
            // Generar el archivo DOT para repuestos
            string contenidoDotRepuestos = ArbolAVLRepuestos.GenerarGraphviz();
            GraphvizExporter.GenerarArchivoDot(dotFileRepuestos, contenidoDotRepuestos);
            // Convertir el archivo DOT a PNG para repuestos
            GraphvizExporter.ConvertirDotAPng(dotFileRepuestos);

            string dotFileServicios = "servicios.dot";
            // Generar el archivo DOT para servicios
            string contenidoDotServicios = ArbolBinarioServicios.GenerarGraphviz();
            GraphvizExporter.GenerarArchivoDot(dotFileServicios, contenidoDotServicios);
            // Convertir el archivo DOT a PNG para servicios
            GraphvizExporter.ConvertirDotAPng(dotFileServicios);

            string dotFileFacturas = "facturas.dot";
            // Generar el archivo DOT para facturas
            string contenidoDotFacturas = ArbolBFacturas.GenerarGraphviz();
            GraphvizExporter.GenerarArchivoDot(dotFileFacturas, contenidoDotFacturas);
            // Convertir el archivo DOT a PNG para facturas
            GraphvizExporter.ConvertirDotAPng(dotFileFacturas);

            
        }

        
        
            
    }
    

}*/