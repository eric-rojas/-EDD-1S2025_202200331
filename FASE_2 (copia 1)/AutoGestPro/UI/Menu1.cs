
/*using Gtk;
using System;
using AutoGestPro.Core;
using AutoGestPro.Utils;
using AutoGestPro.UI;

namespace AutoGestPro
{
    public class Menu1 : Window
    {
        private readonly ListaUsuarios _listaUsuarios;
        private readonly ListaVehiculos _listaVehiculos;
        private readonly GeneradorServicio _generadorServicio;
        private readonly ArbolAVLRepuestos _arbolAVLRepuestos;
        private readonly ArbolBFacturas _arbolBFacturas;
        private readonly ArbolBinarioServicios _arbolBinarioServicios;
        private readonly LogueoUsuarios _logueoUsuarios;

        
        public Menu1(ListaUsuarios listaUsuarios, ListaVehiculos listaVehiculos, GeneradorServicio generadorServicio, ArbolAVLRepuestos arbolAVLRepuestos, ArbolBinarioServicios arbolBinarioServicios, ArbolBFacturas arbolBFacturas, LogueoUsuarios logueoUsuarios) : base("AutoGestPro - Menú Root")
        {
            _listaUsuarios = listaUsuarios;
            _listaVehiculos = listaVehiculos;
            _generadorServicio = generadorServicio;
            _arbolAVLRepuestos = arbolAVLRepuestos;
            _arbolBFacturas = arbolBFacturas;
            _arbolBinarioServicios = arbolBinarioServicios;
            _logueoUsuarios = logueoUsuarios; // Aquí es donde se debe hacer la corrección




            SetDefaultSize(400, 300);
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

            Button Btn_IngresoManual = new Button("Gestion de Usuairos y Vehiculos");
            Btn_IngresoManual.Clicked += GoIngresoManual;
            vbox.PackStart(Btn_IngresoManual, false, false, 0);

            Button Btn_IngresoManual2 = new Button("Gestión de Repuestos");
            Btn_IngresoManual2.Clicked += GoIngresoManual2;
            vbox.PackStart(Btn_IngresoManual2, false, false, 0);

            Button Btn_RepuestosView = new Button("Visualizar Repuestos");
            Btn_RepuestosView.Clicked += GoRepuestosView;
            vbox.PackStart(Btn_RepuestosView, false, false, 0);

            Button Btn_ServiciosView = new Button("Generar Servicios");
            Btn_ServiciosView.Clicked += GoServiciosView;
            vbox.PackStart(Btn_ServiciosView, false, false, 0);

            Button Btn_GenerarReportes = new Button("Generar Reportes");
            Btn_GenerarReportes.Clicked += GoReportes;
            vbox.PackStart(Btn_GenerarReportes, false, false, 0);

            Button Btn_Edicion = new Button("Edición Avanzada");
            Btn_Edicion.Clicked += GoEdicionView;
            vbox.PackStart(Btn_Edicion, false, false, 0);

           


            

            Add(vbox);
            ShowAll();
        }

        private void GoCargaMasiva(object? sender, EventArgs e)
        {
            CargaMasiva cargaMasiva = new CargaMasiva(_listaUsuarios, _listaVehiculos, _arbolAVLRepuestos);
            cargaMasiva.ShowAll();
        }

        private void GoMostrarLista(object? sender, EventArgs e)
        {
            Console.WriteLine("\n=== Lista de Usuarios ===");
            _listaUsuarios.Mostrar();
            Console.WriteLine("=======================\n");

            Console.WriteLine("\n=== Lista de Vehículos ===");
            _listaVehiculos.Mostrar();
            Console.WriteLine("=======================\n");

            Console.WriteLine("\n=== Lista de Repuestos ===");
            _arbolAVLRepuestos.Mostrar();
            Console.WriteLine("=======================\n");

            Console.WriteLine("\n=== Lista de Facturas ===");
            _arbolBFacturas.Mostrar();
            Console.WriteLine("=======================\n");

            Console.WriteLine("\n=== Lista de Servicios");
            _arbolBinarioServicios.Mostrar();
            Console.WriteLine("=======================\n");
        }

        private void GoIngresoManual2(object? sender, EventArgs e)
        {
            Ingreso2 ingreso2 = new Ingreso2(_arbolAVLRepuestos);
            ingreso2.ShowAll();
        }

        private void GoRepuestosView(object? sender, EventArgs e)
        {
            RepuestosView repuestosView = new RepuestosView(_arbolAVLRepuestos);
            repuestosView.ShowAll();
        }

        private void GoServiciosView(object? sender, EventArgs e)
        {
            ServiciosView serviciosView = new ServiciosView(_arbolAVLRepuestos, _listaVehiculos, _arbolBinarioServicios, _arbolBFacturas);

            serviciosView.ShowAll();
        }

      
   

        private void GoIngresoManual(object? sender, EventArgs e)
        {
            Ingreso ingreso = new Ingreso(_listaUsuarios, _listaVehiculos);
            ingreso.ShowAll();
        }



        private void GoRepuestos(object? sender, EventArgs e)
        {
            RepuestosView repuestosView = new RepuestosView(_arbolAVLRepuestos);
            repuestosView.ShowAll();
        }

        private void GoEdicionView(object? sender, EventArgs e)
        {
            EdicionView edicion = new EdicionView(_listaUsuarios, _listaVehiculos);
            edicion.ShowAll();
        }


        

        private void GoReportes(object? sender, EventArgs e)
        {
            // Generar JSON de logueo
            string rutaReportes = "./AutoGestPro/Reports";
            //_logueoUsuarios.CargarRegistros();
            _logueoUsuarios.GenerarJson(rutaReportes);

            // Usuarios y Vehículos
            GraphvizExporter.GenerarArchivoDot("usuarios.dot", _listaUsuarios.GenerarGraphviz());
            GraphvizExporter.GenerarArchivoDot("vehiculos.dot", _listaVehiculos.GenerarGraphviz());
            
            // Árboles
            GraphvizExporter.GenerarArchivoDot("repuestos.dot", _arbolAVLRepuestos.GenerarGraphviz());
            GraphvizExporter.GenerarArchivoDot("servicios.dot", _arbolBinarioServicios.GenerarGraphviz());
            GraphvizExporter.GenerarArchivoDot("facturas.dot", _arbolBFacturas.GenerarGraphviz());

            // Conversión a PNG
            GraphvizExporter.ConvertirDotAPng("usuarios.dot");
            GraphvizExporter.ConvertirDotAPng("vehiculos.dot");
            GraphvizExporter.ConvertirDotAPng("repuestos.dot");
            GraphvizExporter.ConvertirDotAPng("servicios.dot");
            GraphvizExporter.ConvertirDotAPng("facturas.dot");
            
            MessageDialog info = new MessageDialog(
                this, 
                DialogFlags.Modal,
                MessageType.Info, 
                ButtonsType.Ok, 
                "Reportes generados exitosamente!");
            info.Run();
            info.Destroy();
        }  

        

                          
    }
}*/


