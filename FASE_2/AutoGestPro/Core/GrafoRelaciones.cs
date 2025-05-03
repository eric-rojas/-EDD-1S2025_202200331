// 📄 GrafoRelaciones.cs
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace AutoGestPro.Core.Estructuras
{
    public class GrafoRelaciones
    {
        private Dictionary<NodoGrafo, List<NodoGrafo>> _adyacencias;

        public GrafoRelaciones()
        {
            _adyacencias = new Dictionary<NodoGrafo, List<NodoGrafo>>();
        }

        public void InsertarRelacion(int idVehiculo, int idRepuesto)
        {
            var nodoVehiculo = new NodoGrafo(idVehiculo, "Vehiculo");
            var nodoRepuesto = new NodoGrafo(idRepuesto, "Repuesto");

            if (!_adyacencias.ContainsKey(nodoVehiculo))
                _adyacencias[nodoVehiculo] = new List<NodoGrafo>();

            if (!_adyacencias.ContainsKey(nodoRepuesto))
                _adyacencias[nodoRepuesto] = new List<NodoGrafo>();

            if (!_adyacencias[nodoVehiculo].Contains(nodoRepuesto))
                _adyacencias[nodoVehiculo].Add(nodoRepuesto);

            if (!_adyacencias[nodoRepuesto].Contains(nodoVehiculo))
                _adyacencias[nodoRepuesto].Add(nodoVehiculo);
        }

        public void GenerarGraphviz(string nombreArchivo)
        {
            string carpeta = "./Reportes";
            Directory.CreateDirectory(carpeta);
            string rutaDot = Path.Combine(carpeta, nombreArchivo + ".dot");
            string rutaPng = Path.Combine(carpeta, nombreArchivo + ".png");

            StringBuilder dot = new StringBuilder();
            dot.AppendLine("graph GrafoVehiculosRepuestos {");
            dot.AppendLine("node [shape=circle, fontname=Arial];");

            HashSet<string> conexiones = new HashSet<string>();

            foreach (var nodo in _adyacencias)
            {
                foreach (var destino in nodo.Value)
                {
                    string clave = nodo.Key.ToString().CompareTo(destino.ToString()) < 0
                        ? $"{nodo.Key} -- {destino}"
                        : $"{destino} -- {nodo.Key}";

                    if (!conexiones.Contains(clave))
                    {
                        dot.AppendLine($"\"{nodo.Key}\" -- \"{destino}\";");
                        conexiones.Add(clave);
                    }
                }
            }

            dot.AppendLine("}");
            File.WriteAllText(rutaDot, dot.ToString());
            Process.Start("dot", $"-Tpng {rutaDot} -o {rutaPng}");
        }
    }
}
