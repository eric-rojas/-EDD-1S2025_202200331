using System;
using System.Runtime.InteropServices;

namespace AutoGestPro.Core
{
    public class Repuesto
    {
        public int ID { get; set; }
        public string RepuestoNombre { get; set; }
        public string Detalles { get; set; }
        public double Costo { get; set; }

        public Repuesto(int id, string repuesto, string detalles, double costo)
        {
            ID = id;
            RepuestoNombre = repuesto;
            Detalles = detalles;
            Costo = costo;
        }

        public override string ToString()
        {
            return $"ID: {ID}, Repuesto: {RepuestoNombre}, Detalles: {Detalles}, Costo: {Costo:C}";
        }
    }
    
    public unsafe struct NodoRepuesto // creamos estructura unsafe
    {
        public int ID;
        public fixed char Repuesto[50]; // fixed hace que tengamos una memoria estipulada, en este caso tenemos 50 caracteres
        public fixed char Detalles[100];
        public double Costo;
        public NodoRepuesto* Next; // como es una estructura de listaDoblementeEnlazada, creamos punteros para adelante (next) en este caso no nescesitamos prev como en listaVehiculos.cs

        public NodoRepuesto(int id, string repuesto, string detalles, double costo)
        {
            ID = id;
            Costo = costo;
            Next = null; //esto ya esta explicado en listavehiculos.cs

            fixed (char* r = Repuesto)
                repuesto.AsSpan().CopyTo(new Span<char>(r, 50)); // esto tambien se explico en listavehiculos.cs

            fixed (char* d = Detalles)
                detalles.AsSpan().CopyTo(new Span<char>(d, 100));
        }

        public override string ToString()
        {
            fixed (char* r = Repuesto, d = Detalles)
            {
                // Esto etorna un string formateado con toda la información del repuesto
                // :C formatea el Costo como moneda espero sea buena practica y funcione pa algo xd, si no valio keso
                return $"ID: {ID}, Repuesto: {new string(r)}, Detalles: {new string(d)}, Costo: {Costo:C}";
            }
        }
    }

    public unsafe class ListaRepuestos
    {
        private NodoRepuesto* head;

        public ListaRepuestos()
        {
            head = null;
        }

        public void Insertar(int id, string repuesto, string detalles, double costo)
        {
            /* Asigna memoria no administrada del tamaño de NodoRepuesto y devuelve un puntero a esa memoria
             Marshal.AllocHGlobal, pro otro lado,  reserva memoria en el heap no administrado
            El casting (NodoRepuesto*) convierte el puntero IntPtr a un puntero de tipo NodoRepuesto*/
            // jaja es un desmadre pero espero q jale, estodo );
            NodoRepuesto* nuevoNodo = (NodoRepuesto*)Marshal.AllocHGlobal(sizeof(NodoRepuesto));
            *nuevoNodo = new NodoRepuesto(id, repuesto, detalles, costo);

            if (head == null) // si la cabeza esta vacía 
            {
                head = nuevoNodo; // entonces la cabeza pasa a ser nuevoNodo 
                head->Next = head; // y el brazo q apunta a proximo (next) se va a cabeza
            }
            else
            {
                NodoRepuesto* temp = head;
                // Recorrer hasta llegar al último nodo (el que apunta a head)
                while (temp->Next != head)
                {
                    temp = temp->Next;
                }
                // Ahora temp es el último nodo
                temp->Next = nuevoNodo;    // El último apunta al nuevo
                nuevoNodo->Next = head;    // El nuevo apunta al primero (head)
            }
        }

        public void Eliminar(int id)
        {
            if (head == null) return;

            if (head->ID == id && head->Next == head)
            {
                Marshal.FreeHGlobal((IntPtr)head);
                head = null;
                return;
            }

            NodoRepuesto* temp = head;
            NodoRepuesto* prev = null;
            do
            {
                if (temp->ID == id)
                {
                    if (prev != null)
                    {
                        prev->Next = temp->Next;
                    }
                    else
                    {
                        NodoRepuesto* last = head;
                        while (last->Next != head)
                        {
                            last = last->Next;
                        }
                        head = head->Next;
                        last->Next = head;
                    }
                    Marshal.FreeHGlobal((IntPtr)temp);
                    return;
                }
                prev = temp;
                temp = temp->Next;
            } while (temp != head);
        }

        public void Mostrar()
        {
            if (head == null)
            {
                Console.WriteLine("Lista vacía.");
                return;
            }

            NodoRepuesto* temp = head;
            do
            {
                Console.WriteLine(temp->ToString()); // bueno aqui utilizamos el tostring que habiamos hecho anteriormente para poder setear los valores con forme a lo que hiciemos
                temp = temp->Next;
            } while (temp != head);
        }

        // esto es un metodo para liberar la memoria, lo usa el aux asi q lo usamos
        ~ListaRepuestos()
        {
            if (head == null) return;

            NodoRepuesto* temp = head;
            do
            {
                NodoRepuesto* next = temp->Next;
                Marshal.FreeHGlobal((IntPtr)temp);
                temp = next;
            } while (temp != head);
        }
    }
}
