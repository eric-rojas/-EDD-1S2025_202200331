using System;
using System.Runtime.InteropServices;
using System.Text;

namespace AutoGestPro.Core
{
    public unsafe struct NodoServicio // lo mismo de las otras listas
    {
        public int ID;
        public int Id_Repuesto;
        public int Id_Vehiculo;
        public fixed char Detalles[100];
        public float Costo;
        public NodoServicio* Next;

        public NodoServicio(int id, int idRepuesto, int idVehiculo, string detalles, float costo) // lo mismo, no lo voy a explicar
        {
            ID = id;
            Id_Repuesto = idRepuesto;
            Id_Vehiculo = idVehiculo;
            Costo = costo;
            Next = null;

            fixed (char* d = Detalles)
                detalles.AsSpan().CopyTo(new Span<char>(d, 100));
        }

        public override string ToString() // lo mismo
        {
            fixed (char* d = Detalles)
            {
                return $"ID: {ID}, Id_Repuesto: {Id_Repuesto}, Id_Vehiculo: {Id_Vehiculo}, Detalles: {new string(d)}, Costo: {Costo:C}";
            }
        }
    }

    public unsafe class ColaServicios // ya explicado
    {
        private NodoServicio* frente;
        private NodoServicio* final;

        public ColaServicios()
        {
            frente = null;
            final = null;
        }

        public void Encolar(int id, int idRepuesto, int idVehiculo, string detalles, float costo) // ya explicado
        {
            NodoServicio* nuevoNodo = (NodoServicio*)Marshal.AllocHGlobal(sizeof(NodoServicio));
            *nuevoNodo = new NodoServicio(id, idRepuesto, idVehiculo, detalles, costo);

            if (final == null)
            {
                frente = nuevoNodo;
                final = nuevoNodo;
            }
            else // este si lo explico, como es una cola al querer insertar nuevo nodo entonces 
            {
                final->Next = nuevoNodo; //el nodo final su next va a apuntar al nuevo nodo 
                final = nuevoNodo; //y el nuevo nodo se convierte en final
            }
        }

        public void Desencolar() //ya explicado
        {
            if (frente == null)
            {
                Console.WriteLine("La cola está vacía.");
                return;
            }

            NodoServicio* temp = frente;
            frente = frente->Next;
            if (frente == null)
                final = null;

            Marshal.FreeHGlobal((IntPtr)temp);
        }   

        public void Mostrar() //ya explicado
        {
            if (frente == null)
            {
                Console.WriteLine("La cola está vacía.");
                return;
            }

            NodoServicio* temp = frente;
            while (temp != null)
            {
                Console.WriteLine(temp->ToString());
                temp = temp->Next;
            }
        }

        ~ColaServicios()
        {
            while (frente != null)
            {
                NodoServicio* temp = frente;
                frente = frente->Next;
                Marshal.FreeHGlobal((IntPtr)temp);
            }
        }


        public unsafe string GenerarGraphviz()
        {
            if (frente == null)
            {
                return "digraph G {\n    node [shape=record];\n    NULL [label = \"{NULL}\"];\n}\n";
            }

            StringBuilder dot = new StringBuilder();
            dot.AppendLine("digraph G {");
            dot.AppendLine("    rankdir=LR;"); // Left to Right
            dot.AppendLine("    node [shape=record];");
            dot.AppendLine("    subgraph cluster_0 {");
            dot.AppendLine("        label = \"Cola de Servicios\";");
            dot.AppendLine("        style=filled;");
            dot.AppendLine("        color=lightblue;");

            // Crear los nodos con formato vertical
            NodoServicio* actual = frente;
            while (actual != null)
            {
                dot.AppendLine($"        node{actual->ID} [label=\"{{" +
                    $"{{ID: {actual->ID}}}|" +
                    $"{{ID Repuesto: {actual->Id_Repuesto}}}|" +
                    $"{{ID Vehículo: {actual->Id_Vehiculo}}}|" +
                    $"{{Servicio: {new string(actual->Detalles).TrimEnd('\0')}}}|" +
                    $"{{Costo: {actual->Costo:C}}}" +
                    $"}}\"];");
                actual = actual->Next;
            }

            // Crear las conexiones con curvas
            actual = frente;
            while (actual->Next != null)
            {
                dot.AppendLine($"        node{actual->ID} -> node{actual->Next->ID} [color=blue, constraint=true];");
                actual = actual->Next;
            }

            dot.AppendLine("    }");
            dot.AppendLine("}");
            return dot.ToString();
        }







    }
}
