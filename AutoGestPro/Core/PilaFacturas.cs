using System;
using System.Runtime.InteropServices;
using System.Text;  

namespace AutoGestPro.Core
{
    public unsafe struct NodoFactura
    {
        public int ID;
        public int ID_Orden;
        public float Total;
        public NodoFactura* Next;

        public NodoFactura(int id, int idOrden, float total)
        {
            ID = id;
            ID_Orden = idOrden;
            Total = total;
            Next = null;
        }

        public override string ToString()
        {
            return $"ID: {ID}, ID_Orden: {ID_Orden}, Total: {Total:C}";
        }
    }

    public unsafe class PilaFacturas
    {
        private NodoFactura* top;
        private int contadorID;

        public PilaFacturas()
        {
            top = null;
            contadorID = 1;
        }

        public void Apilar(int idOrden, float total)
        {
            NodoFactura* nuevoNodo = (NodoFactura*)Marshal.AllocHGlobal(sizeof(NodoFactura));
            *nuevoNodo = new NodoFactura(contadorID++, idOrden, total);
            
            nuevoNodo->Next = top;
            top = nuevoNodo;
        }

        public void Desapilar()
        {
            if (top == null) return;
            
            NodoFactura* temp = top;
            top = top->Next;
            Marshal.FreeHGlobal((IntPtr)temp);
        }

        public void Mostrar()
        {
            if (top == null)
            {
                Console.WriteLine("Pila vacía.");
                return;
            }

            NodoFactura* temp = top;
            while (temp != null)
            {
                Console.WriteLine(temp->ToString());
                temp = temp->Next;
            }
        }

        ~PilaFacturas()
        {
            while (top != null)
            {
                NodoFactura* temp = top;
                top = top->Next;
                Marshal.FreeHGlobal((IntPtr)temp);
            }
        }


        public unsafe string GenerarGraphviz()
        {
            if (top == null)
            {
                return "digraph G {\n    node [shape=record];\n    NULL [label = \"{NULL}\"];\n}\n";
            }

            StringBuilder dot = new StringBuilder();
            dot.AppendLine("digraph G {");
            dot.AppendLine("    rankdir=TB;"); // Orientación de arriba hacia abajo
            dot.AppendLine("    node [shape=record];");
            dot.AppendLine("    subgraph cluster_0 {");
            dot.AppendLine("        label = \"Pila de Facturas\";");
            dot.AppendLine("        style=filled;");
            dot.AppendLine("        color=lightgrey;");

            // Crear los nodos
            NodoFactura* actual = top;
            while (actual != null)
            {
                dot.AppendLine($"        node{actual->ID} [label=\"{{" +
                    $"ID: {actual->ID} | " +
                    $"ID_Orden: {actual->ID_Orden} | " +
                    $"Total: {actual->Total:C}" +
                    $"}}\"];");
                actual = actual->Next;
            }

            // Crear las conexiones entre nodos (de arriba hacia abajo)
            actual = top;
            while (actual->Next != null)
            {
                dot.AppendLine($"        node{actual->ID} -> node{actual->Next->ID} [dir=back, color=\"blue\"];");
                actual = actual->Next;
            }

            dot.AppendLine("    }");
            dot.AppendLine("}");
            return dot.ToString();
        }





    }
}
