// Inicio.cs
using Gtk;
using System;
using AutoGestPro.Core;
using AutoGestPro.UI;

namespace AutoGestPro.UI
{
    /*
    public class Inicio : Window
    {
        private Entry txtCorreo;
        private Entry txtPassword;

        public Inicio() : base("Inicio de Sesión - AutoGestPro")
        {
            SetDefaultSize(300, 200);
            SetPosition(WindowPosition.Center);

            // Crear el contenedor principal
            Box vbox = new Box(Orientation.Vertical, 5);
            vbox.Margin = 10;

            // Título
            Label lblTitulo = new Label("Iniciar Sesión");
            lblTitulo.MarginBottom = 10;
            vbox.PackStart(lblTitulo, false, false, 0);

            // Campo de correo
            Label lblCorreo = new Label("Correo:");
            vbox.PackStart(lblCorreo, false, false, 0);
            
            txtCorreo = new Entry();
            txtCorreo.PlaceholderText = "Ingrese su correo";
            vbox.PackStart(txtCorreo, false, false, 0);

            // Campo de contraseña
            Label lblPassword = new Label("Contraseña:");
            vbox.PackStart(lblPassword, false, false, 0);
            
            txtPassword = new Entry();
            txtPassword.Visibility = false; // Para ocultar la contraseña
            txtPassword.PlaceholderText = "Ingrese su contraseña";
            vbox.PackStart(txtPassword, false, false, 0);

            // Botón de inicio de sesión
            Button btnLogin = new Button("Iniciar Sesión");
            btnLogin.MarginTop = 10;
            btnLogin.Clicked += OnLoginClicked;
            vbox.PackStart(btnLogin, false, false, 0);

            Add(vbox);

            // Manejar el cierre de la ventana
            DeleteEvent += delegate { Application.Quit(); };
        }

        private void OnLoginClicked(object sender, EventArgs e)
        {
            string correo = txtCorreo.Text;
            string password = txtPassword.Text;

            // Validar credenciales
            if (correo == "root@gmail.com" && password == "root123")
            {
                // Crear y mostrar el menú principal
                Menu1 menu = new Menu1();
                menu.Show();
                
                // Ocultar la ventana de inicio de sesión
                this.Hide();
                
                // Cambiar el comportamiento de cierre del menú principal
                menu.DeleteEvent += (o, args) => 
                {
                    Application.Quit();
                };
            }
            else
            {
                // Mostrar mensaje de error
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
    */

    public class Inicio : Window
{
    private Entry txtCorreo;
    private Entry txtPassword;
    
    // Definir el evento para login exitoso
    public event System.Action LoginExitoso;

    public Inicio() : base("Inicio de Sesión - AutoGestPro")
{
    try
    {
        SetDefaultSize(300, 200);
        SetPosition(WindowPosition.Center);

        // Usar Box en lugar de VBox
        var vbox = new Box(Orientation.Vertical, 5);
        vbox.Margin = 10;

        var lblTitulo = new Label("Iniciar Sesión");
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
        
        Console.WriteLine("Ventana de inicio creada"); // Para debug
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error al crear la ventana de inicio: {ex.Message}");
        Console.WriteLine(ex.StackTrace);
    }
}

   

    private void OnLoginClicked(object sender, EventArgs e)
    {
        if (txtCorreo.Text == "r" && txtPassword.Text == "1")
        {
            LoginExitoso?.Invoke(); // Disparar el evento de login exitoso
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