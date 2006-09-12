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
	/// An instrumented version of StatementFactory that counts calls to  methods. 
	/// </summary>
  /// <remarks>
  /// $Id: StatementFactoryCounter.cs,v 1.2 2005/05/26 14:24:30 ian Exp $
  ///</remarks>
  public class StatementFactoryCounter : StatementFactory {
       
    public int MakeStatementUUUCalled = 0;
    public override Statement MakeStatement(UriRef subject, UriRef predicate, UriRef value) {
      ++MakeStatementUUUCalled;
      return new StatementStub();
    }
    
    public int MakeStatementBUUCalled = 0;
    public override Statement MakeStatement(BlankNode subject, UriRef predicate, UriRef value) {
      ++MakeStatementBUUCalled;
      return new StatementStub();
    }
    
    public int MakeStatementUUBCalled = 0;
    public override Statement MakeStatement(UriRef subject, UriRef predicate, BlankNode value) {
      ++MakeStatementUUBCalled;
      return new StatementStub();
    }
    
    public int MakeStatementBUBCalled = 0;
    public override Statement MakeStatement(BlankNode subject, UriRef predicate, BlankNode value) {
      ++MakeStatementBUBCalled;
      return new StatementStub();
    }
    
    public int MakeStatementUUPCalled = 0;
    public override Statement MakeStatement(UriRef subject, UriRef predicate, PlainLiteral value) {
      ++MakeStatementUUPCalled;
      return new StatementStub();
    }
    
    public int MakeStatementBUPCalled = 0;
    public override Statement MakeStatement(BlankNode subject, UriRef predicate, PlainLiteral value) {
     ++MakeStatementBUPCalled;
      return new StatementStub();
    }
    
    public int MakeStatementUUTCalled = 0;
    public override Statement MakeStatement(UriRef subject, UriRef predicate, TypedLiteral value) {
      ++MakeStatementUUTCalled;
      return new StatementStub();
    }
    
    public int MakeStatementBUTCalled = 0;
    public override Statement MakeStatement(BlankNode subject, UriRef predicate, TypedLiteral value) {
      ++MakeStatementBUTCalled;
      return new StatementStub();
    }
    
    //~ public int MakeStatementUUXCalled = 0;
    //~ public override Statement MakeStatement(UriRef subject, UriRef predicate, XmlLiteral value) {
      //~ ++MakeStatementUUXCalled;
      //~ return new StatementStub();
    //~ }
    
    //~ public int MakeStatementBUXCalled = 0;
    //~ public override Statement MakeStatement(BlankNode subject, UriRef predicate, XmlLiteral value) {
      //~ ++MakeStatementBUXCalled;
      //~ return new StatementStub();
    //~ }
    
  }
}
