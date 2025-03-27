using System;
using Gtk;
using AutoGestPro.Core;
using System.Collections.Generic;

public class Menu2CancelarFacturas : Window
{
    private ArbolBFacturas arbolBFacturas;
    private Usuario usuarioLogueado;

    private ListBox listBoxFacturas;
    private Button btnCancelarFactura;
    private Entry entryFacturaID;

    public Menu2CancelarFacturas(Usuario usuario, ArbolBFacturas arbolFacturas)
        : base("Cancelar Facturas")
    {
        this.usuarioLogueado = usuario;
        this.arbolBFacturas = arbolFacturas;

        SetDefaultSize(400, 300);
        SetPosition(WindowPosition.Center);

        VBox vbox = new VBox(false, 5);
        Add(vbox);

        // Campo de texto para ingresar el ID de la factura a cancelar
        entryFacturaID = new Entry();
        entryFacturaID.PlaceholderText = "Ingresa ID de la factura a cancelar";
        vbox.PackStart(entryFacturaID, false, false, 5);

        // Lista para mostrar las facturas
        listBoxFacturas = new ListBox();
        vbox.PackStart(listBoxFacturas, true, true, 5);

        // Botón para cancelar la factura
        btnCancelarFactura = new Button("Cancelar Factura");
        btnCancelarFactura.Clicked += OnCancelarFacturaClicked;
        vbox.PackStart(btnCancelarFactura, false, false, 5);

        ShowAll();

        // Mostrar las facturas del usuario logueado
        MostrarFacturas();
    }

    // Mostrar las facturas del usuario logueado
    private void MostrarFacturas()
    {
        // Obtener las facturas del usuario logueado
        List<Factura> facturas = arbolBFacturas.ObtenerFacturasPorUsuario(usuarioLogueado.ID);

        // Limpiar la lista antes de agregar nuevas
        foreach (var widget in listBoxFacturas.Children)
        {
            widget.Destroy();
        }

        // Mostrar las facturas en la lista
        foreach (var factura in facturas)
        {
            listBoxFacturas.Add(new Label(factura.ToString()));
        }

        listBoxFacturas.ShowAll();
    }

    // Método que se ejecuta al hacer clic en "Cancelar Factura"
    private void OnCancelarFacturaClicked(object sender, EventArgs e)
    {
        int idFactura;
        if (int.TryParse(entryFacturaID.Text, out idFactura))
        {
            // Buscar la factura por ID
            Factura factura = arbolBFacturas.BuscarPorID(idFactura);

            if (factura != null && factura.ID_Usuario == usuarioLogueado.ID)
            {
                // Eliminar la factura
                CancelarFactura(idFactura);
                MostrarFacturas(); // Actualizar la lista de facturas mostradas
                ShowMessage("Factura cancelada correctamente.");
            }
            else
            {
                ShowMessage("Factura no encontrada o no pertenece a este usuario.");
            }
        }
        else
        {
            ShowMessage("Por favor ingresa un ID válido.");
        }
    }

    // Método para cancelar (eliminar) la factura
    private void CancelarFactura(int idFactura)
    {
        bool facturaEliminada = arbolBFacturas.Eliminar(idFactura);
        if (!facturaEliminada)
        {
            ShowMessage("Error al cancelar la factura.");
        }
    }

    // Mostrar un mensaje en la interfaz de usuario
    private void ShowMessage(string message)
    {
        MessageDialog md = new MessageDialog(this, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok, message);
        md.Run();
        md.Destroy();
    }
}
