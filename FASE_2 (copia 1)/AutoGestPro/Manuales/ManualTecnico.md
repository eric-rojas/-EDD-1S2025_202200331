# Manual Técnico - AutoGestPro

## Introducción
Este documento describe la arquitectura, implementación y funcionamiento interno de AutoGestPro.

## Arquitectura del Proyecto
```

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

```

## Implementación de Estructuras de Datos

### Lista Doble Enlazada (Vehículos)
```csharp
    
    public unsafe class ListaVehiculos
    {
        private NodoVehiculo* head; 
        private NodoVehiculo* tail; 

        public ListaVehiculos()
        {
            head = null; 
            tail = null;
        }

        public void Insertar(int id, int idUsuario, string marca, string modelo, string placa) 
        {
            NodoVehiculo* nuevoNodo = (NodoVehiculo*)NativeMemory.Alloc((nuint)sizeof(NodoVehiculo)); 

            *nuevoNodo = new NodoVehiculo(id, idUsuario, marca, modelo, placa); 

            if (head == null)
            {
                head = tail = nuevoNodo; 
            }
            else 
            {
                tail->Next = nuevoNodo; 
                nuevoNodo->Prev = tail; 
                tail = nuevoNodo; 

            }
        }
    }
```

### Carga Masiva desde JSON
```csharp

private void CargarUsuarios(string filePath)
        {
            try
            {
                Console.WriteLine("\n=== Iniciando carga de USUARIOS ===");
                string json = File.ReadAllText(filePath);
                
                var usuarios = JsonConvert.DeserializeObject<List<Usuario>>(json);
                ValidarJSONUsuarios(usuarios);

                if (usuarios != null && usuarios.Count > 0)
                {
                    Console.WriteLine($"✓ JSON deserializado exitosamente - {usuarios.Count} usuarios encontrados");
                    foreach (var usuario in usuarios)
                    {
                        _listaUsuarios.Insertar(usuario);
                        Console.WriteLine($"→ {usuario}");
                    }
                    MostrarMensajeExito($"Se cargaron {usuarios.Count} usuarios correctamente");
                }
            }
            catch (Exception e)
            {
                MostrarError($"Error al cargar usuarios: {e.Message}");
            }
        }
```

### Generación de Reportes con Graphviz
```csharp
public string GenerarGraphviz()
        {
            if (head == null) return "digraph G {\nrankdir=LR;\nnode [shape=record];\n}";

            StringBuilder dot = new StringBuilder();
            dot.AppendLine("digraph G {");
            dot.AppendLine("rankdir=LR;");
            dot.AppendLine("node [shape=record];");

            NodoRepuesto* temp = head;
            do
            {
                dot.AppendLine($"node{temp->ID} [label=\"{{ID: {temp->ID} | Repuesto: {new string(temp->Repuesto, 0, 50).TrimEnd('\0')} | Detalles: {new string(temp->Detalles, 0, 100).TrimEnd('\0')} | Costo: {temp->Costo:C}}}\"];");

                temp = temp->Next;
            } while (temp != head);

            temp = head;
            do
            {
                dot.AppendLine($"node{temp->ID} -> node{temp->Next->ID};");
                temp = temp->Next;
            } while (temp != head);

            dot.AppendLine("}");
            return dot.ToString();
        }
```

## Gestión de Servicios y Facturación

### Creación de Servicio
```csharp
public class ArbolBinarioServicios
    {
        private NodoServicio raiz;
        private ArbolAVLRepuestos repuestos; // Referencia al árbol de repuestos
        private ListaVehiculos vehiculos;   // Referencia a la lista de vehículos

        public ArbolBinarioServicios(ArbolAVLRepuestos repuestos, ListaVehiculos vehiculos)
        {
            raiz = null;
            this.repuestos = repuestos;
            this.vehiculos = vehiculos;
        }

        public ArbolBinarioServicios()
        {
            raiz = null;
            this.repuestos = null;
            this.vehiculos = null;
        }

        public bool ExisteID(int id)
        {
            return BuscarPorID(id) != null;
        }

        // Insertar un servicio con validaciones
        public bool Insertar(Servicio servicio)
        {
            // Validar que el ID sea único
            if (ExisteID(servicio.ID))
            {
                Console.WriteLine($"Error: Ya existe un servicio con el ID {servicio.ID}.");
                return false;
            }

            // Validar que el repuesto exista
            if (repuestos != null && !repuestos.ExisteID(servicio.ID_Repuesto))
            {
                Console.WriteLine($"Error: El repuesto con ID {servicio.ID_Repuesto} no existe en el sistema.");
                return false;
            }

            // Validar que el vehículo exista
            if (vehiculos != null && !vehiculos.ExisteID(servicio.ID_Vehiculo))
            {
                Console.WriteLine($"Error: El vehículo con ID {servicio.ID_Vehiculo} no existe en el sistema.");
                return false;
            }

            // Si pasa todas las validaciones, insertar el servicio
            raiz = InsertarRecursivo(raiz, servicio);
            return true;
        }
```

### Generación de Factura
```csharp
public ArbolBFacturas()
    {
        raiz = new NodoArbolB { EsHoja = true };
    }

    public void Insertar(Factura factura)
    {
        if (ExisteID(factura.ID))
        {
            Console.WriteLine($"Error: Ya existe una factura con el ID {factura.ID}.");
            return;
        }

        if (raiz.Facturas.Count == (2 * ORDEN) - 1)
        {
            NodoArbolB nuevoNodo = new NodoArbolB { EsHoja = false };
            nuevoNodo.Hijos.Add(raiz);
            DividirHijo(nuevoNodo, 0, raiz);
            raiz = nuevoNodo;
        }
        InsertarNoLleno(raiz, factura);
    }

    public bool Insertar(int idServicio, double total, int idUsuario)
    {
        Factura factura = new Factura(GenerarNuevoID(), idServicio, total, idUsuario);
        Insertar(factura);
        return true;
    }

    public int GenerarNuevoID()
    {
        return contadorID++;
    }
```

