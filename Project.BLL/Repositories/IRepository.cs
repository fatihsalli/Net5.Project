using Project.Entity.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Repositories
{
    public interface IRepository<T> where T : BaseEntity
    {
        //List
        IQueryable<T> GetAll();
        //Create
        void Insert(T entity);
        //Delete
        void Remove(T entity);
        //Update
        void Update(T entity);
        //Get
        T GetById(int id);
        //SaveChanges
        string SaveChanges(string message);


    }
}
