

/*
public class CargaMasiva : Window
{
    private ListaUsuarios listaUsuarios;
    private ListaVehiculos listaVehiculos;
    private ListaRepuestos listaRepuestos;

    public CargaMasiva(ListaUsuarios usuarios, ListaVehiculos vehiculos, ListaRepuestos repuestos) : base("Carga Masiva")
    {
        this.listaUsuarios = usuarios;
        this.listaVehiculos = vehiculos;
        this.listaRepuestos = repuestos;

        SetDefaultSize(400, 150);
        SetPosition(WindowPosition.Center);

        // Crear un contenedor para los elementos
        VBox vbox = new VBox(false, 5);

        // Label
        Label label = new Label("Seleccione un archivo JSON para cargar:");
        vbox.PackStart(label, false, false, 0);

        // Botón para cargar archivo JSON
        Button btnCargarArchivo = new Button("Cargar Archivo JSON");
        btnCargarArchivo.Clicked += OnCargarArchivoClicked;
        vbox.PackStart(btnCargarArchivo, false, false, 0);

        Add(vbox);
    }

    private void OnCargarArchivoClicked(object sender, EventArgs e)
    {
        FileChooserDialog fileChooser = new FileChooserDialog(
            "Seleccione un archivo JSON",
            this,
            FileChooserAction.Open,
            "Cancelar", ResponseType.Cancel,
            "Abrir", ResponseType.Accept);

        FileFilter filter = new FileFilter();
        filter.Name = "Archivos JSON";
        filter.AddPattern("*.json");
        fileChooser.AddFilter(filter);

        if (fileChooser.Run() == (int)ResponseType.Accept)
        {
            string filePath = fileChooser.Filename;
            CargarJSON(filePath);
        }

        fileChooser.Destroy();
    }

    private void CargarJSON(string filePath)
    {
        try
        {
            string jsonContent = File.ReadAllText(filePath);

            if (jsonContent.Contains("Placa"))
            {
                var vehiculos = JsonConvert.DeserializeObject<LocalVehiculo[]>(jsonContent);
                foreach (var vehiculo in vehiculos)
                {
                    listaVehiculos.Insertar(
                        vehiculo.ID, 
                        vehiculo.ID_Usuario,
                        vehiculo.Marca,
                        vehiculo.Modelo,
                        vehiculo.Placa
                    );
                }
            }
            else if (jsonContent.Contains("Correo"))
            {
                var usuarios = JsonConvert.DeserializeObject<LocalUsuario[]>(jsonContent);
                foreach (var usuario in usuarios)
                {
                    var usuarioObj = new Ussuario(
                        usuario.ID,
                        usuario.Nombres,
                        usuario.Apellidos,
                        usuario.Correo,
                        usuario.Contraseña
                    );
                    listaUsuarios.Insertar(usuarioObj);
                }
            }
            else if (jsonContent.Contains("Repuesto"))
            {
                var repuestos = JsonConvert.DeserializeObject<LocalRepuesto[]>(jsonContent);
                foreach (var repuesto in repuestos)
                {
                    listaRepuestos.Insertar(
                        repuesto.ID,
                        repuesto.Repuesto,
                        repuesto.Detalles,
                        repuesto.Costo
                    );
                }
            }

            MessageDialog successDialog = new MessageDialog(
                this,
                DialogFlags.Modal,
                MessageType.Info,
                ButtonsType.Ok,
                "Archivo JSON cargado correctamente."
            );
            successDialog.Run();
            successDialog.Destroy();
        }
        catch (Exception ex)
        {
            MessageDialog errorDialog = new MessageDialog(
                this,
                DialogFlags.Modal,
                MessageType.Error,
                ButtonsType.Ok,
                $"Error al cargar el archivo JSON: {ex.Message}"
            );
            errorDialog.Run();
            errorDialog.Destroy();
        }
    }

    // Clases para deserializar el JSON
    public class LocalVehiculo
    {
        public int ID { get; set; }
        public int ID_Usuario { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Placa { get; set; }
    }

    public class LocalUsuario
    {
        public int ID { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Correo { get; set; }
        public string Contraseña { get; set; }
    }

    public class LocalRepuesto
    {
        public int ID { get; set; }
        public string Repuesto { get; set; }
        public string Detalles { get; set; }
        public double Costo { get; set; }
    }
}

using Gtk;
using System;
using System.IO;
using Newtonsoft.Json;
using AutoGestPro.Core;



namespace AutoGestPro.Core
public class CargaMasiva : Window
{

    private ListaUsuarios listaUsuarios;
    private ListaVehiculos listaVehiculos;
    private ListaRepuestos listaRepuestos;
    public CargaMasiva() : base("Carga Masiva")
    {
        SetDefaultSize(400, 150);
        SetPosition(WindowPosition.Center);

        // Crear un contenedor para los elementos
        VBox vbox = new VBox(false, 5);

        // Label
        Label label = new Label("Seleccione un archivo JSON para cargar:");
        vbox.PackStart(label, false, false, 0);

        // Botón para cargar archivo JSON
        Button btnCargarArchivo = new Button("Cargar Archivo JSON");
        btnCargarArchivo.Clicked += OnCargarArchivoClicked;
        vbox.PackStart(btnCargarArchivo, false, false, 0);

        Add(vbox);
    }

    private void OnCargarArchivoClicked(object sender, EventArgs e)
    {
        // Crear un diálogo para seleccionar archivo
        FileChooserDialog fileChooser = new FileChooserDialog(
            "Seleccione un archivo JSON",
            this,
            FileChooserAction.Open,
            "Cancelar", ResponseType.Cancel,
            "Abrir", ResponseType.Accept);

        // Filtrar solo archivos JSON
        FileFilter filter = new FileFilter();
        filter.Name = "Archivos JSON";
        filter.AddPattern("*.json");
        fileChooser.AddFilter(filter);

        // Si el usuario selecciona un archivo
        if (fileChooser.Run() == (int)ResponseType.Accept)
        {
            string filePath = fileChooser.Filename;
            CargarJSON(filePath);
        }

        fileChooser.Destroy();
    }

    private void CargarJSON(string filePath)
    {
        try
        {
            // Leer el contenido del archivo JSON
            string json = File.ReadAllText(filePath);

            // Deserializar el JSON a una lista de nodos
            var usuarios = JsonConvert.DeserializeObject<ListaUsuarios[]>(json);

            // Mostrar los datos en consola (o procesarlos como necesites)
            Console.WriteLine("Datos cargados correctamente:");
            // Insertar los nodos en la lista global
            foreach (var usuario in usuarios)
            {
                ListaUsuarios.Insertar(usuario.id, new string(usuario.Nombres), new string(usuario.Apellidos),
                    new string(usuario.Correo), new string(usuario.Contrasenia));
            }

            
        }
        catch (Exception e)
        {
            // Mostrar mensaje de error si algo falla
            MessageDialog errorDialog = new MessageDialog(
                this,
                DialogFlags.Modal,
                MessageType.Error,
                ButtonsType.Ok,
                $"Error al cargar el archivo JSON: {e.Message}");
            errorDialog.Run();
            errorDialog.Destroy();
        }
    }

}
*/


