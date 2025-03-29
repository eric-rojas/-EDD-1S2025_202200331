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



}


// antes: 

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
}*/


using Gtk;
using System;
using System.Collections.Generic;
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
        private readonly ArbolAVLRepuestos _arbolAVLRepuestos;
        private readonly ArbolBFacturas _arbolBFacturas;
        private readonly ArbolBinarioServicios _arbolBinarioServicios;
        private readonly LogueoUsuarios _logueoUsuarios;
        
        // Lista para mantener referencias a las ventanas abiertas
        private List<Window> _ventanasAbiertas = new List<Window>();
        
        // Referencias a los botones
        private Dictionary<string, Button> _botones = new Dictionary<string, Button>();

        public Menu2(Usuario usuario, ListaVehiculos listaVehiculos, ListaUsuarios listaUsuarios, 
                    ArbolAVLRepuestos arbolAVLRepuestos, ArbolBFacturas arbolBFacturas, 
                    ArbolBinarioServicios arbolBinarioServicios) : base("Menú Usuario")
        {
            try
            {
                _usuarioLogueado = usuario;
                _listaVehiculos = listaVehiculos;  
                _listaUsuarios = listaUsuarios;    
                _arbolAVLRepuestos = arbolAVLRepuestos;  
                _arbolBFacturas = arbolBFacturas;
                _arbolBinarioServicios = arbolBinarioServicios;
                _logueoUsuarios = new LogueoUsuarios();
                
                // Registrar entrada en sistema de logueo
                try
                {
                    _logueoUsuarios.RegistrarEntrada(_usuarioLogueado.Correo);
                }
                catch (Exception ex)
                {
                    ErrorHandler.LogError("Menu2", "RegistrarEntrada", ex);
                    // Continuamos aunque falle el registro
                }

                SetDefaultSize(400, 300);
                SetPosition(WindowPosition.Center);
                
                // Manejar el evento de cierre
                DeleteEvent += (o, args) => {
                    try
                    {
                        // Cerrar todas las ventanas secundarias
                        foreach (var ventana in _ventanasAbiertas)
                        {
                            try
                            {
                                if (ventana != null && ventana.Handle != IntPtr.Zero)
                                    ventana.Destroy();
                            }
                            catch (Exception ex)
                            {
                                ErrorHandler.LogError("Menu2", "CerrarVentanaSecundaria", ex);
                            }
                        }
                        
                        _ventanasAbiertas.Clear();
                        Destroy();
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.LogError("Menu2", "DeleteEvent", ex);
                    }
                };

                CrearInterfaz();
                
                ShowAll();
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("Menu2", "Constructor", ex);
                ErrorHandler.MostrarError(this, "Error al inicializar el menú de usuario: " + ex.Message);
            }
        }
        
        private void CrearInterfaz()
        {
            try
            {
                var vbox = new VBox();
                vbox.Spacing = 5;
                vbox.Margin = 10;
                
                string nombreUsuario = _usuarioLogueado?.Nombres ?? "Usuario";
                var lblBienvenida = new Label($"Bienvenido: {nombreUsuario}");
                lblBienvenida.MarginBottom = 10;
                vbox.PackStart(lblBienvenida, false, false, 10);

                AgregarBoton(vbox, "Registro Vehículo", GoRegistroVehiculo);
                AgregarBoton(vbox, "Servicios", GoServiciosView);
                AgregarBoton(vbox, "Facturas", GoFacturasView);
                AgregarBoton(vbox, "Cancelar Facturas", GoCancelarFacturasView);
                AgregarBoton(vbox, "Mostrar Lista", GoMostrarLista);
                
                Add(vbox);
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("Menu2", "CrearInterfaz", ex);
                throw; // Relanzamos para que sea manejada por el constructor
            }
        }
        
        private void AgregarBoton(VBox vbox, string texto, EventHandler handler)
        {
            try
            {
                Button btn = new Button(texto);
                btn.Clicked += handler;
                vbox.PackStart(btn, false, false, 5);
                _botones[texto] = btn;
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("Menu2", "AgregarBoton", ex);
                throw;
            }
        }

        private void GoRegistroVehiculo(object sender, EventArgs e)
        {
            try
            {
                // Deshabilitar botón mientras se procesa
                _botones["Registro Vehículo"].Sensitive = false;
                
                Menu2InsertarVehiculo insertarVehiculo = new Menu2InsertarVehiculo(_usuarioLogueado, _listaVehiculos);
                
                // Registrar la ventana
                _ventanasAbiertas.Add(insertarVehiculo);
                
                // Configurar evento para eliminar de la lista al cerrarse
                insertarVehiculo.DeleteEvent += (o, args) => {
                    try
                    {
                        _ventanasAbiertas.Remove(insertarVehiculo);
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.LogError("Menu2", "InsertarVehiculoDeleteEvent", ex);
                    }
                };
                
                insertarVehiculo.ShowAll();
                
                // Rehabilitar botón
                _botones["Registro Vehículo"].Sensitive = true;
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("Menu2", "GoRegistroVehiculo", ex);
                ErrorHandler.MostrarError(this, "Error al abrir la ventana de registro de vehículo: " + ex.Message);
                
                // Asegurar que el botón se rehabilita incluso si hay error
                _botones["Registro Vehículo"].Sensitive = true;
            }
        }

        private void GoServiciosView(object sender, EventArgs e)
        {
            try
            {
                // Deshabilitar botón mientras se procesa
                _botones["Servicios"].Sensitive = false;
                
                Menu2ServiciosView serviciosView = new Menu2ServiciosView(_usuarioLogueado, _listaVehiculos, _arbolBinarioServicios);
                
                // Registrar la ventana
                _ventanasAbiertas.Add(serviciosView);
                
                // Configurar evento para eliminar de la lista al cerrarse
                serviciosView.DeleteEvent += (o, args) => {
                    try
                    {
                        _ventanasAbiertas.Remove(serviciosView);
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.LogError("Menu2", "ServiciosViewDeleteEvent", ex);
                    }
                };
                
                serviciosView.ShowAll();
                
                // Rehabilitar botón
                _botones["Servicios"].Sensitive = true;
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("Menu2", "GoServiciosView", ex);
                ErrorHandler.MostrarError(this, "Error al abrir la ventana de servicios: " + ex.Message);
                
                // Asegurar que el botón se rehabilita incluso si hay error
                _botones["Servicios"].Sensitive = true;
            }
        }

        private void GoFacturasView(object sender, EventArgs e)
        {
            try
            {
                // Deshabilitar botón mientras se procesa
                _botones["Facturas"].Sensitive = false;
                
                Menu2FacturasView facturasView = new Menu2FacturasView(_usuarioLogueado, _arbolBFacturas);
                
                // Registrar la ventana
                _ventanasAbiertas.Add(facturasView);
                
                // Configurar evento para eliminar de la lista al cerrarse
                facturasView.DeleteEvent += (o, args) => {
                    try
                    {
                        _ventanasAbiertas.Remove(facturasView);
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.LogError("Menu2", "FacturasViewDeleteEvent", ex);
                    }
                };
                
                facturasView.ShowAll();
                
                // Rehabilitar botón
                _botones["Facturas"].Sensitive = true;
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("Menu2", "GoFacturasView", ex);
                ErrorHandler.MostrarError(this, "Error al abrir la ventana de facturas: " + ex.Message);
                
                // Asegurar que el botón se rehabilita incluso si hay error
                _botones["Facturas"].Sensitive = true;
            }
        }

        private void GoCancelarFacturasView(object sender, EventArgs e)
        {
            try
            {
                // Deshabilitar botón mientras se procesa
                _botones["Cancelar Facturas"].Sensitive = false;
                
                Menu2CancelarFacturas cancelarFacturasView = new Menu2CancelarFacturas(_usuarioLogueado, _arbolBFacturas);
                
                // Registrar la ventana
                _ventanasAbiertas.Add(cancelarFacturasView);
                
                // Configurar evento para eliminar de la lista al cerrarse
                cancelarFacturasView.DeleteEvent += (o, args) => {
                    try
                    {
                        _ventanasAbiertas.Remove(cancelarFacturasView);
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.LogError("Menu2", "CancelarFacturasViewDeleteEvent", ex);
                    }
                };
                
                cancelarFacturasView.ShowAll();
                
                // Rehabilitar botón
                _botones["Cancelar Facturas"].Sensitive = true;
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("Menu2", "GoCancelarFacturasView", ex);
                ErrorHandler.MostrarError(this, "Error al abrir la ventana de cancelación de facturas: " + ex.Message);
                
                // Asegurar que el botón se rehabilita incluso si hay error
                _botones["Cancelar Facturas"].Sensitive = true;
            }
        }

        private void GoMostrarLista(object sender, EventArgs e)
        {
            try
            {
                // Deshabilitar botón mientras se procesa
                _botones["Mostrar Lista"].Sensitive = false;
                
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
                
                ErrorHandler.MostrarInfo(this, "Datos mostrados en la consola.");
                
                // Rehabilitar botón
                _botones["Mostrar Lista"].Sensitive = true;
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("Menu2", "GoMostrarLista", ex);
                ErrorHandler.MostrarError(this, "Error al mostrar listas: " + ex.Message);
                
                // Asegurar que el botón se rehabilita incluso si hay error
                _botones["Mostrar Lista"].Sensitive = true;
            }
        }

        protected override void OnDestroyed()
        {
            try
            {
                // Registrar salida en sistema de logueo
                if (_usuarioLogueado != null)
                {
                    try
                    {
                        _logueoUsuarios.RegistrarSalida(_usuarioLogueado.Correo);
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.LogError("Menu2", "RegistrarSalida", ex);
                    }
                }
                
                // Desconectar todos los eventos de botones
                foreach (var kvp in _botones)
                {
                    Button btn = kvp.Value;
                    string texto = kvp.Key;
                    
                    try
                    {
                        switch (texto)
                        {
                            case "Registro Vehículo":
                                btn.Clicked -= GoRegistroVehiculo;
                                break;
                            case "Servicios":
                                btn.Clicked -= GoServiciosView;
                                break;
                            case "Facturas":
                                btn.Clicked -= GoFacturasView;
                                break;
                            case "Cancelar Facturas":
                                btn.Clicked -= GoCancelarFacturasView;
                                break;
                            case "Mostrar Lista":
                                btn.Clicked -= GoMostrarLista;
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.LogError("Menu2", "DesconectarEvento_" + texto, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("Menu2", "OnDestroyed", ex);
            }
            finally
            {
                base.OnDestroyed();
            }
        }
    }
}