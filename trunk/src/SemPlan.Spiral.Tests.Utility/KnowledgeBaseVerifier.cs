#region Copyright (c) 2006 Ian Davis and James Carlyle
/*------------------------------------------------------------------------------
COPYRIGHT AND PERMISSION NOTICE

Copyright (c) 2006 Ian Davis and James Carlyle

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

namespace SemPlan.Spiral.Tests.Utility {
  using NUnit.Framework;
  using SemPlan.Spiral.Core;
  using SemPlan.Spiral.Tests.Core;
  using SemPlan.Spiral.Utility;
  using System;  
  using System.Collections;
  using System.IO;
	/// <summary>
	/// A verifier for knowledge bases
	/// </summary>
  /// <remarks>
  /// $Id: KnowledgeBaseVerifier.cs,v 1.1 2006/01/06 15:24:59 ian Exp $
  ///</remarks>
    public class KnowledgeBaseVerifier {
      private ArrayList itsTriples;
      private string lastFailureDescription;

      public KnowledgeBaseVerifier() {
        itsTriples = new ArrayList();
      }      
    
      public void expect(string nTriple) {
        itsTriples.Add(nTriple);
      }

      public string LastFailureDescription {
        get { return lastFailureDescription; }
      }

      public bool verify(KnowledgeBase knowledge) {
        NTripleListVerifier verifier = new NTripleListVerifier();
        
        foreach (string nTriple in itsTriples) {
          verifier.Expect(nTriple);
        }

        StringWriter received = new StringWriter();
        NTripleWriter writer = new NTripleWriter(received);
        knowledge.Write(writer);
  
        StringReader receivedReader = new StringReader(received.ToString());
        string receivedLine = receivedReader.ReadLine();
        while (receivedLine != null) {
    
          string trimmed = receivedLine.Trim();
          if (trimmed.Length > 0 &&  ! trimmed.StartsWith("#") ) {
            verifier.Receive( trimmed );
           }
          receivedLine = receivedReader.ReadLine();
        }

        bool verifyResult = verifier.Verify();
        lastFailureDescription = verifier.GetLastFailureDescription();
        return verifyResult;
        
      }

    
    }

    
}