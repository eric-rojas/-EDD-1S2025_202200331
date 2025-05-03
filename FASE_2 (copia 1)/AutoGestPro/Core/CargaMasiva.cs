/*using Gtk;
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
        private readonly ListaVehiculos _listaVehiculos;
        private readonly ArbolAVLRepuestos _arbolRepuestos;

        public CargaMasiva(ListaUsuarios listaUsuarios, ListaVehiculos listaVehiculos, ArbolAVLRepuestos arbolRepuestos) : base("Carga Masiva")
        {
            _listaUsuarios = listaUsuarios;
            _listaVehiculos = listaVehiculos;
            _arbolRepuestos = arbolRepuestos;

            SetDefaultSize(400, 250);
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

            // Información adicional
            Label labelInfo = new Label("La carga validará que los IDs sean únicos y que las relaciones entre entidades sean válidas.");
            labelInfo.LineWrap = true;
            vbox.PackStart(labelInfo, false, false, 10);

            Add(vbox);
            ShowAll();
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
                if (usuarios == null || usuarios.Count == 0)
                {
                    throw new Exception("El archivo no contiene usuarios o el formato es inválido");
                }

                ValidarJSONUsuarios(usuarios);

                int contadorCargados = 0;
                Console.WriteLine($"✓ JSON deserializado exitosamente - {usuarios.Count} usuarios encontrados");
                
                foreach (var usuario in usuarios)
                {
                    // Usando el método Insertar que ya incluye validaciones de ID y correo únicos
                    if (_listaUsuarios.Insertar(usuario))
                    {
                        contadorCargados++;
                        Console.WriteLine($"→ Usuario cargado: {usuario}");
                    }
                    else
                    {
                        Console.WriteLine($"✗ No se pudo cargar el usuario con ID: {usuario.ID}, posible duplicado");
                    }
                }
                
                MostrarMensajeExito($"Se cargaron {contadorCargados} usuarios correctamente de {usuarios.Count} en total");
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
                if (vehiculos == null || vehiculos.Count == 0)
                {
                    throw new Exception("El archivo no contiene vehículos o el formato es inválido");
                }

                ValidarJSONVehiculos(vehiculos);

                int contadorCargados = 0;
                Console.WriteLine($"✓ JSON deserializado exitosamente - {vehiculos.Count} vehículos encontrados");
                
                foreach (var vehiculo in vehiculos)
                {
                    // Validar que el usuario exista antes de insertar el vehículo
                    if (_listaUsuarios.Buscar(vehiculo.ID_Usuario) == null)
                    {
                        Console.WriteLine($"✗ No se pudo cargar el vehículo con ID: {vehiculo.ID}, el usuario con ID: {vehiculo.ID_Usuario} no existe");
                        continue;
                    }
                    
                    if (_listaVehiculos.Insertar(vehiculo))
                    {
                        contadorCargados++;
                        Console.WriteLine($"→ Vehículo cargado: {vehiculo}");
                    }
                    else
                    {
                        Console.WriteLine($"✗ No se pudo cargar el vehículo con ID: {vehiculo.ID}, posible duplicado");
                    }
                }
                
                MostrarMensajeExito($"Se cargaron {contadorCargados} vehículos correctamente de {vehiculos.Count} en total");
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
                if (repuestos == null || repuestos.Count == 0)
                {
                    throw new Exception("El archivo no contiene repuestos o el formato es inválido");
                }

                ValidarJSONRepuestos(repuestos);

                int contadorCargados = 0;
                Console.WriteLine($"✓ JSON deserializado exitosamente - {repuestos.Count} repuestos encontrados");
                
                foreach (var repuesto in repuestos)
                {
                    if (_arbolRepuestos.Insertar(repuesto))
                    {
                        contadorCargados++;
                        Console.WriteLine($"→ Repuesto cargado: {repuesto}");
                    }
                    else
                    {
                        Console.WriteLine($"✗ No se pudo cargar el repuesto con ID: {repuesto.ID}, posible duplicado");
                    }
                }
                
                MostrarMensajeExito($"Se cargaron {contadorCargados} repuestos correctamente de {repuestos.Count} en total");
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
                if (usuario.ID <= 0) 
                    throw new Exception($"Usuario tiene ID inválido: {usuario.ID}. Debe ser un número positivo.");
                
                if (string.IsNullOrEmpty(usuario.Nombres)) 
                    throw new Exception($"Usuario con ID {usuario.ID} no tiene nombres");
                
                if (string.IsNullOrEmpty(usuario.Apellidos)) 
                    throw new Exception($"Usuario con ID {usuario.ID} no tiene apellidos");
                
                if (string.IsNullOrEmpty(usuario.Correo)) 
                    throw new Exception($"Usuario con ID {usuario.ID} no tiene correo");
                
                if (usuario.Edad <= 0) 
                    throw new Exception($"Usuario con ID {usuario.ID} tiene una edad inválida: {usuario.Edad}");
                
                if (string.IsNullOrEmpty(usuario.Contrasenia)) 
                    throw new Exception($"Usuario con ID {usuario.ID} no tiene contraseña");
                
                // Validar formato de correo (básico)
                if (!usuario.Correo.Contains("@") || !usuario.Correo.Contains(".")) 
                    throw new Exception($"Usuario con ID {usuario.ID} tiene un formato de correo inválido: {usuario.Correo}");
            }
        }

        private void ValidarJSONVehiculos(List<Vehiculo> vehiculos)
        {
            if (vehiculos == null) throw new Exception("El archivo no contiene un formato válido de vehículos");
            
            foreach (var vehiculo in vehiculos)
            {
                if (vehiculo.ID <= 0) 
                    throw new Exception($"Vehículo tiene ID inválido: {vehiculo.ID}. Debe ser un número positivo.");
                
                if (vehiculo.ID_Usuario <= 0) 
                    throw new Exception($"Vehículo con ID {vehiculo.ID} tiene un ID de usuario inválido: {vehiculo.ID_Usuario}");
                
                if (string.IsNullOrEmpty(vehiculo.Marca)) 
                    throw new Exception($"Vehículo con ID {vehiculo.ID} no tiene marca");
                
                if (string.IsNullOrEmpty(vehiculo.Modelo)) 
                    throw new Exception($"Vehículo con ID {vehiculo.ID} no tiene modelo");
                
                if (string.IsNullOrEmpty(vehiculo.Placa)) 
                    throw new Exception($"Vehículo con ID {vehiculo.ID} no tiene placa");
                
                
            }
        }

        private void ValidarJSONRepuestos(List<Repuesto> repuestos)
        {
            if (repuestos == null) throw new Exception("El archivo no contiene un formato válido de repuestos");
            
            foreach (var repuesto in repuestos)
            {
                if (repuesto.ID <= 0) 
                    throw new Exception($"Repuesto tiene ID inválido: {repuesto.ID}. Debe ser un número positivo.");
                
                if (string.IsNullOrEmpty(repuesto.RepuestoNombre)) 
                    throw new Exception($"Repuesto con ID {repuesto.ID} no tiene nombre");
                
                if (string.IsNullOrEmpty(repuesto.Detalles)) 
                    throw new Exception($"Repuesto con ID {repuesto.ID} no tiene detalles");
                
                if (repuesto.Costo <= 0) 
                    throw new Exception($"Repuesto con ID {repuesto.ID} tiene un costo inválido: {repuesto.Costo}. Debe ser mayor que cero.");
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
}*/

