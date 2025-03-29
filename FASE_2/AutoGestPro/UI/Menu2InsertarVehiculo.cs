/*using Gtk;
using System;
using AutoGestPro.Core;
using AutoGestPro.UI;
public class Menu2InsertarVehiculo : Window
{
    private Usuario usuario;
    private ListaVehiculos listaVehiculos;
    private Entry txtId, txtMarca, txtModelo, txtPlaca;

    public Menu2InsertarVehiculo(Usuario usuario, ListaVehiculos listaVehiculos) : base("Registrar Vehículo")
    {
        this.usuario = usuario;
        this.listaVehiculos = listaVehiculos;
        
        SetDefaultSize(300, 250);
        BuildUI();
    }

    private void BuildUI()
    {
        var vbox = new VBox();
        
        // Campo ID
        var lblId = new Label("ID del vehículo:");
        txtId = new Entry();
        vbox.PackStart(lblId, false, false, 5);
        vbox.PackStart(txtId, false, false, 5);
        
        // ... Agregar más campos (Marca, Modelo, Placa) de la misma forma ...
        // Campo Marca
        var lblMarca = new Label("Marca:");
        txtMarca = new Entry();
        vbox.PackStart(lblMarca, false, false, 5);
        vbox.PackStart(txtMarca, false, false, 5);

        // Campo Modelo
        var lblModelo = new Label("Modelo:");
        txtModelo = new Entry();
        vbox.PackStart(lblModelo, false, false, 5);
        vbox.PackStart(txtModelo, false, false, 5);

        // Campo Placa
        var lblPlaca = new Label("Placa:");
        txtPlaca = new Entry();
        vbox.PackStart(lblPlaca, false, false, 5);
        vbox.PackStart(txtPlaca, false, false, 5);
        
        var btnGuardar = new Button("Guardar");
        btnGuardar.Clicked += OnGuardarClicked;
        vbox.PackStart(btnGuardar, false, false, 10);
        
        Add(vbox);
    }

    private void OnGuardarClicked(object sender, EventArgs e)
    {
        if (!int.TryParse(txtId.Text, out int idVehiculo))
        {
            MostrarError("ID inválido");
            return;
        }
        
        if (listaVehiculos.ExisteID(idVehiculo))
        {
            MostrarError("Este ID de vehículo ya está registrado");
            return;
        }
        
        var nuevoVehiculo = new Vehiculo(
            idVehiculo,
            usuario.ID,
            txtMarca.Text,
            txtModelo.Text,
            txtPlaca.Text
        );
        
        listaVehiculos.Insertar(nuevoVehiculo);
        MostrarExito("Vehículo registrado exitosamente");
        Destroy();
    }
    
    private void MostrarError(string mensaje)
    {
       
        var dialog = new MessageDialog(
            this,
            DialogFlags.Modal,
            MessageType.Error,
            ButtonsType.Ok,
            mensaje
        );
        dialog.Run();
        dialog.Destroy();
    }
    
    private void MostrarExito(string mensaje)
    {
        
        var dialog = new MessageDialog(
            this,
            DialogFlags.Modal,
            MessageType.Info,
            ButtonsType.Ok,
            mensaje
        );
        dialog.Run();
        dialog.Destroy();
    }
}*/


using Gtk;
using System;
using AutoGestPro.Core;
using AutoGestPro.UI;
using AutoGestPro.Utils;

namespace AutoGestPro.UI
{
    public class Menu2InsertarVehiculo : Window
    {
        private readonly Usuario _usuario;
        private readonly ListaVehiculos _listaVehiculos;
        private Entry _txtId, _txtMarca, _txtModelo, _txtPlaca;
        private Button _btnGuardar;

        public Menu2InsertarVehiculo(Usuario usuario, ListaVehiculos listaVehiculos) : base("Registrar Vehículo")
        {
            try
            {
                _usuario = usuario;
                _listaVehiculos = listaVehiculos;
                
                SetDefaultSize(300, 250);
                SetPosition(WindowPosition.Center);
                
                // Configurar evento de cierre
                DeleteEvent += (o, args) => {
                    try
                    {
                        Destroy();
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.LogError("Menu2InsertarVehiculo", "DeleteEvent", ex);
                    }
                };
                
                BuildUI();
                ShowAll();
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("Menu2InsertarVehiculo", "Constructor", ex);
                ErrorHandler.MostrarError(this, "Error al inicializar la ventana: " + ex.Message);
            }
        }

