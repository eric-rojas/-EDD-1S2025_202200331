using System;
using System.Text;
using System.Collections.Generic;

namespace AutoGestPro.Core
{
    // Definición de la clase Repuesto
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

    // Clase Nodo para el árbol AVL
    public class NodoRepuesto
    {
        public Repuesto Repuesto { get; set; }
        public NodoRepuesto Izquierda { get; set; }
        public NodoRepuesto Derecha { get; set; }
        public int Altura { get; set; }

        public NodoRepuesto(Repuesto repuesto)
        {
            Repuesto = repuesto;
            Izquierda = null;
            Derecha = null;
            Altura = 1;
        }
    }

    // Implementación del árbol AVL de repuestos
    public class ArbolAVLRepuestos
    {
        private NodoRepuesto raiz;

        public ArbolAVLRepuestos()
        {
            raiz = null;
        }

        // Verificar si existe un ID en el árbol
        public bool ExisteID(int id)
        {
            return Buscar(id) != null;
        }

        // Obtener altura de un nodo
        private int ObtenerAltura(NodoRepuesto nodo)
        {
            return nodo == null ? 0 : nodo.Altura;
        }

        // Obtener factor de balance
        private int ObtenerBalance(NodoRepuesto nodo)
        {
            return nodo == null ? 0 : ObtenerAltura(nodo.Izquierda) - ObtenerAltura(nodo.Derecha);
        }

        // Rotación derecha
        private NodoRepuesto RotacionDerecha(NodoRepuesto y)
        {
            NodoRepuesto x = y.Izquierda;
            NodoRepuesto T2 = x.Derecha;

            // Realizar rotación
            x.Derecha = y;
            y.Izquierda = T2;

            // Actualizar alturas
            y.Altura = Math.Max(ObtenerAltura(y.Izquierda), ObtenerAltura(y.Derecha)) + 1;
            x.Altura = Math.Max(ObtenerAltura(x.Izquierda), ObtenerAltura(x.Derecha)) + 1;

            return x;
        }

        // Rotación izquierda
        private NodoRepuesto RotacionIzquierda(NodoRepuesto x)
        {
            NodoRepuesto y = x.Derecha;
            NodoRepuesto T2 = y.Izquierda;

            // Realizar rotación
            y.Izquierda = x;
            x.Derecha = T2;

            // Actualizar alturas
            x.Altura = Math.Max(ObtenerAltura(x.Izquierda), ObtenerAltura(x.Derecha)) + 1;
            y.Altura = Math.Max(ObtenerAltura(y.Izquierda), ObtenerAltura(y.Derecha)) + 1;

            return y;
        }

        // Insertar un repuesto con validación de ID único
        public bool Insertar(Repuesto repuesto)
        {
            // Validar que el ID sea único
            if (ExisteID(repuesto.ID))
            {
                Console.WriteLine($"Error: Ya existe un repuesto con el ID {repuesto.ID}.");
                return false;
            }

            // Insertar el repuesto
            raiz = InsertarRecursivo(raiz, repuesto);
            return true;
        }

        // Sobrecarga para mantener compatibilidad
        public bool Insertar(int id, string nombre, string detalles, double costo)
        {
            Repuesto repuesto = new Repuesto(id, nombre, detalles, costo);
            return Insertar(repuesto);
        }

        private NodoRepuesto InsertarRecursivo(NodoRepuesto nodo, Repuesto repuesto)
        {
            // Inserción normal en un BST
            if (nodo == null)
                return new NodoRepuesto(repuesto);

            if (repuesto.ID < nodo.Repuesto.ID)
                nodo.Izquierda = InsertarRecursivo(nodo.Izquierda, repuesto);
            else if (repuesto.ID > nodo.Repuesto.ID)
                nodo.Derecha = InsertarRecursivo(nodo.Derecha, repuesto);
            else
                return nodo; // ID duplicado, no se inserta

            // Actualizar altura del nodo actual
            nodo.Altura = 1 + Math.Max(ObtenerAltura(nodo.Izquierda), ObtenerAltura(nodo.Derecha));

            // Obtener factor de balance
            int balance = ObtenerBalance(nodo);

            // Casos de balanceo
            // Caso Izquierda-Izquierda
            if (balance > 1 && repuesto.ID < nodo.Izquierda.Repuesto.ID)
                return RotacionDerecha(nodo);

            // Caso Derecha-Derecha
            if (balance < -1 && repuesto.ID > nodo.Derecha.Repuesto.ID)
                return RotacionIzquierda(nodo);

            // Caso Izquierda-Derecha
            if (balance > 1 && repuesto.ID > nodo.Izquierda.Repuesto.ID)
            {
                nodo.Izquierda = RotacionIzquierda(nodo.Izquierda);
                return RotacionDerecha(nodo);
            }

            // Caso Derecha-Izquierda
            if (balance < -1 && repuesto.ID < nodo.Derecha.Repuesto.ID)
            {
                nodo.Derecha = RotacionDerecha(nodo.Derecha);
                return RotacionIzquierda(nodo);
            }

            return nodo;
        }

