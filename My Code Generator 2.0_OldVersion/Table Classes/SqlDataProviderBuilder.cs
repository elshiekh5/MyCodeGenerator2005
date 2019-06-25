using System;

using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Collections;
namespace SPGen
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class SqlDataProviderBuilder:Generator 
	{
        public SqlDataProviderBuilder()
		{
            ClassName = global.SqlDataProviderClass;
		}

        private string GeneateUsingBlock()
        {
            string Usingblock = "";
            Usingblock += "using System;\n";
            Usingblock += "using System.Collections;\n";
            Usingblock += "using System.Data;\n";
            Usingblock += "using System.Data.SqlClient;\n";
            Usingblock += "using System.Data.SqlTypes;\n";
            Usingblock += "using System.Configuration;\n";
            return Usingblock;
        }
        //
        private string GenerateClassBody()
        {
            StringBuilder MethodsBuilder = new StringBuilder();
            MethodsBuilder.Append(CreateInstanceProprety());
            MethodsBuilder.Append("\n" + CreateGetConnectionMethod());
            MethodsBuilder.Append("\n" + CreateInsertUpdateDeleteMethod(StoredProcedureTypes.Create));
            MethodsBuilder.Append("\n" + CreateInsertUpdateDeleteMethod(StoredProcedureTypes.Update));
			MethodsBuilder.Append("\n" + CreateInsertUpdateDeleteMethod(StoredProcedureTypes.Delete));
            MethodsBuilder.Append("\n" + CreateGetAllMethod(StoredProcedureTypes.GetAll));
            if (ID != null)
                MethodsBuilder.Append("\n" + CreateGetOneMethod(StoredProcedureTypes.GetOneByID));
            MethodsBuilder.Append("\n" + CreatePopulateObjectMethod());
            return MethodsBuilder.ToString();
        }
        //
        private string GenerateClass(string classBody)
        {
            string xmlDocumentation ="/// <summary>\n";
            xmlDocumentation += "/// " + SqlProvider.obj.TableName + " SQL data provider which represents the data access layer of " + SqlProvider.obj.TableName + ".\n";
            xmlDocumentation += "/// </summary>\n";
            string _class = "";
            _class += GeneateUsingBlock();
            _class += xmlDocumentation;
            _class += "public class " + ClassName;
            _class += "\n{\n" + classBody + "\n}";
            return _class;
        }
        //
		public static void Create()
		{
			SqlDataProviderBuilder dp=new SqlDataProviderBuilder();
            dp.CreateClassFile();
		}
        //
		private void CreateClassFile()
		{
			try
			{
                string _class = GenerateClass(GenerateClassBody());
				DirectoryInfo dInfo= Directory.CreateDirectory(Globals.ClassesDirectory+global.TableProgramatlyName);
				string path = dInfo.FullName+"\\"+ClassName+".cs";
				// Create a file to write to.
				using (StreamWriter sw = File.CreateText(path)) 
				{
					
					sw.WriteLine(_class);				
				}    
				
			}
			catch(Exception ex)
			{
				MessageBox.Show("My Generated Code Exception:"+ex.Message);
				
			}
		}
        //
        #region ClassMember
        private string CreateInstanceProprety()
        {
            try
            {
                string xmlDocumentation = "\t/// <summary>\n";
                xmlDocumentation += "\t/// Gets instance of " + ClassName + " calss.\n";
                xmlDocumentation += "\t/// <example>" + ClassName + " edp=" + ClassName + ".Instance.</example>\n";
                xmlDocumentation += "\t/// </summary>";
                
                
                StringBuilder methodBody = new StringBuilder();
                methodBody.Append(xmlDocumentation);
                methodBody.Append("\n\tpublic static " + ClassName + "  Instance");
                methodBody.Append("\n\t{");
                methodBody.Append("\n\t\tget{");
                methodBody.Append("\n\t\t\treturn new " + ClassName + "();");
                methodBody.Append("\n\t\t} ");
                methodBody.Append("\n\t} ");
                methodBody.Append("\n\t" + Globals.MetthodsSeparator);
                return methodBody.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("My Generated Code Exception:" + ex.Message);
                return "";
            }
        }

        private string CreateGetConnectionMethod()
        {
            try
            {
                //XML Documentaion
                string xmlDocumentation = "\t/// <summary>\n";
                xmlDocumentation += "\t/// Creates and returns a new SqlConnection Which its connection string depends on AppSettings[\"Connectionstring\"].\n";
                xmlDocumentation += "\t/// </summary>\n";
                xmlDocumentation += "\t/// <returns></returns>";
                //Method Body 
                StringBuilder methodBody = new StringBuilder();
                methodBody.Append(xmlDocumentation);
                methodBody.Append("\n\tpublic SqlConnection GetSqlConnection()");
                methodBody.Append("\n\t{");
                methodBody.Append("\n\t\treturn new SqlConnection(ConfigurationSettings.AppSettings[\"Connectionstring\"]);");
                methodBody.Append("\n\t} ");
                methodBody.Append("\n\t" + Globals.MetthodsSeparator);
                return methodBody.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("My Generated Code Exception:" + ex.Message);
                return "";
            }
        }

        private string CreateInsertUpdateDeleteMethod(StoredProcedureTypes type)
        {
			string id = Globals.GetProgramatlyName(ID.Name);
			id = Globals.ConvetStringToCamelCase(id);
			//
            string ProcName = global.TableProgramatlyName + "_" + type.ToString();
			string MethodName = MethodName = type.ToString() ;
			string MethodReturn = "bool";
            //XML Documentaion
            string xmlInsertDocumentation = "\t/// <summary>\n";
            xmlInsertDocumentation += "\t/// Converts the " + SqlProvider.obj.TableName + " object properties to SQL paramters and executes the create " + SqlProvider.obj.TableName + " procedure \n";
            xmlInsertDocumentation += "\t/// and updates the " + SqlProvider.obj.TableName + " object with the SQL data by reference.\n";
            xmlInsertDocumentation += "\t/// <example>[Example]bool result=" + ClassName + ".Instance." + MethodName + "(" + global.EntityClassObject + ");.</example>\n";
            xmlInsertDocumentation += "\t/// </summary>\n";
            xmlInsertDocumentation += "\t/// <param name=\"" + global.EntityClassObject + "\">The " + SqlProvider.obj.TableName + " object.</param>\n";
            xmlInsertDocumentation += "\t/// <returns>The result of create query.</returns>";
            //
            string xmlUpdateDocumentation = "\t/// <summary>\n";
            xmlUpdateDocumentation += "\t/// Converts the " + SqlProvider.obj.TableName + " object properties to SQL paramters and executes the update " + SqlProvider.obj.TableName + " procedure.\n";
			xmlUpdateDocumentation += "\t/// <example>[Example]bool result=" + ClassName + ".Instance." + MethodName + "(" + global.EntityClassObject + ");.</example>\n";
            xmlUpdateDocumentation += "\t/// </summary>\n";
            xmlUpdateDocumentation += "\t/// <param name=\"" + global.EntityClassObject + "\">The " + SqlProvider.obj.TableName + " object.</param>\n";
            xmlUpdateDocumentation += "\t/// <returns>The result of update query.</returns>";
			//
			string xmlDeleteDocumentation = "\t/// <summary>\n";
			xmlDeleteDocumentation += "\t/// Deletes single " + SqlProvider.obj.TableName + " object .\n";
			xmlDeleteDocumentation += "\t/// <example>[Example]bool result=" + ClassName + ".Instance." + MethodName + "(" + id + ");.</example>\n";
			xmlDeleteDocumentation += "\t/// </summary>\n";
			xmlDeleteDocumentation += "\t/// <param name=\"" + id + "\">The " + global.EntityClassObject + " id.</param>\n";
			xmlDeleteDocumentation += "\t/// <returns>The result of delete query.</returns>";
           	//method body
            try
            {
                StringBuilder methodBody = new StringBuilder();
                if (type == StoredProcedureTypes.Create)
                    methodBody.Append(xmlInsertDocumentation);
				else if (type == StoredProcedureTypes.Update)
                    methodBody.Append(xmlUpdateDocumentation);
				else if(type == StoredProcedureTypes.Delete)
					methodBody.Append(xmlDeleteDocumentation);
				//
				if(type == StoredProcedureTypes.Delete)
					methodBody.Append("\n\tpublic  " + MethodReturn + " " + MethodName + "(" + Globals.GetAliasDataType(ID.Datatype) + " " + id + ")");
				else
					methodBody.Append("\n\tpublic  " + MethodReturn + " " + MethodName + "(" + global.TableEntityClass + " " + global.EntityClassObject + ")");
                methodBody.Append("\n\t{");
                methodBody.Append("\n\t\tbool result=false;");
                methodBody.Append("\n\t\tusing( SqlConnection myConnection = GetSqlConnection()) ");
                methodBody.Append("\n\t\t{");
                methodBody.Append("\n\t\t\tSqlCommand myCommand = new SqlCommand(\"" + ProcName + "\", myConnection);");
                methodBody.Append("\n\t\t\tmyCommand.CommandType = CommandType.StoredProcedure;");
                methodBody.Append("\n\t\t\t// Set the parameters");
				if (type == StoredProcedureTypes.Delete)
				{
					methodBody.Append("\n\t\t\tmyCommand.Parameters.Add(\"@" + Globals.GetProgramatlyName(ID.Name) + "\", SqlDbType." + Globals.GetSqlDataType(ID.Datatype).ToString() + "," + ID.Length + ").Value = " + id + ";");
				}
				else
				{
					foreach (Column colCurrent in Fields)
					{
						if (ID != null && colCurrent.Name == ID.Name && type == StoredProcedureTypes.Create && ID.IdentityIncrement > 0)
							methodBody.Append("\n\t\t\tmyCommand.Parameters.Add(\"@" + Globals.GetProgramatlyName(colCurrent.Name) + "\", SqlDbType." + Globals.GetSqlDataType(colCurrent.Datatype).ToString() + "," + colCurrent.Length + ").Direction = ParameterDirection.Output;");
                        else if (colCurrent.Datatype.ToLower() == SqlDbType.NText.ToString().ToLower())
                            methodBody.Append("\n\t\t\tmyCommand.Parameters.Add(\"@" + Globals.GetProgramatlyName(colCurrent.Name) + "\", SqlDbType." + Globals.GetSqlDataType(colCurrent.Datatype).ToString() + ").Value = " + global.EntityClassObject + "." + Globals.GetProgramatlyName(colCurrent.Name) + ";");
                        else
							methodBody.Append("\n\t\t\tmyCommand.Parameters.Add(\"@" + Globals.GetProgramatlyName(colCurrent.Name) + "\", SqlDbType." + Globals.GetSqlDataType(colCurrent.Datatype).ToString() + "," + colCurrent.Length + ").Value = " + global.EntityClassObject + "." + Globals.GetProgramatlyName(colCurrent.Name) + ";");
					}
				}
                methodBody.Append("\n\t\t\t// Execute the command");
                methodBody.Append("\n\t\t\tmyConnection.Open();");
                methodBody.Append("\n\t\t\tif(myCommand.ExecuteNonQuery()>0)");
                methodBody.Append("\n\t\t\t{");
                methodBody.Append("\n\t\t\t\tresult=true;");
                if (ID != null && type == StoredProcedureTypes.Create && ID.IdentityIncrement > 0)
                {
                    methodBody.Append("\n\t\t\t\t//Get ID value from database and set it in object");
                    methodBody.Append("\n\t\t\t\t" + global.EntityClassObject + "." + Globals.GetProgramatlyName(ID.Name) + "= (" + Globals.GetAliasDataType(ID.Datatype) + ") myCommand.Parameters[\"@" + Globals.GetProgramatlyName(ID.Name) + "\"].Value;");
                }
                if (ID.Name.ToLower() == "id" && global.EntityClassObject == "Site_RolesObj")
                    MessageBox.Show(SqlProvider.obj.ID.Name);
                methodBody.Append("\n\t\t\t}");
                methodBody.Append("\n\t\t\tmyConnection.Close();");
                methodBody.Append("\n\t\t\treturn result;");
                methodBody.Append("\n\t\t}");
                methodBody.Append("\n\t}");
                methodBody.Append("\n\t" + Globals.MetthodsSeparator);
                return methodBody.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("My Generated Code Exception:" + ex.Message);
                return "";
            }
        }

		//Scimple Creation Method-----------------------------------
		public  string CreateInsertUpdateDeleteMethod(StoredProcedureTypes type, string MethodName, Hashtable allParameters)
		{
			string id = Globals.GetProgramatlyName(ID.Name);
			id = Globals.ConvetStringToCamelCase(id);
			//
			string ProcName = global.TableProgramatlyName + "_" + MethodName;
			string MethodReturn = "bool";
			//XML Documentaion
			string xmlInsertDocumentation = "\t/// <summary>\n";
			xmlInsertDocumentation += "\t/// Converts the " + SqlProvider.obj.TableName + " object properties to SQL paramters and executes the create " + SqlProvider.obj.TableName + " procedure \n";
			xmlInsertDocumentation += "\t/// and updates the " + SqlProvider.obj.TableName + " object with the SQL data by reference.\n";
			xmlInsertDocumentation += "\t/// <example>[Example]bool result=" + ClassName + ".Instance." + MethodName + "(" + global.EntityClassObject + ");.</example>\n";
			xmlInsertDocumentation += "\t/// </summary>\n";
			xmlInsertDocumentation += "\t/// <param name=\"" + global.EntityClassObject + "\">The " + SqlProvider.obj.TableName + " object.</param>\n";
			xmlInsertDocumentation += "\t/// <returns>The result of create query.</returns>";
			//
			string xmlUpdateDocumentation = "\t/// <summary>\n";
			xmlUpdateDocumentation += "\t/// Converts the " + SqlProvider.obj.TableName + " object properties to SQL paramters and executes the update " + SqlProvider.obj.TableName + " procedure.\n";
			xmlUpdateDocumentation += "\t/// <example>[Example]bool result=" + ClassName + ".Instance." + MethodName + "(" + global.EntityClassObject + ");.</example>\n";
			xmlUpdateDocumentation += "\t/// </summary>\n";
			xmlUpdateDocumentation += "\t/// <param name=\"" + global.EntityClassObject + "\">The " + SqlProvider.obj.TableName + " object.</param>\n";
			xmlUpdateDocumentation += "\t/// <returns>The result of update query.</returns>";
			//
			string xmlDeleteDocumentation = "\t/// <summary>\n";
			xmlDeleteDocumentation += "\t/// Deletes single " + SqlProvider.obj.TableName + " object .\n";
			xmlDeleteDocumentation += "\t/// <example>[Example]bool result=" + ClassName + ".Instance." + MethodName + "(" + id + ");.</example>\n";
			xmlDeleteDocumentation += "\t/// </summary>\n";
			xmlDeleteDocumentation += "\t/// <param name=\"" + id + "\">The " + global.EntityClassObject + " id.</param>\n";
			xmlDeleteDocumentation += "\t/// <returns>The result of delete query.</returns>";
			//method body
			try
			{
				StringBuilder methodBody = new StringBuilder();
				if (type == StoredProcedureTypes.Create)
					methodBody.Append(xmlInsertDocumentation);
				else if (type == StoredProcedureTypes.Update)
					methodBody.Append(xmlUpdateDocumentation);
				else if (type == StoredProcedureTypes.Delete)
					methodBody.Append(xmlDeleteDocumentation);
				//
				if (type == StoredProcedureTypes.Delete)
					methodBody.Append("\n\tpublic  " + MethodReturn + " " + MethodName + "(" + Globals.GetAliasDataType(ID.Datatype) + " " + id + ")");
				else
					methodBody.Append("\n\tpublic  " + MethodReturn + " " + MethodName + "(" + global.TableEntityClass + " " + global.EntityClassObject + ")");
				methodBody.Append("\n\t{");
				methodBody.Append("\n\t\tbool result=false;");
				methodBody.Append("\n\t\tusing( SqlConnection myConnection = GetSqlConnection()) ");
				methodBody.Append("\n\t\t{");
				methodBody.Append("\n\t\t\tSqlCommand myCommand = new SqlCommand(\"" + ProcName + "\", myConnection);");
				methodBody.Append("\n\t\t\tmyCommand.CommandType = CommandType.StoredProcedure;");
				methodBody.Append("\n\t\t\t// Set the parameters");
				if (type == StoredProcedureTypes.Delete)
				{
					methodBody.Append("\n\t\t\tmyCommand.Parameters.Add(\"@" + Globals.GetProgramatlyName(ID.Name) + "\", SqlDbType." + Globals.GetSqlDataType(ID.Datatype).ToString() + "," + ID.Length + ").Value = " + id + ";");
				}
				else
				{
					foreach (Column colCurrent in Fields)
					{
						if (!allParameters.Contains(colCurrent.Name))
							continue;
						if (ID != null && colCurrent.Name == ID.Name && type == StoredProcedureTypes.Create && ID.IdentityIncrement > 0)
							methodBody.Append("\n\t\t\tmyCommand.Parameters.Add(\"@" + Globals.GetProgramatlyName(colCurrent.Name) + "\", SqlDbType." + Globals.GetSqlDataType(colCurrent.Datatype).ToString() + "," + colCurrent.Length + ").Direction = ParameterDirection.Output;");
						else if (colCurrent.Datatype.ToLower() == SqlDbType.NText.ToString().ToLower())
							methodBody.Append("\n\t\t\tmyCommand.Parameters.Add(\"@" + Globals.GetProgramatlyName(colCurrent.Name) + "\", SqlDbType." + Globals.GetSqlDataType(colCurrent.Datatype).ToString() + ").Value = " + global.EntityClassObject + "." + Globals.GetProgramatlyName(colCurrent.Name) + ";");
						else
							methodBody.Append("\n\t\t\tmyCommand.Parameters.Add(\"@" + Globals.GetProgramatlyName(colCurrent.Name) + "\", SqlDbType." + Globals.GetSqlDataType(colCurrent.Datatype).ToString() + "," + colCurrent.Length + ").Value = " + global.EntityClassObject + "." + Globals.GetProgramatlyName(colCurrent.Name) + ";");
					}
				}
				methodBody.Append("\n\t\t\t// Execute the command");
				methodBody.Append("\n\t\t\tmyConnection.Open();");
				methodBody.Append("\n\t\t\tif(myCommand.ExecuteNonQuery()>0)");
				methodBody.Append("\n\t\t\t{");
				methodBody.Append("\n\t\t\t\tresult=true;");
				if (ID != null && type == StoredProcedureTypes.Create && ID.IdentityIncrement > 0)
				{
					methodBody.Append("\n\t\t\t\t//Get ID value from database and set it in object");
					methodBody.Append("\n\t\t\t\t" + global.EntityClassObject + "." + Globals.GetProgramatlyName(ID.Name) + "= (" + Globals.GetAliasDataType(ID.Datatype) + ") myCommand.Parameters[\"@" + Globals.GetProgramatlyName(ID.Name) + "\"].Value;");
				}
				if (ID.Name.ToLower() == "id" && global.EntityClassObject == "Site_RolesObj")
					MessageBox.Show(SqlProvider.obj.ID.Name);
				methodBody.Append("\n\t\t\t}");
				methodBody.Append("\n\t\t\tmyConnection.Close();");
				methodBody.Append("\n\t\t\treturn result;");
				methodBody.Append("\n\t\t}");
				methodBody.Append("\n\t}");
				methodBody.Append("\n\t" + Globals.MetthodsSeparator);
				return methodBody.ToString();
			}
			catch (Exception ex)
			{
				MessageBox.Show("My Generated Code Exception:" + ex.Message);
				return "";
			}
		}
		//----------------------------------------------------------
        private string CreateGetAllMethod(StoredProcedureTypes type)
        {
            try
            {
                string ProcName = global.TableProgramatlyName + "_" + type.ToString();
                string MethodName = type.ToString() + global.TableProgramatlyName;
                string MethodReturn = "DataTable";
                //XML Documentaion
                string xmlDocumentation = "\t/// <summary>\n";
                xmlDocumentation += "\t/// Gets All " + SqlProvider.obj.TableName + " Records.\n";
                xmlDocumentation += "\t/// <example>[Example]DataTable dt" + global.TableProgramatlyName + "=" + ClassName + ".Instance." + MethodName + "();.</example>\n";
                xmlDocumentation += "\t/// </summary>\n";
                xmlDocumentation += "\t/// <returns>The result of query.</returns>";
                //Method Body
                
                //
                StringBuilder methodBody = new StringBuilder();
                methodBody.Append(xmlDocumentation);
                methodBody.Append("\n\tpublic  " + MethodReturn + " " + MethodName + "()");
                methodBody.Append("\n\t{");
                methodBody.Append("\n\t\tusing( SqlConnection myConnection = GetSqlConnection()) ");
                methodBody.Append("\n\t\t{");
                methodBody.Append("\n\t\t\tSqlCommand myCommand = new SqlCommand(\"" + ProcName + "\", myConnection);");
                methodBody.Append("\n\t\t\tmyCommand.CommandType = CommandType.StoredProcedure;");
                methodBody.Append("\n\t\t\tSqlDataAdapter da=new SqlDataAdapter(myCommand);");
                methodBody.Append("\n\t\t\tDataTable dt=new DataTable();");
                methodBody.Append("\n\t\t\t// Execute the command");
                methodBody.Append("\n\t\t\tmyConnection.Open();");
                methodBody.Append("\n\t\t\tda.Fill(dt);");

                methodBody.Append("\n\t\t\tmyConnection.Close();");
                methodBody.Append("\n\t\t\treturn dt;");
                methodBody.Append("\n\t\t}");
                methodBody.Append("\n\t}");
                methodBody.Append("\n\t" + Globals.MetthodsSeparator);
                return methodBody.ToString();
            }
            catch (Exception ex)
            {

                MessageBox.Show("My Generated Code Exception:" + ex.Message);
                return "";

            }

        }

        private string CreateGetOneMethod(StoredProcedureTypes type)
        {
            if (ID != null)
            {
                try
                {
					string id = Globals.GetProgramatlyName(ID.Name);
					id = Globals.ConvetStringToCamelCase(id);
					//
                    string ProcName = global.TableProgramatlyName + "_" + type.ToString();
                    string MethodName = "Get" + global.TableProgramatlyName + "Object";
                    string MethodReturn = global.TableEntityClass;
                    //XML Documentaion
                    string xmlDocumentation = "\t/// <summary>\n";
                    xmlDocumentation += "\t/// Gets single " + SqlProvider.obj.TableName + " object .\n";
					xmlDocumentation += "\t/// <example>[Example]" + global.TableEntityClass +" "+ global.EntityClassObject + "=" + ClassName + ".Instance." + MethodName + "(" + id + ");.</example>\n";
                    xmlDocumentation += "\t/// </summary>\n";
					xmlDocumentation += "\t/// <param name=\"" + id + "\">The " + global.EntityClassObject + " id.</param>\n";
                    xmlDocumentation += "\t/// <returns>" + SqlProvider.obj.TableName + " object.</returns>";
                    //Method Body
                    StringBuilder methodBody = new StringBuilder();
                    methodBody.Append(xmlDocumentation);
					methodBody.Append("\n\tpublic  " + MethodReturn + " " + MethodName + "(" + Globals.GetAliasDataType(ID.Datatype) + " " + id + ")");
                    methodBody.Append("\n\t{");
                    methodBody.Append("\n\t\t" + global.TableEntityClass + " " + global.EntityClassObject + " = null;");
                    methodBody.Append("\n\t\tusing( SqlConnection myConnection = GetSqlConnection()) ");
                    methodBody.Append("\n\t\t{");
                    methodBody.Append("\n\t\t\tSqlCommand myCommand = new SqlCommand(\"" + ProcName + "\", myConnection);");
                    methodBody.Append("\n\t\t\tmyCommand.CommandType = CommandType.StoredProcedure;");
                    methodBody.Append("\n\t\t\t// Set the parameters");

					methodBody.Append("\n\t\t\tmyCommand.Parameters.Add(\"@" + Globals.GetProgramatlyName(ID.Name) + "\", SqlDbType." + Globals.GetSqlDataType(ID.Datatype).ToString() + "," + ID.Length + ").Value = " + id + ";");
                    methodBody.Append("\n\t\t\t// Execute the command");
                    methodBody.Append("\n\t\t\tmyConnection.Open();");
                    methodBody.Append("\n\t\t\tusing(SqlDataReader dr = myCommand.ExecuteReader(CommandBehavior.CloseConnection))");
                    methodBody.Append("\n\t\t\t{");
                    methodBody.Append("\n\t\t\t\tif(dr.Read())");
                    methodBody.Append("\n\t\t\t\t{");
                    methodBody.Append("\n\t\t\t\t\t" + global.EntityClassObject + " = " + global.PopulateMethodName + "(dr);");
                    methodBody.Append("\n\t\t\t\t}");
                    methodBody.Append("\n\t\t\t\tdr.Close();");
                    methodBody.Append("\n\t\t}");
                    methodBody.Append("\n\t\t\tmyConnection.Close();");
                    //-------------------------------------


                    methodBody.Append("\n\t\t\treturn " + global.EntityClassObject + ";");
                    methodBody.Append("\n\t\t}");
                    methodBody.Append("\n\t}");
                    methodBody.Append("\n\t" + Globals.MetthodsSeparator);
                    return methodBody.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("My Generated Code Exception:" + ex.Message);
                    return "";
                }
            }
            else
                return "";
        }

        private string CreatePopulateObjectMethod()
        {
            try
            {
                // 
                string MethodName = "Populate" + global.TableEntityClass + "FromIDataReader";
                string MethodReturn =global.TableEntityClass;
                //XML Documentaion
                string xmlDocumentation = "\t/// <summary>\n";
                xmlDocumentation += "\t/// Populates " + SqlProvider.obj.TableName + " Entity From IDataReader .\n";
                xmlDocumentation += "\t/// <example>[Example]" + global.TableEntityClass + global.EntityClassObject + "=" + MethodName + "(reader);.</example>\n";
                xmlDocumentation += "\t/// </summary>\n";
                xmlDocumentation += "\t/// <param name=\"reader\"></param>\n";
                xmlDocumentation += "\t/// <returns>" + SqlProvider.obj.TableName + " object.</returns>";
                //Method Body
                StringBuilder methodBody = new StringBuilder();
                methodBody.Append(xmlDocumentation);
                methodBody.Append("\n\tprivate " + global.TableEntityClass + " " + MethodName + "(IDataReader reader)");
                methodBody.Append("\n\t{");
                methodBody.Append("\n\t\t//Create a new " + SqlProvider.obj.TableName + " object");
                methodBody.Append("\n\t\t" + global.TableEntityClass + " " + global.EntityClassObject + " = new " + global.TableEntityClass + "();");
                methodBody.Append("\n\t\t//Fill the object with data");
                //
                foreach (Column colCurrent in Fields)
                {
                    methodBody.Append("\n\t\tif (reader[\"" + Globals.GetProgramatlyName(colCurrent.Name) + "\"] != DBNull.Value)");
                    methodBody.Append("\n\t\t\t" + global.EntityClassObject + "." + Globals.GetProgramatlyName(colCurrent.Name) + " = (" + Globals.GetAliasDataType(colCurrent.Datatype) + ") reader[\"" + Globals.GetProgramatlyName(colCurrent.Name) + "\"];");
                }
                //
                methodBody.Append("\n\t\t//Return the populated object");
                methodBody.Append("\n\t\treturn " + global.EntityClassObject + ";");
                methodBody.Append("\n\t}");
                methodBody.Append("\n\t" + Globals.MetthodsSeparator);
                return methodBody.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("My Generated Code Exception:" + ex.Message);
                return "";
            }
        }
        #endregion
	}
}

