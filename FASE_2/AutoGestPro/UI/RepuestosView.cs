/*
// visualizacion preorden, inorden y postorden de los repuestos

using System;
using Gtk;
using System.Collections.Generic;
using AutoGestPro.Core;

namespace AutoGestPro.UI
{
    public class RepuestosView : Window
    {
        private ComboBox comboTipoOrden;
        private ListStore storeRepuestos;
        private TreeView treeRepuestos;
        private ArbolAVLRepuestos arbolRepuestos;

        public RepuestosView(ArbolAVLRepuestos arbolRepuestos) : base("Visualización de Repuestos")
        {
            this.arbolRepuestos = arbolRepuestos;
            SetDefaultSize(600, 400);
            SetPosition(WindowPosition.Center);
            
            // Contenedor principal
            VBox mainBox = new VBox(false, 10);
            mainBox.BorderWidth = 15;
            
            // Título
            Label lblTitulo = new Label();
            lblTitulo.Markup = "<span font='16' weight='bold'>Visualización de Repuestos</span>";
            mainBox.PackStart(lblTitulo, false, false, 10);
            
            // Combobox para selección de orden
            HBox hboxTipoOrden = new HBox(false, 5);
            
            // Modelo para el combobox
            ListStore storeOrden = new ListStore(typeof(string));
            storeOrden.AppendValues("Pre-Orden");
            storeOrden.AppendValues("In-Orden");
            storeOrden.AppendValues("Post-Orden");
            
            comboTipoOrden = new ComboBox();
            comboTipoOrden.Model = storeOrden;
            
            // Configurar renderizado del texto
            CellRendererText cell = new CellRendererText();
            comboTipoOrden.PackStart(cell, true);
            comboTipoOrden.AddAttribute(cell, "text", 0);
            comboTipoOrden.Active = 1; // In-Orden por defecto
            
            // Estilo para el combobox (color gris)
            comboTipoOrden.Name = "comboOrden";
            Rc.ParseString("style \"comboStyle\" { bg[NORMAL] = \"#808080\" fg[NORMAL] = \"#FFFFFF\" } widget \"*.comboOrden\" style \"comboStyle\"");
            
            // Evento para actualizar la vista
            comboTipoOrden.Changed += OnComboTipoOrdenChanged;
            
            // Agregar a la caja horizontal
            hboxTipoOrden.PackStart(comboTipoOrden, true, true, 0);
            
            // Agregar caja horizontal al contenedor principal
            mainBox.PackStart(hboxTipoOrden, false, false, 0);
            
            // TreeView para mostrar los repuestos
            CreateTreeView();
            
            // ScrolledWindow para el TreeView
            ScrolledWindow scrollRepuestos = new ScrolledWindow();
            scrollRepuestos.ShadowType = ShadowType.EtchedIn;
            scrollRepuestos.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
            scrollRepuestos.Add(treeRepuestos);
            
            // Agregar el ScrolledWindow al contenedor principal
            mainBox.PackStart(scrollRepuestos, true, true, 0);
            
            // Botones adicionales (opcional)
            HBox hboxBotones = new HBox(true, 5);
            
            Button btnCerrar = new Button("Cerrar");
            btnCerrar.Clicked += (sender, e) => this.Destroy();
            
            Button btnExportar = new Button("Exportar a PDF");
            btnExportar.Clicked += OnExportarClicked;
            
            hboxBotones.PackStart(btnExportar, true, true, 0);
            hboxBotones.PackStart(btnCerrar, true, true, 0);
            
            mainBox.PackStart(hboxBotones, false, false, 0);
            
            // Añadir el contenedor principal a la ventana
            Add(mainBox);
            
            // Cargar datos por defecto (In-Orden)
            CargarDatos();
            
            // Mostrar todos los widgets
            ShowAll();
        }
        
        private void CreateTreeView()
        {
            // Modelo para el TreeView
            storeRepuestos = new ListStore(typeof(string), typeof(string), typeof(string), typeof(string));
            
            // TreeView
            treeRepuestos = new TreeView(storeRepuestos);
            treeRepuestos.HeadersVisible = true;
            
            // Configurar columnas
            treeRepuestos.AppendColumn("Id", new CellRendererText(), "text", 0);
            treeRepuestos.AppendColumn("Repuesto", new CellRendererText(), "text", 1);
            treeRepuestos.AppendColumn("Detalles", new CellRendererText(), "text", 2);
            treeRepuestos.AppendColumn("Costo", new CellRendererText(), "text", 3);
            
            // Configurar anchos de columnas
            foreach (TreeViewColumn column in treeRepuestos.Columns)
            {
                column.Resizable = true;
                column.Alignment = 0.5f;
            }
            
            treeRepuestos.Columns[0].MinWidth = 50;
            treeRepuestos.Columns[1].MinWidth = 150;
            treeRepuestos.Columns[2].MinWidth = 200;
            treeRepuestos.Columns[3].MinWidth = 100;
        }
        
        private void CargarDatos()
        {
            // Limpiar datos anteriores
            storeRepuestos.Clear();
            
            // Obtener el tipo de orden seleccionado
            TreeIter iter;
            if (!comboTipoOrden.GetActiveIter(out iter))
                return;
                
            string tipoOrden = (string)comboTipoOrden.Model.GetValue(iter, 0);
            List<Repuesto> repuestos = null;
            
            // Obtener los repuestos según el orden seleccionado
            switch (tipoOrden)
            {
                case "Pre-Orden":
                    repuestos = arbolRepuestos.RecorridoPreOrden();
                    break;
                case "In-Orden":
                    repuestos = arbolRepuestos.ObtenerTodos(); // Este método ya devuelve en InOrden
                    break;
                case "Post-Orden":
                    repuestos = arbolRepuestos.RecorridoPostOrden();
                    break;
                default:
                    repuestos = arbolRepuestos.ObtenerTodos();
                    break;
            }
            
            // Llenar la tabla con los datos
            if (repuestos != null)
            {
                foreach (var repuesto in repuestos)
                {
                    storeRepuestos.AppendValues(
                        repuesto.ID.ToString(),
                        repuesto.RepuestoNombre,
                        repuesto.Detalles,
                        repuesto.Costo.ToString("C")
                    );
                }
            }
        }
        
        private void OnComboTipoOrdenChanged(object sender, EventArgs e)
        {
            CargarDatos();
        }
        
        private void OnExportarClicked(object sender, EventArgs e)
        {
            // Implementar exportación a PDF o generar reporte visual
            try
            {
                // Aquí podrías usar GraphvizExporter.cs para generar una visualización
                // del árbol AVL o exportar la lista actual a un PDF
                
                string graphviz = arbolRepuestos.GenerarGraphviz();
                // Guardar el contenido Graphviz a un archivo o procesarlo
                
                MessageDialog dialog = new MessageDialog(
                    this,
                    DialogFlags.Modal,
                    MessageType.Info,
                    ButtonsType.Ok,
                    "Visualización del árbol generada correctamente.");
                dialog.Run();
                dialog.Destroy();
            }
            catch (Exception ex)
            {
                MessageDialog dialog = new MessageDialog(
                    this,
                    DialogFlags.Modal,
                    MessageType.Error,
                    ButtonsType.Ok,
                    $"Error al generar la visualización: {ex.Message}");
                dialog.Run();
                dialog.Destroy();
            }
        }
        
        // Método estático para mostrar la ventana
        public static void Show(ArbolAVLRepuestos arbolRepuestos)
        {
            new RepuestosView(arbolRepuestos);
        }
    }
}*/

