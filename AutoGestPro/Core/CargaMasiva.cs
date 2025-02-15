/*
using Gtk;
using System;
using System.IO;
using Newtonsoft.Json;
using AutoGestPro.Core;

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

*/