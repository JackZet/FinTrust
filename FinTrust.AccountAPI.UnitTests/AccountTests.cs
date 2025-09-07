using FinTrust.AccountAPI.Domain;
using FinTrust.AccountAPI.Domain.Entities;

namespace FinTrust.AccountAPI.UnitTests;

public class AccountTests
{
    [Fact]
    public void Account_Should_AllowAccessIfUserIsMemberAndHasViewerRole()
    {
        
        var user = new User
        {
            Id = Guid.NewGuid(),
            UserOid = "user-1",
        };

        var memberRoles = new List<string>
        {
            Roles.AccountViewer
        };

        var account = new Account
        {
            AccountId = "account-1",
            Members = [user],
            Balance = 100 
        };
       
        
        Assert.True(account.UserCanReadAccount(user, memberRoles));
    }
    
    [Fact]
    public void Account_Should_DenyAccessIfUserIsNotMemberAndHasViewerRole()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            UserOid = "user-1",
        };

        var memberRoles = new List<string>
        {
            Roles.AccountViewer
        };

        var account = new Account
        {
            AccountId = "account-1",
            Members = [],
            Balance = 100 
        };
       
        Assert.False(account.UserCanReadAccount(user, memberRoles));
    }
    
    [Fact]
    public void Account_Should_AllowAccessIfUserIsAdminAndNotMember()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            UserOid = "user-1",
        };

        var memberRoles = new List<string>
        {
            Roles.AccountAdmin
        };

        var account = new Account
        {
            AccountId = "account-1",
            Members = [],
            Balance = 100 
        };
       
        Assert.True(account.UserCanReadAccount(user, memberRoles));
    }
    
    [Fact]
    public void Account_Should_DenyAccessIfUserIsMemberAndHasNoRoles()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            UserOid = "user-1",
        };

        var memberRoles = new List<string>();

        var account = new Account
        {
            AccountId = "account-1",
            Members = [user],
            Balance = 100 
        };
       
        Assert.False(account.UserCanReadAccount(user, memberRoles));
    }
}
