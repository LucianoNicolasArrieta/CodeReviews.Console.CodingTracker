﻿using ConsoleTableExt;
using System.Configuration;
using System.Data.SQLite;

namespace coding_tracker
{
    internal class CodingController
    {
        string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");

        public void Insert(CodingSession codingSession)
        {
            string start_time = codingSession.StartTime.ToString("d/M/yyyy H:m", System.Globalization.CultureInfo.InvariantCulture);
            string end_time = codingSession.EndTime.ToString("d/M/yyyy H:m", System.Globalization.CultureInfo.InvariantCulture);
            string duration = codingSession.Duration.Hours.ToString() + "h " + codingSession.Duration.Minutes.ToString() + "min";

            using (SQLiteConnection myConnection = new SQLiteConnection(connectionString))
            {
                string query = "INSERT INTO coding_tracker ('start_time', 'end_time', 'duration') VALUES (@start_time, @end_time, @duration)";
                SQLiteCommand command = new SQLiteCommand(query, myConnection);

                myConnection.Open();

                command.Parameters.AddWithValue("@start_time", start_time);
                command.Parameters.AddWithValue("@end_time", end_time);
                command.Parameters.AddWithValue("@duration", duration);
                command.ExecuteNonQuery();
            }

            Console.Clear();
            Console.WriteLine("Record added to database successfully!");
        }

        public void ViewAll()
        {
            using (SQLiteConnection myConnection = new SQLiteConnection(connectionString))
            {
                myConnection.Open();

                string query = "SELECT * FROM coding_tracker";
                SQLiteCommand command = new SQLiteCommand(query, myConnection);
                SQLiteDataReader records = command.ExecuteReader();

                if (records != null)
                {
                    var tableData = new List<CodingSession>();

                    while (records.Read())
                    {
                        CodingSession cs = new CodingSession(records[1].ToString(), records[2].ToString());
                        cs.Id = Convert.ToInt32(records[0]);
                        tableData.Add(cs);
                    }

                    ConsoleTableBuilder.From(tableData).ExportAndWriteLine();
                }
            }
        }

        public void Update(int id)
        {
            UserInput user_input = new UserInput();

            if (!CheckIdExists(id))
            {
                Console.WriteLine($"Record with Id = {id} doesn't exist. Please try again.");
                Update(user_input.IdInput());
            }

            CodingSession newCodingSession = user_input.CodingSessionInput();
            string startTime = newCodingSession.StartTime.ToString("d/M/yyyy H:m", System.Globalization.CultureInfo.InvariantCulture);
            string endTime = newCodingSession.EndTime.ToString("d/M/yyyy H:m", System.Globalization.CultureInfo.InvariantCulture);
            string duration = newCodingSession.Duration.Hours.ToString() + "h " + newCodingSession.Duration.Minutes.ToString() + "min";

            using (SQLiteConnection myConnection = new SQLiteConnection(connectionString))
            {
                myConnection.Open();

                string query = $"UPDATE coding_tracker SET start_time = '{startTime}', end_time = '{endTime}', duration = '{duration}' WHERE Id = {id}";
                SQLiteCommand command = new SQLiteCommand(query, myConnection);
                command.ExecuteNonQuery();
            }

            Console.Clear();
            Console.WriteLine("Record updated successfully!");
        }

        private bool CheckIdExists(int id)
        {
            bool exists;
            using (SQLiteConnection myConnection = new SQLiteConnection(connectionString))
            {
                myConnection.Open();

                var cmd = myConnection.CreateCommand();
                cmd.CommandText = $"SELECT * FROM coding_tracker WHERE Id={id}";
                SQLiteDataReader reader = cmd.ExecuteReader();

                exists = reader.HasRows;

                reader.Close();
            }
            return exists;
        }

        public void ViewRecords()
        {
            this.ViewAll();
            Console.WriteLine("\nPress any key to continue to main menu");
            Console.ReadKey();
            Console.Clear();
        }

        internal void Delete(int id)
        {
            UserInput user_input = new UserInput();

            if (!CheckIdExists(id))
            {
                Console.WriteLine($"Record with Id = {id} doesn't exist. Please try again.");
                Update(user_input.IdInput());
            }

            using (SQLiteConnection myConnection = new SQLiteConnection(connectionString))
            {
                myConnection.Open();

                string query = $"DELETE FROM coding_tracker WHERE Id = {id}";
                SQLiteCommand command = new SQLiteCommand(query, myConnection);
                command.ExecuteNonQuery();

                myConnection.Close();
            }

            Console.Clear();
            Console.WriteLine("Record deleted successfully!");
        }
    }
}