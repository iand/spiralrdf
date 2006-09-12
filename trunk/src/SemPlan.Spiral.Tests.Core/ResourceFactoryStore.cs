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
	/// An instrumented version of ResourceFactory that can verify arguments to method calls. 
	/// </summary>
  /// <remarks>
  /// $Id: ResourceFactoryStore.cs,v 1.2 2005/05/26 14:24:30 ian Exp $
  ///</remarks>
  public class ResourceFactoryStore : ResourceFactory {
    private bool itsWasMakeBlankNodeCalledWithANodeId;
    private MethodCallStore itsMethodCalls;

    public ResourceFactoryStore() {
      itsMethodCalls = new MethodCallStore();
    }
    
    public override UriRef MakeUriRef(string uriRef) {
      itsMethodCalls.RecordMethodCall("MakeUriRef", uriRef);
      return null;
    }

    public bool WasMakeUriRefCalledWith(string uriRef) {
      return itsMethodCalls.WasMethodCalledWith("MakeUriRef", uriRef);
    }

    public override BlankNode MakeBlankNode() {
      itsMethodCalls.RecordMethodCall("MakeBlankNode", null);
      return null;
    }

    public bool WasMakeBlankNodeCalledWith() {
      return itsMethodCalls.WasMethodCalledWith("MakeBlankNode", null);
    }
   
     public override BlankNode MakeBlankNode(string nodeId) {
      itsMethodCalls.RecordMethodCall("MakeBlankNode", nodeId);
      itsWasMakeBlankNodeCalledWithANodeId = true;
      return null;
    }

    public bool WasMakeBlankNodeCalledWith(string nodeId) {
      return itsMethodCalls.WasMethodCalledWith("MakeBlankNode", nodeId);
    }
   
    public bool WasMakeBlankNodeCalledWithANodeId() {
      return itsWasMakeBlankNodeCalledWithANodeId;
    }

    public override PlainLiteral MakePlainLiteral(string lexicalValue) {
      itsMethodCalls.RecordMethodCall("MakePlainLiteral", lexicalValue);
      return null;
    }

    public bool WasMakePlainLiteralCalledWith(string lexicalValue) {
      return itsMethodCalls.WasMethodCalledWith("MakePlainLiteral", lexicalValue);
    }

    
    public override PlainLiteral MakePlainLiteral(string lexicalValue, string language) {
      itsMethodCalls.RecordMethodCall("MakePlainLiteral", lexicalValue, language);
      return null;
    }
    
    public bool WasMakePlainLiteralCalledWith(string lexicalValue, string language) {
      return itsMethodCalls.WasMethodCalledWith("MakePlainLiteral", lexicalValue, language);
    }
    
    public override TypedLiteral MakeTypedLiteral(string lexicalValue, string dataTypeUriRef) {
      itsMethodCalls.RecordMethodCall("MakeTypedLiteral", lexicalValue, dataTypeUriRef);
      return null;
    }
    
    public bool WasMakeTypedLiteralCalledWith(string lexicalValue, string dataTypeUriRef) {
      return itsMethodCalls.WasMethodCalledWith("MakeTypedLiteral", lexicalValue, dataTypeUriRef);
    }
    
  }
}
