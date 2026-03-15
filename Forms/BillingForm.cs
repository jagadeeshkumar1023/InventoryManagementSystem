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
    public partial class BillingForm : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-BM0F44D;Initial Catalog=InventoryDB;Integrated Security=True");

        decimal grandTotal = 0;

        public BillingForm()
        {
            InitializeComponent();
        }


        void LoadProducts()
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT ProductId, ProductName FROM Products", con);
            DataTable dt = new DataTable();
            da.Fill(dt);

            cmbProduct.DisplayMember = "ProductName";
            cmbProduct.ValueMember = "ProductId";
            cmbProduct.DataSource = dt;
        }


        private void lblTitle_Click(object sender, EventArgs e)
        {

        }

        private void BillingForm_Load(object sender, EventArgs e)
        {
            LoadProducts();

        }

        private void dgvBilling_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void lblQuantity_Click(object sender, EventArgs e)
        {

        }
        bool ValidateBilling()
        {
            if (txtCustomer.Text.Trim() == "")
            {
                MessageBox.Show("Enter Customer Name");
                return false;
            }

            if (cmbProduct.SelectedIndex == -1)
            {
                MessageBox.Show("Select Product");
                return false;
            }

            if (txtQuantity.Text.Trim() == "")
            {
                MessageBox.Show("Enter Quantity");
                return false;
            }

            if (!int.TryParse(txtQuantity.Text, out _))
            {
                MessageBox.Show("Quantity must be numeric");
                return false;
            }

            return true;
        }

        private void txtQuantity_TextChanged(object sender, EventArgs e)
        {


            if (txtQuantity.Text == "" || txtPrice.Text == "")
                return;

            decimal price = Convert.ToDecimal(txtPrice.Text);
            int qty = Convert.ToInt32(txtQuantity.Text);

            txtTotal.Text = (price * qty).ToString();
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {


            if (!ValidateBilling())
                return;

            int productId = Convert.ToInt32(cmbProduct.SelectedValue);
            string productName = cmbProduct.Text;
            decimal price = Convert.ToDecimal(txtPrice.Text);
            int qty = Convert.ToInt32(txtQuantity.Text);
            decimal total = Convert.ToDecimal(txtTotal.Text);

            dgvBilling.Rows.Add(productId, productName, price, qty, total);

            grandTotal += total;

            txtGrandTotal.Text = grandTotal.ToString();

            txtQuantity.Clear();
            txtTotal.Clear();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {


            if (dgvBilling.SelectedRows.Count > 0)
            {
                decimal value = Convert.ToDecimal(dgvBilling.SelectedRows[0].Cells["Total"].Value);

                grandTotal -= value;

                txtGrandTotal.Text = grandTotal.ToString();

                dgvBilling.Rows.RemoveAt(dgvBilling.SelectedRows[0].Index);
            }
        }

        private void cmbProduct_SelectedIndexChanged(object sender, EventArgs e)
        {


            if (cmbProduct.SelectedValue == null)
                return;

            SqlCommand cmd = new SqlCommand("SELECT Price FROM Products WHERE ProductId=@id", con);
            cmd.Parameters.AddWithValue("@id", cmbProduct.SelectedValue);

            con.Open();
            txtPrice.Text = cmd.ExecuteScalar().ToString();
            con.Close();
        }

        private void btnGenerateBill_Click(object sender, EventArgs e)
        {
         
        
            if (txtCustomer.Text.Trim() == "")
            {
                MessageBox.Show("Enter Customer Name");
                return;
            }

            if (dgvBilling.Rows.Count == 0)
            {
                MessageBox.Show("Add items first");
                return;
            }

            con.Open();

            SqlCommand cmd = new SqlCommand(
            "INSERT INTO Sales (CustomerName, SaleDate, TotalAmount) VALUES (@name, GETDATE(), @total); SELECT SCOPE_IDENTITY();",
            con);

            cmd.Parameters.AddWithValue("@name", txtCustomer.Text);
            cmd.Parameters.AddWithValue("@total", Convert.ToDecimal(txtGrandTotal.Text));

            int saleId = Convert.ToInt32(cmd.ExecuteScalar());

            // LOOP THROUGH DATAGRIDVIEW
            foreach (DataGridViewRow row in dgvBilling.Rows)
            {
                int productId = Convert.ToInt32(row.Cells["ProductId"].Value);
                decimal price = Convert.ToDecimal(row.Cells["Price"].Value);
                int qty = Convert.ToInt32(row.Cells["Quantity"].Value);
                decimal total = Convert.ToDecimal(row.Cells["Total"].Value);

                // BLOCK 4 → INSERT SALEITEMS
                SqlCommand itemCmd = new SqlCommand(
                "INSERT INTO SaleItems (SaleId, ProductId, Price, Quantity, Total) VALUES (@saleId,@pid,@price,@qty,@total)",
                con);

                itemCmd.Parameters.AddWithValue("@saleId", saleId);
                itemCmd.Parameters.AddWithValue("@pid", productId);
                itemCmd.Parameters.AddWithValue("@price", price);
                itemCmd.Parameters.AddWithValue("@qty", qty);
                itemCmd.Parameters.AddWithValue("@total", total);

                itemCmd.ExecuteNonQuery();

                // BLOCK 5 → UPDATE PRODUCT STOCK
                SqlCommand stockCmd = new SqlCommand(
                "UPDATE Products SET Quantity = Quantity - @qty WHERE ProductId = @pid",
                con);

                stockCmd.Parameters.AddWithValue("@qty", qty);
                stockCmd.Parameters.AddWithValue("@pid", productId);

                stockCmd.ExecuteNonQuery();
            }

            con.Close();

            MessageBox.Show("Bill Generated Successfully");

            dgvBilling.Rows.Clear();
            txtGrandTotal.Clear();
        }
    }
       
}


       



       
    
    
    
    
    
  
