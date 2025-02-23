using Gtk;
using System;
using AutoGestPro.Core;

namespace AutoGestPro.UI
{
    public class ServiciosView : Window
    {
        private Entry entryID;
        private Entry entryIdRepuesto;
        private Entry entryIdVehiculo;
        private Entry entryDetalles;
        private Entry entryCosto;
        private Button btnGuardar;

        // Referencias a las estructuras de datos
            private ListaRepuestos _repuestos;
            private ListaVehiculos _vehiculos;
            private ColaServicios _servicios;
            private PilaFacturas _facturas;
            private GeneradorServicio _generadorServicio;

            public ServiciosView(
                ListaRepuestos repuestos,
                ListaVehiculos vehiculos,
                ColaServicios servicios,
                PilaFacturas facturas) : base("Crear Servicio")
            {
                _repuestos = repuestos;
                _vehiculos = vehiculos;
                _servicios = servicios;
                _facturas = facturas;

                // Crear el generador de servicios
                _generadorServicio = new GeneradorServicio(
                    vehiculos, 
                    repuestos, 
                    servicios, 
                    facturas, 
                    new MatrizBitacora()); // Creamos una nueva instancia de MatrizBitacora

                InitializeComponents();
            }

        private void InitializeComponents()
        {
            // Configurar la ventana
            SetDefaultSize(400, 300);
            SetPosition(WindowPosition.Center);

            // Crear contenedor principal
            var vbox = new VBox(false, 5);
            vbox.BorderWidth = 10;

            // Crear los campos de entrada
            // ID
            var hboxID = new HBox(false, 5);
            hboxID.PackStart(new Label("ID:"), false, false, 0);
            entryID = new Entry();
            entryID.IsEditable = false; // El ID será autogenerado
            hboxID.PackStart(entryID, true, true, 0);
            vbox.PackStart(hboxID, false, false, 0);

            // ID Repuesto
            var hboxRepuesto = new HBox(false, 5);
            hboxRepuesto.PackStart(new Label("ID Repuesto:"), false, false, 0);
            entryIdRepuesto = new Entry();
            hboxRepuesto.PackStart(entryIdRepuesto, true, true, 0);
            vbox.PackStart(hboxRepuesto, false, false, 0);

            // ID Vehículo
            var hboxVehiculo = new HBox(false, 5);
            hboxVehiculo.PackStart(new Label("ID Vehículo:"), false, false, 0);
            entryIdVehiculo = new Entry();
            hboxVehiculo.PackStart(entryIdVehiculo, true, true, 0);
            vbox.PackStart(hboxVehiculo, false, false, 0);

            // Detalles
            var hboxDetalles = new HBox(false, 5);
            hboxDetalles.PackStart(new Label("Detalles:"), false, false, 0);
            entryDetalles = new Entry();
            hboxDetalles.PackStart(entryDetalles, true, true, 0);
            vbox.PackStart(hboxDetalles, false, false, 0);

            // Costo
            var hboxCosto = new HBox(false, 5);
            hboxCosto.PackStart(new Label("Costo:"), false, false, 0);
            entryCosto = new Entry();
            hboxCosto.PackStart(entryCosto, true, true, 0);
            vbox.PackStart(hboxCosto, false, false, 0);

            // Botón Guardar
            btnGuardar = new Button("Guardar");
            btnGuardar.Clicked += OnGuardarClicked;
            vbox.PackStart(btnGuardar, false, false, 0);

            Add(vbox);
        }

        private void OnGuardarClicked(object sender, EventArgs e)
        {
            try
            {
                // Validar y obtener valores
                if (!int.TryParse(entryIdRepuesto.Text, out int idRepuesto))
                {
                    ShowError("ID de repuesto inválido");
                    return;
                }

                if (!int.TryParse(entryIdVehiculo.Text, out int idVehiculo))
                {
                    ShowError("ID de vehículo inválido");
                    return;
                }

                if (!float.TryParse(entryCosto.Text, out float costo))
                {
                    ShowError("Costo inválido");
                    return;
                }

                string detalles = entryDetalles.Text;
                if (string.IsNullOrEmpty(detalles))
                {
                    ShowError("Los detalles son requeridos");
                    return;
                }

                // Generar el servicio
                bool resultado = _generadorServicio.GenerarNuevoServicio(
                    idVehiculo, idRepuesto, detalles, costo);

                if (resultado)
                {
                    ShowSuccess("Servicio generado exitosamente");
                    LimpiarCampos();
                }
                else
                {
                    ShowError("Error al generar el servicio");
                }
            }
            catch (Exception ex)
            {
                ShowError($"Error: {ex.Message}");
            }
        }

        private void ShowError(string message)
        {
            var dialog = new MessageDialog(
                this,
                DialogFlags.Modal,
                MessageType.Error,
                ButtonsType.Ok,
                message);
            dialog.Run();
            dialog.Destroy();
        }

        private void ShowSuccess(string message)
        {
            var dialog = new MessageDialog(
                this,
                DialogFlags.Modal,
                MessageType.Info,
                ButtonsType.Ok,
                message);
            dialog.Run();
            dialog.Destroy();
        }

        private void LimpiarCampos()
        {
            entryIdRepuesto.Text = "";
            entryIdVehiculo.Text = "";
            entryDetalles.Text = "";
            entryCosto.Text = "";
        }
    }
}