using LibraryProject.Identity;
using LibraryProject.IRepository;
using LibraryProject.Models;
using System.Collections.Generic;
using System.Linq;

namespace LibraryProject.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        ApplicationDbContext db;
        public CategoryRepository()
        {
            db = new ApplicationDbContext();
        }
        public Category Add(Category category)
        {
            if (!checkCategory(category))
            {
                this.db.Categories.Add(category);
                this.db.SaveChanges();
                return category;
            }
            else
            {
                return null;
            }


        }

        public bool checkCategory(Category category)
        {
            return GetAll().Contains(category);
            //  throw new NotImplementedException();
        }

        public bool Delete(Category category)
        {
            db.Categories.Remove(category);
            db.SaveChanges();
            return true;
        }



        public List<Category> GetAll()
        {
            return db.Categories.ToList();
        }

        public Category GetById(int? id)
        {
            Category category = db.Categories.Find(id);

            return category;
        }

        public Category Update(Category category)
        {
            if (!checkCategory(category))
            {
                var categoryOld = db.Categories.Find(category.id);
                categoryOld.name = category.name;
                //db.Entry(author).State = EntityState.Modified;
                db.SaveChanges();
                return categoryOld;
            }
            else
            {
                return null;
            }
        }
    }
}
