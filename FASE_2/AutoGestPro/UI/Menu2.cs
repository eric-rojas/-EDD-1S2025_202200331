/*
using Gtk;
using System;
using AutoGestPro.Core;
using AutoGestPro.Utils;
using AutoGestPro.UI;

public class Menu2 : Window
{
    private Usuario _usuarioLogueado;
    private readonly ListaVehiculos _listaVehiculos;
    private readonly ListaUsuarios _listaUsuarios;
    
    private readonly GeneradorServicio _generadorServicio;
    private readonly ArbolAVLRepuestos _arbolAVLRepuestos;
    private readonly ArbolBFacturas _arbolBFacturas;
    private readonly ArbolBinarioServicios _arbolBinarioServicios;
    private Menu2ServiciosView serviciosView;
    private Menu2InsertarVehiculo insertarVehiculo;
    

    private Menu2FacturasView facturasView;

    public Menu2(Usuario usuario, ListaVehiculos listaVehiculos, ListaUsuarios listaUsuarios, ArbolAVLRepuestos arbolAVLRepuestos, ArbolBFacturas arbolBFacturas, ArbolBinarioServicios arbolBinarioServicios) 
    : base("Menú Usuario")
    {
        _usuarioLogueado = usuario;
        _listaVehiculos = listaVehiculos;  
        _listaUsuarios = listaUsuarios;    
        _arbolAVLRepuestos = arbolAVLRepuestos;  
        _arbolBFacturas = arbolBFacturas;
        _arbolBinarioServicios = arbolBinarioServicios;

        SetDefaultSize(400, 300);
        SetPosition(WindowPosition.Center);

        var vbox = new VBox();
        vbox.Spacing = 5;
        
        var lblBienvenida = new Label($"Bienvenido: {_usuarioLogueado.Nombres}");
        vbox.PackStart(lblBienvenida, false, false, 10);

        Button Btn_RegistroVehiculo = new Button("Registro Vehículo");
        Btn_RegistroVehiculo.Clicked += GoRegistroVehiculo;
        vbox.PackStart(Btn_RegistroVehiculo, false, false, 0);

        Button Btn_ServiciosView = new Button("Sercicios");
        Btn_ServiciosView.Clicked += GoServiciosView;
        vbox.PackStart(Btn_ServiciosView, false, false, 0);

        // boton para ver facturas
        Button Btn_Facturas = new Button("Facturas");
        Btn_Facturas.Clicked += GoFacturasView;
        vbox.PackStart(Btn_Facturas, false, false, 0);

        // boton para cancelar facturas
        Button Btn_CancelarFacturas = new Button("Cancelar Facturas");
        Btn_CancelarFacturas.Clicked += GoCancelarFacturasView;
        vbox.PackStart(Btn_CancelarFacturas, false, false, 0);
        
        Button Btn_MostrarLista = new Button("Mostrar Lista");
        Btn_MostrarLista.Clicked += GoMostrarLista;
        vbox.PackStart(Btn_MostrarLista, false, false, 0);
        
        Add(vbox);
        ShowAll();
    }




    private void GoRegistroVehiculo(object sender, EventArgs e)
    {

        Menu2InsertarVehiculo insertarVehiculo = new Menu2InsertarVehiculo(_usuarioLogueado, _listaVehiculos);
        insertarVehiculo.ShowAll();
    }

    private void GoServiciosView(object sender, EventArgs e)
    {
        if (serviciosView == null || !serviciosView.Visible)
        {
            serviciosView = new Menu2ServiciosView(_usuarioLogueado, _listaVehiculos, _arbolBinarioServicios);
            serviciosView.ShowAll();
        }
        else
        {
            serviciosView.Present(); // Si ya está abierta, traerla al frente
        }
    }

  
    private void GoFacturasView(object sender, EventArgs e)
    {
        Menu2FacturasView facturasView = new Menu2FacturasView(_usuarioLogueado, _arbolBFacturas);
        facturasView.ShowAll();
    }

    private void GoCancelarFacturasView(object sender, EventArgs e)
    {
        Menu2CancelarFacturas cancelarFacturasView = new Menu2CancelarFacturas(_usuarioLogueado, _arbolBFacturas);
        cancelarFacturasView.ShowAll();
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

            Console.WriteLine("\n=== Lista de Facturas ===");
            _arbolBFacturas.Mostrar();
            Console.WriteLine("=======================\n");

            Console.WriteLine("\n=== Lista de Servicios");
            _arbolBinarioServicios.Mostrar();
            Console.WriteLine("=======================\n");
        }



}*/


