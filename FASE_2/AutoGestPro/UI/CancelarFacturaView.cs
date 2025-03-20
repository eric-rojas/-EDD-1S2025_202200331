/*using Gtk;
using System;
using AutoGestPro.Core;

namespace AutoGestPro.UI
{
    public class CancelarFacturaView : Window
    {
        private PilaFacturas _pilaFacturas;
        private Label lblInfoFactura;
        private Button btnCancelar;

        public CancelarFacturaView(PilaFacturas pilaFacturas) : base("Cancelar Factura")
        {
            _pilaFacturas = pilaFacturas;
            InitializeComponents();
            MostrarFacturaActual();
        }

        private void InitializeComponents()
        {
            // Configurar la ventana
            SetDefaultSize(400, 200);
            SetPosition(WindowPosition.Center);

            var vbox = new VBox(false, 5);
            vbox.BorderWidth = 10;

            // Label para mostrar la información de la factura
            lblInfoFactura = new Label("");
            vbox.PackStart(lblInfoFactura, true, true, 0);

            // Botón de cancelar
            btnCancelar = new Button("Cancelar Factura");
            btnCancelar.Clicked += OnCancelarClicked;
            vbox.PackStart(btnCancelar, false, false, 0);

            Add(vbox);
        }

        private void MostrarFacturaActual()
        {
            if (_pilaFacturas.MostrarTope())
            {
                btnCancelar.Sensitive = true; // Habilitar el botón
            }
            else
            {
                lblInfoFactura.Text = "No hay facturas pendientes para cancelar";
                btnCancelar.Sensitive = false; // Deshabilitar el botón
            }
        }

        private void OnCancelarClicked(object sender, EventArgs e)
        {
            var dialog = new MessageDialog(
                this,
                DialogFlags.Modal,
                MessageType.Question,
                ButtonsType.YesNo,
                "¿Está seguro que desea cancelar esta factura?");

            ResponseType response = (ResponseType)dialog.Run();
            dialog.Destroy();

            if (response == ResponseType.Yes)
            {
                _pilaFacturas.Desapilar();
                var successDialog = new MessageDialog(
                    this,
                    DialogFlags.Modal,
                    MessageType.Info,
                    ButtonsType.Ok,
                    "Factura cancelada exitosamente");
                successDialog.Run();
                successDialog.Destroy();
                this.Destroy(); // Cerrar la ventana después de cancelar
            }
        }
    }
}


namespace AutoGestPro.UI
{
    public class CancelarFacturaView : Window
    {
        private PilaFacturas _pilaFacturas;
        private Label lblInfoFactura;
        private Button btnCancelar;
        private bool _isClosing;

        public CancelarFacturaView(PilaFacturas pilaFacturas) : base("Cancelar Factura")
        {
            _pilaFacturas = pilaFacturas;
            _isClosing = false;

            // Manejar el evento DeleteEvent
            this.DeleteEvent += (o, args) =>
            {
                if (!_isClosing)
                {
                    args.RetVal = true;
                    this.Hide();
                    this.Dispose();
                }
            };

            InitializeComponents();
            MostrarFacturaActual();
        }

        private void InitializeComponents()
        {
            SetDefaultSize(400, 200);
            SetPosition(WindowPosition.Center);

            var vbox = new VBox(false, 5);
            vbox.BorderWidth = 10;

            lblInfoFactura = new Label("");
            vbox.PackStart(lblInfoFactura, true, true, 0);

            btnCancelar = new Button("Cancelar Factura");
            btnCancelar.Clicked += OnCancelarClicked;
            vbox.PackStart(btnCancelar, false, false, 0);

            Add(vbox);
        }

        private void MostrarFacturaActual()
        {
            if (_pilaFacturas.MostrarTope())
            {
                btnCancelar.Sensitive = true;
            }
            else
            {
                lblInfoFactura.Text = "No hay facturas pendientes para cancelar";
                btnCancelar.Sensitive = false;
            }
        }

        private void OnCancelarClicked(object sender, EventArgs e)
        {
            // Verificar que hay una factura para cancelar
            if (_pilaFacturas.MostrarTope())
            {
                // Obtener la información de la factura antes de la confirmación
                string infoFactura = _pilaFacturas.GetInfoTope();
                
                using (var dialog = new MessageDialog(
                    this,
                    DialogFlags.Modal,
                    MessageType.Question,
                    ButtonsType.YesNo,
                    "¿Está seguro que desea cancelar esta factura?"))
                {
                    ResponseType response = (ResponseType)dialog.Run();
                    dialog.Destroy();

                    if (response == ResponseType.Yes)
                    {
                        // Desapilar la factura
                        _pilaFacturas.Desapilar();
                        
                        // Mostrar confirmación con los datos de la factura
                        using (var successDialog = new MessageDialog(
                            this,
                            DialogFlags.Modal,
                            MessageType.Info,
                            ButtonsType.Ok,
                            $"Factura cancelada exitosamente:\n\n{infoFactura}"))
                        {
                            successDialog.Run();
                            successDialog.Destroy();
                        }

                        _isClosing = true;
                        this.Hide();
                        this.Dispose();
                    }
                }
            }
        }

        protected override void OnDestroyed()
        {
            base.OnDestroyed();
            if (btnCancelar != null)
            {
                btnCancelar.Clicked -= OnCancelarClicked;
            }
        }
    }
}*/