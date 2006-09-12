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
  using System.Collections;


	/// <summary>
	/// An instrumented version of ResourceFactory that counts calls to  methods. 
	/// </summary>
  /// <remarks>
  /// $Id: ResourceFactoryCounter.cs,v 1.2 2005/05/26 14:24:30 ian Exp $
  ///</remarks>
  public class ResourceFactoryCounter : ResourceFactory {
          
    public int MakeUriRefCalled = 0;
    public override UriRef MakeUriRef(string uriRef) {
     ++MakeUriRefCalled;
      return null;
    }

    public int MakeBlankNodeCalled = 0;
    public override BlankNode MakeBlankNode() {
      ++MakeBlankNodeCalled;
      return null;
    }

    public int MakeBlankNodeWithNodeIdCalled = 0;
    public override BlankNode MakeBlankNode(string nodeId) {
      ++MakeBlankNodeWithNodeIdCalled;
      return null;
    }

    public int MakePlainLiteralCalled = 0;
    public override PlainLiteral MakePlainLiteral(string lexicalValue) {
      ++MakePlainLiteralCalled;
      return null;
    }

    public int MakePlainLiteralWithLanguageCalled = 0;
    public override PlainLiteral MakePlainLiteral(string lexicalValue, string language) {
      ++MakePlainLiteralWithLanguageCalled;
      return null;
    }

    public int MakeTypedLiteralCalled = 0;
    public override TypedLiteral MakeTypedLiteral(string lexicalValue, string dataTypeUriRef) {
      ++MakeTypedLiteralCalled;
      return null;
    }
    
  }
}
