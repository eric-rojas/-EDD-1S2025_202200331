using Gtk;
using System;
using AutoGestPro.Core;
using AutoGestPro.UI;
public class Menu2InsertarVehiculo : Window
{
    private Usuario usuario;
    private ListaVehiculos listaVehiculos;
    private Entry txtId, txtMarca, txtModelo, txtPlaca;

    public Menu2InsertarVehiculo(Usuario usuario, ListaVehiculos listaVehiculos) : base("Registrar Vehículo")
    {
        this.usuario = usuario;
        this.listaVehiculos = listaVehiculos;
        
        SetDefaultSize(300, 250);
        BuildUI();
    }

    private void BuildUI()
    {
        var vbox = new VBox();
        
        // Campo ID
        var lblId = new Label("ID del vehículo:");
        txtId = new Entry();
        vbox.PackStart(lblId, false, false, 5);
        vbox.PackStart(txtId, false, false, 5);
        
        // ... Agregar más campos (Marca, Modelo, Placa) de la misma forma ...
        // Campo Marca
        var lblMarca = new Label("Marca:");
        txtMarca = new Entry();
        vbox.PackStart(lblMarca, false, false, 5);
        vbox.PackStart(txtMarca, false, false, 5);

        // Campo Modelo
        var lblModelo = new Label("Modelo:");
        txtModelo = new Entry();
        vbox.PackStart(lblModelo, false, false, 5);
        vbox.PackStart(txtModelo, false, false, 5);

        // Campo Placa
        var lblPlaca = new Label("Placa:");
        txtPlaca = new Entry();
        vbox.PackStart(lblPlaca, false, false, 5);
        vbox.PackStart(txtPlaca, false, false, 5);
        
        var btnGuardar = new Button("Guardar");
        btnGuardar.Clicked += OnGuardarClicked;
        vbox.PackStart(btnGuardar, false, false, 10);
        
        Add(vbox);
    }

    private void OnGuardarClicked(object sender, EventArgs e)
    {
        if (!int.TryParse(txtId.Text, out int idVehiculo))
        {
            MostrarError("ID inválido");
            return;
        }
        
        if (listaVehiculos.ExisteID(idVehiculo))
        {
            MostrarError("Este ID de vehículo ya está registrado");
            return;
        }
        
        var nuevoVehiculo = new Vehiculo(
            idVehiculo,
            usuario.ID,
            txtMarca.Text,
            txtModelo.Text,
            txtPlaca.Text
        );
        
        listaVehiculos.Insertar(nuevoVehiculo);
        MostrarExito("Vehículo registrado exitosamente");
        Destroy();
    }
    
    private void MostrarError(string mensaje)
    {
       
        var dialog = new MessageDialog(
            this,
            DialogFlags.Modal,
            MessageType.Error,
            ButtonsType.Ok,
            mensaje
        );
        dialog.Run();
        dialog.Destroy();
    }
    
    private void MostrarExito(string mensaje)
    {
        
        var dialog = new MessageDialog(
            this,
            DialogFlags.Modal,
            MessageType.Info,
            ButtonsType.Ok,
            mensaje
        );
        dialog.Run();
        dialog.Destroy();
    }
}