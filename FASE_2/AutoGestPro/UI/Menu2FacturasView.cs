/*
//antes
using System;
using System.Collections.Generic;
using Gtk;
using AutoGestPro.Core;

public class Menu2FacturasView : Window
{
    private ArbolBFacturas arbolBFacturas; 
    private ListBox arbolFacturasListBox;
    private Button btnActualizar;
    private ListaVehiculos listaVehiculos;
    private ArbolBinarioServicios arbolServicios;
    private Usuario usuarioLogueado;

   


    public Menu2FacturasView(Usuario usuario, ArbolBFacturas arbolFacturas) 
    : base("Facturas Pendientes")
    {
        if (usuario == null) throw new ArgumentNullException(nameof(usuario), "El usuario logueado no puede ser null.");
        if (arbolFacturas == null) throw new ArgumentNullException(nameof(arbolFacturas), "El árbol de facturas no puede ser null.");

        this.arbolBFacturas = arbolFacturas;
        this.usuarioLogueado = usuario;

        SetDefaultSize(400, 300);
        SetPosition(WindowPosition.Center);

        VBox vbox = new VBox(false, 5);
        Add(vbox);

        arbolFacturasListBox = new ListBox();
        vbox.PackStart(arbolFacturasListBox, true, true, 5);

        btnActualizar = new Button("Actualizar");
        btnActualizar.Clicked += OnActualizarClicked;
        vbox.PackStart(btnActualizar, false, false, 5);

        ShowAll();
    }


    private void OnActualizarClicked(object sender, EventArgs e)
    {
        MostrarFacturas();
    }

    private void MostrarFacturas()
    {
        if (arbolBFacturas == null)
        {
            Console.WriteLine("Error: arbolBFacturas es null");
            return;
        }

        if (usuarioLogueado == null)
        {
            Console.WriteLine("Error: usuarioLogueado es null");
            return;
        }

        List<Factura> facturas = arbolBFacturas.ObtenerFacturasPorUsuario(usuarioLogueado.ID) ?? new List<Factura>();

        foreach (var widget in arbolFacturasListBox.Children)
        {
            widget.Destroy();
        }

        foreach (var factura in facturas)
        {
            arbolFacturasListBox.Add(new Label(factura.ToString()));
        }

        arbolFacturasListBox.ShowAll();
    }

}*/


using System;
using System.Collections.Generic;
using Gtk;
using AutoGestPro.Core;
using AutoGestPro.Utils;

namespace AutoGestPro.UI
{
    public class Menu2FacturasView : Window
    {
        private readonly ArbolBFacturas _arbolBFacturas;
        private readonly Usuario _usuarioLogueado;
        
        private ListBox _facturasListBox;
        private Button _btnActualizar;
        private ScrolledWindow _scrolledWindow;

        public Menu2FacturasView(Usuario usuario, ArbolBFacturas arbolFacturas) 
        : base("Facturas Pendientes")
        {
            try
            {
                // Validaciones iniciales
                if (usuario == null) throw new ArgumentNullException(nameof(usuario), "El usuario logueado no puede ser null.");
                if (arbolFacturas == null) throw new ArgumentNullException(nameof(arbolFacturas), "El árbol de facturas no puede ser null.");

                _arbolBFacturas = arbolFacturas;
                _usuarioLogueado = usuario;

                SetDefaultSize(400, 300);
                SetPosition(WindowPosition.Center);
                
                // Configurar evento de cierre
                DeleteEvent += (o, args) => {
                    try
                    {
                        Destroy();
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.LogError("Menu2FacturasView", "DeleteEvent", ex);
                    }
                };

                CrearInterfaz();
                
                // Mostrar facturas al inicio
                MostrarFacturas();
                
                ShowAll();
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("Menu2FacturasView", "Constructor", ex);
                ErrorHandler.MostrarError(this, "Error al inicializar la ventana de facturas: " + ex.Message);
            }
        }
        
