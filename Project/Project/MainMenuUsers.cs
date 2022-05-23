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
        int rooms, price;
        private MySqlDataReader dr;

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
                String username, location;
                DateTime startDate;
                int apId, userId;

                username = row.Cells[1].Value.ToString();
                location = row.Cells[2].Value.ToString();

                apId = Convert.ToInt32(row.Cells[5].Value.ToString());
                userId = Convert.ToInt32(row.Cells[6].Value.ToString());

                command = new MySqlCommand();
                command.CommandType = CommandType.Text;
                command.Connection = connection;

                command.CommandText = "DELETE FROM bookings WHERE username = '" + username + "' AND  location = '" + location + "' AND apartmentId = '" + apId + "' AND userId = '" + userId + "'";

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

        private int getUserId(string username)
        {
            int userId = 0;

            if (username == "laura_user")
            {
                 userId = 1;
            }
            else if (username == "ioana_user")
            {
                userId = 20;
            }
            else if (username == "edi_user")
            {
                userId = 21;
            }
            else if (username == "test_user")
            {
                userId = 22;
            }

            return userId;
        }

        private void createBooking_Click(object sender, EventArgs e)
        {
            this.location = locationDropDown.SelectedItem.ToString();

            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();

            startDate = startDateCalendar.SelectionStart;
            string startDateString = startDate.ToString("u");
            var item = startDateString.Split(' ');
            string finalStartDate = item[0];

            endDate = startDateCalendar.SelectionEnd;
            string endDateString = endDate.ToString("u");
            var item1 = endDateString.Split(' ');
            string finalEndDate = item1[0];

            rooms = Convert.ToInt32(nrOfRoomsDropDown.SelectedItem.ToString());
            price = Convert.ToInt32(priceDropDown.SelectedItem.ToString());

            string newusername = username;
            int userId = getUserId(newusername);
            int apId = 2;

            connection.Open();
            command = new MySqlCommand();
            command.Connection = connection;
            command.CommandText = "INSERT INTO bookings (username, location, startDate, endDate, apartmentId, userId) VALUES ('" + newusername + "' , '" + location + "' , '" + finalStartDate + "' , '" + finalEndDate + "' , '" + apId + "' , '" + userId + "')";

            //dr = command.ExecuteReader();
            int sql;
            sql = command.ExecuteNonQuery();

            if (sql > 0)
            {
                MessageBox.Show("Booking made successfully!");
            }
            else
            {
                MessageBox.Show("An error occured!");
            }
            connection.Close();

        }

        private void showPossibleBookings_Click(object sender, EventArgs e)
        {
            this.location = locationDropDown.SelectedItem.ToString();

            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();

            startDate = startDateCalendar.SelectionStart;
            string startDateString = startDate.ToString("u");
            var item = startDateString.Split(' ');
            string finalStartDate = item[0];

            endDate = startDateCalendar.SelectionEnd;
            string endDateString = endDate.ToString("u");
            var item1 = endDateString.Split(' ');
            string finalEndDate = item1[0];


            rooms = Convert.ToInt32(nrOfRoomsDropDown.SelectedItem.ToString());
            price = Convert.ToInt32(priceDropDown.SelectedItem.ToString());

            connection.Open();
            command = new MySqlCommand();
            command.Connection = connection;
            command.CommandText = "SELECT * FROM apartments INNER JOIN bookings ON apartments.id = bookings.apartmentId WHERE bookings.startDate <= '" + finalStartDate + "' AND bookings.endDate >=  '" + finalEndDate + "' AND apartments.nrOfRooms = '" + rooms + "' AND apartments.price = '" + price + "' AND apartments.location = '" + location + "'";
            //command.CommandText = "SELECT * FROM apartments INNER JOIN bookings ON apartments.id = bookings.apartmentId WHERE bookings.startDate < '" + finalStartDate + "' AND bookings.startDate BETWEEN CAST('" + finalStartDate + "' AS DATE) and CAST('" + finalEndDate  + "' AS DATE) AND bookings.endDate >  '" + finalEndDate + "' AND bookings.endDate BETWEEN CAST('" + finalStartDate + "' AS DATE) and CAST('" + finalEndDate + "' AS DATE) AND apartments.nrOfRooms = '" + rooms + "' AND apartments.price = '" + price + "' AND apartments.location = '" + location + "'";
            dr = command.ExecuteReader();

            if (dr.Read())
            {
                MessageBox.Show("Looks like a reservation is alrdeady made!");
            }
            else
            {
                MessageBox.Show("You can make you reservation now by clicking on the 'Create booking' button!");
            }
            connection.Close();
        }

        private void startDateCalendar_DateChanged(object sender, DateRangeEventArgs e)
        {
            
        }

    }
}
