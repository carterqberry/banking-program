// Project Prologue
// Name: Carter Quesenberry
// Date: 7/13/2024
// Purpose: Lab #04 Banking Program - Part 3
// 
// I declare that the following code was written by me or provided 
// by the instructor for this project. I understand that copying source 
// code from any other source constitutes plagiarism, and that I will receive 
// a zero on this project if I am found in violation of this policy.

// main program class for the banking application:
public class Program
{
    public static void Main()
    {
        // create account manager to manage accounts:
        AccountManager manager = new AccountManager();

        // main program loop:
        while (true)
        {
            // print menu options:
            Console.WriteLine("1. Create Account");
            Console.WriteLine("2. Search Account");
            Console.WriteLine("3. Exit");
            Console.Write("Choose an option: ");

            // get users choice:
            string choice = Console.ReadLine();

            // process users choice:
            switch (choice)
            {
                case "1":
                    CreateAccount(manager);
                    break;
                case "2":
                    SearchAccount(manager);
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    Console.WriteLine();
                    break;
            }
        }
    }

    // create a new account based on user input:
    private static void CreateAccount(AccountManager manager)
    {
    // prompt for account holders name:
    Console.WriteLine();
    Console.Write("Enter name: ");
    string name = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(name))
    {
        Console.WriteLine("Name cannot be empty.");
        return;
    }

    string address = "";
    decimal balance = 0;

    if (!manager.HasExistingAccount(name))
    {
        Console.Write("Enter address: ");
        address = Console.ReadLine();
    }
    else
    {
        Console.WriteLine("Account found.");
        Console.WriteLine();
        var existingAccounts = manager.FindAccounts(name);
        if (existingAccounts != null && existingAccounts.Count > 0)
        {
            address = existingAccounts[0].GetAddress();
        }
    }

    // display the account type options:
    Console.WriteLine("Select account type:");
    Console.WriteLine("1. Savings");
    Console.WriteLine("2. Checking");
    Console.WriteLine("3. CD");
    Console.Write("Choose an option: ");
    string typeChoice = Console.ReadLine();

    decimal minBalance = 100m; // default minimum balance
    if (typeChoice == "3") // CD Account
    {
        minBalance = 500m;
        Console.WriteLine($"Note: CD Accounts require a minimum balance of {minBalance:C}");
    }

    Console.Write($"Enter initial balance (minimum {minBalance:C}): ");
    string balanceInput = Console.ReadLine().Trim();
    
    if (balanceInput.StartsWith("$"))
    {
        balanceInput = balanceInput.Substring(1);
    }

    if (!decimal.TryParse(balanceInput, out balance) || balance < minBalance)    {
        Console.WriteLine("Invalid balance.");
        Console.WriteLine();
        return;
    }

    IAccount account;
    try
    {
        // create the account based on user choice:
        account = typeChoice switch
        {
            "1" => new SavingsAccount(name, address, balance),
            "2" => new CheckingAccount(name, address, balance),
            "3" => new CDAccount(name, address, balance),
            _ => throw new InvalidOperationException("Invalid account type")
        };
    }
    catch (ArgumentException ex)
    {
        // if account creation fails because of invalid parameters, display error message:
        Console.WriteLine();
        Console.WriteLine($"Failed to create account. {ex.Message}");
        Console.WriteLine();
        return;
    }

