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
        IProofOfWorkAlgorithm _proofOfWorkAlg;
        public List<Block> Chain { get; private set; }

        // Goes in to the next mined block. 
        public List<Transaction> CurrentTransactions { get; private set; }

        int index;

        public BlockChain()
        {
            index = 0;
            this.CurrentTransactions = new List<Transaction>();
            this.Chain = new List<Block>();

            //Create Genesis Block
            NewBlock(100, "1");            
        }

        public Block NewBlock(int proof, string previousHash = "")
        {
            var block = new Block(this.Chain.Count + 1, 
                                  this.CurrentTransactions, 
                                  proof, 
                                  previousHash == "" ? Hash(Chain.Last()) : previousHash);
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

        private static string Hash(Block block)
        {
            var message = Encoding.ASCII.GetBytes(block.ToJson());
            return Hashing.Hash(message);
        }

        public Block LastBlock()
        {
            return this.Chain.Last();
        }
    }
}
