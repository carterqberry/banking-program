// Project Prologue
// Name: Carter Quesenberry
// Date: 7/13/2024
// Purpose: Lab #04 Banking Program - Part 3
// 
// I declare that the following code was written by me or provided 
// by the instructor for this project. I understand that copying source 
// code from any other source constitutes plagiarism, and that I will receive 
// a zero on this project if I am found in violation of this policy.

// abstract base class for all account types:
public abstract class Account : IAccount
{
    // enum for the possible account states:
    public enum AccountState
    {
        New,
        Active,
        UnderAudit,
        Frozen,
        Closed
    }

    // enum for the possible account types:
    public enum AccountType
    {
        Savings,
        Checking,
        CD
    }

    // min balance for account. can be overriden:
    protected virtual decimal MinimumBalance => 100m;

    private string name;
    private string address;
    protected string accountNumber;
    private decimal balance;
    private AccountState state;
    protected decimal serviceFee;
    protected AccountType accountType;

    // constructor for creating an account:
    protected Account(string name, string address, decimal balance, AccountType type)
    {
        if (!SetName(name) || !SetAddress(address) || !SetBalance(balance))
        {
            throw new ArgumentException("Invalid account parameters");
        }
        accountType = type;
        GenerateAccountNumber();
    }

    // default constructor:
    public Account() : this("", "", 100m, AccountType.Savings) { }

    // set the name of the account holder:
    public bool SetName(string inName)
    {
        if (string.IsNullOrEmpty(inName))
        {
            return false;
        }
        name = inName;
        return true;
    }

    // get the name of the account holder:
    public string GetName()
    {
        return name;
    }

    // Set the address of the account holder:
     public bool SetAddress(string inAddress)
    {
        if (string.IsNullOrEmpty(inAddress))
        {
            return false;
        }
        address = inAddress;
        return true;
    }

    // get the address of the account holder:
    public string GetAddress()
    {
        return address;
    }

    // deposit funds into the account:
    public void PayInFunds(decimal amount)
    {
        balance += amount;
    }

    // withdraw funds from the account:
    public bool WithdrawFunds(decimal amount)
    {
        if (balance - amount < 0)
        {
            return false;
        }
        balance -= amount;
        return true;
    }

    // set the balance of the account:
    public virtual bool SetBalance(decimal inBalance)
    {
        if (inBalance < MinimumBalance)
        {
            throw new ArgumentException($"Initial balance must be at least {MinimumBalance:C} for this account type.");

        }
        balance = inBalance;
        return true;
    }

    // get the current balance of the account.
    public decimal GetBalance()
    {
        return balance;
    }

    // set the state of the account.
    public void SetAccountState(AccountState state)
    {
        this.state = state;
    }

    // get the current state of the account.
    public AccountState GetAccountState()
    {
        return state;
    }

    // get the account number:
    public string GetAccountNumber()
    {
        return accountNumber;
    }

    // set the service fee for the account:
    public virtual bool SetServiceFee(decimal fee)
    {
        if (fee < 0) return false;
        serviceFee = fee;
        return true;
    }

    // get the current service fee for the account.
    public virtual decimal GetServiceFee()
    {
        return serviceFee;
    }

    // generates a unique account number.
    protected void GenerateAccountNumber()
    {
        Random rand = new Random();
        string number = rand.Next(100000, 999999).ToString();
        char typeIndicator = accountType switch
        {
            AccountType.Savings => 'S',
            AccountType.Checking => 'C',
            AccountType.CD => 'D',
            _ => throw new InvalidOperationException("Invalid account type")
        };
        accountNumber = $"{number}{typeIndicator}";
    }
}

public class SavingsAccount : Account
{
    // constructor for creating a savings account:
    public SavingsAccount(string name, string address, decimal balance)
        : base(name, address, balance, AccountType.Savings) { }

    // sets the service fee for the savings account (always 0).
    public override bool SetServiceFee(decimal fee)
    {
        return base.SetServiceFee(0);
    }
}

public class CheckingAccount : Account
{
    private const decimal DefaultFee = 5.00m;

    // constructor for creating a checking account:
    public CheckingAccount(string name, string address, decimal balance)
        : base(name, address, balance, AccountType.Checking)
    {
        SetServiceFee(DefaultFee);
    }

    // gets the minimum balance for a checking account:
    protected override decimal MinimumBalance => 10m;

    // sets the service fee for the checking account:
    public override bool SetServiceFee(decimal fee)
    {
        return base.SetServiceFee(Math.Max(fee, DefaultFee));
    }
}

public class CDAccount : Account
{
    private const decimal DefaultFee = 8.00m;

    // constructor for creating a CD account:
    public CDAccount(string name, string address, decimal balance)
        : base(name, address, balance, AccountType.CD)
    {
        SetServiceFee(DefaultFee);
    }

    // gets the minimum balance for a CD account:
    protected override decimal MinimumBalance => 500m;

    // sets the service fee for the CD account:
    public override bool SetServiceFee(decimal fee)
    {
        return base.SetServiceFee(Math.Max(fee, DefaultFee));
    }
}