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
	/// An instrumented version of Parser that throws an exception for any of its Parse methods
	/// </summary>
  /// <remarks>
  /// $Id: ParserThrower.cs,v 1.2 2005/05/26 14:24:30 ian Exp $
  ///</remarks>
  public class ParserThrower : Parser {

    public event StatementHandler NewStatement;

    public void OnNewStatement(SemPlan.Spiral.Core.Statement s) {
    
    }
  
    public void SetResourceFactory(SemPlan.Spiral.Core.ResourceFactory factory) {
    
    }
  
    public void SetStatementFactory(SemPlan.Spiral.Core.StatementFactory factory) {
    
    }
  
    public void Parse(TextReader reader, string baseUri) {
      throw new ParserException("This exception is always thrown");
    }
  
    public void Parse(Stream stream, string baseUri) {
      throw new ParserException("This exception is always thrown");
    }
  }
  
  
  
}