using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using LAB_REPOS.MEJORES_5.RSA;

namespace LAB_REPOS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CifradoRSAController : ControllerBase
    {
        // GET: api/CifradoRSA
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/CifradoRSA/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/CifradoRSA
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }


    }
}
