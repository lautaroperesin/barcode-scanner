using Jugueteria.AccesoDatos;
using Jugueteria.Modelo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Xamarin.Forms;

namespace Jugueteria
{
    public partial class MainPage : ContentPage
    {
        public static ConexionBD conexionBD;
        private const int TiempoEspera = 10000;
        private const int TiempoEsperaTextChanged = 1500;
        private System.Timers.Timer temporizador;
        private System.Timers.Timer temporizadorTextChanged;
        private bool productoEscaneado;

        public MainPage()
        {
            InitializeComponent();
            conexionBD = new ConexionBD();
            IniciarTemporizador();
            IniciarTemporizadorTextChanged();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            Device.BeginInvokeOnMainThread(async () =>
            {
                await Task.Delay(100);
                EntradaEscaner.Focus();
            });
        }

        public void EntradaEscaner_Completed(object sender, EventArgs e)
        {
            ProcesarEntrada();
        }

        private void EntradaEscaner_TextChanged(object sender, TextChangedEventArgs e)
        {
            ReiniciarTemporizadorTextChanged();
        }

        public void MostrarProducto(string codigo)
        {

            Producto producto = App.ConexionBD.ObtenerProductoPorCodigo(codigo);
            if (producto != null)
            {
                NombreProducto.Text = producto.producto_nombre;
                PrecioProducto.Text = $"$ {producto.producto_precio_venta}";
                ImagenLogo.IsVisible = false;
                InfoProducto.IsVisible = true;
            }
            else
            {
                NombreProducto.Text = "PRODUCTO NO ENCONTRADO";
                PrecioProducto.Text = string.Empty;
                ImagenLogo.IsVisible = false;
                InfoProducto.IsVisible = true;
            }
        }

        private void ProcesarEntrada()
        {
            string codigo = EntradaEscaner.Text;
            if (!string.IsNullOrWhiteSpace(codigo))
            {
                MostrarProducto(codigo);
                EntradaEscaner.Text = string.Empty;
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Task.Delay(2000);
                    EntradaEscaner.Focus();
                });
                productoEscaneado = true;
                ReiniciarTemporizador();
            }
        }
        private void IniciarTemporizador()
        {
            temporizador = new System.Timers.Timer(TiempoEspera);
            temporizador.Elapsed += (sender, e) => Temporizador_Elapsed();
            temporizador.AutoReset = false;
            temporizador.Start();
        }

        private void ReiniciarTemporizador()
        {
            if (temporizador != null)
            {
                temporizador.Stop();
                temporizador.Start();
                productoEscaneado = true;
            }
        }

        private void Temporizador_Elapsed()
        {
            if (!productoEscaneado)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    InfoProducto.IsVisible = false;
                    ImagenLogo.IsVisible = true;
                });
            }
            productoEscaneado = false;
            temporizador.Start();
        }

        private void IniciarTemporizadorTextChanged()
        {
            temporizadorTextChanged = new System.Timers.Timer(TiempoEsperaTextChanged);
            temporizadorTextChanged.Elapsed += (sender, e) => TemporizadorTextChanged_Elapsed();
            temporizadorTextChanged.AutoReset = false;
        }

        private void ReiniciarTemporizadorTextChanged()
        {
            if (temporizadorTextChanged != null)
            {
                temporizadorTextChanged.Stop();
                temporizadorTextChanged.Start();
            }
        }

        private void TemporizadorTextChanged_Elapsed()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                ProcesarEntrada();
            });
        }
    }
}
