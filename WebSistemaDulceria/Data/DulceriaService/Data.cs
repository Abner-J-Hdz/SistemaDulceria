using Datos;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebSistemaDulceria.Models.DulceriaModels;

namespace WebSistemaDulceria.Data.DulceriaService
{
    public class Data:IData
    {
        private readonly string connectionString;

        public Data(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("ConexionDulceria");
        }

        public List<ArticuloViewModel> ArticulosVendidos()
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_ObtenerArticulosVendidos", sql))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        var response = new List<ArticuloViewModel>();
                        sql.Open();

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                response.Add(MapToArticulos(reader));
                            }
                        }
                        sql.Close();
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private ArticuloViewModel MapToArticulos(SqlDataReader reader)
        {
            decimal cantidadDecimal = (decimal)reader["Cantidad"];
            decimal cantidad = Math.Round(cantidadDecimal, 2);
            return new ArticuloViewModel()
            {
                IdArticulo = (int)reader["IdArticulo"],
                CantidadVendida = (decimal)reader["Cantidad"],
                Nombre = reader["Nombre"].ToString(),
                CodInterno = reader["CodInterno"].ToString(),
                CodBarra = reader["CodBarra"].ToString(),
                DataLabel = cantidad.ToString() + " - " +  reader["Nombre"].ToString()
            };
        }

    }
}
