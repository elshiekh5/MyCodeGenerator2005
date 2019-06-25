using System;
using System.Drawing;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
//

using System.Web.Configuration;
namespace SPGen
{
	/// <summary>
	/// Main UI for SPGen
	/// </summary>
	public class frmMain : System.Windows.Forms.Form
	{		
		

		private System.Windows.Forms.StatusBar statbarMain;
		private System.Windows.Forms.StatusBarPanel statbarpnlMain;
		private System.Windows.Forms.Button cmdConnect;
		private System.Windows.Forms.TextBox txtPassword;
		private System.Windows.Forms.TextBox txtUser;
		private System.Windows.Forms.ComboBox selServers;
		private System.Windows.Forms.Splitter spltrMain;
		private System.Windows.Forms.Panel pnlConnectTo;
		private System.Windows.Forms.TreeView tvwServerExplorer;
		private System.Windows.Forms.ImageList imglstMain;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtphysicalPath;
		private System.Windows.Forms.TextBox txtServerName;
		private System.Windows.Forms.TextBox txtSiteName;
        private CheckBox cbCreateSecurityModel;
		private Button btnGenerateSimpleOperation;
		private CheckBox cbHasMasterBox;
		private ComboBox selProjectType;
		private TextBox txtProjectPort;
		private Label label4;
		private System.ComponentModel.IContainer components;

