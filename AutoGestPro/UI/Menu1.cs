// filepath: /home/rojas/Documentos/Universidad/1er S 2025/EDD/-EDD-1S2025_202200331/AutoGestPro/UI/Menu1.cs
using Gtk;
using System;
using AutoGestPro.Core;
using AutoGestPro.Utils;
using AutoGestPro.UI;

namespace AutoGestPro
{
    public class Menu1 : Window
    {
        private readonly ListaUsuarios _listaUsuarios;
        private readonly ListaVehiculos _listaVehiculos;
        private readonly ListaRepuestos _listaRepuestos;

        private readonly ColaServicios _colaServicios;

        private readonly PilaFacturas _pilaFacturas;
        private readonly GeneradorServicio _generadorServicio;

        public Menu1(ListaUsuarios listaUsuarios, ListaVehiculos listaVehiculos, ListaRepuestos listaRepuestos, ColaServicios colaServicios, PilaFacturas pilaFacturas, GeneradorServicio generadorServicio) : base("AutoGestPro - Menú Principal")
        {
            _listaUsuarios = listaUsuarios;
            _listaVehiculos = listaVehiculos;
            _listaRepuestos = listaRepuestos;
            _colaServicios = colaServicios;
            _pilaFacturas = pilaFacturas;
            _generadorServicio = generadorServicio;

            SetDefaultSize(400, 300);
            SetPosition(WindowPosition.Center);

            VBox vbox = new VBox();
            vbox.Spacing = 5;

            Label label = new Label("Menú Principal");
            vbox.PackStart(label, false, false, 0);

            Button Btn_CargaMasiva = new Button("Carga Masiva");
            Btn_CargaMasiva.Clicked += GoCargaMasiva;
            vbox.PackStart(Btn_CargaMasiva, false, false, 0);

            Button Btn_MostrarLista = new Button("Mostrar Lista");
            Btn_MostrarLista.Clicked += GoMostrarLista;
            vbox.PackStart(Btn_MostrarLista, false, false, 0);

            Button Btn_IngresoManual = new Button("Ingreso Manual");
            Btn_IngresoManual.Clicked += GoIngresoManual;
            vbox.PackStart(Btn_IngresoManual, false, false, 0);

            Button Btn_GestionUsuarios = new Button("Gestión de Usuarios");
            Btn_GestionUsuarios.Clicked += GoGestionUsuarios;
            vbox.PackStart(Btn_GestionUsuarios, false, false, 0);

            Button Btn_GenerarServicio = new Button("Generar Servicio");
            Btn_GenerarServicio.Clicked += GoGenerarServicio;
            vbox.PackStart(Btn_GenerarServicio, false, false, 0);

            Button Btn_CancelarFactura = new Button("Cancelar Factura");
            Btn_CancelarFactura.Clicked += GoCancelarFactura;
            vbox.PackStart(Btn_CancelarFactura, false, false, 0);

            Button Btn_GenerarReportes = new Button("Generar Reportes");
            Btn_GenerarReportes.Clicked += GoReportes;
            vbox.PackStart(Btn_GenerarReportes, false, false, 0);

            



            Add(vbox);
            ShowAll();
        }

        private void GoCargaMasiva(object? sender, EventArgs e)
        {
            CargaMasiva cargaMasiva = new CargaMasiva(_listaUsuarios, _listaVehiculos, _listaRepuestos);
            cargaMasiva.ShowAll();
        }

        private void GoMostrarLista(object? sender, EventArgs e)
        {
            Console.WriteLine("\n=== Lista de Usuarios ===");
            _listaUsuarios.Mostrar();
            Console.WriteLine("=======================\n");

            Console.WriteLine("\n=== Lista de Vehículos ===");
            _listaVehiculos.Mostrar();
            Console.WriteLine("=======================\n");

            Console.WriteLine("\n=== Lista de Repuestos ===");
            _listaRepuestos.Mostrar();
            Console.WriteLine("=======================\n");
        }

        private void GoGestionUsuarios(object? sender, EventArgs e)
        {
            UsuariosView usuariosView = new UsuariosView(_listaUsuarios);
            usuariosView.ShowAll();
        }

        private void GoIngresoManual(object? sender, EventArgs e)
        {
            Ingreso ingreso = new Ingreso(_listaUsuarios, _listaRepuestos, _listaVehiculos);
            ingreso.ShowAll();
        }

        private void GoReportes(object? sender, EventArgs e)
        {
            string dotFileUsuarios = "usuarios.dot";
            string dotfileRepuestos = "repuestos.dot";
            string dotfileVehiculos = "vehiculos.dot";
            string dotFileColaServicios = "colaServicios.dot";
            string dotFilePilaFacturas = "pilaFacturas.dot";
            

            // Generar el archivo DOT para usuarios
            string contenidoDotUsuarios = _listaUsuarios.GenerarGraphviz();
            GraphvizExporter.GenerarArchivoDot(dotFileUsuarios, contenidoDotUsuarios);

            //Generar el archivo Dot para Repuestos
            string contenidoDotRepuestos = _listaRepuestos.GenerarGraphviz();
            GraphvizExporter.GenerarArchivoDot(dotfileRepuestos, contenidoDotRepuestos);

            //Generar el archivo Dot para Vehiculos
            string contenidoDotVehiculos = _listaVehiculos.GenerarGraphviz();
            GraphvizExporter.GenerarArchivoDot(dotfileVehiculos, contenidoDotVehiculos);

            // Generar el archivo Dot para Cola de Servicios
            string contenidoDotColaServicios = _colaServicios.GenerarGraphviz();
            GraphvizExporter.GenerarArchivoDot(dotFileColaServicios, contenidoDotColaServicios);

            // Generar el archivo Dot para Pila de Facturas
            string contenidoDotPilaFacturas = _pilaFacturas.GenerarGraphviz();
            GraphvizExporter.GenerarArchivoDot(dotFilePilaFacturas, contenidoDotPilaFacturas);

        
            // Convertir el archivo DOT a PNG para usuarios
            GraphvizExporter.ConvertirDotAPng(dotFileUsuarios);

            // Convertir el archivo DOT a PNG para repuestos
            GraphvizExporter.ConvertirDotAPng(dotfileRepuestos);

            // Convertir el archivo DOT a PNG para vehiculos
            GraphvizExporter.ConvertirDotAPng(dotfileVehiculos);

            // Convertir el archivo DOT a PNG para cola de servicios
            GraphvizExporter.ConvertirDotAPng(dotFileColaServicios);

            // Convertir el archivo DOT a PNG para pila de facturas
            GraphvizExporter.ConvertirDotAPng(dotFilePilaFacturas);




        }

        private void GoGenerarServicio(object? sender, EventArgs e)
        {
            ServiciosView serviciosView = new ServiciosView(_listaRepuestos, _listaVehiculos, _colaServicios, _pilaFacturas);
            serviciosView.ShowAll();
        }

        /*
        private void GoCancelarFactura(object? sender, EventArgs e)
        {
            CancelarFacturaView cancelarView = new CancelarFacturaView(_pilaFacturas);
            cancelarView.ShowAll();
        }
        */

        private void GoCancelarFactura(object? sender, EventArgs e)
        {
            var cancelarView = new CancelarFacturaView(_pilaFacturas);
            cancelarView.ShowAll();
        }
      
        
    }
}