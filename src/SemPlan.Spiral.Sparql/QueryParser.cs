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
  using SemPlan.Spiral.Expressions;
  using System;
  using System.Collections;
  using System.Text.RegularExpressions;

	/// <summary>
	/// Represents a parser of the Sparql query syntax
	/// </summary>
  /// <remarks> 
  /// $Id: QueryParser.cs,v 1.9 2006/02/10 22:10:12 ian Exp $
  ///</remarks>
  public class QueryParser {
    private QueryTokenizer itsTokenEnum;
    private Hashtable itsPrefixes;
    private bool itsExplain;
    private State itsState;
    
    public QueryParser() {
      InitializeParserState();
    }

    public bool Explain {
      get {
        return itsExplain;
      }
      set {
        itsExplain = value;
        itsState.Explain = Explain;
      }
    }    

   
    private void InitializeParserState() {
      itsPrefixes = new Hashtable();
      itsState = new PrologState();
    }
    
    public Query Parse( String sparql ) {
      SparqlQuery query = new SparqlQuery();
      ParseInto( query, sparql);
      return query;
    }

    public Query Parse( String sparql, string baseUri ) {
      SparqlQuery query = new SparqlQuery();
      query.Base = baseUri;
      ParseInto( query, sparql);
      return query;
    }
    
    public void ParseInto( Query query, String sparql ) {
      InitializeParserState();
      itsState.Explain = Explain;

      itsTokenEnum = TokenizeQuery( sparql );
      while ( itsTokenEnum.MoveNext() ) {
        string token = (string)itsTokenEnum.TokenText;
        if (Explain) Console.WriteLine("  itsTokenEnum.TokenText is " + token  + " (" + itsTokenEnum.Type + ")");
        
        itsState = itsState.Handle(this, itsTokenEnum, query);
        itsState.Explain = Explain;
      }      

    }

    public void RegisterPrefix( string prefix, string uri) {
      itsPrefixes[prefix] = uri;
    }

    public string GetPrefixUri( string prefix) {
      return (string)itsPrefixes[prefix];
    }

    public bool HasPrefix( string prefix) {
      return itsPrefixes.Contains(prefix);
    }

    public BlankNode GetBlankNode( string label) {
      return new BlankNode();
    }





    private QueryTokenizer TokenizeQuery( string sparql ) {
      return new QueryTokenizer( sparql );
    }

    internal PatternTerm ParseTokenToMember( Query query ) {
      string token = (string)itsTokenEnum.TokenText;
      //if (Explain) Console.WriteLine("Parsing token to member: " + token + " (" + itsTokenEnum.Type + ")");

      switch ( itsTokenEnum.Type ) {
        case QueryTokenizer.TokenType.Variable:
          return new Variable(  token );
    
        case QueryTokenizer.TokenType.QuotedIRIRef:    
          return query.CreateUriRef(  token );

        case QueryTokenizer.TokenType.RDFLiteral:    
          if ( token.StartsWith("\"") && token.EndsWith("\"") ) {
            return new PlainLiteral(  token.Substring(1, token.Length - 2 )  );
          }
          else if ( token.StartsWith("\"") && token.LastIndexOf("@") > token.LastIndexOf("\"") ) {
            return new PlainLiteral(  token.Substring(1, token.LastIndexOf("\"") - 1 ), token.Substring(  token.LastIndexOf("@") + 1) );
          }
          else if ( token.StartsWith("\"") && token.LastIndexOf("^^<") > token.LastIndexOf("\"")  && token.EndsWith(">"))  {
            return new PlainLiteral(  token.Substring(1, token.LastIndexOf("\"") - 1 ), token.Substring(  token.LastIndexOf("^^<") + 3, token.Length -  token.LastIndexOf("^^<") - 3) );
          }
          else if ( token.StartsWith("'") && token.EndsWith("\'") ) {
            return new PlainLiteral(  token.Substring(1, token.Length - 2 )  );
          }
          else if ( token.StartsWith("'") && token.LastIndexOf("@") > token.LastIndexOf("\"") ) {
            return new PlainLiteral(  token.Substring(1, token.LastIndexOf("\'") - 1 ), token.Substring(  token.LastIndexOf("@") + 1) );
          }
          else if ( token.StartsWith("'") && token.LastIndexOf("^^<") > token.LastIndexOf("\'")  && token.EndsWith(">"))  {
            return new PlainLiteral(  token.Substring(1, token.LastIndexOf("\'") - 1 ), token.Substring(  token.LastIndexOf("^^<") + 3, token.Length -  token.LastIndexOf("^^<") - 3) );
          }
          else {
            return new PlainLiteral(  itsTokenEnum.TokenText  );
          }

        case QueryTokenizer.TokenType.QName:    
          string prefix = token.Substring( 0, token.IndexOf( ":" ) + 1 );
          string localPart = token.Substring( token.IndexOf( ":" ) + 1);
      
          if (! HasPrefix( prefix ) ) {
            throw new SparqlException("Error parsing prefix '" + prefix + "' at character "  + itsTokenEnum.TokenAbsolutePosition + ". This prefix has not been declared.");
          }
          return query.CreateUriRef(  GetPrefixUri( prefix ) + localPart );

        case QueryTokenizer.TokenType.PrefixName: 
          return query.CreateUriRef(  GetPrefixUri( token ));

        case QueryTokenizer.TokenType.NumericInteger:    
          return new TypedLiteral(  itsTokenEnum.TokenText, "http://www.w3.org/2001/XMLSchema#integer"  );

        case QueryTokenizer.TokenType.NumericDecimal:    
          return new TypedLiteral(  itsTokenEnum.TokenText, "http://www.w3.org/2001/XMLSchema#decimal"  );

        case QueryTokenizer.TokenType.NumericDouble:    
          return new TypedLiteral(  itsTokenEnum.TokenText, "http://www.w3.org/2001/XMLSchema#double"  );

        case QueryTokenizer.TokenType.Boolean:    
          return new TypedLiteral(  itsTokenEnum.TokenText, "http://www.w3.org/2001/XMLSchema#boolean"  );
          
        case QueryTokenizer.TokenType.KeywordA:    
          return new UriRef(  "http://www.w3.org/1999/02/22-rdf-syntax-ns#type"  );
          
        case QueryTokenizer.TokenType.BlankNode:    
          return GetBlankNode( itsTokenEnum.TokenText );
          
      }

      throw new SparqlException("Unknown token '" + token  + " (" + itsTokenEnum.Type + ")" + "' found at character "  + itsTokenEnum.TokenAbsolutePosition);
    }
    
     internal Expression ParseExpression( ) {
      Expression expression = null;
      int groupDepth = 0;
      do {
        switch ( itsTokenEnum.Type ) {
          case QueryTokenizer.TokenType.Variable:
            expression = new VariableExpression( new Variable(  itsTokenEnum.TokenText ) );
            if ( groupDepth <= 0) return expression;
            break;
          case QueryTokenizer.TokenType.BeginGroup:
            ++groupDepth;
            break;
          case QueryTokenizer.TokenType.EndGroup:
            --groupDepth;
            if ( groupDepth <= 0) return expression;
            break;
        }
      } while ( itsTokenEnum.MoveNext() );
      
      return expression;
    }
   

  
    
    private class EndOfQueryState : State {
      public override bool IsEnd {
        get { return true; }
      }

      public override State Handle(QueryParser parser, QueryTokenizer tokenizer, Query query) { 
        return this;
      }
      
    }


  }
}