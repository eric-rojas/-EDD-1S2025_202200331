/*using System;
using Gtk;
using AutoGestPro.Core;
using System.Collections.Generic;

public class Menu2CancelarFacturas : Window
{
    private ArbolBFacturas arbolBFacturas;
    private Usuario usuarioLogueado;

    private ListBox listBoxFacturas;
    private Button btnCancelarFactura;
    private Entry entryFacturaID;

    public Menu2CancelarFacturas(Usuario usuario, ArbolBFacturas arbolFacturas)
        : base("Cancelar Facturas")
    {
        this.usuarioLogueado = usuario;
        this.arbolBFacturas = arbolFacturas;

        SetDefaultSize(400, 300);
        SetPosition(WindowPosition.Center);

        VBox vbox = new VBox(false, 5);
        Add(vbox);

        // Campo de texto para ingresar el ID de la factura a cancelar
        entryFacturaID = new Entry();
        entryFacturaID.PlaceholderText = "Ingresa ID de la factura a cancelar";
        vbox.PackStart(entryFacturaID, false, false, 5);

        // Lista para mostrar las facturas
        listBoxFacturas = new ListBox();
        vbox.PackStart(listBoxFacturas, true, true, 5);

        // Botón para cancelar la factura
        btnCancelarFactura = new Button("Cancelar Factura");
        btnCancelarFactura.Clicked += OnCancelarFacturaClicked;
        vbox.PackStart(btnCancelarFactura, false, false, 5);

        ShowAll();

        // Mostrar las facturas del usuario logueado
        MostrarFacturas();
    }

    // Mostrar las facturas del usuario logueado
    private void MostrarFacturas()
    {
        // Obtener las facturas del usuario logueado
        List<Factura> facturas = arbolBFacturas.ObtenerFacturasPorUsuario(usuarioLogueado.ID);

        // Limpiar la lista antes de agregar nuevas
        foreach (var widget in listBoxFacturas.Children)
        {
            widget.Destroy();
        }

        // Mostrar las facturas en la lista
        foreach (var factura in facturas)
        {
            listBoxFacturas.Add(new Label(factura.ToString()));
        }

        listBoxFacturas.ShowAll();
    }

    // Método que se ejecuta al hacer clic en "Cancelar Factura"
    private void OnCancelarFacturaClicked(object sender, EventArgs e)
    {
        int idFactura;
        if (int.TryParse(entryFacturaID.Text, out idFactura))
        {
            // Buscar la factura por ID
            Factura factura = arbolBFacturas.BuscarPorID(idFactura);

            if (factura != null && factura.ID_Usuario == usuarioLogueado.ID)
            {
                // Eliminar la factura
                CancelarFactura(idFactura);
                MostrarFacturas(); // Actualizar la lista de facturas mostradas
                ShowMessage("Factura cancelada correctamente.");
            }
            else
            {
                ShowMessage("Factura no encontrada o no pertenece a este usuario.");
            }
        }
        else
        {
            ShowMessage("Por favor ingresa un ID válido.");
        }
    }

    // Método para cancelar (eliminar) la factura
    private void CancelarFactura(int idFactura)
    {
        bool facturaEliminada = arbolBFacturas.Eliminar(idFactura);
        if (!facturaEliminada)
        {
            ShowMessage("Error al cancelar la factura.");
        }
    }

    // Mostrar un mensaje en la interfaz de usuario
    private void ShowMessage(string message)
    {
        MessageDialog md = new MessageDialog(this, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok, message);
        md.Run();
        md.Destroy();
    }
}*/


using System;
using Gtk;
using AutoGestPro.Core;
using AutoGestPro.Utils;
using System.Collections.Generic;

namespace AutoGestPro.UI
{
    public class Menu2CancelarFacturas : Window
    {
        private ArbolBFacturas _arbolBFacturas;
        private Usuario _usuarioLogueado;

        private ListBox _listBoxFacturas;
        private Button _btnCancelarFactura;
        private Entry _entryFacturaID;
        private ScrolledWindow _scrolledWindow;

        public Menu2CancelarFacturas(Usuario usuario, ArbolBFacturas arbolFacturas)
            : base("Cancelar Facturas")
        {
            try
            {
                _usuarioLogueado = usuario;
                _arbolBFacturas = arbolFacturas;

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
                        ErrorHandler.LogError("Menu2CancelarFacturas", "DeleteEvent", ex);
                    }
                };

