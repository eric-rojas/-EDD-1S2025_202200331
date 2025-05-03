// 📄 NodoGrafo.cs
namespace AutoGestPro.Core.Estructuras
{
    public class NodoGrafo
    {
        public int ID { get; set; }
        public string Tipo { get; set; } // "Vehiculo" o "Repuesto"

        public NodoGrafo(int id, string tipo)
        {
            ID = id;
            Tipo = tipo;
        }

        public override bool Equals(object obj)
        {
            return obj is NodoGrafo otro && ID == otro.ID && Tipo == otro.Tipo;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ID, Tipo);
        }

        public override string ToString()
        {
            return $"{Tipo.Substring(0, 1)}{ID}";
        }
    }
}