using System;
using Gtk;
using System.Collections.Generic;
using AutoGestPro.Core;
using AutoGestPro.Utils;

namespace AutoGestPro.UI
{
    public class RepuestosView : Window
    {
        private ComboBox _comboTipoOrden;
        private ListStore _storeRepuestos;
        private TreeView _treeRepuestos;
        private readonly ArbolAVLRepuestos _arbolRepuestos;
        private Button _btnExportar;
        private Button _btnCerrar;

        public RepuestosView(ArbolAVLRepuestos arbolRepuestos) : base("Visualización de Repuestos")
        {
            try
            {
                _arbolRepuestos = arbolRepuestos;
                SetDefaultSize(600, 400);
                SetPosition(WindowPosition.Center);
                
                // Configurar evento de cierre
                DeleteEvent += (o, args) => {
                    try
                    {
                        Destroy();
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.LogError("RepuestosView", "DeleteEvent", ex);
                    }
                };
                
                CrearInterfaz();
                
                // Cargar datos por defecto (In-Orden)
                CargarDatos();
                
                ShowAll();
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("RepuestosView", "Constructor", ex);
                ErrorHandler.MostrarError(this, "Error al inicializar la ventana de repuestos: " + ex.Message);
            }
        }
        
        private void CrearInterfaz()
        {
            try
            {
                // Contenedor principal
                VBox mainBox = new VBox(false, 10);
                mainBox.BorderWidth = 15;
                
                // Título
                Label lblTitulo = new Label();
                lblTitulo.Markup = "<span font='16' weight='bold'>Visualización de Repuestos</span>";
                mainBox.PackStart(lblTitulo, false, false, 10);
                
                // Combobox para selección de orden
                HBox hboxTipoOrden = new HBox(false, 5);
                
                // Etiqueta para el combo
                Label lblTipoOrden = new Label("Tipo de recorrido:");
                hboxTipoOrden.PackStart(lblTipoOrden, false, false, 5);
                
                // Modelo para el combobox
                ListStore storeOrden = new ListStore(typeof(string));
                storeOrden.AppendValues("Pre-Orden");
                storeOrden.AppendValues("In-Orden");
                storeOrden.AppendValues("Post-Orden");
                
                _comboTipoOrden = new ComboBox();
                _comboTipoOrden.Model = storeOrden;
                
                // Configurar renderizado del texto
                CellRendererText cell = new CellRendererText();
                _comboTipoOrden.PackStart(cell, true);
                _comboTipoOrden.AddAttribute(cell, "text", 0);
                _comboTipoOrden.Active = 1; // In-Orden por defecto
                
                // Estilo para el combobox (color gris)
                try
                {
                    _comboTipoOrden.Name = "comboOrden";
                    Rc.ParseString("style \"comboStyle\" { bg[NORMAL] = \"#808080\" fg[NORMAL] = \"#FFFFFF\" } widget \"*.comboOrden\" style \"comboStyle\"");
                }
                catch (Exception ex)
                {
                    // Si falla el estilo, solo lo registramos pero continuamos
                    ErrorHandler.LogError("RepuestosView", "EstiloCombo", ex);
                }
                
                // Evento para actualizar la vista
                _comboTipoOrden.Changed += OnComboTipoOrdenChanged;
                
                // Agregar a la caja horizontal
                hboxTipoOrden.PackStart(_comboTipoOrden, true, true, 0);
                
                // Agregar caja horizontal al contenedor principal
                mainBox.PackStart(hboxTipoOrden, false, false, 0);
                
                // TreeView para mostrar los repuestos
                CrearTreeView();
                
                // ScrolledWindow para el TreeView
                ScrolledWindow scrollRepuestos = new ScrolledWindow();
                scrollRepuestos.ShadowType = ShadowType.EtchedIn;
                scrollRepuestos.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);
                scrollRepuestos.Add(_treeRepuestos);
                
                // Agregar el ScrolledWindow al contenedor principal
                mainBox.PackStart(scrollRepuestos, true, true, 0);
                
                // Botones adicionales
                HBox hboxBotones = new HBox(true, 5);
                
                _btnCerrar = new Button("Cerrar");
                _btnCerrar.Clicked += OnCerrarClicked;
                
                _btnExportar = new Button("Exportar a PDF");
                _btnExportar.Clicked += OnExportarClicked;
                
                hboxBotones.PackStart(_btnExportar, true, true, 0);
                hboxBotones.PackStart(_btnCerrar, true, true, 0);
                
                mainBox.PackStart(hboxBotones, false, false, 0);
                
                // Añadir el contenedor principal a la ventana
                Add(mainBox);
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("RepuestosView", "CrearInterfaz", ex);
                throw; // Relanzamos para que sea manejada por el constructor
            }
        }
        