                CrearInterfaz();
                MostrarFacturas();
                
                ShowAll();
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("Menu2CancelarFacturas", "Constructor", ex);
                ErrorHandler.MostrarError(this, "Error al inicializar la ventana de cancelación de facturas: " + ex.Message);
            }
        }

        private void CrearInterfaz()
        {
            try
            {
                VBox vbox = new VBox(false, 5);
                vbox.Margin = 10;
                
                // Título
                Label titulo = new Label("Cancelación de Facturas");
                titulo.MarginBottom = 10;
                vbox.PackStart(titulo, false, false, 0);
                
                // Campo de texto para ingresar el ID de la factura a cancelar
                HBox hboxEntry = new HBox(false, 5);
                hboxEntry.PackStart(new Label("ID Factura:"), false, false, 0);
                
                _entryFacturaID = new Entry();
                _entryFacturaID.PlaceholderText = "Ingresa ID de la factura a cancelar";
                hboxEntry.PackStart(_entryFacturaID, true, true, 0);
                
                vbox.PackStart(hboxEntry, false, false, 5);

                // Etiqueta informativa
                Label lblInstrucciones = new Label("Facturas disponibles para cancelar:");
                vbox.PackStart(lblInstrucciones, false, false, 5);
                
                // Lista para mostrar las facturas (con scroll)
                _scrolledWindow = new ScrolledWindow();
                _scrolledWindow.ShadowType = ShadowType.EtchedIn;
                _scrolledWindow.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
                
                _listBoxFacturas = new ListBox();
                _listBoxFacturas.SelectionMode = SelectionMode.Single;
                _listBoxFacturas.HeightRequest = 150;
                
                // Manejar la selección de una factura
                _listBoxFacturas.RowSelected += OnFacturaSeleccionada;
                
                _scrolledWindow.Add(_listBoxFacturas);
                vbox.PackStart(_scrolledWindow, true, true, 5);

                // Botón para cancelar la factura
                _btnCancelarFactura = new Button("Cancelar Factura");
                _btnCancelarFactura.Clicked += OnCancelarFacturaClicked;
                vbox.PackStart(_btnCancelarFactura, false, false, 5);

                Add(vbox);
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("Menu2CancelarFacturas", "CrearInterfaz", ex);
                throw; // Relanzamos para que sea manejada por el constructor
            }
        }

        // Método para manejar la selección de una factura en la lista
        private void OnFacturaSeleccionada(object sender, RowSelectedArgs args)
        {
            try
            {
                if (args.Row != null)
                {
                    // Obtener el widget dentro de la fila seleccionada
                    Widget widget = args.Row.Child;
                    if (widget is Label label)
                    {
                        // Extraer el ID de la factura del texto del label
                        string labelText = label.Text;
                        string[] partes = labelText.Split(new[] { ':' }, 2);
                        if (partes.Length > 0)
                        {
                            string idStr = partes[0].Trim();
                            if (int.TryParse(idStr, out int id))
                            {
                                // Asignar el ID al campo de entrada
                                _entryFacturaID.Text = id.ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("Menu2CancelarFacturas", "OnFacturaSeleccionada", ex);
                // No mostramos error al usuario para no interrumpir la experiencia
            }
        }

        // Mostrar las facturas del usuario logueado
        private void MostrarFacturas()
        {
            try
            {
                // Obtener las facturas del usuario logueado
                List<Factura> facturas = null;
                
                try
                {
                    facturas = _arbolBFacturas.ObtenerFacturasPorUsuario(_usuarioLogueado.ID);
                }
                catch (Exception ex)
                {
                    ErrorHandler.LogError("Menu2CancelarFacturas", "ObtenerFacturas", ex);
                    ErrorHandler.MostrarError(this, "Error al obtener las facturas: " + ex.Message);
                    return;
                }
                
                if (facturas == null)
                {
                    ErrorHandler.MostrarInfo(this, "No hay facturas disponibles.");
                    return;
                }

                // Limpiar la lista antes de agregar nuevas
                foreach (var widget in _listBoxFacturas.Children)
                {
                    try
                    {
                        widget.Destroy();
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.LogError("Menu2CancelarFacturas", "LimpiarLista", ex);
                    }
                }

                // Mostrar las facturas en la lista
                if (facturas.Count == 0)
                {
                    Label noFacturasLabel = new Label("No hay facturas disponibles");
                    _listBoxFacturas.Add(noFacturasLabel);
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
                            _listBoxFacturas.Add(facturaLabel);
                        }
                    }
                }

                _listBoxFacturas.ShowAll();
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("Menu2CancelarFacturas", "MostrarFacturas", ex);
                ErrorHandler.MostrarError(this, "Error al mostrar las facturas: " + ex.Message);
            }
        }
        
        // Método para formatear la información de la factura de manera legible
        private string FormatearFactura(Factura factura)
        {
            try
            {
                return $"{factura.ID}: {factura.ID_Servicio} - Total: Q{factura.Total.ToString("0.00")}";
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("Menu2CancelarFacturas", "FormatearFactura", ex);
                return factura.ToString();
            }
        }

        // Método que se ejecuta al hacer clic en "Cancelar Factura"
        private void OnCancelarFacturaClicked(object sender, EventArgs e)
        {
            try
            {
                // Deshabilitar botón mientras se procesa
                _btnCancelarFactura.Sensitive = false;
                
                string idText = _entryFacturaID.Text?.Trim() ?? string.Empty;
                
                if (string.IsNullOrEmpty(idText))
                {
                    ErrorHandler.MostrarError(this, "Por favor ingresa un ID de factura.");
                    _btnCancelarFactura.Sensitive = true;
                    return;
                }
                
                if (!int.TryParse(idText, out int idFactura))
                {
                    ErrorHandler.MostrarError(this, "Por favor ingresa un ID válido (número entero).");
                    _btnCancelarFactura.Sensitive = true;
                    return;
                }

                // Buscar la factura por ID
                Factura factura = null;
                try
                {
                    factura = _arbolBFacturas.BuscarPorID(idFactura);
                }
                catch (Exception ex)
                {
                    ErrorHandler.LogError("Menu2CancelarFacturas", "BuscarFactura", ex);
                    ErrorHandler.MostrarError(this, "Error al buscar la factura: " + ex.Message);
                    _btnCancelarFactura.Sensitive = true;
                    return;
                }

                if (factura == null)
                {
                    ErrorHandler.MostrarError(this, "No se encontró ninguna factura con el ID especificado.");
                    _btnCancelarFactura.Sensitive = true;
                    return;
                }

                if (factura.ID_Usuario != _usuarioLogueado.ID)
                {
                    ErrorHandler.MostrarError(this, "La factura no pertenece a este usuario.");
                    _btnCancelarFactura.Sensitive = true;
                    return;
                }

                // Confirmar eliminación
                MessageDialog confirmDialog = new MessageDialog(
                    this,
                    DialogFlags.Modal,
                    MessageType.Question,
                    ButtonsType.YesNo,
                    $"¿Está seguro de cancelar la factura {idFactura}?"
                );
                
                ResponseType response = (ResponseType)confirmDialog.Run();
                confirmDialog.Destroy();
                
                if (response == ResponseType.Yes)
                {
                    // Eliminar la factura
                    try
                    {
                        bool facturaEliminada = _arbolBFacturas.Eliminar(idFactura);
                        
                        if (facturaEliminada)
                        {
                            MostrarFacturas(); // Actualizar la lista de facturas mostradas
                            _entryFacturaID.Text = string.Empty; // Limpiar el campo de ID
                            ErrorHandler.MostrarInfo(this, "Factura cancelada correctamente.");
                        }
                        else
                        {
                            ErrorHandler.MostrarError(this, "Error al cancelar la factura.");
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.LogError("Menu2CancelarFacturas", "EliminarFactura", ex);
                        ErrorHandler.MostrarError(this, "Error al cancelar la factura: " + ex.Message);
                    }
                }
                
                _btnCancelarFactura.Sensitive = true;
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("Menu2CancelarFacturas", "OnCancelarFacturaClicked", ex);
                ErrorHandler.MostrarError(this, "Error al procesar la cancelación: " + ex.Message);
                _btnCancelarFactura.Sensitive = true;
            }
        }

        protected override void OnDestroyed()
        {
            try
            {
                // Desconectar eventos
                if (_btnCancelarFactura != null)
                    _btnCancelarFactura.Clicked -= OnCancelarFacturaClicked;
                
                if (_listBoxFacturas != null)
                    _listBoxFacturas.RowSelected -= OnFacturaSeleccionada;
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("Menu2CancelarFacturas", "OnDestroyed", ex);
            }
            finally
            {
                base.OnDestroyed();
            }
        }
    }
}
