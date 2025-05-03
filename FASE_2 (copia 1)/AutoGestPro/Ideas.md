
--|Core
----|ArbolAVLRepuestos.cs
----|ArbolBFacturas.cs
----|ArbolBinarioServicios.cs
----|CargaMasiva.cs
----|GeneradorServicio.cs
----|ListaUsuarios.cs
----|ListaVehiculos.cs
--|UI
----|Inicio.cs (aqui es el logueo, entra al menu1 o menu2 dependiendo del usuario root o usuario normal)
----|Menu1.cs (entran usuarios root)
----|Ingreso.cs (se abre en Menu1.cs)
----|Ingreso2.cs (se abre en Menu1.cs)
----|RepuestosView.cs (se abre en Menu1.cs)
----|ServiciosView.cs (se abre en Menu1.cs)
----|EdicionView.cs (se abre en Menu1.cs)
----|Menu2.cs (este menu es el que quiero empezar, es para los usuarios normales)
----|Menu2InsertarVehiculo.cs 
----|Menu2ServiciosView.cs 
----|Menu2FacturasView.cs 
----|Menu2CancelarFacturas.cs 
--|Utils
Program.cs


necesito hacer que todas las ui tengan un trycatch o verificar su limpio funicionamiento para que no se me cierren las ventanas y muera el proyecto, al momento tengo ese problema... el proyecto, al ser cargado con datos, eliminar datos, cargar otros datos y modificar pues se muere