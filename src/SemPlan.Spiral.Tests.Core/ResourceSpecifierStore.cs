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
  using System;
  
	/// <summary>
	/// An instrumented version of ResourceSpecifier that stores arguments to method calls
	/// </summary>
  /// <remarks>
  /// $Id: ResourceSpecifierStore.cs,v 1.2 2005/05/26 14:24:30 ian Exp $
  ///</remarks>
  public class ResourceSpecifierStore : ResourceSpecifier {

    private MethodCallStore itsMethodCalls;
    
    public ResourceSpecifierStore() {
      itsMethodCalls = new MethodCallStore();
    }

    public bool MatchesUriRef(string uriRef) {
      itsMethodCalls.RecordMethodCall("MatchesUriRef", uriRef);
      return false;
    }
    public bool WasMatchesUriRefCalledWith(string uriRef)  {
      return itsMethodCalls.WasMethodCalledWith("MatchesUriRef", uriRef);
    }
    
    public bool MatchesPlainLiteral(string lexicalValue) {
      itsMethodCalls.RecordMethodCall("MatchesPlainLiteral", lexicalValue);
      return false;
    }
    public bool WasMatchesPlainLiteralCalledWith(string lexicalValue)  {
      return itsMethodCalls.WasMethodCalledWith("MatchesPlainLiteral", lexicalValue);
    }

    public bool MatchesPlainLiteral(string lexicalValue, string language) {
      itsMethodCalls.RecordMethodCall("MatchesPlainLiteral", lexicalValue, language);
      return false;
    }
    public bool WasMatchesPlainLiteralCalledWith(string lexicalValue, string language) {
      return itsMethodCalls.WasMethodCalledWith("MatchesPlainLiteral", lexicalValue, language);
    }
    
    public bool MatchesTypedLiteral(string lexicalValue, string datatype) {
      itsMethodCalls.RecordMethodCall("MatchesTypedLiteral", lexicalValue, datatype);
      return false;
    }
    public bool WasMatchesTypedLiteralCalledWith(string lexicalValue, string datatype) {
      return itsMethodCalls.WasMethodCalledWith("MatchesTypedLiteral", lexicalValue, datatype);
    }
    
    private bool itsMatchedBlankNodeCalled = false;
    public bool MatchesBlankNode() {
      itsMatchedBlankNodeCalled = true;
      return false;
    }
    public bool WasMatchesBlankNodeCalled()  {
      return itsMatchedBlankNodeCalled;
    }


  }
}