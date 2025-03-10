using Gtk;
using System;
using AutoGestPro.Core;

namespace AutoGestPro.UI
{
    public class Ingreso : Window
    {
        private readonly ListaUsuarios _listaUsuarios;
        private readonly ListaVehiculos _listaVehiculos;
        private readonly ListaRepuestos _listaRepuestos;

        public Ingreso(ListaUsuarios listaUsuarios, ListaRepuestos listaRepuestos, ListaVehiculos listaVehiculos) : base("AutoGestPro - Ingreso")
        {
            _listaUsuarios = listaUsuarios;
            _listaVehiculos = listaVehiculos;
            _listaRepuestos = listaRepuestos;

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

            // ingreso repuesto
            Button Btn_VerRepuestos = new Button("Ingreso Repuestos");
            Btn_VerRepuestos.Clicked += GoRepuestos;
            vbox.PackStart(Btn_VerRepuestos, false, false, 0);



            Add (vbox);

            
        }

        private void GoUsuarios(object sender, EventArgs e)
        {
            
           
            Entry entryId = new Entry();
            MessageDialog dialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Question, ButtonsType.OkCancel, "Ingrese el usuario:");
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

                Entry entryNombres = new Entry();
                Entry entryApellidos = new Entry();
                Entry entryCorreo = new Entry();
                Entry entryContrasenia = new Entry();

                MessageDialog dialogNombres = new MessageDialog(this, DialogFlags.Modal, MessageType.Question, ButtonsType.OkCancel, "Ingrese los nombres:");
                dialogNombres.ContentArea.PackStart(entryNombres, false, false, 0);
                dialogNombres.ContentArea.ShowAll();
                if (dialogNombres.Run() == (int)ResponseType.Ok)
                {
                    string nombres = entryNombres.Text;

                    MessageDialog dialogApellidos = new MessageDialog(this, DialogFlags.Modal, MessageType.Question, ButtonsType.OkCancel, "Ingrese los apellidos:");
                    dialogApellidos.ContentArea.PackStart(entryApellidos, false, false, 0);
                    dialogApellidos.ContentArea.ShowAll();
                    if (dialogApellidos.Run() == (int)ResponseType.Ok)
                    {
                        string apellidos = entryApellidos.Text;

                        MessageDialog dialogCorreo = new MessageDialog(this, DialogFlags.Modal, MessageType.Question, ButtonsType.OkCancel, "Ingrese el correo:");
                        dialogCorreo.ContentArea.PackStart(entryCorreo, false, false, 0);
                        dialogCorreo.ContentArea.ShowAll();
                        if (dialogCorreo.Run() == (int)ResponseType.Ok)
                        {
                            string correo = entryCorreo.Text;

                            MessageDialog dialogContrasenia = new MessageDialog(this, DialogFlags.Modal, MessageType.Question, ButtonsType.OkCancel, "Ingrese la contraseña:");
                            dialogContrasenia.ContentArea.PackStart(entryContrasenia, false, false, 0);
                            dialogContrasenia.ContentArea.ShowAll();
                            if (dialogContrasenia.Run() == (int)ResponseType.Ok)
                            {
                                string contrasenia = entryContrasenia.Text;

                                // Aquí puedes agregar el código para insertar el usuario en la lista
                                Usuario nuevoUsuario = new Usuario(id, nombres, apellidos, correo, contrasenia);
                                _listaUsuarios.Insertar(nuevoUsuario);
                            }
                            dialogContrasenia.Destroy();
                        }
                        dialogCorreo.Destroy();
                    }
                    dialogApellidos.Destroy();
                }
                dialogNombres.Destroy();
            }
            

