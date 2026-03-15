using InventoryManagementSystem.DataAccess;
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
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void lblUsername_Click(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = DatabaseHelper.GetConnection())
            {
                string query =
                "SELECT COUNT(*) FROM Users WHERE Username=@user AND Password=@pass";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@user", txtUsername.Text);
                cmd.Parameters.AddWithValue("@pass", txtPassword.Text);

                con.Open();

                int count = (int)cmd.ExecuteScalar();

                if (count > 0)
                {
                    MessageBox.Show("Login Successful");
                }
                else
                {
                    MessageBox.Show("Invalid Login");
                }
            }
        }
    }
    }

    

