using System;
using System.IO;
using System.Text;
namespace SPGen
{
	public enum ProjectType
	{
		Simple,
		All
	}
	/// <summary>
	/// Summary description for ProjectBuilder.
	/// </summary>
	public class ProjectBuilder 
	{
		public static string ProjectName="";
		public static string PhysicalPath="";
		public static string ProjectPort = "";
		public static string ServerName="";
        public static bool CreateSecurityModel = false;
		public static bool HasMasterBox = false;
		public static ProjectType ProjectType ;

        public static string NameSpace
        {
            get { return ProjectName; }
        }
		public static void CreateProject()
		{
			SqlProvider.obj.Connect();
			CreateThetemplateFiles();
            AdminNavigationBuilder adminNavBuilder = new AdminNavigationBuilder();

			foreach(SQLDMO.Table table in SqlProvider.obj.Tables )
			{
				if(!table.SystemObject)
				{
                    SqlProvider.obj.Refresh();
					SqlProvider.obj.Table=table;
					SqlProvider.obj.TableName=table.Name;
					//
                    StoredProcedure.Create();
					SqlDataProviderBuilder.Create();
					//DataProviderBuilder.Create();This Step was Canceled
                    ClassEntityBuilder.Create();
                    ClassFactoryBuilder.Create();
                    
					if(!SqlProvider.obj.ISTableForRelations)
					{
						//Create User Control
						Create_InterfaceBuilder.Create(InterfaceType.WEbUserControl);
						Create_CodeBehindBuilder.Create(InterfaceType.WEbUserControl);
						//Admin Add Page
						Create_InterfaceBuilder.Create(InterfaceType.WebForm);
						Create_CodeBehindBuilder.Create(InterfaceType.WebForm);
						//-----------------------------------------------------------
						//Edit User Control
						Update_CodeBehindBuilder.Create(InterfaceType.WEbUserControl);
						Update_InterfaceBuilder.Create(InterfaceType.WEbUserControl);
						//Admin Edit Page
						Update_InterfaceBuilder.Create(InterfaceType.WebForm);
						Update_CodeBehindBuilder.Create(InterfaceType.WebForm);
						//-----------------------------------------------------------
						//Get All User Control
						GetAll_InterfaceBuilder.Create(InterfaceType.WEbUserControl);
						GetAll_CodeBehindBuilder.Create(InterfaceType.WEbUserControl);
						//Admin Default Page
						GetAll_InterfaceBuilder.Create(InterfaceType.WebForm);
						GetAll_CodeBehindBuilder.Create(InterfaceType.WebForm);
						//
						adminNavBuilder.AddItems();
					}
				}
			}
			if (ProjectBuilder.ProjectType == ProjectType.All)
			{
				SecurityBuilder.Create();
				adminNavBuilder.CreateControlFile();
			}
			
			SqlProvider.obj.DisConnect();
            CreateSLN();

            WebConfigBuilder.Create();
			//VirtualDirectoryBuilder.Create();
		}

		public static void CreateThetemplateFiles()
		{
			if (!Directory.Exists(Globals.BaseDirectory))
			{
				if (ProjectBuilder.ProjectType == ProjectType.Simple)
					DirectoriesManager.Copy("SimpleProjectBuilderSrc", Globals.BaseDirectory, true);
				else
					DirectoriesManager.Copy("ProjectBuilderSrc", Globals.BaseDirectory, true);
			}
		}
		private static void CreateSLN()
		{
			//-----------------------------------
			string slnFile="ProjectBuilder.sln";
			StreamReader _reader			= null;
			
			string lineOfText;
			StringBuilder sb = new StringBuilder();
			if( false == System.IO.File.Exists( slnFile )) 
			{
				throw new Exception("File " + slnFile + " does not exists");
			}
			using( Stream stream = System.IO.File.OpenRead( slnFile ) ) 
			{
				_reader = new StreamReader( stream );
				while(true) 
				{
					lineOfText = _reader.ReadLine();
					if( lineOfText == null ) 
					{
						string _class=sb.ToString();
						_class = _class.Replace("{0}", ProjectPort);
                        _class = _class.Replace("{1}", PhysicalPath);
//						_class=_class.Replace("{1}",Guid.NewGuid().ToString());
//						_class=_class.Replace("{2}",Guid.NewGuid().ToString());
						//-----------------------------------
						string path = Globals.BaseDirectory+ProjectName+".sln";
						// Create a file to write to.
						using (StreamWriter sw = File.CreateText(path)) 
						{
							sw.WriteLine(_class);				
						}    
						return;
						//-----------------------------------
					}
					else
						sb.Append(lineOfText + Environment.NewLine);
				}
				
				
			}
			//-----------------------------------
		}
		//----------------------------------
		private static void CreateWebinfo()
		{
			//-----------------------------------
			string webinfo="ProjectBuilder.csproj.webinfo";
			StreamReader _reader			= null;
			
			string lineOfText;
			StringBuilder sb = new StringBuilder();
			if( false == System.IO.File.Exists( webinfo )) 
			{
				throw new Exception("File " + webinfo + " does not exists");
			}
			using( Stream stream = System.IO.File.OpenRead( webinfo ) ) 
			{
				_reader = new StreamReader( stream );
				while(true) 
				{
					lineOfText = _reader.ReadLine();
					if( lineOfText == null ) 
					{
						string _class=sb.ToString();
						_class=_class.Replace("{0}",ProjectName);
						//						_class=_class.Replace("{1}",Guid.NewGuid().ToString());
						//						_class=_class.Replace("{2}",Guid.NewGuid().ToString());
						//-----------------------------------
						string path = Globals.BaseDirectory+ProjectName+".csproj.webinfo";
						// Create a file to write to.
						using (StreamWriter sw = File.CreateText(path)) 
						{
							
							sw.WriteLine(_class);				
						}    
						return;
						//-----------------------------------
					}
					else
						sb.Append(lineOfText + Environment.NewLine);
				}
				
				
			}
			//-----------------------------------
		}
	}
}
