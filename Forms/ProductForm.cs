using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using InventoryManagementSystem.DataAccess;

namespace InventoryManagementSystem.Forms
{
    public partial class ProductForm : Form
    {
        int selectedProductId = 0;

        public ProductForm()
        {
            InitializeComponent();
        }

        private void ProductForm_Load(object sender, EventArgs e)
        {
            LoadProducts();
        }

        private void LoadProducts()
        {
            using (SqlConnection con = DatabaseHelper.GetConnection())
            {
                string query = "SELECT * FROM Products";

                SqlDataAdapter da = new SqlDataAdapter(query, con);

                DataTable dt = new DataTable();

                da.Fill(dt);

                dgvProducts.DataSource = dt;
            }
        }

        // VALIDATION METHOD
        private bool ValidateProduct()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Product name is required");
                txtName.Focus();
                return false;
            }

            if (!decimal.TryParse(txtPrice.Text, out decimal price))
            {
                MessageBox.Show("Enter valid price");
                txtPrice.Focus();
                return false;
            }

            if (price <= 0)
            {
                MessageBox.Show("Price must be greater than 0");
                txtPrice.Focus();
                return false;
            }

            if (!int.TryParse(txtQty.Text, out int qty))
            {
                MessageBox.Show("Enter valid quantity");
                txtQty.Focus();
                return false;
            }

            if (qty < 0)
            {
                MessageBox.Show("Quantity cannot be negative");
                txtQty.Focus();
                return false;
            }

            return true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateProduct())
                return;

            using (SqlConnection con = DatabaseHelper.GetConnection())
            {
                string query = "INSERT INTO Products(ProductName,Price,Quantity) VALUES(@name,@price,@qty)";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@name", txtName.Text);
                cmd.Parameters.AddWithValue("@price", txtPrice.Text);
                cmd.Parameters.AddWithValue("@qty", txtQty.Text);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Product Added Successfully");

            LoadProducts();
            ClearFields();
        }

        private void dgvProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvProducts.Rows[e.RowIndex];

            selectedProductId = Convert.ToInt32(row.Cells[0].Value);

            txtName.Text = row.Cells[1].Value.ToString();
            txtPrice.Text = row.Cells[2].Value.ToString();
            txtQty.Text = row.Cells[3].Value.ToString();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedProductId == 0)
            {
                MessageBox.Show("Select a product first");
                return;
            }

            if (!ValidateProduct())
                return;

            using (SqlConnection con = DatabaseHelper.GetConnection())
            {
                string query = "UPDATE Products SET ProductName=@name, Price=@price, Quantity=@qty WHERE ProductId=@id";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@name", txtName.Text);
                cmd.Parameters.AddWithValue("@price", txtPrice.Text);
                cmd.Parameters.AddWithValue("@qty", txtQty.Text);
                cmd.Parameters.AddWithValue("@id", selectedProductId);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Product Updated Successfully");

            LoadProducts();
            ClearFields();
        }
        private void lblSearch_Click(object sender, EventArgs e)
        {

        }
        

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedProductId == 0)
            {
                MessageBox.Show("Select a product first");
                return;
            }

            using (SqlConnection con = DatabaseHelper.GetConnection())
            {
                string query = "DELETE FROM Products WHERE ProductId=@id";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@id", selectedProductId);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Product Deleted Successfully");

            LoadProducts();
            ClearFields();
        }
        

        private void btnSearch_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = DatabaseHelper.GetConnection())
            {
                string query = "SELECT * FROM Products WHERE ProductName LIKE @name";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@name", "%" + txtSearch.Text + "%");

                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();

                da.Fill(dt);

                dgvProducts.DataSource = dt;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            txtName.Text = "";
            txtPrice.Text = "";
            txtQty.Text = "";
            selectedProductId = 0;
        }
    }
}





