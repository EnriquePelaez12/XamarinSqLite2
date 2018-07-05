using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;
using XamarinSqLite2.Servicios;

namespace XamarinSqLite2.Modelo
{
    public class AccesoDatosSqLite : IDisposable
    {
        private SQLiteConnection m_conexion;
        private static object m_ControlBloqueo;
        static AccesoDatosSqLite()
        {
            m_ControlBloqueo = new object();
        }

        public AccesoDatosSqLite()
        {
            try
            {
                m_conexion = DependencyService.Get<ISqLiteConexion>().ConexionBaseDatos();
                m_conexion.CreateTable<Tarea>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public List<Tarea> Tareas { get; set; }

        public List<Tarea> ObtenerTareas()
        {
            List<Tarea> resultado = null;
            if (m_conexion != null)
            {
                lock (m_ControlBloqueo)
                {
                    try
                    {
                        resultado = new List<Tarea>(m_conexion.Table<Tarea>());
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }
            }

            return resultado;
        }

        public List<Tarea> ObtenerTareasPendientes()
        {
            List<Tarea> resultado = null;
            if (m_conexion != null)
            {
                lock (m_ControlBloqueo)
                {
                    try
                    {
                        IEnumerable<Tarea> tareasFiltradas = from t in m_conexion.Table<Tarea>()
                                                             where t.Completada == false
                                                             select t;

                        resultado = new List<Tarea>(tareasFiltradas);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }
            }

            return resultado;
        }

        public List<Tarea> ObtenerTareaId(int id)
        {
            List<Tarea> resultado = null;
            if (m_conexion != null)
            {
                lock (m_ControlBloqueo)
                {
                    try
                    {
                        IEnumerable<Tarea> tareasFiltradas = from t in m_conexion.Table<Tarea>()
                                                             where t.Id == id
                                                             select t;

                        resultado = new List<Tarea>(tareasFiltradas);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }

            }

            return resultado;
        }
        public int GuardarTarea(Tarea tarea)
        {
            int id = 0;
            int numReg = 0;
            int nErrores = 0;
            if ((m_conexion != null) && (tarea != null))
            {
                try
                {
                    if (tarea.Id != 0)
                    {
                        lock (m_ControlBloqueo)
                        {
                            numReg = m_conexion.Update(tarea);
                        }

                        if (numReg == 0)
                        {
                            nErrores++;
                        }
                        else
                        {
                            id = tarea.Id;
                        }
                    }
                    else
                    {
                        lock (m_ControlBloqueo)
                        {
                            numReg = m_conexion.Insert(tarea);
                        }

                        if (numReg == 0)
                        {
                            nErrores++;
                        }
                        else
                        {
                            id = tarea.Id;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }

            if (nErrores == 0)
            {
                return id;
            }
            else
            {
                return 0;
            }
        }

        public bool EliminarTarea(int id)
        {
            int numReg = 0;
            int nErrores = 0;
            if ((m_conexion != null) && (id > 0))
            {
                lock (m_ControlBloqueo)
                {
                    try
                    {
                        numReg = m_conexion.Delete<Tarea>(id);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }

                if (numReg == 0)
                {
                    nErrores++;
                }
            }

            if (nErrores == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool LimpiarTablaTareas()
        {
            int numReg = 0;
            int nErrores = 0;
            if (m_conexion != null)
            {
                lock (m_ControlBloqueo)
                {
                    try
                    {
                        numReg = m_conexion.DeleteAll<Tarea>();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }
            }

            if (nErrores == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool GuardarTareas(IEnumerable<Tarea> tareas)
        {
            int nErrores = 0;
            int id = 0;
            if ((m_conexion != null) && (tareas != null))
            {
                foreach (Tarea tarea in tareas)
                {
                    id = GuardarTarea(tarea);

                    if (id == 0)
                    {
                        nErrores++;
                    }
                }
            }

            return (nErrores == 0);
        }

        #region IDisposable Support
        private bool disposedValue = false; // Para detectar llamadas redundantes

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (m_conexion != null)
                    {
                        m_conexion.Dispose();
                        m_conexion = null;
                    }
                }

                // TODO: libere los recursos no administrados (objetos no administrados) y reemplace el siguiente finalizador.
                // TODO: configure los campos grandes en nulos.

                disposedValue = true;
            }
        }

        // TODO: reemplace un finalizador solo si el anterior Dispose(bool disposing) tiene código para liberar los recursos no administrados.
        // ~AccesoDatosSqLite() {
        //   // No cambie este código. Coloque el código de limpieza en el anterior Dispose(colocación de bool).
        //   Dispose(false);
        // }

        // Este código se agrega para implementar correctamente el patrón descartable.
        public void Dispose()
        {
            // No cambie este código. Coloque el código de limpieza en el anterior Dispose(colocación de bool).
            Dispose(true);
            // TODO: quite la marca de comentario de la siguiente línea si el finalizador se ha reemplazado antes.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
