using MySql.Data.MySqlClient;
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

namespace Project
{
    public partial class MainMenuAdmin : Form
    {

        public MySqlConnection connection;
        public MySqlCommand command, verifyUniqueUser;
        private MySqlDataReader dr, drUniqueUser;
        private MySqlDataAdapter dataAdapter;
        private DataSet dataSetUser;

        List<string> users = new List<string>();

        public MainMenuAdmin()
        {
            InitializeComponent();
            connection = new MySqlConnection("datasource = 127.0.0.1; port = 3306; username = root; password =; database = airbnbdb ");
            //myCon.ConnectionString = "server=localhost;user id=airbnb;database=airbnbdb;persistsecurityinfo=True";
        }

        private void exitMenuAdminBtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void exitMenuAdminBtn_MouseEnter(object sender, EventArgs e)
        {
            exitMenuAdminBtn.ForeColor = Color.Black;
        }

        private void exitMenuAdminBtn_MouseLeave(object sender, EventArgs e)
        {
            exitMenuAdminBtn.ForeColor = Color.White;
        }

        private void updateBtn_Click(object sender, EventArgs e)
        {
            String username, password;
            int id;

            id = Convert.ToInt32(userIdTxtBox.Text);
            username = newUsernameTxtBox.Text.ToString();
            password = newPasswordTxtBox.Text.ToString();

            command = new MySqlCommand();
            command.CommandType = CommandType.Text;
            command.Connection = connection;
            command.CommandText = "UPDATE users SET username = '" + username + "' AND  password = '" + password + "' WHERE id = '" + id + "'";

            connection.Open();
            command.ExecuteReader();

            connection.Close();

            getAllUsers();
        }

        private void deleteBtn_Click(object sender, EventArgs e)
        {

            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                String username, password;

                username = row.Cells[1].Value.ToString();
                password = row.Cells[2].Value.ToString();

                command = new MySqlCommand();
                command.CommandType = CommandType.Text;
                command.Connection = connection;
                command.CommandText = "DELETE FROM users WHERE username = '" + username + "' AND  password = '" + password + "'";

                connection.Open();
                command.ExecuteReader();

                connection.Close();
            }

            getAllUsers();

        }

        private void getAllUsers()
        {
            dataGridView1.DataSource = null;

            connection.Open();
            MySqlDataAdapter mySqlDataAdapterUsers = new MySqlDataAdapter("SELECT * FROM users", connection);
            //command.Connection = connection;

            DataSet dataSetUsers = new DataSet();
            mySqlDataAdapterUsers.Fill(dataSetUsers, "users");

            dataGridView1.DataSource = dataSetUsers.Tables[0];

            connection.Close();
        }

        private void MainMenuAdmin_Load(object sender, EventArgs e)
        {

        }

        private void newUsernameTxtBox_Click(object sender, EventArgs e)
        {
            newUsernameTxtBox.SelectAll();
        }

        private void newPasswordTxtBox_Click(object sender, EventArgs e)
        {
            newPasswordTxtBox.SelectAll();
        }

        private void seeAllUsersBtn_Click(object sender, EventArgs e)
        {
            userCredentialPanel.Visible = true;
            userButtonsPanel.Visible = true;

            apartmentCredentialsPanel.Visible = false;
            apartmentButtonsPanel.Visible = false;
            getAllUsers();
        }

