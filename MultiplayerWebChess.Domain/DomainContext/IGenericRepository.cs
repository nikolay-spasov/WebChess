using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerWebChess.Domain.DomainContext
{
    public interface IGenericRepository<TEntity>
    {
        IQueryable<TEntity> Get();

        void Insert(TEntity entity);

        TEntity GetById(object id);

        void Delete(object id);

        void Delete(TEntity entityToDelete);

        void Update(TEntity entityToUpdate);
    }
}