using Gtk;
using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using AutoGestPro.Core;

namespace AutoGestPro.Core
{
    /*
    public class CargaMasiva : Window

    {
        private readonly ListaUsuarios _listaUsuarios; // Cambiar el tipo de la lista
        private readonly ListaVehiculos _listaVehiculos;
        private readonly ListaRepuestos _listaRepuestos; 

        public CargaMasiva(ListaUsuarios listaUsuarios, ListaVehiculos listaVehiculos, ListaRepuestos listaRepuestos) : base("Carga Masiva") // Cambiar el constructor para recibir las listas
        {
            // Inicializar las listas
            _listaUsuarios = listaUsuarios; // Usamos la lista pasada por parámetro
            _listaVehiculos = listaVehiculos;
            _listaRepuestos = listaRepuestos;

            // Configurar la ventana
            SetDefaultSize(400, 150);
            SetPosition(WindowPosition.Center);

            // Crear un contenedor para los elementos
            VBox vbox = new VBox();
            vbox.Spacing = 5;

            // Label
            Label label = new Label("Seleccione un archivo JSON para cargar:");
            vbox.PackStart(label, false, false, 0);

            // Botón para cargar archivo JSON
            Button btnCargarArchivo = new Button("Cargar Archivo JSON");
            btnCargarArchivo.Clicked += OnCargarArchivoClicked;
            vbox.PackStart(btnCargarArchivo, false, false, 0);

            // Botón para cargar archivo JSON de vehículos
            // boton para cargar archivo JSON de repuestos
            

            Add(vbox);
        }

        private void OnCargarArchivoClicked(object? sender, EventArgs e)
        {
            // Crear un diálogo para seleccionar archivo
            FileChooserDialog fileChooser = new FileChooserDialog(
                "Seleccione un archivo JSON",
                this,
                FileChooserAction.Open,
                "Cancelar", ResponseType.Cancel,
                "Abrir", ResponseType.Accept);

            // Filtrar solo archivos JSON
            FileFilter filter = new FileFilter();
            filter.Name = "Archivos JSON";
            filter.AddPattern("*.json");
            fileChooser.AddFilter(filter);

            // Si el usuario selecciona un archivo
            if (fileChooser.Run() == (int)ResponseType.Accept)
            {
                string filePath = fileChooser.Filename;
                CargarJSON(filePath);
            }

            fileChooser.Destroy();
        }

        private void CargarJSON(string filePath)
        {
            try
            {
                Console.WriteLine("\n=== Iniciando carga de archivo JSON ===");
                Console.WriteLine($"Archivo seleccionado: {filePath}");

                string json = File.ReadAllText(filePath);
                Console.WriteLine("✓ Archivo leído correctamente");

                var usuarios = JsonConvert.DeserializeObject<List<Usuario>>(json);
                
                if (usuarios != null && usuarios.Count > 0)
                {
                    Console.WriteLine($"✓ JSON deserializado exitosamente - {usuarios.Count} usuarios encontrados");
                    Console.WriteLine("\nUsuarios cargados:");
                    
                    foreach (var usuario in usuarios)
                    {
                       _listaUsuarios.Insertar(usuario);  // Usamos la lista pasada por parámetro
                        Console.WriteLine($"→ {usuario}");
                    }

                    Console.WriteLine("\n✓ Todos los usuarios fueron insertados correctamente");
                    Console.WriteLine("=== Carga completada ===\n");

                    MessageDialog successDialog = new MessageDialog(
                        this,
                        DialogFlags.Modal,
                        MessageType.Info,
                        ButtonsType.Ok,
                        $"Archivo JSON cargado exitosamente: {usuarios.Count} usuarios importados");
                    successDialog.Run();
                    successDialog.Destroy();
                }
                else
                {
                    Console.WriteLine("⚠ El archivo JSON está vacío o tiene un formato incorrecto");
                    throw new Exception("No se encontraron usuarios en el archivo JSON");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"\n❌ Error durante la carga del archivo:");
                Console.WriteLine($"→ {e.Message}");
                Console.WriteLine("=== Carga fallida ===\n");

                MessageDialog errorDialog = new MessageDialog(
                    this,
                    DialogFlags.Modal,
                    MessageType.Error,
                    ButtonsType.Ok,
                    $"Error al cargar el archivo JSON: {e.Message}");
                errorDialog.Run();
                errorDialog.Destroy();
            }
        }
    }
    */

