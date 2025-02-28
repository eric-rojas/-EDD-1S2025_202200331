using System;
using System.IO;

namespace ProyectoGestion.Core
{
    // Definición del miembro
    public class Miembro
    {
        public int Identificador { get; set; }
        public string PrimerNombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string CorreoElectronico { get; set; }
        public string Clave { get; set; }

        public Miembro(int id, string primerNombre, string apellidoPaterno, string correo, string clave)
        {
            Identificador = id;
            PrimerNombre = primerNombre;
            ApellidoPaterno = apellidoPaterno;
            CorreoElectronico = correo;
            Clave = clave;
        }

        public override string ToString()
        {
            return $"ID: {Identificador}, Nombre: {PrimerNombre} {ApellidoPaterno}, Correo: {CorreoElectronico}";
        }
    }

    // Definición del nodo
    public class Elemento
    {
        public Miembro Miembro { get; set; }
        public Elemento SiguienteElemento { get; set; }

        public Elemento(Miembro miembro)
        {
            Miembro = miembro;
            SiguienteElemento = null;
        }
    }

    // Lista de miembros utilizando unsafe code
    public class ListaMiembros
    {
        private Elemento cabeza;

        public ListaMiembros()
        {
            cabeza = null;
        }

        public void Agregar(Miembro miembro)
        {
            Elemento nuevoElemento = new Elemento(miembro);
            if (cabeza == null)
            {
                cabeza = nuevoElemento;
            }
            else
            {
                Elemento actual = cabeza;
                while (actual.SiguienteElemento != null)
                {
                    actual = actual.SiguienteElemento;
                }
                actual.SiguienteElemento = nuevoElemento;
            }
        }

        public Miembro BuscarPorId(int id)
        {
            Elemento actual = cabeza;
            while (actual != null)
            {
                if (actual.Miembro.Identificador == id)
                    return actual.Miembro;
                actual = actual.SiguienteElemento;
            }
            return null;
        }

        public bool Actualizar(int id, string nuevoPrimerNombre, string nuevoApellidoPaterno, string nuevoCorreo, string nuevaClave)
        {
            Miembro miembro = BuscarPorId(id);
            if (miembro != null)
            {
                miembro.PrimerNombre = nuevoPrimerNombre;
                miembro.ApellidoPaterno = nuevoApellidoPaterno;
                miembro.CorreoElectronico = nuevoCorreo;
                miembro.Clave = nuevaClave;
                return true;
            }
            return false;
        }

        public bool EliminarPorId(int id)
        {
            if (cabeza == null) return false;

            if (cabeza.Miembro.Identificador == id)
            {
                cabeza = cabeza.SiguienteElemento;
                return true;
            }

            Elemento actual = cabeza;
            while (actual.SiguienteElemento != null && actual.SiguienteElemento.Miembro.Identificador != id)
            {
                actual = actual.SiguienteElemento;
            }

            if (actual.SiguienteElemento == null) return false;

            actual.SiguienteElemento = actual.SiguienteElemento.SiguienteElemento;
            return true;
        }

        public void MostrarLista()
        {
            Elemento actual = cabeza;
            while (actual != null)
            {
                Console.WriteLine(actual.Miembro);
                actual = actual.SiguienteElemento;
            }
        }

        public string GenerarGraphviz()
        {
            if (cabeza == null)
            {
                return "digraph G {\n    node [shape=record];\n    NULL [label = \"{NULL}\"];\n}\n";
            }

            var graphviz = "digraph G {\n";
            graphviz += "    node [shape=record];\n";
            graphviz += "    rankdir=LR;\n";
            graphviz += "    subgraph cluster_0 {\n";
            graphviz += "        label = \"Lista de Miembros\";\n";

            Elemento actual = cabeza;
            int index = 0;
            while (actual != null)
            {
                graphviz += $"        n{index} [label = \"{{<data> ID: {actual.Miembro.Identificador} \\n" +
                        $"Primer Nombre: {actual.Miembro.PrimerNombre} \\n" +
                        $"Apellido Paterno: {actual.Miembro.ApellidoPaterno} \\n" +
                        $"Correo: {actual.Miembro.CorreoElectronico} \\n" +
                        $"Clave: {actual.Miembro.Clave} \\n" +
                        $"Siguiente: }}\"];\n";
                actual = actual.SiguienteElemento;
                index++;
            }

            actual = cabeza;
            for (int i = 0; actual != null && actual.SiguienteElemento != null; i++)
            {
                graphviz += $"        n{i} -> n{i + 1};\n";
                actual = actual.SiguienteElemento;
            }

            graphviz += "    }\n";
            graphviz += "}\n";
            return graphviz;
        }
    }
}
