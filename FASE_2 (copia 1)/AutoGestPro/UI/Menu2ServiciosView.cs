/*using System;
using System.Collections.Generic;
using Gtk;
using AutoGestPro.Core;

public class Menu2ServiciosView : Window
{
    private ListaVehiculos listaVehiculos;
    private ArbolBinarioServicios arbolServicios;
    private Usuario usuarioLogueado;
    
    private ComboBox comboBoxRecorridos;
    private ListBox listBoxServicios;
    private Button btnActualizar;

    public Menu2ServiciosView(Usuario usuario, ListaVehiculos listaVehiculos, ArbolBinarioServicios arbolServicios) 
        : base("Servicios por Vehículo")
    {
        this.usuarioLogueado = usuario;
        this.listaVehiculos = listaVehiculos;
        this.arbolServicios = arbolServicios;

        SetDefaultSize(400, 300);
        SetPosition(WindowPosition.Center);
        
        VBox vbox = new VBox(false, 5);
        Add(vbox);

        // Dropdown para seleccionar tipo de recorrido
        comboBoxRecorridos = new ComboBox();
        var store = new ListStore(typeof(string));
        comboBoxRecorridos.Model = store;
        var renderer = new CellRendererText();
        comboBoxRecorridos.PackStart(renderer, true);
        comboBoxRecorridos.AddAttribute(renderer, "text", 0);
        store.AppendValues("Pre-Orden");
        store.AppendValues("In-Orden");
        store.AppendValues("Post-Orden");
        comboBoxRecorridos.Active = 0; // Por defecto, Pre-Orden
        vbox.PackStart(comboBoxRecorridos, false, false, 5);

        // Lista para mostrar los servicios
        listBoxServicios = new ListBox();
        vbox.PackStart(listBoxServicios, true, true, 5);

        // Botón para actualizar la lista
        btnActualizar = new Button("Actualizar");
        btnActualizar.Clicked += OnActualizarClicked;
        vbox.PackStart(btnActualizar, false, false, 5);

        ShowAll();
    }

    private void OnActualizarClicked(object sender, EventArgs e)
    {
        MostrarServicios(comboBoxRecorridos.Active + 1);
    }

    private void MostrarServicios(int tipoRecorrido)
    {
        List<Servicio> servicios = new List<Servicio>();

        // Obtener los vehículos del usuario logueado
        List<Vehiculo> vehiculosUsuario = listaVehiculos.BuscarPorUsuario(usuarioLogueado.ID);


        // Recorrer cada vehículo y obtener sus servicios
        foreach (var vehiculo in vehiculosUsuario)
        {
            servicios.AddRange(arbolServicios.BuscarPorVehiculo(vehiculo.ID));
        }

        // Aplicar el recorrido seleccionado
        switch (tipoRecorrido)
        {
            case 1:
                servicios = arbolServicios.RecorridoPreOrden();
                break;
            case 2:
                servicios = arbolServicios.RecorridoInOrden();
                break;
            case 3:
                servicios = arbolServicios.RecorridoPostOrden();
                break;
        }

        // Mostrar los servicios en la UI
        listBoxServicios.Foreach((widget) => widget.Destroy());
        foreach (var servicio in servicios)
        {
            listBoxServicios.Add(new Label(servicio.ToString()));
        }

        listBoxServicios.ShowAll();
    }
}*/

using System;
using System.Collections.Generic;
using Gtk;
using AutoGestPro.Core;
using AutoGestPro.Utils;

namespace AutoGestPro.UI
{
    public class Menu2ServiciosView : Window
    {
        private readonly ListaVehiculos _listaVehiculos;
        private readonly ArbolBinarioServicios _arbolServicios;
        private readonly Usuario _usuarioLogueado;
        
        private ComboBox _comboBoxRecorridos;
        private ListBox _listBoxServicios;
        private Button _btnActualizar;
        private ScrolledWindow _scrolledWindow;

        public Menu2ServiciosView(Usuario usuario, ListaVehiculos listaVehiculos, ArbolBinarioServicios arbolServicios) 
            : base("Servicios por Vehículo")
        {
            try
            {
                _usuarioLogueado = usuario;
                _listaVehiculos = listaVehiculos;
                _arbolServicios = arbolServicios;

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
                        ErrorHandler.LogError("Menu2ServiciosView", "DeleteEvent", ex);
                    }
                };
                
