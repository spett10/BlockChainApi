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

namespace BlockChainApi.Controllers
{
    public class BlockChainController : ApiController
    {
        private static readonly string _id = Guid.NewGuid().ToString().Replace("-","");

        private static BlockChain BlockChain;
        private static IProofOfWorkAlgorithm proofOfWorkAlgorithm;

        static BlockChainController()
        {
            BlockChain = new BlockChain();
            proofOfWorkAlgorithm = new SimpleProofOfWorkAlgorithm('0', 4);
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
            var proof = proofOfWorkAlgorithm.ProofOfWork(lastProof);

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
            catch(Exception e)
            {
                return InternalServerError();
            }
            
        }
    }
}
