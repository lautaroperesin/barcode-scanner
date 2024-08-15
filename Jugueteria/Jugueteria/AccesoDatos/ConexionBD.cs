
using Jugueteria.Modelo;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Xamarin.Forms;

namespace Jugueteria.AccesoDatos
{
    public class ConexionBD : IDisposable
    {
        private string connectionString = "Server=192.168.0.117;" +
                                          "Uid=user_jugueteria;" +
                                          "Pwd=12345;" +
                                          "Database=ventas";
        private MySqlConnection connection;
        private bool disposed = false;
        public ConexionBD()
        {
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();
            }
            catch (MySqlException ex) 
            {
                MostrarAlerta("Error en la conexion a la base de datos: ", ex.Message);
            }
        }

        public Producto ObtenerProductoPorCodigo(string codigo)
        {
            Producto producto = null;
            try
            {
                string query = "SELECT producto_nombre, producto_precio_venta FROM producto WHERE producto_codigo = @codigo";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@codigo", codigo);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            producto = new Producto
                            {
                                producto_nombre = reader.GetString("producto_nombre"),
                                producto_precio_venta = reader.GetDecimal("producto_precio_venta"),
                            };
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                MostrarAlerta("Error en la consulta a la base de datos: ", ex.Message);
            }
            catch (Exception ex)
            {
                MostrarAlerta("Error: ", ex.Message);
            }
            return producto;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (connection != null)
                    {
                        connection.Close();
                        connection.Dispose();
                    }
                }
                disposed = true;
            }
        }
        ~ConexionBD()
        {
            Dispose(false);
        }
        private async void MostrarAlerta(string titulo, string mensaje)
        {
            await Application.Current.MainPage.DisplayAlert(titulo, mensaje, "OK");
        }
    }
}
