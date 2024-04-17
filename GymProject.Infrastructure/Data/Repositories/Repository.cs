using GymProject.Infrastructure.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GymProject.Infrastructure.Data.Repositories
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext _context;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        public virtual async Task <TEntity> GetById(int id)
        {
           var entity= await _context.Set<TEntity>().FindAsync(id);
            if (entity==null)
            {
                throw new Exception($"Entity with ${id} is not found");
            }
            return entity;
        }

        public virtual async Task Add(TEntity entity)
        {
             await _context.Set<TEntity>().AddAsync(entity);
             await _context.SaveChangesAsync();
        }

        public virtual async Task Update(TEntity entity)
        {
           
            _context.Set<TEntity>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public virtual async Task Delete(TEntity entity)
        {
            if (entity is IDeletable deletableEntity)
            {
                deletableEntity.IsDeleted = true;
                deletableEntity.DeleteTime = DateTime.Now;
            }
            else
            {
                throw new InvalidOperationException("Entity does not support soft delete.");
            }

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllNotDeleted()
        {
            var isDeletedProperty = typeof(TEntity).GetProperty("IsDeleted");
            if (isDeletedProperty == null || isDeletedProperty.PropertyType != typeof(bool))
            {
                throw new InvalidOperationException($"Entity {typeof(TEntity).Name} does not contain a boolean property named IsDeleted.");
            }

            var entities = await _context.Set<TEntity>().ToListAsync();

            return entities.Where(e => isDeletedProperty.GetValue(e) is bool isDeleted && !isDeleted);
        }
    }
}
