using LibraryProject.Identity;
using LibraryProject.IRepository;
using LibraryProject.Models;
using System.Collections.Generic;
using System.Linq;

namespace LibraryProject.Repository
{
    public class AuthorRepository : IAuthorRepository
    {
        ApplicationDbContext db;
        public AuthorRepository()
        {
            db = new ApplicationDbContext();
        }


        public Author Add(Author author)
        {
            if (!checkCategory(author))
            {
                this.db.Authors.Add(author);
                this.db.SaveChanges();
                return author;
            }
            else
            {
                return null;
            }
        }

        public bool checkCategory(Author author)
        {
            return GetAll().Contains(author);
        }

        public bool Delete(Author author)
        {
            db.Authors.Remove(author);
            db.SaveChanges();
            return true;
        }

        public List<Author> GetAll()
        {
            return db.Authors.ToList();
        }

        public Author GetById(int? id)
        {
            Author author = db.Authors.Find(id);

            return author;
        }

        public Author Update(Author author)
        {
            if (!checkCategory(author))
            {
                var authorOld = db.Authors.Find(author.id);
                authorOld.name = author.name;
                //db.Entry(author).State = EntityState.Modified;
                db.SaveChanges();
                return authorOld;
            }
            else
            {
                return null;
            }
        }
    }
}
