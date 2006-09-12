#region Copyright (c) 2004 Ian Davis and James Carlyle
/*------------------------------------------------------------------------------
COPYRIGHT AND PERMISSION NOTICE

Copyright (c) 2004 Ian Davis and James Carlyle

Permission is hereby granted, free of charge, to any person obtaining a copy of 
this software and associated documentation files (the "Software"), to deal in 
the Software without restriction, including without limitation the rights to 
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies 
of the Software, and to permit persons to whom the Software is furnished to do 
so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE 
SOFTWARE.
------------------------------------------------------------------------------*/
#endregion


namespace SemPlan.Spiral.Tests.Core {
  using NUnit.Framework;
  using System;
	/// <summary>
	/// Compares a list of NTriples against an Expected list to see if the two lists are equivilent
	/// </summary>
  /// <remarks>
  /// $Id: NTripleListVerifierTest.cs,v 1.2 2005/05/26 14:24:30 ian Exp $
  ///</remarks>
  [TestFixture]
  public class NTripleListVerifierTest {

    [Test]
    public void VerifySingleUUUMatch() {
      NTripleListVerifier verifier = new NTripleListVerifier();
      verifier.Expect("<http://example/subj> <http://example/pred> <http://example/obj> .");

      verifier.Receive("<http://example/subj> <http://example/pred> <http://example/obj> .");
      Assert.IsTrue( verifier.Verify() );
    }
    
    [Test]
    public void VerifySingleUUUMismatch() {
      NTripleListVerifier verifier = new NTripleListVerifier();
      verifier.Expect("<http://example/subj> <http://example/pred> <http://example/obj> .");
      
      verifier.Receive("<http://example/subj> <http://example/pred> <http://example/obj2> .");
      Assert.IsFalse( verifier.Verify() );
    }

    [Test]
    public void VerifySingleUUPMatch() {
      NTripleListVerifier verifier = new NTripleListVerifier();
      verifier.Expect("<http://example/subj> <http://example/pred> \"fizz\" .");

      verifier.Receive("<http://example/subj> <http://example/pred> \"fizz\" .");
      Assert.IsTrue( verifier.Verify() );
    }

    [Test]
    public void VerifySingleUUPMatchWithLanguage() {
      NTripleListVerifier verifier = new NTripleListVerifier();
      verifier.Expect("<http://example/subj> <http://example/pred> \"fizz\"@en .");

      verifier.Receive("<http://example/subj> <http://example/pred> \"fizz\"@en .");
      Assert.IsTrue( verifier.Verify() );
    }

    
    [Test]
    public void VerifySingleUUPMismatchByLexicalValue() {
      NTripleListVerifier verifier = new NTripleListVerifier();
      verifier.Expect("<http://example/subj> <http://example/pred> \"fizz\" .");
      
      verifier.Receive("<http://example/subj> <http://example/pred> \"bang\" .");
      Assert.IsFalse( verifier.Verify() );
    }

    [Test]
    public void VerifySingleUUPMismatchWithLanguageByLexicalValue() {
      NTripleListVerifier verifier = new NTripleListVerifier();
      verifier.Expect("<http://example/subj> <http://example/pred> \"fizz\"@en .");
      
      verifier.Receive("<http://example/subj> <http://example/pred> \"bang\"@en .");
      Assert.IsFalse( verifier.Verify() );
    }

    [Test]
    public void VerifySingleUUPMismatchByLanguage() {
      NTripleListVerifier verifier = new NTripleListVerifier();
      verifier.Expect("<http://example/subj> <http://example/pred> \"fizz\"@en .");
      
      verifier.Receive("<http://example/subj> <http://example/pred> \"fizz\"@fr .");
      Assert.IsFalse( verifier.Verify() );
    }


    [Test]
    public void VerifySingleUUTMatch() {
      NTripleListVerifier verifier = new NTripleListVerifier();
      verifier.Expect("<http://example/subj> <http://example/pred> \"fizz\"^^<http://example.com/type> .");

      verifier.Receive("<http://example/subj> <http://example/pred> \"fizz\"^^<http://example.com/type> .");
      Assert.IsTrue( verifier.Verify() );
    }
    
