using Project.DAL.Context;
using Project.Entity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Repositories.CategoryRepository
{
    //Neden "BaseRepository<Category>" ı da ilave ettik? Çünkü "BaseRepository" içindeki tüm fonksiyonların "Category" özelinde uygulanarak gelmesi için yaptık. "ICategoryRepository" ekleme nedenimiz ise concrete nesnenin abstract yapılanmasıdır. Dependency injectiondan (IOC Containerdan isterken) "CategoryRepository" talep ederken "ICategoryRepository" ile talep edeceğiz.
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        //"BaseRepository" constructorına bir değer vermemiz gerekiyor. IOC containerden "ECommerCeAPIDbContext" context adıyla temin edilir ve base'e yani "BaseRepository" constructorına bu gönderilir.
        public CategoryRepository(ProjectContext context) : base(context)
        {

        }
    }
}