        private void CrearInterfaz()
        {
            try
            {
                VBox vbox = new VBox(false, 5);
                vbox.Margin = 10;
                
                // Título
                Label titulo = new Label("Facturas Pendientes");
                titulo.MarginBottom = 10;
                vbox.PackStart(titulo, false, false, 0);
                
                // Instrucciones
                Label instrucciones = new Label("Listado de facturas del usuario actual:");
                vbox.PackStart(instrucciones, false, false, 5);
                
                // Lista con scroll
                _scrolledWindow = new ScrolledWindow();
                _scrolledWindow.ShadowType = ShadowType.EtchedIn;
                _scrolledWindow.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
                
                _facturasListBox = new ListBox();
                _facturasListBox.SelectionMode = SelectionMode.Single;
                _facturasListBox.HeightRequest = 150;
                
                _scrolledWindow.Add(_facturasListBox);
                vbox.PackStart(_scrolledWindow, true, true, 5);
                
                // Botón actualizar
                _btnActualizar = new Button("Actualizar");
                _btnActualizar.Clicked += OnActualizarClicked;
                vbox.PackStart(_btnActualizar, false, false, 5);
                
                Add(vbox);
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("Menu2FacturasView", "CrearInterfaz", ex);
                throw; // Relanzamos para que sea manejada por el constructor
            }
        }

        private void OnActualizarClicked(object sender, EventArgs e)
        {
            try
            {
                // Deshabilitar botón mientras se procesa
                _btnActualizar.Sensitive = false;
                
                MostrarFacturas();
                
                // Rehabilitar botón
                _btnActualizar.Sensitive = true;
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("Menu2FacturasView", "OnActualizarClicked", ex);
                ErrorHandler.MostrarError(this, "Error al actualizar la lista de facturas: " + ex.Message);
                
                // Asegurar que el botón se rehabilita incluso si hay error
                _btnActualizar.Sensitive = true;
            }
        }

        private void MostrarFacturas()
        {
            try
            {
                if (_arbolBFacturas == null || _usuarioLogueado == null)
                {
                    ErrorHandler.MostrarError(this, "Error: Datos de usuario o facturas no disponibles.");
                    return;
                }

                // Obtener facturas del usuario
                List<Factura> facturas = null;
                
                try
                {
                    facturas = _arbolBFacturas.ObtenerFacturasPorUsuario(_usuarioLogueado.ID);
                }
                catch (Exception ex)
                {
                    ErrorHandler.LogError("Menu2FacturasView", "ObtenerFacturas", ex);
                    ErrorHandler.MostrarError(this, "Error al obtener las facturas: " + ex.Message);
                    return;
                }
                
                // Si es null, inicializar lista vacía
                facturas = facturas ?? new List<Factura>();

                // Limpiar la lista antes de agregar nuevas
                foreach (var widget in _facturasListBox.Children)
                {
                    try
                    {
                        widget.Destroy();
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.LogError("Menu2FacturasView", "LimpiarLista", ex);
                    }
                }

                // Mostrar las facturas en la lista
                if (facturas.Count == 0)
                {
                    Label noFacturasLabel = new Label("No hay facturas pendientes");
                    noFacturasLabel.Halign = Align.Start;
                    noFacturasLabel.MarginStart = 5;
                    _facturasListBox.Add(noFacturasLabel);
                }
                else
                {
                    foreach (var factura in facturas)
                    {
                        if (factura != null)
                        {
                            Label facturaLabel = new Label(FormatearFactura(factura));
                            facturaLabel.Halign = Align.Start;
                            facturaLabel.MarginStart = 5;
                            facturaLabel.MarginEnd = 5;
                            _facturasListBox.Add(facturaLabel);
                        }
                    }
                }

                _facturasListBox.ShowAll();
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("Menu2FacturasView", "MostrarFacturas", ex);
                ErrorHandler.MostrarError(this, "Error al mostrar las facturas: " + ex.Message);
            }
        }
        
        // Método para formatear la información de la factura de manera legible
        private string FormatearFactura(Factura factura)
        {
            try
            {
                return $"{factura.ID}: Servicio #{factura.ID_Servicio} - Total: Q{factura.Total.ToString("0.00")}";
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("Menu2FacturasView", "FormatearFactura", ex);
                return factura.ToString();
            }
        }
        
        protected override void OnDestroyed()
        {
            try
            {
                // Desconectar eventos
                if (_btnActualizar != null)
                    _btnActualizar.Clicked -= OnActualizarClicked;
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("Menu2FacturasView", "OnDestroyed", ex);
            }
            finally
            {
                base.OnDestroyed();
            }
        }
    }
}
