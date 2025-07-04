using FondoXYZ.web.Data;
using FondoXYZ.web.Services.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

public class TarifaService(ApplicationDbContext context, ILogger<TarifaService> logger) : ITarifaService
{
    public async Task<decimal> ConsultarTarifaAsync(int alojamientoId, DateTime fechaInicio, DateTime fechaFin, int numPersonas, bool servicioLavanderia)
    {
        var query = "EXEC sp_ConsultarTarifas @AlojamientoId, @FechaInicio, @FechaFin, @NumPersonas, @ServicioLavanderia";

        // Crear la conexión
        using var command = context.Database.GetDbConnection().CreateCommand();
        command.CommandText = query;

        command.Parameters.Add(new SqlParameter("@AlojamientoId", alojamientoId));
        command.Parameters.Add(new SqlParameter("@FechaInicio", fechaInicio));
        command.Parameters.Add(new SqlParameter("@FechaFin", fechaFin));
        command.Parameters.Add(new SqlParameter("@NumPersonas", numPersonas));
        command.Parameters.Add(new SqlParameter("@ServicioLavanderia", servicioLavanderia));

        try
        {
            if (command.Connection.State != ConnectionState.Open)
            {
                await command.Connection.OpenAsync();
            }

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var precioTotal = reader.IsDBNull(reader.GetOrdinal("PrecioTotal")) ? 0 : reader.GetDecimal(reader.GetOrdinal("PrecioTotal"));
                return precioTotal;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error al ejecutar el procedimiento ");
        }

        return 0;
    }
}



