/*using System;
using System.Runtime.InteropServices;

namespace AutoGestPro.Core
{
    public unsafe class GeneradorServicio
    {
        private readonly ListaVehiculos _vehiculos;
        //private readonly ListaRepuestos _repuestos;
        //private readonly ColaServicios _servicios;
        //private readonly PilaFacturas _facturas;
        private readonly MatrizBitacora _bitacora;
        private int _contadorIDServicio;

        public GeneradorServicio(
            ListaVehiculos vehiculos,
            ListaRepuestos repuestos,
            ColaServicios servicios,
            PilaFacturas facturas,
            MatrizBitacora bitacora)
        {
            _vehiculos = vehiculos;
            _repuestos = repuestos;
            _servicios = servicios;
            _facturas = facturas;
            _bitacora = bitacora;
            _contadorIDServicio = 1;
        }

        public bool GenerarNuevoServicio(int idVehiculo, int idRepuesto, string detalles, float costoServicio)
        {
            try
            {
                // 1. Validar existencia de vehículo y repuesto
                var vehiculo = _vehiculos.Buscar(idVehiculo);
                if (vehiculo == null)
                {
                    throw new ServicioException($"El vehículo con ID {idVehiculo} no existe.");
                }

                var repuesto = _repuestos.Buscar(idRepuesto);
                if (repuesto == null)
                {
                    throw new ServicioException($"El repuesto con ID {idRepuesto} no existe.");
                }

                // 2. Validar el detalle del servicio y costo
                if (string.IsNullOrEmpty(detalles))
                {
                    throw new ServicioException("Los detalles del servicio son requeridos.");
                }

                if (costoServicio <= 0)
                {
                    throw new ServicioException("El costo del servicio debe ser mayor a 0.");
                }

                // 3. Calcular costo total (costo del servicio + costo del repuesto)
                float costoTotal = (float)(costoServicio + (float)repuesto.Costo);

                // 4. Crear y encolar el servicio
                _servicios.Encolar(
                    _contadorIDServicio,
                    idRepuesto,
                    idVehiculo,
                    detalles,
                    costoServicio
                );

                // 5. Generar factura
                GenerarFactura(_contadorIDServicio, costoTotal);

                // 6. Actualizar bitácora
                ActualizarBitacora(idRepuesto, idVehiculo, detalles);

                // 7. Incrementar el contador de servicios
                _contadorIDServicio++;

                return true;
            }
            catch (ServicioException ex)
            {
                Console.WriteLine($"Error al generar servicio: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado: {ex.Message}");
                return false;
            }
        }

        private void GenerarFactura(int idServicio, float costoTotal)
        {
            try
            {
                _facturas.Apilar(idServicio, costoTotal);
            }
            catch (Exception ex)
            {
                throw new ServicioException($"Error al generar la factura: {ex.Message}");
            }
        }

        private void ActualizarBitacora(int idRepuesto, int idVehiculo, string detalles)
        {
            try
            {
                _bitacora.InsertarRelacion(idRepuesto, idVehiculo, detalles);
            }
            catch (Exception ex)
            {
                throw new ServicioException($"Error al actualizar la bitácora: {ex.Message}");
            }
        }

        public void CancelarFactura()
        {
            try
            {
                if (_facturas.MostrarTope())
                {
                    _facturas.Desapilar();
                    Console.WriteLine("Factura cancelada exitosamente.");
                }
                else
                {
                    Console.WriteLine("No hay facturas pendientes para cancelar.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cancelar la factura: {ex.Message}");
            }
        }
    }

    public class ServicioException : Exception
    {
        public ServicioException(string message) : base(message) { }
    }
}*/


using System;

namespace AutoGestPro.Core
{
    public class GeneradorServicio
    {
        private readonly ListaVehiculos _vehiculos;
        private readonly ArbolBinarioServicios _servicios;
        private readonly ArbolBFacturas _facturas;
        private readonly ArbolAVLRepuestos _repuestos;
        private int _contadorIDServicio;

        

        public GeneradorServicio(
            ListaVehiculos vehiculos,
            ArbolBinarioServicios servicios,
            ArbolAVLRepuestos repuestos,
            ArbolBFacturas facturas)
        {
            _vehiculos = vehiculos;
            _servicios = servicios;
            _repuestos = repuestos;
            _facturas = facturas;
            _contadorIDServicio = 1;
        }

        public bool GenerarNuevoServicio(int idVehiculo, int idRepuesto, string detalles, float costoServicio)
        {
            try
            {
            // 1. Validar existencia del vehículo
            var vehiculo = _vehiculos.Buscar(idVehiculo);
            if (vehiculo == null)
            {
                throw new ServicioException($"El vehículo con ID {idVehiculo} no existe.");
            }

            // 2. Validar existencia del repuesto
            var repuesto = _repuestos.Buscar(idRepuesto);
            if (repuesto == null)
            {
                throw new ServicioException($"El repuesto con ID {idRepuesto} no existe.");
            }

            // 3. Validar detalles y costo
            if (string.IsNullOrEmpty(detalles))
            {
                throw new ServicioException("Los detalles del servicio son requeridos.");
            }
            if (costoServicio <= 0)
            {
                throw new ServicioException("El costo del servicio debe ser mayor a 0.");
            }

            // 4. Calcular costo total (servicio + repuesto)
            float costoTotal = costoServicio + (float)repuesto.Costo;

            // 5. Crear e insertar el servicio en el árbol binario
            var nuevoServicio = new Servicio(
                _contadorIDServicio,     // ID del servicio
                idRepuesto,              // ID del repuesto
                idVehiculo,              // ID del vehículo
                detalles,                // Detalles del servicio
                (double)costoTotal       // Costo total (servicio + repuesto)
            );
            _servicios.Insertar(nuevoServicio);

            // 6. Generar factura
            int idUsuario = 1; // You should pass the actual user ID here
            GenerarFactura(_contadorIDServicio, costoTotal, idUsuario);

            // 7. Incrementar el contador de servicios
            _contadorIDServicio++;

            return true;
            }
            catch (ServicioException ex)
            {
            Console.WriteLine($"Error al generar servicio: {ex.Message}");
            return false;
            }
            catch (Exception ex)
            {
            Console.WriteLine($"Error inesperado: {ex.Message}");
            return false;
            }
        }

        private void GenerarFactura(int idServicio, float costoTotal, int idUsuario)
        {
            try
            {
                // Crear la nueva factura, ahora pasando el idUsuario
                var nuevaFactura = new Factura(idServicio, idServicio, costoTotal, idUsuario);
                
                // Insertar la factura en el árbol B
                _facturas.Insertar(nuevaFactura);
            }
            catch (Exception ex)
            {
                throw new ServicioException($"Error al generar la factura: {ex.Message}");
            }
        }

    }

    public class ServicioException : Exception
    {
        public ServicioException(string message) : base(message) { }
    }
}
