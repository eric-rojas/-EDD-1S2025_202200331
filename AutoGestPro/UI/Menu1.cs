using Gtk;
using System;
using AutoGestPro.Core;
using AutoGestPro.UI; // Add this line to import the UsuariosView class

namespace AutoGestPro
{
    public class Menu1 : Window
    {
        private readonly ListaUsuarios _listaUsuarios;
        private readonly ListaVehiculos _listaVehiculos;
        private readonly ListaRepuestos _listaRepuestos;

        public Menu1(ListaUsuarios listaUsuarios, ListaVehiculos listaVehiculos, ListaRepuestos listaRepuestos) : base("AutoGestPro - Menú Principal")
        {
            _listaUsuarios = listaUsuarios;
            _listaVehiculos = listaVehiculos;
            _listaRepuestos = listaRepuestos;

            SetDefaultSize(400, 300);
            SetPosition(WindowPosition.Center);

            // Crear un contenedor para los elementos
            VBox vbox = new VBox();
            vbox.Spacing = 5;

            // Label
            Label label = new Label("Menú Principal");
            vbox.PackStart(label, false, false, 0);

            // Cargar Archivo
            Button Btn_CargaMasiva = new Button("Carga Masiva");
            Btn_CargaMasiva.Clicked += GoCargaMasiva;
            vbox.PackStart(Btn_CargaMasiva, false, false, 0);

            // Mostrar Lista
            Button Btn_MostrarLista = new Button("Mostrar Lista");
            Btn_MostrarLista.Clicked += GoMostrarLista;
            vbox.PackStart(Btn_MostrarLista, false, false, 0);

            // Mostrar Ingreso Manual
            Button Btn_IngresoManual = new Button("Ingreso Manual");
            Btn_IngresoManual.Clicked += GoIngresoManual;
            vbox.PackStart(Btn_IngresoManual, false, false, 0);

            // Mostrar Gestion de Usuarios
            Button Btn_GestionUsuarios = new Button("Gestión de Usuarios");
            Btn_GestionUsuarios.Clicked += GoGestionUsuarios;
            vbox.PackStart(Btn_GestionUsuarios, false, false, 0);

            // Mostrar Generar Servicio
            Button Btn_GenerarServicio = new Button("Generar Servicio");
            Btn_GenerarServicio.Clicked += GoMostrarLista;
            vbox.PackStart(Btn_GenerarServicio, false, false, 0);

            // Cancelar Factura
            Button Btn_CancelarFactura = new Button("Cancelar Factura");
            Btn_CancelarFactura.Clicked += GoMostrarLista; 
            vbox.PackStart(Btn_CancelarFactura, false, false, 0);

            Add(vbox);
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
    }
}