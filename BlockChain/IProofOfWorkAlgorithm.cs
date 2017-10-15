using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChainSecurity
{
    public interface IProofOfWorkAlgorithm
    {
        int ProofOfWork(int lastProof);
        bool ValidateProof(int lastProof, int proof);
    }
}
