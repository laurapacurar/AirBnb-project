using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Project
{
    public partial class Registration : Form
    {
        public MySqlConnection connection;
        public MySqlCommand command, verifyUniqueUser;
        private MySqlDataReader dr, drUniqueUser;

        public Registration()
        {
            InitializeComponent();
            connection = new MySqlConnection("datasource = 127.0.0.1; port = 3306; username = root; password =; database = airbnbdb ");
        }

        private void backToLoginLbl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Hide();
        }

        private bool verifySamePasswords()
        {
            if (newPassTxtBox.Text != confirmPassTxtBox.Text)
            {
                MessageBox.Show("Recheck passwords!");
                return false;
            }
            else
            {
                return true;
            }
        }

        private void exitBtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Registration_Load(object sender, EventArgs e)
        {

        }

        private void registerBtn_MouseEnter(object sender, EventArgs e)
        {
            registerBtn.ForeColor = Color.Black;
        }

        private void registerBtn_MouseLeave(object sender, EventArgs e)
        {
            registerBtn.ForeColor = Color.White;
        }

        private void newUserTxtBox_TextChanged(object sender, EventArgs e)
        {
            newUserTxtBox.ForeColor = Color.White;
        }

        private void newPassTxtBox_TextChanged(object sender, EventArgs e)
        {
            newPassTxtBox.ForeColor = Color.White;
            newPassTxtBox.PasswordChar = '*';
        }

        private void confirmPassTxtBox_TextChanged(object sender, EventArgs e)
        {
            confirmPassTxtBox.ForeColor = Color.White;
            confirmPassTxtBox.PasswordChar = '*';
        }

        private void exitBtn_MouseEnter(object sender, EventArgs e)
        {
            exitBtn.ForeColor = Color.Black;
        }

        private void exitBtn_MouseLeave(object sender, EventArgs e)
        {
            exitBtn.ForeColor = Color.White;
        }

        private void newUserTxtBox_Click(object sender, EventArgs e)
        {
            newUserTxtBox.SelectAll();
        }

        private void newPassTxtBox_click(object sender, EventArgs e)
        {
            newPassTxtBox.SelectAll();
        }

        private void confirmPassTxtBox_Click(object sender, EventArgs e)
        {
            confirmPassTxtBox.SelectAll();
        }

        private bool verifyValidUsername()
        {
            string newUsername = newUserTxtBox.Text;
            if (newUsername.Contains("_user") || newUsername.Contains("_admin"))
            {
                return true;
            }
            else
            {
                MessageBox.Show("The username must contain _user OR _admin !");
                return false;
            }
        }

/*        private bool verifyUniqueUser()
        {
            string newUsername = newUserTxtBox.Text;
            connection.Open();
            command = new MySqlCommand();
            command.Connection = connection;
            command.CommandText = "SELECT COUNT(*) FROM users WHERE username = '" + newUsername + "'";

            dr = command.ExecuteReader();

            if (dr.Read())
            {
                connection.Close();
                return true;
            }
            else
            {
                MessageBox.Show("User already exists!");
                connection.Close();
                return false;
            }
            
        }*/

        private void registerBtn_Click(object sender, EventArgs e)
        {
            string newUsername = newUserTxtBox.Text;

            connection.Open();
            command = new MySqlCommand();
            command.Connection = connection;
            command.CommandText = "INSERT INTO users (username, password) VALUES ('" + newUserTxtBox.Text + "', '" + newPassTxtBox.Text + "'" + ")";

            verifyUniqueUser = new MySqlCommand();
            verifyUniqueUser.Connection = connection;
            verifyUniqueUser.CommandText = "SELECT * FROM users WHERE username = '" + newUsername + "'";

            drUniqueUser = verifyUniqueUser.ExecuteReader();
            if (!drUniqueUser.Read())
            {
                if (verifySamePasswords() && verifyValidUsername())
                {
                    drUniqueUser.Close();
                    dr = command.ExecuteReader();
                    MessageBox.Show("New user added successfully!");
                    Login login = new Login();
                    login.Show();
                    this.Hide();
                }
            }
            else 
            {
                MessageBox.Show("User already exists!");
            }
            connection.Close();
        }
    }
}
