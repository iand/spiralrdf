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

using MySql.Data.MySqlClient;

namespace SemPlan.Spiral.MySql {
  using System;
  using System.Text;
  using System.Collections;
  using SemPlan.Spiral.Core;
  using System.Data;

  /// <summary>
	/// Represents something that collects all the graph terms used in a query
	/// </summary>
  /// <remarks>
  /// $Id: TermCollector.cs,v 1.1 2006/03/08 22:42:36 ian Exp $
  ///</remarks>
  public class TermCollector : QueryGroupVisitor {
    private Hashtable itsRequiredTerms;
    private Hashtable itsOptionalTerms;
    private int itsOptionalNestingDepth;
    
    public TermCollector(QueryGroup group) {
      itsRequiredTerms = new Hashtable();
      itsOptionalTerms = new Hashtable();
      itsOptionalNestingDepth = 0;
      group.Accept( this );
    }
    
    
    
    public ICollection RequiredTerms {
      get {
        return itsRequiredTerms.Keys;
      }
    }

    public ICollection OptionalTerms {
      get {
        return itsOptionalTerms.Keys;
      }
    }

    public void visit(QueryGroupAnd group) {
      foreach (QueryGroup subgroup in group.Groups) {
        subgroup.Accept( this );
      }
    }
    
    public void visit(QueryGroupConstraints group) {
    
    }
    
    public void visit(QueryGroupOptional group) {
      ++itsOptionalNestingDepth;
      group.Group.Accept( this );
      --itsOptionalNestingDepth;
    }
    
    public void visit(QueryGroupOr group) {
      foreach (QueryGroup subgroup in group.Groups) {
        subgroup.Accept( this );
      }
    }
    
    public void visit(QueryGroupPatterns group) {
    
      foreach (Pattern pattern in group.Patterns) {
        if ( ! ( pattern.GetSubject() is Variable ) ) {
          if (itsOptionalNestingDepth == 0) {
            itsRequiredTerms[ pattern.GetSubject() ] = "y";
          }
          else {
            itsOptionalTerms[ pattern.GetSubject() ] = "y";
          }
        }
        
        if ( ! ( pattern.GetPredicate() is Variable ) ) {
          if (itsOptionalNestingDepth == 0) {
            itsRequiredTerms[ pattern.GetPredicate() ] = "y";
          }
          else {
            itsOptionalTerms[ pattern.GetPredicate() ] = "y";
          }
        }

        if ( ! ( pattern.GetObject() is Variable ) ) {
          if (itsOptionalNestingDepth == 0) {
            itsRequiredTerms[ pattern.GetObject() ] = "y";
          }
          else {
            itsOptionalTerms[ pattern.GetObject() ] = "y";
          }
        }
      }
    }

  }
}