using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SPGen
{

	public partial class Form1 : Form
	{
		public string ProcedureAndMethodName ;
		Hashtable allParameters = new Hashtable();
		Hashtable whereParameters = new Hashtable();
		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			ProcedureAndMethodName = txtProcedureAndMethodName.Text;
			SqlProvider.obj.DisConnect();
			SqlProvider.obj.Connect();
			selDatabases.DataSource = SqlProvider.obj.Databases;
			selOperationType.SelectedIndex = 0;
		}

		private void selDatabases_SelectedIndexChanged(object sender, EventArgs e)
		{
			SqlProvider.obj.Database = selDatabases.SelectedValue.ToString();
			ArrayList aTables = new ArrayList();
			foreach (Table tblCurrent in SqlProvider.obj.Tables)
			{
				if (!tblCurrent.SystemObject)
					aTables.Add(tblCurrent.Name);
			}
			selTables.DataSource = aTables;
			selTables.Enabled = true;
		}

		private void selTables_SelectedIndexChanged(object sender, EventArgs e)
		{
			lvOperationParametars.Items.Clear();
			lvWhereparameter.Items.Clear();
			SqlProvider.obj.TableName = selTables.SelectedValue.ToString();
			ListViewItem lvItem;
			ListViewItem lvItem2;
			foreach (Column c in SqlProvider.obj.Fields)
			{
				lvItem = lvOperationParametars.Items.Add(c.Name);
				lvItem.Checked = true;
				lvItem.Name = c.Name;
				lvItem2 = lvWhereparameter.Items.Add(c.Name);
				lvItem2.Checked = false;
				lvItem2.Name = c.Name;
				//CreateWhereValueItem(c);
			}
			lvOperationParametars.Enabled = true;
			lvWhereparameter.Enabled = true;
			//---------------------------------------
			if (selOperationType.SelectedIndex == 0)
				lvWhereparameter.Enabled = false;
			else
				lvWhereparameter.Enabled = true;
		}

		private void btnGenerate_Click(object sender, EventArgs e)
		{
			StoredProcedure.WriteStoredProcedure(GenerateProcedure(ProcedureAndMethodName));
			txtProcedure.Text += Environment.NewLine;
			string _class;
			_class= new SqlDataProviderBuilder().CreateInsertUpdateDeleteMethod(StoredProcedureTypes.Create, ProcedureAndMethodName, allParameters);
			_class += new ClassFactoryBuilder().CreateInsertUpdateDeleteMethod(StoredProcedureTypes.Create, ProcedureAndMethodName, allParameters);
			//Admin Add Page---------------------------------------------
			InterfaceType type = (InterfaceType)selInterfaceType.SelectedIndex;
			Create_InterfaceBuilder.Create(type, allParameters, ProcedureAndMethodName);
			Create_CodeBehindBuilder.Create(type, allParameters, ProcedureAndMethodName);
			FileManager.CreateFile(Globals.BaseDirectory + "TempClass.cs", _class);
			//-----------------------------------------------------------
		}
		//
		private string GenerateProcedure(string procedureName)
		{

			foreach (ListViewItem item in lvOperationParametars.Items)
			{
				if (item.Checked)
				{
					if (!allParameters.Contains(item.Text))
					{
						allParameters[item.Text] = item.Text;
					}
				}
			}
			foreach (ListViewItem item in lvWhereparameter.Items)
			{
				if (item.Checked)
				{
					if (!allParameters.Contains(item.Text))
					{
						allParameters[item.Text] = item.Text;
					}
					if (!whereParameters.Contains(item.Text))
					{
						whereParameters[item.Text] = item.Text;
					}
				}
			}
			string sp = "";

			switch (selOperationType.SelectedIndex)
			{
				case 0://StoredProcedureTypes.Create:
					sp = GenerateInsertProcedure(allParameters, whereParameters);
					break;

				case 1://StoredProcedureTypes.Update:
					sp = GenerateUpdatePrcedure(allParameters, whereParameters);
					break;
				case 2://StoredProcedureTypes.GetAll:

					break;
			}

			return sp;
		}
		//
		private string CreateWhereStatement(Hashtable whereParameters)
		{
			StringBuilder whereStatement = new StringBuilder();
			foreach (ListViewItem item in lvWhereparameter.Items)
			{
				if (item.Checked)
				{
					if (true)//(!allParameters.Contains(item.Text))
					{
						if (whereStatement.Length == 0)
						{
							whereStatement.Append(" Where");
						}

						else
						{
							whereStatement.Append(" And ");
						}
						whereStatement.Append(Environment.NewLine);
						whereStatement.Append(item.Text + " = @" + item.Text);
						whereStatement.Append(Environment.NewLine);
					}
				}
			}
			/* the where values
			CheckBox cb;
			TextBox txt;
			foreach (Column colCurrent in SqlProvider.obj.Fields)
			{
				if (whereParameters.Contains(colCurrent.Name))
					continue;
				cb= this.Controls["cb" + colCurrent.Name] as CheckBox;
				if (cb.Checked)
				{
					if (whereStatement.Length == 0)
						whereStatement.Append(" Where ");
					else
						whereStatement.Append(" And ");
					txt = this.Controls["txt" + colCurrent.Name] as TextBox;
					if (colCurrent.Datatype == "char" ||
						colCurrent.Datatype == "nchar" ||
						colCurrent.Datatype == "nvarchar" ||
						colCurrent.Datatype == "varchar" ||
						colCurrent.Datatype == "text" ||
						colCurrent.Datatype == "ntext"
						)
					{
						whereStatement.Append(colCurrent.Name + " = '" + txt.Text + "'");
					}
					else
					{
						whereStatement.Append(colCurrent.Name + " = " + txt.Text);
					}
					whereStatement.Append(Environment.NewLine);
			
				}
				
			} */
			return whereStatement.ToString();
		}
		//

		private string GenerateInsertProcedure(Hashtable allParameters, Hashtable whereParameters)
		{
			try
			{
				StringBuilder sGeneratedCode = new StringBuilder();
				StringBuilder sParamDeclaration = new StringBuilder();
				StringBuilder sBody = new StringBuilder();
				StringBuilder sINSERTValues = new StringBuilder();
				string finnal = "";
				// Setup SP code, begining is the same no matter the type
				sGeneratedCode.AppendFormat("CREATE PROCEDURE {0}.[{1}_{2}]", new string[] { SqlProvider.obj.DatabaseOwner, Globals.GetProgramatlyName(SqlProvider.obj.TableName), ProcedureAndMethodName });
				sGeneratedCode.Append(Environment.NewLine);

				// Setup body code, different for UPDATE and INSERT
				sBody.AppendFormat("INSERT INTO [{0}] (", SqlProvider.obj.TableName);
				sBody.Append(Environment.NewLine);
				sINSERTValues.Append("VALUES (");
				sINSERTValues.Append(Environment.NewLine);

				#region Add Parametars
				foreach (Column colCurrent in SqlProvider.obj.Fields)
				{
					if (!allParameters.Contains(colCurrent.Name))
						continue;
					sParamDeclaration.AppendFormat("    @{0} {1}", new string[] { Globals.GetProgramatlyName(colCurrent.Name), colCurrent.Datatype });

					if (SqlProvider.obj.ID != null && colCurrent.Name == SqlProvider.obj.ID.Name && Globals.CheckIsAddedBySql(colCurrent))
						sParamDeclaration.AppendFormat(" out");
					// Only binary, char, nchar, nvarchar, varbinary and varchar may have their length declared								
					if (
						colCurrent.Datatype == "binary" ||
						colCurrent.Datatype == "char" ||
						colCurrent.Datatype == "nchar" ||
						colCurrent.Datatype == "nvarchar" ||
						colCurrent.Datatype == "varbinary" ||
						colCurrent.Datatype == "varchar")
						sParamDeclaration.AppendFormat("({0})", colCurrent.Length);

					sParamDeclaration.Append(",");
					sParamDeclaration.Append(Environment.NewLine);

					// Body construction

					//not Added BySQL
					if (SqlProvider.obj.ID == null || colCurrent.Name != SqlProvider.obj.ID.Name || !Globals.CheckIsAddedBySql(colCurrent))
					{
						if (!whereParameters.Contains(colCurrent.Name))
						{
							sINSERTValues.AppendFormat("    @{0},", Globals.GetProgramatlyName(colCurrent.Name));
							sINSERTValues.Append(Environment.NewLine);

							sBody.AppendFormat("    [{0}],", colCurrent.Name);
							sBody.Append(Environment.NewLine);
						}
					}

					if (SqlProvider.obj.ID != null && colCurrent.Name == SqlProvider.obj.ID.Name && Globals.CheckIsAddedBySql(colCurrent))
					{
						finnal = "Set @" + Globals.GetProgramatlyName(colCurrent.Name) + " = @@Identity";
					}
				}

				#endregion
				// Now stitch the body parts together into the SP whole			
				sGeneratedCode.Append(sParamDeclaration.Remove(sParamDeclaration.Length - 3, 3));
				sGeneratedCode.Append(Environment.NewLine);
				sGeneratedCode.Append("AS");
				sGeneratedCode.Append(Environment.NewLine);
				sGeneratedCode.Append(sBody.Remove(sBody.Length - 3, 3));

				sGeneratedCode.Append(")");
				sGeneratedCode.Append(Environment.NewLine);
				sGeneratedCode.Append(sINSERTValues.Remove(sINSERTValues.Length - 3, 3));
				sGeneratedCode.Append(")");

				//
				sGeneratedCode.Append(Environment.NewLine);
				sGeneratedCode.Append(CreateWhereStatement(whereParameters));
				//
				sGeneratedCode.Append(Environment.NewLine);
				sGeneratedCode.Append(finnal);



				//WriteStoredProcedure(sGeneratedCode.ToString());
				return sGeneratedCode.ToString();

			}
			catch (Exception ex)
			{
				MessageBox.Show("My Generated Code Exception:" + ex.Message);
				return "";
			}

		}
		//---------------------------------
		/// <summary>
		/// Generates code for an UPDATE or INSERT Stored Procedure
		/// </summary>
		/// <param name="sptypeGenerate">The type of SP to generate, INSERT or UPDATE</param>
		/// <param name="Fields">A ColumnCollection collection</param>
		/// <returns>The SP code</returns>
		private string GenerateUpdatePrcedure(Hashtable allParameters, Hashtable whereParameters)
		{
			try
			{


				StringBuilder sGeneratedCode = new StringBuilder();
				StringBuilder sParamDeclaration = new StringBuilder();
				StringBuilder sBody = new StringBuilder();
				StringBuilder sINSERTValues = new StringBuilder();

				// Setup SP code, begining is the same no matter the type
				sGeneratedCode.AppendFormat("CREATE PROCEDURE {0}.[{1}_{2}]", new string[] { SqlProvider.obj.DatabaseOwner, Globals.GetProgramatlyName(SqlProvider.obj.TableName), ProcedureAndMethodName });
				sGeneratedCode.Append(Environment.NewLine);

				// Setup body code, different for UPDATE and INSERT
				sBody.AppendFormat("UPDATE [{0}]", SqlProvider.obj.TableName);
				sBody.Append(Environment.NewLine);
				sBody.Append("SET");
				sBody.Append(Environment.NewLine);

				#region Add Parametars

				foreach (Column colCurrent in SqlProvider.obj.Fields)
				{
					if (!allParameters.Contains(colCurrent.Name))
						continue;
					sParamDeclaration.AppendFormat("    @{0} {1}", new string[] { Globals.GetProgramatlyName(colCurrent.Name), colCurrent.Datatype });


					// Only binary, char, nchar, nvarchar, varbinary and varchar may have their length declared								
					if (
						colCurrent.Datatype == "binary" ||
						colCurrent.Datatype == "char" ||
						colCurrent.Datatype == "nchar" ||
						colCurrent.Datatype == "nvarchar" ||
						colCurrent.Datatype == "varbinary" ||
						colCurrent.Datatype == "varchar")
						sParamDeclaration.AppendFormat("({0})", colCurrent.Length);

					sParamDeclaration.Append(",");
					sParamDeclaration.Append(Environment.NewLine);

					// Body construction, different for INSERT and UPDATE

					if (SqlProvider.obj.ID == null || colCurrent.Name != SqlProvider.obj.ID.Name)
					{

						sBody.AppendFormat("    [{0}] = @{1},", new string[] { colCurrent.Name, Globals.GetProgramatlyName(colCurrent.Name) });
						sBody.Append(Environment.NewLine);

					}
				}

				#endregion

				// Now stitch the body parts together into the SP whole			
				sGeneratedCode.Append(sParamDeclaration.Remove(sParamDeclaration.Length - 3, 3));
				sGeneratedCode.Append(Environment.NewLine);
				sGeneratedCode.Append("AS");
				sGeneratedCode.Append(Environment.NewLine);
				sGeneratedCode.Append(sBody.Remove(sBody.Length - 3, 3));

				sGeneratedCode.Append(Environment.NewLine);
				//
				sGeneratedCode.Append(Environment.NewLine);
				sGeneratedCode.Append(CreateWhereStatement(whereParameters));


				//WriteStoredProcedure(sGeneratedCode.ToString());
				return sGeneratedCode.ToString();
			}
			catch (Exception ex)
			{
				MessageBox.Show("My Generated Code Exception:" + ex.Message);
				return "";

			}
		}
		//---------------------------------
		private void WriteStoredProcedure(string procedure)
		{
			SqlProvider.obj.ExecuteNonQuery(procedure);
		}


		private void lvWhereparameter_ItemChecked(object sender, ItemCheckedEventArgs e)
		{
			whereParameters.Clear();
			ListViewItem lvItem = e.Item;

			foreach (ListViewItem item in lvOperationParametars.Items)
			{
				if (item.Text == lvItem.Text)
				{
					if (lvItem.Checked)
						item.Checked = false;
				}
					

			}

		}

		private void txtProcedureAndMethodName_Leave(object sender, EventArgs e)
		{
			ProcedureAndMethodName = txtProcedureAndMethodName.Text;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			txtProcedure.Text = GenerateProcedure(ProcedureAndMethodName);
		}

		private void selOperationType_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (selOperationType.SelectedIndex == 0)
				lvWhereparameter.Enabled = false;
			else
				lvWhereparameter.Enabled = true;
		}

		private void lvOperationParametars_SelectedIndexChanged(object sender, EventArgs e)
		{

		}

		private void lvOperationParametars_ItemChecked(object sender, ItemCheckedEventArgs e)
		{
			allParameters.Clear();
		}
	}
}