using Gtk;
using System;
using System.Collections.Generic;
using AutoGestPro.Core;
using AutoGestPro.Utils;
using AutoGestPro.UI;

namespace AutoGestPro
{
    public class Menu1 : Window
    {
        private readonly ListaUsuarios _listaUsuarios;
        private readonly ListaVehiculos _listaVehiculos;
        private readonly GeneradorServicio _generadorServicio;
        private readonly ArbolAVLRepuestos _arbolAVLRepuestos;
        private readonly ArbolBFacturas _arbolBFacturas;
        private readonly ArbolBinarioServicios _arbolBinarioServicios;
        private readonly LogueoUsuarios _logueoUsuarios;
        
        // Lista para mantener referencias a las ventanas abiertas
        private List<Window> _ventanasAbiertas = new List<Window>();
        
        // Referencias a los botones
        private Dictionary<string, Button> _botones = new Dictionary<string, Button>();

        public Menu1(ListaUsuarios listaUsuarios, ListaVehiculos listaVehiculos, GeneradorServicio generadorServicio, 
                    ArbolAVLRepuestos arbolAVLRepuestos, ArbolBinarioServicios arbolBinarioServicios, 
                    ArbolBFacturas arbolBFacturas, LogueoUsuarios logueoUsuarios) : base("AutoGestPro - Menú Root")
        {
            try
            {
                _listaUsuarios = listaUsuarios;
                _listaVehiculos = listaVehiculos;
                _generadorServicio = generadorServicio;
                _arbolAVLRepuestos = arbolAVLRepuestos;
                _arbolBFacturas = arbolBFacturas;
                _arbolBinarioServicios = arbolBinarioServicios;
                _logueoUsuarios = logueoUsuarios;

                SetDefaultSize(400, 300);
                SetPosition(WindowPosition.Center);
                
                // Manejar el evento de cierre
                DeleteEvent += (o, args) => {
                    try
                    {
                        // Cerrar todas las ventanas secundarias
                        foreach (var ventana in _ventanasAbiertas)
                        {
                            try
                            {
                                if (ventana != null && ventana.Handle != IntPtr.Zero)
                                    ventana.Destroy();
                            }
                            catch (Exception ex)
                            {
                                ErrorHandler.LogError("Menu1", "CerrarVentanaSecundaria", ex);
                            }
                        }
                        
                        _ventanasAbiertas.Clear();
                        Destroy();
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.LogError("Menu1", "DeleteEvent", ex);
                    }
                };

                CrearInterfaz();
                
                ShowAll();
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("Menu1", "Constructor", ex);
                ErrorHandler.MostrarError(this, "Error al inicializar el menú principal: " + ex.Message);
            }
        }
        
