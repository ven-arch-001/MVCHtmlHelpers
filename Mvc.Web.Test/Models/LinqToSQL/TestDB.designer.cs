﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Mvc.Web.Test.Models.LinqToSQL
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
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="TestDB")]
	public partial class TestDBDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertGender(Gender instance);
    partial void UpdateGender(Gender instance);
    partial void DeleteGender(Gender instance);
    partial void InsertState(State instance);
    partial void UpdateState(State instance);
    partial void DeleteState(State instance);
    partial void InsertUser(User instance);
    partial void UpdateUser(User instance);
    partial void DeleteUser(User instance);
    #endregion
		
		public TestDBDataContext() : 
				base(global::System.Configuration.ConfigurationManager.ConnectionStrings["TestDBConnectionString"].ConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public TestDBDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public TestDBDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public TestDBDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public TestDBDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<Gender> Genders
		{
			get
			{
				return this.GetTable<Gender>();
			}
		}
		
		public System.Data.Linq.Table<State> States
		{
			get
			{
				return this.GetTable<State>();
			}
		}
		
		public System.Data.Linq.Table<User> Users
		{
			get
			{
				return this.GetTable<User>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Gender")]
	public partial class Gender : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _GenderId;
		
		private char _Code;
		
		private string _Text;
		
		private EntitySet<User> _Users;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnGenderIdChanging(int value);
    partial void OnGenderIdChanged();
    partial void OnCodeChanging(char value);
    partial void OnCodeChanged();
    partial void OnTextChanging(string value);
    partial void OnTextChanged();
    #endregion
		
		public Gender()
		{
			this._Users = new EntitySet<User>(new Action<User>(this.attach_Users), new Action<User>(this.detach_Users));
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_GenderId", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int GenderId
		{
			get
			{
				return this._GenderId;
			}
			set
			{
				if ((this._GenderId != value))
				{
					this.OnGenderIdChanging(value);
					this.SendPropertyChanging();
					this._GenderId = value;
					this.SendPropertyChanged("GenderId");
					this.OnGenderIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Code", DbType="Char(1) NOT NULL")]
		public char Code
		{
			get
			{
				return this._Code;
			}
			set
			{
				if ((this._Code != value))
				{
					this.OnCodeChanging(value);
					this.SendPropertyChanging();
					this._Code = value;
					this.SendPropertyChanged("Code");
					this.OnCodeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Text", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string Text
		{
			get
			{
				return this._Text;
			}
			set
			{
				if ((this._Text != value))
				{
					this.OnTextChanging(value);
					this.SendPropertyChanging();
					this._Text = value;
					this.SendPropertyChanged("Text");
					this.OnTextChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Gender_User", Storage="_Users", ThisKey="GenderId", OtherKey="GenderId")]
		public EntitySet<User> Users
		{
			get
			{
				return this._Users;
			}
			set
			{
				this._Users.Assign(value);
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
		
		private void attach_Users(User entity)
		{
			this.SendPropertyChanging();
			entity.Gender = this;
		}
		
		private void detach_Users(User entity)
		{
			this.SendPropertyChanging();
			entity.Gender = null;
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.State")]
	public partial class State : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _StateId;
		
		private string _Code;
		
		private string _Text;
		
		private EntitySet<User> _Users;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnStateIdChanging(int value);
    partial void OnStateIdChanged();
    partial void OnCodeChanging(string value);
    partial void OnCodeChanged();
    partial void OnTextChanging(string value);
    partial void OnTextChanged();
    #endregion
		
		public State()
		{
			this._Users = new EntitySet<User>(new Action<User>(this.attach_Users), new Action<User>(this.detach_Users));
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_StateId", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int StateId
		{
			get
			{
				return this._StateId;
			}
			set
			{
				if ((this._StateId != value))
				{
					this.OnStateIdChanging(value);
					this.SendPropertyChanging();
					this._StateId = value;
					this.SendPropertyChanged("StateId");
					this.OnStateIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Code", DbType="Char(2) NOT NULL", CanBeNull=false)]
		public string Code
		{
			get
			{
				return this._Code;
			}
			set
			{
				if ((this._Code != value))
				{
					this.OnCodeChanging(value);
					this.SendPropertyChanging();
					this._Code = value;
					this.SendPropertyChanged("Code");
					this.OnCodeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Text", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string Text
		{
			get
			{
				return this._Text;
			}
			set
			{
				if ((this._Text != value))
				{
					this.OnTextChanging(value);
					this.SendPropertyChanging();
					this._Text = value;
					this.SendPropertyChanged("Text");
					this.OnTextChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="State_User", Storage="_Users", ThisKey="StateId", OtherKey="StateId")]
		public EntitySet<User> Users
		{
			get
			{
				return this._Users;
			}
			set
			{
				this._Users.Assign(value);
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
		
		private void attach_Users(User entity)
		{
			this.SendPropertyChanging();
			entity.State = this;
		}
		
		private void detach_Users(User entity)
		{
			this.SendPropertyChanging();
			entity.State = null;
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.[User]")]
	public partial class User : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _UserId;
		
		private string _LastName;
		
		private string _FirstName;
		
		private string _SSN;
		
		private string _Address;
		
		private System.DateTime _BirthDate;
		
		private int _GenderId;
		
		private int _StateId;
		
		private System.Nullable<System.DateTime> _UpdateDate;
		
		private string _UpdateBy;
		
		private EntityRef<Gender> _Gender;
		
		private EntityRef<State> _State;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnUserIdChanging(int value);
    partial void OnUserIdChanged();
    partial void OnLastNameChanging(string value);
    partial void OnLastNameChanged();
    partial void OnFirstNameChanging(string value);
    partial void OnFirstNameChanged();
    partial void OnSSNChanging(string value);
    partial void OnSSNChanged();
    partial void OnAddressChanging(string value);
    partial void OnAddressChanged();
    partial void OnBirthDateChanging(System.DateTime value);
    partial void OnBirthDateChanged();
    partial void OnGenderIdChanging(int value);
    partial void OnGenderIdChanged();
    partial void OnStateIdChanging(int value);
    partial void OnStateIdChanged();
    partial void OnUpdateDateChanging(System.Nullable<System.DateTime> value);
    partial void OnUpdateDateChanged();
    partial void OnUpdateByChanging(string value);
    partial void OnUpdateByChanged();
    #endregion
		
		public User()
		{
			this._Gender = default(EntityRef<Gender>);
			this._State = default(EntityRef<State>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserId", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int UserId
		{
			get
			{
				return this._UserId;
			}
			set
			{
				if ((this._UserId != value))
				{
					this.OnUserIdChanging(value);
					this.SendPropertyChanging();
					this._UserId = value;
					this.SendPropertyChanged("UserId");
					this.OnUserIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_LastName", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string LastName
		{
			get
			{
				return this._LastName;
			}
			set
			{
				if ((this._LastName != value))
				{
					this.OnLastNameChanging(value);
					this.SendPropertyChanging();
					this._LastName = value;
					this.SendPropertyChanged("LastName");
					this.OnLastNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FirstName", DbType="VarChar(50)")]
		public string FirstName
		{
			get
			{
				return this._FirstName;
			}
			set
			{
				if ((this._FirstName != value))
				{
					this.OnFirstNameChanging(value);
					this.SendPropertyChanging();
					this._FirstName = value;
					this.SendPropertyChanged("FirstName");
					this.OnFirstNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SSN", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string SSN
		{
			get
			{
				return this._SSN;
			}
			set
			{
				if ((this._SSN != value))
				{
					this.OnSSNChanging(value);
					this.SendPropertyChanging();
					this._SSN = value;
					this.SendPropertyChanged("SSN");
					this.OnSSNChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Address", DbType="VarChar(50)")]
		public string Address
		{
			get
			{
				return this._Address;
			}
			set
			{
				if ((this._Address != value))
				{
					this.OnAddressChanging(value);
					this.SendPropertyChanging();
					this._Address = value;
					this.SendPropertyChanged("Address");
					this.OnAddressChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_BirthDate", DbType="Date NOT NULL")]
		public System.DateTime BirthDate
		{
			get
			{
				return this._BirthDate;
			}
			set
			{
				if ((this._BirthDate != value))
				{
					this.OnBirthDateChanging(value);
					this.SendPropertyChanging();
					this._BirthDate = value;
					this.SendPropertyChanged("BirthDate");
					this.OnBirthDateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_GenderId", DbType="Int NOT NULL")]
		public int GenderId
		{
			get
			{
				return this._GenderId;
			}
			set
			{
				if ((this._GenderId != value))
				{
					if (this._Gender.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnGenderIdChanging(value);
					this.SendPropertyChanging();
					this._GenderId = value;
					this.SendPropertyChanged("GenderId");
					this.OnGenderIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_StateId", DbType="Int NOT NULL")]
		public int StateId
		{
			get
			{
				return this._StateId;
			}
			set
			{
				if ((this._StateId != value))
				{
					if (this._State.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnStateIdChanging(value);
					this.SendPropertyChanging();
					this._StateId = value;
					this.SendPropertyChanged("StateId");
					this.OnStateIdChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UpdateDate", DbType="DateTime")]
		public System.Nullable<System.DateTime> UpdateDate
		{
			get
			{
				return this._UpdateDate;
			}
			set
			{
				if ((this._UpdateDate != value))
				{
					this.OnUpdateDateChanging(value);
					this.SendPropertyChanging();
					this._UpdateDate = value;
					this.SendPropertyChanged("UpdateDate");
					this.OnUpdateDateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UpdateBy", DbType="VarChar(50)")]
		public string UpdateBy
		{
			get
			{
				return this._UpdateBy;
			}
			set
			{
				if ((this._UpdateBy != value))
				{
					this.OnUpdateByChanging(value);
					this.SendPropertyChanging();
					this._UpdateBy = value;
					this.SendPropertyChanged("UpdateBy");
					this.OnUpdateByChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Gender_User", Storage="_Gender", ThisKey="GenderId", OtherKey="GenderId", IsForeignKey=true)]
		public Gender Gender
		{
			get
			{
				return this._Gender.Entity;
			}
			set
			{
				Gender previousValue = this._Gender.Entity;
				if (((previousValue != value) 
							|| (this._Gender.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._Gender.Entity = null;
						previousValue.Users.Remove(this);
					}
					this._Gender.Entity = value;
					if ((value != null))
					{
						value.Users.Add(this);
						this._GenderId = value.GenderId;
					}
					else
					{
						this._GenderId = default(int);
					}
					this.SendPropertyChanged("Gender");
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="State_User", Storage="_State", ThisKey="StateId", OtherKey="StateId", IsForeignKey=true)]
		public State State
		{
			get
			{
				return this._State.Entity;
			}
			set
			{
				State previousValue = this._State.Entity;
				if (((previousValue != value) 
							|| (this._State.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._State.Entity = null;
						previousValue.Users.Remove(this);
					}
					this._State.Entity = value;
					if ((value != null))
					{
						value.Users.Add(this);
						this._StateId = value.StateId;
					}
					else
					{
						this._StateId = default(int);
					}
					this.SendPropertyChanged("State");
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
