using System;
using System.Collections.Generic;
using Gtk;
using AutoGestPro.Core;

public class Menu2FacturasView : Window
{
    private ArbolBFacturas arbolBFacturas; 
    private ListBox arbolFacturasListBox;
    private Button btnActualizar;
    private ListaVehiculos listaVehiculos;
    private ArbolBinarioServicios arbolServicios;
    private Usuario usuarioLogueado;

   


    public Menu2FacturasView(Usuario usuario, ArbolBFacturas arbolFacturas) 
    : base("Facturas Pendientes")
    {
        if (usuario == null) throw new ArgumentNullException(nameof(usuario), "El usuario logueado no puede ser null.");
        if (arbolFacturas == null) throw new ArgumentNullException(nameof(arbolFacturas), "El árbol de facturas no puede ser null.");

        this.arbolBFacturas = arbolFacturas;
        this.usuarioLogueado = usuario;

        SetDefaultSize(400, 300);
        SetPosition(WindowPosition.Center);

        VBox vbox = new VBox(false, 5);
        Add(vbox);

        arbolFacturasListBox = new ListBox();
        vbox.PackStart(arbolFacturasListBox, true, true, 5);

        btnActualizar = new Button("Actualizar");
        btnActualizar.Clicked += OnActualizarClicked;
        vbox.PackStart(btnActualizar, false, false, 5);

        ShowAll();
    }


    private void OnActualizarClicked(object sender, EventArgs e)
    {
        MostrarFacturas();
    }

    private void MostrarFacturas()
    {
        if (arbolBFacturas == null)
        {
            Console.WriteLine("Error: arbolBFacturas es null");
            return;
        }

        if (usuarioLogueado == null)
        {
            Console.WriteLine("Error: usuarioLogueado es null");
            return;
        }

        List<Factura> facturas = arbolBFacturas.ObtenerFacturasPorUsuario(usuarioLogueado.ID) ?? new List<Factura>();

        foreach (var widget in arbolFacturasListBox.Children)
        {
            widget.Destroy();
        }

        foreach (var factura in facturas)
        {
            arbolFacturasListBox.Add(new Label(factura.ToString()));
        }

        arbolFacturasListBox.ShowAll();
    }


    /*private void MostrarFacturas()
    {
        
        List<Factura> facturas = arbolBFacturas.ObtenerFacturasPorUsuario(usuarioLogueado.ID);

        
        foreach (var widget in arbolFacturasListBox.Children)
        {
            widget.Destroy(); 
        }

        
        foreach (var factura in facturas)
        {
            arbolFacturasListBox.Add(new Label(factura.ToString())); 
        }

        
        arbolFacturasListBox.ShowAll();
    }*/

}
