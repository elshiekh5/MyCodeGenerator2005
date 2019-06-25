using System;
using System.Collections;
using System.Data ;
using System.Data.SqlClient ;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;
namespace SPGen
{
	/// <summary>
	/// Provides helper functions for SQLDMO
	/// </summary>
	public class SqlProvider
	{
		public static SqlProvider obj;
		static SqlProvider()
		{
			obj=new SqlProvider();
		}
		
		public string ServerName="";
		public string UserName="";
		public string Password="";
		public string Database="";  
		public string _TableName="";
        public Column id = null;
        private Table _Table = null;
        private ArrayList _TableParentTables = null;
    
		public string TableName
		{
			get{return _TableName;}
			set
			{
				_TableName=value;
				//Reload Data
				Table=null;
				TableParentTables=null;
				
			}
		}
		public  string databaseOwner="dbo";

		public  string DatabaseOwner
		{
			get{return databaseOwner;}
		}
        public void Refresh()
        {

		    _TableName="";
            id = null;
            _Table = null;
            TableParentTables = null;
        }
		
		public Column  ID
		{
			get
			{
				if(Fields.Count>0)
				{
					Column identity=null;
					Column guid=null;
					foreach (Column colCurrent in Fields)
					{
						if(colCurrent.InPrimaryKey)
						{
							id=colCurrent;
							break;
						}
						else if(colCurrent.Identity )
						{
							identity=colCurrent;
						}
						else if(colCurrent.IsRowGuidCol )
						{
							guid=colCurrent;
						}

					}
					if(id!=null)
						return id;
					else if(identity!=null)
						return identity;
					else if(guid !=null)
						return guid;
				}
				return null;	
			}
		}
		/// <summary>
		/// SQLDMO.SQLServer Connection
		/// </summary>
		private SQLDMO.SQLServer Connection = new SQLDMO.SQLServerClass();

		/// <summary>
		/// Connects this.Connection
		/// </summary>
		public void Connect()
		{
			Connection.Connect(ServerName, UserName, Password);
		}

		/// <summary>
		/// DisConnects this.Connection
		/// </summary>
		public void DisConnect()
		{
			Connection.DisConnect();
		}

		/// <summary>
		/// An array of Registered SQL Servers
		/// </summary>
		/// <remarks>
		/// Thanks to Leppie @ CodeProject for the following
		/// </remarks>
		public Array RegisteredServers
		{
			get
			{
				ArrayList aServers = new ArrayList();
				SQLDMO.ApplicationClass acServers = new SQLDMO.ApplicationClass();

				for (int iServerGroupCount = 1; iServerGroupCount <= acServers.ServerGroups.Count; iServerGroupCount++)
					for (int iServerCount = 1; iServerCount <= acServers.ServerGroups.Item(iServerGroupCount).RegisteredServers.Count; iServerCount++)
						aServers.Add(acServers.ServerGroups.Item(iServerGroupCount).RegisteredServers.Item(iServerCount).Name);

				return aServers.ToArray();
			}
		}

		/// <summary>
		/// An array of Databases in a SQL Server
		/// </summary>
		public Array Databases
		{
			get
			{
				ArrayList aDatabases = new ArrayList();

				foreach(SQLDMO.Database dbCurrent in Connection.Databases)
					aDatabases.Add(dbCurrent.Name);

				return aDatabases.ToArray();
			}
		}

		/// <summary>
		/// Array of Tables in a Database
		/// </summary>
		public  SQLDMO.Tables Tables
		{
			get
			{
				ArrayList aTables = new ArrayList();
				SQLDMO.Database dbCurrent = (SQLDMO.Database)Connection.Databases.Item(this.Database, Connection);
/*
				foreach(Table tblCurrent in dbCurrent.Tables)
				{
					aTables.Add(tblCurrent.Name);
				}
				
				return aTables.ToArray();*/
				return dbCurrent.Tables;
			}
		}
		/// <summary>
		/// A Table collection of Fields (Columns) in a Table
		/// </summary>
		
