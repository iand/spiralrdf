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
  using SemPlan.Spiral.Core;
  
	/// <summary>
	/// An instrumented version of RdfWriter that counts calls to  methods. 
	/// </summary>
  /// <remarks>
  /// $Id: RdfWriterCounter.cs,v 1.2 2005/05/26 14:24:30 ian Exp $
  ///</remarks>
  public class RdfWriterCounter : RdfWriter {
    public int StartOutputCalled = 0;
    public void StartOutput() {
      ++StartOutputCalled; 
    }

    public int EndOutputCalled = 0;
    public void EndOutput() {
      ++EndOutputCalled;
    }

    public int StartSubjectCalled = 0;
    public void StartSubject() {
      ++StartSubjectCalled;
    }
    
    public int EndSubjectCalled = 0;
    public void EndSubject() {
      ++EndSubjectCalled;
    }

    public int StartPredicateCalled = 0;
    public void StartPredicate() {
      ++StartPredicateCalled;
    }
    
    public int EndPredicateCalled = 0;
    public void EndPredicate() {
      ++EndPredicateCalled;
    }

    public int StartObjectCalled = 0;
    public void StartObject() {
      ++StartObjectCalled;
    }

    public int EndObjectCalled = 0;
    public void EndObject() {
      ++EndObjectCalled;
    }

    public int WriteUriRefCalled = 0;
    public void WriteUriRef(string uriRef) {
      ++WriteUriRefCalled;
    }

    public int WritePlainLiteralCalled = 0;
    public void WritePlainLiteral(string lexicalValue) {
      ++WritePlainLiteralCalled;
    }

    public int WritePlainLiteralLanguageCalled = 0;
    public void WritePlainLiteral(string lexicalValue, string language) {
      ++WritePlainLiteralLanguageCalled;
    }
    
    public int WriteTypedLiteralCalled = 0;
    public void WriteTypedLiteral(string lexicalValue, string uriRef) {
      ++WriteTypedLiteralCalled;
    }
    
    public int WriteBlankNodeCalled = 0;
    public void WriteBlankNode(string nodeId) {
      ++WriteBlankNodeCalled;
    }
  }
}