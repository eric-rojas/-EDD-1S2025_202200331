// Actualizacion de repuestos

using Gtk;
using System;
using AutoGestPro.Core;
namespace AutoGestPro.UI
{

    public class Ingreso2 : Window
    {
        private Entry codigoEntry, nombreEntry, cantidadEntry, precioEntry;
        private Button agregarButton, editarButton, eliminarButton;
        private TreeView repuestosView;
        private ArbolAVLRepuestos arbolRepuestos;

        public Ingreso2(ArbolAVLRepuestos arbol) : base("Gestión de Repuestos")
        {
            arbolRepuestos = arbol;
            SetDefaultSize(600, 400);
            SetPosition(WindowPosition.Center);
            DeleteEvent += delegate {  this.Destroy();};

            VBox vbox = new VBox();
            HBox hbox = new HBox();

            codigoEntry = new Entry { PlaceholderText = "Código" };
            nombreEntry = new Entry { PlaceholderText = "Nombre" };
            cantidadEntry = new Entry { PlaceholderText = "Cantidad" };
            precioEntry = new Entry { PlaceholderText = "Precio" };
            
            agregarButton = new Button("Agregar");
            editarButton = new Button("Editar");
            eliminarButton = new Button("Eliminar");
            
            hbox.PackStart(codigoEntry, false, false, 5);
            hbox.PackStart(nombreEntry, false, false, 5);
            hbox.PackStart(cantidadEntry, false, false, 5);
            hbox.PackStart(precioEntry, false, false, 5);
            hbox.PackStart(agregarButton, false, false, 5);
            hbox.PackStart(editarButton, false, false, 5);
            hbox.PackStart(eliminarButton, false, false, 5);

            repuestosView = new TreeView();
            ConfigurarTreeView();
            ScrolledWindow scrolledWindow = new ScrolledWindow();
            scrolledWindow.Add(repuestosView);
            
            vbox.PackStart(hbox, false, false, 5);
            vbox.PackStart(scrolledWindow, true, true, 5);

            Add(vbox);
            ShowAll();

            agregarButton.Clicked += OnAgregarClicked;
            editarButton.Clicked += OnEditarClicked;
            eliminarButton.Clicked += OnEliminarClicked;
        }

        private void ConfigurarTreeView()
        {
            repuestosView.AppendColumn("Código", new CellRendererText(), "text", 0);
            repuestosView.AppendColumn("Nombre", new CellRendererText(), "text", 1);
            repuestosView.AppendColumn("Cantidad", new CellRendererText(), "text", 2);
            repuestosView.AppendColumn("Precio", new CellRendererText(), "text", 3);
        }

        private void OnAgregarClicked(object sender, EventArgs e)
        {
            int codigo = int.Parse(codigoEntry.Text);
            string nombre = nombreEntry.Text;
            string cantidad = cantidadEntry.Text;
            double precio = double.Parse(precioEntry.Text);
            
            arbolRepuestos.Insertar(new Repuesto(codigo, nombre, cantidad, precio));
            ActualizarTreeView();
        }

        private void OnEditarClicked(object sender, EventArgs e)
        {
            // Lógica para editar repuesto
            int codigo = int.Parse(codigoEntry.Text);
            Repuesto repuestoExistente = arbolRepuestos.Buscar(codigo);

            if (repuestoExistente != null)
            {
                repuestoExistente.RepuestoNombre = nombreEntry.Text;
                repuestoExistente.Detalles = cantidadEntry.Text;
                repuestoExistente.Costo = double.Parse(precioEntry.Text);
                ActualizarTreeView();
            }
            else
            {
                MessageDialog dialog = new MessageDialog(this, 
                    DialogFlags.Modal, 
                    MessageType.Error, 
                    ButtonsType.Ok, 
                    "No se encontró el repuesto con el código especificado.");
                dialog.Run();
                dialog.Destroy();
            }
        }

        private void OnEliminarClicked(object sender, EventArgs e)
        {
            int codigo = int.Parse(codigoEntry.Text);
            arbolRepuestos.Eliminar(codigo);
            ActualizarTreeView();
        }

        private void ActualizarTreeView()
        {
            ListStore store = new ListStore(typeof(string), typeof(string), typeof(string), typeof(string));
            foreach (var repuesto in arbolRepuestos.ObtenerTodos())
            {
                store.AppendValues(repuesto.ID.ToString(), repuesto.RepuestoNombre, repuesto.Detalles.ToString(), repuesto.Costo.ToString());
            }
            repuestosView.Model = store;
        }
    }


}
/*public class Ingreso2 : Window
{
    private Entry codigoEntry;
    private Entry nombreEntry;
    private Entry descripcionEntry;
    private Entry precioEntry;
    private Repuesto repuesto;
    private Action<Repuesto> onUpdate;

    public Ingreso2(Repuesto repuesto, Action<Repuesto> onUpdate) : base("Actualizar Repuesto")
    { 
        this.repuesto = repuesto;
        this.onUpdate = onUpdate;
        SetDefaultSize(400, 300);
        SetPosition(WindowPosition.Center);
        
        VBox vbox = new VBox(false, 8);
        vbox.BorderWidth = 10;

        vbox.PackStart(new Label("Código:"), false, false, 0);
        codigoEntry = new Entry { Text = repuesto.ID.ToString(), Sensitive = false };
        vbox.PackStart(codigoEntry, false, false, 0);

        vbox.PackStart(new Label("Nombre:"), false, false, 0);
        nombreEntry = new Entry { Text = repuesto.RepuestoNombre };
        vbox.PackStart(nombreEntry, false, false, 0);

        vbox.PackStart(new Label("Descripción:"), false, false, 0);
        descripcionEntry = new Entry { Text = repuesto.Detalles };
        vbox.PackStart(descripcionEntry, false, false, 0);

        vbox.PackStart(new Label("Precio:"), false, false, 0);
        precioEntry = new Entry { Text = repuesto.Costo.ToString() };
        vbox.PackStart(precioEntry, false, false, 0);

        Button actualizarButton = new Button("Actualizar");
        actualizarButton.Clicked += OnActualizarClicked;
        vbox.PackStart(actualizarButton, false, false, 0);

        Button cancelarButton = new Button("Cancelar");
        cancelarButton.Clicked += (sender, e) => Destroy();
        vbox.PackStart(cancelarButton, false, false, 0);

        Add(vbox);
        ShowAll();
    }

    private void OnActualizarClicked(object sender, EventArgs e)
    {
        repuesto.RepuestoNombre = nombreEntry.Text;
        repuesto.Detalles = descripcionEntry.Text;
        
        if (decimal.TryParse(precioEntry.Text, out decimal costo))
        {
            repuesto.Costo = (double)costo;
            onUpdate?.Invoke(repuesto);
            Destroy();
        }
        else
        {
            MessageDialog errorDialog = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "Precio inválido");
            errorDialog.Run();
            errorDialog.Destroy();
        }
    }
}*/
