using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using XamarinSqLite2.Droid.Servicios;
using XamarinSqLite2.Servicios;

[assembly: Xamarin.Forms.Dependency(typeof(SqLiteConexionAndroid))]
namespace XamarinSqLite2.Droid.Servicios
{
    public class SqLiteConexionAndroid: ISqLiteConexion
    {
        public SQLiteConnection ConexionBaseDatos()
        {
            string nombreBaseDatos = "ListaTareas.db3";
            string rutaFisica = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), nombreBaseDatos);
            return new SQLiteConnection(rutaFisica);
        }

    }
}