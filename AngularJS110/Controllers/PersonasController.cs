using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AngularJS109.Models;

namespace AngularJS109.Controllers
{
    public class PersonasController : ApiController
    {
        [HttpGet]
        [ActionName("obtenerPersonas")]
        public List<Persona> obtenerPersonas()
        {
            List<Persona> lstPersona = new List<Persona>();
            for (int i = 0; i < 10; i++)
            {
                Persona p = new Persona();
                p.name = "Persona " + i;
                p.isActive = (i % 2 == 1);
                p.guid = Guid.NewGuid() + "";
                lstPersona.Add(p);
            }
            return lstPersona;
        }
    }
}
