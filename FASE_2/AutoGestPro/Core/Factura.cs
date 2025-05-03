// 📄 Factura.cs
using System;
using System.Security.Cryptography;
using System.Text;

namespace AutoGestPro.Core
{
    public class Factura
    {
        public int ID { get; set; }
        public int ID_Servicio { get; set; }
        public double Total { get; set; }
        public string Fecha { get; set; }
        public string MetodoPago { get; set; }

        public Factura(int id, int idServicio, double total, string fecha, string metodoPago)
        {
            ID = id;
            ID_Servicio = idServicio;
            Total = total;
            Fecha = fecha;
            MetodoPago = metodoPago;
        }

        public string GetHash()
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                string datos = $"{ID}{ID_Servicio}{Total}{Fecha}{MetodoPago}";
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(datos));
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }

        public override string ToString()
        {
            return $"Factura #{ID} | Servicio: {ID_Servicio} | Total: Q{Total:0.00} | Fecha: {Fecha} | Método: {MetodoPago}";
        }
    }
}