using Gtk;
using System;
using AutoGestPro.Core;
using AutoGestPro.Utils;
using AutoGestPro.UI;

namespace AutoGestPro
{
    public class Menu2 : Window
    {
        private Usuario _usuarioLogueado;
        private readonly ListaVehiculos _listaVehiculos;
        private readonly ListaUsuarios _listaUsuarios;
        private readonly GeneradorServicio _generadorServicio;
        private readonly ArbolAVLRepuestos _arbolAVLRepuestos;
        private readonly ArbolBFacturas _arbolBFacturas;
        private readonly ArbolBinarioServicios _arbolBinarioServicios;
        private readonly LogueoUsuarios _logueoUsuarios;

        public Menu2(Usuario usuario, ListaVehiculos listaVehiculos, ListaUsuarios listaUsuarios, ArbolAVLRepuestos arbolAVLRepuestos, ArbolBFacturas arbolBFacturas, ArbolBinarioServicios arbolBinarioServicios) 
        : base("Menú Usuario")
        {
            _usuarioLogueado = usuario;
            _listaVehiculos = listaVehiculos;  
            _listaUsuarios = listaUsuarios;    
            _arbolAVLRepuestos = arbolAVLRepuestos;  
            _arbolBFacturas = arbolBFacturas;
            _arbolBinarioServicios = arbolBinarioServicios;
            _logueoUsuarios = new LogueoUsuarios();

            _logueoUsuarios.RegistrarEntrada(_usuarioLogueado.Correo);

            SetDefaultSize(400, 300);
            SetPosition(WindowPosition.Center);

            var vbox = new VBox();
            vbox.Spacing = 5;
            
            var lblBienvenida = new Label($"Bienvenido: {_usuarioLogueado.Nombres}");
            vbox.PackStart(lblBienvenida, false, false, 10);

            Button Btn_RegistroVehiculo = new Button("Registro Vehículo");
            Btn_RegistroVehiculo.Clicked += GoRegistroVehiculo;
            vbox.PackStart(Btn_RegistroVehiculo, false, false, 0);

            Button Btn_ServiciosView = new Button("Servicios");
            Btn_ServiciosView.Clicked += GoServiciosView;
            vbox.PackStart(Btn_ServiciosView, false, false, 0);

            Button Btn_Facturas = new Button("Facturas");
            Btn_Facturas.Clicked += GoFacturasView;
            vbox.PackStart(Btn_Facturas, false, false, 0);

            Button Btn_CancelarFacturas = new Button("Cancelar Facturas");
            Btn_CancelarFacturas.Clicked += GoCancelarFacturasView;
            vbox.PackStart(Btn_CancelarFacturas, false, false, 0);

            Button Btn_MostrarLista = new Button("Mostrar Lista");
            Btn_MostrarLista.Clicked += GoMostrarLista;
            vbox.PackStart(Btn_MostrarLista, false, false, 0);
            
            Add(vbox);
            ShowAll();
        }

        private void GoRegistroVehiculo(object sender, EventArgs e)
        {
            Menu2InsertarVehiculo insertarVehiculo = new Menu2InsertarVehiculo(_usuarioLogueado, _listaVehiculos);
            insertarVehiculo.ShowAll();
        }

        private void GoServiciosView(object sender, EventArgs e)
        {
            Menu2ServiciosView serviciosView = new Menu2ServiciosView(_usuarioLogueado, _listaVehiculos, _arbolBinarioServicios);
            serviciosView.ShowAll();
        }

        private void GoFacturasView(object sender, EventArgs e)
        {
            Menu2FacturasView facturasView = new Menu2FacturasView(_usuarioLogueado, _arbolBFacturas);
            facturasView.ShowAll();
        }

        private void GoCancelarFacturasView(object sender, EventArgs e)
        {
            Menu2CancelarFacturas cancelarFacturasView = new Menu2CancelarFacturas(_usuarioLogueado, _arbolBFacturas);
            cancelarFacturasView.ShowAll();
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

            Console.WriteLine("\n=== Lista de Facturas ===");
            _arbolBFacturas.Mostrar();
            Console.WriteLine("=======================\n");

            Console.WriteLine("\n=== Lista de Servicios");
            _arbolBinarioServicios.Mostrar();
            Console.WriteLine("=======================\n");
        }

        protected override void OnDestroyed()
        {
            if (_usuarioLogueado != null)
            {
                _logueoUsuarios.RegistrarSalida(_usuarioLogueado.Correo);
            }
            base.OnDestroyed();
        }
    }
}
