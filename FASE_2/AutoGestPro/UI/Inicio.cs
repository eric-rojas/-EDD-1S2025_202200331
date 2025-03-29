/*// Inicio.cs
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
        //public event Action<string> LoginExitoso; // Ahora pasamos el correo del usuario logueado
        public event Action<Usuario> LoginExitoso;

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

                var lblCredenciales = new Label("Credenciales Root:\nadmin@usac.com\na");
                lblCredenciales.Selectable = true;
                vbox.PackStart(lblCredenciales, false, false, 0);

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
            if (correo == "admin@usac.com" && contrasenia == "a")
        {
            Console.WriteLine("Inicio de sesión exitoso como root.");
            
            // Crear un usuario root temporal
            Usuario rootUser = new Usuario(
                id: -1, // ID especial para root
                nombres: "Administrador",
                apellidos: "Root",
                correo: "admin@usac.com",
                edad: 0, // Opcional
                contrasenia: "admin123"
            );
            
            LoginExitoso?.Invoke(rootUser);
            return;
        }

            // Buscar en la lista de usuarios normales
            Usuario usuario = listaUsuarios.BuscarPorCorreo(correo);
            if (usuario != null && usuario.Contrasenia == contrasenia)
            {
                Console.WriteLine($"Inicio de sesión exitoso como usuario: {usuario.Nombres}");
                LoginExitoso?.Invoke(usuario); // Pasamos el correo del usuario logueado
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
        private ListaUsuarios listaUsuarios;
        private LogueoUsuarios logueoUsuarios;

        public event Action<Usuario> LoginExitoso;

        public Inicio(ListaUsuarios usuarios) : base("Inicio de Sesión - AutoGestPro")
        {
            try
            {
                listaUsuarios = usuarios;
                logueoUsuarios = new LogueoUsuarios();

                SetDefaultSize(300, 200);
                SetPosition(WindowPosition.Center);

                var vbox = new Box(Orientation.Vertical, 5);
                vbox.Margin = 10;

                var lblTitulo = new Label("Inicio de Sesión");
                vbox.PackStart(lblTitulo, false, false, 10);

                var lblCredenciales = new Label("Credenciales Root:\nadmin@usac.com\nadmin123");
                lblCredenciales.Selectable = true;
                vbox.PackStart(lblCredenciales, false, false, 0);

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
            
            if (correo == "admin@usac.com" && contrasenia == "admin123")
            {
                Console.WriteLine("Inicio de sesión exitoso como root.");
                
                Usuario rootUser = new Usuario(
                    id: -1,
                    nombres: "Administrador",
                    apellidos: "Root",
                    correo: "admin@usac.com",
                    edad: 0,
                    contrasenia: "admin123"
                );
                
                logueoUsuarios.RegistrarEntrada(rootUser.Correo);
                LoginExitoso?.Invoke(rootUser);
                return;
            }

            Usuario usuario = listaUsuarios.BuscarPorCorreo(correo);
            if (usuario != null && usuario.Contrasenia == contrasenia)
            {
                Console.WriteLine($"Inicio de sesión exitoso como usuario: {usuario.Nombres}");
                logueoUsuarios.RegistrarEntrada(usuario.Correo);
                LoginExitoso?.Invoke(usuario);
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

        protected override void OnDestroyed()
        {
            if (!string.IsNullOrEmpty(txtCorreo.Text.Trim()))
            {
                logueoUsuarios.RegistrarSalida(txtCorreo.Text.Trim());
            }
            base.OnDestroyed();
        }
    }
}*/


using Gtk;
using System;
using AutoGestPro.Core;
using AutoGestPro.Utils;

namespace AutoGestPro.UI
{
    public class Inicio : Window
    {
        private Entry txtCorreo;
        private Entry txtPassword;
        private ListaUsuarios listaUsuarios;
        private LogueoUsuarios logueoUsuarios;
        private Button btnLogin;

        public event Action<Usuario> LoginExitoso;

