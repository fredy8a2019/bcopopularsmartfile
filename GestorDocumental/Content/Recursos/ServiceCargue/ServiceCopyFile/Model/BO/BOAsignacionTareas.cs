namespace BO
{
    using System;

    public class BOAsignacionTareas
    {
        private int _Estado;
        private int _Etapa;
        private DateTime _HoraInicio;
        private DateTime _HoraTerminacion;
        private decimal _Negocio;
        private decimal _Usuario;

        public int Estado
        {
            get
            {
                return this._Estado;
            }
            set
            {
                this._Estado = value;
            }
        }

        public int Etapa
        {
            get
            {
                return this._Etapa;
            }
            set
            {
                this._Etapa = value;
            }
        }

        public DateTime HoraInicio
        {
            get
            {
                return this._HoraInicio;
            }
            set
            {
                this._HoraInicio = value;
            }
        }

        public DateTime HoraTerminacion
        {
            get
            {
                return this._HoraTerminacion;
            }
            set
            {
                this._HoraTerminacion = value;
            }
        }

        public decimal Negocio
        {
            get
            {
                return this._Negocio;
            }
            set
            {
                this._Negocio = value;
            }
        }

        public decimal Usuario
        {
            get
            {
                return this._Usuario;
            }
            set
            {
                this._Usuario = value;
            }
        }
    }
}

