using LibraryProject.Identity;
using LibraryProject.IRepository;
using LibraryProject.Models;
using System.Collections.Generic;
using System.Linq;

namespace LibraryProject.Repository
{
    public class PublisherRepository : IPublisherRepository
    {
        ApplicationDbContext db;
        public PublisherRepository()
        {
            db = new ApplicationDbContext();
        }
        public Publisher Add(Publisher publisher)
        {
            if (!checkCategory(publisher))
            {
                this.db.Publishers.Add(publisher);
                this.db.SaveChanges();
                return publisher;
            }
            else
            {
                return null;
            }
        }

        public bool checkCategory(Publisher publisher)
        {
            return GetAll().Contains(publisher);
        }

        public bool Delete(Publisher publisher)
        {
            db.Publishers.Remove(publisher);
            db.SaveChanges();
            return true;
        }

        public List<Publisher> GetAll()
        {
            return db.Publishers.ToList();
        }

        public Publisher GetById(int? id)
        {
            Publisher publisher = db.Publishers.Find(id);

            return publisher;
        }

        public Publisher Update(Publisher publisher)
        {
            if (!checkCategory(publisher))
            {

                var publisherOld = db.Publishers.Find(publisher.id);
                publisherOld.name = publisher.name;
                //db.Entry(author).State = EntityState.Modified;
                db.SaveChanges();
                return publisherOld;
            }
            else
            {
                return null;
            }
        }
    }
}
