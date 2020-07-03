using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AptechSem3.Service.ModelService
{
    interface IService<T>
    {
        bool create(T t);

        T findById(String id);

        List<T> findAll();

        bool update(T t);

        bool deleteById(String id);
    }
}