        private void BuildUI()
        {
            try
            {
                var vbox = new VBox();
                vbox.Margin = 10;
                vbox.Spacing = 5;
                
                // Título
                var lblTitulo = new Label("Registro de Nuevo Vehículo");
                lblTitulo.MarginBottom = 10;
                vbox.PackStart(lblTitulo, false, false, 0);
                
                // Campo ID
                var lblId = new Label("ID del vehículo:");
                _txtId = new Entry();
                vbox.PackStart(lblId, false, false, 0);
                vbox.PackStart(_txtId, false, false, 5);
                
                // Campo Marca
                var lblMarca = new Label("Marca:");
                _txtMarca = new Entry();
                vbox.PackStart(lblMarca, false, false, 0);
                vbox.PackStart(_txtMarca, false, false, 5);

                // Campo Modelo
                var lblModelo = new Label("Modelo:");
                _txtModelo = new Entry();
                vbox.PackStart(lblModelo, false, false, 0);
                vbox.PackStart(_txtModelo, false, false, 5);

                // Campo Placa
                var lblPlaca = new Label("Placa:");
                _txtPlaca = new Entry();
                vbox.PackStart(lblPlaca, false, false, 0);
                vbox.PackStart(_txtPlaca, false, false, 5);
                
                // Mostrar información del usuario
                var lblUsuarioInfo = new Label($"Usuario: {_usuario.Nombres} {_usuario.Apellidos} (ID: {_usuario.ID})");
                lblUsuarioInfo.Halign = Align.Start;
                lblUsuarioInfo.MarginTop = 10;
                vbox.PackStart(lblUsuarioInfo, false, false, 0);
                
                // Botón guardar
                _btnGuardar = new Button("Guardar");
                _btnGuardar.Clicked += OnGuardarClicked;
                _btnGuardar.MarginTop = 10;
                vbox.PackStart(_btnGuardar, false, false, 0);
                
                Add(vbox);
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("Menu2InsertarVehiculo", "BuildUI", ex);
                throw; // Relanzamos para que sea manejada por el constructor
            }
        }

        private void OnGuardarClicked(object sender, EventArgs e)
        {
            try
            {
                // Deshabilitar botón mientras se procesa
                _btnGuardar.Sensitive = false;
                
                // Validar ID
                if (string.IsNullOrWhiteSpace(_txtId.Text))
                {
                    MostrarError("Debe ingresar un ID para el vehículo");
                    _btnGuardar.Sensitive = true;
                    return;
                }
                
                if (!int.TryParse(_txtId.Text, out int idVehiculo))
                {
                    MostrarError("ID inválido. Debe ser un número entero.");
                    _btnGuardar.Sensitive = true;
                    return;
                }
                
                // Validar si ya existe el ID
                try
                {
                    if (_listaVehiculos.ExisteID(idVehiculo))
                    {
                        MostrarError("Este ID de vehículo ya está registrado");
                        _btnGuardar.Sensitive = true;
                        return;
                    }
                }
                catch (Exception ex)
                {
                    ErrorHandler.LogError("Menu2InsertarVehiculo", "ComprobarIDExistente", ex);
                    MostrarError("Error al comprobar si el ID ya existe: " + ex.Message);
                    _btnGuardar.Sensitive = true;
                    return;
                }
                
                // Validar otros campos
                if (string.IsNullOrWhiteSpace(_txtMarca.Text))
                {
                    MostrarError("Debe ingresar la marca del vehículo");
                    _btnGuardar.Sensitive = true;
                    return;
                }
                
                if (string.IsNullOrWhiteSpace(_txtModelo.Text))
                {
                    MostrarError("Debe ingresar el modelo del vehículo");
                    _btnGuardar.Sensitive = true;
                    return;
                }
                
                if (string.IsNullOrWhiteSpace(_txtPlaca.Text))
                {
                    MostrarError("Debe ingresar la placa del vehículo");
                    _btnGuardar.Sensitive = true;
                    return;
                }
                
                // Crear y guardar el vehículo
                try
                {
                    var nuevoVehiculo = new Vehiculo(
                        idVehiculo,
                        _usuario.ID,
                        _txtMarca.Text.Trim(),
                        _txtModelo.Text.Trim(),
                        _txtPlaca.Text.Trim()
                    );
                    
                    bool resultado = _listaVehiculos.Insertar(nuevoVehiculo);
                    
                    if (resultado)
                    {
                        MostrarExito("Vehículo registrado exitosamente");
                        Destroy();
                    }
                    else
                    {
                        MostrarError("No se pudo registrar el vehículo");
                        _btnGuardar.Sensitive = true;
                    }
                }
                catch (Exception ex)
                {
                    ErrorHandler.LogError("Menu2InsertarVehiculo", "InsertarVehiculo", ex);
                    MostrarError("Error al registrar el vehículo: " + ex.Message);
                    _btnGuardar.Sensitive = true;
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("Menu2InsertarVehiculo", "OnGuardarClicked", ex);
                MostrarError("Error al procesar el registro: " + ex.Message);
                _btnGuardar.Sensitive = true;
            }
        }
        
        private void MostrarError(string mensaje)
        {
            try
            {
                using (var dialog = new MessageDialog(
                    this,
                    DialogFlags.Modal,
                    MessageType.Error,
                    ButtonsType.Ok,
                    mensaje))
                {
                    dialog.Run();
                    dialog.Destroy();
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("Menu2InsertarVehiculo", "MostrarError", ex);
                Console.WriteLine($"Error al mostrar diálogo de error: {ex.Message}");
            }
        }
        
        private void MostrarExito(string mensaje)
        {
            try
            {
                using (var dialog = new MessageDialog(
                    this,
                    DialogFlags.Modal,
                    MessageType.Info,
                    ButtonsType.Ok,
                    mensaje))
                {
                    dialog.Run();
                    dialog.Destroy();
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("Menu2InsertarVehiculo", "MostrarExito", ex);
                Console.WriteLine($"Error al mostrar diálogo de éxito: {ex.Message}");
            }
        }
        
        protected override void OnDestroyed()
        {
            try
            {
                // Desconectar eventos
                if (_btnGuardar != null)
                    _btnGuardar.Clicked -= OnGuardarClicked;
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("Menu2InsertarVehiculo", "OnDestroyed", ex);
            }
            finally
            {
                base.OnDestroyed();
            }
        }
    }
}