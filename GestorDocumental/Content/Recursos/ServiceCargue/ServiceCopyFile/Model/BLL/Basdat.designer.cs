﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.269
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BLL
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="GestorDocumental")]
	public partial class BasdatDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Definiciones de métodos de extensibilidad
    partial void OnCreated();
    partial void InsertParametros(Parametros instance);
    partial void UpdateParametros(Parametros instance);
    partial void DeleteParametros(Parametros instance);
    partial void InsertRecepcion(Recepcion instance);
    partial void UpdateRecepcion(Recepcion instance);
    partial void DeleteRecepcion(Recepcion instance);
    #endregion
		
		public BasdatDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public BasdatDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public BasdatDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public BasdatDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<Parametros> Parametros
		{
			get
			{
				return this.GetTable<Parametros>();
			}
		}
		
		public System.Data.Linq.Table<Recepcion> Recepcion
		{
			get
			{
				return this.GetTable<Recepcion>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Parametros")]
	public partial class Parametros : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private short _id;
		
		private string _codigo;
		
		private string _valor;
		
		private string _descripcion;
		
    #region Definiciones de métodos de extensibilidad
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnidChanging(short value);
    partial void OnidChanged();
    partial void OncodigoChanging(string value);
    partial void OncodigoChanged();
    partial void OnvalorChanging(string value);
    partial void OnvalorChanged();
    partial void OndescripcionChanging(string value);
    partial void OndescripcionChanged();
    #endregion
		
		public Parametros()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_id", AutoSync=AutoSync.OnInsert, DbType="SmallInt NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public short id
		{
			get
			{
				return this._id;
			}
			set
			{
				if ((this._id != value))
				{
					this.OnidChanging(value);
					this.SendPropertyChanging();
					this._id = value;
					this.SendPropertyChanged("id");
					this.OnidChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_codigo", DbType="VarChar(50)")]
		public string codigo
		{
			get
			{
				return this._codigo;
			}
			set
			{
				if ((this._codigo != value))
				{
					this.OncodigoChanging(value);
					this.SendPropertyChanging();
					this._codigo = value;
					this.SendPropertyChanged("codigo");
					this.OncodigoChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_valor", DbType="VarChar(200)")]
		public string valor
		{
			get
			{
				return this._valor;
			}
			set
			{
				if ((this._valor != value))
				{
					this.OnvalorChanging(value);
					this.SendPropertyChanging();
					this._valor = value;
					this.SendPropertyChanged("valor");
					this.OnvalorChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_descripcion", DbType="VarChar(200)")]
		public string descripcion
		{
			get
			{
				return this._descripcion;
			}
			set
			{
				if ((this._descripcion != value))
				{
					this.OndescripcionChanging(value);
					this.SendPropertyChanging();
					this._descripcion = value;
					this.SendPropertyChanged("descripcion");
					this.OndescripcionChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Recepcion")]
	public partial class Recepcion : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private long _id;
		
		private System.Nullable<long> _subgrupo;
		
		private string _rutaArchConciliacion;
		
		private System.Nullable<decimal> _principales;
		
		private System.Nullable<System.DateTime> _fechaRecepcion;
		
		private long _numeroLote;
		
		private System.Nullable<decimal> _idUsuario;
		
		private System.Nullable<bool> _activo;
		
		private System.Nullable<int> _estado;
		
		private System.Nullable<System.DateTime> _fechaCargue;
		
		private System.Nullable<decimal> _nitCliente;
		
    #region Definiciones de métodos de extensibilidad
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnidChanging(long value);
    partial void OnidChanged();
    partial void OnsubgrupoChanging(System.Nullable<long> value);
    partial void OnsubgrupoChanged();
    partial void OnrutaArchConciliacionChanging(string value);
    partial void OnrutaArchConciliacionChanged();
    partial void OnprincipalesChanging(System.Nullable<decimal> value);
    partial void OnprincipalesChanged();
    partial void OnfechaRecepcionChanging(System.Nullable<System.DateTime> value);
    partial void OnfechaRecepcionChanged();
    partial void OnnumeroLoteChanging(long value);
    partial void OnnumeroLoteChanged();
    partial void OnidUsuarioChanging(System.Nullable<decimal> value);
    partial void OnidUsuarioChanged();
    partial void OnactivoChanging(System.Nullable<bool> value);
    partial void OnactivoChanged();
    partial void OnestadoChanging(System.Nullable<int> value);
    partial void OnestadoChanged();
    partial void OnfechaCargueChanging(System.Nullable<System.DateTime> value);
    partial void OnfechaCargueChanged();
    partial void OnnitClienteChanging(System.Nullable<decimal> value);
    partial void OnnitClienteChanged();
    #endregion
		
		public Recepcion()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_id", AutoSync=AutoSync.OnInsert, DbType="BigInt NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public long id
		{
			get
			{
				return this._id;
			}
			set
			{
				if ((this._id != value))
				{
					this.OnidChanging(value);
					this.SendPropertyChanging();
					this._id = value;
					this.SendPropertyChanged("id");
					this.OnidChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_subgrupo", DbType="BigInt")]
		public System.Nullable<long> subgrupo
		{
			get
			{
				return this._subgrupo;
			}
			set
			{
				if ((this._subgrupo != value))
				{
					this.OnsubgrupoChanging(value);
					this.SendPropertyChanging();
					this._subgrupo = value;
					this.SendPropertyChanged("subgrupo");
					this.OnsubgrupoChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_rutaArchConciliacion", DbType="VarChar(200)")]
		public string rutaArchConciliacion
		{
			get
			{
				return this._rutaArchConciliacion;
			}
			set
			{
				if ((this._rutaArchConciliacion != value))
				{
					this.OnrutaArchConciliacionChanging(value);
					this.SendPropertyChanging();
					this._rutaArchConciliacion = value;
					this.SendPropertyChanged("rutaArchConciliacion");
					this.OnrutaArchConciliacionChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_principales", DbType="Decimal(18,0)")]
		public System.Nullable<decimal> principales
		{
			get
			{
				return this._principales;
			}
			set
			{
				if ((this._principales != value))
				{
					this.OnprincipalesChanging(value);
					this.SendPropertyChanging();
					this._principales = value;
					this.SendPropertyChanged("principales");
					this.OnprincipalesChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_fechaRecepcion", DbType="DateTime")]
		public System.Nullable<System.DateTime> fechaRecepcion
		{
			get
			{
				return this._fechaRecepcion;
			}
			set
			{
				if ((this._fechaRecepcion != value))
				{
					this.OnfechaRecepcionChanging(value);
					this.SendPropertyChanging();
					this._fechaRecepcion = value;
					this.SendPropertyChanged("fechaRecepcion");
					this.OnfechaRecepcionChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_numeroLote", DbType="BigInt NOT NULL")]
		public long numeroLote
		{
			get
			{
				return this._numeroLote;
			}
			set
			{
				if ((this._numeroLote != value))
				{
					this.OnnumeroLoteChanging(value);
					this.SendPropertyChanging();
					this._numeroLote = value;
					this.SendPropertyChanged("numeroLote");
					this.OnnumeroLoteChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_idUsuario", DbType="Decimal(18,0)")]
		public System.Nullable<decimal> idUsuario
		{
			get
			{
				return this._idUsuario;
			}
			set
			{
				if ((this._idUsuario != value))
				{
					this.OnidUsuarioChanging(value);
					this.SendPropertyChanging();
					this._idUsuario = value;
					this.SendPropertyChanged("idUsuario");
					this.OnidUsuarioChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_activo", DbType="Bit")]
		public System.Nullable<bool> activo
		{
			get
			{
				return this._activo;
			}
			set
			{
				if ((this._activo != value))
				{
					this.OnactivoChanging(value);
					this.SendPropertyChanging();
					this._activo = value;
					this.SendPropertyChanged("activo");
					this.OnactivoChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_estado", DbType="Int")]
		public System.Nullable<int> estado
		{
			get
			{
				return this._estado;
			}
			set
			{
				if ((this._estado != value))
				{
					this.OnestadoChanging(value);
					this.SendPropertyChanging();
					this._estado = value;
					this.SendPropertyChanged("estado");
					this.OnestadoChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_fechaCargue", DbType="DateTime")]
		public System.Nullable<System.DateTime> fechaCargue
		{
			get
			{
				return this._fechaCargue;
			}
			set
			{
				if ((this._fechaCargue != value))
				{
					this.OnfechaCargueChanging(value);
					this.SendPropertyChanging();
					this._fechaCargue = value;
					this.SendPropertyChanged("fechaCargue");
					this.OnfechaCargueChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_nitCliente", DbType="Decimal(18,0)")]
		public System.Nullable<decimal> nitCliente
		{
			get
			{
				return this._nitCliente;
			}
			set
			{
				if ((this._nitCliente != value))
				{
					this.OnnitClienteChanging(value);
					this.SendPropertyChanging();
					this._nitCliente = value;
					this.SendPropertyChanged("nitCliente");
					this.OnnitClienteChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
#pragma warning restore 1591