            dialog.Destroy();
        }

        /*
        private void GoVehiculos(object sender, EventArgs e)
        {
            Entry entryId = new Entry();
            MessageDialog dialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Question, ButtonsType.OkCancel, "Ingrese el vehículo:");
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

                Entry entryMarca = new Entry();
                Entry entryModelo = new Entry();
                Entry entryID = new Entry();
                Entry entryID_Usuario = new Entry();
                Entry entryPlaca = new Entry();

                MessageDialog dialogMarca = new MessageDialog(this, DialogFlags.Modal, MessageType.Question, ButtonsType.OkCancel, "Ingrese la marca:");
                dialogMarca.ContentArea.PackStart(entryMarca, false, false, 0);
                dialogMarca.ContentArea.ShowAll();
                if (dialogMarca.Run() == (int)ResponseType.Ok)
                {
                    string marca = entryMarca.Text;

                    MessageDialog dialogModelo = new MessageDialog(this, DialogFlags.Modal, MessageType.Question, ButtonsType.OkCancel, "Ingrese el modelo:");
                    dialogModelo.ContentArea.PackStart(entryModelo, false, false, 0);
                    dialogModelo.ContentArea.ShowAll();
                    if (dialogModelo.Run() == (int)ResponseType.Ok)
                    {
                        string modelo = entryModelo.Text;

                       
                            

                        MessageDialog dialogPlaca = new MessageDialog(this, DialogFlags.Modal, MessageType.Question, ButtonsType.OkCancel, "Ingrese la placa:");
                        dialogPlaca.ContentArea.PackStart(entryPlaca, false, false, 0);
                        dialogPlaca.ContentArea.ShowAll();
                        if (dialogPlaca.Run() == (int)ResponseType.Ok)
                        {
                            string placa = entryPlaca.Text;

                            // Aquí puedes agregar el código para insertar el vehículo en la lista
                            Vehiculo nuevoVehiculo = new Vehiculo(id, marca, modelo, placa);
                            _listaVehiculos.Insertar(nuevoVehiculo);
                        }
                        dialogPlaca.Destroy();
                        
                    }
                    dialogMarca.Destroy();
                }
                dialog.Destroy();
            }
        }*/


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
                int id, id_Usuario; // Declarar las variables antes del if

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

        private void GoRepuestos(object sender, EventArgs e)
        {
            Entry entryId = new Entry();
            Entry entryRepuesto = new Entry();
            Entry entryDetalles = new Entry();
            Entry entryCosto = new Entry();

            MessageDialog dialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Question, ButtonsType.OkCancel, "Ingrese los datos del repuesto:");
            Box box = new Box(Orientation.Vertical, 5);
            box.PackStart(new Label("ID:"), false, false, 0);
            box.PackStart(entryId, false, false, 0);
            box.PackStart(new Label("Repuesto:"), false, false, 0);
            box.PackStart(entryRepuesto, false, false, 0);
            box.PackStart(new Label("Detalles:"), false, false, 0);
            box.PackStart(entryDetalles, false, false, 0);
            box.PackStart(new Label("Costo:"), false, false, 0);
            box.PackStart(entryCosto, false, false, 0);
            dialog.ContentArea.PackStart(box, false, false, 0);
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

                string repuesto = entryRepuesto.Text;
                if (string.IsNullOrWhiteSpace(repuesto))
                {
                    MessageDialog errorDialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "El nombre del repuesto no puede estar vacío.");
                    errorDialog.Run();
                    errorDialog.Destroy();
                    return;
                }

                string detalles = entryDetalles.Text;
                if (string.IsNullOrWhiteSpace(detalles))
                {
                    MessageDialog errorDialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "Los detalles no pueden estar vacíos.");
                    errorDialog.Run();
                    errorDialog.Destroy();
                    return;
                }

                

                int costo;
                if (!int.TryParse(entryCosto.Text, out costo))
                {
                    MessageDialog errorDialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "El costo debe ser un número entero.");
                    errorDialog.Run();
                    errorDialog.Destroy();
                    return;
                }

                _listaRepuestos.Insertar(id, repuesto, detalles, costo);
            }
            dialog.Destroy();
        }


    }
}
