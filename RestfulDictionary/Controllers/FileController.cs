using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestfulDictionary.Manager;
using RestfulDictionary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestfulDictionary.Controllers {
    [ApiController]
    [Route("files")]
    public class FileController : ControllerBase {
        // GET @root/files
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpGet]
        public ActionResult<IEnumerable<FileEndPoint>> GetAll() {
            List<FileEndPoint> re = FileManager.GetAll();
            if (re.Count > 0) {
                return Ok(re);
            }

            return NoContent();
        }

        // GET @root/files/byname
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Route("byname")]
        [HttpGet]
        public ActionResult<IEnumerable<FileEndPoint>> Get([FromQuery] string filename) {
            List<FileEndPoint> re = FileManager.Get(filename);
            if (re.Count > 0) {
                return Ok(re);
            }

            return NoContent();
        }

        // GET @root/files/bypeer
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Route("bypeer")]
        [HttpGet]
        public ActionResult<IEnumerable<FileEndPoint>> Get([FromQuery] string ipv4, [FromQuery] string ipv6, [FromQuery] int port) {
            List<FileEndPoint> re = FileManager.Get(PeerManager.Get(ipv4, ipv6, port));
            if (re.Count > 0) {
                return Ok(re);
            }

            return NoContent();
        }

        // POST @root/files
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [HttpPost]
        public IActionResult Post([FromBody] FileEndPoint file) {
            try {
                FileManager.Add(file);
                return StatusCode(201);
            } catch (Exception e) {
                return StatusCode(406, e.ToString());
            }
        }

        // DELETE @root/files
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpDelete]
        public IActionResult Delete([FromQuery] string filename, [FromQuery] string ipv4, [FromQuery] string ipv6, [FromQuery] int port) {
            if(FileManager.Delete(FileManager.Get(filename, PeerManager.Get(ipv4, ipv6, port))) != null) {
                return StatusCode(200);
            } else {
                return StatusCode(204);
            }
        }

    }
}
