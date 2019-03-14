using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryProject.Models;

namespace LibraryProject.IRepository
{
    public interface IAuthorRepository
    {
        List<Author> GetAll();
        bool Delete(Author author);
        Author GetById(int? id);
        Author Add(Author author);
        Author Update(Author author);
        bool checkCategory(Author author);
    }
}
