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
}