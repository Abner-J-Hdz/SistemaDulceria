using Datos;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebSistemaDulceria.Models.DulceriaModels;
using WebSistemaDulceria.Models.DulceriaModels.Graficos;

namespace WebSistemaDulceria.Data.DulceriaService
{
    public class Data:IData
    {
        private readonly string connectionString;

        public Data(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("ConexionDulceria");
        }

        public List<ArticuloViewModel> ArticulosVendidos(DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_ObtenerArticulosVendidos", sql))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@FECHAINICIO", fechaInicio == null ? DateTime.Now : fechaInicio));
                        cmd.Parameters.Add(new SqlParameter("@FECHAFIN", fechaFin == null ? DateTime.Now : fechaFin ));

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

        public List<ClientesMasCompras> ClientesMasCompras(DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                using (SqlConnection sql = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_ObtenerTopCliente", sql))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@FECHAINICIO", fechaInicio == null ? DateTime.Now : fechaInicio));
                        cmd.Parameters.Add(new SqlParameter("@FECHAFIN", fechaFin == null ? DateTime.Now : fechaFin));

                        var response = new List<ClientesMasCompras>();

                        sql.Open();

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                response.Add(MapToclientes(reader));
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
        private ClientesMasCompras MapToclientes(SqlDataReader reader)
        {
            decimal total = (decimal)reader["Total"];
            decimal TotalVenta = Math.Round(total, 2);

            return new ClientesMasCompras()
            {
                Nombre = reader["Nombre"] == DBNull.Value ? "" : reader["Nombre"].ToString(),
                Correo = reader["Correo"] == DBNull.Value ? "" : reader["Correo"].ToString(),
                Total = TotalVenta,
                NumeroVentas = reader["NumeroVentas"] == DBNull.Value ? 0 : (int)reader["NumeroVentas"]
            };
        }


    }
}
