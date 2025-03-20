// Inicio.cs
using Gtk;
using System;
using AutoGestPro.Core;
using AutoGestPro.UI;

namespace AutoGestPro.UI
{
    public class Inicio : Window
    {
        private Entry txtCorreo;
        private Entry txtPassword;
        private ListaUsuarios listaUsuarios; // Agregamos la lista de usuarios

        // Definir el evento para login exitoso
        public event Action<string> LoginExitoso; // Ahora pasamos el correo del usuario logueado

        public Inicio(ListaUsuarios usuarios) : base("Inicio de Sesión - AutoGestPro")
        {
            try
            {
                listaUsuarios = usuarios; // Recibimos la lista de usuarios

                SetDefaultSize(300, 200);
                SetPosition(WindowPosition.Center);

                var vbox = new Box(Orientation.Vertical, 5);
                vbox.Margin = 10;

                var lblTitulo = new Label("Inicio de Sesión");
                vbox.PackStart(lblTitulo, false, false, 10);

                var lblCorreo = new Label("Correo:");
                vbox.PackStart(lblCorreo, false, false, 0);

                txtCorreo = new Entry();
                txtCorreo.PlaceholderText = "Ingrese su correo";
                vbox.PackStart(txtCorreo, false, false, 5);

                var lblPassword = new Label("Contraseña:");
                vbox.PackStart(lblPassword, false, false, 0);

                txtPassword = new Entry();
                txtPassword.Visibility = false;
                txtPassword.PlaceholderText = "Ingrese su contraseña";
                vbox.PackStart(txtPassword, false, false, 5);

                var btnLogin = new Button("Iniciar Sesión");
                btnLogin.MarginTop = 10;
                btnLogin.Clicked += OnLoginClicked;
                vbox.PackStart(btnLogin, false, false, 10);

                Add(vbox);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear la ventana de inicio: {ex.Message}");
            }
        }

        private void OnLoginClicked(object sender, EventArgs e)
        {
            string correo = txtCorreo.Text.Trim();
            string contrasenia = txtPassword.Text.Trim();
            // Verificar si es el usuario root
            //if (correo == "admin@usac.com" && contrasenia == "admin123")
            if (correo == "a" && contrasenia == "a")
            {
                Console.WriteLine("Inicio de sesión exitoso como root.");
                LoginExitoso?.Invoke("root");
                return;
            }

            // Buscar en la lista de usuarios normales
            Usuario usuario = listaUsuarios.BuscarPorCorreo(correo);
            if (usuario != null && usuario.Contrasenia == contrasenia)
            {
                Console.WriteLine($"Inicio de sesión exitoso como usuario: {usuario.Nombres}");
                LoginExitoso?.Invoke(usuario.Correo); // Pasamos el correo del usuario logueado
            }
            else
            {
                MessageDialog errorDialog = new MessageDialog(
                    this,
                    DialogFlags.Modal,
                    MessageType.Error,
                    ButtonsType.Ok,
                    "Credenciales incorrectas. Por favor, intente nuevamente.");
                errorDialog.Run();
                errorDialog.Destroy();
            }
        }
    }
}