        public Inicio(ListaUsuarios usuarios) : base("Inicio de Sesión - AutoGestPro")
        {
            try
            {
                listaUsuarios = usuarios;
                logueoUsuarios = new LogueoUsuarios();

                SetDefaultSize(300, 200);
                SetPosition(WindowPosition.Center);
                
                // Manejar el evento de cierre de la ventana correctamente
                DeleteEvent += (o, args) => {
                    try {
                        Destroy();
                    } catch (Exception ex) {
                        ErrorHandler.LogError("Inicio", "DeleteEvent", ex);
                    }
                };

                var vbox = new Box(Orientation.Vertical, 5);
                vbox.Margin = 10;

                var lblTitulo = new Label("Inicio de Sesión");
                vbox.PackStart(lblTitulo, false, false, 10);

                var lblCredenciales = new Label("Credenciales Root:\nadmin@usac.com\nadmin123");
                lblCredenciales.Selectable = true;
                vbox.PackStart(lblCredenciales, false, false, 0);

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

                btnLogin = new Button("Iniciar Sesión");
                btnLogin.MarginTop = 10;
                btnLogin.Clicked += OnLoginClicked;
                vbox.PackStart(btnLogin, false, false, 10);

                Add(vbox);
                ShowAll();
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("Inicio", "Constructor", ex);
                Console.WriteLine($"Error al crear la ventana de inicio: {ex.Message}");
                
                try {
                    ErrorHandler.MostrarError(null, "Error al iniciar la aplicación: " + ex.Message);
                } catch {
                    // Si falla mostrar el diálogo, no podemos hacer mucho más
                }
            }
        }

        private void OnLoginClicked(object sender, EventArgs e)
        {
            try
            {
                // Deshabilitar botón para prevenir múltiples clicks
                btnLogin.Sensitive = false;
                
                string correo = txtCorreo.Text?.Trim() ?? string.Empty;
                string contrasenia = txtPassword.Text?.Trim() ?? string.Empty;
                
                // Validar campos vacíos
                if (string.IsNullOrEmpty(correo) || string.IsNullOrEmpty(contrasenia))
                {
                    ErrorHandler.MostrarError(this, "Por favor complete todos los campos.");
                    btnLogin.Sensitive = true;
                    return;
                }
                
                if (correo == "admin@usac.com" && contrasenia == "admin123")
                {
                    Console.WriteLine("Inicio de sesión exitoso como root.");
                    
                    Usuario rootUser = new Usuario(
                        id: -1,
                        nombres: "Administrador",
                        apellidos: "Root",
                        correo: "admin@usac.com",
                        edad: 0,
                        contrasenia: "admin123"
                    );
                    
                    try
                    {
                        logueoUsuarios.RegistrarEntrada(rootUser.Correo);
                    }
                    catch (Exception ex)
                    {
                        // Registramos pero continuamos
                        ErrorHandler.LogError("Inicio", "RegistrarEntrada", ex);
                    }
                    
                    LoginExitoso?.Invoke(rootUser);
                    return;
                }

                Usuario usuario = null;
                try
                {
                    usuario = listaUsuarios.BuscarPorCorreo(correo);
                }
                catch (Exception ex)
                {
                    ErrorHandler.LogError("Inicio", "BuscarPorCorreo", ex);
                    ErrorHandler.MostrarError(this, "Error al buscar usuario: " + ex.Message);
                    btnLogin.Sensitive = true;
                    return;
                }
                
                if (usuario != null && usuario.Contrasenia == contrasenia)
                {
                    Console.WriteLine($"Inicio de sesión exitoso como usuario: {usuario.Nombres}");
                    
                    try
                    {
                        logueoUsuarios.RegistrarEntrada(usuario.Correo);
                    }
                    catch (Exception ex)
                    {
                        // Registramos pero continuamos
                        ErrorHandler.LogError("Inicio", "RegistrarEntrada", ex);
                    }
                    
                    LoginExitoso?.Invoke(usuario);
                }
                else
                {
                    ErrorHandler.MostrarError(this, "Credenciales incorrectas. Por favor, intente nuevamente.");
                    btnLogin.Sensitive = true;
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("Inicio", "OnLoginClicked", ex);
                ErrorHandler.MostrarError(this, "Error al iniciar sesión: " + ex.Message);
                btnLogin.Sensitive = true;
            }
        }

        protected override void OnDestroyed()
        {
            try
            {
                // Desconectar eventos
                if (btnLogin != null)
                    btnLogin.Clicked -= OnLoginClicked;
                
                if (!string.IsNullOrEmpty(txtCorreo?.Text?.Trim()))
                {
                    try
                    {
                        logueoUsuarios.RegistrarSalida(txtCorreo.Text.Trim());
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.LogError("Inicio", "RegistrarSalida", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("Inicio", "OnDestroyed", ex);
            }
            finally
            {
                base.OnDestroyed();
            }
        }
    }
}
