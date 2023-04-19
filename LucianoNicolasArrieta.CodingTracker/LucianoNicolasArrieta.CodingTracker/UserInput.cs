﻿namespace coding_tracker
{
    internal class UserInput
    {
        private Menu menu = new Menu();
        private Validator validator = new Validator();
        public CodingSession CodingSessionInput()
        {
            Console.WriteLine("\nWhen did you start coding? Please follow this format: 'd/M/yyyy H:m' (Type 0 to return to menu)");
            string startTime = Console.ReadLine();
            while (!validator.ValidateDate(startTime) && !startTime.Equals("0"))
            {
                Console.WriteLine("The input doesn't follow the specified format (d/M/yyyy H:m). Try again:");
                startTime = Console.ReadLine();
            }
            if (startTime == "0")
            {
                Console.Clear();
                menu.GetUserOption();
            }

            Console.WriteLine("\nWhen did you stop coding? Follow the same previous format: 'd/M/yyyy H:m' (Type 0 to return to menu)");
            string endTime = Console.ReadLine();
            while (!validator.ValidateDate(endTime) && !endTime.Equals("0"))
            {
                Console.WriteLine("The input doesn't follow the specified format (d/M/yyyy H:m). Try again:");
                endTime = Console.ReadLine();
            }
            if (endTime == "0")
            {
                Console.Clear();
                menu.GetUserOption();
            }

            return new CodingSession(startTime, endTime);
        }

        public int IdInput()
        {
            Console.Write("Please enter the ID of the record you want to select (type r to return to menu): ");
            string input = Console.ReadLine();
            int id;
            while (!validator.ValidateNumber(input, out id) && !input.Equals("r"))
            {
                Console.WriteLine("The input isn't a number. Try again:");
                input = Console.ReadLine();
            }
            if (input == "r")
            {
                Console.Clear();
                menu.GetUserOption();
            }

            return id;
        }
    }
}