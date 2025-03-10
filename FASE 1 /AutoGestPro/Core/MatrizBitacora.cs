using System;
using System.Runtime.InteropServices;
using System.Text;

namespace AutoGestPro.Core
{
    public unsafe struct NodoBitacora
    {
        public int IdRepuesto;    // Para la fila
        public int IdVehiculo;    // Para la columna
        public fixed char Detalles[100];
        public NodoBitacora* Arriba;
        public NodoBitacora* Abajo;
        public NodoBitacora* Derecha;
        public NodoBitacora* Izquierda;

        public NodoBitacora(int idRepuesto, int idVehiculo, string detalles)
        {
            IdRepuesto = idRepuesto;
            IdVehiculo = idVehiculo;
            fixed (char* d = Detalles)
            {
                detalles.AsSpan().CopyTo(new Span<char>(d, 100));
            }
            Arriba = null;
            Abajo = null;
            Derecha = null;
            Izquierda = null;
        }

        public override string ToString()
        {
            fixed (char* d = Detalles)
            {
                return new string(d).TrimEnd('\0');
            }
        }
    }

    public unsafe struct NodoEncabezado
    {
        public int Id;
        public NodoEncabezado* Siguiente;
        public NodoBitacora* Acceso;

        public NodoEncabezado(int id)
        {
            Id = id;
            Siguiente = null;
            Acceso = null;
        }
    }

    public unsafe class ListaEncabezado
    {
        public NodoEncabezado* Primero;
        private string tipo;

        public ListaEncabezado(string tipo)
        {
            Primero = null;
            this.tipo = tipo;
        }

        public void InsertarEncabezado(int id)
        {
            NodoEncabezado* nuevo = (NodoEncabezado*)Marshal.AllocHGlobal(sizeof(NodoEncabezado));
            *nuevo = new NodoEncabezado(id);

            if (Primero == null)
            {
                Primero = nuevo;
                return;
            }

            if (id < Primero->Id)
            {
                nuevo->Siguiente = Primero;
                Primero = nuevo;
                return;
            }

            NodoEncabezado* actual = Primero;
            NodoEncabezado* anterior = null;

            while (actual != null && actual->Id < id)
            {
                anterior = actual;
                actual = actual->Siguiente;
            }

            if (actual != null && actual->Id == id)
            {
                Marshal.FreeHGlobal((IntPtr)nuevo);
                return;
            }

            anterior->Siguiente = nuevo;
            nuevo->Siguiente = actual;
        }

        public NodoEncabezado* BuscarEncabezado(int id)
        {
            NodoEncabezado* actual = Primero;
            while (actual != null)
            {
                if (actual->Id == id) return actual;
                actual = actual->Siguiente;
            }
            return null;
        }
    }


    public unsafe class MatrizBitacora : IDisposable
{
    private ListaEncabezado* filas;
    private ListaEncabezado* columnas;
    private bool disposed;

    public MatrizBitacora()
    {
        filas = (ListaEncabezado*)Marshal.AllocHGlobal(sizeof(ListaEncabezado));
        columnas = (ListaEncabezado*)Marshal.AllocHGlobal(sizeof(ListaEncabezado));
        
        *filas = new ListaEncabezado("Repuesto");
        *columnas = new ListaEncabezado("Vehiculo");
        disposed = false;
    }

