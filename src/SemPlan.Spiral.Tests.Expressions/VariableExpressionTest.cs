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

namespace SemPlan.Spiral.Tests.Expressions {
  using NUnit.Framework;
  using SemPlan.Spiral.Core;
  using SemPlan.Spiral.Expressions;
  using System;  
  using System.Collections;
  
	/// <summary>
	/// Programmer tests for VariableExpression class
	/// </summary>
  /// <remarks>
  /// $Id: VariableExpressionTest.cs,v 1.2 2006/02/13 23:47:52 ian Exp $
  ///</remarks>
	[TestFixture]
  public class VariableExpressionTest {
    
    [Test]
    public void TestEquals() {
      VariableExpression expression1 = new VariableExpression( new Variable("scooby") );
      VariableExpression expression2 = new VariableExpression( new Variable("scooby") );
      VariableExpression expression3 = new VariableExpression( new Variable("velma") );
      
      Assert.IsTrue( expression1.Equals( expression2 ), "expression1 should equal expression2" );      
      Assert.IsTrue( ! expression1.Equals( expression3), "expression1 should not equal expression3" );      
    }

    [Test]
    public void TestGetHashCode() {
      VariableExpression expression1 = new VariableExpression( new Variable("scooby") );
      VariableExpression expression2 = new VariableExpression( new Variable("scooby") );
      VariableExpression expression3 = new VariableExpression( new Variable("velma") );

      Assert.IsTrue( expression1.GetHashCode() ==  expression2.GetHashCode() , "expression1 should have same hash code as expression2" );      
      Assert.IsTrue( expression1.GetHashCode() !=  expression3.GetHashCode() , "expression1 should not have same hash code as expression3" );      
    }

    [Test]
    public void EvaluateReturnsVariableNodeBinding() {
      Variable var = new Variable("scooby");
      VariableExpression constraint = new VariableExpression( var );
      
      Bindings bindings = new Bindings();
      bindings.Bind( new Variable("scooby") , new Resource());
      bindings.BindNode(new Variable("scooby"), new UriRef("http://example.com/scooby") );
      
      Assert.AreEqual(new UriRef("http://example.com/scooby"), constraint.Evaluate( bindings ) );      
    }

  }  
}