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

namespace Project
{
    public partial class MainMenuUsers : Form
    {
        public MySqlConnection connection;
        public MySqlCommand command;
        String username, location;

        public MainMenuUsers(String username, String password)
        {
            InitializeComponent();
            connection = new MySqlConnection("datasource = 127.0.0.1; port = 3306; username = root; password =; database = airbnbdb ");

            this.username = username;
            welcomeLabel.Text = "Welcome back " + username + "!";
        }

        private void MainMenuUsers_Load(object sender, EventArgs e)
        {

        }

        private void exitBtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void seeYourBookingsButton_Click(object sender, EventArgs e)
        {
            showBookingsPanel.Visible = true;
            createBookingPanel.Visible = false;

            getMyBookings();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                String username, password;

                username = row.Cells[1].Value.ToString();
                password = row.Cells[2].Value.ToString();

                command = new MySqlCommand();
                command.CommandType = CommandType.Text;
                command.Connection = connection;

                //NEED TO CHANGE QUERY
                command.CommandText = "DELETE FROM bookings WHERE username = '" + username + "' AND  password = '" + password + "'";

                connection.Open();
                command.ExecuteReader();

                connection.Close();
            }

            getMyBookings();
        }

        private void getMyBookings()
        {
            dataGridView1.DataSource = null;

            connection.Open();
            MySqlDataAdapter mySqlDataAdapterBookings = new MySqlDataAdapter("SELECT * FROM bookings WHERE username = '" + username + "'", connection);

            DataSet dataSetBookings = new DataSet();
            mySqlDataAdapterBookings.Fill(dataSetBookings, "bookings");

            dataGridView1.DataSource = dataSetBookings.Tables[0];

            connection.Close();
        }

        private void newBookingBtn_Click(object sender, EventArgs e)
        {
            showBookingsPanel.Visible = false;
            createBookingPanel.Visible = true;
            createBookingPanel.Focus();
        }

        private void locationDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void createBooking_Click(object sender, EventArgs e)
        {

        }

        private void showPossibleBookings_Click(object sender, EventArgs e)
        {
            this.location = locationDropDown.SelectedItem.ToString();
            textBox1.Text = location;

            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();

            startDate = startDateCalendar.SelectionStart;
            endDate = startDateCalendar.SelectionEnd;
            textBox2.Text = startDate.ToString();
            textBox3.Text = endDate.ToString();

            dataGridView1.DataSource = null;

            connection.Open();
            MySqlDataAdapter mySqlDataAdapterBookings = new MySqlDataAdapter("SELECT * FROM bookings WHERE username = '" + username + "'", connection);
        }

        private void startDateCalendar_DateChanged(object sender, DateRangeEventArgs e)
        {
            
        }

    }
}