        // Buscar un repuesto por ID
        public Repuesto Buscar(int id)
        {
            NodoRepuesto nodo = BuscarRecursivo(raiz, id);
            return nodo?.Repuesto;
        }

        private NodoRepuesto BuscarRecursivo(NodoRepuesto nodo, int id)
        {
            if (nodo == null || nodo.Repuesto.ID == id)
                return nodo;

            if (id < nodo.Repuesto.ID)
                return BuscarRecursivo(nodo.Izquierda, id);
            else
                return BuscarRecursivo(nodo.Derecha, id);
        }

        // Encontrar el nodo con el valor mínimo
        private NodoRepuesto NodoMinimo(NodoRepuesto nodo)
        {
            NodoRepuesto actual = nodo;
            while (actual.Izquierda != null)
                actual = actual.Izquierda;
            return actual;
        }

        // Eliminar un repuesto por ID
        public bool Eliminar(int id)
        {
            if (!ExisteID(id))
                return false;

            raiz = EliminarRecursivo(raiz, id);
            return true;
        }

        private NodoRepuesto EliminarRecursivo(NodoRepuesto nodo, int id)
        {
            // Paso 1: Realizar la eliminación normal de BST
            if (nodo == null)
                return nodo;

            // Si el id a eliminar es menor que el id del nodo actual, buscar en el subárbol izquierdo
            if (id < nodo.Repuesto.ID)
                nodo.Izquierda = EliminarRecursivo(nodo.Izquierda, id);
            // Si el id a eliminar es mayor que el id del nodo actual, buscar en el subárbol derecho
            else if (id > nodo.Repuesto.ID)
                nodo.Derecha = EliminarRecursivo(nodo.Derecha, id);
            // Si el id a eliminar es igual al id del nodo actual, este es el nodo a eliminar
            else
            {
                // Caso 1: Nodo con un solo hijo o sin hijos
                if (nodo.Izquierda == null)
                    return nodo.Derecha;
                else if (nodo.Derecha == null)
                    return nodo.Izquierda;

                // Caso 2: Nodo con dos hijos
                // Obtener el sucesor inorden (el más pequeño en el subárbol derecho)
                NodoRepuesto sucesor = NodoMinimo(nodo.Derecha);
                
                // Copiar los datos del sucesor a este nodo
                nodo.Repuesto = sucesor.Repuesto;
                
                // Eliminar el sucesor
                nodo.Derecha = EliminarRecursivo(nodo.Derecha, sucesor.Repuesto.ID);
            }

            // Si el árbol tiene solo un nodo, retornarlo
            if (nodo == null)
                return nodo;

            // Paso 2: Actualizar la altura del nodo actual
            nodo.Altura = 1 + Math.Max(ObtenerAltura(nodo.Izquierda), ObtenerAltura(nodo.Derecha));

            // Paso 3: Obtener el factor de balance
            int balance = ObtenerBalance(nodo);

            // Paso 4: Balancear el árbol si es necesario

            // Caso Izquierda-Izquierda
            if (balance > 1 && ObtenerBalance(nodo.Izquierda) >= 0)
                return RotacionDerecha(nodo);

            // Caso Izquierda-Derecha
            if (balance > 1 && ObtenerBalance(nodo.Izquierda) < 0)
            {
                nodo.Izquierda = RotacionIzquierda(nodo.Izquierda);
                return RotacionDerecha(nodo);
            }

            // Caso Derecha-Derecha
            if (balance < -1 && ObtenerBalance(nodo.Derecha) <= 0)
                return RotacionIzquierda(nodo);

            // Caso Derecha-Izquierda
            if (balance < -1 && ObtenerBalance(nodo.Derecha) > 0)
            {
                nodo.Derecha = RotacionDerecha(nodo.Derecha);
                return RotacionIzquierda(nodo);
            }

            return nodo;
        }

