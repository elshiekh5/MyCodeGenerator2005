using System;
using System.IO;
using System.Text ;
using System.Windows.Forms;
using System.Data;
using System.Collections;
namespace SPGen
{
	/// <summary>
	/// Summary description for Ceate_CodeBehindBuilder.
	/// </summary>
	public class Update_CodeBehindBuilder :CodeBehindBuilder 
	{
		//
		Hashtable allParameters = null;
		//
		public Update_CodeBehindBuilder(InterfaceType type)
		{
			Type = type;
			ClassName=global.UpdateCodeBehindClass;
		}

        private string GeneratePageLoad()
		{
            string pageLoadBody = "\n\t\tlblResult.Text=\"\";";
            pageLoadBody = "\n\t\tif(!IsPostBack)";
            pageLoadBody += "\n\t\t\tLoadData();";
            return GeneratePageLoadHandler(pageLoadBody); 

		}

        private string CreateLoadData()
		{
			StringBuilder loadData=new StringBuilder();
			string id=Globals.GetProgramatlyName(ID.Name);
			id = Globals.ConvetStringToCamelCase(id);
			loadData.Append("\n\tprivate void LoadData()");
			loadData.Append("\n\t{");
			loadData.Append("\n\t\tif(Request.QueryString[\""+id+"\"]!=null)");
			loadData.Append("\n\t\t{");
			loadData.Append("\n\t\t\t"+Globals.GetAliasDataType(ID.Datatype)+" "+id+" = Convert.To"+Globals.GetDataType(ID.Datatype)+"(Request.QueryString[\""+id+"\"]);");
			
			loadData.Append("\n\t\t\t"+global.TableEntityClass+" "+global.EntityClassObject+" ="+global.TableFactoryClass+".Get"+global.TableProgramatlyName+"Object("+id+");");
			string datatype;
			foreach(Column column in Fields)
			{
				if (allParameters != null && !allParameters.Contains(column.Name))
					continue;
				if(ID==null||column.Name !=ID.Name)//||!Globals.CheckIsAddedBySql(ID))
				{
					TableConstraint cnstr=SqlProvider.obj.GetParentColumn(column.Name);
					datatype=Globals.GetAliasDataType(column.Datatype);
					string ddl="";
					if(datatype=="string")
					{
						if (Globals.GetSqlDataType(column.Datatype) == SqlDbType.NText)
							loadData.Append("\n\t\t\tftb" + Globals.GetProgramatlyName(column.Name) + ".Text = " + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + ";");
						else
						{
							if (cnstr == null)
								loadData.Append("\n\t\t\ttxt" + Globals.GetProgramatlyName(column.Name) + ".Text = " + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + ";");
							else
							{
								ddl = "ddl" + Globals.GetProgramatlyName(cnstr.ParentTable);

								loadData.Append("\n\t\t\tMyDropDownList.LoadDataAndSetSelectedValue(" + ddl + "," + Globals.GetProgramatlyName(cnstr.ParentTable) + "Factory.GetAll(),\"" +
									SqlProvider.obj.GetExpectedNameForParent(cnstr.ParentTable) + "\",\"" + Globals.GetProgramatlyName(cnstr.ParentColID) + "\"," + global.EntityClassObject + "." +
									Globals.GetProgramatlyName(column.Name) + ".ToString());");
							}
						}
					}
					else if(datatype=="bool")
						loadData.Append("\n\t\t\tcb"+Globals.GetProgramatlyName(column.Name)+".Checked = "+global.EntityClassObject+"."+Globals.GetProgramatlyName(column.Name)+";");		
					else if(datatype!="byte[]"&&datatype!="Object"&&datatype!="Guid")
					{
						if(cnstr==null)
							loadData.Append("\n\t\t\ttxt"+Globals.GetProgramatlyName(column.Name)+".Text = "+global.EntityClassObject+"."+Globals.GetProgramatlyName(column.Name)+".ToString();");
						else
						{
							ddl="ddl"+Globals.GetProgramatlyName(cnstr.ParentTable);
							
								loadData.Append("\n\t\t\tMyDropDownList.LoadDataAndSetSelectedValue("+ddl+","+Globals.GetProgramatlyName(cnstr.ParentTable)+"Factory.GetAll(),\""+
								SqlProvider.obj.GetExpectedNameForParent(cnstr.ParentTable)+"\",\""+Globals.GetProgramatlyName(cnstr.ParentColID)+"\","+global.EntityClassObject+"."+
								Globals.GetProgramatlyName(column.Name)+".ToString());");
						}
					}

				}
			}

			loadData.Append("\n\t\t}");
			loadData.Append("\n\t\telse");
			loadData.Append("\n\t\tthis.Visible=false;");
			loadData.Append("\n\t}");
			return loadData.ToString();
		}
		//
        private string Create_CreateButtonHandler()
		{
            StringBuilder _UpdateButtonHandler = new StringBuilder();

			_UpdateButtonHandler.Append("\n\tprotected void btnUpdate_Click(object sender, System.EventArgs e)");
			_UpdateButtonHandler.Append("\n\t{");
            _UpdateButtonHandler.Append("\n\t\tif (!Page.IsValid)");
            _UpdateButtonHandler.Append("\n\t\t{");
            _UpdateButtonHandler.Append("\n\t\t\treturn;");
            _UpdateButtonHandler.Append("\n\t\t}");
			_UpdateButtonHandler.Append("\n\t\t"+global.TableEntityClass+" "+global.EntityClassObject+" = new "+global.TableEntityClass+"();");
			string datatype;
			_UpdateButtonHandler.Append("\n\t\t"+global.EntityClassObject+"."+Globals.GetProgramatlyName(ID.Name)+" = Convert.To"+Globals.GetDataType(ID.Datatype)+"(Request.QueryString[\""+Globals.GetProgramatlyName(ID.Name)+"\"]);");
			foreach(Column column in Fields)
			{
				if(ID==null||column.Name !=ID.Name)//||!Globals.CheckIsAddedBySql(ID))
				{
					TableConstraint cnstr=SqlProvider.obj.GetParentColumn(column.Name);
					datatype=Globals.GetAliasDataType(column.Datatype);

					if(datatype=="string")
					{
						if (Globals.GetSqlDataType(column.Datatype) == SqlDbType.NText)
							_UpdateButtonHandler.Append("\n\t\t" + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + " = ftb" + Globals.GetProgramatlyName(column.Name) + ".Text;");
						else
						{
							if (cnstr == null)
								_UpdateButtonHandler.Append("\n\t\t" + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + " = txt" + Globals.GetProgramatlyName(column.Name) + ".Text;");
							else
								_UpdateButtonHandler.Append("\n\t\t" + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + " = ddl" + Globals.GetProgramatlyName(cnstr.ParentTable) + ".SelectedValue;");
						}
					}
					else if(datatype=="bool")
						_UpdateButtonHandler.Append("\n\t\t"+global.EntityClassObject+"."+Globals.GetProgramatlyName(column.Name)+" = cb"+Globals.GetProgramatlyName(column.Name)+".Checked;");		
					else if(datatype=="Guid")
						_UpdateButtonHandler.Append("\n\t\t"+global.EntityClassObject+"."+Globals.GetProgramatlyName(column.Name)+" = Guid.NewGuid();");		
					else if(datatype!="byte[]"&&datatype!="Object")
					{
						if(cnstr==null)
							_UpdateButtonHandler.Append("\n\t\t"+global.EntityClassObject+"."+Globals.GetProgramatlyName(column.Name)+" = Convert.To"+Globals.GetDataType(column.Datatype)+"(txt"+Globals.GetProgramatlyName(column.Name)+".Text);");
						else
							_UpdateButtonHandler.Append("\n\t\t"+global.EntityClassObject+"."+Globals.GetProgramatlyName(column.Name)+" = Convert.To"+Globals.GetDataType(column.Datatype)+"(ddl"+Globals.GetProgramatlyName(cnstr.ParentTable)+".SelectedValue);");

					}
				}
			}
            _UpdateButtonHandler.Append("\n\t\tbool result = "+global.TableFactoryClass+"."+MethodType.Update.ToString()+"("+global.EntityClassObject+");");
            _UpdateButtonHandler.Append("\n\t\tif(result)");
			_UpdateButtonHandler.Append("\n\t\t{");
			//_UpdateButtonHandler.Append("\n\t\t\tlblResult.ForeColor=Color.Blue ;");
			//_UpdateButtonHandler.Append("\n\t\t\tlblResult.Text=\" „ «· ⁄œÌ· »‰Ã«Õ\";");
			_UpdateButtonHandler.Append("\n\t\t\tResponse.Redirect(\"default.aspx\");");
			
			_UpdateButtonHandler.Append("\n\t\t}");
			_UpdateButtonHandler.Append("\n\t\telse");
			_UpdateButtonHandler.Append("\n\t\t{");
			_UpdateButtonHandler.Append("\n\t\t\tlblResult.ForeColor=Color.Red ;");
			_UpdateButtonHandler.Append("\n\t\t\tlblResult.Text=\"›‘·  ⁄„·Ì… «· ⁄œÌ·\";");
			_UpdateButtonHandler.Append("\n\t\t}");
			_UpdateButtonHandler.Append("\n\t}");
			return _UpdateButtonHandler.ToString();
		}
		//
		public static void Create(InterfaceType type)
		{
			Update_CodeBehindBuilder cr = new Update_CodeBehindBuilder(type);
			cr.CreateTheFile();
		}
		//
		public static void Create(InterfaceType type, Hashtable allParameters, string operation)
		{
			Update_CodeBehindBuilder cr = new Update_CodeBehindBuilder(type);
			Globals global = new Globals();
			cr.ClassName = global.TableProgramatlyName + "_" + operation;
			cr.allParameters = allParameters;
			cr.CreateTheFile();
		}
        //
        private string GenerateClassBody()
        {
            StringBuilder classBody = new StringBuilder();
            classBody.Append(GeneratePageLoad());
            classBody.Append("\n" + CreateLoadData());
            classBody.Append("\n" + Create_CreateButtonHandler());
            return classBody.ToString();
        }
        //
        private void CreateTheFile()
		{
			DirectoryInfo dInfo;
			string path;
			try
			{
				if (Type == InterfaceType.WEbUserControl)
				{
					dInfo = Directory.CreateDirectory(Globals.UserControlsDirectory + global.TableProgramatlyName);
					path = dInfo.FullName + "\\" + ClassName + ".ascx.cs";
				}
				else
				{
					//
					string directoryPath = Globals.AdminFolder + global.TableProgramatlyName + "\\";
					path = directoryPath + "\\Edit.aspx.cs";
					DirectoriesManager.ChechDirectory(directoryPath);
				}
				// Create the file.
				string _class = GenerateClass(GenerateClassBody());
				FileManager.CreateFile(path, _class);				
			}
			catch(Exception ex)
			{
				MessageBox.Show("My Generated Code Exception:"+ex.Message);
				
			}
		}
		
	}
}