    // store the new account:
    if (manager.StoreAccount(account))
    {
        // if it is successfully stored, display success message and account info:
        Console.WriteLine();
        Console.WriteLine("Account created successfully:");
        DisplayAccountInfo(account);
        Console.WriteLine();
    }
    else
    {
        // if it fails to store, display fail message:
        Console.WriteLine();
        Console.WriteLine("Failed to store account.");
        Console.WriteLine();
    }
}

    // searches for an account by name and displays its information:
    private static void SearchAccount(AccountManager manager)
    {
        // prompt for the name to search:
        Console.WriteLine();
        Console.Write("Enter name to search: ");
        string name = Console.ReadLine();

        // try and find the account:
        List<IAccount> accounts = manager.FindAccounts(name);
        if (accounts != null && accounts.Count > 0)
        {
            //if found, display account information:
            Console.WriteLine();
            Console.WriteLine($"Found {accounts.Count} account(s) for {name}:");
            Console.WriteLine();
            for (int i = 0; i < accounts.Count; i++)
            {
                Console.WriteLine($"Account {i + 1}:");
                DisplayAccountInfo(accounts[i]);
                Console.WriteLine();
            }

            // if there are multiple accounts, prompt user to select one:
            IAccount selectedAccount;
            if (accounts.Count > 1)
            {
                Console.Write("Select an account number to operate on: ");
                if (int.TryParse(Console.ReadLine(), out int selection) && selection > 0 && selection <= accounts.Count)
                {
                    selectedAccount = accounts[selection - 1];
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("Invalid selection. Returning to main menu.");
                    Console.WriteLine();
                    return;
                }
            }
            else
            {
                selectedAccount = accounts[0];
                Console.WriteLine();
            }

            // prompt for deposit or withdrawal:
            while (true)
            {
                Console.WriteLine("1. Deposit");
                Console.WriteLine("2. Withdraw");
                Console.WriteLine("3. Return to main menu");
                Console.Write("Choose an option: ");
                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        DepositToAccount(selectedAccount);
                        break;
                    case "2":
                        WithdrawFromAccount(selectedAccount);
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        Console.WriteLine();
                        break;
                }

                // Display updated account info after each transaction
                Console.WriteLine();
                DisplayAccountInfo(selectedAccount);
                Console.WriteLine();
            }
        }
        else
        {
            // if it is not found, display an error message:
            Console.WriteLine();
            Console.WriteLine("Account not found.");
            Console.WriteLine();
        }
    }
    
    // deposit funds into account:
    private static void DepositToAccount(IAccount account)
    {
        // prompt for deposit amount:
        Console.Write("Enter deposit amount: $");
        if (!decimal.TryParse(Console.ReadLine(), out decimal amount) || amount <= 0)
        {
            // invalid input handling:
            Console.WriteLine();
            Console.WriteLine("Invalid amount.");
            Console.WriteLine();
            return;
        }

        // perform the deposit using the PayInFunds method:
        account.PayInFunds(amount);
        Console.WriteLine($"Deposit successful. New balance: {account.GetBalance():C}");
        Console.WriteLine();
    }

    // withdraw frunds from account:
    private static void WithdrawFromAccount(IAccount account)
    {
        // prompt for withdrawal amount:
        Console.Write("Enter withdrawal amount: $");
        if (!decimal.TryParse(Console.ReadLine(), out decimal amount) || amount <= 0)
        {
            // invalid input handling:
            Console.WriteLine();
            Console.WriteLine("Invalid amount.");
            Console.WriteLine();
            return;
        }

        // attempt to withdraw funds:
        if (account.WithdrawFunds(amount))
        {
            // withdrawal successful:
            Console.WriteLine();
            Console.WriteLine($"Withdrawal successful. New balance: {account.GetBalance():C}");
        }
        else
        {
            // withdrawal failed:
            Console.WriteLine();
            Console.WriteLine("Withdrawal failed. Insufficient funds.");
        }
    }

    // display the information of an account:
    private static void DisplayAccountInfo(IAccount account)
    {
        Console.WriteLine($"Name: {account.GetName()}");
        Console.WriteLine($"Address: {account.GetAddress()}");
        Console.WriteLine($"Account Number: {account.GetAccountNumber()}");
        Console.WriteLine($"Balance: {account.GetBalance():C}");
        Console.WriteLine($"Service Fee: {account.GetServiceFee():C}");
        Console.WriteLine($"Account Type: {account.GetType().Name}");
    }
}