    // Método público para liberar recursos
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    // Método protegido para liberar recursos
    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                try
                {
                    // Liberar nodos de bitácora de las filas
                    if (filas != null && filas->Primero != null)
                    {
                        NodoEncabezado* filaActual = filas->Primero;
                        while (filaActual != null)
                        {
                            if (filaActual->Acceso != null)
                            {
                                LiberarNodosBitacora(filaActual->Acceso);
                            }
                            NodoEncabezado* tempFila = filaActual;
                            filaActual = filaActual->Siguiente;
                            Marshal.FreeHGlobal((IntPtr)tempFila);
                        }
                    }

                    // Liberar nodos de bitácora de las columnas y encabezados de columna
                    if (columnas != null && columnas->Primero != null)
                    {
                        NodoEncabezado* columnaActual = columnas->Primero;
                        while (columnaActual != null)
                        {
                            NodoEncabezado* tempCol = columnaActual;
                            columnaActual = columnaActual->Siguiente;
                            Marshal.FreeHGlobal((IntPtr)tempCol);
                        }
                    }

                    // Liberar las listas principales
                    if (filas != null)
                    {
                        Marshal.FreeHGlobal((IntPtr)filas);
                        filas = null;
                    }

                    if (columnas != null)
                    {
                        Marshal.FreeHGlobal((IntPtr)columnas);
                        columnas = null;
                    }
                }
                catch
                {
                    // Ignorar errores durante la liberación de memoria
                }
            }
            disposed = true;
        }
    }
        private void LiberarListaEncabezado(ListaEncabezado* lista)
        {
            if (lista == null) return;

            NodoEncabezado* actual = lista->Primero;
            while (actual != null)
            {
                NodoEncabezado* siguiente = actual->Siguiente;
                
                // Liberar nodos de la bitácora conectados a este encabezado
                if (actual->Acceso != null)
                {
                    LiberarNodosBitacora(actual->Acceso);
                }

                Marshal.FreeHGlobal((IntPtr)actual);
                actual = siguiente;
            }
        }

        private void LiberarNodosBitacora(NodoBitacora* nodo)
        {
            if (nodo == null) return;

            NodoBitacora* actual = nodo;
            while (actual != null)
            {
                NodoBitacora* siguiente = actual->Derecha;
                Marshal.FreeHGlobal((IntPtr)actual);
                actual = siguiente;
            }
        }

        
        public void InsertarRelacion(int idRepuesto, int idVehiculo, string detalles)
        {
            Console.WriteLine($"Insertando relación: R{idRepuesto} - V{idVehiculo}: {detalles}");

            // Crear nuevo nodo
            NodoBitacora* nuevo = (NodoBitacora*)Marshal.AllocHGlobal(sizeof(NodoBitacora));
            *nuevo = new NodoBitacora(idRepuesto, idVehiculo, detalles);

            // Obtener o crear encabezados
            filas->InsertarEncabezado(idRepuesto);
            columnas->InsertarEncabezado(idVehiculo);

            NodoEncabezado* encabezadoFila = filas->BuscarEncabezado(idRepuesto);
            NodoEncabezado* encabezadoColumna = columnas->BuscarEncabezado(idVehiculo);

            if (encabezadoFila == null || encabezadoColumna == null)
            {
                Console.WriteLine("Error: No se pudieron crear los encabezados");
                Marshal.FreeHGlobal((IntPtr)nuevo);
                return;
            }

            Console.WriteLine("Encabezados creados correctamente");

            // Insertar en fila
            if (encabezadoFila->Acceso == null)
            {
                encabezadoFila->Acceso = nuevo;
                Console.WriteLine("Nodo insertado como primer acceso de fila");
            }
            else
            {
                NodoBitacora* temp = encabezadoFila->Acceso;
                NodoBitacora* anterior = null;

                while (temp != null && temp->IdVehiculo < idVehiculo)
                {
                    anterior = temp;
                    temp = temp->Derecha;
                }

                if (anterior == null)
                {
                    nuevo->Derecha = encabezadoFila->Acceso;
                    encabezadoFila->Acceso = nuevo;
                }
                else
                {
                    anterior->Derecha = nuevo;
                    nuevo->Derecha = temp;
                }
            }

            // Insertar en columna
            if (encabezadoColumna->Acceso == null)
            {
                encabezadoColumna->Acceso = nuevo;
                Console.WriteLine("Nodo insertado como primer acceso de columna");
            }
            else
            {
                NodoBitacora* temp = encabezadoColumna->Acceso;
                NodoBitacora* anterior = null;

                while (temp != null && temp->IdRepuesto < idRepuesto)
                {
                    anterior = temp;
                    temp = temp->Abajo;
                }

                if (anterior == null)
                {
                    nuevo->Abajo = encabezadoColumna->Acceso;
                    encabezadoColumna->Acceso = nuevo;
                }
                else
                {
                    anterior->Abajo = nuevo;
                    nuevo->Abajo = temp;
                }
            }
            Console.WriteLine("Relación insertada correctamente");
        }

        public void MostrarBitacora()
        {
            Console.WriteLine("\n=== Bitácora de Servicios ===");
            
            // Mostrar encabezados de columnas
            Console.Write("\t");
            NodoEncabezado* columna = columnas->Primero;
            while (columna != null)
            {
                Console.Write($"V{columna->Id}\t");
                columna = columna->Siguiente;
            }
            Console.WriteLine();

            // Mostrar filas
            NodoEncabezado* fila = filas->Primero;
            while (fila != null)
            {
                Console.Write($"R{fila->Id}\t");
                
                columna = columnas->Primero;
                NodoBitacora* actual = fila->Acceso;

                while (columna != null)
                {
                    if (actual != null && actual->IdVehiculo == columna->Id)
                    {
                        Console.Write($"{actual->ToString()}\t");
                        actual = actual->Derecha;
                    }
                    else
                    {
                        Console.Write("-\t");
                    }
                    columna = columna->Siguiente;
                }
                Console.WriteLine();
                fila = fila->Siguiente;
            }
        }

        
        public unsafe string GenerarGraphviz()
        {
            try
            {
                StringBuilder dot = new StringBuilder();
                dot.AppendLine("digraph G {");
                dot.AppendLine("  rankdir=TB;");
                dot.AppendLine("  nodesep=0.5;");
                dot.AppendLine("  ranksep=0.5;");
                dot.AppendLine("  node [shape=box, fontsize=10, width=1.5, height=0.5, fixedsize=true];");

                // Verificar si la matriz está inicializada correctamente
                if (filas == null || columnas == null)
                {
                    dot.AppendLine("  matriz [label=\"Error: Matriz no inicializada\"];");
                    dot.AppendLine("}");
                    return dot.ToString();
                }

                // Nodo matriz principal
                dot.AppendLine("  matriz [label=\"MATRIZ\", style=filled, fillcolor=gray];");

                // Si no hay datos, mostrar matriz vacía
                if (filas->Primero == null || columnas->Primero == null)
                {
                    dot.AppendLine("  empty [label=\"Matriz vacía\"];");
                    dot.AppendLine("  matriz -> empty;");
                    dot.AppendLine("}");
                    return dot.ToString();
                }

                // Encabezados de columnas (vehículos)
                NodoEncabezado* columnaActual = columnas->Primero;
                if (columnaActual != null)
                {
                    while (columnaActual != null)
                    {
                        dot.AppendLine($"  columna{columnaActual->Id} [label=\"V{columnaActual->Id}\", style=filled, fillcolor=lightgreen];");
                        columnaActual = columnaActual->Siguiente;
                    }
                }

                // Encabezados de filas (repuestos)
                NodoEncabezado* filaActual = filas->Primero;
                if (filaActual != null)
                {
                    while (filaActual != null)
                    {
                        dot.AppendLine($"  fila{filaActual->Id} [label=\"R{filaActual->Id}\", style=filled, fillcolor=lightblue];");
                        filaActual = filaActual->Siguiente;
                    }
                }

                // Primer rango: MATRIZ y encabezados de columnas
                dot.AppendLine("  { rank=same; matriz;");
                columnaActual = columnas->Primero;
                while (columnaActual != null)
                {
                    dot.AppendLine($"    columna{columnaActual->Id};");
                    columnaActual = columnaActual->Siguiente;
                }
                dot.AppendLine("  }");

                // Conexiones básicas
                if (columnas->Primero != null)
                {
                    dot.AppendLine($"  matriz -> columna{columnas->Primero->Id} [dir=both];");
                }
                if (filas->Primero != null)
                {
                    dot.AppendLine($"  matriz -> fila{filas->Primero->Id} [dir=both];");
                }

                // Conectar encabezados de columnas
                columnaActual = columnas->Primero;
                while (columnaActual != null && columnaActual->Siguiente != null)
                {
                    dot.AppendLine($"  columna{columnaActual->Id} -> columna{columnaActual->Siguiente->Id} [dir=both];");
                    columnaActual = columnaActual->Siguiente;
                }

                // Conectar filas y nodos internos
                filaActual = filas->Primero;
                while (filaActual != null)
                {
                    if (filaActual->Siguiente != null)
                    {
                        dot.AppendLine($"  fila{filaActual->Id} -> fila{filaActual->Siguiente->Id} [dir=both];");
                    }

                    // Nodos internos
                    NodoBitacora* nodoActual = filaActual->Acceso;
                    if (nodoActual != null)
                    {
                        // Subgrafo para nodos de esta fila
                        dot.AppendLine($"  {{ rank=same; fila{filaActual->Id};");
                        while (nodoActual != null)
                        {
                            string detalle = new string(nodoActual->Detalles).TrimEnd('\0');
                            dot.AppendLine($"    nodo{nodoActual->IdRepuesto}_{nodoActual->IdVehiculo} [label=\"{detalle}\", style=filled, fillcolor=lightyellow];");
                            nodoActual = nodoActual->Derecha;
                        }
                        dot.AppendLine("  }");

                        // Conexiones de nodos
                        nodoActual = filaActual->Acceso;
                        while (nodoActual != null)
                        {
                            // Conexión con encabezado de fila
                            if (nodoActual == filaActual->Acceso)
                            {
                                dot.AppendLine($"  fila{filaActual->Id} -> nodo{nodoActual->IdRepuesto}_{nodoActual->IdVehiculo} [dir=both];");
                            }

                            // Conexiones horizontales
                            if (nodoActual->Derecha != null)
                            {
                                dot.AppendLine($"  nodo{nodoActual->IdRepuesto}_{nodoActual->IdVehiculo} -> nodo{nodoActual->IdRepuesto}_{nodoActual->Derecha->IdVehiculo} [dir=both];");
                            }

                            // Conexiones verticales
                            if (nodoActual->Abajo != null)
                            {
                                dot.AppendLine($"  nodo{nodoActual->IdRepuesto}_{nodoActual->IdVehiculo} -> nodo{nodoActual->Abajo->IdRepuesto}_{nodoActual->IdVehiculo} [dir=both];");
                            }

                            nodoActual = nodoActual->Derecha;
                        }
                    }

                    filaActual = filaActual->Siguiente;
                }

                dot.AppendLine("}");
                return dot.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en GenerarGraphviz: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                return "digraph G { node [label=\"Error al generar gráfico\"]; }";
            }
        }
        
        ~MatrizBitacora()
        {
            Dispose(false);
        }
    }
}