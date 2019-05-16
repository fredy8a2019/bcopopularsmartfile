namespace BO
{
    using System;

    public class BOCargueLotes
    {
        private bool _ArchivoCargado;
        private decimal _Cliente;
        private int _ConsecutivoLote;
        private DateTime _FechaCargue;
        private string _Lote;
        private DateTime _LoteFecha;
        private string _LoteScaner;
        private decimal _Negocio;
        private string _NomArchivo;
        private int _Paginas;
        private string _CodBarras;        
        private decimal _Usuario;
        private long _idRecepcion;

        public bool ArchivoCargado
        {
            get
            {
                return this._ArchivoCargado;
            }
            set
            {
                this._ArchivoCargado = value;
            }
        }

        public decimal Cliente
        {
            get
            {
                return this._Cliente;
            }
            set
            {
                this._Cliente = value;
            }
        }

        public int ConsecutivoLote
        {
            get
            {
                return this._ConsecutivoLote;
            }
            set
            {
                this._ConsecutivoLote = value;
            }
        }

        public DateTime FechaCargue
        {
            get
            {
                return this._FechaCargue;
            }
            set
            {
                this._FechaCargue = value;
            }
        }

        public string Lote
        {
            get
            {
                return this._Lote;
            }
            set
            {
                this._Lote = value;
            }
        }

        public DateTime LoteFecha
        {
            get
            {
                return this._LoteFecha;
            }
            set
            {
                this._LoteFecha = value;
            }
        }

        public string LoteScaner
        {
            get
            {
                return this._LoteScaner;
            }
            set
            {
                this._LoteScaner = value;
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

        public string NomArchivo
        {
            get
            {
                return this._NomArchivo;
            }
            set
            {
                this._NomArchivo = value;
            }
        }

        public int Paginas
        {
            get
            {
                return this._Paginas;
            }
            set
            {
                this._Paginas = value;
            }
        }

        public string CodBarras
        {
            get
            {
                return this._CodBarras;
            }
            set
            {
                this._CodBarras = value;
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

        public long idRecepcion
        {
            get
            {
                return this._idRecepcion;
            }
            set
            {
                this._idRecepcion = value;
            }
        }
    }
}

