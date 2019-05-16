namespace BO
{
    using System;
    public class BOClientes
    {
        private int _ContidadLotes;
        private decimal _Cliente;
        private string _RutaOrigen;

        public int ContidadLotes
        {
            get { return _ContidadLotes; }
            set { _ContidadLotes = value; }
        }
        
        public decimal Cliente
        {
            get { return _Cliente; }
            set { _Cliente = value; }
        }
        
        public string RutaOrigen
        {
            get { return _RutaOrigen; }
            set { _RutaOrigen = value; }
        }


    }
}
