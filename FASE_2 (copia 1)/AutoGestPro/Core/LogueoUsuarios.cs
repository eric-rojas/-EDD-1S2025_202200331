using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace AutoGestPro.Core
{
    public class LogEntry
    {
        public string Usuario { get; set; }
        public DateTime Entrada { get; set; }
        public DateTime? Salida { get; set; } 
    }

    public class LogueoUsuarios
    {
        private List<LogEntry> registros;
        private string rutaArchivo;

        public LogueoUsuarios(string ruta = "log_usuarios.json") // Permite ruta personalizada
        {
            rutaArchivo = ruta;
            CargarRegistros();
        }

        public void CargarRegistros()
        {
            if (File.Exists(rutaArchivo))
            {
                string json = File.ReadAllText(rutaArchivo);
                registros = JsonConvert.DeserializeObject<List<LogEntry>>(json) ?? new List<LogEntry>();
            }
            else
            {
                registros = new List<LogEntry>();
            }
        }

        private void GuardarRegistros()

        {
         
            string json = JsonConvert.SerializeObject(registros, Formatting.Indented);
            File.WriteAllText(rutaArchivo, json);
        }

        public void RegistrarEntrada(string usuario)
        {
            registros.Add(new LogEntry { Usuario = usuario, Entrada = DateTime.Now });
            GuardarRegistros();
        }

        public void RegistrarSalida(string usuario)
        {
            var log = registros.FindLast(l => l.Usuario == usuario && l.Salida == null);
            if (log != null)
            {
                log.Salida = DateTime.Now;
                GuardarRegistros();
            }
        }

        public void GenerarJson(string carpetaDestino)
        {
            string rutaCompleta = Path.Combine(carpetaDestino, "log_usuarios.json");

            // Asegurar que la carpeta existe
            Directory.CreateDirectory(carpetaDestino);

            // Guardar el archivo en la carpeta de reportes
            string json = JsonConvert.SerializeObject(registros, Formatting.Indented);
            File.WriteAllText(rutaCompleta, json);

            Console.WriteLine($"✓ Log de usuarios guardado en: {rutaCompleta}");
        }

        public List<LogEntry> ObtenerRegistros()
        {
            return registros;
        }
    }
}
