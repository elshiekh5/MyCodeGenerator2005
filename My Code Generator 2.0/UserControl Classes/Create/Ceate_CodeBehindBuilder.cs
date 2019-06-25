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
	public class Create_CodeBehindBuilder :CodeBehindBuilder 
	{
        //For Build The ddl Load Data Methods
        private StringBuilder loadMethodsBuilder = new StringBuilder();
		private StringBuilder loadDataMethodBody = new StringBuilder();
        //
		Hashtable allParameters=null;
		//
        private string GenerateLoadDataMethod()
        {
            string loadDataMethod = "";
            if (loadDataMethodBody.Length> 0)
            {
                loadDataMethod = "\n\t\tprivate void LoadData()";
                loadDataMethod += "\n\t\t{" + loadDataMethodBody + "\n\t\t}";

            }
            return loadDataMethod;
        }
        //
        private string GeneratePageLoad()
		{
            GenerateSupLoadMethods();
            //
            string pageLoadBody = "\n\t\tlblResult.Text=\"\";";
            if (loadDataMethodBody.Length > 0)
			{
                pageLoadBody="\n\t\tif(!IsPostBack)";
                pageLoadBody+="\n\t\t\tLoadData();";
                
			}
            return GeneratePageLoadHandler(pageLoadBody); 
		}
        //
        public void GenerateSupLoadMethods()
        {
            string datatype;
            foreach (SQLDMO.Column column in Fields)
            {
                if (ID == null || column.Name != ID.Name || !Globals.CheckIsAddedBySql(ID))
                {
                    datatype = Globals.GetAliasDataType(column.Datatype);
                    if (datatype != "bool" && datatype != "byte[]" && datatype != "Object" && datatype != "Guid")
                    {
                        TableConstraint cnstr = SqlProvider.obj.GetParentColumn(column.Name);
                        if (cnstr != null)
                        {
                            CreateSupLoadMethod(cnstr);
                        }

                    }
                }
            }
        }
        //
		private void CreateSupLoadMethod(TableConstraint cnstr)
		{
			string ddl="ddl"+Globals.GetProgramatlyName(cnstr.ParentTable);
            loadMethodsBuilder.Append("\n\t\tprivate void Load_" + ddl + "()");
            loadMethodsBuilder.Append("\n\t\t{");
            loadMethodsBuilder.Append("\n\t\t\tDataTable dtSource= " + Globals.GetProgramatlyName(cnstr.ParentTable) + "Factory.GetAll();");
            loadMethodsBuilder.Append("\n\t\t\t" + ddl + ".DataSource=dtSource;");
            loadMethodsBuilder.Append("\n\t\t\t" + ddl + ".DataTextField=\"" + SqlProvider.obj.GetExpectedNameForParent(cnstr.ParentTable) + "\";");
            loadMethodsBuilder.Append("\n\t\t\t" + ddl + ".DataValueField=\"" + Globals.GetProgramatlyName(cnstr.ParentColID) + "\";");
			loadMethodsBuilder.Append("\n\t\t\t" + ddl + ".DataBind();");
			loadMethodsBuilder.Append("\n\t\t\t" + ddl + ".UpdateAfterCallBack = true;");
			loadMethodsBuilder.Append("\n\t\t\tif (" + ddl + ".Items.Count > 0)");
			loadMethodsBuilder.Append("\n\t\t\t{");
			loadMethodsBuilder.Append("\n\t\t\t" + ddl + ".Items.Insert(0, new ListItem(\"----«Œ ‹‹‹—----\",\" \"));");
			loadMethodsBuilder.Append("\n\t\t\t}");
			
            loadMethodsBuilder.Append("\n\t\t}");
			//
			loadDataMethodBody.Append("\n\t\t\tLoad_"+ddl+"();");

		}
        //
        public Create_CodeBehindBuilder(InterfaceType type)
		{
			Type = type;
			ClassName=global.CreateCodeBehindClass;
		}
        //
        private string Create_CreateButtonHandler()
		{
            StringBuilder _CreateButtonHandler=new StringBuilder();
            _CreateButtonHandler.Append("\n\tprotected void btnCreate_Click(object sender, System.EventArgs e)");
			_CreateButtonHandler.Append("\n\t{");
            _CreateButtonHandler.Append("\n\t\tif (!Page.IsValid)");
            _CreateButtonHandler.Append("\n\t\t{");
            _CreateButtonHandler.Append("\n\t\t\treturn;");
            _CreateButtonHandler.Append("\n\t\t}");
			_CreateButtonHandler.Append("\n\t\t"+global.TableEntityClass+" "+global.EntityClassObject+" = new "+global.TableEntityClass+"();");
			string datatype;
			foreach(SQLDMO.Column column in Fields)
			{
				if (allParameters !=null&& !allParameters.Contains(column.Name))
					continue;
				if(ID==null||column.Name !=ID.Name||!Globals.CheckIsAddedBySql(ID))
				{
					datatype=Globals.GetAliasDataType(column.Datatype);
					TableConstraint cnstr=SqlProvider.obj.GetParentColumn(column.Name);
					if(datatype=="string")
					{
						if (Globals.GetSqlDataType(column.Datatype) == SqlDbType.NText)
							_CreateButtonHandler.Append("\n\t\t" + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + " = fck" + Globals.GetProgramatlyName(column.Name) + ".Value;");
						else
						{
							if (cnstr == null)
								_CreateButtonHandler.Append("\n\t\t" + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + " = txt" + Globals.GetProgramatlyName(column.Name) + ".Text;");
							else
								_CreateButtonHandler.Append("\n\t\t" + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + " = ddl" + Globals.GetProgramatlyName(cnstr.ParentTable) + ".SelectedValue;");
						}
					}
					//DateTime
					else if (Globals.GetSqlDataType(column.Datatype) == SqlDbType.DateTime)
						_CreateButtonHandler.Append("\n\t\t" + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + " = Convert.To" + Globals.GetDataType(column.Datatype) + "(txt" + Globals.GetProgramatlyName(column.Name) + ".Value);");
					//DateTime
					else if (datatype == "bool")
						_CreateButtonHandler.Append("\n\t\t" + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + " = cb" + Globals.GetProgramatlyName(column.Name) + ".Checked;");		
					else if(datatype=="Guid")
						_CreateButtonHandler.Append("\n\t\t"+global.EntityClassObject+"."+Globals.GetProgramatlyName(column.Name)+" = Guid.NewGuid();");		
					else if(datatype!="byte[]"&&datatype!="Object")
					{
						if(cnstr==null)
                            _CreateButtonHandler.Append("\n\t\t" + global.EntityClassObject + "." + Globals.GetProgramatlyName(column.Name) + " = Convert.To"+Globals.GetDataType(column.Datatype) + "(txt" + Globals.GetProgramatlyName(column.Name) + ".Text);");
						else
							_CreateButtonHandler.Append("\n\t\t"+global.EntityClassObject+"."+Globals.GetProgramatlyName(column.Name)+" = Convert.To"+Globals.GetDataType(column.Datatype)+"(ddl"+Globals.GetProgramatlyName(cnstr.ParentTable)+".SelectedValue);");

					}
				}
			}
            _CreateButtonHandler.Append("\n\t\tbool result = " + global.TableFactoryClass + "." + MethodType.Create.ToString() + "(" + global.EntityClassObject + ");");
            _CreateButtonHandler.Append("\n\t\tif(result)");
			
            _CreateButtonHandler.Append("\n\t\t{");
			_CreateButtonHandler.Append("\n\t\t\tlblResult.ForeColor=Color.Blue ;");
			_CreateButtonHandler.Append("\n\t\t\tlblResult.Text=\" „ «·≈÷«›… »‰Ã«Õ\";");
			_CreateButtonHandler.Append("\n\t\t\t"+global.ClearControlsMethod+";");
			_CreateButtonHandler.Append("\n\t\t}");
			_CreateButtonHandler.Append("\n\t\telse");
			_CreateButtonHandler.Append("\n\t\t{");
			_CreateButtonHandler.Append("\n\t\t\tlblResult.ForeColor=Color.Red ;");
			_CreateButtonHandler.Append("\n\t\t\tlblResult.Text=\"›‘·  ⁄„·Ì…  «·≈÷«›…\";");
			_CreateButtonHandler.Append("\n\t\t}");
			_CreateButtonHandler.Append("\n\t}");
			return _CreateButtonHandler.ToString();
		}
        //
        private string CreateClearControlsMethod()
		{
			//
			StringBuilder method=new StringBuilder();
			method.Append("\n\tprivate void "+global.ClearControlsMethod);
			method.Append("\n\t{");
			string datatype;
			foreach(SQLDMO.Column column in Fields)
			{
				if(ID==null||column.Name !=ID.Name||!Globals.CheckIsAddedBySql(ID))
				{
					if (allParameters != null && !allParameters.Contains(column.Name))
						continue;
					datatype=Globals.GetAliasDataType(column.Datatype);
					
					if (datatype == "bool")
					{
						method.Append("\n\t\tcb" + Globals.GetProgramatlyName(column.Name) + ".Checked=false;");
					}
					else if (Globals.GetSqlDataType(column.Datatype) == SqlDbType.NText)
					{
						method.Append("\n\t\tfck" + Globals.GetProgramatlyName(column.Name) + ".Value=\"\";");
					}
					else if (datatype != "byte[]" && datatype != "Object" && datatype != "Guid")
					{
						TableConstraint cnstr = SqlProvider.obj.GetParentColumn(column.Name);
						if (cnstr == null)
						{
							//dateTime
							if (Globals.GetSqlDataType(column.Datatype) == SqlDbType.DateTime)
							{
								method.Append("\n\t\ttxt" + Globals.GetProgramatlyName(column.Name) + ".Value=\"\";");
							}
							//datetime
							else{
							method.Append("\n\t\ttxt" + Globals.GetProgramatlyName(column.Name) + ".Text=\"\";");
								}
						}
						else
						{
							method.Append("\n\t\tddl" + Globals.GetProgramatlyName(cnstr.ParentTable) + ".SelectedIndex=-1;");
							method.Append("\n\t\tddl" + Globals.GetProgramatlyName(cnstr.ParentTable) + ".DataBind();");
						}
					}
				}
			}
			method.Append("\n\t}");
			return method.ToString();
		}
		//
		public static void Create(InterfaceType type)
		{
			Create_CodeBehindBuilder cr = new Create_CodeBehindBuilder(type);
            cr.CreateClassFile();
		}
        //
		//
		public static void Create(InterfaceType type, Hashtable allParameters, string operation)
		{
			Create_CodeBehindBuilder cr = new Create_CodeBehindBuilder(type);
			Globals global = new Globals();
			cr.ClassName = global.TableProgramatlyName +"_"+ operation;
			cr.allParameters = allParameters;
			cr.CreateClassFile();
		}
        private string GenerateClassBody()
        {
            StringBuilder classBody = new StringBuilder();
            classBody.Append(GeneratePageLoad());
            classBody.Append("\n" + GenerateLoadDataMethod());
            classBody.Append("\n" + loadMethodsBuilder);
            classBody.Append("\n" + Create_CreateButtonHandler());
            classBody.Append("\n" + CreateClearControlsMethod());
            return classBody.ToString();
        }
        //
        private void CreateClassFile()
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
					path = directoryPath + "\\Add.aspx.cs";
					DirectoriesManager.ChechDirectory(directoryPath);
				}
				// Create the file.
                string _class = GenerateClass(GenerateClassBody());
                FileManager.CreateFile(path, _class);
				//CREATE THE WEB FORM  
				
			}
			catch(Exception ex)
			{
				MessageBox.Show("My Generated Code Exception:"+ex.Message);
				
			}
		}
		
	}
}
