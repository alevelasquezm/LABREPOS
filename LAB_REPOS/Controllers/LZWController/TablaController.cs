using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using LAB_REPOS.MEJORES_5.LZW;
using LAB_REPOS.Controllers;

namespace LAB_REPOS.Controllers.LZWController
{
    [Route("api/[controller]")]
    [ApiController]
    public class TablaController : ControllerBase
    {
        // GET: api/Tabla
        [HttpGet]
        public IEnumerable<TablaComprimir> Get()
        {
            return CheckExistence.Exist.tabla;
        }

        // GET: api/Tabla/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Tabla
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Tabla/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