using Gtk;
using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using AutoGestPro.Utils;

namespace AutoGestPro.Core
{
    public class CargaMasiva : Window
    {
        private readonly BlockchainUsuarios _listaUsuarios;
        private readonly ListaVehiculos _listaVehiculos;
        private readonly ArbolAVLRepuestos _arbolRepuestos;
        
        // Referencias a los botones
        private Button _btnUsuarios;
        private Button _btnVehiculos; 
        private Button _btnRepuestos;

        public CargaMasiva(BlockchainUsuarios listaUsuarios, ListaVehiculos listaVehiculos, ArbolAVLRepuestos arbolRepuestos) : base("Carga Masiva")
        {
            try
            {
                _listaUsuarios = listaUsuarios;
                _listaVehiculos = listaVehiculos;
                _arbolRepuestos = arbolRepuestos;

                SetDefaultSize(400, 250);
                SetPosition(WindowPosition.Center);
                
                // Manejar evento de cierre correctamente
                DeleteEvent += (o, args) => {
                    try
                    {
                        args.RetVal = true; // Prevenir cierre automático
                        Destroy(); // Cerrar manualmente
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error en DeleteEvent: {ex.Message}");
                        ErrorHandler.LogError("CargaMasiva", "DeleteEvent", ex);
                    }
                    //return true;
                };

                CrearInterfaz();
                ShowAll();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear ventana de carga masiva: {ex.Message}");
                ErrorHandler.LogError("CargaMasiva", "Constructor", ex);
                MostrarError($"Error al inicializar la ventana: {ex.Message}");
            }
        }
        
