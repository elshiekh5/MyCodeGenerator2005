namespace SPGen
{
	partial class Form2
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
			this.lvT_Tables = new System.Windows.Forms.ListView();
			this.lvcT_TableHeader = new System.Windows.Forms.ColumnHeader();
			this.SuspendLayout();
			// 
			// lvT_Tables
			// 
			this.lvT_Tables.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lvT_Tables.CheckBoxes = true;
			this.lvT_Tables.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.lvcT_TableHeader});
			this.lvT_Tables.FullRowSelect = true;
			this.lvT_Tables.GridLines = true;
			this.lvT_Tables.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lvT_Tables.HideSelection = false;
			this.lvT_Tables.Location = new System.Drawing.Point(148, 92);
			this.lvT_Tables.Name = "lvT_Tables";
			this.lvT_Tables.Size = new System.Drawing.Size(154, 346);
			this.lvT_Tables.Sorting = System.Windows.Forms.SortOrder.Ascending;
			this.lvT_Tables.TabIndex = 18;
			this.lvT_Tables.UseCompatibleStateImageBehavior = false;
			this.lvT_Tables.View = System.Windows.Forms.View.Details;
			// 
			// lvcT_TableHeader
			// 
			this.lvcT_TableHeader.Text = "Table / view name";
			this.lvcT_TableHeader.Width = 431;
			// 
			// Form2
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(809, 531);
			this.Controls.Add(this.lvT_Tables);
			this.Name = "Form2";
			this.Text = "Form2";
			this.Load += new System.EventHandler(this.Form2_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListView lvT_Tables;
		private System.Windows.Forms.ColumnHeader lvcT_TableHeader;

	}
}