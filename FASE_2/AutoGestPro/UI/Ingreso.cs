//YA NO SE USA
// ingreso de vehiculos y usuarios

using Gtk;
using System;
using AutoGestPro.Core;

namespace AutoGestPro.UI
{
    public class Ingreso : Window
    {
        private readonly ListaUsuarios _listaUsuarios;
        private readonly ListaVehiculos _listaVehiculos;

        public Ingreso(ListaUsuarios listaUsuarios, ListaVehiculos listaVehiculos) : base("AutoGestPro - Ingreso")
        {
            _listaUsuarios = listaUsuarios;
            _listaVehiculos = listaVehiculos;

            SetDefaultSize(400, 300);
            SetPosition(WindowPosition.Center);

            // Crear un contenedor para los elementos
            Box vbox = new Box(Orientation.Vertical, 5);

            // Label
            Label label = new Label("Ingreso Manual");
            vbox.PackStart(label, false, false, 0);

            // ingreso usuarios
            Button Btn_VerUsuarios = new Button("Ingreso Usuarios");
            Btn_VerUsuarios.Clicked += GoUsuarios;
            vbox.PackStart(Btn_VerUsuarios, false, false, 0);

            // ingreso vehiculos
            Button Btn_VerVehiculos = new Button("Ingreso Vehiculos");
            Btn_VerVehiculos.Clicked += GoVehiculos;
            vbox.PackStart(Btn_VerVehiculos, false, false, 0);

            Add(vbox);
        }

        private void GoUsuarios(object sender, EventArgs e)
        {
            Entry entryId = new Entry();
            MessageDialog dialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Question, ButtonsType.OkCancel, "Ingrese el ID del usuario:");
            dialog.ContentArea.PackStart(entryId, false, false, 0);
            dialog.ContentArea.ShowAll();

            if (dialog.Run() == (int)ResponseType.Ok)
            {
                int id;
                if (!int.TryParse(entryId.Text, out id))
                {
                    MessageDialog errorDialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "El ID debe ser un número entero.");
                    errorDialog.Run();
                    errorDialog.Destroy();
                    return;
                }

                dialog.Destroy();

                Entry entryNombres = new Entry();
                MessageDialog dialogNombres = new MessageDialog(this, DialogFlags.Modal, MessageType.Question, ButtonsType.OkCancel, "Ingrese los nombres:");
                dialogNombres.ContentArea.PackStart(entryNombres, false, false, 0);
                dialogNombres.ContentArea.ShowAll();

                if (dialogNombres.Run() == (int)ResponseType.Ok)
                {
                    string nombres = entryNombres.Text;
                    dialogNombres.Destroy();

                    Entry entryApellidos = new Entry();
                    MessageDialog dialogApellidos = new MessageDialog(this, DialogFlags.Modal, MessageType.Question, ButtonsType.OkCancel, "Ingrese los apellidos:");
                    dialogApellidos.ContentArea.PackStart(entryApellidos, false, false, 0);
                    dialogApellidos.ContentArea.ShowAll();

                    if (dialogApellidos.Run() == (int)ResponseType.Ok)
                    {
                        string apellidos = entryApellidos.Text;
                        dialogApellidos.Destroy();

                        Entry entryCorreo = new Entry();
                        MessageDialog dialogCorreo = new MessageDialog(this, DialogFlags.Modal, MessageType.Question, ButtonsType.OkCancel, "Ingrese el correo:");
                        dialogCorreo.ContentArea.PackStart(entryCorreo, false, false, 0);
                        dialogCorreo.ContentArea.ShowAll();

                        if (dialogCorreo.Run() == (int)ResponseType.Ok)
                        {
                            string correo = entryCorreo.Text;
                            dialogCorreo.Destroy();

                            Entry entryEdad = new Entry();
                            MessageDialog dialogEdad = new MessageDialog(this, DialogFlags.Modal, MessageType.Question, ButtonsType.OkCancel, "Ingrese la edad:");
                            dialogEdad.ContentArea.PackStart(entryEdad, false, false, 0);
                            dialogEdad.ContentArea.ShowAll();

                            if (dialogEdad.Run() == (int)ResponseType.Ok)
                            {
                                int edad;
                                if (!int.TryParse(entryEdad.Text, out edad))
                                {
                                    MessageDialog errorDialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "La edad debe ser un número entero.");
                                    errorDialog.Run();
                                    errorDialog.Destroy();
                                    return;
                                }
                                dialogEdad.Destroy();

                                Entry entryContrasenia = new Entry();
                                MessageDialog dialogContrasenia = new MessageDialog(this, DialogFlags.Modal, MessageType.Question, ButtonsType.OkCancel, "Ingrese la contraseña:");
                                dialogContrasenia.ContentArea.PackStart(entryContrasenia, false, false, 0);
                                dialogContrasenia.ContentArea.ShowAll();

                                if (dialogContrasenia.Run() == (int)ResponseType.Ok)
                                {
                                    string contrasenia = entryContrasenia.Text;
                                    dialogContrasenia.Destroy();

                                    // Creación y almacenamiento del usuario
                                    Usuario nuevoUsuario = new Usuario(id, nombres, apellidos, correo, edad, contrasenia);
                                    _listaUsuarios.Insertar(nuevoUsuario);
                                }
                            }
                        }
                    }
                }
            }
            dialog.Destroy();
        }


        private void GoVehiculos(object sender, EventArgs e)
        {
            Entry entryId = new Entry();
            Entry entryIdUsuario = new Entry();
            Entry entryMarca = new Entry();
            Entry entryModelo = new Entry();
            Entry entryPlaca = new Entry();

            MessageDialog dialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Question, ButtonsType.OkCancel, "Ingrese los datos del vehículo:");
            Box box = new Box(Orientation.Vertical, 5);
            box.PackStart(new Label("ID:"), false, false, 0);
            box.PackStart(entryId, false, false, 0);
            box.PackStart(new Label("ID Usuario:"), false, false, 0);
            box.PackStart(entryIdUsuario, false, false, 0);
            box.PackStart(new Label("Marca:"), false, false, 0);
            box.PackStart(entryMarca, false, false, 0);
            box.PackStart(new Label("Modelo:"), false, false, 0);
            box.PackStart(entryModelo, false, false, 0);
            box.PackStart(new Label("Placa:"), false, false, 0);
            box.PackStart(entryPlaca, false, false, 0);
            dialog.ContentArea.PackStart(box, false, false, 0);
            dialog.ContentArea.ShowAll();

            if (dialog.Run() == (int)ResponseType.Ok)
            {
                int id, id_Usuario;
                if (!int.TryParse(entryId.Text, out id) || !int.TryParse(entryIdUsuario.Text, out id_Usuario))
                {
                    MessageDialog errorDialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "El ID y el ID de usuario deben ser números enteros.");
                    errorDialog.Run();
                    errorDialog.Destroy();
                }
                else
                {
                    string marca = entryMarca.Text;
                    string modelo = entryModelo.Text;
                    string placa = entryPlaca.Text;
                    _listaVehiculos.Insertar(id, id_Usuario, marca, modelo, placa);
                }
            }
            dialog.Destroy();
        }
    }
}