        private void CrearInterfaz()
        {
            try
            {
                VBox vbox = new VBox();
                vbox.Spacing = 10;
                vbox.Margin = 10;

                // Título
                Label labelTitulo = new Label("Seleccione el tipo de datos a cargar:");
                vbox.PackStart(labelTitulo, false, false, 5);

                // Botones para cada tipo de carga
                _btnUsuarios = new Button("Cargar Usuarios (JSON)");
                _btnUsuarios.Clicked += OnCargarUsuariosClicked;
                vbox.PackStart(_btnUsuarios, false, false, 0);

                _btnVehiculos = new Button("Cargar Vehículos (JSON)");
                _btnVehiculos.Clicked += OnCargarVehiculosClicked;
                vbox.PackStart(_btnVehiculos, false, false, 0);

                _btnRepuestos = new Button("Cargar Repuestos (JSON)");
                _btnRepuestos.Clicked += OnCargarRepuestosClicked;
                vbox.PackStart(_btnRepuestos, false, false, 0);

                // Información adicional
                Label labelInfo = new Label("La carga validará que los IDs sean únicos y que las relaciones entre entidades sean válidas.");
                labelInfo.LineWrap = true;
                vbox.PackStart(labelInfo, false, false, 10);

                Add(vbox);
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("CargaMasiva", "CrearInterfaz", ex);
                throw; // Relanzamos para que sea manejada por el constructor
            }
        }

        // Manejadores de eventos separados para cada botón
        private void OnCargarUsuariosClicked(object sender, EventArgs e)
        {
            try
            {
                // Deshabilitar botón mientras se procesa
                _btnUsuarios.Sensitive = false;
                OnCargarArchivoClicked(TipoCarga.Usuarios);
                _btnUsuarios.Sensitive = true;
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("CargaMasiva", "OnCargarUsuariosClicked", ex);
                _btnUsuarios.Sensitive = true;
            }
        }

        private void OnCargarVehiculosClicked(object sender, EventArgs e)
        {
            try
            {
                // Deshabilitar botón mientras se procesa
                _btnVehiculos.Sensitive = false;
                OnCargarArchivoClicked(TipoCarga.Vehiculos);
                _btnVehiculos.Sensitive = true;
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("CargaMasiva", "OnCargarVehiculosClicked", ex);
                _btnVehiculos.Sensitive = true;
            }
        }

