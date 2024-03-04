using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymProject.Infrastructure.Data.Interfaces
{
    public interface IRepository<TEntity>
    {
        Task <TEntity> GetById(int id);
        Task Add(TEntity entity);
        Task Update(TEntity entity);
        Task Delete(TEntity entity);
        Task< IEnumerable<TEntity>> GetAll();

    }
}
