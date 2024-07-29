// Project Prologue
// Name: Carter Quesenberry
// Date: 7/13/2024
// Purpose: Lab #04 Banking Program - Part 3
// 
// I declare that the following code was written by me or provided 
// by the instructor for this project. I understand that copying source 
// code from any other source constitutes plagiarism, and that I will receive 
// a zero on this project if I am found in violation of this policy.

using System.Collections.Generic;

// manage the storage and retrieval of bank accounts:
public class AccountManager
{
    private Dictionary<string, List<IAccount>> accountsByName = new Dictionary<string, List<IAccount>>();

    // stores an account in the manager:
    public bool StoreAccount(IAccount account)
    {
        if (account == null || string.IsNullOrEmpty(account.GetName()))
            return false;

        if (!accountsByName.ContainsKey(account.GetName()))
        {
            accountsByName[account.GetName()] = new List<IAccount>();
        }
        accountsByName[account.GetName()].Add(account);
        return true;
    }

    // finds an account by the account number:
    public List<IAccount> FindAccounts(string name)
    {
        if (accountsByName.TryGetValue(name, out List<IAccount> accounts))
            return accounts;
        return null;
    }

    public bool HasExistingAccount(string name)
    {
        return accountsByName.ContainsKey(name);
    }
}