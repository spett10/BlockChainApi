using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlockChainSecurity;

namespace BlockChainTests
{
    [TestClass]
    public class ProofOfWorkTests
    {
        [TestMethod]
        public void SimpleProofOfWorkAlgorithm()
        {
            var lastproof = 1234;

            // Even upping it to five takes some time (i.e. more than 4 seconds)
            var pow = new SimpleProofOfWorkAlgorithm('0', 4);

            var work = pow.ProofOfWork(lastproof);

            Assert.IsTrue(pow.ValidateProof(lastproof, work));
        }
    }
}
