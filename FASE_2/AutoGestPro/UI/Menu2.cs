
using Gtk;
using System;
using AutoGestPro.Core;
using AutoGestPro.Utils;
using AutoGestPro.UI;

namespace AutoGestPro
{
    public class Menu2 : Window
    {
        private readonly ListaUsuarios _listaUsuarios;
        private readonly ListaVehiculos _listaVehiculos;
        private readonly GeneradorServicio _generadorServicio;
        private readonly ArbolAVLRepuestos _arbolAVLRepuestos;
        private readonly ArbolBFacturas _arbolBFacturas;
        private readonly ArbolBinarioServicios _arbolBinarioServicios;

        

        public Menu2(ListaUsuarios listaUsuarios, ListaVehiculos listaVehiculos,  GeneradorServicio generadorServicio, ArbolAVLRepuestos arbolAVLRepuestos, ArbolBinarioServicios arbolBinarioServicios, ArbolBFacturas arbolBFacturas) : base("AutoGestPro - Menú Principal")
        {
            _listaUsuarios = listaUsuarios;
            _listaVehiculos = listaVehiculos;
            _generadorServicio = generadorServicio;
            
            _arbolAVLRepuestos = arbolAVLRepuestos;
            _arbolBFacturas = arbolBFacturas;
            _arbolBinarioServicios = arbolBinarioServicios;


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

            Button Btn_IngresoManual = new Button("Gestion de Usuairos y Vehiculos");
            Btn_IngresoManual.Clicked += GoIngresoManual;
            vbox.PackStart(Btn_IngresoManual, false, false, 0);

            Button Btn_IngresoManual2 = new Button("Gestión de Repuestos");
            Btn_IngresoManual2.Clicked += GoIngresoManual2;
            vbox.PackStart(Btn_IngresoManual2, false, false, 0);

            Button Btn_RepuestosView = new Button("Visualizar Repuestos");
            Btn_RepuestosView.Clicked += GoRepuestosView;
            vbox.PackStart(Btn_RepuestosView, false, false, 0);

            Button Btn_ServiciosView = new Button("Generar Servicios");
            Btn_ServiciosView.Clicked += GoServiciosView;
            vbox.PackStart(Btn_ServiciosView, false, false, 0);

            Button Btn_GenerarReportes = new Button("Diferenciasssssssss");
            Btn_GenerarReportes.Clicked += GoReportes;
            vbox.PackStart(Btn_GenerarReportes, false, false, 0);

            Button Btn_Edicion = new Button("Edición Avanzada");
            Btn_Edicion.Clicked += GoEdicionView;
            vbox.PackStart(Btn_Edicion, false, false, 0);

         


            

            Add(vbox);
            ShowAll();
        }

        private void GoCargaMasiva(object? sender, EventArgs e)
        {
            CargaMasiva cargaMasiva = new CargaMasiva(_listaUsuarios, _listaVehiculos, _arbolAVLRepuestos);
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
            _arbolAVLRepuestos.Mostrar();
            Console.WriteLine("=======================\n");
        }

        private void GoIngresoManual2(object? sender, EventArgs e)
        {
            Ingreso2 ingreso2 = new Ingreso2(_arbolAVLRepuestos);
            ingreso2.ShowAll();
        }

        private void GoRepuestosView(object? sender, EventArgs e)
        {
            RepuestosView repuestosView = new RepuestosView(_arbolAVLRepuestos);
            repuestosView.ShowAll();
        }

        private void GoServiciosView(object? sender, EventArgs e)
        {
            ServiciosView serviciosView = new ServiciosView(_arbolAVLRepuestos, _listaVehiculos, _arbolBinarioServicios, _arbolBFacturas);

            serviciosView.ShowAll();
        }

      
   

        private void GoIngresoManual(object? sender, EventArgs e)
        {
            Ingreso ingreso = new Ingreso(_listaUsuarios, _listaVehiculos);
            ingreso.ShowAll();
        }



        private void GoRepuestos(object? sender, EventArgs e)
        {
            RepuestosView repuestosView = new RepuestosView(_arbolAVLRepuestos);
            repuestosView.ShowAll();
        }

        private void GoEdicionView(object? sender, EventArgs e)
        {
            EdicionView edicion = new EdicionView(_listaUsuarios, _listaVehiculos);
            edicion.ShowAll();
        }


        

        private void GoReportes(object? sender, EventArgs e)
        {
            // Usuarios y Vehículos
            GraphvizExporter.GenerarArchivoDot("usuarios.dot", _listaUsuarios.GenerarGraphviz());
            GraphvizExporter.GenerarArchivoDot("vehiculos.dot", _listaVehiculos.GenerarGraphviz());
            
            // Árboles
            GraphvizExporter.GenerarArchivoDot("repuestos.dot", _arbolAVLRepuestos.GenerarGraphviz());
            GraphvizExporter.GenerarArchivoDot("servicios.dot", _arbolBinarioServicios.GenerarGraphviz());
            GraphvizExporter.GenerarArchivoDot("facturas.dot", _arbolBFacturas.GenerarGraphviz());

            // Conversión a PNG
            GraphvizExporter.ConvertirDotAPng("usuarios.dot");
            GraphvizExporter.ConvertirDotAPng("vehiculos.dot");
            GraphvizExporter.ConvertirDotAPng("repuestos.dot");
            GraphvizExporter.ConvertirDotAPng("servicios.dot");
            GraphvizExporter.ConvertirDotAPng("facturas.dot");
            
            MessageDialog info = new MessageDialog(
                this, 
                DialogFlags.Modal,
                MessageType.Info, 
                ButtonsType.Ok, 
                "Reportes generados exitosamente!");
            info.Run();
            info.Destroy();
        }  

                  
    }
}