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
    public partial class SupplierForm : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-BM0F44D;Initial Catalog=InventoryDB;Integrated Security=True");
        int supplierId = 0;

        public SupplierForm()
        {
            InitializeComponent();
            SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-BM0F44D;Initial Catalog=InventoryDB;Integrated Security=True");

            int supplierId = 0;

        }
        void LoadSuppliers()
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Suppliers", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dgvSuppliers.DataSource = dt;
        }
        bool ValidateSupplier()
        {
            if (txtSupplierName.Text.Trim() == "")
            {
                MessageBox.Show("Supplier Name is required");
                txtSupplierName.Focus();
                return false;
            }

            if (txtPhone.Text.Trim() == "")
            {
                MessageBox.Show("Phone number is required");
                txtPhone.Focus();
                return false;
            }

            if (!long.TryParse(txtPhone.Text, out _))
            {
                MessageBox.Show("Phone must be numeric");
                txtPhone.Focus();
                return false;
            }

            if (txtAddress.Text.Trim() == "")
            {
                MessageBox.Show("Address is required");
                txtAddress.Focus();
                return false;
            }

            return true;
        }
        private void SupplierForm_Load(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
           
        
            if (!ValidateSupplier())
                return;

            con.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO Suppliers (SupplierName,Phone,Address) VALUES (@name,@phone,@address)", con);

            cmd.Parameters.AddWithValue("@name", txtSupplierName.Text);
            cmd.Parameters.AddWithValue("@phone", txtPhone.Text);
            cmd.Parameters.AddWithValue("@address", txtAddress.Text);

            cmd.ExecuteNonQuery();
            con.Close();

            MessageBox.Show("Supplier Added");

            LoadSuppliers();
        }

        private void dgvSuppliers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvSuppliers.Rows[e.RowIndex];

                supplierId = Convert.ToInt32(row.Cells["SupplierId"].Value);

                txtSupplierName.Text = row.Cells["SupplierName"].Value.ToString();
                txtPhone.Text = row.Cells["Phone"].Value.ToString();
                txtAddress.Text = row.Cells["Address"].Value.ToString();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
           
        
            if (supplierId == 0)
            {
                MessageBox.Show("Select supplier first");
                return;
            }

            if (!ValidateSupplier())
                return;

            con.Open();

            SqlCommand cmd = new SqlCommand("UPDATE Suppliers SET SupplierName=@name, Phone=@phone, Address=@address WHERE SupplierId=@id", con);

            cmd.Parameters.AddWithValue("@name", txtSupplierName.Text);
            cmd.Parameters.AddWithValue("@phone", txtPhone.Text);
            cmd.Parameters.AddWithValue("@address", txtAddress.Text);
            cmd.Parameters.AddWithValue("@id", supplierId);

            cmd.ExecuteNonQuery();
            con.Close();

            MessageBox.Show("Supplier Updated");

            LoadSuppliers();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
           
        
            if (supplierId == 0)
            {
                MessageBox.Show("Select supplier first");
                return;
            }

            con.Open();

            SqlCommand cmd = new SqlCommand("DELETE FROM Suppliers WHERE SupplierId=@id", con);
            cmd.Parameters.AddWithValue("@id", supplierId);

            cmd.ExecuteNonQuery();
            con.Close();

            MessageBox.Show("Supplier Deleted");

            LoadSuppliers();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
           
        
            txtSupplierName.Clear();
            txtPhone.Clear();
            txtAddress.Clear();
            supplierId = 0;
        }
    }
    }
    
    
    
    

