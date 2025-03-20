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
}