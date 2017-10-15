using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using BlockChainSecurity;
using Newtonsoft;
using System.Web.Http;
using System.Web.Http.Results;
using System.Collections.Specialized;
using System.Net.Http.Formatting;
using BlockChainApi.Models;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlockChainApi.Controllers
{
    public class BlockChainController : ApiController
    {
        private static readonly string _id = Guid.NewGuid().ToString().Replace("-","");

        private static BlockChain BlockChain;
        private static List<string> _nodes;

        static BlockChainController()
        {
            BlockChain = new BlockChain(new SimpleProofOfWorkAlgorithm('0', 4));
            _nodes = new List<string>();
        }

        // GET api/blockchain
        /// <summary>
        /// Mine a new block. Add the current transactions to the new block (implicity). Reward ourselves for findting the block by giviing us self 1 amount.
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/mine")]
        public IHttpActionResult Get()
        {
            var lastBlock = BlockChain.LastBlock();
            var lastProof = lastBlock.Proof;
            var proof = BlockChain.ProofOfWorkAlgorithm.ProofOfWork(lastProof);

            // Reward ourselves for mining. 0 means that this node did it, and id is this nodes identifier.
            BlockChain.NewTransaction("0", _id, 1);

            // Forge the new block by adding it to the chain
            var block = BlockChain.NewBlock(proof);

            var response = new MineResult()
            {
                Message = "New Block Forged",
                Index = block.Index,
                Transactions = block.Transactions,
                Proof = block.Proof,
                PreviousHash = block.PreviousHash
            };

            return Json(response);
        }

        [HttpGet]
        [Route("api/chain")]
        public JsonResult<List<Block>> FullChain()
        {
            return Json<List<Block>>(BlockChain.Chain);
        }

        // POST api/transactions/new
        [HttpPost]
        [Route("api/transactions/new")]
        public IHttpActionResult NewTransaction([FromBody] TransactionRequest request)
        {
            try
            {
                var amount = request.Amount;
                if(amount <= 0)
                {
                    return BadRequest();
                }

                //Create Transaction (Todo, send in request instead of each field). 
                var index = BlockChain.NewTransaction(request.Sender, request.Recipient, amount);
                var message = $"Transaction will be added to Block {index}";
                return Json(message);
            }
            catch
            {
                return InternalServerError();
            }            
        }

        [HttpGet]
        [Route("api/nodes/resolve")]
        public async Task<IHttpActionResult> Consensus()
        {
            var newChain = await ResolveConflitcs();

            ConsensusResponse result = null;

            if(newChain != null)
            {
                BlockChain.Chain = newChain;
                result = new ConsensusResponse()
                {
                    Message = "Our chain was replaced",
                    Chain = newChain
                };
            }
            else
            {
                result = new ConsensusResponse()
                {
                    Message = "Our chain is authoritative",
                    Chain = BlockChain.Chain
                };
            }

            return Json(result);
        }


        [HttpPost]
        [Route("api/nodes/register")]
        public IHttpActionResult RegisterNode([FromBody] RegisterNodeRequest request)
        {
            if(request.Nodes.Count < 1)
            {
                return BadRequest("Please supply valid list of nodes");
            }

            RegisterNodes(request.Nodes);

            var response = new RegisterNodeResponse()
            {
                Message = "New nodes have been added",
                TotalNodes = _nodes
            };

            return Json(response);
        }

        // Todo: use a dictionary or a hashmap?
        private void RegisterNodes(List<string> nodes)
        {
            foreach(var n in nodes)
            {
                if (!_nodes.Exists(x => x == n))
                {
                    _nodes.Add(n);
                }
            }
            
        }

        private async Task<List<Block>> ResolveConflitcs()
        {
            var neighbours = _nodes;
            List<Block> newChain = null;

            // Only looking for chains longer than us.
            int maxLength = BlockChain.Chain.Count;

            foreach(var n in neighbours)
            {
                var chain = await GetChainAsync(n);

                if(chain != null)
                {
                    var length = chain.Count;

                    if(length > maxLength && BlockChain.ValidChain(chain))
                    {
                        maxLength = length;
                        newChain = chain;
                    }
                }
            }

            return newChain;
        }

        private async Task<List<Block>> GetChainAsync(string node)
        {
            using (var client = new HttpClient())
            {
                List<Block> chain = null;
                HttpResponseMessage response = await client.GetAsync(node + "/api/chain");
                if(response.IsSuccessStatusCode)
                {
                    chain = await response.Content.ReadAsAsync<List<Block>>();
                }

                return chain;
            }            
        }
    }
}
