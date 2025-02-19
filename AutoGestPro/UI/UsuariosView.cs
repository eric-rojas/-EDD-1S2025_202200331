using Gtk;
using System;
using AutoGestPro.Core;

namespace AutoGestPro.UI
{
    public class UsuariosView : Window
    {
        private readonly ListaUsuarios _listaUsuarios;

        public UsuariosView(ListaUsuarios listaUsuarios) : base("AutoGestPro - Gestión de Usuarios")
        {
            _listaUsuarios = listaUsuarios;

            SetDefaultSize(400, 300);
            SetPosition(WindowPosition.Center);

            // Crear un contenedor para los elementos
            Box vbox = new Box(Orientation.Vertical, 5);

            // Label
            Label label = new Label("Gestión de Usuarios");
            vbox.PackStart(label, false, false, 0);

            // ver Usuarios
            Button Btn_VerUsuarios = new Button("Ver Usuarios");
            Btn_VerUsuarios.Clicked += GoVerUsuarios;
            vbox.PackStart(Btn_VerUsuarios, false, false, 0);

            // editar Usuarios
            Button Btn_EditarUsuarios = new Button("Editar Usuarios");
            Btn_EditarUsuarios.Clicked += GoEditarUsuarios;
            vbox.PackStart(Btn_EditarUsuarios, false, false, 0);

            // eliminar Usuarios
            Button Btn_EliminarUsuarios = new Button("Eliminar Usuarios");
            Btn_EliminarUsuarios.Clicked += GoEliminarUsuarios;
            vbox.PackStart(Btn_EliminarUsuarios, false, false, 0);

            

            

            Add(vbox);
        }

        private void GoVerUsuarios(object sender, EventArgs e)
        {
            
           
            Entry entryId = new Entry();
            MessageDialog dialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Question, ButtonsType.OkCancel, "Ingrese el ID del usuario:");
            dialog.ContentArea.PackStart(entryId, false, false, 0);
            dialog.ContentArea.ShowAll();

            if (dialog.Run() == (int)ResponseType.Ok)
            {
                int id;
                if (int.TryParse(entryId.Text, out id))
                {
                    Usuario usuario = _listaUsuarios.Buscar(id);
                    if (usuario != null)
                    {
                        MessageDialog infoDialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok, $"Usuario encontrado: {usuario.ID}, {usuario.Nombres}, {usuario.Apellidos}, {usuario.Correo}, {usuario.Contraseña}");
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
                else
                {
                    MessageDialog errorDialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "ID inválido.");
                    errorDialog.Run();
                    errorDialog.Destroy();
                }
            }

            dialog.Destroy();
        }

        private void GoEditarUsuarios(object sender, EventArgs e)
        {
            Entry entryId = new Entry();
            Entry entryNombres = new Entry();
            Entry entryApellidos = new Entry();
            Entry entryCorreo = new Entry();
            Entry entryContraseña = new Entry();

            MessageDialog dialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Question, ButtonsType.OkCancel, "Ingrese los datos del usuario:");
            dialog.ContentArea.PackStart(new Label("ID:"), false, false, 0);
            dialog.ContentArea.PackStart(entryId, false, false, 0);
            dialog.ContentArea.PackStart(new Label("Nombres:"), false, false, 0);
            dialog.ContentArea.PackStart(entryNombres, false, false, 0);
            dialog.ContentArea.PackStart(new Label("Apellidos:"), false, false, 0);
            dialog.ContentArea.PackStart(entryApellidos, false, false, 0);
            dialog.ContentArea.PackStart(new Label("Correo:"), false, false, 0);
            dialog.ContentArea.PackStart(entryCorreo, false, false, 0);
            dialog.ContentArea.PackStart(new Label("Contraseña:"), false, false, 0);
            dialog.ContentArea.PackStart(entryContraseña, false, false, 0);
            dialog.ContentArea.ShowAll();

            if (dialog.Run() == (int)ResponseType.Ok)
            {
                int id;
                if (int.TryParse(entryId.Text, out id))
                {
                    Usuario usuario = _listaUsuarios.Buscar(id);
                    if (usuario != null)
                    {
                        usuario.Nombres = entryNombres.Text;
                        usuario.Apellidos = entryApellidos.Text;
                        usuario.Correo = entryCorreo.Text;
                        usuario.Contraseña = entryContraseña.Text;

                        MessageDialog infoDialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok, "Usuario editado correctamente.");
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
                else
                {
                    MessageDialog errorDialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "ID inválido.");
                    errorDialog.Run();
                    errorDialog.Destroy();  
                }
            }

            dialog.Destroy();
        }


        private void GoEliminarUsuarios(object sender, EventArgs e)
        {
            Entry entryId = new Entry();
            MessageDialog dialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Question, ButtonsType.OkCancel, "Ingrese el ID del usuario:");
            dialog.ContentArea.PackStart(entryId, false, false, 0);
            dialog.ContentArea.ShowAll();

            if (dialog.Run() == (int)ResponseType.Ok)
            {
                int id;
                if (int.TryParse(entryId.Text, out id))
                {
                    Usuario usuario = _listaUsuarios.Buscar(id);
                    if (usuario != null)
                    {
                        _listaUsuarios.Eliminar(usuario.ID);
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
                else
                {
                    MessageDialog errorDialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "ID inválido.");
                    errorDialog.Run();
                    errorDialog.Destroy();
                }
            }

            dialog.Destroy();
        }


       
    }














}