using TieredBankAPI.Data;
using TieredBankAPI.Models;

namespace TieredBankAPI.DAL
{
    public class AccountRepository   // 账户存储库
    {
        private TieredBankAPIContext _db;
        public AccountRepository(TieredBankAPIContext context)
        {
            _db = context;
        }

        // Basic CRUD
        // GET
        public ICollection<Account> GetAccounts()
        {
            return _db.Account.ToList();
        }

        public Account GetAccountById(int id)
        {
            return _db.Account.Find(id);
        }
        public List<Account> GetAccountByCustomerId(int customerId)
        {
            return _db.Account.Where(a => a.CustomerId == customerId).ToList();
        }

        public void InsertAccount(Account account)
        {
            _db.Account.Add(account);
        }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
