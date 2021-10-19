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
    [Route("")]
    public class PeerController : ControllerBase {
        // GET @root
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpGet]
        public ActionResult<IEnumerable<Peer>> GetAll() {
            List<Peer> re = PeerManager.GetAll();
            if (re.Count > 0) {
                return Ok(re);
            }

            return NoContent();
        }

        // GET @root/peer
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Route("peer")]
        [HttpGet]
        public ActionResult<IEnumerable<Peer>> Get([FromQuery]string ipv4, [FromQuery] string ipv6, [FromQuery] int port) {
            Peer re = PeerManager.Get(ipv4, ipv6, port);
            if (re != null) {
                return Ok(re);
            }

            return NoContent();
        }

        // GET @root/peer
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Route("peer/fromfile")]
        [HttpGet]
        public ActionResult<IEnumerable<Peer>> Get([FromQuery] string filename) {
            List<Peer> re = PeerManager.Get(filename);
            if (re != null) {
                return Ok(re);
            }

            return NoContent();
        }

        // POST @root
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [HttpPost]
        public IActionResult Post([FromBody] Peer peer) {
            try {
                PeerManager.Add(peer);
                return StatusCode(201);
            } catch (Exception e) {
                return StatusCode(406, e.ToString());
            }
        }

        // DELETE @root
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpDelete]
        public IActionResult Delete([FromQuery] string ipv4, [FromQuery] string ipv6, [FromQuery] int port) {
            if (PeerManager.Delete(PeerManager.Get(ipv4, ipv6, port)) != null) {
                return StatusCode(200);
            } else {
                return StatusCode(204);
            }
        }

        [Obsolete]
        // PUT @root
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [HttpPut]
        public void Put([FromBody] Peer peer) {
            PeerManager.Update(peer);
        }

    }
}