    public class CargaMasiva : Window
    {
        private readonly ListaUsuarios _listaUsuarios;
        private readonly ListaVehiculos _listaVehiculos;
        private readonly ListaRepuestos _listaRepuestos;

        public CargaMasiva(ListaUsuarios listaUsuarios, ListaVehiculos listaVehiculos, ListaRepuestos listaRepuestos) : base("Carga Masiva")
        {
            _listaUsuarios = listaUsuarios;
            _listaVehiculos = listaVehiculos;
            _listaRepuestos = listaRepuestos;

            SetDefaultSize(400, 200);
            SetPosition(WindowPosition.Center);

            VBox vbox = new VBox();
            vbox.Spacing = 10;

            // Título
            Label labelTitulo = new Label("Seleccione el tipo de datos a cargar:");
            vbox.PackStart(labelTitulo, false, false, 5);

            // Botones para cada tipo de carga
            Button btnUsuarios = new Button("Cargar Usuarios (JSON)");
            btnUsuarios.Clicked += (s, e) => OnCargarArchivoClicked(TipoCarga.Usuarios);
            vbox.PackStart(btnUsuarios, false, false, 0);

            Button btnVehiculos = new Button("Cargar Vehículos (JSON)");
            btnVehiculos.Clicked += (s, e) => OnCargarArchivoClicked(TipoCarga.Vehiculos);
            vbox.PackStart(btnVehiculos, false, false, 0);

            Button btnRepuestos = new Button("Cargar Repuestos (JSON)");
            btnRepuestos.Clicked += (s, e) => OnCargarArchivoClicked(TipoCarga.Repuestos);
            vbox.PackStart(btnRepuestos, false, false, 0);

            Add(vbox);
        }

