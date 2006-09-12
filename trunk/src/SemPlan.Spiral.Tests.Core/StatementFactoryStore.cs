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
	/// An instrumented version of StatementFactory that can verify arguments to method calls. s
	/// </summary>
  /// <remarks>
  /// $Id: StatementFactoryStore.cs,v 1.2 2005/05/26 14:24:30 ian Exp $
  ///</remarks>
  public class StatementFactoryStore : StatementFactory {

    private MethodCallStore itsMethodCalls;

    public StatementFactoryStore() {
      itsMethodCalls = new MethodCallStore();
    }

    
    public override Statement MakeStatement(UriRef subject, UriRef predicate, UriRef value) {
      itsMethodCalls.RecordMethodCall("MakeStatement", subject, predicate, value);
      return new StatementStub();
    }

    public bool WasMakeStatementCalledWith(UriRef subject, UriRef predicate, UriRef value) {
      return itsMethodCalls.WasMethodCalledWith("MakeStatement", subject, predicate, value);
    }
    
    public override Statement MakeStatement(BlankNode subject, UriRef predicate, UriRef value) {
      itsMethodCalls.RecordMethodCall("MakeStatement", subject, predicate, value);
      return new StatementStub();
    }
    
    public bool WasMakeStatementCalledWith(BlankNode subject, UriRef predicate, UriRef value) {
      return itsMethodCalls.WasMethodCalledWith("MakeStatement", subject, predicate, value);
    }
    
    public override Statement MakeStatement(UriRef subject, UriRef predicate, BlankNode value) {
      itsMethodCalls.RecordMethodCall("MakeStatement", subject, predicate, value);
      return new StatementStub();
    }
    
    public bool WasMakeStatementCalledWith(UriRef subject, UriRef predicate, BlankNode value) {
      return itsMethodCalls.WasMethodCalledWith("MakeStatement", subject, predicate, value);
    }
    
    public override Statement MakeStatement(BlankNode subject, UriRef predicate, BlankNode value) {
      itsMethodCalls.RecordMethodCall("MakeStatement", subject, predicate, value);
      return new StatementStub();
    }
    
    public bool WasMakeStatementCalledWith(BlankNode subject, UriRef predicate, BlankNode value) {
      return itsMethodCalls.WasMethodCalledWith("MakeStatement", subject, predicate, value);
    }
    
    public override Statement MakeStatement(UriRef subject, UriRef predicate, PlainLiteral value) {
      itsMethodCalls.RecordMethodCall("MakeStatement", subject, predicate, value);
      return new StatementStub();
    }
    
    public bool WasMakeStatementCalledWith(UriRef subject, UriRef predicate, PlainLiteral value) {
      return itsMethodCalls.WasMethodCalledWith("MakeStatement", subject, predicate, value);
    }
    
    public override Statement MakeStatement(BlankNode subject, UriRef predicate, PlainLiteral value) {
      itsMethodCalls.RecordMethodCall("MakeStatement", subject, predicate, value);
      return new StatementStub();
    }
    
    public bool WasMakeStatementCalledWith(BlankNode subject, UriRef predicate, PlainLiteral value) {
      return itsMethodCalls.WasMethodCalledWith("MakeStatement", subject, predicate, value);
    }
    
    public override Statement MakeStatement(UriRef subject, UriRef predicate, TypedLiteral value) {
      itsMethodCalls.RecordMethodCall("MakeStatement", subject, predicate, value);
      return new StatementStub();
    }
    
    public bool WasMakeStatementCalledWith(UriRef subject, UriRef predicate, TypedLiteral value) {
      return itsMethodCalls.WasMethodCalledWith("MakeStatement", subject, predicate, value);
    }
    
    public override Statement MakeStatement(BlankNode subject, UriRef predicate, TypedLiteral value) {
      itsMethodCalls.RecordMethodCall("MakeStatement", subject, predicate, value);
      return new StatementStub();
    }
    
    public bool WasMakeStatementCalledWith(BlankNode subject, UriRef predicate, TypedLiteral value) {
      return itsMethodCalls.WasMethodCalledWith("MakeStatement", subject, predicate, value);
    }

    //~ public override Statement MakeStatement(UriRef subject, UriRef predicate, XmlLiteral value) {
      //~ itsMethodCalls.RecordMethodCall("MakeStatement", subject, predicate, value);
      //~ return new StatementStub();
    //~ }
    
    //~ public bool WasMakeStatementCalledWith(UriRef subject, UriRef predicate, XmlLiteral value) {
      //~ return itsMethodCalls.WasMethodCalledWith("MakeStatement", subject, predicate, value);
    //~ }
    
    //~ public override Statement MakeStatement(BlankNode subject, UriRef predicate, XmlLiteral value) {
      //~ itsMethodCalls.RecordMethodCall("MakeStatement", subject, predicate, value);
      //~ return new StatementStub();
    //~ }
    
    //~ public bool WasMakeStatementCalledWith(BlankNode subject, UriRef predicate, XmlLiteral value) {
      //~ return itsMethodCalls.WasMethodCalledWith("MakeStatement", subject, predicate, value);
    //~ }

    
  }
}