        private void CrearTreeView()
        {
            try
            {
                // Modelo para el TreeView
                _storeRepuestos = new ListStore(typeof(string), typeof(string), typeof(string), typeof(string));
                
                // TreeView
                _treeRepuestos = new TreeView(_storeRepuestos);
                _treeRepuestos.HeadersVisible = true;
                
                // Configurar columnas
                _treeRepuestos.AppendColumn("Id", new CellRendererText(), "text", 0);
                _treeRepuestos.AppendColumn("Repuesto", new CellRendererText(), "text", 1);
                _treeRepuestos.AppendColumn("Detalles", new CellRendererText(), "text", 2);
                _treeRepuestos.AppendColumn("Costo", new CellRendererText(), "text", 3);
                
                // Configurar anchos de columnas
                foreach (TreeViewColumn column in _treeRepuestos.Columns)
                {
                    column.Resizable = true;
                    column.Alignment = 0.5f;
                }
                
                _treeRepuestos.Columns[0].MinWidth = 50;
                _treeRepuestos.Columns[1].MinWidth = 150;
                _treeRepuestos.Columns[2].MinWidth = 200;
                _treeRepuestos.Columns[3].MinWidth = 100;
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("RepuestosView", "CrearTreeView", ex);
                throw; // Relanzamos para que sea manejada por CrearInterfaz
            }
        }
        
