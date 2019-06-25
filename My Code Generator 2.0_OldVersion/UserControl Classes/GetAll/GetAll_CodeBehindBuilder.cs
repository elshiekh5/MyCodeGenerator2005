using System;
using System.IO;
using System.Text ;
using System.Windows.Forms;
using System.Collections;
namespace SPGen
{
	/// <summary>
	/// Summary description for Ceate_CodeBehindBuilder.
	/// </summary>
	public class GetAll_CodeBehindBuilder :CodeBehindBuilder 
	{
		Hashtable allParameters = null;
		//
		public GetAll_CodeBehindBuilder(InterfaceType type)
		{
			Type = type;
			ClassName=global.GetAllCodeBehindClass;
		}
        //
		private string GeneratePageLoad()
		{
            string pageLoadBody = "\n\t\tlblResult.Text=\"\";";
            pageLoadBody = "\n\t\tif(!IsPostBack)";
            pageLoadBody += "\n\t\t\tLoadData();";
            return GeneratePageLoadHandler(pageLoadBody); 
		}
		//
        private string CreateLoadData()
		{
			StringBuilder loadData=new StringBuilder();
			loadData.Append("\n\tprivate void LoadData()");
			loadData.Append("\n\t{");
			loadData.Append("\n\t\tDataTable dtSource= "+global.TableFactoryClass+"."+StoredProcedureTypes.GetAll.ToString()+"();");
			loadData.Append("\n\t\tif(dtSource!=null&&dtSource.Rows.Count >0)");
			loadData.Append("\n\t\t{");
			loadData.Append("\n\t\t\t"+global.ViewAllDataGrid+".DataSource= dtSource;");
			if(ID!=null)
				loadData.Append("\n\t\t\t"+global.ViewAllDataGrid+".DataKeyField=\""+Globals.GetProgramatlyName(ID.Name)+"\";");
			loadData.Append("\n\t\t\tif("+global.ViewAllDataGrid+".PageSize>=dtSource.Rows.Count)");
			loadData.Append("\n\t\t\t{");
			loadData.Append("\n\t\t\t\t"+global.ViewAllDataGrid+".AllowPaging=false;");
			loadData.Append("\n\t\t\t}");
			loadData.Append("\n\t\t\t"+global.ViewAllDataGrid+".DataBind();");
			loadData.Append("\n\t\t\t" + global.ViewAllDataGrid + ".Visible = true;");
			loadData.Append("\n\t\t\tlblMsg.Visible = false;");
			loadData.Append("\n\t\t}");
			loadData.Append("\n\t\telse");
			loadData.Append("\n\t\t{");
			loadData.Append("\n\t\t\t" + global.ViewAllDataGrid + ".Visible=false;");
			loadData.Append("\n\t\t\tlblMsg.Visible = true;");
			loadData.Append("\n\t\t}");
			loadData.Append("\n\t}");
			return loadData.ToString();
		}
		//
        private string CreatePageIndexHandler()
		{
            StringBuilder pageIndexHandler=new StringBuilder();
			pageIndexHandler.Append("\n\tprotected void " + global.ViewAllDataGrid + "_PageIndexChanged(object source,DataGridPageChangedEventArgs e)");
			pageIndexHandler.Append("\n\t{");
			pageIndexHandler.Append("\n\t\t"+global.ViewAllDataGrid+".CurrentPageIndex=e.NewPageIndex;");
			pageIndexHandler.Append("\n\t\tLoadData();");
			pageIndexHandler.Append("\n\t}");
			return pageIndexHandler.ToString();
		}
		//
		private string CreateItemDataBoundHandler()
		{
			StringBuilder pageIndexHandler = new StringBuilder();
			pageIndexHandler.Append("\n\tprotected void " + global.ViewAllDataGrid + "_ItemDataBound(object source, DataGridItemEventArgs e)");
			pageIndexHandler.Append("\n\t{");
			pageIndexHandler.Append("\n\t\tif (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)");
			pageIndexHandler.Append("\n\t\t{");
			pageIndexHandler.Append("\n\t\t\tImageButton lbtnDelete = (ImageButton)e.Item.FindControl(\"lbtnDelete\");");
			pageIndexHandler.Append("\n\t\t\tlbtnDelete.Attributes.Add(\"onclick\", \"return confirm('Â· ›⁄·«  —Ìœ «·Õ–› ø')\");");
			pageIndexHandler.Append("\n\t\t}");
			pageIndexHandler.Append("\n\t}");
			return pageIndexHandler.ToString();
		}
		//
		private string CreateDeleteCommandHandler()
		{
			string dataGridID = "dg" + global.TableProgramatlyName;
			string id = Globals.GetProgramatlyName(ID.Name);
			id = Globals.ConvetStringToCamelCase(id);
			StringBuilder pageIndexHandler = new StringBuilder();
			pageIndexHandler.Append("\n\tprotected void " + dataGridID + "_DeleteCommand(object source, DataGridCommandEventArgs e)");
			pageIndexHandler.Append("\n\t{");
			pageIndexHandler.Append("\n\t\t" + Globals.GetAliasDataType(ID.Datatype) + " " + id + " = Convert.To" + Globals.GetDataType(ID.Datatype) + "("+dataGridID+".DataKeys[e.Item.ItemIndex]);");
			pageIndexHandler.Append("\n\t\tif(" + global.TableFactoryClass + "." + MethodType.Delete.ToString() + "(" + id + "))");
			pageIndexHandler.Append("\n\t\t{");
			pageIndexHandler.Append("\n\t\t\tLoadData();");
			pageIndexHandler.Append("\n\t\t}");
			pageIndexHandler.Append("\n\t}");
			return pageIndexHandler.ToString();
			//
		}
		//
		public static void Create(InterfaceType type)
		{
			GetAll_CodeBehindBuilder cr = new GetAll_CodeBehindBuilder(type);
			cr.CreateTheFile();
		}
		//
		public static void Create(InterfaceType type, Hashtable allParameters, string operation)
		{
			GetAll_CodeBehindBuilder cr = new GetAll_CodeBehindBuilder(type);
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
			classBody.Append("\n" + CreateItemDataBoundHandler());
            classBody.Append("\n" + CreatePageIndexHandler());
			classBody.Append("\n" + CreateDeleteCommandHandler());
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
					path = directoryPath + "\\default.aspx.cs";
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
