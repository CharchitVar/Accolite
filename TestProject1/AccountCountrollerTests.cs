using BankingApi.Controllers;
using BankingAPI.Controllers;
using BankingAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Xunit;

public class AccountsControllerTests
{
	private readonly AccountsController _controller;
	private readonly UsersController _usersController;

	public AccountsControllerTests()
	{
		_controller = new AccountsController();
		_usersController = new UsersController();
	}

	[Fact]
	public void CreateAccount_ShouldReturnAccount()
	{
		var user = new User { Name = "Test User" };
		var createdUser = (_usersController.CreateUser(user) as OkObjectResult).Value as User;
		var account = new Account { UserId = createdUser.Id };
		var result = _controller.CreateAccount(account);

		var okResult = Assert.IsType<OkObjectResult>(result);
		var createdAccount = Assert.IsType<Account>(okResult.Value);

		Assert.NotNull(createdAccount);
		Assert.Equal(createdUser.Id, createdAccount.UserId);
	}

	[Fact]
	public void DeleteAccount_ShouldRemoveAccount()
	{
		var user = new User { Name = "Test User" };
		var createdUser = (_usersController.CreateUser(user) as OkObjectResult).Value as User;
		var account = new Account { UserId = createdUser.Id };
		var createdAccount = (_controller.CreateAccount(account) as OkObjectResult).Value as Account;

		_controller.DeleteAccount(createdAccount.Id);

		var result = _controller.GetAccountById(createdAccount.Id);

		Assert.IsType<NotFoundResult>(result);
	}

	[Fact]
	public void Deposit_ShouldIncreaseBalance()
	{
		var user = new User { Name = "Test User" };
		var createdUser = (_usersController.CreateUser(user) as OkObjectResult).Value as User;
		var account = new Account { UserId = createdUser.Id };
		var createdAccount = (_controller.CreateAccount(account) as OkObjectResult).Value as Account;

		_controller.Deposit(createdAccount.Id, 1000);

		var updatedAccount = (_controller.GetAccountById(createdAccount.Id) as OkObjectResult).Value as Account;

		Assert.Equal(1100, updatedAccount.Balance);
	}

	[Fact]
	public void Withdraw_ShouldDecreaseBalance()
	{
		var user = new User { Name = "Test User" };
		var createdUser = (_usersController.CreateUser(user) as OkObjectResult).Value as User;
		var account = new Account { UserId = createdUser.Id, Balance = 1000 };
		var createdAccount = (_controller.CreateAccount(account) as OkObjectResult).Value as Account;

		_controller.Withdraw(createdAccount.Id, 500);

		var updatedAccount = (_controller.GetAccountById(createdAccount.Id) as OkObjectResult).Value as Account;

		Assert.Equal(500, updatedAccount.Balance);
	}
}
