using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text;

namespace BlockChainSecurity
{
    public class Block
    {
        public int Index { get; private set; }
        public uint UnixTimestamp { get; private set; }
        public List<Transaction> Transactions { get; private set; }

        // The proof that we did the work for this block. It is easily verified - it should depend on the proof contained in the preceding block. So by having access to proof of
        // Two adjacent blocks, we can verify that the proof in the foremost block is correct, and thus that the work was done and that the block is valid. 
        public int Proof { get; private set; }

        // Hash of the previous Block in a chain. If a given block is corrupted, then the hash contained in the next block of this prev. block will not check out.
        public string PreviousHash { get; private set; }

        public Block(int index, List<Transaction> transactions, int proof, string previousHash)
        {
            this.Index = index;
            this.UnixTimestamp = DateTime.Now.ToUnixTimeStamp();
            this.Transactions = transactions;
            this.Proof = proof;
            this.PreviousHash = previousHash;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public string Hash()
        {
            var message = Encoding.ASCII.GetBytes(this.ToJson());
            return Hashing.Hash(message);
        }
    }
}