        // Editar un repuesto
        public bool Editar(int id, string nuevoNombre, string nuevosDetalles, double nuevoCosto)
        {
            NodoRepuesto nodo = BuscarRecursivo(raiz, id);
            if (nodo == null)
                return false;

            nodo.Repuesto.RepuestoNombre = nuevoNombre;
            nodo.Repuesto.Detalles = nuevosDetalles;
            nodo.Repuesto.Costo = nuevoCosto;
            return true;
        }

        // Mostrar todos los repuestos (recorrido InOrden)
        public void Mostrar()
        {
            List<Repuesto> repuestos = new List<Repuesto>();
            InOrdenRecursivo(raiz, repuestos);
            
            foreach (var repuesto in repuestos)
            {
                Console.WriteLine(repuesto);
            }
        }

        // Obtener todos los repuestos ordenados por ID
        public List<Repuesto> ObtenerTodos()
        {
            List<Repuesto> repuestos = new List<Repuesto>();
            InOrdenRecursivo(raiz, repuestos);
            return repuestos;
        }

        private void InOrdenRecursivo(NodoRepuesto nodo, List<Repuesto> resultado)
        {
            if (nodo == null)
                return;
            
            InOrdenRecursivo(nodo.Izquierda, resultado);
            resultado.Add(nodo.Repuesto);
            InOrdenRecursivo(nodo.Derecha, resultado);
        }

        // Generar visualización Graphviz
        public string GenerarGraphviz()
        {
            if (raiz == null)
                return "digraph G {\n    node [shape=record];\n    NULL [label = \"{NULL}\"];\n}\n";

            StringBuilder dot = new StringBuilder();
            dot.AppendLine("digraph ArbolAVL {");
            dot.AppendLine("    node [shape=rectangle];");
            GenerarNodosGraphviz(raiz, dot);
            GenerarConexionesGraphviz(raiz, dot);
            dot.AppendLine("}");
            return dot.ToString();
        }

        private void GenerarNodosGraphviz(NodoRepuesto nodo, StringBuilder dot)
        {
            if (nodo == null)
                return;

            dot.AppendLine($"    \"{nodo.Repuesto.ID}\" [label=\"ID: {nodo.Repuesto.ID}\\nRepuesto: {nodo.Repuesto.RepuestoNombre}\\nDetalles: {nodo.Repuesto.Detalles}\\nCosto: {nodo.Repuesto.Costo:C}\\nAltura: {nodo.Altura}\"];");
            GenerarNodosGraphviz(nodo.Izquierda, dot);
            GenerarNodosGraphviz(nodo.Derecha, dot);
        }

        private void GenerarConexionesGraphviz(NodoRepuesto nodo, StringBuilder dot)
        {
            if (nodo == null)
                return;

            if (nodo.Izquierda != null)
                dot.AppendLine($"    \"{nodo.Repuesto.ID}\" -> \"{nodo.Izquierda.Repuesto.ID}\" [color=blue, label=\"I\"];");
            if (nodo.Derecha != null)
                dot.AppendLine($"    \"{nodo.Repuesto.ID}\" -> \"{nodo.Derecha.Repuesto.ID}\" [color=red, label=\"D\"];");

            GenerarConexionesGraphviz(nodo.Izquierda, dot);
            GenerarConexionesGraphviz(nodo.Derecha, dot);
        }

        // Recorridos adicionales para obtener los repuestos en diferentes órdenes

        // Recorrido PreOrden (Raíz-Izquierda-Derecha)
        public List<Repuesto> RecorridoPreOrden()
        {
            List<Repuesto> resultado = new List<Repuesto>();
            PreOrdenRecursivo(raiz, resultado);
            return resultado;
        }

        private void PreOrdenRecursivo(NodoRepuesto nodo, List<Repuesto> resultado)
        {
            if (nodo == null)
                return;
            
            resultado.Add(nodo.Repuesto);
            PreOrdenRecursivo(nodo.Izquierda, resultado);
            PreOrdenRecursivo(nodo.Derecha, resultado);
        }

        // Recorrido PostOrden (Izquierda-Derecha-Raíz)
        public List<Repuesto> RecorridoPostOrden()
        {
            List<Repuesto> resultado = new List<Repuesto>();
            PostOrdenRecursivo(raiz, resultado);
            return resultado;
        }

        private void PostOrdenRecursivo(NodoRepuesto nodo, List<Repuesto> resultado)
        {
            if (nodo == null)
                return;
            
            PostOrdenRecursivo(nodo.Izquierda, resultado);
            PostOrdenRecursivo(nodo.Derecha, resultado);
            resultado.Add(nodo.Repuesto);
        }
    }
}