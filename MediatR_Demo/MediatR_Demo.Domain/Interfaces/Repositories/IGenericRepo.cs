using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediatR_Demo.Domain.Interfaces.Repositories
{
    public interface IGenericRepo<TEntity> where TEntity : class
    {
        Task<ICollection<TEntity>> GetAllAsync();
        Task<TEntity?> GetByIdAsync(Guid id);
        Task AddAsync(TEntity entity);
        Task SaveAsync();

        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}
