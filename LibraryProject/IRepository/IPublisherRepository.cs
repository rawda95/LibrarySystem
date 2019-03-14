using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryProject.Models;
namespace LibraryProject.IRepository
{
   public interface IPublisherRepository
    {
        List<Publisher> GetAll();
        bool Delete(Publisher publisher);
        Publisher GetById(int? id);
        Publisher Add(Publisher publisher);
        Publisher Update(Publisher publisher);
        bool checkCategory(Publisher publisher);
    }
}
