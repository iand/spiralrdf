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
  using NUnit.Framework;
  using SemPlan.Spiral.Core;
  using SemPlan.Spiral.Expressions;
  using SemPlan.Spiral.Utility;
  using System;  
  using System.Collections;
  
	/// <summary>
	/// Programmer tests for Query class
	/// </summary>
  /// <remarks>
  /// $Id: QueryTest.cs,v 1.6 2006/02/16 15:25:24 ian Exp $
  ///</remarks>
	[TestFixture]
  public class QueryTest {
    
    [Test]
    public void EqualsComparesSelectAll() {
      Query query1 = new Query();
      Query query2 = new Query();
      Query query3 = new Query();
      
      query1.SelectAll = false;
      query2.SelectAll = false;
      query3.SelectAll = true;

      Assert.IsTrue( query1.Equals( query2 ), "Query1 should equal query2" );      
      Assert.IsTrue( ! query1.Equals( query3), "Query1 should not equal query3" );      
    }

    [Test]
    public void GetHashCodeUsesSelectAll() {
      Query query1 = new Query();
      Query query2 = new Query();
      Query query3 = new Query();
      
      query1.SelectAll = false;
      query2.SelectAll = false;
      query3.SelectAll = true;

      Assert.IsTrue( query1.GetHashCode() ==  query2.GetHashCode() , "Query1 should have same hash code as query2" );      
      Assert.IsTrue( query1.GetHashCode() !=  query3.GetHashCode() , "Query1 should not have same hash code as query3" );      
    }

    [Test]
    public void EqualsComparesPatterns() {
      Query query1 = new Query();
      Query query2 = new Query();
      Query query3 = new Query();
      
      query1.AddPattern( new Pattern( new UriRef("http://example.com/subject"), new UriRef("http://example.com/predicate"), new Variable("var") ) );
      query2.AddPattern( new Pattern( new UriRef("http://example.com/subject"), new UriRef("http://example.com/predicate"), new Variable("var") ) );
      query3.AddPattern( new Pattern( new UriRef("http://example.com/other"), new UriRef("http://example.com/predicate"), new Variable("var") ) );

      Assert.IsTrue( query1.Equals( query2 ), "Query1 should equal query2" );      
      Assert.IsTrue( ! query1.Equals( query3), "Query1 should not equal query3" );      
    }

    [Test]
    public void GetHashCodeUsesPatterns() {
      Query query1 = new Query();
      Query query2 = new Query();
      Query query3 = new Query();
      
      query1.AddPattern( new Pattern( new UriRef("http://example.com/subject"), new UriRef("http://example.com/predicate"), new Variable("var") ) );
      query2.AddPattern( new Pattern( new UriRef("http://example.com/subject"), new UriRef("http://example.com/predicate"), new Variable("var") ) );
      query3.AddPattern( new Pattern( new UriRef("http://example.com/other"), new UriRef("http://example.com/predicate"), new Variable("var") ) );

      Assert.IsTrue( query1.GetHashCode() ==  query2.GetHashCode() , "Query1 should have same hash code as query2" );      
      Assert.IsTrue( query1.GetHashCode() !=  query3.GetHashCode() , "Query1 should not have same hash code as query2" );      
    }


    [Test]
    public void EqualsComparesQueryGroup() {
      Query query1 = new Query();
      Query query2 = new Query();
      Query query3 = new Query();
      
      query1.QueryGroup = new QueryGroupOr();
      query2.QueryGroup = new QueryGroupOr();
      query3.QueryGroup = new QueryGroupAnd();

      Assert.IsTrue( query1.Equals( query2 ), "Query1 should equal query2" );      
      Assert.IsTrue( ! query1.Equals( query3), "Query1 should not equal query3" );      
    }

    [Test]
    public void GetHashCodeUsesQueryGroup() {
      Query query1 = new Query();
      Query query2 = new Query();
      Query query3 = new Query();
      
      query1.QueryGroup = new QueryGroupOr();
      query2.QueryGroup = new QueryGroupOr();
      query3.QueryGroup = new QueryGroupAnd();

      Assert.IsTrue( query1.GetHashCode() ==  query2.GetHashCode() , "Query1 should have same hash code as query2" );      
      Assert.IsTrue( query1.GetHashCode() !=  query3.GetHashCode() , "Query1 should not have same hash code as query2" );      
    }


    [Test]
    public void EqualsComparesPatternCount() {
      Query query1 = new Query();
      Query query2 = new Query();
      Query query3 = new Query();
      
      query1.AddPattern( new Pattern( new UriRef("http://example.com/subject"), new UriRef("http://example.com/predicate"), new Variable("var") ) );
      query2.AddPattern( new Pattern( new UriRef("http://example.com/subject"), new UriRef("http://example.com/predicate"), new Variable("var") ) );
      query3.AddPattern( new Pattern( new UriRef("http://example.com/subject"), new UriRef("http://example.com/predicate"), new Variable("var") ) );
      query3.AddPattern( new Pattern( new UriRef("http://example.com/other"), new UriRef("http://example.com/predicate"), new Variable("var") ) );

      Assert.IsTrue( query1.Equals( query2 ), "Query1 should equal query2" );      
      Assert.IsTrue( ! query1.Equals( query3), "Query1 should not equal query3" );      
    }


    [Test]
    public void EqualsComparesOptionalGroup() {
      Query query1 = new Query();
      Query query2 = new Query();
      Query query3 = new Query();
      
      query1.PatternGroup.OptionalGroup.AddPattern( new Pattern( new UriRef("http://example.com/subject"), new UriRef("http://example.com/predicate"), new Variable("var") ) );
      query2.PatternGroup.OptionalGroup.AddPattern( new Pattern( new UriRef("http://example.com/subject"), new UriRef("http://example.com/predicate"), new Variable("var") ) );
      query3.PatternGroup.OptionalGroup.AddPattern( new Pattern( new UriRef("http://example.com/other"), new UriRef("http://example.com/predicate"), new Variable("var") ) );

      Assert.IsTrue( query1.Equals( query2 ), "Query1 should equal query2" );      
      Assert.IsTrue( ! query1.Equals( query3), "Query1 should not equal query3" );      
    }

    [Test]
    public void GetHashCodeUsesOptionalGroup() {
      Query query1 = new Query();
      Query query2 = new Query();
      Query query3 = new Query();
      
      query1.PatternGroup.OptionalGroup.AddPattern( new Pattern( new UriRef("http://example.com/subject"), new UriRef("http://example.com/predicate"), new Variable("var") ) );
      query2.PatternGroup.OptionalGroup.AddPattern( new Pattern( new UriRef("http://example.com/subject"), new UriRef("http://example.com/predicate"), new Variable("var") ) );
      query3.PatternGroup.OptionalGroup.AddPattern( new Pattern( new UriRef("http://example.com/other"), new UriRef("http://example.com/predicate"), new Variable("var") ) );

      Assert.IsTrue( query1.GetHashCode() ==  query2.GetHashCode() , "Query1 should have same hash code as query2" );      
      Assert.IsTrue( query1.GetHashCode() !=  query3.GetHashCode() , "Query1 should not have same hash code as query2" );      
    }

    [Test]
    public void EqualsComparesOptionalPatternCount() {
      Query query1 = new Query();
      Query query2 = new Query();
      Query query3 = new Query();
      
      query1.PatternGroup.OptionalGroup.AddPattern( new Pattern( new UriRef("http://example.com/subject"), new UriRef("http://example.com/predicate"), new Variable("var") ) );
      query2.PatternGroup.OptionalGroup.AddPattern( new Pattern( new UriRef("http://example.com/subject"), new UriRef("http://example.com/predicate"), new Variable("var") ) );
      query3.PatternGroup.OptionalGroup.AddPattern( new Pattern( new UriRef("http://example.com/subject"), new UriRef("http://example.com/predicate"), new Variable("var") ) );
      query3.PatternGroup.OptionalGroup.AddPattern( new Pattern( new UriRef("http://example.com/other"), new UriRef("http://example.com/predicate"), new Variable("var") ) );

      Assert.IsTrue( query1.Equals( query2 ), "Query1 should equal query2" );      
      Assert.IsTrue( ! query1.Equals( query3), "Query1 should not equal query3" );      
    }

    [Test]
    public void EqualsComparesVariables() {
      Query query1 = new Query();
      Query query2 = new Query();
      Query query3 = new Query();
      
      query1.AddVariable( new Variable("var") );
      query2.AddVariable( new Variable("var") );
      query3.AddVariable( new Variable("other") );

      Assert.IsTrue( query1.Equals( query2 ), "Query1 should equal query2" );      
      Assert.IsTrue( ! query1.Equals( query3), "Query1 should not equal query3" );      
    }

    [Test]
    public void GetHashCodeUsesVariables() {
      Query query1 = new Query();
      Query query2 = new Query();
      Query query3 = new Query();
      
      query1.AddVariable( new Variable("var") );
      query2.AddVariable( new Variable("var") );
      query3.AddVariable( new Variable("other") );

      Assert.IsTrue( query1.GetHashCode() ==  query2.GetHashCode() , "Query1 should have same hash code as query2" );      
      Assert.IsTrue( query1.GetHashCode() !=  query3.GetHashCode() , "Query1 should not have same hash code as query2" );      
    }
    
    [Test]
    public void EqualsComparesVariableCounts() {
      Query query1 = new Query();
      Query query2 = new Query();
      Query query3 = new Query();
      
      query1.AddVariable( new Variable("var") );
      query2.AddVariable( new Variable("var") );
      query3.AddVariable( new Variable("var") );
      query3.AddVariable( new Variable("other") );

      Assert.IsTrue( query1.Equals( query2 ), "Query1 should equal query2" );      
      Assert.IsTrue( ! query1.Equals( query3), "Query1 should not equal query3" );      
    }

    [Test]
    public void EqualsComparesConstraints() {
      Query query1 = new Query();
      Query query2 = new Query();
      Query query3 = new Query();
      
      query1.AddConstraint( new Constraint(new VariableExpression( new Variable("var") ) ) );
      query2.AddConstraint( new Constraint( new VariableExpression( new Variable("var") ) ) );
      query3.AddConstraint( new Constraint( new VariableExpression( new Variable("other") ) ) );

      Assert.IsTrue( query1.Equals( query2 ), "Query1 should equal query2" );      
      Assert.IsTrue( ! query1.Equals( query3), "Query1 should not equal query3" );      
    }

    [Test]
    public void GetHashCodeUsesConstraints() {
      Query query1 = new Query();
      Query query2 = new Query();
      Query query3 = new Query();
      
      query1.AddConstraint( new Constraint(new VariableExpression( new Variable("var") ) ) );
      query2.AddConstraint( new Constraint( new VariableExpression( new Variable("var") ) ) );
      query3.AddConstraint( new Constraint( new VariableExpression( new Variable("other") ) ) );

      Assert.IsTrue( query1.GetHashCode() ==  query2.GetHashCode() , "Query1 should have same hash code as query2" );      
      Assert.IsTrue( query1.GetHashCode() !=  query3.GetHashCode() , "Query1 should not have same hash code as query3" );      
    }

    [Test]
    public void EqualsComparesBase() {
      Query query1 = new Query();
      Query query2 = new Query();
      Query query3 = new Query();
      
      query1.Base="http://example.org/a";
      query2.Base="http://example.org/a";
      query3.Base="http://example.org/b";

      Assert.IsTrue( query1.Equals( query2 ), "Query1 should equal query2" );      
      Assert.IsTrue( ! query1.Equals( query3), "Query1 should not equal query3" );      
    }

    [Test]
    public void GetHashCodeUsesBase() {
      Query query1 = new Query();
      Query query2 = new Query();
      Query query3 = new Query();
      
      query1.Base="http://example.org/a";
      query2.Base="http://example.org/a";
      query3.Base="http://example.org/b";

      Assert.IsTrue( query1.GetHashCode() ==  query2.GetHashCode() , "Query1 should have same hash code as query2" );      
      Assert.IsTrue( query1.GetHashCode() !=  query3.GetHashCode() , "Query1 should not have same hash code as query3" );      
    }

    [Test]
    public void EqualsComparesOrderDirection() {
      Query query1 = new Query();
      Query query2 = new Query();
      Query query3 = new Query();
      
      query1.OrderDirection=Query.SortOrder.Ascending;
      query2.OrderDirection=Query.SortOrder.Ascending;
      query3.OrderDirection=Query.SortOrder.Descending;

      Assert.IsTrue( query1.Equals( query2 ), "Query1 should equal query2" );      
      Assert.IsTrue( ! query1.Equals( query3), "Query1 should not equal query3" );      
    }

    [Test]
    public void GetHashCodeUsesOrderDirection() {
      Query query1 = new Query();
      Query query2 = new Query();
      Query query3 = new Query();
      
      query1.OrderDirection=Query.SortOrder.Ascending;
      query2.OrderDirection=Query.SortOrder.Ascending;
      query3.OrderDirection=Query.SortOrder.Descending;

      Assert.IsTrue( query1.GetHashCode() ==  query2.GetHashCode() , "Query1 should have same hash code as query2" );      
      Assert.IsTrue( query1.GetHashCode() !=  query3.GetHashCode() , "Query1 should not have same hash code as query3" );      
    }
    
    [Test]
    public void EqualsComparesOrderBy() {
      Query query1 = new Query();
      Query query2 = new Query();
      Query query3 = new Query();
      
      query1.OrderBy = new VariableExpression( new Variable("x") );
      query2.OrderBy = new VariableExpression( new Variable("x") );
      query3.OrderBy = new VariableExpression( new Variable("y") );

      Assert.IsTrue( query1.Equals( query2 ), "Query1 should equal query2" );      
      Assert.IsTrue( ! query1.Equals( query3), "Query1 should not equal query3" );      
    }

    [Test]
    public void GetHashCodeUsesOrderBy() {
      Query query1 = new Query();
      Query query2 = new Query();
      Query query3 = new Query();
      
      query1.OrderBy = new VariableExpression( new Variable("x") );
      query2.OrderBy = new VariableExpression( new Variable("x") );
      query3.OrderBy = new VariableExpression( new Variable("y") );

      Assert.IsTrue( query1.GetHashCode() ==  query2.GetHashCode() , "Query1 should have same hash code as query2" );      
      Assert.IsTrue( query1.GetHashCode() !=  query3.GetHashCode() , "Query1 should not have same hash code as query3" );      
    }

    [Test]
    public void ResultLimitDefaultsToMaximumIntegerValue() {
      Query query1 = new Query();
      Assert.AreEqual( Int32.MaxValue, query1.ResultLimit, "Query should have default value for ResultLimit" );      
    }

    

    [Test]
    public void EqualsComparesResultLimit() {
      Query query1 = new Query();
      Query query2 = new Query();
      Query query3 = new Query();
      
      query1.ResultLimit = 5;
      query2.ResultLimit = 5;
      query3.ResultLimit = 4;

      Assert.IsTrue( query1.Equals( query2 ), "Query1 should equal query2" );      
      Assert.IsTrue( ! query1.Equals( query3), "Query1 should not equal query3" );      
    }

    [Test]
    public void GetHashCodeUsesResultLimit() {
      Query query1 = new Query();
      Query query2 = new Query();
      Query query3 = new Query();
      
      query1.ResultLimit = 5;
      query2.ResultLimit = 5;
      query3.ResultLimit = 4;

      Assert.IsTrue( query1.GetHashCode() ==  query2.GetHashCode() , "Query1 should have same hash code as query2" );      
      Assert.IsTrue( query1.GetHashCode() !=  query3.GetHashCode() , "Query1 should not have same hash code as query3" );      
    }

    [Test]
    public void EqualsComparesResultOffset() {
      Query query1 = new Query();
      Query query2 = new Query();
      Query query3 = new Query();
      
      query1.ResultOffset = 5;
      query2.ResultOffset = 5;
      query3.ResultOffset = 4;

      Assert.IsTrue( query1.Equals( query2 ), "Query1 should equal query2" );      
      Assert.IsTrue( ! query1.Equals( query3), "Query1 should not equal query3" );      
    }

    [Test]
    public void GetHashCodeUsesResultOffset() {
      Query query1 = new Query();
      Query query2 = new Query();
      Query query3 = new Query();
      
      query1.ResultOffset = 5;
      query2.ResultOffset = 5;
      query3.ResultOffset = 4;

      Assert.IsTrue( query1.GetHashCode() ==  query2.GetHashCode() , "Query1 should have same hash code as query2" );      
      Assert.IsTrue( query1.GetHashCode() !=  query3.GetHashCode() , "Query1 should not have same hash code as query3" );      
    }

    [Test]
    public void VariablesIncludesOnlyExplicitlyAddedVariablesIfNotSelectAll() {
      Query query = new Query();
      query.PatternGroup.AddPattern( new Pattern( new Variable("var"),  new UriRef("http://example.com/predicate"),  new Variable("foo")) );
      query.AddVariable( new Variable("foo") );

      query.SelectAll = false;
      
      IList variables = query.Variables;
      
      Assert.AreEqual( 1, variables.Count, "Should only be one variable");
    }

    [Test]
    public void VariablesIncludesAllVariablesIfSelectAll() {
      Query query = new Query();
      query.PatternGroup.AddPattern( new Pattern( new Variable("var"),  new UriRef("http://example.com/predicate"),  new Variable("foo")) );
      query.AddVariable( new Variable("foo") );

      query.SelectAll = true;
      
      IList variables = query.Variables;
      
      Assert.AreEqual( 2, variables.Count, "Should only be one variable");
    }
    
    
    [Test]
    public void EqualsComparesResultForm() {
      Query query1 = new Query();
      Query query2 = new Query();
      Query query3 = new Query();
      
      query1.ResultForm = Query.ResultFormType.Select;
      query2.ResultForm = Query.ResultFormType.Select;
      query3.ResultForm = Query.ResultFormType.Ask;

      Assert.IsTrue( query1.Equals( query2 ), "Query1 should equal query2" );      
      Assert.IsTrue( ! query1.Equals( query3), "Query1 should not equal query3" );      
    }

    [Test]
    public void GetHashCodeComparesResultForm() {
      Query query1 = new Query();
      Query query2 = new Query();
      Query query3 = new Query();
      
      query1.ResultForm = Query.ResultFormType.Select;
      query2.ResultForm = Query.ResultFormType.Select;
      query3.ResultForm = Query.ResultFormType.Ask;

      Assert.IsTrue( query1.GetHashCode() ==  query2.GetHashCode() , "Query1 should have same hash code as query2" );      
      Assert.IsTrue( query1.GetHashCode() !=  query3.GetHashCode() , "Query1 should not have same hash code as query3" );      
    }
 
    [Test]
    public void EqualsComparesDescribeTerms() {
      Query query1 = new Query();
      Query query2 = new Query();
      Query query3 = new Query();
      
      query1.AddDescribeTerm( new Variable("var") );
      query2.AddDescribeTerm( new Variable("var") );
      query3.AddDescribeTerm( new Variable("other") );

      Assert.IsTrue( query1.Equals( query2 ), "Query1 should equal query2" );      
      Assert.IsTrue( ! query1.Equals( query3), "Query1 should not equal query3" );      
    }

    [Test]
    public void GetHashCodeUsesDescribeTerms() {
      Query query1 = new Query();
      Query query2 = new Query();
      Query query3 = new Query();
      
      query1.AddDescribeTerm( new Variable("var") );
      query2.AddDescribeTerm( new Variable("var") );
      query3.AddDescribeTerm( new Variable("other") );

      Assert.IsTrue( query1.GetHashCode() ==  query2.GetHashCode() , "Query1 should have same hash code as query2" );      
      Assert.IsTrue( query1.GetHashCode() !=  query3.GetHashCode() , "Query1 should not have same hash code as query2" );      
    }


  }  
}