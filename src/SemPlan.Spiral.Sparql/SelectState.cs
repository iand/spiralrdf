#region Copyright (c) 2006 Ian Davis and James Carlyle
/*------------------------------------------------------------------------------
COPYRIGHT AND PERMISSION NOTICE

Copyright (c) 2006 Ian Davis and James Carlyle

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
  using SemPlan.Spiral.Core;
  using System;
  using System.Collections;
  using System.Text.RegularExpressions;

	/// <summary>
	/// Represents a parser of the Sparql query syntax
	/// </summary>
  /// <remarks> 
  /// $Id: SelectState.cs,v 1.1 2006/02/10 13:26:28 ian Exp $
  ///</remarks>
  internal class SelectState : State {
    public override State Handle(QueryParser parser, QueryTokenizer tokenizer, Query query) { 
      if (Explain) Console.WriteLine("Entering SELECT state");
      do {
        if (Explain) Console.WriteLine("  tokenizer.TokenText is " + tokenizer.TokenText  + " (" + tokenizer.Type + ")");
        switch (tokenizer.Type) {
          case QueryTokenizer.TokenType.KeywordWhere:
            if ( tokenizer.MoveNext() ) {
              if (tokenizer.TokenText.Equals("{") ) {
                return new WhereState();
              }
            }
            break;
  
          case QueryTokenizer.TokenType.Variable:
            query.AddVariable( new Variable( tokenizer.TokenText )  );
            break;
  
          case QueryTokenizer.TokenType.Wildcard:
            query.SelectAll = true;
            break;
  
          case QueryTokenizer.TokenType.BeginGroup:
            return new WhereState();
        }
      } while ( tokenizer.MoveNext() ) ;

      return this;
    }
  }
}