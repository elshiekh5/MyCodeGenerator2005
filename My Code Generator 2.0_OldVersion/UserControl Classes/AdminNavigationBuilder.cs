using System;
using System.Text ;
namespace SPGen
{
	/// <summary>
	/// Summary description for AdminNavigationBuilder.
	/// </summary>
	public class AdminNavigationBuilder:SimplefaceBuilder 
	{
		public AdminNavigationBuilder()
		{
			ClassName="AdminNavigation";
			HeaderTitle="Navigation Menu";
		}
		private  StringBuilder _NavigationItems=new StringBuilder();
		
        protected override string GenerateControls()
        {
            StringBuilder controls= new StringBuilder();
            controls.Append("\n\t\t\t\t<tr>");
			controls.Append("\n\t\t\t\t\t<td class=\"AdminNavMain\" align=\"center\">Users</td>");
			controls.Append("\n\t\t\t\t</tr>");
			/*
			controls.Append("\n\t\t\t\t<tr>");
			controls.Append("\n\t\t\t\t\t<td class=\"AdminNavSub\"><a class=\"AdminNav\" href=\"/"+Globals.AdminWebPath+"Users/CreateUser.aspx\">Create User</a></td>");
			controls.Append("\n\t\t\t\t</tr>");
			*/
			controls.Append("\n\t\t\t\t<tr>");
			controls.Append("\n\t\t\t\t\t<td class=\"AdminNavSub\"><a class=\"AdminNav\" href=\"/"+Globals.AdminWebPath+"Users/ViewAllUsers.aspx\">View All Users</a></td>");
			controls.Append("\n\t\t\t\t</tr>");
            return controls.ToString() + _NavigationItems.ToString();
        }
        //
		public  void AddItems()
		{
			Globals global=new Globals();
	 		//depend on the current table
            _NavigationItems.Append("\n\t\t\t\t<tr>");
            _NavigationItems.Append("\n\t\t\t\t\t<td class=\"AdminNavMain\" align=\"center\">" + global.TableSqlName + "</td>");
            _NavigationItems.Append("\n\t\t\t\t</tr>");
            _NavigationItems.Append("\n\t\t\t\t<tr>");
            _NavigationItems.Append("\n\t\t\t\t\t<td class=\"AdminNavSub\"><a class=\"AdminNav\" href=\"/" + Globals.AdminWebPath + global.TableProgramatlyName + "/" + Globals.CreatePageName + "\">Create " + global.TableSqlName + "</a></td>");
            _NavigationItems.Append("\n\t\t\t\t</tr>");
            _NavigationItems.Append("\n\t\t\t\t<tr>");
            _NavigationItems.Append("\n\t\t\t\t\t<td class=\"AdminNavSub\"><a class=\"AdminNav\" href=\"/" + Globals.AdminWebPath + global.TableProgramatlyName + "/" + Globals.GetAllPageName + "\">View All " + global.TableSqlName + "</a></td>");
            _NavigationItems.Append("\n\t\t\t\t</tr>");
		}
        //
        public override void CreateControlFile()
		{
			string path= Globals.UserControlsDirectory+"Admin\\"+ClassName+".ascx";
            FileManager.CreateFile(path, GenerateUserControl());	
		} 
        
        
	}
}
