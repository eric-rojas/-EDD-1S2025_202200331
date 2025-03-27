using System;
using System.Collections.Generic;
using System.IO;

public class Factura
{
    public int ID { get; set; }
    public int ID_Servicio { get; set; }
    public double Total { get; set; }
    public int ID_Usuario { get; set; }  // Campo agregado para asociar la factura a un usuario

    public Factura(int id, int idServicio, double total, int idUsuario)
    {
        ID = id;
        ID_Servicio = idServicio;
        Total = total;
        ID_Usuario = idUsuario;  // Asociar el usuario
    }

    public override string ToString()
    {
        return $"ID: {ID}, ID_Servicio: {ID_Servicio}, Total: {Total:C}";
    }
}


public class NodoArbolB
{
    public List<Factura> Facturas { get; set; } = new List<Factura>();
    public List<NodoArbolB> Hijos { get; set; } = new List<NodoArbolB>();
    public bool EsHoja { get; set; } = true;

    public override string ToString()
    {
        return string.Join(" | ", Facturas);
    }
}

public class ArbolBFacturas
{
    private NodoArbolB raiz;
    private const int ORDEN = 5;
    private int contadorID = 1;

    public ArbolBFacturas()
    {
        raiz = new NodoArbolB { EsHoja = true };
    }

    public void Insertar(Factura factura)
    {
        if (ExisteID(factura.ID))
        {
            Console.WriteLine($"Error: Ya existe una factura con el ID {factura.ID}.");
            return;
        }

        if (raiz.Facturas.Count == (2 * ORDEN) - 1)
        {
            NodoArbolB nuevoNodo = new NodoArbolB { EsHoja = false };
            nuevoNodo.Hijos.Add(raiz);
            DividirHijo(nuevoNodo, 0, raiz);
            raiz = nuevoNodo;
        }
        InsertarNoLleno(raiz, factura);
    }

    public bool Insertar(int idServicio, double total, int idUsuario)
    {
        Factura factura = new Factura(GenerarNuevoID(), idServicio, total, idUsuario);
        Insertar(factura);
        return true;
    }

    public int GenerarNuevoID()
    {
        return contadorID++;
    }

    private void InsertarNoLleno(NodoArbolB nodo, Factura factura)
    {
        int i = nodo.Facturas.Count - 1;

        if (nodo.EsHoja)
        {
            while (i >= 0 && factura.ID < nodo.Facturas[i].ID)
            {
                i--;
            }
            nodo.Facturas.Insert(i + 1, factura);
        }
        else
        {
            while (i >= 0 && factura.ID < nodo.Facturas[i].ID)
                i--;
            i++;

            if (nodo.Hijos[i].Facturas.Count == (2 * ORDEN) - 1)
            {
                DividirHijo(nodo, i, nodo.Hijos[i]);
                if (factura.ID > nodo.Facturas[i].ID)
                    i++;
            }
            InsertarNoLleno(nodo.Hijos[i], factura);
        }
    }

    private void DividirHijo(NodoArbolB padre, int indice, NodoArbolB hijo)
    {
        NodoArbolB nuevoHijo = new NodoArbolB { EsHoja = hijo.EsHoja };
        int medio = ORDEN - 1;

        padre.Facturas.Insert(indice, hijo.Facturas[medio]);
        padre.Hijos.Insert(indice + 1, nuevoHijo);

        nuevoHijo.Facturas.AddRange(hijo.Facturas.GetRange(medio + 1, ORDEN - 1));
        hijo.Facturas.RemoveRange(medio, ORDEN);

        if (!hijo.EsHoja)
        {
            nuevoHijo.Hijos.AddRange(hijo.Hijos.GetRange(medio + 1, ORDEN));
            hijo.Hijos.RemoveRange(medio + 1, ORDEN);
        }
    }

    public Factura BuscarPorID(int id)
    {
        return BuscarEnNodo(raiz, id);
    }

    private Factura BuscarEnNodo(NodoArbolB nodo, int id)
    {
        int i = 0;
        while (i < nodo.Facturas.Count && id > nodo.Facturas[i].ID)
            i++;

        if (i < nodo.Facturas.Count && nodo.Facturas[i].ID == id)
            return nodo.Facturas[i];

        if (nodo.EsHoja)
            return null;

        return BuscarEnNodo(nodo.Hijos[i], id);
    }