        private void CrearInterfaz()
        {
            try
            {
                VBox vbox = new VBox();
                vbox.Spacing = 5;

                Label label = new Label("Menú Principal");
                vbox.PackStart(label, false, false, 0);

                AgregarBoton(vbox, "Carga Masiva", GoCargaMasiva);
                AgregarBoton(vbox, "Mostrar Lista", GoMostrarLista);
                //AgregarBoton(vbox, "Gestion de Usuarios y Vehiculos", GoIngresoManual);
                AgregarBoton(vbox, "Gestión de Repuestos", GoIngresoManual2);
                AgregarBoton(vbox, "Visualizar Repuestos", GoRepuestosView);
                AgregarBoton(vbox, "Generar Servicios", GoServiciosView);
                AgregarBoton(vbox, "Generar Reportes", GoReportes);
                AgregarBoton(vbox, "Edición Avanzada", GoEdicionView);

                Add(vbox);
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("Menu1", "CrearInterfaz", ex);
                throw; // La excepción será capturada en el constructor
            }
        }
        
        private void AgregarBoton(VBox vbox, string texto, EventHandler handler)
        {
            try
            {
                Button btn = new Button(texto);
                btn.Clicked += handler;
                vbox.PackStart(btn, false, false, 0);
                _botones[texto] = btn;
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("Menu1", "AgregarBoton", ex);
            }
        }

        private void GoCargaMasiva(object sender, EventArgs e)
        {
            try
            {
                CargaMasiva cargaMasiva = new CargaMasiva(_listaUsuarios, _listaVehiculos, _arbolAVLRepuestos);
                
                // Registrar la ventana
                _ventanasAbiertas.Add(cargaMasiva);
                
                // Configurar evento para eliminar de la lista al cerrarse
                cargaMasiva.DeleteEvent += (o, args) => {
                    try
                    {
                        _ventanasAbiertas.Remove(cargaMasiva);
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.LogError("Menu1", "CargaMasivaDeleteEvent", ex);
                    }
                };
                
                cargaMasiva.ShowAll();
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("Menu1", "GoCargaMasiva", ex);
                ErrorHandler.MostrarError(this, "Error al abrir la ventana de carga masiva: " + ex.Message);
            }
        }