		public Table Table
		{
			get
			{
				if(_Table==null)
				{
					SQLDMO.Database dbCurrent = (SQLDMO.Database)Connection.Databases.Item(this.Database, Connection);
					_Table = (Table)dbCurrent.Tables.Item(this.TableName, Connection);
				}
				  return _Table;
			}
			set
			{
				_Table=value;
			}
		}
		/// <summary>
		/// A ColumnCollection collection of Fields (Columns) in a Table
		/// </summary>
		public ColumnCollection Fields
		{
			get
			{
				return Table.Columns;
			}
		}
		//--------------------------------
		public ArrayList TableParentTables
		{
			get
			{
				if(_TableParentTables==null)
				{
					_TableParentTables= GetParentTables(TableName);
				}
				return _TableParentTables;

			}
			set
			{
				_TableParentTables=value;
			}
		}
		//----------------------------------
		public bool ISTableForRelations
		{
			get
			{
				int x=Table.Columns.Count;
				string Tablevv=Table.Name;
				int y=TableParentTables.Count+1;
				int s=TableParentTables.Count+1;
				string sss=ID.Name;
				if(Table.Columns.Count==TableParentTables.Count)
					return true;
				else if(Table.Columns.Count==TableParentTables.Count+1&&ID!=null)
					return true;
				else
					return false;
			}
		}
		//----------------------------------
		public TableConstraint GetParentColumn(string childColumn)
		{
			if(TableParentTables.Count>0)
			{
				foreach(TableConstraint cnt in TableParentTables)
				{
					if(cnt.ChildColID == childColumn)
						return cnt;
				}
				return null;
			}
			else
				return null;
		}
		public string GetExpectedNameForParent(string ParentTable)
		{
			try
			{
				SQLDMO.Database dbCurrent = (SQLDMO.Database)Connection.Databases.Item(this.Database, Connection);
				Table pTable = (Table)dbCurrent.Tables.Item(ParentTable, Connection);
				ColumnCollection c= pTable.Columns;
				return c.Item(2).Name;
			}
			catch
			{
				return "";
			}
		}
		//----------------------------------
		public ArrayList GetParentTables(string TableName) 
		{
			#region sql
			string sql =@"SELECT     INFORMATION_SCHEMA.TABLE_CONSTRAINTS.TABLE_NAME AS ChildTable, CONSTRAINT_COLUMN_USAGE_1.COLUMN_NAME AS ChildColID, 
                      INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS.CONSTRAINT_NAME AS ConstraintName, 
                      INFORMATION_SCHEMA.CONSTRAINT_TABLE_USAGE.TABLE_NAME AS ParentTable, 
                      INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE.COLUMN_NAME AS ParentColID, 
                      REFERENTIAL_CONSTRAINTS_1.UPDATE_RULE AS UpdateRule, REFERENTIAL_CONSTRAINTS_1.DELETE_RULE AS DeleteRule
FROM         INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE INNER JOIN
                      INFORMATION_SCHEMA.CONSTRAINT_TABLE_USAGE INNER JOIN
                      INFORMATION_SCHEMA.TABLE_CONSTRAINTS INNER JOIN
                      INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS ON 
                      INFORMATION_SCHEMA.TABLE_CONSTRAINTS.CONSTRAINT_NAME = INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS.CONSTRAINT_NAME ON
                       INFORMATION_SCHEMA.CONSTRAINT_TABLE_USAGE.CONSTRAINT_NAME = INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS.UNIQUE_CONSTRAINT_NAME
                       ON 
                      INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE.CONSTRAINT_NAME = INFORMATION_SCHEMA.CONSTRAINT_TABLE_USAGE.CONSTRAINT_NAME
                       AND 
                      INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE.TABLE_NAME = INFORMATION_SCHEMA.CONSTRAINT_TABLE_USAGE.TABLE_NAME INNER JOIN
                      INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE CONSTRAINT_COLUMN_USAGE_1 ON 
                      INFORMATION_SCHEMA.TABLE_CONSTRAINTS.CONSTRAINT_NAME = CONSTRAINT_COLUMN_USAGE_1.CONSTRAINT_NAME AND 
                      INFORMATION_SCHEMA.TABLE_CONSTRAINTS.TABLE_NAME = CONSTRAINT_COLUMN_USAGE_1.TABLE_NAME INNER JOIN
                      INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS REFERENTIAL_CONSTRAINTS_1 ON 
                      CONSTRAINT_COLUMN_USAGE_1.CONSTRAINT_NAME = REFERENTIAL_CONSTRAINTS_1.CONSTRAINT_NAME
WHERE     (INFORMATION_SCHEMA.TABLE_CONSTRAINTS.CONSTRAINT_TYPE LIKE N'FOREIGN%') AND 
                      (INFORMATION_SCHEMA.TABLE_CONSTRAINTS.TABLE_NAME = N'{0}')";
			#endregion
			
			ArrayList constrants=new ArrayList();
			string connectionString="data source="+SqlProvider.obj.ServerName+";uid="+SqlProvider.obj.UserName+";pwd="+SqlProvider.obj.Password+"; Initial catalog="+SqlProvider.obj.Database;
			SqlConnection cnn=new SqlConnection(connectionString);
			SqlCommand cmd=new SqlCommand(string.Format(sql,TableName),cnn);
			cmd.CommandType=CommandType.Text;
			cnn.Open();
			using(SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
			{
				// Populate the collection of BlogActivityRecords
				//
				while (dr.Read())
					constrants.Add(PopulateConstaraintFromIDataReader(dr));
			
				// Get total records
				dr.NextResult();
				dr.Read();
				dr.Close();
			}
			cnn.Close();
			return constrants;
			//--------------------------------------------------
		}
		//--------------------------------
		private TableConstraint PopulateConstaraintFromIDataReader(IDataReader dr)
		{
			TableConstraint cnstrnt=new TableConstraint();
			cnstrnt.ChildTable=dr["ChildTable"].ToString();
			cnstrnt.ChildColID=dr["ChildColID"].ToString();
			cnstrnt.ConstraintName=dr["ConstraintName"].ToString();
			cnstrnt.ParentTable=dr["ParentTable"].ToString();
			cnstrnt.ParentColID=dr["ParentColID"].ToString();
			cnstrnt.UpdateRule=dr["UpdateRule"].ToString();
			cnstrnt.DeleteRule=dr["DeleteRule"].ToString();
			return cnstrnt;
		}
		//--------------------------------
		public int ExecuteNonQuery(string sql)
		{
			try
			{
				string connectionString="data source="+SqlProvider.obj.ServerName+";uid="+SqlProvider.obj.UserName+";pwd="+SqlProvider.obj.Password+"; Initial catalog="+SqlProvider.obj.Database;
				SqlConnection cnn=new SqlConnection(connectionString);
				SqlCommand cmd=new SqlCommand(sql,cnn);
				cmd.CommandType=CommandType.Text;
				cnn.Open();
				int result=cmd.ExecuteNonQuery();
				cnn.Close();
				return result;
			}
			catch(Exception ex )
			{
				System.Windows.Forms.MessageBox.Show(ex.Message);
				return -1;
			}
		}
		public string GetTheConnectionString()
		{
			return "server="+ServerName+";uid="+UserName+";pwd="+Password+";database="+Database;
		}
	}
}