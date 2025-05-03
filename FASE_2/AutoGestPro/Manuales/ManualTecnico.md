## Manual Técnico

### Introducción
Este documento describe la arquitectura, implementación y funcionamiento interno de AutoGestPro, un sistema de gestión para talleres mecánicos que utiliza estructuras avanzadas de datos para optimizar la administración de clientes, vehículos, repuestos, servicios y facturación.

### Arquitectura del Proyecto

```
AutoGestPro/
├── Core/
│   ├── ArbolAVLRepuestos.cs
│   ├── ArbolBFacturas.cs
│   ├── ArbolBinarioServicios.cs
│   ├── CargaMasiva.cs
│   ├── GeneradorServicio.cs
│   ├── ListaUsuarios.cs
│   ├── ListaVehiculos.cs
├── UI/
│   ├── Inicio.cs
│   ├── Menu1.cs
│   ├── Ingreso.cs
│   ├── Ingreso2.cs
│   ├── RepuestosView.cs
│   ├── ServiciosView.cs
│   ├── EdicionView.cs
│   ├── Menu2.cs
│   ├── Menu2InsertarVehiculo.cs
│   ├── Menu2ServiciosView.cs
│   ├── Menu2FacturasView.cs
│   ├── Menu2CancelarFacturas.cs
├── Utils/
│   └── Program.cs
```

### Implementación de Estructuras de Datos

#### Lista Doble Enlazada - Vehículos

```csharp
public unsafe class ListaVehiculos {
    private NodoVehiculo* head;
    private NodoVehiculo* tail;

    public ListaVehiculos() {
        head = null;
        tail = null;
    }

    public void Insertar(int id, int idUsuario, string marca, string modelo, string placa) {
        NodoVehiculo* nuevoNodo = (NodoVehiculo*)NativeMemory.Alloc((nuint)sizeof(NodoVehiculo));
        *nuevoNodo = new NodoVehiculo(id, idUsuario, marca, modelo, placa);

        if (head == null) {
            head = tail = nuevoNodo;
        } else {
            tail->Next = nuevoNodo;
            nuevoNodo->Prev = tail;
            tail = nuevoNodo;
        }
    }
}
```

#### Carga Masiva desde JSON

```csharp
private void CargarUsuarios(string filePath) {
    try {
        Console.WriteLine("\n=== Iniciando carga de USUARIOS ===");
        string json = File.ReadAllText(filePath);
        var usuarios = JsonConvert.DeserializeObject<List<Usuario>>(json);
        ValidarJSONUsuarios(usuarios);

        if (usuarios != null && usuarios.Count > 0) {
            Console.WriteLine($"✓ JSON deserializado exitosamente - {usuarios.Count} usuarios encontrados");
            foreach (var usuario in usuarios) {
                _listaUsuarios.Insertar(usuario);
                Console.WriteLine($"→ {usuario}");
            }
            MostrarMensajeExito($"Se cargaron {usuarios.Count} usuarios correctamente");
        }
    } catch (Exception e) {
        MostrarError($"Error al cargar usuarios: {e.Message}");
    }
}
```

#### Generación de Reportes - Graphviz

```csharp
public string GenerarGraphviz() {
    if (head == null) return "digraph G {\nrankdir=LR;\nnode [shape=record];\n}";

    StringBuilder dot = new StringBuilder();
    dot.AppendLine("digraph G {");
    dot.AppendLine("rankdir=LR;");
    dot.AppendLine("node [shape=record];");

    NodoRepuesto* temp = head;
    do {
        dot.AppendLine($"node{temp->ID} [label=\"{{ID: {temp->ID} | Repuesto: {new string(temp->Repuesto, 0, 50).TrimEnd('\0')} | Detalles: {new string(temp->Detalles, 0, 100).TrimEnd('\0')} | Costo: {temp->Costo:C}}}\"];");
        temp = temp->Next;
    } while (temp != head);

    temp = head;
    do {
        dot.AppendLine($"node{temp->ID} -> node{temp->Next->ID};");
        temp = temp->Next;
    } while (temp != head);

    dot.AppendLine("}");
    return dot.ToString();
}
```

### Gestión de Servicios y Facturación

#### Árbol Binario - Servicios

```csharp
public class ArbolBinarioServicios {
    private NodoServicio raiz;
    private ArbolAVLRepuestos repuestos;
    private ListaVehiculos vehiculos;

    public ArbolBinarioServicios(ArbolAVLRepuestos repuestos, ListaVehiculos vehiculos) {
        raiz = null;
        this.repuestos = repuestos;
        this.vehiculos = vehiculos;
    }

    public bool Insertar(Servicio servicio) {
        if (ExisteID(servicio.ID)) {
            Console.WriteLine($"Error: Ya existe un servicio con el ID {servicio.ID}.");
            return false;
        }

        if (repuestos != null && !repuestos.ExisteID(servicio.ID_Repuesto)) {
            Console.WriteLine($"Error: El repuesto con ID {servicio.ID_Repuesto} no existe.");
            return false;
        }

        if (vehiculos != null && !vehiculos.ExisteID(servicio.ID_Vehiculo)) {
            Console.WriteLine($"Error: El vehículo con ID {servicio.ID_Vehiculo} no existe.");
            return false;
        }

        raiz = InsertarRecursivo(raiz, servicio);
        return true;
    }
}
```

#### Árbol B - Facturación

```csharp
public ArbolBFacturas() {
    raiz = new NodoArbolB { EsHoja = true };
}

public void Insertar(Factura factura) {
    if (ExisteID(factura.ID)) {
        Console.WriteLine($"Error: Ya existe una factura con el ID {factura.ID}.");
        return;
    }

    if (raiz.Facturas.Count == (2 * ORDEN) - 1) {
        NodoArbolB nuevoNodo = new NodoArbolB { EsHoja = false };
        nuevoNodo.Hijos.Add(raiz);
        DividirHijo(nuevoNodo, 0, raiz);
        raiz = nuevoNodo;
    }

    InsertarNoLleno(raiz, factura);
}

public int GenerarNuevoID() {
    return contadorID++;
}
```