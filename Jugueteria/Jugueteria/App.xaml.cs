using Jugueteria.AccesoDatos;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Jugueteria
{
    public partial class App : Application
    {
        public static ConexionBD ConexionBD { get; private set; }
        public App()
        {
            InitializeComponent();
            MainPage = new MainPage();
        }
        protected override void OnStart()
        {
            ConexionBD = new ConexionBD();
        }

        protected override void OnSleep()
        {
            ConexionBD.Dispose();
        }

        protected override void OnResume()
        {
            ConexionBD = new ConexionBD();
        }
    }
}
