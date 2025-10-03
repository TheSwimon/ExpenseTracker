# Expense Tracker CLI Application


Expense Tracker is a C# CLI application that helps with tracking the expenses for the user. It provides commands for managing the expenses records.

It is inspired by a project idea provided by [roadmap.sh](https://roadmap.sh/projects/expense-tracker).

## features
* Adding a new record
* Updating a record
* Deleting a record
* Logging summary of expenses
* Logging details of all expenses

## prerequisite

#### You need to install a few things to be able to utilize the application:
[.NET SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)\
[Git](https://git-scm.com/downloads)\
Code editor (optional)

## Installation

#### Clone Repository
```
git clone https://github.com/TheSwimon/ExpenseTracker.git
cd ExpenseTracker
```

#### Build the project and navigate to the bin directory
```
dotnet build
cd bin\debug\net9.0
```

#### Run the executable and pass the "\help" command for instructions
```
ExpenseTracker \help

output:
Available operations: Add, Update, Delete, List details of all expenses, List summary of all expenses.

Example commands:
# Add entry - "ExpenseTracker add --description "pizza" --category "groceries" --amount 20"
# Update entry - "ExpenseTracker update --id 9 --description "khinkali" --amount 25"
# Delete entry - "ExpenseTracker delete --id 8"
# Details of all expenses - "list"
# summary of all expenses - "summary"

Guide:
order of the key-value pairs in Add command doesn't matter and all properties must be present.
order of the key-value pairs in Update command doesn't matter, you have to provide at least one value to update the property, the first key-value pair must be the ID of the record.
```
