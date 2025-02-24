# Manual Técnico - AutoGestPro

## Introducción
Este documento describe la arquitectura, implementación y funcionamiento interno de AutoGestPro.

## Arquitectura del Proyecto
```
📂 AutoGestPro
├── 📂 Core (Lógica del negocio)
│   ├── ListaUsuarios.cs (Lista simplemente enlazada)
│   ├── ListaVehiculos.cs (Lista doblemente enlazada)
│   ├── GeneradorServicio.cs 
│   ├── ListaRepuestos.cs (Lista circular)
│   ├── ColaServicios.cs (Cola para servicios)
│   ├── PilaFacturas.cs (Pila para facturas)
│   ├── MatrizBitacora.cs (Matriz dispersa)
│   ├── CargaMasiva.cs (Manejo de JSON)
├── 📂 UI (Interfaz gráfica con GTK)
│   ├── Inicio.cs (Ventana principal o login)
│   ├── Menu1.cs (Menú principal)
│   ├── Ingreso.cs (ingreso Manual)
│   ├── UsuariosView.cs (Gestión de usuarios)
│   ├── ServiciosView.cs (Generar servicio y facturas)
│   ├── ReportesView.cs (Visualización con Graphviz)
│   ├── CancelarFActuraView.cs 
├── 📂 Utils (Utilidades generales)
│   ├── GraphvizExporter.cs (Generación de reportes visuales)
│   ├── Usuarios.json
│   ├── Vehiculos.json
│   ├── Repuestos.json
├── 📂 Reports (Reportes Generados)
├── Program.cs (Punto de entrada del sistema)
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
public bool GenerarNuevoServicio(int idVehiculo, int idRepuesto, string detalles, float costoServicio)
        {
            try
            {
                
                var vehiculo = _vehiculos.Buscar(idVehiculo);
                if (vehiculo == null)
                {
                    throw new ServicioException($"El vehículo con ID {idVehiculo} no existe.");
                }

                var repuesto = _repuestos.Buscar(idRepuesto);
                if (repuesto == null)
                {
                    throw new ServicioException($"El repuesto con ID {idRepuesto} no existe.");
                }

                
                if (string.IsNullOrEmpty(detalles))
                {
                    throw new ServicioException("Los detalles del servicio son requeridos.");
                }

                if (costoServicio <= 0)
                {
                    throw new ServicioException("El costo del servicio debe ser mayor a 0.");
                }

                
                float costoTotal = (float)(costoServicio + (float)repuesto.Costo);

               
                _servicios.Encolar(
                    _contadorIDServicio,
                    idRepuesto,
                    idVehiculo,
                    detalles,
                    costoServicio
                );

                
                GenerarFactura(_contadorIDServicio, costoTotal);

                
                ActualizarBitacora(idRepuesto, idVehiculo, detalles);

                
                _contadorIDServicio++;

                return true;
            }
            catch (ServicioException ex)
            {
                Console.WriteLine($"Error al generar servicio: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado: {ex.Message}");
                return false;
            }
        }
```

### Generación de Factura
```csharp
private void GenerarFactura(int idServicio, float costoTotal)
        {
            try
            {
                _facturas.Apilar(idServicio, costoTotal);
            }
            catch (Exception ex)
            {
                throw new ServicioException($"Error al generar la factura: {ex.Message}");
            }
        }
```