    [Test]
    public void VerifySingleUUTMismatchByLexicalValue() {
      NTripleListVerifier verifier = new NTripleListVerifier();
      verifier.Expect("<http://example/subj> <http://example/pred> \"fizz\"^^<http://example.com/type> .");
      
      verifier.Receive("<http://example/subj> <http://example/pred> \"bang\"^^<http://example.com/type> .");
      Assert.IsFalse( verifier.Verify() );
    }

    [Test]
    public void VerifySingleUUTMismatchByDatatype() {
      NTripleListVerifier verifier = new NTripleListVerifier();
      verifier.Expect("<http://example/subj> <http://example/pred> \"fizz\"^^<http://example.com/type> .");
      
      verifier.Receive("<http://example/subj> <http://example/pred> \"fizz\"^^<http://example.com/foo> .");
      Assert.IsFalse( verifier.Verify() );
    }
    
    [Test]
    public void VerifyNonBlankNodeTriplesComparedInAnyOrder() {
      NTripleListVerifier verifier = new NTripleListVerifier();
      verifier.Expect("<http://example/subj> <http://example/pred> \"fizz\"@en .");
      verifier.Expect("<http://example/subj> <http://example/pred> \"fizz\"^^<http://example.com/type> .");
      verifier.Expect("<http://example/subj> <http://example/pred> <http://example/obj> .");

      verifier.Receive("<http://example/subj> <http://example/pred> <http://example/obj> .");
      verifier.Receive("<http://example/subj> <http://example/pred> \"fizz\"^^<http://example.com/type> .");
      verifier.Receive("<http://example/subj> <http://example/pred> \"fizz\"@en .");
      Assert.IsTrue( verifier.Verify() );
    }
    
    [Test]
    public void VerifyRecievedCountMustNotBeLessThanExpected() {
      NTripleListVerifier verifier = new NTripleListVerifier();
      verifier.Expect("<http://example/subj> <http://example/pred> \"fizz\"@en .");
      verifier.Expect("<http://example/subj> <http://example/pred> \"fizz\"@fr .");
      verifier.Expect("<http://example/subj> <http://example/pred> \"fizz\"^^<http://example.com/type> .");

      verifier.Receive("<http://example/subj> <http://example/pred> \"fizz\"^^<http://example.com/type> .");
      verifier.Receive("<http://example/subj> <http://example/pred> \"fizz\"@en .");
      Assert.IsFalse( verifier.Verify() );
    }
    
    [Test]
    public void VerifyRecievedCountMustNotBeGreaterThanExpected() {
      NTripleListVerifier verifier = new NTripleListVerifier();
      verifier.Expect("<http://example/subj> <http://example/pred> \"fizz\"^^<http://example.com/type> .");

      verifier.Receive("<http://example/subj> <http://example/pred> \"fizz\"^^<http://example.com/type> .");
      verifier.Receive("<http://example/subj> <http://example/pred> \"fizz\"@en .");
      Assert.IsFalse( verifier.Verify() );
    }

    [Test]
    public void VerifyConsidersDuplicateReceivedTriples() {
      NTripleListVerifier verifier = new NTripleListVerifier();
      verifier.Expect("<http://example/subj> <http://example/pred> \"fizz\"^^<http://example.com/type> .");

      verifier.Receive("<http://example/subj> <http://example/pred> \"fizz\"^^<http://example.com/type> .");
      verifier.Receive("<http://example/subj> <http://example/pred> \"fizz\"^^<http://example.com/type> .");
      Assert.IsFalse( verifier.Verify() );
    }


    [Test]
    public void VerifySingleUUBMatchWithSameNodeId() {
      NTripleListVerifier verifier = new NTripleListVerifier();
      verifier.Expect("<http://example/subj> <http://example/pred> _:genid1 .");

      verifier.Receive("<http://example/subj> <http://example/pred> _:genid1 .");
      Assert.IsTrue( verifier.Verify() );
    }

