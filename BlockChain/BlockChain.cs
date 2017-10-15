using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace BlockChainSecurity
{
    public class BlockChain
    {
        public static IProofOfWorkAlgorithm ProofOfWorkAlgorithm { get; private set; }
        public List<Block> Chain { get; set; }

        // Goes in to the next mined block. 
        public List<Transaction> CurrentTransactions { get; private set; }

        public BlockChain(IProofOfWorkAlgorithm proofOfWorkAlgorithm)
        {
            this.CurrentTransactions = new List<Transaction>();
            this.Chain = new List<Block>();
            ProofOfWorkAlgorithm = proofOfWorkAlgorithm;

            //Create Genesis Block
            NewBlock(100, "1");            
        }

        public Block NewBlock(int proof, string previousHash = "")
        {
            var block = new Block(this.Chain.Count + 1, 
                                  this.CurrentTransactions, 
                                  proof, 
                                  previousHash == "" ? Chain.Last().Hash() : previousHash);
            //Reset transactions
            this.CurrentTransactions = new List<Transaction>();
            this.Chain.Add(block);
            return block;
        }

        public int NewTransaction(string sender, string recipient, int amount)
        {
            var transaction = new Transaction(sender, recipient, amount);
            this.CurrentTransactions.Add(transaction);

            //We return the index of the next block to be mined in the chain - this is the blopck this transaction will eventually be added to. 
            return LastBlock().Index + 1;
        }

        public Block LastBlock()
        {
            return this.Chain.Last();
        }

        /// <summary>
        /// Determine if the given blockchain is valid.
        /// </summary>
        /// <param name="chain"></param>
        /// <returns></returns>
        public static bool ValidChain(List<Block> chain)
        {
            var previousBlock = chain.ElementAt(0);
            var currentIndex = 1;

            while(currentIndex < chain.Count)
            {
                var block = chain.ElementAt(currentIndex);

                //Check that hash is correct
                if(!block.PreviousHash.SequenceEqual(previousBlock.Hash()))
                {
                    return false;
                }

                //Check that proof of work is correct
                if(!ProofOfWorkAlgorithm.ValidateProof(previousBlock.Proof, block.Proof))
                {
                    return false;
                }

                //Next block
                previousBlock = block;
                currentIndex++;
            }

            return true;
        }
    }
}