        private bool verifyValidUsername()
        {
            string newUsername = newUsernameTxtBox.Text;
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

        private void insertBtn_Click(object sender, EventArgs e)
        {
            string username = newUsernameTxtBox.Text;
            string password = newPasswordTxtBox.Text;

            command = new MySqlCommand();
            connection.Open();
            command.CommandType = CommandType.Text;
            command.Connection = connection;
            command.CommandText = "INSERT INTO users(username, password) VALUES('" + newUsernameTxtBox.Text + "', '" + newPasswordTxtBox.Text + "'" + ")";

            verifyUniqueUser = new MySqlCommand();
            verifyUniqueUser.Connection = connection;
            verifyUniqueUser.CommandText = "SELECT * FROM users WHERE username = '" + username + "'";

            drUniqueUser = verifyUniqueUser.ExecuteReader();
            if(!drUniqueUser.Read())
            {
                if (verifyValidUsername())
                {
                    drUniqueUser.Close();
                    dr = command.ExecuteReader();
                }
            }
            else
            {
                MessageBox.Show("User already exists!");
            }

            connection.Close();

            getAllUsers();
        }

        private void getAllApartments()
        {
            dataGridView1.DataSource = null;

            connection.Open();
            MySqlDataAdapter mySqlDataAdapterUsers = new MySqlDataAdapter("SELECT * FROM apartments", connection);
            //command.Connection = connection;

            DataSet dataSetUsers = new DataSet();
            mySqlDataAdapterUsers.Fill(dataSetUsers, "apartments");

            dataGridView1.DataSource = dataSetUsers.Tables[0];

            connection.Close();
        }

        private void seeAllApartmentBtn_Click(object sender, EventArgs e)
        {
            userCredentialPanel.Visible = false;
            userButtonsPanel.Visible = false;

            apartmentCredentialsPanel.Visible = true;
            apartmentButtonsPanel.Visible = true;
            getAllApartments();
        }

        private Boolean verifyLocation(string location)
        {
            if (location != "Cluj-Napoca" || location != "Baia Mare" || location != "Brasov" || location != "Timisoara")
            {
                MessageBox.Show("Sorry! We do not cover that area.");
                return false;
            }
            return true;
        }

        private void insertApBtn_Click(object sender, EventArgs e)
        {
            string location = locationTxtBox.Text;
            int rooms = Convert.ToInt32(roomsTxtBox.Text);
            int price = Convert.ToInt32(priceTxtBox.Text);

            if (verifyLocation(location))
            {
                command = new MySqlCommand();
                command.CommandType = CommandType.Text;
                command.Connection = connection;
                command.CommandText = "INSERT INTO apartments(location, nrOfRooms, price) VALUES('" + location + "', '" + rooms + "', '" + price + "'" + ")";

                connection.Open();
                command.ExecuteReader();

                connection.Close();
                getAllApartments();
            }
    
        }

        private void deleteApButton_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                String location;
                int rooms, price;

                location = row.Cells[1].Value.ToString();
                rooms = Convert.ToInt32(row.Cells[2].Value);
                price = Convert.ToInt32(row.Cells[3].Value);

                command = new MySqlCommand();
                command.CommandType = CommandType.Text;
                command.Connection = connection;
                command.CommandText = "DELETE FROM apartments WHERE location = '" + location + "' AND  nrOfRooms = '" + rooms + "' AND price = '" + price + "'";

                connection.Open();
                command.ExecuteReader();

                connection.Close();
            }

            getAllApartments();
        }

        private void updateApBtn_Click(object sender, EventArgs e)
        {
            string location = locationTxtBox.Text;
            int rooms = Convert.ToInt32(roomsTxtBox.Text);
            int price = Convert.ToInt32(priceTxtBox.Text);
            int id = Convert.ToInt32(userIdTxtBox.Text);

            command = new MySqlCommand();
            command.CommandType = CommandType.Text;
            command.Connection = connection;
            command.CommandText = "UPDATE apartments SET location = '" + location + "' AND  nrOfRooms = '" + rooms + "' AND price = '" + price + "' WHERE id = '" + id + "'";

            connection.Open();
            command.ExecuteReader();

            connection.Close();

            getAllApartments();
        }

        private void userIdTxtBox_Click(object sender, EventArgs e)
        {
            userIdTxtBox.SelectAll();
        }

        private void locationTxtBox_Click(object sender, EventArgs e)
        {
            locationTxtBox.SelectAll();
        }

        private void roomsTxtBox_Click(object sender, EventArgs e)
        {
            roomsTxtBox.SelectAll();
        }

        private void priceTxtBox_Click(object sender, EventArgs e)
        {
            priceTxtBox.SelectAll();
        }
    }
}
