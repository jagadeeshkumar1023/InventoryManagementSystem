using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventoryManagementSystem.Forms
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        private void btnProducts_Click(object sender, EventArgs e)
        {
            ProductForm frm = new ProductForm();
            frm.Show();

        }

        private void btnSuppliers_Click(object sender, EventArgs e)
        {
            SupplierForm frm = new SupplierForm();
            frm.Show();

        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Reports Module Coming Soon");
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            LoginForm login = new LoginForm();
            login.Show();
            this.Close();
        }
    }
}
