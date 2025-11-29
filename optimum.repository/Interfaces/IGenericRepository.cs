using optimum.data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimum.repository.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        Task AddAsync(TEntity entity);
        Task<TEntity> GetByIdAsync(int id);

        void Update(TEntity entity);

        Task<IReadOnlyList<TEntity>> GetAllAsync();

        void Delete(TEntity entity);


        Task<School> AddAsync(School school, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<School> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    }

}