        private void GoMostrarLista(object sender, EventArgs e)
        {
            try
            {
                Console.WriteLine("\n=== Lista de Usuarios ===");
                _listaUsuarios.Mostrar();
                Console.WriteLine("=======================\n");

                Console.WriteLine("\n=== Lista de Vehículos ===");
                _listaVehiculos.Mostrar();
                Console.WriteLine("=======================\n");

                Console.WriteLine("\n=== Lista de Repuestos ===");
                _arbolAVLRepuestos.Mostrar();
                Console.WriteLine("=======================\n");

                Console.WriteLine("\n=== Lista de Facturas ===");
                _arbolBFacturas.Mostrar();
                Console.WriteLine("=======================\n");

                Console.WriteLine("\n=== Lista de Servicios");
                _arbolBinarioServicios.Mostrar();
                Console.WriteLine("=======================\n");
                
                ErrorHandler.MostrarInfo(this, "Datos mostrados en la consola.");
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("Menu1", "GoMostrarLista", ex);
                ErrorHandler.MostrarError(this, "Error al mostrar listas: " + ex.Message);
            }
        }

        private void GoIngresoManual2(object sender, EventArgs e)
        {
            try
            {
                Ingreso2 ingreso2 = new Ingreso2(_arbolAVLRepuestos);
                
                // Registrar la ventana
                _ventanasAbiertas.Add(ingreso2);
                
                // Configurar evento para eliminar de la lista al cerrarse
                ingreso2.DeleteEvent += (o, args) => {
                    try
                    {
                        _ventanasAbiertas.Remove(ingreso2);
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.LogError("Menu1", "Ingreso2DeleteEvent", ex);
                    }
                };
                
                ingreso2.ShowAll();
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("Menu1", "GoIngresoManual2", ex);
                ErrorHandler.MostrarError(this, "Error al abrir la gestión de repuestos: " + ex.Message);
            }
        }

        private void GoRepuestosView(object sender, EventArgs e)
        {
            try
            {
                RepuestosView repuestosView = new RepuestosView(_arbolAVLRepuestos);
                
                // Registrar la ventana
                _ventanasAbiertas.Add(repuestosView);
                
                // Configurar evento para eliminar de la lista al cerrarse
                repuestosView.DeleteEvent += (o, args) => {
                    try
                    {
                        _ventanasAbiertas.Remove(repuestosView);
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.LogError("Menu1", "RepuestosViewDeleteEvent", ex);
                    }
                };
                
                repuestosView.ShowAll();
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("Menu1", "GoRepuestosView", ex);
                ErrorHandler.MostrarError(this, "Error al abrir la visualización de repuestos: " + ex.Message);
            }
        }

        private void GoServiciosView(object sender, EventArgs e)
        {
            try
            {
                ServiciosView serviciosView = new ServiciosView(
                    _arbolAVLRepuestos, 
                    _listaVehiculos, 
                    _arbolBinarioServicios, 
                    _arbolBFacturas);
                
                // Registrar la ventana
                _ventanasAbiertas.Add(serviciosView);
                
                // Configurar evento para eliminar de la lista al cerrarse
                serviciosView.DeleteEvent += (o, args) => {
                    try
                    {
                        _ventanasAbiertas.Remove(serviciosView);
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.LogError("Menu1", "ServiciosViewDeleteEvent", ex);
                    }
                };
                
                serviciosView.ShowAll();
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("Menu1", "GoServiciosView", ex);
                ErrorHandler.MostrarError(this, "Error al abrir la generación de servicios: " + ex.Message);
            }
        }

        /*private void GoIngresoManual(object sender, EventArgs e)
        {
            try
            {
                Ingreso ingreso = new Ingreso(_listaUsuarios, _listaVehiculos);
                
                // Registrar la ventana
                _ventanasAbiertas.Add(ingreso);
                
                // Configurar evento para eliminar de la lista al cerrarse
                ingreso.DeleteEvent += (o, args) => {
                    try
                    {
                        _ventanasAbiertas.Remove(ingreso);
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.LogError("Menu1", "IngresoDeleteEvent", ex);
                    }
                };
                
                ingreso.ShowAll();
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("Menu1", "GoIngresoManual", ex);
                ErrorHandler.MostrarError(this, "Error al abrir la gestión de usuarios y vehículos: " + ex.Message);
            }
        }*/