    [Test]
    public void VerifySingleUUBMatchWithDifferentNodeId() {
      NTripleListVerifier verifier = new NTripleListVerifier();
      verifier.Expect("<http://example/subj> <http://example/pred> _:genid1 .");

      verifier.Receive("<http://example/subj> <http://example/pred> _:gook .");
      Assert.IsTrue( verifier.Verify() );
    }

    [Test]
    public void VerifyMultipleUUBMatchWithSameNodeId() {
      NTripleListVerifier verifier = new NTripleListVerifier();
      verifier.Expect("<http://example/subj> <http://example/pred> _:genid1 .");
      verifier.Expect("<http://example/subj> <http://example/pred2> _:genid1 .");

      verifier.Receive("<http://example/subj> <http://example/pred> _:genid1 .");
      verifier.Receive("<http://example/subj> <http://example/pred2> _:genid1 .");
      Assert.IsTrue( verifier.Verify() );
    }

    [Test]
    public void VerifyMultipleUUBMatchWithDifferentNodeId() {
      NTripleListVerifier verifier = new NTripleListVerifier();
      verifier.Expect("<http://example/subj> <http://example/pred> _:genid1 .");
      verifier.Expect("<http://example/subj> <http://example/pred2> _:genid1 .");

      verifier.Receive("<http://example/subj> <http://example/pred> _:jim .");
      verifier.Receive("<http://example/subj> <http://example/pred2> _:jim .");
      Assert.IsTrue( verifier.Verify() );
    }

    [Test]
    public void VerifySingleBUUMatchWithSameNodeId() {
      NTripleListVerifier verifier = new NTripleListVerifier();
      verifier.Expect("_:genid1 <http://example/pred> <http://example/obj> .");

      verifier.Receive("_:genid1 <http://example/pred> <http://example/obj> .");
      Assert.IsTrue( verifier.Verify() );
    }

    [Test]
    public void VerifySingleBUUMatchWithDifferentNodeId() {
      NTripleListVerifier verifier = new NTripleListVerifier();
      verifier.Expect("_:genid1 <http://example/pred> <http://example/obj> .");

      verifier.Receive("_:jimbob <http://example/pred> <http://example/obj> .");
      Assert.IsTrue( verifier.Verify() );
    }


    [Test]
    public void VerifyMultipleBUUMatchWithSameNodeId() {
      NTripleListVerifier verifier = new NTripleListVerifier();
      verifier.Expect("_:genid1 <http://example/pred> <http://example/obj> .");
      verifier.Expect("_:genid1 <http://example/pred2> <http://example/obj> .");

      verifier.Receive("_:genid1 <http://example/pred> <http://example/obj> .");
      verifier.Receive("_:genid1 <http://example/pred2> <http://example/obj> .");
      Assert.IsTrue( verifier.Verify() );
    }

    [Test]
    public void VerifyMultipleBUUMatchWithDifferentNodeId() {
      NTripleListVerifier verifier = new NTripleListVerifier();
      verifier.Expect("_:genid1 <http://example/pred> <http://example/obj> .");
      verifier.Expect("_:genid1 <http://example/pred2> <http://example/obj> .");

      verifier.Receive("_:buffy <http://example/pred> <http://example/obj> .");
      verifier.Receive("_:buffy <http://example/pred2> <http://example/obj> .");
      Assert.IsTrue( verifier.Verify() );
    }

    [Test]
    public void VerifySingleBUBMatchWithSameNodeId() {
      NTripleListVerifier verifier = new NTripleListVerifier();
      verifier.Expect("_:genid1 <http://example/pred> _:genid1 .");

      verifier.Receive("_:genid1 <http://example/pred> _:genid1 .");
      Assert.IsTrue( verifier.Verify() );
    }

