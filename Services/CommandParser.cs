using ExpenseTracker.Models;
using ExpenseTracker.Models.Command_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Services
{
    // This class validates command structure. it is not responsible for validating business logic.
    public class CommandParser
    {

        public CommandParser()
        {

        }


        public AddCommandData ParseAdd(string[] args, int requiredParameters)
        {

            if (args.Length != requiredParameters) throw new ArgumentException(InvalidCommandMessage());

            // checks if the flag is present and at the correct position
            int descriptionFlagIndex = Array.IndexOf(args, "--description");
            int amountFlagIndex = Array.IndexOf(args, "--amount");

            int categoryFlagIndex = Array.IndexOf(args, "--category");
            if (descriptionFlagIndex == -1 || descriptionFlagIndex % 2 != 0 || (amountFlagIndex == -1 || amountFlagIndex % 2 != 0 || categoryFlagIndex == -1 || categoryFlagIndex % 2 != 0))
            {
                throw new ArgumentException(InvalidCommandMessage());
            }


            if (!int.TryParse(args[amountFlagIndex + 1], out int _amount))
            {
                throw new ArgumentException("Invalid value: amount should be of type integer.");
            }


            var addCommandData = new AddCommandData
            {
                Description = args[descriptionFlagIndex + 1],
                Category = args[categoryFlagIndex + 1],
                Amount = _amount,

            };


            return addCommandData;
        }

        // update method doesn't have required parameters. user should be able to update any number of properties.
        public UpdateCommandData ParseUpdate(string[] args)
        {

            // args array only contains flags and their corresponding values in pairs, therefore the amount of arguments should be even.
            // it should have minimum of 4 elements, eg. --id 2 --description "new description"
            if (args.Length < 4 || args.Length % 2 != 0)
            {
                throw new ArgumentException(InvalidCommandMessage());
            }


            if (args[0] != "--id")
            {
                throw new ArgumentException(InvalidCommandMessage());
            }

            if (!int.TryParse(args[1], out int id))
            {
                throw new ArgumentException("Invalid argument: Id should be of type integer");
            }


            // if no error was thrown that means that id is present in a correct format
            var updateCommandData = new UpdateCommandData();
            updateCommandData.Id = id;

            args = args.Skip(2).ToArray();

            for (int i = 0; i < args.Length - 1; i += 2)
            {
                if (args[i] == "--category")
                {
                    updateCommandData.Category = args[i + 1];
                }

                else if (args[i] == "--description")
                {
                    updateCommandData.Description = args[i + 1];
                }

                else if (args[i] == "--amount")
                {
                    if (int.TryParse(args[i + 1], out int _amount))
                    {
                        updateCommandData.Amount = _amount;
                    }
                    else
                    {
                        throw new ArgumentException("Invalid argument: amount should be of type integer.");
                    }
                }
            }

            if (string.IsNullOrEmpty(updateCommandData.Description) && string.IsNullOrEmpty(updateCommandData.Category) && updateCommandData.Amount == null)
            {
                throw new ArgumentException(InvalidCommandMessage());
            }


            return updateCommandData;
        }

        public DeleteCommandData ParseDelete(string[] args, int requiredParameters)
        {
            if (args.Length != requiredParameters) throw new ArgumentException(InvalidCommandMessage());


            if (args[0] != "--id")
            {
                throw new ArgumentException(InvalidCommandMessage());
            }

            if (!int.TryParse(args[1], out int id))
            {
                throw new ArgumentException("Invalid argument: Id should be of type integer");
            }

            var deleteCommandData = new DeleteCommandData();
            deleteCommandData.Id = id;
            return deleteCommandData;
        }


        public string InvalidCommandMessage()
        {
            return @"Invalid command: write \help for instructions";
        }
    }
}
