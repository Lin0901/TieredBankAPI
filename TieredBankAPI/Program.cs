using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using System.Reflection.Metadata.Ecma335;
using TieredBankAPI.Data;
using TieredBankAPI.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TieredBankAPIContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TieredBankAPIContext") ?? throw new InvalidOperationException("Connection string 'TieredBankAPIContext' not found.")));



var app = builder.Build();


// async Òì²½±à³Ì Delegate Î¯ÍÐ

app.MapGet("/accounts", async (TieredBankAPIContext db) =>
{
    return await db.Account.ToListAsync();
});


app.MapGet("/customers", async (TieredBankAPIContext db) =>
{
    return await db.Customer.ToListAsync();
});


app.MapGet("/customers/{id}/total", async (TieredBankAPIContext db, string id) =>
{
    Customer customer = await db.Customer.FindAsync(id);

    if(customer != null)
    {
        List<Account> accounts = await db.Account.Where(a => a.CustomerId ==
        customer.Id).ToListAsync();

        decimal sum = accounts.Sum(a => a.Balance);

        var response = JsonSerializer.Serialize(new
        {
            Sum = sum
        });

        return Results.Ok(response);
    }

    return Results.NotFound();


});




app.Run();

