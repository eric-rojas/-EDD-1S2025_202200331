// 📄 ArbolMerkleFacturas.cs
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace AutoGestPro.Core.Estructuras
{
    public class ArbolMerkleFacturas
    {
        public List<NodoMerkle> Hojas { get; private set; }
        private NodoMerkle raiz;

        public ArbolMerkleFacturas()
        {
            Hojas = new List<NodoMerkle>();
            raiz = null;
        }

        public void Insertar(Factura factura)
        {
            if (BuscarPorIdServicio(factura.ID_Servicio) != null)
                return;

            if (Buscar(factura.ID) != null)
                return;

            Hojas.Add(new NodoMerkle(factura));
            Construir();
        }

        private void Construir()
        {
            if (Hojas.Count == 0) { raiz = null; return; }

            List<NodoMerkle> actual = new List<NodoMerkle>(Hojas);
            while (actual.Count > 1)
            {
                List<NodoMerkle> siguiente = new List<NodoMerkle>();

                for (int i = 0; i < actual.Count; i += 2)
                {
                    var izq = actual[i];
                    var der = (i + 1 < actual.Count) ? actual[i + 1] : null;
                    siguiente.Add(new NodoMerkle(izq, der));
                }

                actual = siguiente;
            }

            raiz = actual[0];
        }

        public Factura Buscar(int idFactura)
        {
            foreach (var hoja in Hojas)
                if (hoja.Factura.ID == idFactura) return hoja.Factura;
            return null;
        }

        public Factura BuscarPorIdServicio(int idServicio)
        {
            foreach (var hoja in Hojas)
                if (hoja.Factura.ID_Servicio == idServicio) return hoja.Factura;
            return null;
        }

        public bool Eliminar(int idFactura)
        {
            var hoja = Hojas.Find(h => h.Factura.ID == idFactura);
            if (hoja != null)
            {
                Hojas.Remove(hoja);
                Construir();
                return true;
            }
            return false;
        }

        public void GenerarGraphviz(string nombreArchivo)
        {
            if (raiz == null) return;

            string carpeta = "./Reportes";
            Directory.CreateDirectory(carpeta);
            string rutaDot = Path.Combine(carpeta, nombreArchivo + ".dot");
            string rutaPng = Path.Combine(carpeta, nombreArchivo + ".png");

            StringBuilder dot = new StringBuilder();
            dot.AppendLine("digraph MerkleTree {");
            dot.AppendLine("node [shape=record, fontname=Arial];");

            Dictionary<string, int> ids = new Dictionary<string, int>();
            int contador = 0;
            GenerarDotRecursivo(raiz, dot, ids, ref contador);

            dot.AppendLine("}");
            File.WriteAllText(rutaDot, dot.ToString());

            Process.Start("dot", $"-Tpng {rutaDot} -o {rutaPng}");
        }

        private void GenerarDotRecursivo(NodoMerkle nodo, StringBuilder dot, Dictionary<string, int> ids, ref int contador)
        {
            if (nodo == null) return;

            if (!ids.ContainsKey(nodo.Hash))
                ids[nodo.Hash] = contador++;

            int idActual = ids[nodo.Hash];
            string label = nodo.Factura != null
                ? $"Factura #{nodo.Factura.ID}\nS:{nodo.Factura.ID_Servicio}\nQ{nodo.Factura.Total:0.00}" 
                : $"Hash: {nodo.Hash.Substring(0, 10)}";

            dot.AppendLine($"n{idActual} [label=\"{label}\"];");

            if (nodo.Left != null)
            {
                if (!ids.ContainsKey(nodo.Left.Hash))
                    ids[nodo.Left.Hash] = contador++;
                dot.AppendLine($"n{idActual} -> n{ids[nodo.Left.Hash]};");
                GenerarDotRecursivo(nodo.Left, dot, ids, ref contador);
            }

            if (nodo.Right != null)
            {
                if (!ids.ContainsKey(nodo.Right.Hash))
                    ids[nodo.Right.Hash] = contador++;
                dot.AppendLine($"n{idActual} -> n{ids[nodo.Right.Hash]};");
                GenerarDotRecursivo(nodo.Right, dot, ids, ref contador);
            }
        }
    }
}