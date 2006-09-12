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

namespace SemPlan.Spiral.Sparql {
  using System;
	/// <summary>
	/// Represents an exception message thrown by the sparql query parser
	/// </summary>
  /// <remarks> 
  /// $Id: SparqlException.cs,v 1.2 2006/01/20 10:37:44 ian Exp $
  ///</remarks>
  public class SparqlException : ApplicationException {
    private int itsPositionInQuery;
    private int itsPositionInLine;
    private int itsLineInQuery;
    private string itsTokenText;
  
		public SparqlException (string message) : base (message) {}
		public SparqlException (string message, Exception e) : base (message, e) {}

		public SparqlException (string message, QueryTokenizer tokenizer) : base (message) {
      itsPositionInQuery = tokenizer.TokenAbsolutePosition;
      itsPositionInLine = tokenizer.TokenLinePosition;
      itsLineInQuery = tokenizer.TokenLine;
    }

		public SparqlException (string message, QueryTokenizer tokenizer, Exception e) : base (message, e) {
      itsPositionInQuery = tokenizer.TokenAbsolutePosition;
      itsPositionInLine = tokenizer.TokenLinePosition;
      itsLineInQuery = tokenizer.TokenLine;
      itsTokenText = tokenizer.TokenText;
    }

    public int PositionInQuery {
      get { return itsPositionInQuery; }
    }

    public int PositionInLine {
      get { return itsPositionInLine; }
    }

    public int LineInQuery {
      get { return itsLineInQuery; }
    }

    public string TokenText {
      get { return itsTokenText; }
    }
  }
}