﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using System.Security.Cryptography;

namespace StudentAttendanceRegister
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void Exit_Button_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public string CalculateMD5Hash(string input)
        {

            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }

            return sb.ToString();
        }

        private void Login_Button_Click(object sender, EventArgs e)
        {

            string connectionString = ConfigurationManager.AppSettings["DBConnection"];

            // Provide the query string with a parameter placeholder.
            string queryString = "SELECT COUNT(1) FROM users WHERE login_id = @login_id AND password_hash = @password_hash;";

            // Create and open the connection in a using block. This
            // ensures that all resources will be closed and disposed
            // when the code exits.
            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {

                // Create the Command and Parameter objects.
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@login_id", loginID_text_box.Text);
                command.Parameters.AddWithValue("@password_hash", CalculateMD5Hash(pw_textBox.Text));

                // Open the connection in a try/catch block. 
                // Create and execute the DataReader, writing the result
                // set to the console window.
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    try
                    {
                        reader.Read();
                        if (String.Format("{0}", reader[0]) == "1")
                        {
                            this.Hide();
                            SystemMenu sm = new SystemMenu();
                            sm.Show();
                        }
                        else
                        {
                            MessageBox.Show("Invalid Username or Password");
                        }
                    }
                    finally
                    {
                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Console.ReadLine();
            }
        }
    }
}
