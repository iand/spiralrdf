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
	/// Programmer tests for QueryGroupConstraints class
	/// </summary>
  /// <remarks>
  /// $Id: QueryGroupConstraintsTest.cs,v 1.1 2006/02/16 15:25:24 ian Exp $
  ///</remarks>
	[TestFixture]
  public class QueryGroupConstraintsTest {

    [Test]
    public void EqualsComparesGroups() {
      QueryGroupConstraints group1 = new QueryGroupConstraints();
      QueryGroupConstraints group2 = new QueryGroupConstraints();
      QueryGroupConstraints group3 = new QueryGroupConstraints();
      
      group1.Add( new Constraint( new VariableExpression( new Variable("foo") ) ) );
      group2.Add( new Constraint( new VariableExpression( new Variable("foo") ) ) );
      group3.Add( new Constraint( new VariableExpression( new Variable("bar") ) ) );

      Assert.IsTrue( group1.Equals( group2 ), "group1 should equal group2" );      
      Assert.IsTrue( ! group1.Equals( group3), "group1 should not equal query3" );      
    }

    [Test]
    public void GetHashCodeUsesGroups() {
      QueryGroupConstraints group1 = new QueryGroupConstraints();
      QueryGroupConstraints group2 = new QueryGroupConstraints();
      QueryGroupConstraints group3 = new QueryGroupConstraints();
      
      group1.Add( new Constraint( new VariableExpression( new Variable("foo") ) ) );
      group2.Add( new Constraint( new VariableExpression( new Variable("foo") ) ) );
      group3.Add( new Constraint( new VariableExpression( new Variable("bar") ) ) );

      Assert.IsTrue( group1.GetHashCode() ==  group2.GetHashCode() , "group1 should have same hash code as group2" );      
      Assert.IsTrue( group1.GetHashCode() !=  group3.GetHashCode() , "group1 should not have same hash code as query2" );      
    }



    
  
  }  

  


  
}