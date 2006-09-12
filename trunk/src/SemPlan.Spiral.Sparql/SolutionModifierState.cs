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
  using SemPlan.Spiral.Core;
  using System;
  using System.Collections;
  using System.Text.RegularExpressions;

	/// <summary>
	/// Represents a parser of the Sparql query syntax
	/// </summary>
  /// <remarks> 
  /// $Id: SolutionModifierState.cs,v 1.3 2006/02/10 22:10:12 ian Exp $
  ///</remarks>
  internal class SolutionModifierState : State {
    public override State Handle( QueryParser parser, QueryTokenizer tokenizer, Query query ) {
      switch (tokenizer.Type) {
        case QueryTokenizer.TokenType.KeywordOrder:
          if ( tokenizer.MoveNext() ) {
            if (tokenizer.Type == QueryTokenizer.TokenType.KeywordBy ) {
              if ( tokenizer.MoveNext() ) {
                if (tokenizer.Type  == QueryTokenizer.TokenType.KeywordAsc ) {
                  query.OrderDirection = Query.SortOrder.Ascending;
                  if (! tokenizer.MoveNext() ) {
                    throw new SparqlException("Error parsing ORDER BY declaration, missing ordering expression", tokenizer);
                  }
                }
                else if (tokenizer.Type  == QueryTokenizer.TokenType.KeywordDesc )  {                      
                  query.OrderDirection = Query.SortOrder.Descending;
                  if (! tokenizer.MoveNext() ) {
                    throw new SparqlException("Error parsing ORDER BY declaration, missing ordering expression", tokenizer);
                  }
                }
                
                Expression orderExpression = parser.ParseExpression( );
                query.OrderBy = orderExpression;
              }
              else {
                throw new SparqlException("Error parsing ORDER BY declaration, missing ordering expression", tokenizer);
              }
            }
            else {
              throw new SparqlException("Error parsing ORDER BY declaration, expecting BY keyword but encountered '" + tokenizer.TokenText + "'", tokenizer);
            }
          }
          else {
            throw new SparqlException("Error parsing ORDER BY declaration, expecting BY keyword but encountered end of query", tokenizer);
          }
          
          break;
        case QueryTokenizer.TokenType.BeginGroup:
          break;

        case QueryTokenizer.TokenType.KeywordLimit:
          if ( tokenizer.MoveNext() ) {
            if (tokenizer.Type  == QueryTokenizer.TokenType.NumericInteger ) {
              query.ResultLimit = Convert.ToInt32( tokenizer.TokenText );
              break;
            }
            else {
              throw new SparqlException("Error parsing LIMIT declaration, expecting integer but encountered '" + tokenizer.TokenText + "'", tokenizer);
            }
          }
          else {
            throw new SparqlException("Error parsing LIMIT declaration, expecting number but encountered end of query", tokenizer);
          }          

        case QueryTokenizer.TokenType.KeywordOffset:
          if ( tokenizer.MoveNext() ) {
            if (tokenizer.Type  == QueryTokenizer.TokenType.NumericInteger ) {
              query.ResultOffset = Convert.ToInt32( tokenizer.TokenText );
              break;
            }
            else {
              throw new SparqlException("Error parsing OFFSET declaration, expecting integer but encountered '" + tokenizer.TokenText + "'", tokenizer);
            }
          }
          else {
            throw new SparqlException("Error parsing OFFSET declaration, expecting number but encountered end of query", tokenizer);
          }          

      }
      return this;
    }
  }
  


}