        private void OnCargarRepuestosClicked(object sender, EventArgs e)
        {
            try
            {
                // Deshabilitar botón mientras se procesa
                _btnRepuestos.Sensitive = false;
                OnCargarArchivoClicked(TipoCarga.Repuestos);
                _btnRepuestos.Sensitive = true;
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("CargaMasiva", "OnCargarRepuestosClicked", ex);
                _btnRepuestos.Sensitive = true;
            }
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
            try
            {
                using (FileChooserDialog fileChooser = new FileChooserDialog(
                    $"Seleccione el archivo JSON de {tipo}",
                    this,
                    FileChooserAction.Open,
                    "Cancelar", ResponseType.Cancel,
                    "Abrir", ResponseType.Accept))
                {
                    FileFilter filter = new FileFilter();
                    filter.Name = "Archivos JSON";
                    filter.AddPattern("*.json");
                    fileChooser.AddFilter(filter);

                    try
                    {
                        ResponseType respuesta = (ResponseType)fileChooser.Run();
                        
                        if (respuesta == ResponseType.Accept)
                        {
                            string filePath = fileChooser.Filename;
                            fileChooser.Destroy();
                            
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
                        else
                        {
                            fileChooser.Destroy();
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.LogError("CargaMasiva", "FileChooserRun", ex);
                        try { fileChooser.Destroy(); } catch { }
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("CargaMasiva", "OnCargarArchivoClicked", ex);
                MostrarError($"Error al abrir el selector de archivos: {ex.Message}");
            }
        }

        private void CargarUsuarios(string filePath)
        {
            try
            {
                Console.WriteLine("\n=== Iniciando carga de USUARIOS ===");
                
                if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                {
                    throw new Exception("La ruta del archivo es inválida o el archivo no existe");
                }
                
                string json = File.ReadAllText(filePath);
                
                var usuarios = JsonConvert.DeserializeObject<List<Usuario>>(json);
                if (usuarios == null || usuarios.Count == 0)
                {
                    throw new Exception("El archivo no contiene usuarios o el formato es inválido");
                }

                ValidarJSONUsuarios(usuarios);

                int contadorCargados = 0;
                Console.WriteLine($"✓ JSON deserializado exitosamente - {usuarios.Count} usuarios encontrados");
                
                foreach (var usuario in usuarios)
                {
                    // Usando el método Insertar que ya incluye validaciones de ID y correo únicos
                    if (_listaUsuarios.Insertar(usuario))
                    {
                        contadorCargados++;
                        Console.WriteLine($"→ Usuario cargado: {usuario}");
                    }
                    else
                    {
                        Console.WriteLine($"✗ No se pudo cargar el usuario con ID: {usuario.ID}, posible duplicado");
                    }
                }
                
                MostrarMensajeExito($"Se cargaron {contadorCargados} usuarios correctamente de {usuarios.Count} en total");
            }
            catch (Exception e)
            {
                ErrorHandler.LogError("CargaMasiva", "CargarUsuarios", e);
                MostrarError($"Error al cargar usuarios: {e.Message}");
            }
        }

        private void CargarVehiculos(string filePath)
        {
            try
            {
                Console.WriteLine("\n=== Iniciando carga de VEHÍCULOS ===");
                
                if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                {
                    throw new Exception("La ruta del archivo es inválida o el archivo no existe");
                }
                
                string json = File.ReadAllText(filePath);
                
                var vehiculos = JsonConvert.DeserializeObject<List<Vehiculo>>(json);
                if (vehiculos == null || vehiculos.Count == 0)
                {
                    throw new Exception("El archivo no contiene vehículos o el formato es inválido");
                }

                ValidarJSONVehiculos(vehiculos);

                int contadorCargados = 0;
                Console.WriteLine($"✓ JSON deserializado exitosamente - {vehiculos.Count} vehículos encontrados");
                
                foreach (var vehiculo in vehiculos)
                {
                    // Validar que el usuario exista antes de insertar el vehículo
                    if (_listaUsuarios.Buscar(vehiculo.ID_Usuario) == null)
                    {
                        Console.WriteLine($"✗ No se pudo cargar el vehículo con ID: {vehiculo.ID}, el usuario con ID: {vehiculo.ID_Usuario} no existe");
                        continue;
                    }
                    
                    if (_listaVehiculos.Insertar(vehiculo))
                    {
                        contadorCargados++;
                        Console.WriteLine($"→ Vehículo cargado: {vehiculo}");
                    }
                    else
                    {
                        Console.WriteLine($"✗ No se pudo cargar el vehículo con ID: {vehiculo.ID}, posible duplicado");
                    }
                }
                
                MostrarMensajeExito($"Se cargaron {contadorCargados} vehículos correctamente de {vehiculos.Count} en total");
            }
            catch (Exception e)
            {
                ErrorHandler.LogError("CargaMasiva", "CargarVehiculos", e);
                MostrarError($"Error al cargar vehículos: {e.Message}");
            }
        }

        private void CargarRepuestos(string filePath)
        {
            try
            {
                Console.WriteLine("\n=== Iniciando carga de REPUESTOS ===");
                
                if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                {
                    throw new Exception("La ruta del archivo es inválida o el archivo no existe");
                }
                
                string json = File.ReadAllText(filePath);
                
                var repuestos = JsonConvert.DeserializeObject<List<Repuesto>>(json);
                if (repuestos == null || repuestos.Count == 0)
                {
                    throw new Exception("El archivo no contiene repuestos o el formato es inválido");
                }

                ValidarJSONRepuestos(repuestos);

                int contadorCargados = 0;
                Console.WriteLine($"✓ JSON deserializado exitosamente - {repuestos.Count} repuestos encontrados");
                
                foreach (var repuesto in repuestos)
                {
                    if (_arbolRepuestos.Insertar(repuesto))
                    {
                        contadorCargados++;
                        Console.WriteLine($"→ Repuesto cargado: {repuesto}");
                    }
                    else
                    {
                        Console.WriteLine($"✗ No se pudo cargar el repuesto con ID: {repuesto.ID}, posible duplicado");
                    }
                }
                
                MostrarMensajeExito($"Se cargaron {contadorCargados} repuestos correctamente de {repuestos.Count} en total");
            }
            catch (Exception e)
            {
                ErrorHandler.LogError("CargaMasiva", "CargarRepuestos", e);
                MostrarError($"Error al cargar repuestos: {e.Message}");
            }
        }

        // Métodos de validación
        private void ValidarJSONUsuarios(List<Usuario> usuarios)
        {
            if (usuarios == null) throw new Exception("El archivo no contiene un formato válido de usuarios");
            
            foreach (var usuario in usuarios)
            {
                if (usuario.ID <= 0) 
                    throw new Exception($"Usuario tiene ID inválido: {usuario.ID}. Debe ser un número positivo.");
                
                if (string.IsNullOrEmpty(usuario.Nombres)) 
                    throw new Exception($"Usuario con ID {usuario.ID} no tiene nombres");
                
                if (string.IsNullOrEmpty(usuario.Apellidos)) 
                    throw new Exception($"Usuario con ID {usuario.ID} no tiene apellidos");
                
                if (string.IsNullOrEmpty(usuario.Correo)) 
                    throw new Exception($"Usuario con ID {usuario.ID} no tiene correo");
                
                if (usuario.Edad <= 0) 
                    throw new Exception($"Usuario con ID {usuario.ID} tiene una edad inválida: {usuario.Edad}");
                
                if (string.IsNullOrEmpty(usuario.Contrasenia)) 
                    throw new Exception($"Usuario con ID {usuario.ID} no tiene contraseña");
                
                // Validar formato de correo (básico)
                if (!usuario.Correo.Contains("@") || !usuario.Correo.Contains(".")) 
                    throw new Exception($"Usuario con ID {usuario.ID} tiene un formato de correo inválido: {usuario.Correo}");
            }
        }

        private void ValidarJSONVehiculos(List<Vehiculo> vehiculos)
        {
            if (vehiculos == null) throw new Exception("El archivo no contiene un formato válido de vehículos");
            
            foreach (var vehiculo in vehiculos)
            {
                if (vehiculo.ID <= 0) 
                    throw new Exception($"Vehículo tiene ID inválido: {vehiculo.ID}. Debe ser un número positivo.");
                
                if (vehiculo.ID_Usuario <= 0) 
                    throw new Exception($"Vehículo con ID {vehiculo.ID} tiene un ID de usuario inválido: {vehiculo.ID_Usuario}");
                
                if (string.IsNullOrEmpty(vehiculo.Marca)) 
                    throw new Exception($"Vehículo con ID {vehiculo.ID} no tiene marca");
                
                if (string.IsNullOrEmpty(vehiculo.Modelo)) 
                    throw new Exception($"Vehículo con ID {vehiculo.ID} no tiene modelo");
                
                if (string.IsNullOrEmpty(vehiculo.Placa)) 
                    throw new Exception($"Vehículo con ID {vehiculo.ID} no tiene placa");
            }
        }

        private void ValidarJSONRepuestos(List<Repuesto> repuestos)
        {
            if (repuestos == null) throw new Exception("El archivo no contiene un formato válido de repuestos");
            
            foreach (var repuesto in repuestos)
            {
                if (repuesto.ID <= 0) 
                    throw new Exception($"Repuesto tiene ID inválido: {repuesto.ID}. Debe ser un número positivo.");
                
                if (string.IsNullOrEmpty(repuesto.RepuestoNombre)) 
                    throw new Exception($"Repuesto con ID {repuesto.ID} no tiene nombre");
                
                if (string.IsNullOrEmpty(repuesto.Detalles)) 
                    throw new Exception($"Repuesto con ID {repuesto.ID} no tiene detalles");
                
                if (repuesto.Costo <= 0) 
                    throw new Exception($"Repuesto con ID {repuesto.ID} tiene un costo inválido: {repuesto.Costo}. Debe ser mayor que cero.");
            }
        }

        private void MostrarMensajeExito(string mensaje)
        {
            try
            {
                using (MessageDialog dialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok, mensaje))
                {
                    dialog.Run();
                    dialog.Destroy();
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("CargaMasiva", "MostrarMensajeExito", ex);
                Console.WriteLine($"Error al mostrar mensaje de éxito: {ex.Message}");
            }
        }

        private void MostrarError(string mensaje)
        {
            try
            {
                Console.WriteLine($"\n❌ Error: {mensaje}");
                
                using (MessageDialog dialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, mensaje))
                {
                    dialog.Run();
                    dialog.Destroy();
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("CargaMasiva", "MostrarError", ex);
                Console.WriteLine($"Error al mostrar mensaje de error: {ex.Message}");
            }
        }
        
        protected override void OnDestroyed()
        {
            try
            {
                // Desconectar eventos
                if (_btnUsuarios != null)
                    _btnUsuarios.Clicked -= OnCargarUsuariosClicked;
                
                if (_btnVehiculos != null)
                    _btnVehiculos.Clicked -= OnCargarVehiculosClicked;
                
                if (_btnRepuestos != null)
                    _btnRepuestos.Clicked -= OnCargarRepuestosClicked;
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("CargaMasiva", "OnDestroyed", ex);
                Console.WriteLine($"Error al destruir la ventana: {ex.Message}");
            }
            finally
            {
                base.OnDestroyed();
            }
        }
    }
}