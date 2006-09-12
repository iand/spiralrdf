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
	/// An instrumented version of ResourceFactory that does nothing and returns stub values
	/// </summary>
  /// <remarks>
  /// $Id: ResourceFactoryStub.cs,v 1.2 2005/05/26 14:24:30 ian Exp $
  ///</remarks>
  public class ResourceFactoryStub : ResourceFactory {
    
    public override UriRef MakeUriRef(string uriRef) {
      return new UriRefStub();
    }

    public override BlankNode MakeBlankNode() {
      return new BlankNodeStub();
    }

    public override BlankNode MakeBlankNode(string nodeId) {
      return new BlankNodeStub();
    }

    public override PlainLiteral MakePlainLiteral(string lexicalValue) {
      return new PlainLiteralStub();
    }

    public override PlainLiteral MakePlainLiteral(string lexicalValue, string language) {
      return new PlainLiteralStub();
    }

    public override TypedLiteral MakeTypedLiteral(string lexicalValue, string dataTypeUriRef) {
      return new TypedLiteralStub();
    }
    
  }
}