    [Test]
    public void VerifySingleBUBMatchWithDifferentNodeId() {
      NTripleListVerifier verifier = new NTripleListVerifier();
      verifier.Expect("_:genid1 <http://example/pred> _:genid1 .");

      verifier.Receive("_:buffy <http://example/pred> _:buffy .");
      Assert.IsTrue( verifier.Verify() );
    }

    [Test]
    public void VerifyMultipleBUBMatchWithSameNodeId() {
      NTripleListVerifier verifier = new NTripleListVerifier();
      verifier.Expect("_:genid1 <http://example/pred> _:genid1 .");
      verifier.Expect("_:genid1 <http://example/pred2> _:genid1 .");

      verifier.Receive("_:genid1 <http://example/pred> _:genid1 .");
      verifier.Receive("_:genid1 <http://example/pred2> _:genid1 .");
      Assert.IsTrue( verifier.Verify() );
    }

    [Test]
    public void VerifyMultipleBUBMatchWithDifferentNodeId() {
      NTripleListVerifier verifier = new NTripleListVerifier();
      verifier.Expect("_:genid1 <http://example/pred> _:genid1 .");
      verifier.Expect("_:genid1 <http://example/pred2> _:genid1 .");

      verifier.Receive("_:buffy <http://example/pred> _:buffy .");
      verifier.Receive("_:buffy <http://example/pred2> _:buffy .");
      Assert.IsTrue( verifier.Verify() );
    }

    [Test]
    public void VerifyMultipleUUBMatchWithTwoNodeIdsReceiveUsesSameIds() {
      NTripleListVerifier verifier = new NTripleListVerifier();
      verifier.Expect("<http://example/subj> <http://example/pred> _:genid1 .");
      verifier.Expect("<http://example/subj> <http://example/pred2> _:genid2 .");

      verifier.Receive("<http://example/subj> <http://example/pred> _:genid1 .");
      verifier.Receive("<http://example/subj> <http://example/pred2> _:genid2 .");
      Assert.IsTrue( verifier.Verify() );
    }
    [Test]
    public void VerifyMultipleUUBMatchWithTwoNodeIdsReceiveUsesDifferentIds() {
      NTripleListVerifier verifier = new NTripleListVerifier();
      verifier.Expect("<http://example/subj> <http://example/pred> _:genid1 .");
      verifier.Expect("<http://example/subj> <http://example/pred2> _:genid2 .");

      verifier.Receive("<http://example/subj> <http://example/pred> _:buffy1 .");
      verifier.Receive("<http://example/subj> <http://example/pred2> _:buffy2 .");
      Assert.IsTrue( verifier.Verify() );
    }

    [Test]
    public void VerifyMultipleUUBMatchWithTwoNodeIdsReceiveUsesDifferentIdsOutOfOrder() {
      NTripleListVerifier verifier = new NTripleListVerifier();
      verifier.Expect("<http://example/subj> <http://example/pred> _:genid1 .");
      verifier.Expect("<http://example/subj> <http://example/pred2> _:genid2 .");

      verifier.Receive("<http://example/subj> <http://example/pred2> _:buffy2 .");
      verifier.Receive("<http://example/subj> <http://example/pred> _:buffy1 .");
      Assert.IsTrue( verifier.Verify() );
    }
    

    [Test]
    public void VerifyMultipleUUBMatchWithThreeNodeIdsReceiveUsesDifferentIdsOutOfOrder() {
      NTripleListVerifier verifier = new NTripleListVerifier();
      verifier.Expect("<http://example/subj> <http://example/pred> _:genid1 .");
      verifier.Expect("<http://example/subj> <http://example/pred2> _:genid2 .");
      verifier.Expect("<http://example/subj> <http://example/pred2> _:genid3 .");

      verifier.Receive("<http://example/subj> <http://example/pred2> _:buffy2 .");
      verifier.Receive("<http://example/subj> <http://example/pred2> _:buffy3 .");
      verifier.Receive("<http://example/subj> <http://example/pred> _:buffy1 .");
      Assert.IsTrue( verifier.Verify() );
    }
        



