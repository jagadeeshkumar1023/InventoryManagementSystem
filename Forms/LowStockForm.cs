using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventoryManagementSystem.Forms
{
    public partial class LowStockForm : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-BM0F44D;Initial Catalog=InventoryDB;Integrated Security=True");
       

        public LowStockForm()
        {
            InitializeComponent();
        }
        void LoadLowStock()
        {
            SqlDataAdapter da = new SqlDataAdapter(
                "SELECT ProductId, ProductName, Quantity FROM Products WHERE Quantity < 5",
                con);

            DataTable dt = new DataTable();
            da.Fill(dt);

            dgvLowStock.DataSource = dt;
        }

        private void LowStockForm_Load(object sender, EventArgs e)
        {

            LoadLowStock();


        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadLowStock();

        }
    }
}