                CrearInterfaz();
                
                // Mostrar servicios al inicio con recorrido pre-orden
                MostrarServicios(1);
                
                ShowAll();
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("Menu2ServiciosView", "Constructor", ex);
                ErrorHandler.MostrarError(this, "Error al inicializar la ventana de servicios: " + ex.Message);
            }
        }
        
        private void CrearInterfaz()
        {
            try
            {
                VBox vbox = new VBox(false, 5);
                vbox.Margin = 10;
                
                // Título
                Label labelTitulo = new Label("Servicios por Vehículo");
                labelTitulo.MarginBottom = 10;
                vbox.PackStart(labelTitulo, false, false, 0);
                
                // Etiqueta informativa
                Label labelInfo = new Label($"Usuario: {_usuarioLogueado.Nombres} {_usuarioLogueado.Apellidos}");
                labelInfo.Halign = Align.Start;
                vbox.PackStart(labelInfo, false, false, 5);
                
                // Dropdown para seleccionar tipo de recorrido
                HBox hboxRecorrido = new HBox(false, 5);
                Label labelRecorrido = new Label("Tipo de recorrido:");
                hboxRecorrido.PackStart(labelRecorrido, false, false, 0);
                
                _comboBoxRecorridos = new ComboBox();
                var store = new ListStore(typeof(string));
                _comboBoxRecorridos.Model = store;
                var renderer = new CellRendererText();
                _comboBoxRecorridos.PackStart(renderer, true);
                _comboBoxRecorridos.AddAttribute(renderer, "text", 0);
                store.AppendValues("Pre-Orden");
                store.AppendValues("In-Orden");
                store.AppendValues("Post-Orden");
                _comboBoxRecorridos.Active = 0; // Por defecto, Pre-Orden
                hboxRecorrido.PackStart(_comboBoxRecorridos, false, false, 0);
                
                vbox.PackStart(hboxRecorrido, false, false, 5);

                // Lista para mostrar los servicios (con scroll)
                _scrolledWindow = new ScrolledWindow();
                _scrolledWindow.ShadowType = ShadowType.EtchedIn;
                _scrolledWindow.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
                
                _listBoxServicios = new ListBox();
                _listBoxServicios.SelectionMode = SelectionMode.None;
                _listBoxServicios.HeightRequest = 150;
                
                _scrolledWindow.Add(_listBoxServicios);
                vbox.PackStart(_scrolledWindow, true, true, 5);

                // Botón para actualizar la lista
                _btnActualizar = new Button("Actualizar");
                _btnActualizar.Clicked += OnActualizarClicked;
                vbox.PackStart(_btnActualizar, false, false, 5);

                Add(vbox);
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("Menu2ServiciosView", "CrearInterfaz", ex);
                throw; // Relanzamos para que sea manejada por el constructor
            }
        }

        private void OnActualizarClicked(object sender, EventArgs e)
        {
            try
            {
                // Deshabilitar botón mientras se procesa
                _btnActualizar.Sensitive = false;
                
                // Obtener tipo de recorrido seleccionado
                int tipoRecorrido = _comboBoxRecorridos.Active + 1;
                
                MostrarServicios(tipoRecorrido);
                
                // Rehabilitar botón
                _btnActualizar.Sensitive = true;
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("Menu2ServiciosView", "OnActualizarClicked", ex);
                ErrorHandler.MostrarError(this, "Error al actualizar la lista de servicios: " + ex.Message);
                
                // Asegurar que el botón se rehabilita incluso si hay error
                _btnActualizar.Sensitive = true;
            }
        }

        private void MostrarServicios(int tipoRecorrido)
        {
            try
            {
                List<Servicio> servicios = new List<Servicio>();
                List<Servicio> serviciosUsuario = new List<Servicio>();

                // Obtener los servicios según el recorrido
                try
                {
                    switch (tipoRecorrido)
                    {
                        case 1:
                            servicios = _arbolServicios.RecorridoPreOrden() ?? new List<Servicio>();
                            break;
                        case 2:
                            servicios = _arbolServicios.RecorridoInOrden() ?? new List<Servicio>();
                            break;
                        case 3:
                            servicios = _arbolServicios.RecorridoPostOrden() ?? new List<Servicio>();
                            break;
                        default:
                            servicios = _arbolServicios.RecorridoPreOrden() ?? new List<Servicio>();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    ErrorHandler.LogError("Menu2ServiciosView", "ObtenerServicios", ex);
                    ErrorHandler.MostrarError(this, "Error al obtener los servicios: " + ex.Message);
                    return;
                }

                // Obtener los vehículos del usuario logueado
                List<Vehiculo> vehiculosUsuario = null;
                try
                {
                    vehiculosUsuario = _listaVehiculos.BuscarPorUsuario(_usuarioLogueado.ID) ?? new List<Vehiculo>();
                }
                catch (Exception ex)
                {
                    ErrorHandler.LogError("Menu2ServiciosView", "ObtenerVehiculosUsuario", ex);
                    ErrorHandler.MostrarError(this, "Error al obtener los vehículos del usuario: " + ex.Message);
                    return;
                }

                // Crear un conjunto con los IDs de vehículos del usuario para búsqueda rápida
                HashSet<int> idsVehiculosUsuario = new HashSet<int>();
                foreach (var vehiculo in vehiculosUsuario)
                {
                    if (vehiculo != null)
                    {
                        idsVehiculosUsuario.Add(vehiculo.ID);
                    }
                }

                // Filtrar servicios solo para los vehículos del usuario
                foreach (var servicio in servicios)
                {
                    if (servicio != null && idsVehiculosUsuario.Contains(servicio.ID_Vehiculo))
                    {
                        serviciosUsuario.Add(servicio);
                    }
                }

                // Limpiar la lista antes de agregar nuevos servicios
                _listBoxServicios.Foreach((widget) => {
                    try
                    {
                        if (widget != null)
                        {
                            widget.Destroy();
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.LogError("Menu2ServiciosView", "LimpiarLista", ex);
                    }
                });

                // Mostrar mensaje cuando no hay servicios
                if (serviciosUsuario.Count == 0)
                {
                    Label noServiciosLabel = new Label("No hay servicios disponibles para sus vehículos");
                    noServiciosLabel.Halign = Align.Start;
                    noServiciosLabel.MarginStart = 5;
                    _listBoxServicios.Add(noServiciosLabel);
                }
                else
                {
                    // Mostrar los servicios en la UI
                    foreach (var servicio in serviciosUsuario)
                    {
                        if (servicio != null)
                        {
                            Label servicioLabel = new Label(FormatearServicio(servicio));
                            servicioLabel.Halign = Align.Start;
                            servicioLabel.MarginStart = 5;
                            servicioLabel.MarginEnd = 5;
                            _listBoxServicios.Add(servicioLabel);
                        }
                    }
                }

                _listBoxServicios.ShowAll();
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("Menu2ServiciosView", "MostrarServicios", ex);
                ErrorHandler.MostrarError(this, "Error al mostrar los servicios: " + ex.Message);
            }
        }
        
        // Método para formatear la información del servicio de manera legible
        private string FormatearServicio(Servicio servicio)
        {
            try
            {
                // Buscar detalles del vehículo
                string vehiculoInfo = "Vehículo desconocido";
                try
                {
                    Vehiculo vehiculo = _listaVehiculos.Buscar(servicio.ID_Vehiculo);
                    if (vehiculo != null)
                    {
                        vehiculoInfo = $"{vehiculo.Marca} {vehiculo.Modelo} ({vehiculo.Placa})";
                    }
                }
                catch (Exception)
                {
                    // Ignorar errores al buscar el vehículo
                }
                
                return $"Servicio #{servicio.ID}: {servicio.Detalles} - {vehiculoInfo} - Costo: Q{servicio.Costo.ToString("0.00")}";
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("Menu2ServiciosView", "FormatearServicio", ex);
                return servicio.ToString();
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
                ErrorHandler.LogError("Menu2ServiciosView", "OnDestroyed", ex);
            }
            finally
            {
                base.OnDestroyed();
            }
        }
    }
}
