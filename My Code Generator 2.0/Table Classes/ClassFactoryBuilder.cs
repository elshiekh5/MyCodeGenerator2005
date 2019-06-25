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
	public class ClassFactoryBuilder:Generator 
	{
		public ClassFactoryBuilder()
		{
			ClassName=global.TableFactoryClass;			
    	}

        private string GeneateUsingBlock()
        {
            string Usingblock = "";
            Usingblock += "using System;\n";
            Usingblock += "using System.Collections;\n";
            Usingblock += "using System.Data;\n";
            return Usingblock;
        }
        //
        private string GenerateClassBody()
        {
            StringBuilder MethodsBuilder = new StringBuilder();
            MethodsBuilder.Append("\n" + CreateInsertUpdateDeleteMethod(StoredProcedureTypes.Create));
            MethodsBuilder.Append("\n" + CreateInsertUpdateDeleteMethod(StoredProcedureTypes.Update));
			MethodsBuilder.Append("\n" + CreateInsertUpdateDeleteMethod(StoredProcedureTypes.Delete));
            MethodsBuilder.Append("\n" + CreateGetAllMethod(StoredProcedureTypes.GetAll));
			MethodsBuilder.Append("\n" + CreateGetAllMethod4Show(StoredProcedureTypes.GetAll4Show));
			if (ID != null)
			{
				MethodsBuilder.Append("\n" + CreateGetOneMethod(StoredProcedureTypes.GetOneByID));
				MethodsBuilder.Append("\n" + CreateGetOne4ShowMethod(StoredProcedureTypes.GetOneByID4Show)); 
			}
			MethodsBuilder.Append("\n" + CreatePopulateObjectMethod());
            return MethodsBuilder.ToString();
        }
        //
        private string GenerateClass(string classBody)
        {
            string xmlDocumentation = "/// <summary>\n";
            xmlDocumentation += "/// The class factory of " + SqlProvider.obj.TableName +".\n";
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
            ClassFactoryBuilder dp = new ClassFactoryBuilder();
            dp.CreateClassFile();
        }
        //
        public void CreateClassFile()
        {
            try
            {
                string _class = GenerateClass(GenerateClassBody());
                DirectoryInfo dInfo = Directory.CreateDirectory(Globals.ClassesDirectory + global.TableProgramatlyName);
                string path = dInfo.FullName + "\\" + ClassName + ".cs";
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path))
                {

                    sw.WriteLine(_class);
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("My Generated Code Exception:" + ex.Message);

            }
        }

        //
        #region ClassMember
        public  string CreateInsertUpdateDeleteMethod(StoredProcedureTypes type)
		{
			try
			{
				string id = Globals.GetProgramatlyName(ID.Name);
				id = Globals.ConvetStringToCamelCase(id);
				string MethodName=type.ToString();
				string sqlDataProviderMethodName = type.ToString() ;
				string MethodReturn="bool";
				string MethodParameters = "(" + global.TableEntityClass + " " + global.EntityClassObject + ")";
				//
				if (type == StoredProcedureTypes.Delete)
					MethodParameters = "(" + Globals.GetAliasDataType(ID.Datatype) + " " + id + ")";
				else
					MethodParameters = "(" + global.TableEntityClass + " " + global.EntityClassObject + ")";
                //XML Documentaion
                string xmlInsertDocumentation = "\t/// <summary>\n";
                xmlInsertDocumentation += "\t/// Creates " + SqlProvider.obj.TableName + " object by calling " + SqlProvider.obj.TableName + " data provider create method.\n";
                xmlInsertDocumentation += "\t/// <example>[Example]bool result=" + ClassName + "."+MethodName+"(" + global.EntityClassObject + ");.</example>\n";
                xmlInsertDocumentation += "\t/// </summary>\n";
                xmlInsertDocumentation += "\t/// <param name=\"" + global.EntityClassObject + "\">The "+ SqlProvider.obj.TableName + " object.</param>\n";
                xmlInsertDocumentation += "\t/// <returns>The result of create operation.</returns>";
                //
                string xmlUpdateDocumentation = "\t/// <summary>\n";
                xmlUpdateDocumentation += "\t/// Updates " + SqlProvider.obj.TableName + " object by calling " + SqlProvider.obj.TableName + " data provider update method.\n";
                xmlUpdateDocumentation += "\t/// <example>[Example]bool result=" + ClassName + "."+MethodName+"(" + global.EntityClassObject + ");.</example>\n";
                xmlUpdateDocumentation += "\t/// </summary>\n";
                xmlUpdateDocumentation += "\t/// <param name=\"" + global.EntityClassObject + "\">The " + SqlProvider.obj.TableName + " object.</param>\n";
                xmlUpdateDocumentation += "\t/// <returns>The result of update operation.</returns>";
				//
				string xmlDeleteDocumentation = "\t/// <summary>\n";
				xmlDeleteDocumentation += "\t/// Deletes single " + SqlProvider.obj.TableName + " object .\n";
				xmlDeleteDocumentation += "\t/// <example>[Example]bool result=" + ClassName + "." + MethodName + "(" + id + ");.</example>\n";
				xmlDeleteDocumentation += "\t/// </summary>\n";
				xmlDeleteDocumentation += "\t/// <param name=\"" + id + "\">The " + global.EntityClassObject + " id.</param>\n";
				xmlDeleteDocumentation += "\t/// <returns>The result of delete operation.</returns>";
                //method body
				StringBuilder methodBody=new StringBuilder();
                if (type == StoredProcedureTypes.Create)
                    methodBody.Append(xmlInsertDocumentation);
                else if (type == StoredProcedureTypes.Update)
                    methodBody.Append(xmlUpdateDocumentation);
				else if (type == StoredProcedureTypes.Delete)
					methodBody.Append(xmlDeleteDocumentation);
				methodBody.Append("\n\tpublic static "+MethodReturn+" "+MethodName+MethodParameters);
				methodBody.Append("\n\t{");
				//
				if (type == StoredProcedureTypes.Delete)
					methodBody.Append("\n\t\treturn " + global.SqlDataProviderClass + ".Instance." + sqlDataProviderMethodName + "(" + id + ");");
				else
					methodBody.Append("\n\t\treturn " + global.SqlDataProviderClass + ".Instance." + sqlDataProviderMethodName + "(" + global.EntityClassObject + ");");

				methodBody.Append("\n\t}");
                methodBody.Append("\n\t" + Globals.MetthodsSeparator);
				return methodBody.ToString();
			}
			catch(Exception ex)
			{
				MessageBox.Show("My Generated Code Exception:"+ex.Message);
				return "";
			}
			

				
		}
		//-------------------------------------
		public  string CreateInsertUpdateDeleteMethod(StoredProcedureTypes type, string MethodName, Hashtable allParameters)
		{
			try
			{
				string id = Globals.GetProgramatlyName(ID.Name);
				id = Globals.ConvetStringToCamelCase(id);
				string sqlDataProviderMethodName = MethodName;
				string MethodReturn = "bool";
				string MethodParameters = "(" + global.TableEntityClass + " " + global.EntityClassObject + ")";
				//
				if (type == StoredProcedureTypes.Delete)
					MethodParameters = "(" + Globals.GetAliasDataType(ID.Datatype) + " " + id + ")";
				else
					MethodParameters = "(" + global.TableEntityClass + " " + global.EntityClassObject + ")";
				//XML Documentaion
				string xmlInsertDocumentation = "\t/// <summary>\n";
				xmlInsertDocumentation += "\t/// Creates " + SqlProvider.obj.TableName + " object by calling " + SqlProvider.obj.TableName + " data provider create method.\n";
				xmlInsertDocumentation += "\t/// <example>[Example]bool result=" + ClassName + "." + MethodName + "(" + global.EntityClassObject + ");.</example>\n";
				xmlInsertDocumentation += "\t/// </summary>\n";
				xmlInsertDocumentation += "\t/// <param name=\"" + global.EntityClassObject + "\">The " + SqlProvider.obj.TableName + " object.</param>\n";
				xmlInsertDocumentation += "\t/// <returns>The result of create operation.</returns>";
				//
				string xmlUpdateDocumentation = "\t/// <summary>\n";
				xmlUpdateDocumentation += "\t/// Updates " + SqlProvider.obj.TableName + " object by calling " + SqlProvider.obj.TableName + " data provider update method.\n";
				xmlUpdateDocumentation += "\t/// <example>[Example]bool result=" + ClassName + "." + MethodName + "(" + global.EntityClassObject + ");.</example>\n";
				xmlUpdateDocumentation += "\t/// </summary>\n";
				xmlUpdateDocumentation += "\t/// <param name=\"" + global.EntityClassObject + "\">The " + SqlProvider.obj.TableName + " object.</param>\n";
				xmlUpdateDocumentation += "\t/// <returns>The result of update operation.</returns>";
				//
				string xmlDeleteDocumentation = "\t/// <summary>\n";
				xmlDeleteDocumentation += "\t/// Deletes single " + SqlProvider.obj.TableName + " object .\n";
				xmlDeleteDocumentation += "\t/// <example>[Example]bool result=" + ClassName + "." + MethodName + "(" + id + ");.</example>\n";
				xmlDeleteDocumentation += "\t/// </summary>\n";
				xmlDeleteDocumentation += "\t/// <param name=\"" + id + "\">The " + global.EntityClassObject + " id.</param>\n";
				xmlDeleteDocumentation += "\t/// <returns>The result of delete operation.</returns>";
				//method body
				StringBuilder methodBody = new StringBuilder();
				if (type == StoredProcedureTypes.Create)
					methodBody.Append(xmlInsertDocumentation);
				else if (type == StoredProcedureTypes.Update)
					methodBody.Append(xmlUpdateDocumentation);
				else if (type == StoredProcedureTypes.Delete)
					methodBody.Append(xmlDeleteDocumentation);
				methodBody.Append("\n\tpublic static " + MethodReturn + " " + MethodName + MethodParameters);
				methodBody.Append("\n\t{");
				//
				if (type == StoredProcedureTypes.Delete)
					methodBody.Append("\n\t\treturn " + global.SqlDataProviderClass + ".Instance." + sqlDataProviderMethodName + "(" + id + ");");
				else
					methodBody.Append("\n\t\treturn " + global.SqlDataProviderClass + ".Instance." + sqlDataProviderMethodName + "(" + global.EntityClassObject + ");");

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
		//-------------------------------------
        //
		public  string CreateGetAllMethod(StoredProcedureTypes type)
		{
			try
			{
				string MethodName = type.ToString();
				string sqlDataProviderMethodName = type.ToString() + global.TableProgramatlyName;
				string MethodReturn="DataTable";
                //XML Documentaion
                string xmlDocumentation = "\t/// <summary>\n";
                xmlDocumentation += "\t/// Gets All " + SqlProvider.obj.TableName + ".\n";
                xmlDocumentation += "\t/// <example>[Example]DataTable dt" + global.TableProgramatlyName + "=" + ClassName + "." + MethodName + "();.</example>\n";
                xmlDocumentation += "\t/// </summary>\n";
                xmlDocumentation += "\t/// <returns>All " + SqlProvider.obj.TableName + ".</returns>";
                //Method Body
				StringBuilder methodBody=new StringBuilder();
                methodBody.Append(xmlDocumentation);
				methodBody.Append("\n\tpublic static "+MethodReturn+" "+MethodName+"()");
				methodBody.Append("\n\t{");
				methodBody.Append("\n\t\treturn " + global.SqlDataProviderClass + ".Instance." + sqlDataProviderMethodName + "();");
				methodBody.Append("\n\t}");
                methodBody.Append("\n\t" + Globals.MetthodsSeparator);
				return methodBody.ToString();
			}
			catch(Exception ex)
			{

				MessageBox.Show("My Generated Code Exception:"+ex.Message);
				return "";

			}
				
		}
		public string CreateGetAllMethod4Show(StoredProcedureTypes type)
		{
			try
			{
				string MethodName = type.ToString();
				string sqlDataProviderMethodName = type.ToString() + global.TableProgramatlyName;
				string MethodReturn = "DataTable";
				//XML Documentaion
				string xmlDocumentation = "\t/// <summary>\n";
				xmlDocumentation += "\t/// Gets All " + SqlProvider.obj.TableName + ".\n";
				xmlDocumentation += "\t/// <example>[Example]DataTable dt" + global.TableProgramatlyName + "=" + ClassName + "." + MethodName + "();.</example>\n";
				xmlDocumentation += "\t/// </summary>\n";
				xmlDocumentation += "\t/// <returns>All " + SqlProvider.obj.TableName + ".</returns>";
				//Method Body
				StringBuilder methodBody = new StringBuilder();
				methodBody.Append(xmlDocumentation);
				methodBody.Append("\n\tpublic static " + MethodReturn + " " + MethodName + "()");
				methodBody.Append("\n\t{");
				methodBody.Append("\n\t\treturn " + global.SqlDataProviderClass + ".Instance." + sqlDataProviderMethodName + "();");
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
        //
		public  string CreateGetOneMethod(StoredProcedureTypes type)
		{
			if(ID!=null)
			{
				try
				{
					string id = Globals.GetProgramatlyName(ID.Name);
					id = Globals.ConvetStringToCamelCase(id);
					//
					string MethodName="Get"+global.TableProgramatlyName+"Object";
					string MethodReturn=global.TableEntityClass;
                    //XML Documentaion
                    string xmlDocumentation = "\t/// <summary>\n";
                    xmlDocumentation += "\t/// Gets single " + SqlProvider.obj.TableName + " object .\n";
					xmlDocumentation += "\t/// <example>[Example]" + global.TableEntityClass  +" " + global.EntityClassObject + "=" + ClassName + "." + MethodName + "(" + id + ");.</example>\n";
                    xmlDocumentation += "\t/// </summary>\n";
					xmlDocumentation += "\t/// <param name=\"" + id + "\">The " + global.EntityClassObject + " id.</param>\n";
                    xmlDocumentation += "\t/// <returns>" + SqlProvider.obj.TableName + " object.</returns>";
                    //Method Body
					StringBuilder methodBody=new StringBuilder();
                    methodBody.Append(xmlDocumentation);
					methodBody.Append("\n\tpublic static  " + MethodReturn + " " + MethodName + "(" + Globals.GetAliasDataType(ID.Datatype) + " " + id + ")");
					methodBody.Append("\n\t{");
					methodBody.Append("\n\t\treturn " + global.SqlDataProviderClass + ".Instance." + MethodName + "(" + id + ");");
					methodBody.Append("\n\t}");
                    methodBody.Append("\n\t" + Globals.MetthodsSeparator);
					return methodBody.ToString();
				}
				catch(Exception ex)
				{
					MessageBox.Show("My Generated Code Exception:"+ex.Message);
					return "";
				}	
			}
			else
				return "";
        }
		public string CreateGetOne4ShowMethod(StoredProcedureTypes type)
		{
			if (ID != null)
			{
				try
				{
					string id = Globals.GetProgramatlyName(ID.Name);
					id = Globals.ConvetStringToCamelCase(id);
					//
					string MethodName = "Get" + global.TableProgramatlyName + "Object4Show";
					string MethodReturn = global.TableEntityClass;
					//XML Documentaion
					string xmlDocumentation = "\t/// <summary>\n";
					xmlDocumentation += "\t/// Gets single " + SqlProvider.obj.TableName + " object4Show .\n";
					xmlDocumentation += "\t/// <example>[Example]" + global.TableEntityClass + " " + global.EntityClassObject + "=" + ClassName + "." + MethodName + "(" + id + ");.</example>\n";
					xmlDocumentation += "\t/// </summary>\n";
					xmlDocumentation += "\t/// <param name=\"" + id + "\">The " + global.EntityClassObject + " id.</param>\n";
					xmlDocumentation += "\t/// <returns>" + SqlProvider.obj.TableName + " object.</returns>";
					//Method Body
					StringBuilder methodBody = new StringBuilder();
					methodBody.Append(xmlDocumentation);
					methodBody.Append("\n\tpublic static  " + MethodReturn + " " + MethodName + "(" + Globals.GetAliasDataType(ID.Datatype) + " " + id + ")");
					methodBody.Append("\n\t{");
					methodBody.Append("\n\t\treturn " + global.SqlDataProviderClass + ".Instance." + MethodName + "(" + id + ");");
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
 
		//
		private string CreatePopulateObjectMethod()
		{
			try
			{
				// 
				string MethodName = "Populate" + global.TableEntityClass + "FromDataRowView";
				string MethodReturn = global.TableEntityClass;
				//XML Documentaion
				string xmlDocumentation = "\t/// <summary>\n";
				xmlDocumentation += "\t/// Populates " + SqlProvider.obj.TableName + " Entity From DataRowView .\n";
				xmlDocumentation += "\t/// <example>[Example]" + global.TableEntityClass + global.EntityClassObject + "=" + MethodName + "(obj);.</example>\n";
				xmlDocumentation += "\t/// </summary>\n";
				xmlDocumentation += "\t/// <param name=\"obj\"></param>\n";
				xmlDocumentation += "\t/// <returns>" + SqlProvider.obj.TableName + " object.</returns>";
				//Method Body
				StringBuilder methodBody = new StringBuilder();
				methodBody.Append(xmlDocumentation);
				methodBody.Append("\n\tprivate " + global.TableEntityClass + " " + MethodName + "(DataRowView obj)");
				methodBody.Append("\n\t{");
				methodBody.Append("\n\t\t//Create a new " + SqlProvider.obj.TableName + " object");
				methodBody.Append("\n\t\t" + global.TableEntityClass + " " + global.EntityClassObject + " = new " + global.TableEntityClass + "();");
				methodBody.Append("\n\t\t//Fill the object with data");
				//
				foreach (SQLDMO.Column colCurrent in Fields)
				{
					methodBody.Append("\n\t\tif (obj[\"" + Globals.GetProgramatlyName(colCurrent.Name) + "\"] != DBNull.Value)");
					methodBody.Append("\n\t\t\t" + global.EntityClassObject + "." + Globals.GetProgramatlyName(colCurrent.Name) + " = (" + Globals.GetAliasDataType(colCurrent.Datatype) + ") obj[\"" + Globals.GetProgramatlyName(colCurrent.Name) + "\"];");
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

