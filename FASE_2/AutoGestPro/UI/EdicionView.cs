/*
//anterior
// se puede ver y editar usuarios y vehículos
using Gtk;
using System;
using AutoGestPro.Core;

namespace AutoGestPro.UI
{
    public class EdicionView : Window
    {
        private readonly ListaUsuarios _listaUsuarios;
        private readonly ListaVehiculos _listaVehiculos;

        public EdicionView(ListaUsuarios listaUsuarios, ListaVehiculos listaVehiculos) : base("AutoGestPro - Gestión de Usuarios")
        {
            _listaUsuarios = listaUsuarios;
            _listaVehiculos = listaVehiculos;

            SetDefaultSize(400, 300);
            SetPosition(WindowPosition.Center);

            // Crear un contenedor para los elementos
            Box vbox = new Box(Orientation.Vertical, 5);

            // Label Usuarios
            Label labelUsuarios = new Label("Gestión de Usuarios");
            vbox.PackStart(labelUsuarios, false, false, 0);

            // ver Usuarios
            Button Btn_VerUsuarios = new Button("Ver Usuarios");
            Btn_VerUsuarios.Clicked += GoVerUsuarios;
            vbox.PackStart(Btn_VerUsuarios, false, false, 0);

            // eliminar Usuarios
            Button Btn_EliminarUsuarios = new Button("Eliminar Usuarios");
            Btn_EliminarUsuarios.Clicked += GoEliminarUsuarios;
            vbox.PackStart(Btn_EliminarUsuarios, false, false, 0);

            // Separator
            vbox.PackStart(new Separator(Orientation.Horizontal), false, false, 10);

            // Label Vehículos
            Label labelVehiculos = new Label("Gestión de Vehículos");
            vbox.PackStart(labelVehiculos, false, false, 0);

            // ver Vehículos
            Button Btn_VerVehiculos = new Button("Ver Vehículos");
            Btn_VerVehiculos.Clicked += GoVerVehiculos;
            vbox.PackStart(Btn_VerVehiculos, false, false, 0);

            // eliminar Vehículos
            Button Btn_EliminarVehiculos = new Button("Eliminar Vehículos");
            Btn_EliminarVehiculos.Clicked += GoEliminarVehiculos;
            vbox.PackStart(Btn_EliminarVehiculos, false, false, 0);

            Add(vbox);
        }

        private void GoVerUsuarios(object sender, EventArgs e)
        {
            Entry entryID = new Entry();
            MessageDialog dialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Question, ButtonsType.OkCancel, "Ingrese el ID:");
            dialog.ContentArea.PackStart(entryID, false, false, 0);
            dialog.ContentArea.ShowAll();

            if (dialog.Run() == (int)ResponseType.Ok)
            {
                string ID = entryID.Text.Trim();
                if (int.TryParse(ID, out int idNumerico))
                {
                    Usuario usuario = _listaUsuarios.Buscar(idNumerico);
                if (usuario != null)
                {
                    MessageDialog infoDialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok, 
                        $"Usuario encontrado:\nID: {usuario.ID}\nNombres: {usuario.Nombres}\nApellidos: {usuario.Apellidos}\nCorreo: {usuario.Correo}\nContraseña: {usuario.Contrasenia}\nEdad: {usuario.Edad}");
                    infoDialog.Run();
                    infoDialog.Destroy();
                }
                else
                {
                    MessageDialog errorDialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "Usuario no encontrado.");
                    errorDialog.Run();
                    errorDialog.Destroy();
                }
            }
            dialog.Destroy();
        }
        }

        

        private void GoEliminarUsuarios(object sender, EventArgs e)
        {
            Entry entryID  = new Entry();
            MessageDialog dialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Question, ButtonsType.OkCancel, "Ingrese el ID del usuario a eliminar:");
            dialog.ContentArea.PackStart(entryID , false, false, 0);
            dialog.ContentArea.ShowAll();

            if (dialog.Run() == (int)ResponseType.Ok)
            {
                string ID = entryID.Text;
                if (int.TryParse(ID, out int idNumerico))
                {
                    Usuario usuario = _listaUsuarios.Buscar(idNumerico);
                if (usuario != null)
                {
                    _listaUsuarios.Eliminar(idNumerico);
                    MessageDialog infoDialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok, "Usuario eliminado correctamente.");
                    infoDialog.Run();
                    infoDialog.Destroy();
                }
                else
                {
                    MessageDialog errorDialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "Usuario no encontrado.");
                    errorDialog.Run();
                    errorDialog.Destroy();
                }
            }
            dialog.Destroy();
        }
        }
        private void GoVerVehiculos(object sender, EventArgs e)
        {
            Entry entryID = new Entry();
            MessageDialog dialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Question, ButtonsType.OkCancel, "Ingrese el ID:");
            dialog.ContentArea.PackStart(entryID, false, false, 0);
            dialog.ContentArea.ShowAll();

            if (dialog.Run() == (int)ResponseType.Ok)
            {
            string ID = entryID.Text.Trim();
            if (int.TryParse(ID, out int idNumerico))
            {
                Vehiculo vehiculo = _listaVehiculos.Buscar(idNumerico);
                if (vehiculo != null)
                {
                MessageDialog infoDialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok, 
                    $"Vehículo encontrado:\nID: {vehiculo.ID}\nMarca: {vehiculo.Marca}\nModelo: {vehiculo.Modelo}\nID_Usuario: {vehiculo.ID_Usuario}\nPlaca: {vehiculo.Placa}");
                infoDialog.Run();
                infoDialog.Destroy();
                }
                else
                {
                MessageDialog errorDialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "Vehículo no encontrado.");
                errorDialog.Run();
                errorDialog.Destroy();
                }
            }
            }
            dialog.Destroy();
        }

        private void GoEliminarVehiculos(object sender, EventArgs e)
        {
            Entry entryID = new Entry();
            MessageDialog dialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Question, ButtonsType.OkCancel, "Ingrese el ID del vehículo a eliminar:");
            dialog.ContentArea.PackStart(entryID, false, false, 0);
            dialog.ContentArea.ShowAll();

            if (dialog.Run() == (int)ResponseType.Ok)
            {
            string ID = entryID.Text.Trim();
            if (int.TryParse(ID, out int idNumerico))
            {
                Vehiculo vehiculo = _listaVehiculos.Buscar(idNumerico);
                if (vehiculo != null)
                {
                _listaVehiculos.Eliminar(idNumerico);
                MessageDialog infoDialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok, "Vehículo eliminado correctamente.");
                infoDialog.Run();
                infoDialog.Destroy();
                }
                else
                {
                MessageDialog errorDialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "Vehículo no encontrado.");
                errorDialog.Run();
                errorDialog.Destroy();
                }
            }
            }
            dialog.Destroy();
        }
    }
}*/