		public frmMain()
		{			
			InitializeComponent();			

			// List Registered Servers
			object[] objServers = (object[])SqlProvider.obj.RegisteredServers;
			selServers.Items.AddRange(objServers);

			// Default connection details, if provided

            NameValueCollection settingsAppSettings = (NameValueCollection)WebConfigurationManager.AppSettings;			

			if (settingsAppSettings["ServerName"] != null && settingsAppSettings["ServerName"] != "")
			{
				selServers.Text = settingsAppSettings["ServerName"];
				SqlProvider.obj.ServerName = settingsAppSettings["ServerName"];
			}
			if (settingsAppSettings["UserName"] != null && settingsAppSettings["UserName"] != "")
			{
				txtUser.Text = settingsAppSettings["UserName"];
				SqlProvider.obj.UserName = settingsAppSettings["UserName"];
			}
			if (settingsAppSettings["Password"] != null && settingsAppSettings["Password"] != "")
			{
				char chPassword = '*';
				txtPassword.PasswordChar = chPassword;
				txtPassword.Text = settingsAppSettings["Password"];
				SqlProvider.obj.Password = settingsAppSettings["Password"];
			}
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
			this.statbarMain = new System.Windows.Forms.StatusBar();
			this.statbarpnlMain = new System.Windows.Forms.StatusBarPanel();
			this.pnlConnectTo = new System.Windows.Forms.Panel();
			this.cmdConnect = new System.Windows.Forms.Button();
			this.txtPassword = new System.Windows.Forms.TextBox();
			this.txtUser = new System.Windows.Forms.TextBox();
			this.selServers = new System.Windows.Forms.ComboBox();
			this.tvwServerExplorer = new System.Windows.Forms.TreeView();
			this.imglstMain = new System.Windows.Forms.ImageList(this.components);
			this.spltrMain = new System.Windows.Forms.Splitter();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.txtphysicalPath = new System.Windows.Forms.TextBox();
			this.txtSiteName = new System.Windows.Forms.TextBox();
			this.txtServerName = new System.Windows.Forms.TextBox();
			this.cbCreateSecurityModel = new System.Windows.Forms.CheckBox();
			this.btnGenerateSimpleOperation = new System.Windows.Forms.Button();
			this.cbHasMasterBox = new System.Windows.Forms.CheckBox();
			this.selProjectType = new System.Windows.Forms.ComboBox();
			this.txtProjectPort = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.statbarpnlMain)).BeginInit();
			this.pnlConnectTo.SuspendLayout();
			this.SuspendLayout();
			// 
			// statbarMain
			// 
			this.statbarMain.Location = new System.Drawing.Point(0, 325);
			this.statbarMain.Name = "statbarMain";
			this.statbarMain.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.statbarpnlMain});
			this.statbarMain.ShowPanels = true;
			this.statbarMain.Size = new System.Drawing.Size(608, 22);
			this.statbarMain.TabIndex = 5;
			// 
			// statbarpnlMain
			// 
			this.statbarpnlMain.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
			this.statbarpnlMain.Name = "statbarpnlMain";
			this.statbarpnlMain.Text = "Awaiting your orders...";
			this.statbarpnlMain.Width = 592;
			// 
			// pnlConnectTo
			// 
			this.pnlConnectTo.Controls.Add(this.cmdConnect);
			this.pnlConnectTo.Controls.Add(this.txtPassword);
			this.pnlConnectTo.Controls.Add(this.txtUser);
			this.pnlConnectTo.Controls.Add(this.selServers);
			this.pnlConnectTo.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlConnectTo.Location = new System.Drawing.Point(0, 0);
			this.pnlConnectTo.Name = "pnlConnectTo";
			this.pnlConnectTo.Size = new System.Drawing.Size(608, 42);
			this.pnlConnectTo.TabIndex = 9;
			// 
			// cmdConnect
			// 
			this.cmdConnect.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.cmdConnect.Location = new System.Drawing.Point(528, 16);
			this.cmdConnect.Name = "cmdConnect";
			this.cmdConnect.Size = new System.Drawing.Size(64, 21);
			this.cmdConnect.TabIndex = 7;
			this.cmdConnect.Text = "Connect";
			this.cmdConnect.Click += new System.EventHandler(this.cmdConnect_Click);
			// 
			// txtPassword
			// 
			this.txtPassword.Location = new System.Drawing.Point(352, 16);
			this.txtPassword.Name = "txtPassword";
			this.txtPassword.Size = new System.Drawing.Size(160, 20);
			this.txtPassword.TabIndex = 6;
			this.txtPassword.Text = "Password";
			this.txtPassword.Enter += new System.EventHandler(this.txtPassword_Enter);
			this.txtPassword.Leave += new System.EventHandler(this.txtPassword_Leave);
			this.txtPassword.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
			// 
			// txtUser
			// 
			this.txtUser.Location = new System.Drawing.Point(184, 16);
			this.txtUser.Name = "txtUser";
			this.txtUser.Size = new System.Drawing.Size(160, 20);
			this.txtUser.TabIndex = 5;
			this.txtUser.Text = "User";
			this.txtUser.Enter += new System.EventHandler(this.txtUser_Enter);
			this.txtUser.Leave += new System.EventHandler(this.txtUser_Leave);
			this.txtUser.TextChanged += new System.EventHandler(this.txtUser_TextChanged);
			// 
			// selServers
			// 
			this.selServers.Location = new System.Drawing.Point(8, 16);
			this.selServers.Name = "selServers";
			this.selServers.Size = new System.Drawing.Size(160, 21);
			this.selServers.TabIndex = 4;
			this.selServers.Text = "Server Name";
			this.selServers.Leave += new System.EventHandler(this.selServers_Leave);
			// 
			// tvwServerExplorer
			// 
			this.tvwServerExplorer.Dock = System.Windows.Forms.DockStyle.Left;
			this.tvwServerExplorer.FullRowSelect = true;
			this.tvwServerExplorer.ImageIndex = 0;
			this.tvwServerExplorer.ImageList = this.imglstMain;
			this.tvwServerExplorer.Location = new System.Drawing.Point(0, 42);
			this.tvwServerExplorer.Name = "tvwServerExplorer";
			this.tvwServerExplorer.SelectedImageIndex = 0;
			this.tvwServerExplorer.Size = new System.Drawing.Size(208, 283);
			this.tvwServerExplorer.TabIndex = 10;
			this.tvwServerExplorer.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvwServerExplorer_BeforeExpand);
			this.tvwServerExplorer.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvwServerExplorer_AfterSelect);
			// 
			// imglstMain
			// 
			this.imglstMain.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imglstMain.ImageStream")));
			this.imglstMain.TransparentColor = System.Drawing.Color.Transparent;
			this.imglstMain.Images.SetKeyName(0, "");
			this.imglstMain.Images.SetKeyName(1, "");
			this.imglstMain.Images.SetKeyName(2, "");
			// 
			// spltrMain
			// 
			this.spltrMain.Location = new System.Drawing.Point(208, 42);
			this.spltrMain.Name = "spltrMain";
			this.spltrMain.Size = new System.Drawing.Size(3, 283);
			this.spltrMain.TabIndex = 11;
			this.spltrMain.TabStop = false;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(232, 56);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 23);
			this.label1.TabIndex = 12;
			this.label1.Text = "physicalPath";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(232, 120);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(100, 23);
			this.label2.TabIndex = 13;
			this.label2.Text = "App Name";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(240, 88);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(112, 23);
			this.label3.TabIndex = 14;
			this.label3.Text = "Server Name";
			// 
			// txtphysicalPath
			// 
			this.txtphysicalPath.Location = new System.Drawing.Point(352, 56);
			this.txtphysicalPath.Name = "txtphysicalPath";
			this.txtphysicalPath.Size = new System.Drawing.Size(100, 20);
			this.txtphysicalPath.TabIndex = 15;
			this.txtphysicalPath.Text = "C:\\";
			// 
			// txtSiteName
			// 
			this.txtSiteName.Location = new System.Drawing.Point(352, 120);
			this.txtSiteName.Name = "txtSiteName";
			this.txtSiteName.Size = new System.Drawing.Size(100, 20);
			this.txtSiteName.TabIndex = 16;
			this.txtSiteName.Text = "ProjectBuilder";
			// 
			// txtServerName
			// 
			this.txtServerName.Location = new System.Drawing.Point(352, 88);
			this.txtServerName.Name = "txtServerName";
			this.txtServerName.Size = new System.Drawing.Size(100, 20);
			this.txtServerName.TabIndex = 17;
			this.txtServerName.Text = "localhost";
			// 
			// cbCreateSecurityModel
			// 
			this.cbCreateSecurityModel.AutoSize = true;
			this.cbCreateSecurityModel.Checked = true;
			this.cbCreateSecurityModel.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbCreateSecurityModel.Enabled = false;
			this.cbCreateSecurityModel.Location = new System.Drawing.Point(356, 168);
			this.cbCreateSecurityModel.Name = "cbCreateSecurityModel";
			this.cbCreateSecurityModel.Size = new System.Drawing.Size(96, 17);
			this.cbCreateSecurityModel.TabIndex = 18;
			this.cbCreateSecurityModel.Text = "Security Model";
			this.cbCreateSecurityModel.UseVisualStyleBackColor = true;
			// 
			// btnGenerateSimpleOperation
			// 
			this.btnGenerateSimpleOperation.Location = new System.Drawing.Point(344, 262);
			this.btnGenerateSimpleOperation.Name = "btnGenerateSimpleOperation";
			this.btnGenerateSimpleOperation.Size = new System.Drawing.Size(147, 23);
			this.btnGenerateSimpleOperation.TabIndex = 19;
			this.btnGenerateSimpleOperation.Text = "Generate Simple Operation";
			this.btnGenerateSimpleOperation.UseVisualStyleBackColor = true;
			this.btnGenerateSimpleOperation.Click += new System.EventHandler(this.btnGenerateSimpleOperation_Click);
			// 
			// cbHasMasterBox
			// 
			this.cbHasMasterBox.AutoSize = true;
			this.cbHasMasterBox.Checked = true;
			this.cbHasMasterBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbHasMasterBox.Location = new System.Drawing.Point(356, 191);
			this.cbHasMasterBox.Name = "cbHasMasterBox";
			this.cbHasMasterBox.Size = new System.Drawing.Size(202, 17);
			this.cbHasMasterBox.TabIndex = 21;
			this.cbHasMasterBox.Text = "Applay Master Box on  User Controls";
			this.cbHasMasterBox.UseVisualStyleBackColor = true;
			// 
			// selProjectType
			// 
			this.selProjectType.FormattingEnabled = true;
			this.selProjectType.Items.AddRange(new object[] {
            "Simple",
            "All"});
			this.selProjectType.Location = new System.Drawing.Point(356, 214);
			this.selProjectType.Name = "selProjectType";
			this.selProjectType.Size = new System.Drawing.Size(121, 21);
			this.selProjectType.TabIndex = 22;
			// 
			// txtProjectPort
			// 
			this.txtProjectPort.Location = new System.Drawing.Point(352, 142);
			this.txtProjectPort.Name = "txtProjectPort";
			this.txtProjectPort.Size = new System.Drawing.Size(100, 20);
			this.txtProjectPort.TabIndex = 23;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(235, 147);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(64, 13);
			this.label4.TabIndex = 24;
			this.label4.Text = "Project Port";
			// 
			// frmMain
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(608, 347);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.txtProjectPort);
			this.Controls.Add(this.selProjectType);
			this.Controls.Add(this.cbHasMasterBox);
			this.Controls.Add(this.btnGenerateSimpleOperation);
			this.Controls.Add(this.cbCreateSecurityModel);
			this.Controls.Add(this.txtServerName);
			this.Controls.Add(this.txtSiteName);
			this.Controls.Add(this.txtphysicalPath);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.spltrMain);
			this.Controls.Add(this.tvwServerExplorer);
			this.Controls.Add(this.pnlConnectTo);
			this.Controls.Add(this.statbarMain);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "frmMain";
			this.Text = "SPGen: Stored Procedure Generator";
			this.Load += new System.EventHandler(this.frmMain_Load);
			((System.ComponentModel.ISupportInitialize)(this.statbarpnlMain)).EndInit();
			this.pnlConnectTo.ResumeLayout(false);
			this.pnlConnectTo.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new frmMain());
		}

		private void cmdConnect_Click(object sender, System.EventArgs e)
		{						
			// First ensure connection details are valid
			if (SqlProvider.obj.ServerName == "" || SqlProvider.obj.UserName == "")
			{   
				MessageBox.Show("Please enter in valid connection details.",this.Text);				
			}
			else
			{
				this.Cursor = Cursors.WaitCursor;
				statbarpnlMain.Text = "Connecting to SQL Server...";

				//Valid connection details				
				tvwServerExplorer.Nodes.Clear();

				// List Databases
				try
				{				
					SqlProvider.obj.Connect();
					Array aDatabases = (Array)SqlProvider.obj.Databases;
					SqlProvider.obj.DisConnect();

					for (int i = 0; i < aDatabases.Length; i++)
					{
						TreeNode treenodeDatabase = new TreeNode(aDatabases.GetValue(i).ToString(), 0, 0);
						treenodeDatabase.Nodes.Add("");
						tvwServerExplorer.Nodes.Add(treenodeDatabase);
					}

					this.Cursor = Cursors.Default;
					statbarpnlMain.Text = "Connectiong successful, databases listed...";
				}
				catch
				{				
					this.Cursor = Cursors.Default;
					statbarpnlMain.Text = "Connectiong un-successful...";
					MessageBox.Show("Connection to database failed. Please check your Server Name, User and Password.", this.Text);
				}
			}
		}

		private void txtPassword_Enter(object sender, System.EventArgs e)
		{
			if (txtPassword.Text == "Password") 
			{
				txtPassword.Text = "";
				char chPassword = '*';
				txtPassword.PasswordChar = chPassword;
			}
		}

		private void txtUser_Enter(object sender, System.EventArgs e)
		{
			if (txtUser.Text == "User") txtUser.Text = "";
		}

		private void tvwServerExplorer_BeforeExpand(object sender, System.Windows.Forms.TreeViewCancelEventArgs e)
		{
			// List all Tables for selected Database

			if (e.Node.ImageIndex == 0)
			{
				this.Cursor = Cursors.WaitCursor;
				// Set database to get tables from						
				SqlProvider.obj.Database = e.Node.Text;				
						
				// Clear dummy node
				e.Node.Nodes.Clear();

				try
				{
					ProjectBuilder.ProjectName=txtSiteName.Text ;
					ProjectBuilder.ProjectPort = txtProjectPort.Text;
					ProjectBuilder.ServerName=txtServerName.Text ;
					ProjectBuilder.PhysicalPath=txtphysicalPath.Text;
					ProjectBuilder.ProjectName=txtSiteName.Text ;
					ProjectBuilder.HasMasterBox = cbHasMasterBox.Checked;
					ProjectBuilder.ProjectType = (ProjectType)selProjectType.SelectedIndex;
					ProjectBuilder.CreateProject();
					this.Cursor = Cursors.Default;

					
					
					
				}
				catch(Exception ex)
				{
					this.Cursor = Cursors.Default;
					statbarpnlMain.Text = "Problem listing Tables...";

					MessageBox.Show(ex.Message);
				}
			}
		}

		private void tvwServerExplorer_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			TreeNode tnodeSelected = (TreeNode)e.Node;
			if(tnodeSelected.ImageIndex == 1)
			{
				// SP selected, generate SP
				TreeNode tnodeTable = (TreeNode)tnodeSelected;				
				SqlProvider.obj.TableName = tnodeTable.Text;
			}
		}
		
		private void txtUser_Leave(object sender, System.EventArgs e)
		{
			if (txtUser.Text == "")
				txtUser.Text = "User";
			else
				SqlProvider.obj.UserName = txtUser.Text;
		}

		private void txtPassword_Leave(object sender, System.EventArgs e)
		{
				SqlProvider.obj.Password = txtPassword.Text;
		}

		private void selServers_Leave(object sender, System.EventArgs e)
		{			
			if (selServers.Text == "")
				selServers.Text = "Select server";
			else
				SqlProvider.obj.ServerName = selServers.Text;
		}

		private void btnGenerateSimpleOperation_Click(object sender, EventArgs e)
		{
			ProjectBuilder.ProjectName = txtSiteName.Text;
			ProjectBuilder.ProjectPort = txtProjectPort.Text;
			ProjectBuilder.ServerName	= txtServerName.Text;
			ProjectBuilder.PhysicalPath = txtphysicalPath.Text;
			ProjectBuilder.ProjectName = txtSiteName.Text;
			ProjectBuilder.HasMasterBox = cbHasMasterBox.Checked;
			ProjectBuilder.ProjectType = (ProjectType)selProjectType.SelectedIndex;
			//
			SqlProvider.obj.Password = txtPassword.Text;
			SqlProvider.obj.ServerName = txtServerName.Text;
			//
			Form1 f = new Form1();
			f.Show();
		}

		private void button2_Click(object sender, EventArgs e)
		{
			Form2 f = new Form2();
			f.Show();
		}

		private void frmMain_Load(object sender, EventArgs e)
		{
			selProjectType.SelectedIndex = 0;
		}

		private void txtPassword_TextChanged(object sender, EventArgs e)
		{

		}

		private void txtUser_TextChanged(object sender, EventArgs e)
		{

		}



	}
}
