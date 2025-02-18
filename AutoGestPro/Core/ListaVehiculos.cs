using System;
using System.Runtime.InteropServices;

/* bueno segun el enunciado este sistema de vehiculos se tiene que manejar con una estructura "unsafe code" esto es para manejar mejor los punteros con direcciones
de memoria exactas. 
en esta seccion implementaremos eso y pues haber si nos sale 
*/
namespace AutoGestPro.Core
{
        public class Vehiculo
    {
        public int ID { get; set; }
        public int ID_Usuario { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Placa { get; set; }

        public Vehiculo(int id, int idUsuario, string marca, string modelo, string placa)
        {
            ID = id;
            ID_Usuario = idUsuario;
            Marca = marca;
            Modelo = modelo;
            Placa = placa;
        }

        public override string ToString()
        {
            return $"ID: {ID}, ID_Usuario: {ID_Usuario}, Marca: {Marca}, Modelo: {Modelo}, Placa: {Placa}";
        }
    }
    public unsafe struct NodoVehiculo // creamos estructura unsafe del nodo vehiculo
    {
        public int Id;

        public int ID_Usuario;
        public fixed char Marca[50]; // fixed hace que tengamos una memoria estipulada, en este caso tenemos 50 caracteres
        
        public fixed char Modelo[50];
        public fixed char Placa[20];
        public NodoVehiculo* Next; // como es una estructura de listaDoblementeEnlazada, creamos punteros para adelante (next)y para atras (prev)
        public NodoVehiculo* Prev;

        public NodoVehiculo(int id, int idUsuario, string marca, string modelo, string placa) 
        // creamos nuestro constructor  de nodo en la cual le decimos que datos va a recibir (entre parentesis) 
        {
            // aqui definimos nuestras variables de id y definimos q nustros punteros esten apuntando a nada al iniciar
            Id = id;
            ID_Usuario = idUsuario;
            Next = null;
            Prev = null;

            /* Bueno si soy sincero no le entendia mucho a esto... mas que todo porq el aux lo uso, pues investigue que onda... pero aqui va la explicación
            De cadenas a arreglos de caracteres:
            en palabras simples, tenemos cadenas de palabras tales como la marca del carro, modelo y placa. bueno estas son palabras y tienen caracteres, les
            asignamos un limite de caracteres [50][50][20] y pues lo que hacemos en el "fixed" es pasar estos caracteres a un arreglo de ellos. es como pasarlos
            a una caja caracter por caracter (letra a letra)... entonces lo vamos haciendo hasta que llenamos la caja con las letras ordenas de la palbra "Toyota"
            por ejemplo. 
            */

            fixed (char* m = Marca) // bueno fixed ya explicamos q es, y pues tenemos char* que es el arreglo de caracteres o caja jajaj segun la explication
            {
                for (int i = 0; i < marca.Length && i < 50; i++) // aqui entramos a un for, eso es una validacion para que se copia hasta q se acaben las letras o caracteres o bien se pase y pues no lo deje.
                    m[i] = marca[i];
            }

            fixed (char* mo = Modelo)
            {
                for (int i = 0; i < modelo.Length && i < 50; i++) // el int i = 0 pues es que vamos a iniciar a contar desde 0, ese i tiene q ser menor que la longitude la palabra q ingresamos y asimismo menor a 50 en este caso.
                    mo[i] = modelo[i];
            }

            fixed (char* p = Placa)
            {
                for (int i = 0; i < placa.Length && i < 20; i++) // ademas tenemos el i++ que quiere decir que esto lo vamos a hacer en un intervalo de 1 osea contar 0,1,2,3....
                    p[i] = placa[i]; // y pues si esto se valida, retornamos eso
            }
        }

        public override string ToString()
        {
            fixed (char* m = Marca, mo = Modelo, p = Placa)
            {
                return $"ID: {Id}, ID_Usuario: {ID_Usuario}, Marca: {new string(m)}, Modelo: {new string(mo)}, Placa: {new string(p)}";
            }
        }
    }

