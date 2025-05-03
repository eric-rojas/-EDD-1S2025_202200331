// 📄 BlockchainUsuarios.cs 
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using Newtonsoft.Json;
using AutoGestPro.Core;

namespace AutoGestPro.Core
{
    public class BlockchainUsuarios
    {
        public List<BlockUsuario> Cadena { get; private set; }

        public BlockchainUsuarios()
        {
            Cadena = new List<BlockUsuario>();
            var bloqueGenesis = new BlockUsuario(0, new Usuario(0, "Genesis", "Block", "", 0, ""), "0000");
            bloqueGenesis.MinarBloque();
            Cadena.Add(bloqueGenesis);
        }

        public bool Insertar(Usuario nuevoUsuario)
        {
            if (ExisteID(nuevoUsuario.ID) || ExisteCorreo(nuevoUsuario.Correo)) return false;
            nuevoUsuario.Contrasenia = EncriptarSHA256(nuevoUsuario.Contrasenia);
            int nuevoIndex = Cadena[Cadena.Count - 1].Index + 1;
            string hashAnterior = Cadena[Cadena.Count - 1].Hash;

            var nuevoBloque = new BlockUsuario(nuevoIndex, nuevoUsuario, hashAnterior);
            nuevoBloque.MinarBloque();
            Cadena.Add(nuevoBloque);
            return true;
        }

        public Usuario BuscarPorCorreo(string correo)
        {
            foreach (var bloque in Cadena)
            {
                if (bloque.DatosUsuario.Correo.Equals(correo, StringComparison.OrdinalIgnoreCase))
                    return bloque.DatosUsuario;
            }
            return null;
        }

        public Usuario Buscar(int id)
        {
            foreach (var bloque in Cadena)
            {
                if (bloque.DatosUsuario.ID == id)
                    return bloque.DatosUsuario;
            }
            return null;
        }

        public bool ExisteID(int id)
        {
            return Buscar(id) != null;
        }

        public bool ExisteCorreo(string correo)
        {
            return BuscarPorCorreo(correo) != null;
        }

        public bool Editar(int id, string nuevosNombres, string nuevosApellidos, string nuevoCorreo, int nuevaEdad, string nuevaContrasenia)
        {
            foreach (var bloque in Cadena)
            {
                if (bloque.DatosUsuario.ID == id)
                {
                    foreach (var otro in Cadena)
                    {
                        if (otro.DatosUsuario.ID != id && otro.DatosUsuario.Correo.Equals(nuevoCorreo, StringComparison.OrdinalIgnoreCase))
                            return false;
                    }
                    bloque.DatosUsuario.Nombres = nuevosNombres;
                    bloque.DatosUsuario.Apellidos = nuevosApellidos;
                    bloque.DatosUsuario.Correo = nuevoCorreo;
                    bloque.DatosUsuario.Edad = nuevaEdad;
                    bloque.DatosUsuario.Contrasenia = EncriptarSHA256(nuevaContrasenia);
                    return true;
                }
            }
            return false;
        }

        public static string EncriptarSHA256(string texto)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(texto));
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }

        public bool ValidarCadena()
        {
            for (int i = 1; i < Cadena.Count; i++)
            {
                var actual = Cadena[i];
                var anterior = Cadena[i - 1];

                if (actual.Hash != actual.GenerarHash() || actual.HashAnterior != anterior.Hash || !actual.Hash.StartsWith("0000"))
                    return false;
            }
            return true;
        }

        public List<Usuario> ObtenerTodos()
        {
            List<Usuario> usuarios = new List<Usuario>();
            foreach (var bloque in Cadena)
            {
                if (bloque.DatosUsuario.ID > 0)
                    usuarios.Add(bloque.DatosUsuario);
            }
            return usuarios;
        }

        public void ExportarJSON(string nombreArchivo)
        {
            string carpeta = "./Reportes";
            Directory.CreateDirectory(carpeta);
            string ruta = Path.Combine(carpeta, nombreArchivo + ".json");

            var listaExportada = new List<object>();
            foreach (var bloque in Cadena)
            {
                listaExportada.Add(new
                {
                    bloque.Index,
                    bloque.Timestamp,
                    Usuario = bloque.DatosUsuario,
                    bloque.Nonce,
                    bloque.HashAnterior,
                    bloque.Hash
                });
            }
            File.WriteAllText(ruta, JsonConvert.SerializeObject(listaExportada, Formatting.Indented));
        }
    }
}
