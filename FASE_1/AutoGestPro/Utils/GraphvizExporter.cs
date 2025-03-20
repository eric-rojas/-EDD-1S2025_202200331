using System;
using System.Diagnostics;
using System.IO;

namespace AutoGestPro.Utils
{
    public static class GraphvizExporter
    {
        public static void GenerarArchivoDot(string nombre, string contenido)
        {
            try
            {
                // Crear la carpeta Reports si no existe
                string carpeta = Path.Combine(Directory.GetCurrentDirectory(), "Reports");
                if (!Directory.Exists(carpeta))
                {
                    Directory.CreateDirectory(carpeta);
                    Console.WriteLine("Carpeta 'Reports' creada exitosamente.");
                }

                // Validar el nombre del archivo
                if (string.IsNullOrEmpty(nombre))
                {
                    Console.WriteLine("Error: El nombre del archivo no puede ser nulo o vacío.");
                    return;
                }

                // Asegurar que el archivo tenga extensión .dot
                if (!nombre.EndsWith(".dot"))
                {
                    nombre += ".dot";
                }

                // Generar la ruta completa del archivo
                string rutaArchivo = Path.Combine(carpeta, nombre);
                File.WriteAllText(rutaArchivo, contenido);
                Console.WriteLine($"✓ Archivo DOT generado exitosamente en: {rutaArchivo}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al generar el archivo DOT: {ex.Message}");
            }
        }

        public static void ConvertirDotAPng(string nombreArchivo)
        {
            try
            {
                // Validar que el nombre no sea nulo
                if (string.IsNullOrEmpty(nombreArchivo))
                {
                    Console.WriteLine("Error: El nombre del archivo no puede ser nulo o vacío.");
                    return;
                }

                // Asegurar que el archivo tenga extensión .dot
                if (!nombreArchivo.EndsWith(".dot"))
                {
                    nombreArchivo += ".dot";
                }

                string carpeta = Path.Combine(Directory.GetCurrentDirectory(), "Reports");
                string rutaDot = Path.Combine(carpeta, nombreArchivo);
                string rutaPng = Path.ChangeExtension(rutaDot, ".png");

                // Verificar que el archivo .dot exista
                if (!File.Exists(rutaDot))
                {
                    Console.WriteLine($"❌ Error: El archivo {nombreArchivo} no existe en la carpeta 'Reports'.");
                    return;
                }

                // Configurar el proceso para ejecutar Graphviz
                var processInfo = new ProcessStartInfo
                {
                    FileName = "dot",
                    Arguments = $"-Tpng \"{rutaDot}\" -o \"{rutaPng}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                // Ejecutar el proceso
                using (var proceso = Process.Start(processInfo))
                {
                    if (proceso == null)
                    {
                        Console.WriteLine("❌ Error: No se pudo iniciar el proceso de conversión.");
                        return;
                    }

                    proceso.WaitForExit();

                    if (proceso.ExitCode == 0)
                    {
                        Console.WriteLine($"✓ Imagen PNG generada exitosamente en: {rutaPng}");
                    }
                    else
                    {
                        string error = proceso.StandardError.ReadToEnd();
                        Console.WriteLine($"❌ Error al convertir a PNG: {error}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en el proceso de conversión: {ex.Message}");
            }
        }

        // Método de conveniencia que genera tanto el DOT como el PNG
        public static void GenerarGrafo(string nombre, string contenido)
        {
            try
            {
                Console.WriteLine($"=== Generando gráfico para: {nombre} ===");
                
                // Generar el archivo DOT
                GenerarArchivoDot(nombre, contenido);
                
                // Intentar convertir a PNG
                ConvertirDotAPng(nombre);
                
                // Verificar que ambos archivos existan
                string carpeta = Path.Combine(Directory.GetCurrentDirectory(), "Reports");
                string rutaDot = Path.Combine(carpeta, $"{nombre}.dot");
                string rutaPng = Path.Combine(carpeta, $"{nombre}.png");
                
                Console.WriteLine($"Verificando archivos generados:");
                Console.WriteLine($"DOT: {(File.Exists(rutaDot) ? "Existe" : "No existe")}");
                Console.WriteLine($"PNG: {(File.Exists(rutaPng) ? "Existe" : "No existe")}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en GenerarGrafo: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                throw;
            }
        }
    }
}