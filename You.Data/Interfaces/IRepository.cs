using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace You.Data
{
    public interface IRepository<T>:IRepositoryAsync<T>,IRepositorySync<T> where T:class
    {
        int pageCount { get; set; }
    }
}
