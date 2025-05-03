// 📄 BlockUsuario.cs 
using System;
using System.Security.Cryptography;
using System.Text;
using System.Globalization;

namespace AutoGestPro.Core
{
    public class BlockUsuario
    {
        public int Index { get; set; }
        public string Timestamp { get; set; }
        public Usuario DatosUsuario { get; set; }
        public int Nonce { get; set; }
        public string HashAnterior { get; set; }
        public string Hash { get; set; }

        public BlockUsuario(int index, Usuario datosUsuario, string hashAnterior)
        {
            Index = index;
            Timestamp = DateTime.UtcNow.ToString("dd-MM-yyyy::HH:mm:ss", CultureInfo.InvariantCulture);
            DatosUsuario = datosUsuario;
            HashAnterior = hashAnterior;
            Nonce = 0;
        }

        public string GenerarHash()
        {
            string contenido = $"{Index}{Timestamp}{SerializarUsuario()}{Nonce}{HashAnterior}";
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(contenido));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

        public void MinarBloque()
        {
            do
            {
                Hash = GenerarHash();
                Nonce++;
            } while (!Hash.StartsWith("0000"));
        }

        private string SerializarUsuario()
        {
            return $"{DatosUsuario.ID}{DatosUsuario.Nombres}{DatosUsuario.Apellidos}{DatosUsuario.Correo}{DatosUsuario.Edad}{DatosUsuario.Contrasenia}";
        }
    }
}
