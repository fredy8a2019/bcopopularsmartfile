﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.18408
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: global::System.Data.Objects.DataClasses.EdmSchemaAttribute()]

// Nombre de archivo original:
// Fecha de generación: 21/01/2014 05:46:45 p.m.
namespace ActualizarListas.Models
{
    
    /// <summary>
    /// No hay ningún comentario para GestorDocumentalModel en el esquema.
    /// </summary>
    public partial class GestorDocumentalModel : global::System.Data.Objects.ObjectContext
    {
        /// <summary>
        /// Inicializa un nuevo objeto GestorDocumentalModel usando la cadena de conexión encontrada en la sección 'GestorDocumentalModel' del archivo de configuración de la aplicación.
        /// </summary>
        public GestorDocumentalModel() : 
                base("name=GestorDocumentalModel", "GestorDocumentalModel")
        {
            this.OnContextCreated();
        }
        /// <summary>
        /// Inicializar un nuevo objeto GestorDocumentalModel.
        /// </summary>
        public GestorDocumentalModel(string connectionString) : 
                base(connectionString, "GestorDocumentalModel")
        {
            this.OnContextCreated();
        }
        /// <summary>
        /// Inicializar un nuevo objeto GestorDocumentalModel.
        /// </summary>
        public GestorDocumentalModel(global::System.Data.EntityClient.EntityConnection connection) : 
                base(connection, "GestorDocumentalModel")
        {
            this.OnContextCreated();
        }
        partial void OnContextCreated();
    }
}
