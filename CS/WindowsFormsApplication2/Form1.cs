using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            SetDataSource();
        }

        private void SetDataSource()
        {
            DataSet dataSource = new DataSet();
            dataSource.ReadXml(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\Contacts.xml");
            gridControl1.DataSource = dataSource.Tables[0];
            gridView1.Columns["Description"].Visible = false;
            gridView1.BestFitColumns();
        }
    }
}
