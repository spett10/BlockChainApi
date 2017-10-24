using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text;

namespace BlockChainSecurity
{
    // I think all properties must have a public setter - if we dont have it the timestamp is not set when the json is deserialized into this object.
    // TODO: make a dto instead, and then map that to the below? No reason really. Even if you can manipulate the below fields (you still could with private with reflection),
    // The properties of the blockchain and consensus means it would not be viable, thats the whole point about the distributed ledger. 
    public class Block
    {
        public int Index { get; set; }
        public uint UnixTimestamp { get; set; }
        public List<Transaction> Transactions { get; set; }

        // The proof that we did the work for this block. It is easily verified - it should depend on the proof contained in the preceding block. So by having access to proof of
        // Two adjacent blocks, we can verify that the proof in the foremost block is correct, and thus that the work was done and that the block is valid. 
        public int Proof { get; set; }

        // Hash of the previous Block in a chain. If a given block is corrupted, then the hash contained in the next block of this prev. block will not check out.
        public string PreviousHash { get; set; }

        public Block(int index, uint timestamp, List<Transaction> transactions, int proof, string previousHash)
        {
            this.Index = index;
            this.UnixTimestamp = timestamp;
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
