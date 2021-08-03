using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EInvoiceInfrastructure.EFRepository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        #region Fields
        private readonly DBContext _context;
        private DbSet<T> _entities;
        #endregion

        #region Constructor
        public Repository(DBContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }
        #endregion

        #region Methods
        public async Task Create(T model)
        {
            try
            {
                if (model == null)
                    throw new ArgumentNullException("model");

                _entities.Add(model);
            }
            catch (Exception)
            {
                throw;
            }
        }  
        public async Task Create(List<T> model)
        {
            try
            {
                if (model == null)
                    throw new ArgumentNullException("model");

                _entities.AddRange(model);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Delete(T id)
        {
            _entities.Remove(id);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _entities.AsNoTracking().ToListAsync();
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public async Task Update( T model)
        {
            _entities.Attach(model);
            _context.Entry(model).State = EntityState.Modified;
            //_entities.Update(model);
        }
        #endregion

    }
}
