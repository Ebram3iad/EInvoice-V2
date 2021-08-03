using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EInvoiceInfrastructure.EFRepository
{
    public interface IRepository<T>where T :class
    {
        Task<IEnumerable<T>> GetAll();
        Task Create(T model);
        Task Create(List<T> model);
        Task Update( T model);
        Task Delete(T id);
        Task Save();
    }
}
