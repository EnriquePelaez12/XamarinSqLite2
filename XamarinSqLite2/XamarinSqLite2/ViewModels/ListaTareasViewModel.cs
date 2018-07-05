using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinSqLite2.Modelo;
using XamarinSqLite2.Servicios;

namespace XamarinSqLite2.ViewModels
{
    public class ListaTareasViewModel : BindableObject
    {
        public static readonly BindableProperty HayOperacionesActivasProperty = BindableProperty.Create(
          "HayOperacionesActivas",
          typeof(bool),
          typeof(ListaTareasViewModel),
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

        public static readonly BindableProperty MostrarTodasProperty = BindableProperty.Create(
          "MostrarTodas",
          typeof(bool),
          typeof(ListaTareasViewModel),
          false);

        public bool MostrarTodas
        {
            get
            {
                return (bool)GetValue(MostrarTodasProperty);
            }
            set
            {
                SetValue(MostrarTodasProperty, value);
                CargarTareasAsync();
            }
        }

        public static readonly BindableProperty TareaSeleccionadaProperty = BindableProperty.Create(
          "TareaSeleccionada",
          typeof(Tarea),
          typeof(ListaTareasViewModel),
          null);

        public Tarea TareaSeleccionada
        {
            get
            {
                return (Tarea)GetValue(TareaSeleccionadaProperty);
            }
            set
            {
                SetValue(TareaSeleccionadaProperty, value);
                if (value != null)
                {
                    MostrarTareaComando.Execute(null);
                }
            }
        }

        private ObservableCollection<TareaViewModel> m_Tareas;

        public ObservableCollection<TareaViewModel> Tareas
        {
            get
            {
                return m_Tareas;
            }
            set
            {
                m_Tareas = value;
                OnPropertyChanged("Tareas");
            }
        }

        private Command m_actualizarComando;
        public Command ActualizarComando
        {
            get
            {
                return (m_actualizarComando ?? (m_actualizarComando = new Command(async () => await ActualizarAsync())));
            }
        }

        async public Task ActualizarAsync()
        {
            int nErrores = 0;

            try
            {
                await CargarTareasAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                nErrores++;
            }
        }

        private Command m_nuevaTareaComando;
        public Command NuevaTareaComando
        {
            get
            {
                return (m_nuevaTareaComando ?? (m_nuevaTareaComando = new Command(async () => await NuevaTareaAsync())));
            }
        }

        async public Task<bool> NuevaTareaAsync()
        {
            int nErrores = 0;
            TareaViewModel contexto = null;
            Tarea nuevaTarea = null;
            try
            {
                nuevaTarea = new Tarea() { Descripcion = "", Completada = false, Id = 0 };
                contexto = new TareaViewModel() { Ficha = nuevaTarea };
                await ServicioNavegacion.Instancia.NavigateTo<TareaViewModel>(contexto);
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
                        Descripcion = TareaSeleccionada.Descripcion,
                        Completada = TareaSeleccionada.Completada,
                        Id = TareaSeleccionada.Id
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

        public Task<bool> CargarTareasAsync()
        {
            return Task<bool>.Run(() =>
            {
                int nErrores = 0;
                List<Tarea> tareasBd = null;
                try
                {
                    using (AccesoDatosSqLite dal = new AccesoDatosSqLite())
                    {
                        if (MostrarTodas == true)
                        {
                            tareasBd = dal.ObtenerTareas();
                        }
                        else
                        {
                            tareasBd = dal.ObtenerTareasPendientes();
                        }
                        if (tareasBd != null)
                        {
                            Tareas = new ObservableCollection<TareaViewModel>();
                            foreach (Tarea t in tareasBd)
                            {
                                Tareas.Add(new TareaViewModel() { Ficha = new Tarea { Id = t.Id, Completada = t.Completada, Descripcion = t.Descripcion } });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    nErrores++;
                }
                return (nErrores == 0);
            });

        }
    }
}
