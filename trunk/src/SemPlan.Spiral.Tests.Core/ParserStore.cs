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
  using System.IO;
  using System.Xml;


	/// <summary>
	/// An instrumented version of Parser that can verify arguments to method calls. 
	/// </summary>
  /// <remarks>
  /// $Id: ParserStore.cs,v 1.2 2005/05/26 14:24:30 ian Exp $
  ///</remarks>
  public class ParserStore : Parser {
    private MethodCallStore itsMethodCalls;

    public ParserStore() {
      itsMethodCalls = new MethodCallStore();
    }

    public void OnNewStatement(Statement s) {
      itsMethodCalls.RecordMethodCall("OnNewStatement", s);
    }

    public bool WasOnNewStatementCalledWith(Statement s) {
      return itsMethodCalls.WasMethodCalledWith("OnNewStatement", s);
    }

  
    public event StatementHandler NewStatement;

    public void SetResourceFactory(ResourceFactory factory) {
      itsMethodCalls.RecordMethodCall("SetResourceFactory", factory);
    }
    public bool WasSetResourceFactoryCalledWith(ResourceFactory factory) {
      return itsMethodCalls.WasMethodCalledWith("SetResourceFactory", factory);
    }

    public void SetStatementFactory(StatementFactory factory) {
      itsMethodCalls.RecordMethodCall("SetStatementFactory", factory);
    }
    public bool WasSetStatementFactoryCalledWith(StatementFactory factory) {
      return itsMethodCalls.WasMethodCalledWith("SetStatementFactory", factory);
    }

		public void Parse(TextReader reader, string baseUri) {
      itsMethodCalls.RecordMethodCall("Parse", reader, baseUri);
    }
    public bool WasParseCalledWith(TextReader reader, string baseUri) {
      return itsMethodCalls.WasMethodCalledWith("Parse", reader, baseUri);
    }

		public void Parse(Stream stream, string baseUri) {
      itsMethodCalls.RecordMethodCall("Parse", stream, baseUri);
    }
    public bool WasParseCalledWith(Stream stream, string baseUri) {
      return itsMethodCalls.WasMethodCalledWith("Parse", stream, baseUri);
    }

  }
}
