using Microsoft.EntityFrameworkCore;
using Project.DAL.Context;
using Project.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly ProjectContext _context;
        private readonly DbSet<T> _entities;

        public BaseRepository(ProjectContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public IQueryable<T> GetAll()
        {
            return _entities.Where(x=> x.IsActive==true);
        }

        public T GetById(int id)
        {
            return _entities.Find(id);
        }

        public void Insert(T entity)
        {
            if (entity != null)
            {
                _entities.Add(entity);
                SaveChanges("Veri Kaydedildi!");
            }
        }

        public void Remove(T entity)
        {
            if (entity != null)
            {
                entity.DeletedDate = DateTime.Now;
                entity.Status = Project.Entity.Enum.DataStatus.Deleted;
                entity.IsActive = false;
                _context.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                //_entities.Remove(entity);
                SaveChanges("Veri Silindi!");
            }
        }

        public string SaveChanges(string message)
        {
            try
            {
                _context.SaveChanges();
                return message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public void Update(T entity)
        {
            if (entity != null)
            {
                entity.ModifiedDate = DateTime.UtcNow;
                entity.Status = Project.Entity.Enum.DataStatus.Updated;
                entity.IsActive = true;
                _context.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                SaveChanges("Veri Güncellendi!");
            }

        }
    }

}
