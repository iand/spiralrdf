#region Copyright (c) 2006 Ian Davis and James Carlyle
/*-------------------s-----------------------------------------------------------
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
  using SemPlan.Spiral.Utility;
  using System;
  using System.Collections;
  using System.Text;
  using System.Xml;

	/// <summary>
	/// Represents query formed from the Sparql syntax
	/// </summary>
  /// <remarks> 
  /// $Id: SparqlQuery.cs,v 1.4 2006/02/10 13:26:28 ian Exp $
  ///</remarks>
  public class SparqlQuery : Query {
    private const string DEFAULT_BASE = "http://localhost/";
    private BoundingStrategy itsBoundingStrategy;
    
    public SparqlQuery() {
      Base = DEFAULT_BASE;
      itsBoundingStrategy = new CbdBoundingStrategy();
    }
    
    public SparqlQuery(string sparql, string baseUri) {
      Base = baseUri;
      itsBoundingStrategy = new CbdBoundingStrategy();
      QueryParser parser = new QueryParser();
      parser.ParseInto( this, sparql);
    }

    public SparqlQuery(string sparql) : this( sparql, DEFAULT_BASE) {

    }
    
    public BoundingStrategy BoundingStrategy {
      get { return itsBoundingStrategy; }
      set { itsBoundingStrategy = value; }
    }
    
    public override string ToString() {
      StringBuilder buffer = new StringBuilder();
      buffer.Append("BASE ");
      buffer.Append( new UriRef( Base ) );
      buffer.Append( Environment.NewLine );
      
      switch (ResultForm) {
        case ResultFormType.Select:
          buffer.Append("SELECT ");
          if ( IsDistinct ) {
            buffer.Append("DISTINCT ");
          }
          
          if ( SelectAll ) {
            buffer.Append("* ");
          }
          else {
            foreach (Variable variable in Variables) {
              buffer.Append( variable);
              buffer.Append(" ");
            }
          }
          break;
        case ResultFormType.Describe:
          buffer.Append("DESCRIBE ");
          if ( SelectAll ) {
            buffer.Append("* ");
          }
          else {
            foreach (PatternTerm term in DescribeTerms) {
              buffer.Append( term);
              buffer.Append(" ");
            }
          }
          break;
        case ResultFormType.Ask:
          buffer.Append("ASK ");
          break;
        case ResultFormType.Construct:
          buffer.Append("CONSTRUCT ");
          break;
      }

      if ( HasPatterns ) {
        buffer.Append("WHERE ");
        buffer.Append( PatternGroup );
      }
      
      if ( IsOrdered ) {
        buffer.Append( " ORDER BY ");
        if ( OrderDirection == SortOrder.Descending) {
          buffer.Append("DESC ( ");
          buffer.Append( OrderBy );
          buffer.Append(")");
        }
        else {
          buffer.Append( OrderBy );
        }
      }
      
      if ( ResultLimit != Int32.MaxValue ) {
        buffer.Append( " LIMIT ");
        buffer.Append( ResultLimit );
      }

      if ( ResultOffset != 0 ) {
        buffer.Append( " OFFSET ");
        buffer.Append( ResultOffset );
      }
      
      return buffer.ToString();
    }
    
 
    public TripleStore ExecuteTripleStore(TripleStore defaultStore) {
      MemoryTripleStore results = new MemoryTripleStore();

      if (HasPatterns) {
        IEnumerator solutions = defaultStore.Solve( this );
        while (solutions.MoveNext()) {
          QuerySolution solution = (QuerySolution)solutions.Current;
          foreach (PatternTerm term in DescribeTerms) {
            if ( term is Variable ) {
              ResourceDescription description = defaultStore.GetDescriptionOf( solution[ ((Variable)term).Name ], this.BoundingStrategy);
              results.Add( description );
            }
          }          
        }
      }
      foreach (PatternTerm term in DescribeTerms) {
        if ( term is UriRef ) {
          ResourceDescription description = defaultStore.GetDescriptionOf( (UriRef)term,  this.BoundingStrategy);
          results.Add( description );
        }
        else {
          if (! HasPatterns) {
            throw new SparqlException("DESCRIBE query has no patterns but requests description of a variable");
          }
        }
      }

      return results;
    }

    public bool ExecuteBoolean(TripleStore defaultStore) {
      throw new NotImplementedException();
    }
    
    public IEnumerator ExecuteEnumerator(TripleStore defaultStore) {
      return defaultStore.Solve( this );
    }
 
    public XmlDocument ExecuteXml(TripleStore defaultStore) {
      SparqlResultsFormatBuilder builder = new SparqlResultsFormatBuilder();
      return builder.Build(ExecuteEnumerator( defaultStore ) , this );
    }
  }
  
}