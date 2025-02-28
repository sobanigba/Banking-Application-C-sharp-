using System.Reflection.Metadata.Ecma335;

namespace Final_Project;

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Security.Principal;

public abstract class Person
{

    public string firstname { get; set; }
    public string lastname { get; set; }
    public abstract void Getinfo();
}
class Program
{
    static List<CustomerData> customer_list = new List<CustomerData>();

    static void Main(string[] args)
    {
        string filePath = "customer_data.csv";
        string filepath2 = "employee_data.csv";
        customer_list = ReadCustomers(filePath);
        // foreach (var customer in customer_list)
        // {

        //     Console.WriteLine($"Account Number: {customer.Account_Number}");
        //     Console.WriteLine($"Name: {customer.First_Name} {customer.Last_Name}");
        //     Console.WriteLine($"Balance: {customer.Balance:C}");
        //     Console.WriteLine($"Account Type: {customer.Account_Type}");
        //     Console.WriteLine($"Loan Type: {customer.Loan_Type}");
        //     Console.WriteLine($"Loan Balance: {customer.Loan_Balance:C}");

        // }
        var employee_list = ReadEmployees(filepath2);

        // foreach (var employee in employee_list)
        // {
        //     Console.WriteLine($"Username: {employee.Username}");
        //     Console.WriteLine($"Password: {employee.Password}");
        //     Console.WriteLine($"Name: {employee.FirstName} {employee.LastName}");
        //     Console.WriteLine($"Title: {employee.Title}");
        // }

        Console.WriteLine($"Check {customer_list.Count} | {employee_list.Count} ");
        Console.WriteLine("Welcome to your Online Banking Application");

        //loop to interact with the user 
        bool exit = false;
        int i = 0;
        while (!exit)
        {
            Console.WriteLine("____________");
            Console.WriteLine("MAIN MENU");
            Console.WriteLine("_____________");
            Console.WriteLine("1.Account Login");
            Console.WriteLine("2.Create Account");
            Console.WriteLine("3.Admininstrator Login");
            Console.WriteLine("4.Quit\n");
            Console.WriteLine("________________________");
            Console.WriteLine("Select Option:");
            string option = Console.ReadLine();

            switch (option)
            {
            }


            switch (option)
            {
                case "1":
                    //account.checkbalance();
                    Accountlogin();
                    break;
                case "2":
                    Createaccount();
                    break;
                case "3":

                    break;
                case "4":
                    exit = true;
                    Console.WriteLine("Thank you for using the banking application. Goodbye!");
                    break;
                default:
                    Console.WriteLine("Invalid option.Please try again.");
                    break;

            }



        }
    }
    public class CustomerData : Person
    {
        public string Account_Number { get; set; }
        public string PIN { get; set; }
        public decimal Balance { get; set; }
        public string Account_Type { get; set; }
        public string Loan_Type { get; set; }
        public decimal Loan_Balance { get; set; }


        public override void Getinfo()
        {

        }
    }
    public class Employee : Person
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Title { get; set; }

