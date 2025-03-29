using Gtk;
using System;
using System.IO;

namespace AutoGestPro.Utils
{
    public static class ErrorHandler
    {
        private static readonly string LogFilePath = "error_log.txt";

        /// <summary>
        /// Registra un error en el archivo de log
        /// </summary>
        public static void LogError(string clase, string metodo, Exception ex)
        {
            try
            {
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string mensaje = $"{timestamp} - [{clase}.{metodo}] - {ex.Message}\n{ex.StackTrace}\n\n";
                
                File.AppendAllText(LogFilePath, mensaje);
                Console.WriteLine($"ERROR: {clase}.{metodo}: {ex.Message}");
            }
            catch
            {
                // Si falla el registro, al menos mostramos en consola
                Console.WriteLine($"ERROR NO REGISTRADO: {ex.Message}");
            }
        }

        /// <summary>
        /// Muestra un diálogo de error
        /// </summary>
        public static void MostrarError(Window parent, string mensaje)
        {
            try
            {
                MessageDialog dialog = new MessageDialog(
                    parent,
                    DialogFlags.Modal,
                    MessageType.Error,
                    ButtonsType.Ok,
                    mensaje);
                
                dialog.Run();
                dialog.Destroy();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al mostrar diálogo: {ex.Message}");
            }
        }

        /// <summary>
        /// Muestra un diálogo informativo
        /// </summary>
        public static void MostrarInfo(Window parent, string mensaje)
        {
            try
            {
                MessageDialog dialog = new MessageDialog(
                    parent,
                    DialogFlags.Modal,
                    MessageType.Info,
                    ButtonsType.Ok,
                    mensaje);
                
                dialog.Run();
                dialog.Destroy();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al mostrar diálogo: {ex.Message}");
            }
        }
    }
}