    [Test]
    public void VerifyMultipleBUUMatchWithTwoNodeIdsReceiveUsesSameIds() {
      NTripleListVerifier verifier = new NTripleListVerifier();
      verifier.Expect("_:genid2 <http://example/pred> <http://example.com/obj> .");
      verifier.Expect("_:genid1 <http://example/pred2> <http://example.com/obj> .");

      verifier.Receive("_:genid2 <http://example/pred> <http://example.com/obj> .");
      verifier.Receive("_:genid1 <http://example/pred2> <http://example.com/obj> .");
      Assert.IsTrue( verifier.Verify() );
    }
    [Test]
    public void VerifyMultipleBUUMatchWithTwoNodeIdsReceiveUsesDifferentIds() {
      NTripleListVerifier verifier = new NTripleListVerifier();
      verifier.Expect("_:genid2 <http://example/pred> <http://example.com/obj> .");
      verifier.Expect("_:genid1 <http://example/pred2> <http://example.com/obj> .");

      verifier.Receive("_:buffy2 <http://example/pred> <http://example.com/obj> .");
      verifier.Receive("_:buffy1 <http://example/pred2> <http://example.com/obj> .");
      Assert.IsTrue( verifier.Verify() );
    }

    [Test]
    public void VerifyMultipleBUUMatchWithTwoNodeIdsReceiveUsesDifferentIdsOutOfOrder() {
      NTripleListVerifier verifier = new NTripleListVerifier();
      verifier.Expect("_:genid2 <http://example/pred> <http://example.com/obj> .");
      verifier.Expect("_:genid1 <http://example/pred2> <http://example.com/obj> .");

      verifier.Receive("_:buffy1 <http://example/pred2> <http://example.com/obj> .");
      verifier.Receive("_:buffy2 <http://example/pred> <http://example.com/obj> .");
      Assert.IsTrue( verifier.Verify() );
    }
    

    [Test]
    public void VerifyMultipleBUUMatchWithThreeNodeIdsReceiveUsesDifferentIdsOutOfOrder() {
      NTripleListVerifier verifier = new NTripleListVerifier();
      verifier.Expect("_:genid2 <http://example/pred> <http://example.com/obj> .");
      verifier.Expect("_:genid1 <http://example/pred2> <http://example.com/obj> .");
      verifier.Expect("_:genid3 <http://example/pred2> <http://example.com/obj> .");

      verifier.Receive("_:buffy1 <http://example/pred2> <http://example.com/obj> .");
      verifier.Receive("_:buffy2 <http://example/pred> <http://example.com/obj> .");
      verifier.Receive("_:buffy3 <http://example/pred2> <http://example.com/obj> .");
      Assert.IsTrue( verifier.Verify() );
    }

    [Test]
    public void VerifyMultipleBUBMatchWithThreeNodeIdsReceiveUsesDifferentIdsOutOfOrder() {
      NTripleListVerifier verifier = new NTripleListVerifier();
      verifier.Expect("_:genid2 <http://example/pred> _:genid1 .");
      verifier.Expect("_:genid1 <http://example/pred2> _:genid2 .");
      verifier.Expect("_:genid3 <http://example/pred2> <http://example.com/obj> .");

      verifier.Receive("_:buffy3 <http://example/pred2> _:buffy2 .");
      verifier.Receive("_:buffy2 <http://example/pred> _:buffy3 .");
      verifier.Receive("_:buffy1 <http://example/pred2> <http://example.com/obj> .");
      Assert.IsTrue( verifier.Verify() );
    }
    
    [Test]
    public void VerifierNormalisesWhitespaceBetweenExpectedTripleComponents() {
      NTripleListVerifier verifier = new NTripleListVerifier();
      verifier.Expect("<http://example/subj>  <http://example/pred>  <http://example/obj>  .");

      verifier.Receive("<http://example/subj> <http://example/pred> <http://example/obj> .");
      Assert.IsTrue( verifier.Verify() );
    }
  }
}
