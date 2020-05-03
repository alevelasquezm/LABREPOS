using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LAB_REPOS.MEJORES_5.ARBOL_B;

namespace LAB_REPOS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArbolBController : ControllerBase
    {
        public BTree tree = new BTree();
        public Path path = new Path();
        public BSearch search = new BSearch();
        // GET: api/ArbolB
        [HttpGet]
        public string Get()
        {
            var content = "Ingresar" + path.InOrder();
            return content;
        }

        // GET: api/ArbolB/5
        [Route("SearchItem")]
        [HttpGet]
        public string Get([FromBody] string brandNew)
        {
            string found;
            var searchedItem = search.Search(brandNew);

            if (searchedItem != null)
            {
                found = "Name: " + searchedItem.Name + "\n" + "Flavor: " + searchedItem.Flavor + "\n" + "Volume: " + searchedItem.Volume + "\n" + "Price: " + searchedItem.Price + "Producer House: " + searchedItem.Producer_House;
            }
            else
            {
                found = "ERROR, NOT FOUND";
            }
            return found;
        }

        // POST: api/ArbolB
        [HttpPost]
        public void Post([FromBody] Soda item)
        {
            tree.insert(item);
        }

    }
}
