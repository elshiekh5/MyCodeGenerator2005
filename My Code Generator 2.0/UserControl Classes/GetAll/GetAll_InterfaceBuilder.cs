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
	public class GetAll_InterfaceBuilder : InterfaceBuilder
    {
		Hashtable allParameters = null;
		//
		public GetAll_InterfaceBuilder(InterfaceType type)
		{
			ClassName = global.GetAllCodeBehindClass;
			HeaderTitle = global.HeaderTitle_GetAll;
			Type = type;
			
        }
		private string GenerateDirective()
		{
			if (Type == InterfaceType.WEbUserControl)
			{
				return "<%@ Register TagPrefix=\"anthem\" Namespace=\"Anthem\" Assembly=\"Anthem\" %><%@ Control Language=\"c#\" AutoEventWireup=\"true\" CodeFile=\"" + ClassName + ".ascx.cs\" Inherits=\"" + ClassName + "\" %>";
			}
			else
			{
				return "<%@ Register TagPrefix=\"anthem\" Namespace=\"Anthem\" Assembly=\"Anthem\" %><%@ Page language=\"c#\" MasterPageFile=\"~/Masters/AdminMasterPage.master\" CodeFile=\"default.aspx.cs\" Inherits=\"Admin" + ClassName + "\" Theme=\"AdminSite\" %>";
			}
		}
        //
        private string GenerateControls()
        {
			string dataGridID = "dg" + global.TableProgramatlyName;
            StringBuilder controls = new StringBuilder();
            controls.Append("\n\t\t\t\t<tr>");
            #region DataGrid Declaretion And Propreties
            controls.Append("\n\t\t\t\t\t<td class=\"Result\" align=\"center\" colspan=\"2\">");
			controls.Append("\n\t\t\t\t\t\t<anthem:datagrid id=\"" + dataGridID + "\" runat=\"server\" SkinId=\"GridViewSkin\" ");
			controls.Append("\n\t\t\t\t\t\tOnDeleteCommand=\"" + dataGridID + "_DeleteCommand\" OnItemDataBound=\"" + dataGridID + "_ItemDataBound\" OnPageIndexChanged=\"" + dataGridID + "_PageIndexChanged\" OnItemCreated=\"" + dataGridID + "_ItemCreated\" AllowPaging=\"True\" AutoGenerateColumns=\"False\">");
            #endregion
			//
			if (Fields.Count > 0)
            {
                controls.Append("\n\t\t\t\t\t\t<Columns>");

                foreach (SQLDMO.Column column in Fields)
                {
                    if (ID == null || column.Name != ID.Name)//||!Globals.CheckIsAddedBySql(ID))
                    {
						if (allParameters != null && !allParameters.Contains(column.Name))
							continue;
                        controls.Append("\n\t\t\t\t\t\t\t<asp:BoundColumn DataField=\"" + column.Name + "\" HeaderText=\"" + column.Name + "\"></asp:BoundColumn>");
                    }
                }
                if (ID != null)
                {
                    controls.Append("\n\t\t\t\t\t\t\t<asp:TemplateColumn HeaderText=\" ⁄œÌ·\">");
                    controls.Append("\n\t\t\t\t\t\t\t<ItemTemplate>");
                    controls.Append("\n\t\t\t\t\t\t\t\t<asp:HyperLink id=\"hlUpdate\" runat=\"server\" ImageUrl=\"/Images/Admin/edit.gif\" CssClass=\"Link\" NavigateUrl='<%# \"Edit.aspx?" + Globals.GetProgramatlyName(ID.Name) + "=\"+DataBinder.Eval(Container.DataItem, \"" + ID.Name + "\") %>'> ⁄œÌ·"+
                    "</asp:HyperLink>");
                    controls.Append("\n\t\t\t\t\t\t\t</ItemTemplate>");
                    controls.Append("\n\t\t\t\t\t\t\t</asp:TemplateColumn>");
                   //Delete column.
					controls.Append("\n\t\t\t\t\t\t\t<asp:TemplateColumn HeaderText=\"Õ–›\">");
                    controls.Append("\n\t\t\t\t\t\t\t<ItemTemplate>");
                    controls.Append("\n\t\t\t\t\t\t\t\t<asp:ImageButton ID=\"lbtnDelete\" AlternateText=\"Õ–›\" ImageUrl=\"/Images/Admin/delete.gif\" CommandName=\"Delete\" runat=\"server\"></asp:ImageButton>");
					controls.Append("\n\t\t\t\t\t\t\t</ItemTemplate>");
                    controls.Append("\n\t\t\t\t\t\t\t</asp:TemplateColumn>");
                }
                controls.Append("\n\t\t\t\t\t\t</Columns>");
        //        controls.Append("\n\t\t\t\t\t\t<PagerStyle NextPageText=\" «· «·Ï &amp;gt;&amp;gt;&amp;gt;\" PrevPageText=\"&amp;lt;&amp;lt;&amp;lt; «·”«»ﬁ\"");
          //      controls.Append("\n\t\t\t\t\t\tHorizontalAlign=\"Right\" ForeColor=\"#4A3C8C\" BackColor=\"#E7E7FF\"></PagerStyle>");
				controls.Append("\n\t\t\t\t\t\t<PagerStyle Mode=\"NumericPages\" />");
				controls.Append("\n\t\t\t\t\t\t</anthem:datagrid>");
				controls.Append("\n\t\t\t\t\t\t<anthem:Label ID=\"lblMsg\" runat=\"server\" ForeColor=\"Red\" Text=\"·«  ÊÃœ »Ì«‰« \"></anthem:Label>");
            }
            controls.Append("\n\t\t\t\t\t</td>");
            controls.Append("\n\t\t\t\t</tr>");
            //--------------------------------

            return controls.ToString();

        }
        //
		public static void Create(InterfaceType type)
        {
			GetAll_InterfaceBuilder cr = new GetAll_InterfaceBuilder(type);
			cr.GenerateInterface();
        }
		//
		public static void Create(InterfaceType type, Hashtable allParameters, string operation)
		{
			GetAll_InterfaceBuilder cr = new GetAll_InterfaceBuilder(type);
			Globals global = new Globals();
			cr.ClassName = global.TableProgramatlyName + "_" + operation;
			cr.allParameters = allParameters;
			cr.GenerateInterface();
		}
		//
		private void GenerateInterface()
		{
			if (Type == InterfaceType.WEbUserControl)
			{
				GenerateUserControlInterface();
			}
			else
			{
				GeneratePageInterface();
			}
		}
		//
		private void GenerateUserControlInterface()
		{
			//Begin create Control and check the free text box editor
			string controls = GenerateControls();
			StringBuilder userControl = new StringBuilder();
			userControl.Append(GenerateDirective());
			userControl.Append("\n" + ControlRegisters);
			userControl.Append("\n" + TableHeader);
			userControl.Append("\n" + GenerateControls());
			userControl.Append("\n" + TableFooter);

			//
			DirectoryInfo dInfo = Directory.CreateDirectory(Globals.UserControlsDirectory + global.TableProgramatlyName);
			string path = dInfo.FullName + "\\" + ClassName + ".ascx";
			//
			FileManager.CreateFile(path, userControl.ToString());

		}
		//
		private void GeneratePageInterface()
		{
			//Begin create Control and check the free text box editor
			string controls = GenerateControls();
			StringBuilder userControl = new StringBuilder();
			userControl.Append(GenerateDirective());
			userControl.Append("\n" + ControlRegisters);
			userControl.Append("\n <asp:Content id=\"Content1\" ContentPlaceHolderID=\"BasicContent\" runat=\"server\">");
			userControl.Append("\n" + TableHeader);
			userControl.Append("\n" + GenerateControls());
			userControl.Append("\n" + TableFooter);
			userControl.Append("\n</asp:Content>");
			//
			string directoryPath = Globals.AdminFolder + global.TableProgramatlyName + "\\";
			string path = directoryPath + "\\default.aspx";
			DirectoriesManager.ChechDirectory(directoryPath);
			FileManager.CreateFile(path, userControl.ToString());
		}
        //
    }
}
