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
	/// Represents an object capable of parsing RDF from a variety of sources
	/// </summary>
  /// <remarks>
  /// $Id: Parser.cs,v 1.2 2005/05/26 14:24:30 ian Exp $
  ///</remarks>
  public interface Parser {

    event StatementHandler NewStatement;
  
  
    /// <summary>
    /// Raise NewStatement event
    /// </summary>
    void OnNewStatement(SemPlan.Spiral.Core.Statement s);
  
    /// <summary>
    /// Assign the factory the parser must use to create resources
    /// </summary>
    void SetResourceFactory(ResourceFactory factory);
  
    /// <summary>
    /// Assign the factory the parser must use to create statements from resources
    /// </summary>
    void SetStatementFactory(StatementFactory factory);
  
    /// <summary>
    /// Parse the RDF using supplied TextReader and base URI
    /// </summary>
    void Parse(TextReader reader, string baseUri);

    /// <summary>
    /// Parse the RDF using supplied stream and base URI
    /// </summary>
    void Parse(Stream stream, string baseUri);

  }
}