        private void GoEdicionView(object sender, EventArgs e)
        {
            try
            {
                EdicionView edicion = new EdicionView(_listaUsuarios, _listaVehiculos);
                
                // Registrar la ventana
                _ventanasAbiertas.Add(edicion);
                
                // Configurar evento para eliminar de la lista al cerrarse
                edicion.DeleteEvent += (o, args) => {
                    try
                    {
                        _ventanasAbiertas.Remove(edicion);
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.LogError("Menu1", "EdicionViewDeleteEvent", ex);
                    }
                };
                
                edicion.ShowAll();
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("Menu1", "GoEdicionView", ex);
                ErrorHandler.MostrarError(this, "Error al abrir la edición avanzada: " + ex.Message);
            }
        }

        private void GoReportes(object sender, EventArgs e)
        {
            try
            {
                // Deshabilitar el botón durante la generación
                _botones["Generar Reportes"].Sensitive = false;
                
                // Generar JSON de logueo
                string rutaReportes = "./AutoGestPro/Reports";
                _logueoUsuarios.GenerarJson(rutaReportes);

                // Usuarios y Vehículos
                GraphvizExporter.GenerarArchivoDot("usuarios.dot", _listaUsuarios.GenerarGraphviz());
                GraphvizExporter.GenerarArchivoDot("vehiculos.dot", _listaVehiculos.GenerarGraphviz());
                
                // Árboles
                GraphvizExporter.GenerarArchivoDot("repuestos.dot", _arbolAVLRepuestos.GenerarGraphviz());
                GraphvizExporter.GenerarArchivoDot("servicios.dot", _arbolBinarioServicios.GenerarGraphviz());
                GraphvizExporter.GenerarArchivoDot("facturas.dot", _arbolBFacturas.GenerarGraphviz());

                // Conversión a PNG
                try {
                    GraphvizExporter.ConvertirDotAPng("usuarios.dot");
                    GraphvizExporter.ConvertirDotAPng("vehiculos.dot");
                    GraphvizExporter.ConvertirDotAPng("repuestos.dot");
                    GraphvizExporter.ConvertirDotAPng("servicios.dot");
                    GraphvizExporter.ConvertirDotAPng("facturas.dot");
                } catch (Exception ex) {
                    ErrorHandler.LogError("Menu1", "ConvertirDotAPng", ex);
                    ErrorHandler.MostrarError(this, "Error al convertir archivos DOT a PNG: " + ex.Message);
                }
                
                // Re-habilitar el botón
                _botones["Generar Reportes"].Sensitive = true;
                
                ErrorHandler.MostrarInfo(this, "Reportes generados exitosamente!");
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("Menu1", "GoReportes", ex);
                ErrorHandler.MostrarError(this, "Error al generar reportes: " + ex.Message);
                
                // Asegurar que el botón se re-habilita incluso si hay error
                _botones["Generar Reportes"].Sensitive = true;
            }
        }

        protected override void OnDestroyed()
        {
            try
            {
                // Desconectar todos los eventos de botones
                foreach (var kvp in _botones)
                {
                    Button btn = kvp.Value;
                    string texto = kvp.Key;
                    
                    try
                    {
                        switch (texto)
                        {
                            case "Carga Masiva":
                                btn.Clicked -= GoCargaMasiva;
                                break;
                            case "Mostrar Lista":
                                btn.Clicked -= GoMostrarLista;
                                break;
                            /*case "Gestion de Usuarios y Vehiculos":
                                btn.Clicked -= GoIngresoManual;
                                break;*/
                            case "Gestión de Repuestos":
                                btn.Clicked -= GoIngresoManual2;
                                break;
                            case "Visualizar Repuestos":
                                btn.Clicked -= GoRepuestosView;
                                break;
                            case "Generar Servicios":
                                btn.Clicked -= GoServiciosView;
                                break;
                            case "Generar Reportes":
                                btn.Clicked -= GoReportes;
                                break;
                            case "Edición Avanzada":
                                btn.Clicked -= GoEdicionView;
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.LogError("Menu1", "DesconectarEvento_" + texto, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("Menu1", "OnDestroyed", ex);
            }
            finally
            {
                base.OnDestroyed();
            }
        }
    }
}