        // Enum para identificar el tipo de carga
        private enum TipoCarga
        {
            Usuarios,
            Vehiculos,
            Repuestos
        }

        private void OnCargarArchivoClicked(TipoCarga tipo)
        {
            FileChooserDialog fileChooser = new FileChooserDialog(
                $"Seleccione el archivo JSON de {tipo}",
                this,
                FileChooserAction.Open,
                "Cancelar", ResponseType.Cancel,
                "Abrir", ResponseType.Accept);

            FileFilter filter = new FileFilter();
            filter.Name = "Archivos JSON";
            filter.AddPattern("*.json");
            fileChooser.AddFilter(filter);

            if (fileChooser.Run() == (int)ResponseType.Accept)
            {
                string filePath = fileChooser.Filename;
                switch (tipo)
                {
                    case TipoCarga.Usuarios:
                        CargarUsuarios(filePath);
                        break;
                    case TipoCarga.Vehiculos:
                        CargarVehiculos(filePath);
                        break;
                    case TipoCarga.Repuestos:
                        CargarRepuestos(filePath);
                        break;
                }
            }

            fileChooser.Destroy();
        }

        private void CargarUsuarios(string filePath)
        {
            try
            {
                Console.WriteLine("\n=== Iniciando carga de USUARIOS ===");
                string json = File.ReadAllText(filePath);
                
                var usuarios = JsonConvert.DeserializeObject<List<Usuario>>(json);
                ValidarJSONUsuarios(usuarios);

                if (usuarios != null && usuarios.Count > 0)
                {
                    Console.WriteLine($"✓ JSON deserializado exitosamente - {usuarios.Count} usuarios encontrados");
                    foreach (var usuario in usuarios)
                    {
                        _listaUsuarios.Insertar(usuario);
                        Console.WriteLine($"→ {usuario}");
                    }
                    MostrarMensajeExito($"Se cargaron {usuarios.Count} usuarios correctamente");
                }
            }
            catch (Exception e)
            {
                MostrarError($"Error al cargar usuarios: {e.Message}");
            }
        }

       

