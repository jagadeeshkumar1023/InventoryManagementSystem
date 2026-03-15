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
    public partial class SalesHistoryForm : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-BM0F44D;Initial Catalog=InventoryDB;Integrated Security=True");

        public SalesHistoryForm()
        {
            InitializeComponent();
        }
        void LoadSales()
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Sales", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dgvSales.DataSource = dt;
        }

        private void SalesHistoryForm_Load(object sender, EventArgs e)
        {
            LoadSales();

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            
        
            SqlDataAdapter da = new SqlDataAdapter(
                "SELECT * FROM Sales WHERE CustomerName LIKE '%" + txtSearch.Text + "%'",
                con);

            DataTable dt = new DataTable();
            da.Fill(dt);

            dgvSales.DataSource = dt;
        }
    }
    }

