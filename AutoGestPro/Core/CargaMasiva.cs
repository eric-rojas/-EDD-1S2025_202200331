

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
    public class CargaMasiva : Window
    {
        private readonly ListaUsuarios _listaUsuarios;
        private ListaVehiculos listaVehiculos;
        private ListaRepuestos listaRepuestos;

        public CargaMasiva(ListaUsuarios listaUsuarios) : base("Carga Masiva")
        {
            // Inicializar las listas
            _listaUsuarios = listaUsuarios; 
            listaVehiculos = new ListaVehiculos();
            listaRepuestos = new ListaRepuestos();

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
}