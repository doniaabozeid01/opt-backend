using Microsoft.EntityFrameworkCore;
using optimum.data.Context;
using optimum.data.Entities;
using optimum.repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace optimum.repository.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly OptimumDbContext _context;

        public GenericRepository(OptimumDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public void Update(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
        }

        public async Task<IReadOnlyList<TEntity>> GetAllAsync()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public void Delete(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }





        public async Task<School> AddAsync(School school, CancellationToken cancellationToken = default)
        {
            if (school == null) throw new ArgumentNullException(nameof(school));

            _context.School.Add(school);
            await _context.SaveChangesAsync(cancellationToken);
            return school;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var existing = await _context.School.FindAsync(new object[] { id }, cancellationToken);
            if (existing == null) return false;

            _context.School.Remove(existing);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<School> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.School.FindAsync(new object[] { id }, cancellationToken);
        }



    }

}