        public override void Getinfo()
        {

        }
    }
    //  method to read employees from a CSV file
    public static List<Employee> ReadEmployees(string filePath)
    {
        var employees = new List<Employee>();

        try
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;

                // Skip header 
                reader.ReadLine();

                while ((line = reader.ReadLine()) != null)
                {
                    var fields = line.Split(',');

                    var employee = new Employee
                    {
                        Username = fields[0],
                        Password = fields[1],
                        firstname = fields[2],
                        lastname = fields[3],
                        Title = fields[4]
                    };

                    employees.Add(employee);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading employee data: {ex.Message}");
        }

        return employees;
    }

    public static List<CustomerData> ReadCustomers(string filePath)
    {
        var customers = new List<CustomerData>();

        try
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;


                while ((line = reader.ReadLine()) != null)
                {
                    var columns = line.Split(',');

                    if (columns.Length == 8)
                    {
                        if (!columns[0].StartsWith("183977"))
                        {
                            continue;
                        }

                        var customer = new CustomerData
                        {
                            Account_Number = columns[0],
                            PIN = columns[1],
                            firstname = columns[2],
                            lastname = columns[3],
                            Balance = decimal.Parse(columns[4]),
                            Account_Type = columns[5],
                            Loan_Type = columns[6],
                            Loan_Balance = decimal.Parse(columns[7])
                        };

                        customers.Add(customer);
                    }
                }
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading data:{ex.Message}");
        }
        return customers;


    }
    public static void Accountlogin()
    {
        Console.WriteLine("ACCOUNT LOGIN");
        bool loop = true;
        string account_num_current = "";
        while (loop)
        {
            Console.Write("Enter your account number:\n");
            string account_num = Console.ReadLine();
            bool isAllDigits = account_num.All(char.IsDigit);
            if (isAllDigits == true)
            {
                if (account_num.Length == 16)
                {
                    account_num_current = account_num;
                    loop = false;
                }
                else
                {
                    Console.WriteLine(" Account number should be 16 digits");
                }
            }
            else
            {
                Console.WriteLine("Account number must be digit only");
            }
            account_num = "";
        }
        loop = true;
        string pin_current = "";
        while (loop)
        {
            Console.Write("Enter your pin:");
            string PIN = Console.ReadLine();
            bool isAllDigits = PIN.All(char.IsDigit);
            if (isAllDigits == true)
            {
                if (PIN.Length == 4)
                {
                    pin_current = PIN;
                    loop = false;
                }
                else
                {
                    Console.WriteLine("PIN should be 4 digits");
                }
            }
            else
            {
                Console.WriteLine(" PIN must be digit only");
            }
            PIN = "";
        }
        int index = accountverification(account_num_current, pin_current);
        if (index != -1)
        {
            Console.WriteLine($"Welcome back,{customer_list[index].firstname} {customer_list[index].lastname} How can we help you today?");
            AccountServices(customer_list[index]);

        }
        else
        {

        }
    }
    static int accountverification(string account_num, string PIN)
    {
        for (int i = 0; i < customer_list.Count; i++)
        {
            if (customer_list[i].Account_Number == account_num)
            {
                if (customer_list[i].PIN == PIN)
                {
                    //client_id = i;
                    return i;
                }
            }
        }
        return -1;
    }


    //make account services display with new functions 
    public static void AccountServices(CustomerData customer)
    {
        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("ACCOUNT SERVICES");
            Console.WriteLine("1. Deposit");
            Console.WriteLine("2. Withdraw");
            Console.WriteLine("3. Transfer");
            Console.WriteLine("4. Make Loan Payment");
            Console.WriteLine("5. Balance Inquiry");
            Console.WriteLine("6. Back to Main Menu");
            Console.Write("Select Option: ");
            string option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    Deposit(customer);
                    break;
                case "2":
                    Withdraw(customer);
                    break;
                case "3":
                    Transfer(customer);
                    break;
                case "4":
                    LoanPayment(customer);
                    break;
                case "5":
                    BalanceInquiry(customer);
                    break;
                case "6":
                    Console.WriteLine("Returning to main menu...");
                    exit = true;
                    break;
                default:
                    Console.WriteLine(" Invalid option. Try again.");
                    break;
            }

        }
    }

    static void Deposit(CustomerData customer)
    {
        Console.Write("How much would you like to deposit?\n");
        string depositfunds = Console.ReadLine();
        bool isAllDigits = depositfunds.All(char.IsDigit);
        if (isAllDigits == true)
        {
            decimal amount = decimal.Parse(depositfunds);
            if (amount > 0)
            {
                customer.Balance += amount;
                Console.WriteLine($" Deposit successful. New balance: {customer.Balance:C}");
                SaveCustomer("customer_data.csv", customer_list);

            }
            else
            {
                Console.WriteLine("  You may only deposit anything above 0");
            }


        }

    }
    static void Withdraw(CustomerData customer)

    {
        Console.WriteLine("How much would you like to withdraw?");
        string withdrawfunds = Console.ReadLine();
        bool isAllDigits = withdrawfunds.All(char.IsDigit);
        if (isAllDigits == true)
        {
            decimal amount = decimal.Parse(withdrawfunds);
            if (amount > 0 && amount <= customer.Balance)
            {
                customer.Balance -= amount;
                Console.WriteLine($" Withdraw successful. New balance: {customer.Balance:C}");


            }
            else
            {
                Console.WriteLine($"Withdraw failed. Insufficient funds. Current balance:{customer.Balance:C}");

            }
        }

    }

    static void Transfer(CustomerData customer)
    {
        Console.Write("How much would you like to transfer today? ");
        string transferFunds = Console.ReadLine();
        bool isAllDigits = transferFunds.All(char.IsDigit);

        if (isAllDigits)
        {
            decimal amount = decimal.Parse(transferFunds);
            if (amount > 0 && amount <= customer.Balance)
            {
                Console.Write("Enter the account number of the recipient: ");
                string transferAccount = Console.ReadLine();
                bool isAllAccountDigits = transferAccount.All(char.IsDigit);

                if (isAllAccountDigits && transferAccount.Length == 16)
                {
                    int recipientIndex = transferverification(transferAccount);

                    if (recipientIndex != -1)
                    {
                        customer.Balance -= amount;
                        customer_list[recipientIndex].Balance += amount;

                        Console.WriteLine($"Transfer successful. New balance: {customer.Balance:C}");
                        Console.WriteLine($"Recipient {customer_list[recipientIndex].firstname} {customer_list[recipientIndex].lastname} received: {amount:C}");
                    }
                    else
                    {
                        Console.WriteLine(" Recipient account number not found.");
                    }
                }
                else
                {
                    Console.WriteLine(" Invalid account number.");
                }
            }
            else
            {
                Console.WriteLine($" Insufficient funds. Current balance: {customer.Balance:C}");
            }
        }



        //create a new function that only goes to account number 

        static int transferverification(string account_num)
        {
            for (int i = 0; i < customer_list.Count; i++)
            {
                if (customer_list[i].Account_Number == account_num)
                {
                    // customer = i;
                    return i;
                }
            }
            return -1;

        }


    }
    static void LoanPayment(CustomerData customer)
    {
        if (customer.Loan_Balance <= 0)
        {
            Console.WriteLine(" You do not have an outstanding loan balance. ");
            return;
        }
        Console.WriteLine($"Current Loan Balance:{customer.Loan_Balance:C}");
        Console.Write("Enter the amount you want to pay toward your loan:");
        string LoanPaymentInput = Console.ReadLine();
        bool isAllDigits = LoanPaymentInput.All(char.IsDigit);
        if (isAllDigits)
        {
            decimal Paymentamount = decimal.Parse(LoanPaymentInput);
            if (Paymentamount > 0)
            {
                if (Paymentamount <= customer.Balance)
                {
                    if (Paymentamount > customer.Loan_Balance)
                    {
                        Paymentamount = customer.Loan_Balance;
                    }
                    customer.Balance -= Paymentamount;
                    customer.Loan_Balance -= Paymentamount;
                    Console.WriteLine($"Payment successful. New loan balance:{customer.Loan_Balance:C}");
                    Console.WriteLine($"New account balance:{customer.Balance:C}");
                }
                else
                {
                    Console.WriteLine("Insufficient funds in your account to make a payment.");

                }
            }
        }
    }

    static void BalanceInquiry(CustomerData customer)
    {
        Console.WriteLine($"Your current balance is: {customer.Balance:C}");
    }
    public static void Createaccount()
    {
        //first name 
        Console.Write("Enter your first name:");
        string firstname = Console.ReadLine();
        // lastname 
        Console.Write("Enter your last name:");
        string lastname = Console.ReadLine();
        // create 4 digit pin 
        string pin;
        while (true)
        {
            Console.Write("Enter a 4 digit PIN:");
            pin = Console.ReadLine();

            if (pin.Length == 4 && pin.All(char.IsDigit))
            {
                break;
            }
            Console.WriteLine("PIN must be 4 digits.");
        }

        // accpunt type options
        string accountType = "";
        while (true)
        {
            Console.WriteLine("what type of account do you want to open?");
            Console.WriteLine("1. Savings");
            Console.WriteLine("2. Checking");
            Console.Write("Select Option: ");
            string choice = Console.ReadLine();

            if (choice == "1")
            {
                accountType = "Savings";
                break;
            }
            else if (choice == "2")
            {
                accountType = "Checking";
                break;
            }
            else
            {
                Console.WriteLine("Please enter 1 or 2.");
            }
        }
        //account number generator 
        Random random = new Random();
        string accountNumber = "183977" + random.Next(100000, 999999).ToString() + random.Next(100000, 999999).ToString();
        // account details 
        decimal initialBalance = 100;
        string loanType = "None";
        decimal loanBalance = 0;
        var newCustomer = new CustomerData
        {
            Account_Number = accountNumber,
            PIN = pin,
            firstname = firstname,
            lastname = lastname,
            Account_Type = accountType,
            Balance = initialBalance,
            Loan_Type = loanType,
            Loan_Balance = loanBalance
        };

        customer_list.Add(newCustomer);
        SaveCustomer(newCustomer);

        Console.WriteLine($"Congratulations {firstname}, {lastname} Your account is open with a initial deposit of {initialBalance} Your account number is: {accountNumber} You can now log into acess account services");
    }
    static void SaveCustomer(string filePath, List<CustomerData> customers)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("Account_Number,PIN,First_Name,Last_Name,Balance,Account_Type,Loan_Type,Loan_Balance");

                foreach (var customer in customers)
                {
                    writer.WriteLine($"{customer.Account_Number},{customer.PIN},{customer.firstname},{customer.lastname},{customer.Balance},{customer.Account_Type},{customer.Loan_Type},{customer.Loan_Balance}");
                }
            }
            Console.WriteLine("Customer data saved successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving customer data: {ex.Message}");
        }
    }


}




