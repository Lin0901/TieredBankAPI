using TieredBankAPI.Data;
using TieredBankAPI.Models;

namespace TieredBankAPI.DAL
{
    public class CustomerRepository  //客户资源库
    {
        private TieredBankAPIContext _db; // 先建立这个 然后等点击using

        public CustomerRepository(TieredBankAPIContext db) //名字 括号里面是上面，让上面等于赋值的下面
        {
            _db = db;   // 把 db 得值，赋给了 _db
        }

        public ICollection<Customer> GetCustomers()
        {

            return _db.Customer.ToList();
        }
        public Customer GetCustomerById(int id)
        {
            return _db.Customer.Find(id);
        }
        public void CreateCustomer(Customer customer)
        {
            _db.Customer.Add(customer);
        }
        public Customer UpdateCustomer(Customer customer)
        {
            _db.Customer.Update(customer);
            return customer;
        }

    }
}
