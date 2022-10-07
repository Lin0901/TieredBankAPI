using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using System.Reflection.Metadata.Ecma335;
using TieredBankAPI.Data;
using TieredBankAPI.Models;
using TieredBankAPI.DAL;
using TieredBankAPI.BLL;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TieredBankAPIContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TieredBankAPIContext") ?? throw new InvalidOperationException("Connection string 'TieredBankAPIContext' not found.")));



var app = builder.Build();


// async Òì²½±à³Ì Delegate Î¯ÍÐ

app.MapGet("/accounts", async (TieredBankAPIContext db) =>
{
    AccountRepository repo = new AccountRepository(db);
    return repo.GetAccounts();

    //return await db.Account.ToListAsync();
});

app.MapGet("/accounts/{id}", (string id, TieredBankAPIContext db) =>
{
    AccountRepository repo = new AccountRepository(db);

    try
    {
        return repo.GetAccountById(int.Parse(id));
    }
    catch(Exception e)
    {
        return null;
    }

});

app.MapGet("/accounts/{id}/balance", (string id, TieredBankAPIContext db) =>
{
    AccountBusinessLogic accountBll = new AccountBusinessLogic(new AccountRepository(db));
    try
    {
        decimal balance = accountBll.GetBalance(int.Parse(id));
        return Results.Ok(balance);
    }
    catch(Exception ex)
    {
        return Results.Problem(ex.Message);
    }

});

app.MapGet("/customers", async (TieredBankAPIContext db) =>
{
    CustomerRepository repo = new CustomerRepository(db);
    return repo.GetCustomers();

    //return await db.Customer.ToListAsync();
});

app.MapGet("/customers/{id}/total", async (TieredBankAPIContext db, string id) =>
{
    CustomerRepository customerRepository = new CustomerRepository(db);
    AccountRepository accountRepository = new AccountRepository(db);

    try
    {
        Customer customer = customerRepository.GetCustomerById(int.Parse(id));
        //Customer customer = await db.Customer.FindAsync(id);

        List<Account> accounts = accountRepository.GetAccountByCustomerId(customer.Id);

        //await db.Account.Where(a => a.CustomerId == customer.Id).ToListAsync();

        decimal sum = accounts.Sum(a => a.Balance);

            var response = JsonSerializer.Serialize(new
            {
                Sum = sum
            });

            return Results.Ok(response);
       
    }
    catch
    {
        return Results.NotFound();
    }


});




app.Run();

