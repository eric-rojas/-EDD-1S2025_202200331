using System;
using System.Collections.Generic;
using Gtk;
using AutoGestPro.Core;

public class Menu2ServiciosView : Window
{
    private ListaVehiculos listaVehiculos;
    private ArbolBinarioServicios arbolServicios;
    private Usuario usuarioLogueado;
    
    private ComboBox comboBoxRecorridos;
    private ListBox listBoxServicios;
    private Button btnActualizar;

    public Menu2ServiciosView(Usuario usuario, ListaVehiculos listaVehiculos, ArbolBinarioServicios arbolServicios) 
        : base("Servicios por Vehículo")
    {
        this.usuarioLogueado = usuario;
        this.listaVehiculos = listaVehiculos;
        this.arbolServicios = arbolServicios;

        SetDefaultSize(400, 300);
        SetPosition(WindowPosition.Center);
        
        VBox vbox = new VBox(false, 5);
        Add(vbox);

        // Dropdown para seleccionar tipo de recorrido
        comboBoxRecorridos = new ComboBox();
        var store = new ListStore(typeof(string));
        comboBoxRecorridos.Model = store;
        var renderer = new CellRendererText();
        comboBoxRecorridos.PackStart(renderer, true);
        comboBoxRecorridos.AddAttribute(renderer, "text", 0);
        store.AppendValues("Pre-Orden");
        store.AppendValues("In-Orden");
        store.AppendValues("Post-Orden");
        comboBoxRecorridos.Active = 0; // Por defecto, Pre-Orden
        vbox.PackStart(comboBoxRecorridos, false, false, 5);

        // Lista para mostrar los servicios
        listBoxServicios = new ListBox();
        vbox.PackStart(listBoxServicios, true, true, 5);

        // Botón para actualizar la lista
        btnActualizar = new Button("Actualizar");
        btnActualizar.Clicked += OnActualizarClicked;
        vbox.PackStart(btnActualizar, false, false, 5);

        ShowAll();
    }

    private void OnActualizarClicked(object sender, EventArgs e)
    {
        MostrarServicios(comboBoxRecorridos.Active + 1);
    }

    private void MostrarServicios(int tipoRecorrido)
    {
        List<Servicio> servicios = new List<Servicio>();

        // Obtener los vehículos del usuario logueado
        List<Vehiculo> vehiculosUsuario = listaVehiculos.BuscarPorUsuario(usuarioLogueado.ID);


        // Recorrer cada vehículo y obtener sus servicios
        foreach (var vehiculo in vehiculosUsuario)
        {
            servicios.AddRange(arbolServicios.BuscarPorVehiculo(vehiculo.ID));
        }

        // Aplicar el recorrido seleccionado
        switch (tipoRecorrido)
        {
            case 1:
                servicios = arbolServicios.RecorridoPreOrden();
                break;
            case 2:
                servicios = arbolServicios.RecorridoInOrden();
                break;
            case 3:
                servicios = arbolServicios.RecorridoPostOrden();
                break;
        }

        // Mostrar los servicios en la UI
        listBoxServicios.Foreach((widget) => widget.Destroy());
        foreach (var servicio in servicios)
        {
            listBoxServicios.Add(new Label(servicio.ToString()));
        }

        listBoxServicios.ShowAll();
    }
}
