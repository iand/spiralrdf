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

namespace SemPlan.Spiral.Utility {
  using SemPlan.Spiral.Core;
  using System;  
  using System.Collections;
  
	/// <summary>
	/// Represents a helper for constructing simple queries
	/// </summary>
  /// <remarks>
  /// $Id: SimpleQueryBuilder.cs,v 1.1 2006/03/07 23:13:27 ian Exp $
  ///</remarks>
  public class SimpleQueryBuilder {
    private Query itsQuery;
    private QueryGroupPatterns itsGroupRequired;
    private QueryGroupPatterns itsGroupOptional;
    private QueryGroupConstraints itsGroupConstraints;
    
    public SimpleQueryBuilder() {
      itsQuery = new Query();
      itsQuery.QueryGroup = new QueryGroupAnd();
    
      itsGroupRequired = new QueryGroupPatterns();
      itsGroupOptional = new QueryGroupPatterns();
      itsGroupConstraints = new QueryGroupConstraints();

      ((QueryGroupAnd)itsQuery.QueryGroup).Add( itsGroupRequired );
      ((QueryGroupAnd)itsQuery.QueryGroup).Add( new QueryGroupOptional( itsGroupOptional ) );
      ((QueryGroupAnd)itsQuery.QueryGroup).Add( itsGroupConstraints );
    }
    
    public Query GetQuery() {
      return itsQuery;
    }

    public void AddPattern(Pattern pattern) {
      itsGroupRequired.Add( pattern );
    }

    public void AddOptional(Pattern pattern) {
      itsGroupOptional.Add( pattern );
    }

    public void AddConstraint(Constraint constraint) {
      itsGroupConstraints.Add( constraint );
    }
  }
}