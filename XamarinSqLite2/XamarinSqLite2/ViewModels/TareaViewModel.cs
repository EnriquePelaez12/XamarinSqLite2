using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinSqLite2.Modelo;
using XamarinSqLite2.Servicios;

namespace XamarinSqLite2.ViewModels
{
    public class TareaViewModel : BindableObject
    {

        public static readonly BindableProperty HayOperacionesActivasProperty = BindableProperty.Create(
            "HayOperacionesActivas",
            typeof(bool),
            typeof(TareaViewModel),
            false);

        public bool HayOperacionesActivas
        {
            get
            {
                return (bool)GetValue(HayOperacionesActivasProperty);
            }
            set
            {
                SetValue(HayOperacionesActivasProperty, value);
            }
        }

        public static readonly BindableProperty FichaProperty = BindableProperty.Create(
          "Ficha",
          typeof(Tarea),
          typeof(TareaViewModel),
          null);

        public Tarea Ficha
        {
            get
            {
                return (Tarea)GetValue(FichaProperty);
            }
            set
            {
                SetValue(FichaProperty, value);
            }
        }

        private Command m_BorrarTareaComando;
        public Command BorrarTareaComando
        {
            get
            {
                return (m_BorrarTareaComando ?? (m_BorrarTareaComando = new Command(async () => await BorrarTareaAsync())));
            }
        }

        public Task<bool> BorrarTareaAsync()
        {
            return Task<bool>.Run(() =>
            {
                int nErrores = 0;

                try
                {
                    HayOperacionesActivas = true;
                    using (AccesoDatosSqLite dal = new AccesoDatosSqLite())
                    {
                        if (Ficha != null)
                        {
                            if (!dal.EliminarTarea(Ficha.Id))
                            {
                                nErrores++;
                            }
                            else
                            {
                                VolverAtras();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    nErrores++;
                }
                finally
                {
                    HayOperacionesActivas = false;
                }
                return (nErrores == 0);
            });
        }

        private Command m_guardarTareaComando;
        public Command GuardarTareaComando
        {
            get
            {
                return (m_guardarTareaComando ?? (m_guardarTareaComando = new Command(async () => await GuardarTareaAsync())));
            }
        }

        public Task<bool> GuardarTareaAsync()
        {
            return Task<bool>.Run(() =>
            {
                int nErrores = 0;
                try
                {
                    HayOperacionesActivas = true;
                    using (AccesoDatosSqLite dal = new AccesoDatosSqLite())
                    {
                        if (Ficha != null)
                        {
                            if (dal.GuardarTarea(Ficha) <= 0)
                            {
                                nErrores++;
                            }
                            else
                            {
                                VolverAtras();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    nErrores++;
                }
                finally
                {
                    HayOperacionesActivas = false;
                }
                return (nErrores == 0);
            });
        }

        private Command m_finalizarTareaComando;
        public Command FinalizarTareaComando
        {
            get
            {
                return (m_finalizarTareaComando ?? (m_finalizarTareaComando = new Command(async () => await FinalizarTareaAsync())));
            }
        }

        public Task<bool> FinalizarTareaAsync()
        {
            return Task<bool>.Run(() =>
            {
                int nErrores = 0;
                try
                {
                    HayOperacionesActivas = true;
                    using (AccesoDatosSqLite dal = new AccesoDatosSqLite())
                    {
                        if (Ficha != null)
                        {
                            Ficha.Completada = true;
                            if (dal.GuardarTarea(Ficha) <= 0)
                            {
                                nErrores++;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    nErrores++;
                }
                finally
                {
                    HayOperacionesActivas = false;
                }
                return (nErrores == 0);
            });
        }

        private Command m_cancelarTareaComando;

        public Command CancelarTareaComando
        {
            get
            {
                return (m_cancelarTareaComando ?? (m_cancelarTareaComando = new Command(() => VolverAtras())));
            }
        }

        public bool VolverAtras()
        {
            int nErrores = 0;

            try
            {
                ServicioNavegacion.Instancia.NavigateBack();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                nErrores++;
            }
            return (nErrores == 0);
        }

        private Command m_mostrarTareaComando;
        public Command MostrarTareaComando
        {
            get
            {
                return (m_mostrarTareaComando ?? (m_mostrarTareaComando = new Command(async () => await MostrarTareaAsync())));
            }
        }
        async public Task<bool> MostrarTareaAsync()
        {
            TareaViewModel vm = null;
            int nErrores = 0;
            try
            {
                vm = new TareaViewModel()
                {
                    Ficha = new Tarea()
                    {
                        Descripcion = Ficha.Descripcion,
                        Completada = Ficha.Completada,
                        Id = Ficha.Id
                    }
                };

                await ServicioNavegacion.Instancia.NavigateTo<TareaViewModel>(vm);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                nErrores++;
            }
            return (nErrores == 0);
        }
    }
}
