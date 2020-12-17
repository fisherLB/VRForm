using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GraphicsEvaluatePlatform.ImageValidSystem
{
    public partial class FrmProcessSingle : Form
    {
        public FrmProcessSingle()
        {
            InitializeComponent();
        }

        private void FrmProcessSingle_Load(object sender, EventArgs e)
        {

            DataTable dtClass = new DataTable("dtClass");
            dtClass.Columns.Add("name");
            dtClass.Columns.Add("id");
            dtClass.Rows.Add(new object[] { "JPG", "0" });
            dtClass.Rows.Add(new object[] { "GIF", "1" });
            dtClass.Rows.Add(new object[] { "BMP", "2" });

            cmbType.DisplayMember = "name";
            cmbType.ValueMember = "id";
            cmbType.DataSource = dtClass;
        }
    }
}