        private void CargarDatos()
        {
            try
            {
                // Deshabilitar combo mientras se carga
                if (_comboTipoOrden != null)
                    _comboTipoOrden.Sensitive = false;
                
                // Limpiar datos anteriores
                if (_storeRepuestos != null)
                    _storeRepuestos.Clear();
                else
                    return;
                
                // Obtener el tipo de orden seleccionado
                TreeIter iter;
                if (_comboTipoOrden == null || !_comboTipoOrden.GetActiveIter(out iter))
                {
                    if (_comboTipoOrden != null)
                        _comboTipoOrden.Sensitive = true;
                    return;
                }
                
                string tipoOrden = (string)_comboTipoOrden.Model.GetValue(iter, 0);
                List<Repuesto> repuestos = null;
                
                // Obtener los repuestos según el orden seleccionado
                try
                {
                    switch (tipoOrden)
                    {
                        case "Pre-Orden":
                            repuestos = _arbolRepuestos.RecorridoPreOrden();
                            break;
                        case "In-Orden":
                            repuestos = _arbolRepuestos.ObtenerTodos(); // Este método ya devuelve en InOrden
                            break;
                        case "Post-Orden":
                            repuestos = _arbolRepuestos.RecorridoPostOrden();
                            break;
                        default:
                            repuestos = _arbolRepuestos.ObtenerTodos();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    ErrorHandler.LogError("RepuestosView", "ObtenerRepuestos", ex);
                    ErrorHandler.MostrarError(this, "Error al obtener los repuestos: " + ex.Message);
                    
                    if (_comboTipoOrden != null)
                        _comboTipoOrden.Sensitive = true;
                    return;
                }
                
                // Llenar la tabla con los datos
                if (repuestos != null)
                {
                    foreach (var repuesto in repuestos)
                    {
                        if (repuesto != null)
                        {
                            try
                            {
                                _storeRepuestos.AppendValues(
                                    repuesto.ID.ToString(),
                                    repuesto.RepuestoNombre ?? string.Empty,
                                    repuesto.Detalles ?? string.Empty,
                                    repuesto.Costo.ToString("C")
                                );
                            }
                            catch (Exception ex)
                            {
                                ErrorHandler.LogError("RepuestosView", "AgregarRepuesto", ex);
                                // Continuamos con el siguiente repuesto
                            }
                        }
                    }
                }
                
                // Mostrar mensaje si no hay repuestos
                if (repuestos == null || repuestos.Count == 0)
                {
                    _storeRepuestos.AppendValues(
                        string.Empty,
                        "No hay repuestos disponibles",
                        string.Empty,
                        string.Empty
                    );
                }
                
                // Rehabilitar combo
                if (_comboTipoOrden != null)
                    _comboTipoOrden.Sensitive = true;
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("RepuestosView", "CargarDatos", ex);
                ErrorHandler.MostrarError(this, "Error al cargar los datos: " + ex.Message);
                
                // Asegurar que el combo se rehabilita
                if (_comboTipoOrden != null)
                    _comboTipoOrden.Sensitive = true;
            }
        }
        
        private void OnComboTipoOrdenChanged(object sender, EventArgs e)
        {
            try
            {
                CargarDatos();
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("RepuestosView", "OnComboTipoOrdenChanged", ex);
                ErrorHandler.MostrarError(this, "Error al cambiar el tipo de orden: " + ex.Message);
            }
        }
        
        private void OnCerrarClicked(object sender, EventArgs e)
        {
            try
            {
                Destroy();
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("RepuestosView", "OnCerrarClicked", ex);
                // Forzar cierre en caso de error
                try { Destroy(); } catch { }
            }
        }
        
        private void OnExportarClicked(object sender, EventArgs e)
        {
            try
            {
                // Deshabilitar botón mientras se procesa
                _btnExportar.Sensitive = false;
                
                // Implementar exportación a PDF o generar reporte visual
                try
                {
                    // Aquí podrías usar GraphvizExporter.cs para generar una visualización
                    // del árbol AVL o exportar la lista actual a un PDF
                    
                    string graphviz = _arbolRepuestos.GenerarGraphviz();
                    // Guardar el contenido Graphviz a un archivo o procesarlo
                    
                    ErrorHandler.MostrarInfo(this, "Visualización del árbol generada correctamente.");
                }
                catch (Exception ex)
                {
                    ErrorHandler.LogError("RepuestosView", "GenerarGraphviz", ex);
                    ErrorHandler.MostrarError(this, "Error al generar la visualización: " + ex.Message);
                }
                
                // Rehabilitar botón
                _btnExportar.Sensitive = true;
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("RepuestosView", "OnExportarClicked", ex);
                
                // Asegurar que el botón se rehabilita incluso si hay error
                _btnExportar.Sensitive = true;
            }
        }
        
        // Método estático para mostrar la ventana
        public static void Show(ArbolAVLRepuestos arbolRepuestos)
        {
            try
            {
                new RepuestosView(arbolRepuestos).Show();
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("RepuestosView", "Show", ex);
                Console.WriteLine($"Error al mostrar la ventana de repuestos: {ex.Message}");
            }
        }
        
        protected override void OnDestroyed()
        {
            try
            {
                // Desconectar eventos
                if (_comboTipoOrden != null)
                    _comboTipoOrden.Changed -= OnComboTipoOrdenChanged;
                
                if (_btnCerrar != null)
                    _btnCerrar.Clicked -= OnCerrarClicked;
                
                if (_btnExportar != null)
                    _btnExportar.Clicked -= OnExportarClicked;
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("RepuestosView", "OnDestroyed", ex);
            }
            finally
            {
                base.OnDestroyed();
            }
        }
    }
}