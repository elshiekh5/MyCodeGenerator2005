namespace SPGen
{
	partial class Form1
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.selDatabases = new System.Windows.Forms.ComboBox();
			this.selTables = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.txtProcedure = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.btnGenerate = new System.Windows.Forms.Button();
			this.selOperationType = new System.Windows.Forms.ComboBox();
			this.label7 = new System.Windows.Forms.Label();
			this.txtProcedureAndMethodName = new System.Windows.Forms.TextBox();
			this.lvOperationParametars = new System.Windows.Forms.ListView();
			this.lvcT_TableHeader = new System.Windows.Forms.ColumnHeader();
			this.lvWhereparameter = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.label5 = new System.Windows.Forms.Label();
			this.selInterfaceType = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// selDatabases
			// 
			this.selDatabases.FormattingEnabled = true;
			this.selDatabases.Location = new System.Drawing.Point(92, 28);
			this.selDatabases.Name = "selDatabases";
			this.selDatabases.Size = new System.Drawing.Size(121, 21);
			this.selDatabases.TabIndex = 1;
			this.selDatabases.SelectedIndexChanged += new System.EventHandler(this.selDatabases_SelectedIndexChanged);
			// 
			// selTables
			// 
			this.selTables.FormattingEnabled = true;
			this.selTables.Location = new System.Drawing.Point(92, 64);
			this.selTables.Name = "selTables";
			this.selTables.Size = new System.Drawing.Size(121, 21);
			this.selTables.TabIndex = 2;
			this.selTables.SelectedIndexChanged += new System.EventHandler(this.selTables_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 31);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(53, 13);
			this.label1.TabIndex = 3;
			this.label1.Text = "Database";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(6, 67);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(33, 13);
			this.label2.TabIndex = 4;
			this.label2.Text = "Table";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(67, 141);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(110, 13);
			this.label3.TabIndex = 7;
			this.label3.Text = "OperationParameters";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(251, 141);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(97, 13);
			this.label4.TabIndex = 8;
			this.label4.Text = "Where parameters";
			// 
			// txtProcedure
			// 
			this.txtProcedure.Location = new System.Drawing.Point(254, 28);
			this.txtProcedure.Multiline = true;
			this.txtProcedure.Name = "txtProcedure";
			this.txtProcedure.Size = new System.Drawing.Size(615, 90);
			this.txtProcedure.TabIndex = 10;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(254, 9);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(109, 13);
			this.label6.TabIndex = 11;
			this.label6.Text = "procedure Statement";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(794, 124);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 12;
			this.button1.Text = "View Procedure";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// btnGenerate
			// 
			this.btnGenerate.Location = new System.Drawing.Point(713, 124);
			this.btnGenerate.Name = "btnGenerate";
			this.btnGenerate.Size = new System.Drawing.Size(75, 23);
			this.btnGenerate.TabIndex = 13;
			this.btnGenerate.Text = "Generate";
			this.btnGenerate.UseVisualStyleBackColor = true;
			this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
			// 
			// selOperationType
			// 
			this.selOperationType.FormattingEnabled = true;
			this.selOperationType.Items.AddRange(new object[] {
            "Create",
            "Update"});
			this.selOperationType.Location = new System.Drawing.Point(92, 96);
			this.selOperationType.Name = "selOperationType";
			this.selOperationType.Size = new System.Drawing.Size(121, 21);
			this.selOperationType.TabIndex = 14;
			this.selOperationType.SelectedIndexChanged += new System.EventHandler(this.selOperationType_SelectedIndexChanged);
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(6, 104);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(82, 13);
			this.label7.TabIndex = 15;
			this.label7.Text = "Operation Type";
			// 
			// txtProcedureAndMethodName
			// 
			this.txtProcedureAndMethodName.Location = new System.Drawing.Point(666, 2);
			this.txtProcedureAndMethodName.Name = "txtProcedureAndMethodName";
			this.txtProcedureAndMethodName.Size = new System.Drawing.Size(100, 20);
			this.txtProcedureAndMethodName.TabIndex = 16;
			this.txtProcedureAndMethodName.Text = "Operation";
			this.txtProcedureAndMethodName.Leave += new System.EventHandler(this.txtProcedureAndMethodName_Leave);
			// 
			// lvOperationParametars
			// 
			this.lvOperationParametars.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lvOperationParametars.CheckBoxes = true;
			this.lvOperationParametars.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.lvcT_TableHeader});
			this.lvOperationParametars.FullRowSelect = true;
			this.lvOperationParametars.GridLines = true;
			this.lvOperationParametars.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lvOperationParametars.HideSelection = false;
			this.lvOperationParametars.Location = new System.Drawing.Point(23, 157);
			this.lvOperationParametars.Name = "lvOperationParametars";
			this.lvOperationParametars.Size = new System.Drawing.Size(154, 448);
			this.lvOperationParametars.Sorting = System.Windows.Forms.SortOrder.Ascending;
			this.lvOperationParametars.TabIndex = 19;
			this.lvOperationParametars.UseCompatibleStateImageBehavior = false;
			this.lvOperationParametars.View = System.Windows.Forms.View.Details;
			this.lvOperationParametars.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lvOperationParametars_ItemChecked);
			this.lvOperationParametars.SelectedIndexChanged += new System.EventHandler(this.lvOperationParametars_SelectedIndexChanged);
			// 
			// lvcT_TableHeader
			// 
			this.lvcT_TableHeader.Text = "Table";
			this.lvcT_TableHeader.Width = 431;
			// 
			// lvWhereparameter
			// 
			this.lvWhereparameter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lvWhereparameter.CheckBoxes = true;
			this.lvWhereparameter.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
			this.lvWhereparameter.FullRowSelect = true;
			this.lvWhereparameter.GridLines = true;
			this.lvWhereparameter.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lvWhereparameter.HideSelection = false;
			this.lvWhereparameter.Location = new System.Drawing.Point(219, 157);
			this.lvWhereparameter.Name = "lvWhereparameter";
			this.lvWhereparameter.Size = new System.Drawing.Size(154, 448);
			this.lvWhereparameter.Sorting = System.Windows.Forms.SortOrder.Ascending;
			this.lvWhereparameter.TabIndex = 20;
			this.lvWhereparameter.UseCompatibleStateImageBehavior = false;
			this.lvWhereparameter.View = System.Windows.Forms.View.Details;
			this.lvWhereparameter.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lvWhereparameter_ItemChecked);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Table";
			this.columnHeader1.Width = 431;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(536, 189);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(66, 13);
			this.label5.TabIndex = 21;
			this.label5.Text = "Output type";
			// 
			// selInterfaceType
			// 
			this.selInterfaceType.FormattingEnabled = true;
			this.selInterfaceType.Items.AddRange(new object[] {
            "User Control",
            "Web Page"});
			this.selInterfaceType.Location = new System.Drawing.Point(654, 186);
			this.selInterfaceType.Name = "selInterfaceType";
			this.selInterfaceType.Size = new System.Drawing.Size(121, 21);
			this.selInterfaceType.TabIndex = 22;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(907, 613);
			this.Controls.Add(this.selInterfaceType);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.lvWhereparameter);
			this.Controls.Add(this.lvOperationParametars);
			this.Controls.Add(this.txtProcedureAndMethodName);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.selOperationType);
			this.Controls.Add(this.btnGenerate);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.txtProcedure);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.selTables);
			this.Controls.Add(this.selDatabases);
			this.Name = "Form1";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ComboBox selDatabases;
		private System.Windows.Forms.ComboBox selTables;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtProcedure;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button btnGenerate;
		private System.Windows.Forms.ComboBox selOperationType;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox txtProcedureAndMethodName;
		private System.Windows.Forms.ListView lvOperationParametars;
		private System.Windows.Forms.ColumnHeader lvcT_TableHeader;
		private System.Windows.Forms.ListView lvWhereparameter;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ComboBox selInterfaceType;
	}
}