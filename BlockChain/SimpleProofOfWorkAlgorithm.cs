using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChainSecurity
{
    /// <summary>
    /// Simple proof of work algoritm. Find a number p' such that hash(pp') contains 4 leading zeroes,
    /// where p is the previous proof, and p' is the new proof
    /// </summary>
    public class SimpleProofOfWorkAlgorithm : IProofOfWorkAlgorithm
    {
        private readonly int _count;
        private readonly char _leadingChar;
        private readonly string _leadingSequence;

        public SimpleProofOfWorkAlgorithm(char leadingChar, int count)
        {
            _leadingChar = leadingChar;
            _count = count;

            _leadingSequence = new String(_leadingChar, _count);
        }

        public int ProofOfWork(int lastProof)
        {
            var proof = 0;
            while(!ValidateProof(lastProof, proof))
            {
                proof++;
            }

            return proof;
        }

        public bool ValidateProof(int lastProof, int proof)
        {
            var message = BitConverter.GetBytes(lastProof).Concat(BitConverter.GetBytes(proof)).ToArray();
            var digest = Hashing.Hash(message);
            return digest.Substring(0, _count).SequenceEqual(_leadingSequence);
        }
    }
}