    // Lista Doblemente Enlazada
    public unsafe class ListaVehiculos
    {
        private NodoVehiculo* head; // definimos la cabeza
        private NodoVehiculo* tail; // y la cola

        public ListaVehiculos()
        {
            head = null; //los definimos como nulos para el inicio del desmadre!
            tail = null;
        }

        public void Insertar(int id, int idUsuario, string marca, string modelo, string placa) // bueno esta funcion es para insertar lo que le pasemos entre ()
        {
            NodoVehiculo* nuevoNodo = (NodoVehiculo*)NativeMemory.Alloc((nuint)sizeof(NodoVehiculo)); // bueno aqui hay algo interesante. TEnemos el NativeMemory.Alloc((nuint) que esto lo puso el aux pero pues tiene su ciencia para manejar bien los unsafe            
            //con unsafe se asigna memoria de forma manual, NativeMemory.Alloc reserva memoria del tamaño de un NodoVehiculo. El resultado se convierte a un puntero de tipo NodoVehiculo

            *nuevoNodo = new NodoVehiculo(id, idUsuario, marca, modelo, placa); // cramos nuea instancia de NodoVehiculo en nuevoNodo y el "*" quiere decir q le asiganamos ese valor al espacio de memoria al que apunta nuevoNodo

            if (head == null)
            {
                head = tail = nuevoNodo; // si (la cabeza esta nula) no hay nada entonces el nuevoNodo va a ser cabeza y cola... osea el primer dato
            }
            else // si esta llena entonces: 
            {
                tail->Next = nuevoNodo; // el apuntador Next del ultimo nodo (tail en este momento) va a apuntar al nuevoNodo
                nuevoNodo->Prev = tail; // el apuntador Previo del nuevoNodo apuntará al ultimo nodo (tail en este momento)
                tail = nuevoNodo; // y el nuevoNodo se convertira en tail porque ahora es el ultimo nodo

                /*
                osea un poco de demostracion 
                lista normal: nodo1 -><- nodo2 -><- nodo3 -><- nodo 4

                lista: nodo1 -><- nodo2 -><- nodo3 -><- nodo 4 (ultimo nodo "tail")                     en este momento queremos meter a "nuevoNodo"
                lista: nodo1 -><- nodo2 -><- nodo3 -><- nodo 4 (ultimo nodo "tail") ->                  el next de tail empieza a apuntar a nuevoNodo
                lista: nodo1 -><- nodo2 -><- nodo3 -><- nodo 4 (ultimo nodo "tail") -> <-               el prev de nuevoNodo empieza a apuntar a tail
                lista: nodo1 -><- nodo2 -><- nodo3 -><- nodo 4  -> <- nuevoNodo (ultimo nodo "tail")    ahora nuevoNodo se convierte en tail
                */

            }
        }

        public void Eliminar(int id)
        {
            NodoVehiculo* actual = head;
            while (actual != null)
            {
                if (actual->Id == id)
                {
                    if (actual->Prev != null)
                        actual->Prev->Next = actual->Next;
                    else
                        head = actual->Next;

                    if (actual->Next != null)
                        actual->Next->Prev = actual->Prev;
                    else
                        tail = actual->Prev;

                    NativeMemory.Free(actual);
                    return;
                }
                actual = actual->Next;
            }
        }

        public void Mostrar()
        {
            NodoVehiculo* actual = head;
            while (actual != null)
            {
                Console.WriteLine(actual->ToString());
                actual = actual->Next;
            }
        }

        public void MostrarReversa()
        {
            NodoVehiculo* actual = tail;
            while (actual != null)
            {
                Console.WriteLine(actual->ToString());
                actual = actual->Prev;
            }
        }

        ~ListaVehiculos()
        {
            NodoVehiculo* actual = head;
            while (actual != null)
            {
                NodoVehiculo* temp = actual;
                actual = actual->Next;
                NativeMemory.Free(temp);
            }
        }
    }





}
