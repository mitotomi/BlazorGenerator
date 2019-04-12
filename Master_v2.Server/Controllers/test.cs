using Master_v2.Server.Models;
using Master_v2.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Master_v2.Server.Controllers
{
    public class test:Controller
    {
        OrderMasterContext _context = new OrderMasterContext();

        public void update(Person person)
        {
            _context.Person.Update(person);
            _context.SaveChanges(); 
        }
    }
}
