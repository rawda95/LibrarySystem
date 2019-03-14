using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryProject.Models;

namespace LibraryProject.IRepository
{
    public interface ICategoryRepository
    {
        List<Category> GetAll();
        bool Delete(Category category);
        Category GetById(int? id);
        Category Add(Category category);
        Category Update(Category category);
        bool checkCategory(Category category);
    }
}
