using System;
using System.IO;
using System.Text ;
using System.Data;
using System.Windows.Forms;
using System.Collections;
namespace SPGen
{
    /// <summary>
    /// Summary description for Ceate_CodeBehindBuilder.
    /// </summary>
    public class Create_InterfaceBuilder : InterfaceBuilder
    {
		//
		Hashtable allParameters = null;
		//
        public Create_InterfaceBuilder(InterfaceType type)
        {
			ClassName = global.CreateCodeBehindClass;
			HeaderTitle = global.HeaderTitle_Create;
			Type = type;
			
        }
		private string GenerateDirective()
		{
			if (Type == InterfaceType.WEbUserControl)
			{
				return  "<%@ Control Language=\"c#\" AutoEventWireup=\"true\" CodeFile=\"" + ClassName + ".ascx.cs\" Inherits=\"" + ClassName + "\" %>";
			}
			else
			{
				return  "<%@ Page language=\"c#\" MasterPageFile=\"~/Masters/AdminMasterPage.master\" CodeFile=\"Add.aspx.cs\" Inherits=\"Admin" + ClassName + "\" Theme=\"AdminSite\" %>";
			}
		}
        //

        private string GenerateControls()
        {
            StringBuilder controls = new StringBuilder();
            controls.Append("\n\t\t\t\t<tr>");
            controls.Append("\n\t\t\t\t\t<td class=\"Result\" align=\"center\" colspan=\"2\">");
            controls.Append("\n\t\t\t\t\t\t<asp:Label id=\"lblResult\" runat=\"server\"></asp:Label>");
            controls.Append("\n\t\t\t\t\t</td>");
            controls.Append("\n\t\t\t\t</tr>");
            string datatype;
            foreach (Column column in Fields)
            {
				if (allParameters != null && !allParameters.Contains(column.Name))
					continue;
                if (ID == null || column.Name != ID.Name || !Globals.CheckIsAddedBySql(ID))
                {

                    datatype = Globals.GetAliasDataType(column.Datatype);
                    if (datatype == "bool")
                    {
                        controls.Append("\n\t\t\t\t<tr>");
                        controls.Append("\n\t\t\t\t\t<td class=\"Title\">" + column.Name + "</td>");
                        controls.Append("\n\t\t\t\t\t<td class=\"Control\">");
                        controls.Append("\n\t\t\t\t\t\t<asp:CheckBox id=\"cb" + Globals.GetProgramatlyName(column.Name) + "\" runat=\"server\" ValidationGroup=\""+ClassName+"\" ></asp:CheckBox>");
                        controls.Append("\n\t\t\t\t\t</td>");
                        controls.Append("\n\t\t\t\t</tr>");
                    }
					else if(Globals.GetSqlDataType(column.Datatype)==SqlDbType.NText)
					{
						controls.Append("\n\t\t\t\t<tr>");
						controls.Append("\n\t\t\t\t\t<td class=\"Title\">" + column.Name + "</td>");
						controls.Append("\n\t\t\t\t\t<td class=\"Control\">");
                        //FREETEXTBOX
                        controls.Append("\n\t\t\t\t\t\t<FTB:FREETEXTBOX id=\"ftb" + Globals.GetProgramatlyName(column.Name) + "\"   runat=\"server\"  TextDirection=\"RightToLeft\" ");
                        controls.Append("\n\t\t\t\t\t\tToolbarLayout=\"Bold,Italic,Underline,Strikethrough,Superscript,Subscript;");
                        controls.Append("\n\t\t\t\t\t\tJustifyLeft,JustifyRight,JustifyCenter,JustifyFull;");
                        controls.Append("\n\t\t\t\t\t\tCut,Copy,Paste,Delete,Undo,Redo,Print,Save,ieSpellCheck|");
                        controls.Append("\n\t\t\t\t\t\tParagraphMenu,FontFacesMenu,FontSizesMenu,FontForeColorsMenu,FontBackColorsMenu,FontForeColorPicker,FontBackColorPicker|StyleMenu,SymbolsMenu,InsertHtmlMenu|InsertRule,InsertDate,InsertTime|WordClean|");
                        controls.Append("\n\t\t\t\t\t\tCreateLink,Unlink;RemoveFormat,BulletedList,NumberedList,Indent,Outdent;InsertTable,EditTable,InsertTableRowBefore,InsertTableRowAfter,DeleteTableRow,InsertTableColumnBefore,InsertTableColumnAfter,DeleteTableColumn|\"");
                        controls.Append("\n\t\t\t\t\t\tSupportFolder=\"/phyEditorImages/FreeTextBox/\" ButtonSet=\"NotSet\"  Width=\"450px\" ButtonWidth=\"21\"></FTB:FREETEXTBOX>");						//
                        //-----------
						controls.Append("\n\t\t\t\t\t</td>");
						controls.Append("\n\t\t\t\t</tr>");
						//
						IshasFreeTextBoxControl = true;
					}
                    else if (datatype != "byte[]" && datatype != "Object" && datatype != "Guid")
                    {
                        TableConstraint cnstr = SqlProvider.obj.GetParentColumn(column.Name);
                        controls.Append("\n\t\t\t\t<tr>");
                        controls.Append("\n\t\t\t\t\t<td class=\"Title\">" + column.Name + "</td>");
                        controls.Append("\n\t\t\t\t\t<td class=\"Control\">");
                        if (cnstr == null)
                        {
							controls.Append("\n\t\t\t\t\t\t<asp:TextBox id=\"txt" + Globals.GetProgramatlyName(column.Name) + "\" runat=\"server\"  CssClass=\"Control\" MaxLength=\"" + Globals.GetTextBoxMaxLength(column) + "\" ValidationGroup=\"" + ClassName + "\" ></asp:TextBox>");
                            if (!column.AllowNulls)
								controls.Append("\n\t\t\t\t\t\t<asp:RequiredFieldValidator id=\"rfv" + Globals.GetProgramatlyName(column.Name) + "\" runat=\"server\" ErrorMessage=\"*\" ControlToValidate=\"txt" + Globals.GetProgramatlyName(column.Name) + "\" ValidationGroup=\"" + ClassName + "\">√œŒ· Â–« «·»Ì«‰</asp:RequiredFieldValidator>");
                        }
                        else
							controls.Append("\n\t\t\t\t\t\t<asp:DropDownList id=\"ddl" + Globals.GetProgramatlyName(cnstr.ParentTable) + "\" runat=\"server\" CssClass=\"Control\" ValidationGroup=\"" + ClassName + "\" ></asp:DropDownList>");

                        controls.Append("\n\t\t\t\t\t</td>");
                        controls.Append("\n\t\t\t\t</tr>");
                    }
                }
            }
            controls.Append("\n\t\t\t\t<tr>");
            controls.Append("\n\t\t\t\t\t<td class=\"Result\" align=\"center\" colspan=\"2\">");
			controls.Append("\n\t\t\t\t\t\t<asp:Button id=\"btnCreate\" runat=\"server\" Width=\"100px\" Text=\"≈÷«›…\" CssClass=\"Submit\" OnClick=\"btnCreate_Click\" ValidationGroup=\"" + ClassName + "\"></asp:Button>");
            controls.Append("\n\t\t\t\t\t</td>");
            controls.Append("\n\t\t\t\t</tr>");

            return controls.ToString();
        }
        //
        public static void Create(InterfaceType type)
        {
			Create_InterfaceBuilder cr = new Create_InterfaceBuilder(type);
			cr.GenerateInterface();
        }
		//
		public static void Create(InterfaceType type, Hashtable allParameters ,string operation )
		{
			Create_InterfaceBuilder cr = new Create_InterfaceBuilder(type);
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
			string controls=GenerateControls();
            StringBuilder userControl = new StringBuilder();
            userControl.Append(GenerateDirective());
            userControl.Append("\n" + ControlRegisters);
            userControl.Append("\n" + TableHeader);
			userControl.Append("\n" + controls);
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
			userControl.Append("\n" + controls);
			userControl.Append("\n" + TableFooter);
			userControl.Append("\n</asp:Content>");	
			//
			string directoryPath = Globals.AdminFolder + global.TableProgramatlyName + "\\";
			string path = directoryPath + "\\Add.aspx";
			DirectoriesManager.ChechDirectory(directoryPath);
			FileManager.CreateFile(path, userControl.ToString());
		}
        //
    }
}
