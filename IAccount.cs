// Project Prologue
// Name: Carter Quesenberry
// Date: 7/13/2024
// Purpose: Lab #04 Banking Program - Part 3
// 
// I declare that the following code was written by me or provided 
// by the instructor for this project. I understand that copying source 
// code from any other source constitutes plagiarism, and that I will receive 
// a zero on this project if I am found in violation of this policy.

public interface IAccount
{  
    // functions to set and get the account holders name:
    bool SetName(string inName);
    string GetName();

    // functions to set and get the account holders address:
    bool SetAddress(string inAddress);
    string GetAddress();

    // functions for account transactions:
    void PayInFunds(decimal amount);
    bool WithdrawFunds(decimal amount);

    //functions for setting and getting the account balance:
    bool SetBalance(decimal inBalance);
    decimal GetBalance();

    //functions for setting and getting account state:
    void SetAccountState(Account.AccountState state);
    Account.AccountState GetAccountState();

    //function getting the account number:
    string GetAccountNumber();

    // function for getting and setting the service fee for the account:
    bool SetServiceFee(decimal fee);
    decimal GetServiceFee();
}