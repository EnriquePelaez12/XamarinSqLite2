using SQLite;

namespace XamarinSqLite2.Modelo
{

    [Table("Tareas")]
    public class Tarea
    {
        private int m_Id;
        [PrimaryKey, AutoIncrement]
        public int Id
        {
            get
            {
                return m_Id;
            }
            set
            {
                this.m_Id = value;
            }
        }

        private string m_Descripcion;
        [MaxLength(200), NotNull]
        public string Descripcion
        {
            get
            {
                return m_Descripcion;
            }
            set
            {
                this.m_Descripcion = value;
            }
        }

        private bool m_Completada;
        [NotNull]
        public bool Completada
        {
            get
            {
                return m_Completada;
            }
            set
            {
                this.m_Completada = value;
            }
        }
    }
}