    public bool ExisteID(int id)
    {
        return BuscarPorID(id) != null;
    }

    public void Mostrar()
    {
        MostrarRecursivo(raiz, 0);
    }

    private void MostrarRecursivo(NodoArbolB nodo, int nivel)
    {
        if (nodo == null)
            return;

        Console.WriteLine(new string(' ', nivel * 4) + nodo);

        foreach (var hijo in nodo.Hijos)
        {
            MostrarRecursivo(hijo, nivel + 1);
        }
    }

    public string GenerarGraphviz()
    {
        using (StringWriter writer = new StringWriter())
        {
            writer.WriteLine("digraph BTree {");
            writer.WriteLine("node [shape=record];");
            GenerarGraphvizRecursivo(raiz, writer);
            writer.WriteLine("}");
            return writer.ToString();
        }
    }

    private void GenerarGraphvizRecursivo(NodoArbolB nodo, TextWriter writer)
    {
        if (nodo == null)
            return;

        string nodeLabel = "\"" + string.Join(" | ", nodo.Facturas) + "\"";
        writer.WriteLine($"{nodo.GetHashCode()} [label={nodeLabel}];");

        for (int i = 0; i < nodo.Hijos.Count; i++)
        {
            writer.WriteLine($"{nodo.GetHashCode()} -> {nodo.Hijos[i].GetHashCode()};");
            GenerarGraphvizRecursivo(nodo.Hijos[i], writer);
        }
    }

    public List<Factura> ObtenerTodas()
    {
        List<Factura> facturas = new List<Factura>();
        ObtenerTodasRecursivo(raiz, facturas);
        return facturas;
    }

    private void ObtenerTodasRecursivo(NodoArbolB nodo, List<Factura> facturas)
    {
        if (nodo == null) return;

        foreach (var factura in nodo.Facturas)
        {
            facturas.Add(factura);
        }

        if (!nodo.EsHoja)
        {
            foreach (var hijo in nodo.Hijos)
            {
                ObtenerTodasRecursivo(hijo, facturas);
            }
        }
    }

    // Método para eliminar una factura
    public bool Eliminar(int idFactura)
    {
        return EliminarRecursivo(raiz, idFactura);
    }

    private bool EliminarRecursivo(NodoArbolB nodo, int idFactura)
    {
        if (nodo == null)
            return false;

        int i = 0;
        while (i < nodo.Facturas.Count && idFactura > nodo.Facturas[i].ID)
            i++;

        if (i < nodo.Facturas.Count && nodo.Facturas[i].ID == idFactura)
        {
            nodo.Facturas.RemoveAt(i);
            return true;
        }

        if (!nodo.EsHoja)
        {
            return EliminarRecursivo(nodo.Hijos[i], idFactura);
        }

        return false;
    }

    /*public bool Eliminar(int id)
    {
        bool eliminado = EliminarRecursivo(raiz, id);
        return eliminado;
    }

    private bool EliminarRecursivo(NodoArbolB nodo, int id)
    {
        int i = 0;
        while (i < nodo.Facturas.Count && id > nodo.Facturas[i].ID)
            i++;

        if (i < nodo.Facturas.Count && nodo.Facturas[i].ID == id)
        {
            nodo.Facturas.RemoveAt(i);
            return true;
        }

        if (nodo.EsHoja)
            return false;

        return EliminarRecursivo(nodo.Hijos[i], id);
    }*/

    public double CalcularTotalFacturas()
    {
        double total = 0;
        foreach (var factura in ObtenerTodas())
        {
            total += factura.Total;
        }
        return total;
    }

    public List<Factura> ObtenerFacturasPorUsuario(int idUsuario)
    {
        List<Factura> facturasUsuario = new List<Factura>();
        foreach (var factura in ObtenerTodas())
        {
            if (factura.ID_Usuario == idUsuario)
            {
                facturasUsuario.Add(factura);
            }
        }
        return facturasUsuario;
    }

}
