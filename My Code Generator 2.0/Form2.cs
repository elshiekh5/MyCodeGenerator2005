using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SPGen
{
	public partial class Form2 : Form
	{
		public Form2()
		{
			InitializeComponent();
		}

		private void Form2_Load(object sender, EventArgs e)
		{
			ListViewItem lvItem =  lvT_Tables.Items.Add("sadsa"); 
			lvItem.Checked = true;
			ListViewItem lvItem2 = lvT_Tables.Items[0];
			MessageBox.Show(lvItem2.Checked.ToString());
			
		}
	}
}