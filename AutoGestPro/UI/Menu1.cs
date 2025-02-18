// Menu1.cs
using Gtk;
using System;
using AutoGestPro.Core;

namespace AutoGestPro
{
    public class Menu1 : Window
    {
        private readonly ListaUsuarios _listaUsuarios;

        public Menu1(ListaUsuarios listaUsuarios) : base("AutoGestPro - Menú Principal")
        {
            _listaUsuarios = listaUsuarios;
            
            SetDefaultSize(300, 200);
            SetPosition(WindowPosition.Center);

            VBox vbox = new VBox();
            vbox.Spacing = 5;

            Label label = new Label("Menú Principal");
            vbox.PackStart(label, false, false, 0);

            Button Btn_CargaMasiva = new Button("Carga Masiva");
            Btn_CargaMasiva.Clicked += GoCargaMasiva;
            vbox.PackStart(Btn_CargaMasiva, false, false, 0);

            Button Btn_MostrarLista = new Button("Mostrar Lista");
            Btn_MostrarLista.Clicked += GoMostrarLista;
            vbox.PackStart(Btn_MostrarLista, false, false, 0);

            Add(vbox);
        }

        private void GoCargaMasiva(object sender, EventArgs e)
        {
            CargaMasiva cargaMasiva = new CargaMasiva(_listaUsuarios);
            cargaMasiva.ShowAll();
        }

        private void GoMostrarLista(object sender, EventArgs e)
        {
            Console.WriteLine("\n=== Lista de Usuarios ===");
            _listaUsuarios.Mostrar();
            Console.WriteLine("=======================\n");
        }
    }
}