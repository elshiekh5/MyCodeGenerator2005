using System;
using System.Data ;
using System.Data.SqlClient ;
using System.Text;
using System.Windows.Forms;
using System.IO;
namespace SPGen
{


	/// <summary>
	/// Stored Procedure Helper class
	/// </summary>
	public class StoredProcedure : Generator
	{
		public static void Create()
		{
			StoredProcedure sp = new StoredProcedure();
			sp.GenerateInsertProcedure();
			sp.GenerateUpdatePrcedure();
			sp.GenerateGetAllProcedure();
			sp.GenerateGetAll4ShowProcedure();
			if (sp.ID != null)
			{
				sp.GenerateGetOneProcedure();
				sp.GenerateGetOne4ShowProcedure();
			}
			sp.GenerateDeleteProcedure();
		}

		

		//---------------------------------

		/// <summary>
		/// Generates code for INSERT Stored Procedure
		/// </summary>
		/// <param name="sptypeGenerate">The type of SP to generate, INSERT</param>
		/// <param name="Fields">A SQLDMO.Columns collection</param>
		/// <returns>The SP code</returns>
		private void GenerateInsertProcedure()
		{
			try
			{
				StringBuilder sGeneratedCode = new StringBuilder();
				StringBuilder sParamDeclaration = new StringBuilder();
				StringBuilder sBody = new StringBuilder();
				StringBuilder sINSERTValues = new StringBuilder();
				string finnal = "";
				// Setup SP code, begining is the same no matter the type
				sGeneratedCode.AppendFormat("CREATE PROCEDURE {0}.[{1}_{2}]", new string[] { SqlProvider.obj.DatabaseOwner, Globals.GetProgramatlyName(Table), StoredProcedureTypes.Create.ToString() });
				sGeneratedCode.Append(Environment.NewLine);

				// Setup body code, different for UPDATE and INSERT
				sBody.AppendFormat("INSERT INTO [{0}] (", Table);
				sBody.Append(Environment.NewLine);
				sINSERTValues.Append("VALUES (");
				sINSERTValues.Append(Environment.NewLine);

				#region Add Parametars
				foreach (SQLDMO.Column colCurrent in Fields)
				{

					sParamDeclaration.AppendFormat("    @{0} {1}", new string[] { Globals.GetProgramatlyName(colCurrent.Name), colCurrent.Datatype });

					if (ID != null && colCurrent.Name == ID.Name && Globals.CheckIsAddedBySql(colCurrent))
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
					if (ID == null || colCurrent.Name != ID.Name || !Globals.CheckIsAddedBySql(colCurrent))
					{
						sINSERTValues.AppendFormat("    @{0},", Globals.GetProgramatlyName(colCurrent.Name));
						sINSERTValues.Append(Environment.NewLine);

						sBody.AppendFormat("    [{0}],", colCurrent.Name);
						sBody.Append(Environment.NewLine);
					}
				}
				if (ID != null && Globals.CheckIsAddedBySql(ID))
				{
					finnal = "Set @" + Globals.GetProgramatlyName(ID.Name) + " = @@Identity";
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
				sGeneratedCode.Append(finnal);


				WriteStoredProcedure(sGeneratedCode.ToString());

			}
			catch (Exception ex)
			{
				MessageBox.Show("My Generated Code Exception:" + ex.Message);

			}
		}

		//---------------------------------
		/// <summary>
		/// Generates code for an UPDATE  Stored Procedure
		/// </summary>
		/// <param name="sptypeGenerate">The type of SP to generate, INSERT or UPDATE</param>
		/// <param name="Fields">A SQLDMO.Columns collection</param>
		/// <returns>The SP code</returns>
		private void GenerateUpdatePrcedure()
		{
			try
			{


				StringBuilder sGeneratedCode = new StringBuilder();
				StringBuilder sParamDeclaration = new StringBuilder();
				StringBuilder sBody = new StringBuilder();
				StringBuilder sINSERTValues = new StringBuilder();
				string finnal = "";
				// Setup SP code, begining is the same no matter the type
				sGeneratedCode.AppendFormat("CREATE PROCEDURE {0}.[{1}_{2}]", new string[] { SqlProvider.obj.DatabaseOwner, Globals.GetProgramatlyName(Table), StoredProcedureTypes.Update.ToString() });
				sGeneratedCode.Append(Environment.NewLine);

				// Setup body code, different for UPDATE and INSERT
				sBody.AppendFormat("UPDATE [{0}]", Table);
				sBody.Append(Environment.NewLine);
				sBody.Append("SET");
				sBody.Append(Environment.NewLine);

				#region Add Parametars

				foreach (SQLDMO.Column colCurrent in Fields)
				{

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

					if (ID == null || colCurrent.Name != ID.Name)
					{

						sBody.AppendFormat("    [{0}] = @{1},", new string[] { colCurrent.Name, Globals.GetProgramatlyName(colCurrent.Name) });
						sBody.Append(Environment.NewLine);

					}
				}
				if (ID != null)
				{
					finnal = "Where    [" + ID.Name + "] =@" + Globals.GetProgramatlyName(ID.Name);
				}
				#endregion

				// Now stitch the body parts together into the SP whole			
				sGeneratedCode.Append(sParamDeclaration.Remove(sParamDeclaration.Length - 3, 3));
				sGeneratedCode.Append(Environment.NewLine);
				sGeneratedCode.Append("AS");
				sGeneratedCode.Append(Environment.NewLine);
				sGeneratedCode.Append(sBody.Remove(sBody.Length - 3, 3));

				sGeneratedCode.Append(Environment.NewLine);
				sGeneratedCode.Append(finnal);


				WriteStoredProcedure(sGeneratedCode.ToString());
			}
			catch (Exception ex)
			{
				MessageBox.Show("My Generated Code Exception:" + ex.Message);

			}
		}

		//---------------------------------
		/// <summary>
		/// Generates code for an GetAll Stored Procedure
		/// </summary>
		/// <param name="sptypeGenerate">The type of SP to generate, INSERT or UPDATE</param>
		/// <param name="Fields">A SQLDMO.Columns collection</param>
		/// <returns>The SP code</returns>
		private void GenerateGetAllProcedure()
		{
			try
			{
				StringBuilder sGeneratedCode = new StringBuilder();
				StringBuilder sParamDeclaration = new StringBuilder();
				StringBuilder sBody = new StringBuilder();
				StringBuilder sINSERTValues = new StringBuilder();
				string finnal = "";
				// Setup SP code, begining is the same no matter the type
				sGeneratedCode.AppendFormat("CREATE PROCEDURE {0}.[{1}_{2}]", new string[] { SqlProvider.obj.DatabaseOwner, Globals.GetProgramatlyName(Table), StoredProcedureTypes.GetAll.ToString() });
				sGeneratedCode.Append(Environment.NewLine);
				 

				// Setup body code, different for UPDATE and INSERT
				sBody.AppendFormat("Select * From [{0}] ", Table);
				sBody.Append(Environment.NewLine);

				sGeneratedCode.Append("AS");
				sGeneratedCode.Append(Environment.NewLine);
				sGeneratedCode.Append(sBody.Remove(sBody.Length - 3, 3));

				sGeneratedCode.Append(Environment.NewLine);
				sGeneratedCode.Append(finnal);

				WriteStoredProcedure(sGeneratedCode.ToString());
			}
			catch (Exception ex)
			{
				MessageBox.Show("My Generated Code Exception:" + ex.Message);

			}
		}
		//---------------------------------
		/// <summary>
		/// Generates code for an GetAll Stored Procedure
		/// </summary>
		/// <param name="sptypeGenerate">The type of SP to generate, INSERT or UPDATE</param>
		/// <param name="Fields">A SQLDMO.Columns collection</param>
		/// <returns>The SP code</returns>
		private void GenerateGetAll4ShowProcedure()
		{
			try
			{
				StringBuilder sGeneratedCode = new StringBuilder();
				StringBuilder sParamDeclaration = new StringBuilder();
				StringBuilder sBody = new StringBuilder();
				StringBuilder sINSERTValues = new StringBuilder();
				string finnal = "";
				// Setup SP code, begining is the same no matter the type
				sGeneratedCode.AppendFormat("CREATE PROCEDURE {0}.[{1}_{2}]", new string[] { SqlProvider.obj.DatabaseOwner, Globals.GetProgramatlyName(Table), StoredProcedureTypes.GetAll4Show.ToString() });
				sGeneratedCode.Append(Environment.NewLine);


				// Setup body code, different for UPDATE and INSERT
				sBody.AppendFormat("Select * From [{0}] ", Table);
				sBody.Append(Environment.NewLine);

				sGeneratedCode.Append("AS");
				sGeneratedCode.Append(Environment.NewLine);
				sGeneratedCode.Append(sBody.Remove(sBody.Length - 3, 3));

				sGeneratedCode.Append(Environment.NewLine);
				sGeneratedCode.Append(finnal);

				WriteStoredProcedure(sGeneratedCode.ToString());
			}
			catch (Exception ex)
			{
				MessageBox.Show("My Generated Code Exception:" + ex.Message);

			}
		}

		//---------------------------------
		/// <summary>
		/// Generates code for an Get One Stored Procedure
		/// </summary>
		/// <param name="sptypeGenerate">The type of SP to generate, INSERT or UPDATE</param>
		/// <param name="Fields">A SQLDMO.Columns collection</param>
		/// <returns>The SP code</returns>
		private void GenerateGetOneProcedure()
		{
			try
			{
				StringBuilder sGeneratedCode = new StringBuilder();
				StringBuilder sParamDeclaration = new StringBuilder();
				StringBuilder sBody = new StringBuilder();
				StringBuilder sINSERTValues = new StringBuilder();
				string finnal = "";
				// Setup SP code, begining is the same no matter the type
				sGeneratedCode.AppendFormat("CREATE PROCEDURE {0}.[{1}_{2}]", new string[] { SqlProvider.obj.DatabaseOwner, Globals.GetProgramatlyName(Table), StoredProcedureTypes.GetOneByID.ToString() });
				sGeneratedCode.Append(Environment.NewLine);

			 
				// Setup body code, different for UPDATE and INSERT

				sBody.AppendFormat("Select * From [{0}] ", Table);
				sBody.Append(Environment.NewLine);
				if (ID != null)
				{
					sParamDeclaration.AppendFormat("    @{0} {1}", new string[] { Globals.GetProgramatlyName(ID.Name), ID.Datatype });
					// Only binary, char, nchar, nvarchar, varbinary and varchar may have their length declared								
					if (
						ID.Datatype == "binary" ||
						ID.Datatype == "char" ||
						ID.Datatype == "nchar" ||
						ID.Datatype == "nvarchar" ||
						ID.Datatype == "varbinary" ||
						ID.Datatype == "varchar")
						sParamDeclaration.AppendFormat("({0})", ID.Length);

					sParamDeclaration.Append(",");
					sParamDeclaration.Append(Environment.NewLine);
					finnal = "Where    [" + ID.Name + "] = @" + Globals.GetProgramatlyName(ID.Name);
				}
			
				// Now stitch the body parts together into the SP whole			
				sGeneratedCode.Append(sParamDeclaration.Remove(sParamDeclaration.Length - 3, 3));
				sGeneratedCode.Append(Environment.NewLine);
				sGeneratedCode.Append("AS");
				sGeneratedCode.Append(Environment.NewLine);
				sGeneratedCode.Append(sBody.Remove(sBody.Length - 3, 3));

				sGeneratedCode.Append(Environment.NewLine);
				sGeneratedCode.Append(finnal);

				WriteStoredProcedure(sGeneratedCode.ToString());
			}
			catch (Exception ex)
			{
				MessageBox.Show("My Generated Code Exception:" + ex.Message);

			}
		}
		//---------------------------------
		/// <summary>
		/// Generates code for an Get One Stored Procedure
		/// </summary>
		/// <param name="sptypeGenerate">The type of SP to generate, INSERT or UPDATE</param>
		/// <param name="Fields">A SQLDMO.Columns collection</param>
		/// <returns>The SP code</returns>
		private void GenerateGetOne4ShowProcedure()
		{
			try
			{
				StringBuilder sGeneratedCode = new StringBuilder();
				StringBuilder sParamDeclaration = new StringBuilder();
				StringBuilder sBody = new StringBuilder();
				StringBuilder sINSERTValues = new StringBuilder();
				string finnal = "";
				 
				// Setup SP code, begining is the same no matter the type
				sGeneratedCode.AppendFormat("CREATE PROCEDURE {0}.[{1}_{2}]", new string[] { SqlProvider.obj.DatabaseOwner, Globals.GetProgramatlyName(Table), StoredProcedureTypes.GetOneByID4Show.ToString() });
				sGeneratedCode.Append(Environment.NewLine);

				// Setup body code, different for UPDATE and INSERT

				sBody.AppendFormat("Select * From [{0}] ", Table);
				sBody.Append(Environment.NewLine);
				if (ID != null)
				{
					sParamDeclaration.AppendFormat("    @{0} {1}", new string[] { Globals.GetProgramatlyName(ID.Name), ID.Datatype });
					// Only binary, char, nchar, nvarchar, varbinary and varchar may have their length declared								
					if (
						ID.Datatype == "binary" ||
						ID.Datatype == "char" ||
						ID.Datatype == "nchar" ||
						ID.Datatype == "nvarchar" ||
						ID.Datatype == "varbinary" ||
						ID.Datatype == "varchar")
						sParamDeclaration.AppendFormat("({0})", ID.Length);

					sParamDeclaration.Append(",");
					sParamDeclaration.Append(Environment.NewLine);
					finnal = "Where    [" + ID.Name + "] = @" + Globals.GetProgramatlyName(ID.Name);
				}

				// Now stitch the body parts together into the SP whole			
				sGeneratedCode.Append(sParamDeclaration.Remove(sParamDeclaration.Length - 3, 3));
				sGeneratedCode.Append(Environment.NewLine);
				sGeneratedCode.Append("AS");
				sGeneratedCode.Append(Environment.NewLine);
				sGeneratedCode.Append(sBody.Remove(sBody.Length - 3, 3));

				sGeneratedCode.Append(Environment.NewLine);
				sGeneratedCode.Append(finnal);

				WriteStoredProcedure(sGeneratedCode.ToString());
			}
			catch (Exception ex)
			{
				MessageBox.Show("My Generated Code Exception:" + ex.Message);

			}
		}
		//---------------------------------
		/// <summary>
		/// Generates code for delete Stored Procedure
		/// </summary>
		/// <param name="sptypeGenerate">The type of SP to generate, INSERT or UPDATE</param>
		/// <param name="Fields">A SQLDMO.Columns collection</param>
		/// <returns>The SP code</returns>
		private void GenerateDeleteProcedure()
		{
			try
			{
				StringBuilder sGeneratedCode = new StringBuilder();
				StringBuilder sParamDeclaration = new StringBuilder();
				StringBuilder sBody = new StringBuilder();
				StringBuilder sINSERTValues = new StringBuilder();
				string finnal = "";
				// Setup SP code, begining is the same no matter the type
				sGeneratedCode.AppendFormat("CREATE PROCEDURE {0}.[{1}_{2}]", new string[] { SqlProvider.obj.DatabaseOwner, Globals.GetProgramatlyName(Table), StoredProcedureTypes.Delete.ToString() });
				// Setup body code
				sBody.AppendFormat("Delete  From [{0}] ", Table);
				sBody.Append(Environment.NewLine);
				if (ID != null)
				{
					sParamDeclaration.AppendFormat("    @{0} {1}", new string[] { Globals.GetProgramatlyName(ID.Name), ID.Datatype });
					// Only binary, char, nchar, nvarchar, varbinary and varchar may have their length declared								
					if (
						ID.Datatype == "binary" ||
						ID.Datatype == "char" ||
						ID.Datatype == "nchar" ||
						ID.Datatype == "nvarchar" ||
						ID.Datatype == "varbinary" ||
						ID.Datatype == "varchar")
						sParamDeclaration.AppendFormat("({0})", ID.Length);

					sParamDeclaration.Append(",");
					sParamDeclaration.Append(Environment.NewLine);
					finnal = "Where    [" + ID.Name + "] = @" + Globals.GetProgramatlyName(ID.Name);
				}

				// Now stitch the body parts together into the SP whole		
				sGeneratedCode.Append(Environment.NewLine);
				sGeneratedCode.Append(sParamDeclaration.Remove(sParamDeclaration.Length - 3, 3));
				sGeneratedCode.Append(Environment.NewLine);
				sGeneratedCode.Append("AS");
				sGeneratedCode.Append(Environment.NewLine);
				sGeneratedCode.Append(sBody.Remove(sBody.Length - 3, 3));
				sGeneratedCode.Append(Environment.NewLine);
				sGeneratedCode.Append(finnal);

				WriteStoredProcedure(sGeneratedCode.ToString());
			}
			catch (Exception ex)
			{
				MessageBox.Show("My Generated Code Exception:" + ex.Message);

			}
		}
		//---------------------------------
		public static void  WriteStoredProcedure(string procedure)
		{
			SqlProvider.obj.ExecuteNonQuery(procedure);
		}
		//----------------------------------

		#region Old and Unused code
		/// <summary>
		/// Generates code for an UPDATE or INSERT Stored Procedure
		/// </summary>
		/// <param name="sptypeGenerate">The type of SP to generate, INSERT or UPDATE</param>
		/// <param name="Fields">A SQLDMO.Columns collection</param>
		/// <returns>The SP code</returns>
		private void Generate(StoredProcedureTypes sptypeGenerate, SqlProvider dmoMain)
		{
			try
			{
				StringBuilder sGeneratedCode = new StringBuilder();
				StringBuilder sParamDeclaration = new StringBuilder();
				StringBuilder sBody = new StringBuilder();
				StringBuilder sINSERTValues = new StringBuilder();
				string finnal = "";
				// Setup SP code, begining is the same no matter the type
				sGeneratedCode.AppendFormat("CREATE PROCEDURE {0}.[{1}_{2}]", new string[] { SqlProvider.obj.DatabaseOwner, Globals.GetProgramatlyName(Table), sptypeGenerate.ToString() });
				sGeneratedCode.Append(Environment.NewLine);

				// Setup body code, different for UPDATE and INSERT
				switch (sptypeGenerate)
				{
					case StoredProcedureTypes.Create:
						sBody.AppendFormat("INSERT INTO [{0}] (", Table);
						sBody.Append(Environment.NewLine);


						sINSERTValues.Append("VALUES (");
						sINSERTValues.Append(Environment.NewLine);
						break;

					case StoredProcedureTypes.Update:
						sBody.AppendFormat("UPDATE [{0}]", Table);
						sBody.Append(Environment.NewLine);
						sBody.Append("SET");
						sBody.Append(Environment.NewLine);
						break;
					case StoredProcedureTypes.GetAll:
						sBody.AppendFormat("Select * From [{0}] ", Table);
						sBody.Append(Environment.NewLine);
						break;
					case StoredProcedureTypes.GetAll4Show:
						sBody.AppendFormat("Select * From [{0}] ", Table);
						sBody.Append(Environment.NewLine);
						break;
					case StoredProcedureTypes.GetOneByID:
						sBody.AppendFormat("Select * From [{0}] ", Table);
						sBody.Append(Environment.NewLine);
						break;
					case StoredProcedureTypes.GetOneByID4Show:
						sBody.AppendFormat("Select * From [{0}] ", Table);
						sBody.Append(Environment.NewLine);
						break;
					case StoredProcedureTypes.Delete:
						sBody.AppendFormat("Delete * From [{0}] ", Table);
						sBody.Append(Environment.NewLine);
						break;
				}
				#region Add Parametars
				if (sptypeGenerate == StoredProcedureTypes.GetAll)
				{
					sGeneratedCode.Append("AS");
					sGeneratedCode.Append(Environment.NewLine);
					sGeneratedCode.Append(sBody.Remove(sBody.Length - 3, 3));


				}

				else if (sptypeGenerate == StoredProcedureTypes.GetAll4Show)
				{
					sGeneratedCode.Append("AS");
					sGeneratedCode.Append(Environment.NewLine);
					sGeneratedCode.Append(sBody.Remove(sBody.Length - 3, 3));
				}
				else if (sptypeGenerate != StoredProcedureTypes.GetOneByID || sptypeGenerate != StoredProcedureTypes.Delete || sptypeGenerate != StoredProcedureTypes.GetOneByID4Show)
				{
					//The finanal of procedure
					if (ID != null)
					{
						sParamDeclaration.AppendFormat("    @{0} {1}", new string[] { Globals.GetProgramatlyName(ID.Name), ID.Datatype });
						finnal = "Where    [" + ID.Name + "] =@" + Globals.GetProgramatlyName(ID.Name);
					}
				}
				else
				{
					foreach (SQLDMO.Column colCurrent in Fields)
					{
						if (ID != null && colCurrent.Name == ID.Name)
							sParamDeclaration.AppendFormat("    @{0} {1}", new string[] { Globals.GetProgramatlyName(colCurrent.Name), colCurrent.Datatype });
						if (ID != null && colCurrent.Name == ID.Name && Globals.CheckIsAddedBySql(colCurrent) && sptypeGenerate == StoredProcedureTypes.Create)
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

						// Body construction, different for INSERT and UPDATE
						switch (sptypeGenerate)
						{
							case StoredProcedureTypes.Create:
								//not Added BySQL
								if (ID == null && colCurrent.Name != ID.Name || !Globals.CheckIsAddedBySql(colCurrent))
								{
									sINSERTValues.AppendFormat("    @{0},", Globals.GetProgramatlyName(colCurrent.Name));
									sINSERTValues.Append(Environment.NewLine);

									sBody.AppendFormat("    [{0}],", colCurrent.Name);
									sBody.Append(Environment.NewLine);
								}
								break;

							case StoredProcedureTypes.Update:
								if (ID == null && colCurrent.Name != ID.Name)
								{
									sBody.AppendFormat("    [{0}] = @{1},", new string[] { colCurrent.Name, Globals.GetProgramatlyName(colCurrent.Name) });
									sBody.Append(Environment.NewLine);

								}
								break;
							case StoredProcedureTypes.GetOneByID:
								//							sBody.AppendFormat("Where    {0} = @{0},", new string[]{colCurrent.Name, });											
								sBody.Append(Environment.NewLine);
								break;
							case StoredProcedureTypes.GetOneByID4Show:
								//							sBody.AppendFormat("Where    {0} = @{0},", new string[]{colCurrent.Name, });											
								sBody.Append(Environment.NewLine);
								break;
						}
					}
					//The finanal of procedure
					if (ID != null)
					{
						if (sptypeGenerate == StoredProcedureTypes.Create && Globals.CheckIsAddedBySql(ID))
							finnal = "Set @" + Globals.GetProgramatlyName(ID.Name) + " = @@Identity";
						else if (sptypeGenerate == StoredProcedureTypes.Update)
							finnal = "Where    [" + ID.Name + "] =@" + Globals.GetProgramatlyName(ID.Name);
					}
				#endregion
					// Now stitch the body parts together into the SP whole			
					sGeneratedCode.Append(sParamDeclaration.Remove(sParamDeclaration.Length - 3, 3));
					sGeneratedCode.Append(Environment.NewLine);
					sGeneratedCode.Append("AS");
					sGeneratedCode.Append(Environment.NewLine);
					sGeneratedCode.Append(sBody.Remove(sBody.Length - 3, 3));
					if (sptypeGenerate == StoredProcedureTypes.Create)
					{
						sGeneratedCode.Append(")");
						sGeneratedCode.Append(Environment.NewLine);
						sGeneratedCode.Append(sINSERTValues.Remove(sINSERTValues.Length - 3, 3));
						sGeneratedCode.Append(")");
					}
				}
				sGeneratedCode.Append(Environment.NewLine);
				sGeneratedCode.Append(finnal);


				WriteStoredProcedure(sGeneratedCode.ToString());
			}
			catch (Exception ex)
			{
				MessageBox.Show("My Generated Code Exception:" + ex.Message);

			}
		}
		#endregion
	}
}
