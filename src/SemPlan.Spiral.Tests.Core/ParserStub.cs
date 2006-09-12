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


namespace SemPlan.Spiral.Core {
  using System;
  using System.IO;
  using System.Xml;


	/// <summary>
	/// An instrumented version of Parser that returns default values
	/// </summary>
  /// <remarks>
  /// $Id: ParserStub.cs,v 1.2 2005/05/26 14:24:30 ian Exp $
  ///</remarks>
  public class ParserStub : Parser {


	/// <summary>
	/// Method used to raise a new event
	/// </summary>
	public void OnNewStatement(Statement s) {
		
	}

	public event StatementHandler NewStatement;

    /// <summary>
    /// Assign the factory the parser must use to create resources
    /// </summary>
    public void SetResourceFactory(ResourceFactory factory) {
    
    }

    /// <summary>
    /// Assign the factory the parser must use to create statements from resources
    /// </summary>
    public void SetStatementFactory(StatementFactory factory) {
    
    }
    
    /// <summary>
    /// Parse the RDF using supplied TextReader and base URI
    /// </summary>
		public void Parse(TextReader reader, string baseUri) {
    
    }

    /// <summary>
    /// Parse the RDF using supplied stream and base URI
    /// </summary>
		public void Parse(Stream stream, string baseUri) {
    
    }

  }
}
