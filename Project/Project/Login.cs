using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Project
{
    public partial class Login : Form
    {
        public MySqlConnection connection;
        public MySqlCommand command;
        private MySqlDataReader dr;

        public Login()
        {
            InitializeComponent();
            connection = new MySqlConnection("datasource = 127.0.0.1; port = 3306; username = root; password =; database = airbnbdb ");
        }

        
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private bool verifyIfUser()
        {
            string username = usernameTxtBox.Text;
            if (username.Contains("_user"))
            {
                return true;
            }
            return false;
        }

        private bool verifyIfAdmin()
        {
            string username = usernameTxtBox.Text;
            if (username.Contains("_admin"))
            {
                return true;
            }
            return false;
        }

        private void loginBtn_Click(object sender, EventArgs e)
        {
            connection.Open();
            command = new MySqlCommand();
            command.Connection = connection;
            command.CommandText = "SELECT * FROM users WHERE username = '" + usernameTxtBox.Text + "' AND password = '" + passTxtBox.Text + "'";
            dr = command.ExecuteReader();

            if (dr.Read())
            {
                if (verifyIfUser())
                {
                    MainMenuUsers mainMenuUsers = new MainMenuUsers(usernameTxtBox.Text, passTxtBox.Text);
                    mainMenuUsers.Show();
                    this.Hide();
                }
                else if (verifyIfAdmin())
                {
                    MainMenuAdmin mainMenuAdmin = new MainMenuAdmin();
                    mainMenuAdmin.Show();
                    this.Hide();
                }
            }
            else
            {
                MessageBox.Show("User not found! Create a new account!");
            }
            connection.Close();
        }

        private void insertNewUserBtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Registration registration = new Registration();
            registration.Show();
            this.Hide();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void usernameTxtBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                usernameTxtBox.ForeColor = Color.White;
            } 
            catch
            {

            }
        }

        private void passTxtBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                passTxtBox.ForeColor = Color.White;
                passTxtBox.PasswordChar = '*';
            }
            catch
            {

            }
        }

        private void usernameTxtBox_click(object sender, EventArgs e)
        {
            usernameTxtBox.SelectAll();
        }

        private void passTxtBox_click(object sender, EventArgs e)
        {
            passTxtBox.SelectAll();
        }

        private void loginBtn_MouseEnter(object sender, EventArgs e)
        {
            loginBtn.ForeColor = Color.Black;
        }

        private void loginBtn_MouseLeave(object sender, EventArgs e)
        {
            loginBtn.ForeColor = Color.White;
        }

        private void exitBtn_MouseEnter(object sender, EventArgs e)
        {
            exitBtn.ForeColor = Color.Black;
        }

        private void exitBtn_MouseLeave(object sender, EventArgs e)
        {
            exitBtn.ForeColor = Color.White;
        }
    }
 }
