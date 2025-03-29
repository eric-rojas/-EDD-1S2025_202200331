using Gtk;
using System;
using AutoGestPro.Core;
using AutoGestPro.Utils;

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
            try
            {
                arbolRepuestos = arbol;
                SetDefaultSize(600, 400);
                SetPosition(WindowPosition.Center);
                
                // Manejar el evento de cierre correctamente
                DeleteEvent += (o, args) => {
                    try
                    {
                        this.Destroy();
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.LogError("Ingreso2", "DeleteEvent", ex);
                    }
                };

                CrearInterfaz();
                ConfigurarEventos();
                ActualizarTreeView();

                ShowAll();
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("Ingreso2", "Constructor", ex);
                ErrorHandler.MostrarError(this, "Error al inicializar la ventana de gestión de repuestos: " + ex.Message);
            }
        }

        private void CrearInterfaz()
        {
            try
            {
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
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("Ingreso2", "CrearInterfaz", ex);
                throw; // Relanzamos para que sea manejada por el constructor
            }
        }
        
        private void ConfigurarEventos()
        {
            try
            {
                agregarButton.Clicked += OnAgregarClicked;
                editarButton.Clicked += OnEditarClicked;
                eliminarButton.Clicked += OnEliminarClicked;
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("Ingreso2", "ConfigurarEventos", ex);
                throw; // Relanzamos para que sea manejada por el constructor
            }
        }

        private void ConfigurarTreeView()
        {
            try
            {
                repuestosView.AppendColumn("Código", new CellRendererText(), "text", 0);
                repuestosView.AppendColumn("Nombre", new CellRendererText(), "text", 1);
                repuestosView.AppendColumn("Cantidad", new CellRendererText(), "text", 2);
                repuestosView.AppendColumn("Precio", new CellRendererText(), "text", 3);
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("Ingreso2", "ConfigurarTreeView", ex);
                throw; // Relanzamos para que sea manejada por el constructor
            }
        }

        private void OnAgregarClicked(object sender, EventArgs e)
        {
            try
            {
                // Deshabilitar botón mientras se procesa
                agregarButton.Sensitive = false;
                
                // Validar campos
                if (!ValidarCampos(out int codigo, out string nombre, out string cantidad, out double precio))
                {
                    agregarButton.Sensitive = true;
                    return;
                }
                
                // Verificar si ya existe el repuesto
                if (arbolRepuestos.Buscar(codigo) != null)
                {
                    ErrorHandler.MostrarError(this, $"Ya existe un repuesto con el código {codigo}.");
                    agregarButton.Sensitive = true;
                    return;
                }
                
                // Insertar nuevo repuesto
                arbolRepuestos.Insertar(new Repuesto(codigo, nombre, cantidad, precio));
                
                // Actualizar vista y limpiar campos
                ActualizarTreeView();
                LimpiarCampos();
                
                ErrorHandler.MostrarInfo(this, "Repuesto agregado correctamente.");
                agregarButton.Sensitive = true;
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("Ingreso2", "OnAgregarClicked", ex);
                ErrorHandler.MostrarError(this, "Error al agregar repuesto: " + ex.Message);
                agregarButton.Sensitive = true;
            }
        }

        private void OnEditarClicked(object sender, EventArgs e)
        {
            try
            {
                // Deshabilitar botón mientras se procesa
                editarButton.Sensitive = false;
                
                // Validar código
                if (!int.TryParse(codigoEntry.Text, out int codigo))
                {
                    ErrorHandler.MostrarError(this, "El código debe ser un número entero válido.");
                    editarButton.Sensitive = true;
                    return;
                }
                
                // Buscar repuesto
                Repuesto repuestoExistente = arbolRepuestos.Buscar(codigo);
                
                if (repuestoExistente == null)
                {
                    ErrorHandler.MostrarError(this, "No se encontró el repuesto con el código especificado.");
                    editarButton.Sensitive = true;
                    return;
                }
                
                // Validar campos restantes
                if (string.IsNullOrWhiteSpace(nombreEntry.Text))
                {
                    ErrorHandler.MostrarError(this, "El nombre no puede estar vacío.");
                    editarButton.Sensitive = true;
                    return;
                }
                
                if (string.IsNullOrWhiteSpace(cantidadEntry.Text))
                {
                    ErrorHandler.MostrarError(this, "La cantidad no puede estar vacía.");
                    editarButton.Sensitive = true;
                    return;
                }
                
                if (!double.TryParse(precioEntry.Text, out double precio))
                {
                    ErrorHandler.MostrarError(this, "El precio debe ser un número válido.");
                    editarButton.Sensitive = true;
                    return;
                }
                
                // Actualizar repuesto
                repuestoExistente.RepuestoNombre = nombreEntry.Text;
                repuestoExistente.Detalles = cantidadEntry.Text;
                repuestoExistente.Costo = precio;
                
                // Actualizar vista y limpiar campos
                ActualizarTreeView();
                LimpiarCampos();
                
                ErrorHandler.MostrarInfo(this, "Repuesto actualizado correctamente.");
                editarButton.Sensitive = true;
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("Ingreso2", "OnEditarClicked", ex);
                ErrorHandler.MostrarError(this, "Error al editar repuesto: " + ex.Message);
                editarButton.Sensitive = true;
            }
        }

        private void OnEliminarClicked(object sender, EventArgs e)
        {
            try
            {
                // Deshabilitar botón mientras se procesa
                eliminarButton.Sensitive = false;
                
                // Validar código
                if (!int.TryParse(codigoEntry.Text, out int codigo))
                {
                    ErrorHandler.MostrarError(this, "El código debe ser un número entero válido.");
                    eliminarButton.Sensitive = true;
                    return;
                }
                
                // Verificar si existe el repuesto
                if (arbolRepuestos.Buscar(codigo) == null)
                {
                    ErrorHandler.MostrarError(this, "No se encontró el repuesto con el código especificado.");
                    eliminarButton.Sensitive = true;
                    return;
                }
                
                // Solicitar confirmación
                MessageDialog confirmDialog = new MessageDialog(
                    this,
                    DialogFlags.Modal,
                    MessageType.Question,
                    ButtonsType.YesNo,
                    $"¿Está seguro de eliminar el repuesto con código {codigo}?"
                );
                
                ResponseType response = (ResponseType)confirmDialog.Run();
                confirmDialog.Destroy();
                
                if (response == ResponseType.Yes)
                {
                    // Eliminar repuesto
                    arbolRepuestos.Eliminar(codigo);
                    
                    // Actualizar vista y limpiar campos
                    ActualizarTreeView();
                    LimpiarCampos();
                    
                    ErrorHandler.MostrarInfo(this, "Repuesto eliminado correctamente.");
                }
                
                eliminarButton.Sensitive = true;
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("Ingreso2", "OnEliminarClicked", ex);
                ErrorHandler.MostrarError(this, "Error al eliminar repuesto: " + ex.Message);
                eliminarButton.Sensitive = true;
            }
        }

        private void ActualizarTreeView()
        {
            try
            {
                ListStore store = new ListStore(typeof(string), typeof(string), typeof(string), typeof(string));
                
                var repuestos = arbolRepuestos.ObtenerTodos();
                if (repuestos != null)
                {
                    foreach (var repuesto in repuestos)
                    {
                        if (repuesto != null)
                        {
                            store.AppendValues(
                                repuesto.ID.ToString(),
                                repuesto.RepuestoNombre ?? string.Empty,
                                repuesto.Detalles?.ToString() ?? string.Empty,
                                repuesto.Costo.ToString("0.00")
                            );
                        }
                    }
                }
                
                repuestosView.Model = store;
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("Ingreso2", "ActualizarTreeView", ex);
                // No lanzamos la excepción para evitar fallos en cascada
                // Pero sí mostramos error al usuario
                ErrorHandler.MostrarError(this, "Error al actualizar la lista de repuestos: " + ex.Message);
            }
        }
        
        private bool ValidarCampos(out int codigo, out string nombre, out string cantidad, out double precio)
        {
            codigo = 0;
            nombre = string.Empty;
            cantidad = string.Empty;
            precio = 0;
            
            // Validar código
            if (!int.TryParse(codigoEntry.Text, out codigo))
            {
                ErrorHandler.MostrarError(this, "El código debe ser un número entero válido.");
                return false;
            }
            
            // Validar nombre
            nombre = nombreEntry.Text?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(nombre))
            {
                ErrorHandler.MostrarError(this, "El nombre no puede estar vacío.");
                return false;
            }
            
            // Validar cantidad
            cantidad = cantidadEntry.Text?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(cantidad))
            {
                ErrorHandler.MostrarError(this, "La cantidad no puede estar vacía.");
                return false;
            }
            
            // Validar precio
            if (!double.TryParse(precioEntry.Text, out precio) || precio < 0)
            {
                ErrorHandler.MostrarError(this, "El precio debe ser un número válido y mayor o igual a cero.");
                return false;
            }
            
            return true;
        }
        
        private void LimpiarCampos()
        {
            try
            {
                codigoEntry.Text = string.Empty;
                nombreEntry.Text = string.Empty;
                cantidadEntry.Text = string.Empty;
                precioEntry.Text = string.Empty;
                codigoEntry.GrabFocus();
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("Ingreso2", "LimpiarCampos", ex);
                // No mostramos error al usuario por ser una operación no crítica
            }
        }
        
        protected override void OnDestroyed()
        {
            try
            {
                // Desconectar todos los eventos
                if (agregarButton != null) 
                    agregarButton.Clicked -= OnAgregarClicked;
                    
                if (editarButton != null)
                    editarButton.Clicked -= OnEditarClicked;
                    
                if (eliminarButton != null)
                    eliminarButton.Clicked -= OnEliminarClicked;
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("Ingreso2", "OnDestroyed", ex);
            }
            finally
            {
                base.OnDestroyed();
            }
        }
    }
}


/*// Actualizacion de repuestos
//anterior
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