using Gtk;
using System;
using System.Collections.Generic;
using AutoGestPro.Core;
using AutoGestPro.Utils;

namespace AutoGestPro.UI
{
    public class EdicionView : Window
    {
        private readonly ListaUsuarios _listaUsuarios;
        private readonly ListaVehiculos _listaVehiculos;
        private Dictionary<string, Button> _botones = new Dictionary<string, Button>();

        public EdicionView(ListaUsuarios listaUsuarios, ListaVehiculos listaVehiculos) : base("AutoGestPro - Gestión de Usuarios")
        {
            try
            {
                _listaUsuarios = listaUsuarios;
                _listaVehiculos = listaVehiculos;

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
                        ErrorHandler.LogError("EdicionView", "DeleteEvent", ex);
                    }
                };

                CrearInterfaz();
                ShowAll();
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("EdicionView", "Constructor", ex);
                ErrorHandler.MostrarError(this, "Error al inicializar la ventana de gestión: " + ex.Message);
            }
        }
        
        private void CrearInterfaz()
        {
            try
            {
                // Crear un contenedor para los elementos
                Box vbox = new Box(Orientation.Vertical, 5);
                vbox.Margin = 10;

                // Label Usuarios
                Label labelUsuarios = new Label("Gestión de Usuarios");
                labelUsuarios.MarginTop = 5;
                vbox.PackStart(labelUsuarios, false, false, 0);

                AgregarBoton(vbox, "Ver Usuarios", GoVerUsuarios);
                AgregarBoton(vbox, "Eliminar Usuarios", GoEliminarUsuarios);

                // Separator
                vbox.PackStart(new Separator(Orientation.Horizontal), false, false, 10);

                // Label Vehículos
                Label labelVehiculos = new Label("Gestión de Vehículos");
                labelVehiculos.MarginTop = 5;
                vbox.PackStart(labelVehiculos, false, false, 0);

                AgregarBoton(vbox, "Ver Vehículos", GoVerVehiculos);
                AgregarBoton(vbox, "Eliminar Vehículos", GoEliminarVehiculos);

                Add(vbox);
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("EdicionView", "CrearInterfaz", ex);
                throw; // Relanzamos para que sea manejada por el constructor
            }
        }
        
        private void AgregarBoton(Box vbox, string texto, EventHandler handler)
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
                ErrorHandler.LogError("EdicionView", "AgregarBoton", ex);
                throw;
            }
        }

        private void GoVerUsuarios(object sender, EventArgs e)
        {
            try
            {
                _botones["Ver Usuarios"].Sensitive = false;

                using (Dialog dialog = new Dialog(
                    "Buscar Usuario", 
                    this,
                    DialogFlags.Modal,
                    "Cancelar", ResponseType.Cancel,
                    "Buscar", ResponseType.Ok))
                {
                    dialog.SetDefaultSize(300, 100);
                    dialog.SetPosition(WindowPosition.Center);
                    
                    Box content = dialog.ContentArea;
                    content.Spacing = 5;
                    content.Margin = 10;
                    
                    content.PackStart(new Label("ID del Usuario:"), false, false, 0);
                    Entry entryID = new Entry();
                    content.PackStart(entryID, false, false, 5);
                    
                    content.ShowAll();
                    
                    if (dialog.Run() == (int)ResponseType.Ok)
                    {
                        string ID = entryID.Text?.Trim() ?? string.Empty;
                        
                        if (string.IsNullOrEmpty(ID))
                        {
                            dialog.Destroy();
                            ErrorHandler.MostrarError(this, "Debe ingresar un ID.");
                            _botones["Ver Usuarios"].Sensitive = true;
                            return;
                        }
                        
                        if (!int.TryParse(ID, out int idNumerico))
                        {
                            dialog.Destroy();
                            ErrorHandler.MostrarError(this, "El ID debe ser un número entero.");
                            _botones["Ver Usuarios"].Sensitive = true;
                            return;
                        }
                        
                        dialog.Destroy();
                        
                        try
                        {
                            Usuario usuario = _listaUsuarios.Buscar(idNumerico);
                            
                            if (usuario != null)
                            {
                                string mensaje = $"Usuario encontrado:\n\n" +
                                                $"ID: {usuario.ID}\n" +
                                                $"Nombres: {usuario.Nombres}\n" +
                                                $"Apellidos: {usuario.Apellidos}\n" +
                                                $"Correo: {usuario.Correo}\n" +
                                                $"Contraseña: {usuario.Contrasenia}\n" +
                                                $"Edad: {usuario.Edad}";
                                
                                ErrorHandler.MostrarInfo(this, mensaje);
                            }
                            else
                            {
                                ErrorHandler.MostrarError(this, "Usuario no encontrado.");
                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorHandler.LogError("EdicionView", "BuscarUsuario", ex);
                            ErrorHandler.MostrarError(this, "Error al buscar usuario: " + ex.Message);
                        }
                    }
                    else
                    {
                        dialog.Destroy();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("EdicionView", "GoVerUsuarios", ex);
                ErrorHandler.MostrarError(this, "Error al mostrar el diálogo: " + ex.Message);
            }
            finally
            {
                _botones["Ver Usuarios"].Sensitive = true;
            }
        }

        private void GoEliminarUsuarios(object sender, EventArgs e)
        {
            try
            {
                _botones["Eliminar Usuarios"].Sensitive = false;

                using (Dialog dialog = new Dialog(
                    "Eliminar Usuario", 
                    this,
                    DialogFlags.Modal,
                    "Cancelar", ResponseType.Cancel,
                    "Buscar", ResponseType.Ok))
                {
                    dialog.SetDefaultSize(300, 100);
                    dialog.SetPosition(WindowPosition.Center);
                    
                    Box content = dialog.ContentArea;
                    content.Spacing = 5;
                    content.Margin = 10;
                    
                    content.PackStart(new Label("ID del Usuario:"), false, false, 0);
                    Entry entryID = new Entry();
                    content.PackStart(entryID, false, false, 5);
                    
                    content.ShowAll();
                    
                    if (dialog.Run() == (int)ResponseType.Ok)
                    {
                        string ID = entryID.Text?.Trim() ?? string.Empty;
                        
                        if (string.IsNullOrEmpty(ID))
                        {
                            dialog.Destroy();
                            ErrorHandler.MostrarError(this, "Debe ingresar un ID.");
                            _botones["Eliminar Usuarios"].Sensitive = true;
                            return;
                        }
                        
                        if (!int.TryParse(ID, out int idNumerico))
                        {
                            dialog.Destroy();
                            ErrorHandler.MostrarError(this, "El ID debe ser un número entero.");
                            _botones["Eliminar Usuarios"].Sensitive = true;
                            return;
                        }
                        
                        dialog.Destroy();
                        
                        try
                        {
                            Usuario usuario = _listaUsuarios.Buscar(idNumerico);
                            
                            if (usuario != null)
                            {
                                // Confirmar eliminación
                                MessageDialog confirmDialog = new MessageDialog(
                                    this,
                                    DialogFlags.Modal,
                                    MessageType.Question,
                                    ButtonsType.YesNo,
                                    $"¿Está seguro de eliminar el usuario '{usuario.Nombres} {usuario.Apellidos}'?"
                                );
                                
                                ResponseType response = (ResponseType)confirmDialog.Run();
                                confirmDialog.Destroy();
                                
                                if (response == ResponseType.Yes)
                                {
                                    _listaUsuarios.Eliminar(idNumerico);
                                    ErrorHandler.MostrarInfo(this, "Usuario eliminado correctamente.");
                                }
                            }
                            else
                            {
                                ErrorHandler.MostrarError(this, "Usuario no encontrado.");
                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorHandler.LogError("EdicionView", "EliminarUsuario", ex);
                            ErrorHandler.MostrarError(this, "Error al eliminar usuario: " + ex.Message);
                        }
                    }
                    else
                    {
                        dialog.Destroy();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("EdicionView", "GoEliminarUsuarios", ex);
                ErrorHandler.MostrarError(this, "Error al mostrar el diálogo: " + ex.Message);
            }
            finally
            {
                _botones["Eliminar Usuarios"].Sensitive = true;
            }
        }
        
        private void GoVerVehiculos(object sender, EventArgs e)
        {
            try
            {
                _botones["Ver Vehículos"].Sensitive = false;

                using (Dialog dialog = new Dialog(
                    "Buscar Vehículo", 
                    this,
                    DialogFlags.Modal,
                    "Cancelar", ResponseType.Cancel,
                    "Buscar", ResponseType.Ok))
                {
                    dialog.SetDefaultSize(300, 100);
                    dialog.SetPosition(WindowPosition.Center);
                    
                    Box content = dialog.ContentArea;
                    content.Spacing = 5;
                    content.Margin = 10;
                    
                    content.PackStart(new Label("ID del Vehículo:"), false, false, 0);
                    Entry entryID = new Entry();
                    content.PackStart(entryID, false, false, 5);
                    
                    content.ShowAll();
                    
                    if (dialog.Run() == (int)ResponseType.Ok)
                    {
                        string ID = entryID.Text?.Trim() ?? string.Empty;
                        
                        if (string.IsNullOrEmpty(ID))
                        {
                            dialog.Destroy();
                            ErrorHandler.MostrarError(this, "Debe ingresar un ID.");
                            _botones["Ver Vehículos"].Sensitive = true;
                            return;
                        }
                        
                        if (!int.TryParse(ID, out int idNumerico))
                        {
                            dialog.Destroy();
                            ErrorHandler.MostrarError(this, "El ID debe ser un número entero.");
                            _botones["Ver Vehículos"].Sensitive = true;
                            return;
                        }
                        
                        dialog.Destroy();
                        
                        try
                        {
                            Vehiculo vehiculo = _listaVehiculos.Buscar(idNumerico);
                            
                            if (vehiculo != null)
                            {
                                string mensaje = $"Vehículo encontrado:\n\n" +
                                                $"ID: {vehiculo.ID}\n" +
                                                $"ID Usuario: {vehiculo.ID_Usuario}\n" +
                                                $"Marca: {vehiculo.Marca}\n" +
                                                $"Modelo: {vehiculo.Modelo}\n" +
                                                $"Placa: {vehiculo.Placa}";
                                
                                ErrorHandler.MostrarInfo(this, mensaje);
                            }
                            else
                            {
                                ErrorHandler.MostrarError(this, "Vehículo no encontrado.");
                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorHandler.LogError("EdicionView", "BuscarVehiculo", ex);
                            ErrorHandler.MostrarError(this, "Error al buscar vehículo: " + ex.Message);
                        }
                    }
                    else
                    {
                        dialog.Destroy();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("EdicionView", "GoVerVehiculos", ex);
                ErrorHandler.MostrarError(this, "Error al mostrar el diálogo: " + ex.Message);
            }
            finally
            {
                _botones["Ver Vehículos"].Sensitive = true;
            }
        }

        private void GoEliminarVehiculos(object sender, EventArgs e)
        {
            try
            {
                _botones["Eliminar Vehículos"].Sensitive = false;

                using (Dialog dialog = new Dialog(
                    "Eliminar Vehículo", 
                    this,
                    DialogFlags.Modal,
                    "Cancelar", ResponseType.Cancel,
                    "Buscar", ResponseType.Ok))
                {
                    dialog.SetDefaultSize(300, 100);
                    dialog.SetPosition(WindowPosition.Center);
                    
                    Box content = dialog.ContentArea;
                    content.Spacing = 5;
                    content.Margin = 10;
                    
                    content.PackStart(new Label("ID del Vehículo:"), false, false, 0);
                    Entry entryID = new Entry();
                    content.PackStart(entryID, false, false, 5);
                    
                    content.ShowAll();
                    
                    if (dialog.Run() == (int)ResponseType.Ok)
                    {
                        string ID = entryID.Text?.Trim() ?? string.Empty;
                        
                        if (string.IsNullOrEmpty(ID))
                        {
                            dialog.Destroy();
                            ErrorHandler.MostrarError(this, "Debe ingresar un ID.");
                            _botones["Eliminar Vehículos"].Sensitive = true;
                            return;
                        }
                        
                        if (!int.TryParse(ID, out int idNumerico))
                        {
                            dialog.Destroy();
                            ErrorHandler.MostrarError(this, "El ID debe ser un número entero.");
                            _botones["Eliminar Vehículos"].Sensitive = true;
                            return;
                        }
                        
                        dialog.Destroy();
                        
                        try
                        {
                            Vehiculo vehiculo = _listaVehiculos.Buscar(idNumerico);
                            
                            if (vehiculo != null)
                            {
                                // Confirmar eliminación
                                MessageDialog confirmDialog = new MessageDialog(
                                    this,
                                    DialogFlags.Modal,
                                    MessageType.Question,
                                    ButtonsType.YesNo,
                                    $"¿Está seguro de eliminar el vehículo {vehiculo.Marca} {vehiculo.Modelo} ({vehiculo.Placa})?"
                                );
                                
                                ResponseType response = (ResponseType)confirmDialog.Run();
                                confirmDialog.Destroy();
                                
                                if (response == ResponseType.Yes)
                                {
                                    _listaVehiculos.Eliminar(idNumerico);
                                    ErrorHandler.MostrarInfo(this, "Vehículo eliminado correctamente.");
                                }
                            }
                            else
                            {
                                ErrorHandler.MostrarError(this, "Vehículo no encontrado.");
                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorHandler.LogError("EdicionView", "EliminarVehiculo", ex);
                            ErrorHandler.MostrarError(this, "Error al eliminar vehículo: " + ex.Message);
                        }
                    }
                    else
                    {
                        dialog.Destroy();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("EdicionView", "GoEliminarVehiculos", ex);
                ErrorHandler.MostrarError(this, "Error al mostrar el diálogo: " + ex.Message);
            }
            finally
            {
                _botones["Eliminar Vehículos"].Sensitive = true;
            }
        }
        
        protected override void OnDestroyed()
        {
            try
            {
                // Desconectar todos los eventos
                foreach (var kvp in _botones)
                {
                    Button btn = kvp.Value;
                    string texto = kvp.Key;
                    
                    try
                    {
                        switch (texto)
                        {
                            case "Ver Usuarios":
                                btn.Clicked -= GoVerUsuarios;
                                break;
                            case "Eliminar Usuarios":
                                btn.Clicked -= GoEliminarUsuarios;
                                break;
                            case "Ver Vehículos":
                                btn.Clicked -= GoVerVehiculos;
                                break;
                            case "Eliminar Vehículos":
                                btn.Clicked -= GoEliminarVehiculos;
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.LogError("EdicionView", "DesconectarEvento_" + texto, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("EdicionView", "OnDestroyed", ex);
            }
            finally
            {
                base.OnDestroyed();
            }
        }
    }
}