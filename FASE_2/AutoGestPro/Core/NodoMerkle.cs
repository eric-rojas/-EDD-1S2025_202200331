// 📄 NodoMerkle.cs
using System.Security.Cryptography;
using System.Text;
using AutoGestPro.Core;

namespace AutoGestPro.Core.Estructuras
{
    public class NodoMerkle
    {
        public string Hash { get; set; }
        public NodoMerkle Left { get; set; }
        public NodoMerkle Right { get; set; }
        public Factura Factura { get; set; }

        public NodoMerkle(Factura factura)
        {
            Factura = factura;
            Hash = factura.GetHash();
        }

        public NodoMerkle(NodoMerkle left, NodoMerkle right)
        {
            Left = left;
            Right = right;
            Factura = null;
            Hash = CalcularHash(left.Hash, right?.Hash);
        }

        private string CalcularHash(string hashIzq, string hashDer)
        {
            string combinado = hashIzq + (hashDer ?? hashIzq);
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combinado));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                    builder.Append(b.ToString("x2"));
                return builder.ToString();
            }
        }
    }
}