        private void CargarVehiculos(string filePath)
        {
            try
            {
                Console.WriteLine("\n=== Iniciando carga de VEHÍCULOS ===");
                string json = File.ReadAllText(filePath);
                
                var vehiculos = JsonConvert.DeserializeObject<List<Vehiculo>>(json);
                ValidarJSONVehiculos(vehiculos);

                if (vehiculos != null && vehiculos.Count > 0)
                {
                    Console.WriteLine($"✓ JSON deserializado exitosamente - {vehiculos.Count} vehículos encontrados");
                    foreach (var vehiculo in vehiculos)
                    {
                        _listaVehiculos.Insertar(vehiculo.ID, vehiculo.ID_Usuario, vehiculo.Marca, vehiculo.Modelo, vehiculo.Placa);
                        Console.WriteLine($"→ {vehiculo}");
                    }
                    MostrarMensajeExito($"Se cargaron {vehiculos.Count} vehículos correctamente");
                }
            }
            catch (Exception e)
            {
                MostrarError($"Error al cargar vehículos: {e.Message}");
            }
        }

        private void CargarRepuestos(string filePath)
        {
            try
            {
                Console.WriteLine("\n=== Iniciando carga de REPUESTOS ===");
                string json = File.ReadAllText(filePath);
                
                var repuestos = JsonConvert.DeserializeObject<List<Repuesto>>(json);
                ValidarJSONRepuestos(repuestos);

                if (repuestos != null && repuestos.Count > 0)
                {
                    Console.WriteLine($"✓ JSON deserializado exitosamente - {repuestos.Count} repuestos encontrados");
                    foreach (var repuesto in repuestos)
                    {
                        _listaRepuestos.Insertar(repuesto.ID, repuesto.RepuestoNombre, repuesto.Detalles, repuesto.Costo);
                        Console.WriteLine($"→ {repuesto}");
                    }
                    MostrarMensajeExito($"Se cargaron {repuestos.Count} repuestos correctamente");
                }
            }
            catch (Exception e)
            {
                MostrarError($"Error al cargar repuestos: {e.Message}");
            }
        }

        // Métodos de validación
        private void ValidarJSONUsuarios(List<Usuario> usuarios)
        {
            if (usuarios == null) throw new Exception("El archivo no contiene un formato válido de usuarios");
            foreach (var usuario in usuarios)
            {
                if (string.IsNullOrEmpty(usuario.Nombres)) throw new Exception($"Usuario con ID {usuario.ID} no tiene nombre");
                if (string.IsNullOrEmpty(usuario.Correo)) throw new Exception($"Usuario con ID {usuario.ID} no tiene correo");
                // Más validaciones específicas
            }
        }

        private void ValidarJSONVehiculos(List<Vehiculo> vehiculos)
        {
            if (vehiculos == null) throw new Exception("El archivo no contiene un formato válido de vehículos");
            foreach (var vehiculo in vehiculos)
            {
                if (string.IsNullOrEmpty(vehiculo.Placa)) throw new Exception($"Vehículo con ID {vehiculo.ID} no tiene placa");
                if (vehiculo.ID_Usuario <= 0) throw new Exception($"Vehículo con ID {vehiculo.ID} tiene un ID de usuario inválido");
                // Más validaciones específicas
            }
        }

        private void ValidarJSONRepuestos(List<Repuesto> repuestos)
        {
            if (repuestos == null) throw new Exception("El archivo no contiene un formato válido de repuestos");
            foreach (var repuesto in repuestos)
            {
                if (string.IsNullOrEmpty(repuesto.RepuestoNombre)) throw new Exception($"Repuesto con ID {repuesto.ID} no tiene nombre");
                if (repuesto.Costo <= 0) throw new Exception($"Repuesto con ID {repuesto.ID} tiene un costo inválido");
                // Más validaciones específicas
            }
        }

        private void MostrarMensajeExito(string mensaje)
        {
            MessageDialog dialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok, mensaje);
            dialog.Run();
            dialog.Destroy();
        }

        private void MostrarError(string mensaje)
        {
            Console.WriteLine($"\n❌ Error: {mensaje}");
            MessageDialog dialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, mensaje);
            dialog.Run();
            dialog.Destroy();
        }
    }

    
}