using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Foundation;
using SQLite;
using UIKit;
using XamarinSqLite2.iOS.Servicios;
using XamarinSqLite2.Servicios;

[assembly: Xamarin.Forms.Dependency(typeof(SqLiteConexionIOS))]
namespace XamarinSqLite2.iOS.Servicios
{
    public class SqLiteConexionIOS : ISqLiteConexion
    {

        public SQLiteConnection ConexionBaseDatos()
        {
            string nombreBaseDatos = "ListaTareas.db3";
            string rutaFisica = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "..", "Library", nombreBaseDatos);
            return new SQLiteConnection(rutaFisica);
        }
    }
}