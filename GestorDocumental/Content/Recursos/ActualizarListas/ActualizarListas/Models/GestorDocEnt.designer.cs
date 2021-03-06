﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.18063
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ActualizarListas.Models
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
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="GestorDocumentalGNF")]
	public partial class GestorDocEntDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Definiciones de métodos de extensibilidad
    partial void OnCreated();
    partial void InsertCampos(Campos instance);
    partial void UpdateCampos(Campos instance);
    partial void DeleteCampos(Campos instance);
    partial void InsertP_Ciudad(P_Ciudad instance);
    partial void UpdateP_Ciudad(P_Ciudad instance);
    partial void DeleteP_Ciudad(P_Ciudad instance);
    partial void InsertCodigosCampo(CodigosCampo instance);
    partial void UpdateCodigosCampo(CodigosCampo instance);
    partial void DeleteCodigosCampo(CodigosCampo instance);
    #endregion
		
		public GestorDocEntDataContext() : 
				base(global::ActualizarListas.Properties.Settings.Default.GestorDocumentalGNFConnectionString1, mappingSource)
		{
			OnCreated();
		}
		
		public GestorDocEntDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public GestorDocEntDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public GestorDocEntDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public GestorDocEntDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<Campos> Campos
		{
			get
			{
				return this.GetTable<Campos>();
			}
		}
		
		public System.Data.Linq.Table<P_Ciudad> P_Ciudad
		{
			get
			{
				return this.GetTable<P_Ciudad>();
			}
		}
		
		public System.Data.Linq.Table<CodigosCampo> CodigosCampo
		{
			get
			{
				return this.GetTable<CodigosCampo>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Campos")]
	public partial class Campos : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _CampId;
		
		private string _CampDescripcion;
		
		private System.Nullable<int> _idPadre;
		
		private string _NomBizagi;
		
		private string _TablaBizagi;
		
		private int _CampAlto;
		
		private int _CampAncho;
		
		private int _CampOrden;
		
		private int _CampNumCaptura;
		
		private System.Nullable<bool> _CampNumCaptura2;
		
		private System.Nullable<bool> _ControlCalidad;
		
		private bool _CampObligatorio;
		
		private int _TcId;
		
		private System.Nullable<int> _GDocId;
		
		private System.Nullable<int> _CampDependiente;
		
		private System.Nullable<int> _GruId;
		
		private System.Nullable<bool> _Activo;
		
		private System.Nullable<short> _ColConciliacion;
		
		private System.Nullable<int> _LongMax;
		
		private System.Nullable<int> _SelectIndex;
		
		private string _ValidationFunction;
		
		private string _ErrorMessage;
		
		private System.Nullable<int> _PosX;
		
		private System.Nullable<int> _PosY;
		
		private System.Nullable<int> _CodFormulario;
		
		private EntitySet<Campos> _Campos2;
		
		private EntitySet<CodigosCampo> _CodigosCampo;
		
		private EntityRef<Campos> _Campos1;
		
    #region Definiciones de métodos de extensibilidad
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnCampIdChanging(int value);
    partial void OnCampIdChanged();
    partial void OnCampDescripcionChanging(string value);
    partial void OnCampDescripcionChanged();
    partial void OnidPadreChanging(System.Nullable<int> value);
    partial void OnidPadreChanged();
    partial void OnNomBizagiChanging(string value);
    partial void OnNomBizagiChanged();
    partial void OnTablaBizagiChanging(string value);
    partial void OnTablaBizagiChanged();
    partial void OnCampAltoChanging(int value);
    partial void OnCampAltoChanged();
    partial void OnCampAnchoChanging(int value);
    partial void OnCampAnchoChanged();
    partial void OnCampOrdenChanging(int value);
    partial void OnCampOrdenChanged();
    partial void OnCampNumCapturaChanging(int value);
    partial void OnCampNumCapturaChanged();
    partial void OnCampNumCaptura2Changing(System.Nullable<bool> value);
    partial void OnCampNumCaptura2Changed();
    partial void OnControlCalidadChanging(System.Nullable<bool> value);
    partial void OnControlCalidadChanged();
    partial void OnCampObligatorioChanging(bool value);
    partial void OnCampObligatorioChanged();
    partial void OnTcIdChanging(int value);
    partial void OnTcIdChanged();
    partial void OnGDocIdChanging(System.Nullable<int> value);
    partial void OnGDocIdChanged();
    partial void OnCampDependienteChanging(System.Nullable<int> value);
    partial void OnCampDependienteChanged();
    partial void OnGruIdChanging(System.Nullable<int> value);
    partial void OnGruIdChanged();
    partial void OnActivoChanging(System.Nullable<bool> value);
    partial void OnActivoChanged();
    partial void OnColConciliacionChanging(System.Nullable<short> value);
    partial void OnColConciliacionChanged();
    partial void OnLongMaxChanging(System.Nullable<int> value);
    partial void OnLongMaxChanged();
    partial void OnSelectIndexChanging(System.Nullable<int> value);
    partial void OnSelectIndexChanged();
    partial void OnValidationFunctionChanging(string value);
    partial void OnValidationFunctionChanged();
    partial void OnErrorMessageChanging(string value);
    partial void OnErrorMessageChanged();
    partial void OnPosXChanging(System.Nullable<int> value);
    partial void OnPosXChanged();
    partial void OnPosYChanging(System.Nullable<int> value);
    partial void OnPosYChanged();
    partial void OnCodFormularioChanging(System.Nullable<int> value);
    partial void OnCodFormularioChanged();
    #endregion
		
		public Campos()
		{
			this._Campos2 = new EntitySet<Campos>(new Action<Campos>(this.attach_Campos2), new Action<Campos>(this.detach_Campos2));
			this._CodigosCampo = new EntitySet<CodigosCampo>(new Action<CodigosCampo>(this.attach_CodigosCampo), new Action<CodigosCampo>(this.detach_CodigosCampo));
			this._Campos1 = default(EntityRef<Campos>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CampId", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int CampId
		{
			get
			{
				return this._CampId;
			}
			set
			{
				if ((this._CampId != value))
				{
					this.OnCampIdChanging(value);
					this.SendPropertyChanging();
					this._CampId = value;
					this.SendPropertyChanged("CampId");
					this.OnCampIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CampDescripcion", DbType="VarChar(200) NOT NULL", CanBeNull=false)]
		public string CampDescripcion
		{
			get
			{
				return this._CampDescripcion;
			}
			set
			{
				if ((this._CampDescripcion != value))
				{
					this.OnCampDescripcionChanging(value);
					this.SendPropertyChanging();
					this._CampDescripcion = value;
					this.SendPropertyChanged("CampDescripcion");
					this.OnCampDescripcionChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_idPadre", DbType="Int")]
		public System.Nullable<int> idPadre
		{
			get
			{
				return this._idPadre;
			}
			set
			{
				if ((this._idPadre != value))
				{
					if (this._Campos1.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnidPadreChanging(value);
					this.SendPropertyChanging();
					this._idPadre = value;
					this.SendPropertyChanged("idPadre");
					this.OnidPadreChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_NomBizagi", DbType="VarChar(100)")]
		public string NomBizagi
		{
			get
			{
				return this._NomBizagi;
			}
			set
			{
				if ((this._NomBizagi != value))
				{
					this.OnNomBizagiChanging(value);
					this.SendPropertyChanging();
					this._NomBizagi = value;
					this.SendPropertyChanged("NomBizagi");
					this.OnNomBizagiChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TablaBizagi", DbType="VarChar(50)")]
		public string TablaBizagi
		{
			get
			{
				return this._TablaBizagi;
			}
			set
			{
				if ((this._TablaBizagi != value))
				{
					this.OnTablaBizagiChanging(value);
					this.SendPropertyChanging();
					this._TablaBizagi = value;
					this.SendPropertyChanged("TablaBizagi");
					this.OnTablaBizagiChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CampAlto", DbType="Int NOT NULL")]
		public int CampAlto
		{
			get
			{
				return this._CampAlto;
			}
			set
			{
				if ((this._CampAlto != value))
				{
					this.OnCampAltoChanging(value);
					this.SendPropertyChanging();
					this._CampAlto = value;
					this.SendPropertyChanged("CampAlto");
					this.OnCampAltoChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CampAncho", DbType="Int NOT NULL")]
		public int CampAncho
		{
			get
			{
				return this._CampAncho;
			}
			set
			{
				if ((this._CampAncho != value))
				{
					this.OnCampAnchoChanging(value);
					this.SendPropertyChanging();
					this._CampAncho = value;
					this.SendPropertyChanged("CampAncho");
					this.OnCampAnchoChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CampOrden", DbType="Int NOT NULL")]
		public int CampOrden
		{
			get
			{
				return this._CampOrden;
			}
			set
			{
				if ((this._CampOrden != value))
				{
					this.OnCampOrdenChanging(value);
					this.SendPropertyChanging();
					this._CampOrden = value;
					this.SendPropertyChanged("CampOrden");
					this.OnCampOrdenChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CampNumCaptura", DbType="Int NOT NULL")]
		public int CampNumCaptura
		{
			get
			{
				return this._CampNumCaptura;
			}
			set
			{
				if ((this._CampNumCaptura != value))
				{
					this.OnCampNumCapturaChanging(value);
					this.SendPropertyChanging();
					this._CampNumCaptura = value;
					this.SendPropertyChanged("CampNumCaptura");
					this.OnCampNumCapturaChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CampNumCaptura2", DbType="Bit")]
		public System.Nullable<bool> CampNumCaptura2
		{
			get
			{
				return this._CampNumCaptura2;
			}
			set
			{
				if ((this._CampNumCaptura2 != value))
				{
					this.OnCampNumCaptura2Changing(value);
					this.SendPropertyChanging();
					this._CampNumCaptura2 = value;
					this.SendPropertyChanged("CampNumCaptura2");
					this.OnCampNumCaptura2Changed();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ControlCalidad", DbType="Bit")]
		public System.Nullable<bool> ControlCalidad
		{
			get
			{
				return this._ControlCalidad;
			}
			set
			{
				if ((this._ControlCalidad != value))
				{
					this.OnControlCalidadChanging(value);
					this.SendPropertyChanging();
					this._ControlCalidad = value;
					this.SendPropertyChanged("ControlCalidad");
					this.OnControlCalidadChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CampObligatorio", DbType="Bit NOT NULL")]
		public bool CampObligatorio
		{
			get
			{
				return this._CampObligatorio;
			}
			set
			{
				if ((this._CampObligatorio != value))
				{
					this.OnCampObligatorioChanging(value);
					this.SendPropertyChanging();
					this._CampObligatorio = value;
					this.SendPropertyChanged("CampObligatorio");
					this.OnCampObligatorioChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_TcId", DbType="Int NOT NULL")]
		public int TcId
		{
			get
			{
				return this._TcId;
			}
			set
			{
				if ((this._TcId != value))
				{
					this.OnTcIdChanging(value);
					this.SendPropertyChanging();
					this._TcId = value;
					this.SendPropertyChanged("TcId");
					this.OnTcIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_GDocId", DbType="Int")]
		public System.Nullable<int> GDocId
		{
			get
			{
				return this._GDocId;
			}
			set
			{
				if ((this._GDocId != value))
				{
					this.OnGDocIdChanging(value);
					this.SendPropertyChanging();
					this._GDocId = value;
					this.SendPropertyChanged("GDocId");
					this.OnGDocIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CampDependiente", DbType="Int")]
		public System.Nullable<int> CampDependiente
		{
			get
			{
				return this._CampDependiente;
			}
			set
			{
				if ((this._CampDependiente != value))
				{
					this.OnCampDependienteChanging(value);
					this.SendPropertyChanging();
					this._CampDependiente = value;
					this.SendPropertyChanged("CampDependiente");
					this.OnCampDependienteChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_GruId", DbType="Int")]
		public System.Nullable<int> GruId
		{
			get
			{
				return this._GruId;
			}
			set
			{
				if ((this._GruId != value))
				{
					this.OnGruIdChanging(value);
					this.SendPropertyChanging();
					this._GruId = value;
					this.SendPropertyChanged("GruId");
					this.OnGruIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Activo", DbType="Bit")]
		public System.Nullable<bool> Activo
		{
			get
			{
				return this._Activo;
			}
			set
			{
				if ((this._Activo != value))
				{
					this.OnActivoChanging(value);
					this.SendPropertyChanging();
					this._Activo = value;
					this.SendPropertyChanged("Activo");
					this.OnActivoChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ColConciliacion", DbType="SmallInt")]
		public System.Nullable<short> ColConciliacion
		{
			get
			{
				return this._ColConciliacion;
			}
			set
			{
				if ((this._ColConciliacion != value))
				{
					this.OnColConciliacionChanging(value);
					this.SendPropertyChanging();
					this._ColConciliacion = value;
					this.SendPropertyChanged("ColConciliacion");
					this.OnColConciliacionChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_LongMax", DbType="Int")]
		public System.Nullable<int> LongMax
		{
			get
			{
				return this._LongMax;
			}
			set
			{
				if ((this._LongMax != value))
				{
					this.OnLongMaxChanging(value);
					this.SendPropertyChanging();
					this._LongMax = value;
					this.SendPropertyChanged("LongMax");
					this.OnLongMaxChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SelectIndex", DbType="Int")]
		public System.Nullable<int> SelectIndex
		{
			get
			{
				return this._SelectIndex;
			}
			set
			{
				if ((this._SelectIndex != value))
				{
					this.OnSelectIndexChanging(value);
					this.SendPropertyChanging();
					this._SelectIndex = value;
					this.SendPropertyChanged("SelectIndex");
					this.OnSelectIndexChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ValidationFunction", DbType="VarChar(100)")]
		public string ValidationFunction
		{
			get
			{
				return this._ValidationFunction;
			}
			set
			{
				if ((this._ValidationFunction != value))
				{
					this.OnValidationFunctionChanging(value);
					this.SendPropertyChanging();
					this._ValidationFunction = value;
					this.SendPropertyChanged("ValidationFunction");
					this.OnValidationFunctionChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ErrorMessage", DbType="VarChar(200)")]
		public string ErrorMessage
		{
			get
			{
				return this._ErrorMessage;
			}
			set
			{
				if ((this._ErrorMessage != value))
				{
					this.OnErrorMessageChanging(value);
					this.SendPropertyChanging();
					this._ErrorMessage = value;
					this.SendPropertyChanged("ErrorMessage");
					this.OnErrorMessageChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PosX", DbType="Int")]
		public System.Nullable<int> PosX
		{
			get
			{
				return this._PosX;
			}
			set
			{
				if ((this._PosX != value))
				{
					this.OnPosXChanging(value);
					this.SendPropertyChanging();
					this._PosX = value;
					this.SendPropertyChanged("PosX");
					this.OnPosXChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PosY", DbType="Int")]
		public System.Nullable<int> PosY
		{
			get
			{
				return this._PosY;
			}
			set
			{
				if ((this._PosY != value))
				{
					this.OnPosYChanging(value);
					this.SendPropertyChanging();
					this._PosY = value;
					this.SendPropertyChanged("PosY");
					this.OnPosYChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CodFormulario", DbType="Int")]
		public System.Nullable<int> CodFormulario
		{
			get
			{
				return this._CodFormulario;
			}
			set
			{
				if ((this._CodFormulario != value))
				{
					this.OnCodFormularioChanging(value);
					this.SendPropertyChanging();
					this._CodFormulario = value;
					this.SendPropertyChanged("CodFormulario");
					this.OnCodFormularioChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Campos_Campos", Storage="_Campos2", ThisKey="CampId", OtherKey="idPadre")]
		public EntitySet<Campos> Campos2
		{
			get
			{
				return this._Campos2;
			}
			set
			{
				this._Campos2.Assign(value);
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Campos_CodigosCampo", Storage="_CodigosCampo", ThisKey="CampId", OtherKey="CampId")]
		public EntitySet<CodigosCampo> CodigosCampo
		{
			get
			{
				return this._CodigosCampo;
			}
			set
			{
				this._CodigosCampo.Assign(value);
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Campos_Campos", Storage="_Campos1", ThisKey="idPadre", OtherKey="CampId", IsForeignKey=true)]
		public Campos Campos1
		{
			get
			{
				return this._Campos1.Entity;
			}
			set
			{
				Campos previousValue = this._Campos1.Entity;
				if (((previousValue != value) 
							|| (this._Campos1.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._Campos1.Entity = null;
						previousValue.Campos2.Remove(this);
					}
					this._Campos1.Entity = value;
					if ((value != null))
					{
						value.Campos2.Add(this);
						this._idPadre = value.CampId;
					}
					else
					{
						this._idPadre = default(Nullable<int>);
					}
					this.SendPropertyChanged("Campos1");
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
		
		private void attach_Campos2(Campos entity)
		{
			this.SendPropertyChanging();
			entity.Campos1 = this;
		}
		
		private void detach_Campos2(Campos entity)
		{
			this.SendPropertyChanging();
			entity.Campos1 = null;
		}
		
		private void attach_CodigosCampo(CodigosCampo entity)
		{
			this.SendPropertyChanging();
			entity.Campos = this;
		}
		
		private void detach_CodigosCampo(CodigosCampo entity)
		{
			this.SendPropertyChanging();
			entity.Campos = null;
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.P_Ciudad")]
	public partial class P_Ciudad : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _CiuId;
		
		private string _CiuNombre;
		
		private int _DeptId;
		
		private System.Nullable<bool> _Activo;
		
    #region Definiciones de métodos de extensibilidad
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnCiuIdChanging(int value);
    partial void OnCiuIdChanged();
    partial void OnCiuNombreChanging(string value);
    partial void OnCiuNombreChanged();
    partial void OnDeptIdChanging(int value);
    partial void OnDeptIdChanged();
    partial void OnActivoChanging(System.Nullable<bool> value);
    partial void OnActivoChanged();
    #endregion
		
		public P_Ciudad()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CiuId", DbType="Int NOT NULL", IsPrimaryKey=true)]
		public int CiuId
		{
			get
			{
				return this._CiuId;
			}
			set
			{
				if ((this._CiuId != value))
				{
					this.OnCiuIdChanging(value);
					this.SendPropertyChanging();
					this._CiuId = value;
					this.SendPropertyChanged("CiuId");
					this.OnCiuIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CiuNombre", DbType="VarChar(200) NOT NULL", CanBeNull=false)]
		public string CiuNombre
		{
			get
			{
				return this._CiuNombre;
			}
			set
			{
				if ((this._CiuNombre != value))
				{
					this.OnCiuNombreChanging(value);
					this.SendPropertyChanging();
					this._CiuNombre = value;
					this.SendPropertyChanged("CiuNombre");
					this.OnCiuNombreChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DeptId", DbType="Int NOT NULL")]
		public int DeptId
		{
			get
			{
				return this._DeptId;
			}
			set
			{
				if ((this._DeptId != value))
				{
					this.OnDeptIdChanging(value);
					this.SendPropertyChanging();
					this._DeptId = value;
					this.SendPropertyChanged("DeptId");
					this.OnDeptIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Activo", DbType="Bit")]
		public System.Nullable<bool> Activo
		{
			get
			{
				return this._Activo;
			}
			set
			{
				if ((this._Activo != value))
				{
					this.OnActivoChanging(value);
					this.SendPropertyChanging();
					this._Activo = value;
					this.SendPropertyChanged("Activo");
					this.OnActivoChanged();
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
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.CodigosCampo")]
	public partial class CodigosCampo : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _CodCampId;
		
		private string _CodId;
		
		private string _CodDescripcion;
		
		private int _CampId;
		
		private int _CodOrden;
		
		private System.Nullable<bool> _Activo;
		
		private string _CodPadre;
		
		private EntityRef<Campos> _Campos;
		
    #region Definiciones de métodos de extensibilidad
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnCodCampIdChanging(int value);
    partial void OnCodCampIdChanged();
    partial void OnCodIdChanging(string value);
    partial void OnCodIdChanged();
    partial void OnCodDescripcionChanging(string value);
    partial void OnCodDescripcionChanged();
    partial void OnCampIdChanging(int value);
    partial void OnCampIdChanged();
    partial void OnCodOrdenChanging(int value);
    partial void OnCodOrdenChanged();
    partial void OnActivoChanging(System.Nullable<bool> value);
    partial void OnActivoChanged();
    partial void OnCodPadreChanging(string value);
    partial void OnCodPadreChanged();
    #endregion
		
		public CodigosCampo()
		{
			this._Campos = default(EntityRef<Campos>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CodCampId", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int CodCampId
		{
			get
			{
				return this._CodCampId;
			}
			set
			{
				if ((this._CodCampId != value))
				{
					this.OnCodCampIdChanging(value);
					this.SendPropertyChanging();
					this._CodCampId = value;
					this.SendPropertyChanged("CodCampId");
					this.OnCodCampIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CodId", DbType="VarChar(100) NOT NULL", CanBeNull=false)]
		public string CodId
		{
			get
			{
				return this._CodId;
			}
			set
			{
				if ((this._CodId != value))
				{
					this.OnCodIdChanging(value);
					this.SendPropertyChanging();
					this._CodId = value;
					this.SendPropertyChanged("CodId");
					this.OnCodIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CodDescripcion", DbType="VarChar(100) NOT NULL", CanBeNull=false)]
		public string CodDescripcion
		{
			get
			{
				return this._CodDescripcion;
			}
			set
			{
				if ((this._CodDescripcion != value))
				{
					this.OnCodDescripcionChanging(value);
					this.SendPropertyChanging();
					this._CodDescripcion = value;
					this.SendPropertyChanged("CodDescripcion");
					this.OnCodDescripcionChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CampId", DbType="Int NOT NULL")]
		public int CampId
		{
			get
			{
				return this._CampId;
			}
			set
			{
				if ((this._CampId != value))
				{
					if (this._Campos.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnCampIdChanging(value);
					this.SendPropertyChanging();
					this._CampId = value;
					this.SendPropertyChanged("CampId");
					this.OnCampIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CodOrden", DbType="Int NOT NULL")]
		public int CodOrden
		{
			get
			{
				return this._CodOrden;
			}
			set
			{
				if ((this._CodOrden != value))
				{
					this.OnCodOrdenChanging(value);
					this.SendPropertyChanging();
					this._CodOrden = value;
					this.SendPropertyChanged("CodOrden");
					this.OnCodOrdenChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Activo", DbType="Bit")]
		public System.Nullable<bool> Activo
		{
			get
			{
				return this._Activo;
			}
			set
			{
				if ((this._Activo != value))
				{
					this.OnActivoChanging(value);
					this.SendPropertyChanging();
					this._Activo = value;
					this.SendPropertyChanged("Activo");
					this.OnActivoChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CodPadre", DbType="VarChar(100)")]
		public string CodPadre
		{
			get
			{
				return this._CodPadre;
			}
			set
			{
				if ((this._CodPadre != value))
				{
					this.OnCodPadreChanging(value);
					this.SendPropertyChanging();
					this._CodPadre = value;
					this.SendPropertyChanged("CodPadre");
					this.OnCodPadreChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Campos_CodigosCampo", Storage="_Campos", ThisKey="CampId", OtherKey="CampId", IsForeignKey=true)]
		public Campos Campos
		{
			get
			{
				return this._Campos.Entity;
			}
			set
			{
				Campos previousValue = this._Campos.Entity;
				if (((previousValue != value) 
							|| (this._Campos.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._Campos.Entity = null;
						previousValue.CodigosCampo.Remove(this);
					}
					this._Campos.Entity = value;
					if ((value != null))
					{
						value.CodigosCampo.Add(this);
						this._CampId = value.CampId;
					}
					else
					{
						this._CampId = default(int);
					}
					this.SendPropertyChanged("Campos");
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
