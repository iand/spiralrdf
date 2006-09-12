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
	/// Programmer tests for PatternGroup class
	/// </summary>
  /// <remarks>
  /// $Id: PatternGroupTest.cs,v 1.4 2006/02/10 13:26:28 ian Exp $
  ///</remarks>
	[TestFixture]
  public class PatternGroupTest {
    

    [Test]
    public void EqualsComparesPatterns() {
      PatternGroup group1 = new PatternGroup();
      PatternGroup group2 = new PatternGroup();
      PatternGroup group3 = new PatternGroup();
      
      group1.AddPattern( new Pattern( new UriRef("http://example.com/subject"), new UriRef("http://example.com/predicate"), new Variable("var") ) );
      group2.AddPattern( new Pattern( new UriRef("http://example.com/subject"), new UriRef("http://example.com/predicate"), new Variable("var") ) );
      group3.AddPattern( new Pattern( new UriRef("http://example.com/other"), new UriRef("http://example.com/predicate"), new Variable("var") ) );

      Assert.IsTrue( group1.Equals( group2 ), "PatternGroup1 should equal group2" );      
      Assert.IsTrue( ! group1.Equals( group3), "PatternGroup1 should not equal group3" );      
    }

    [Test]
    public void GetHashCodeUsesPatterns() {
      PatternGroup group1 = new PatternGroup();
      PatternGroup group2 = new PatternGroup();
      PatternGroup group3 = new PatternGroup();
      
      group1.AddPattern( new Pattern( new UriRef("http://example.com/subject"), new UriRef("http://example.com/predicate"), new Variable("var") ) );
      group2.AddPattern( new Pattern( new UriRef("http://example.com/subject"), new UriRef("http://example.com/predicate"), new Variable("var") ) );
      group3.AddPattern( new Pattern( new UriRef("http://example.com/other"), new UriRef("http://example.com/predicate"), new Variable("var") ) );

      Assert.IsTrue( group1.GetHashCode() ==  group2.GetHashCode() , "PatternGroup1 should have same hash code as group2" );      
      Assert.IsTrue( group1.GetHashCode() !=  group3.GetHashCode() , "PatternGroup1 should not have same hash code as group2" );      
    }

    [Test]
    public void EqualsComparesPatternCount() {
      PatternGroup group1 = new PatternGroup();
      PatternGroup group2 = new PatternGroup();
      PatternGroup group3 = new PatternGroup();
      
      group1.AddPattern( new Pattern( new UriRef("http://example.com/subject"), new UriRef("http://example.com/predicate"), new Variable("var") ) );
      group2.AddPattern( new Pattern( new UriRef("http://example.com/subject"), new UriRef("http://example.com/predicate"), new Variable("var") ) );
      group3.AddPattern( new Pattern( new UriRef("http://example.com/subject"), new UriRef("http://example.com/predicate"), new Variable("var") ) );
      group3.AddPattern( new Pattern( new UriRef("http://example.com/other"), new UriRef("http://example.com/predicate"), new Variable("var") ) );

      Assert.IsTrue( group1.Equals( group2 ), "PatternGroup1 should equal group2" );      
      Assert.IsTrue( ! group1.Equals( group3), "PatternGroup1 should not equal group3" );      
    }


    [Test]
    public void EqualsComparesAlternateGroups() {
      PatternGroup group1 = new PatternGroup();
      PatternGroup group2 = new PatternGroup();
      PatternGroup group3 = new PatternGroup();
      
      PatternGroup group1a = new PatternGroup();
      PatternGroup group2a = new PatternGroup();
      PatternGroup group3a = new PatternGroup();
      group1a.AddPattern( new Pattern( new UriRef("http://example.com/subject"), new UriRef("http://example.com/predicate"), new Variable("var") ) );
      group2a.AddPattern( new Pattern( new UriRef("http://example.com/subject"), new UriRef("http://example.com/predicate"), new Variable("var") ) );
      group3a.AddPattern( new Pattern( new UriRef("http://example.com/other"), new UriRef("http://example.com/predicate"), new Variable("var") ) );

      group1.AddAlternateGroup( group1a );
      group2.AddAlternateGroup( group2a );
      group3.AddAlternateGroup( group3a );

      Assert.IsTrue( group1.Equals( group2 ), "PatternGroup1 should equal group2" );      
      Assert.IsTrue( ! group1.Equals( group3), "PatternGroup1 should not equal group3" );      
    }

    [Test]
    public void GetHashCodeUsesAlternateGroups() {
      PatternGroup group1 = new PatternGroup();
      PatternGroup group2 = new PatternGroup();
      PatternGroup group3 = new PatternGroup();
      
      PatternGroup group1a = new PatternGroup();
      PatternGroup group2a = new PatternGroup();
      PatternGroup group3a = new PatternGroup();
      group1a.AddPattern( new Pattern( new UriRef("http://example.com/subject"), new UriRef("http://example.com/predicate"), new Variable("var") ) );
      group2a.AddPattern( new Pattern( new UriRef("http://example.com/subject"), new UriRef("http://example.com/predicate"), new Variable("var") ) );
      group3a.AddPattern( new Pattern( new UriRef("http://example.com/other"), new UriRef("http://example.com/predicate"), new Variable("var") ) );

      group1.AddAlternateGroup( group1a );
      group2.AddAlternateGroup( group2a );
      group3.AddAlternateGroup( group3a );

      Assert.IsTrue( group1.GetHashCode() ==  group2.GetHashCode() , "PatternGroup1 should have same hash code as group2" );      
      Assert.IsTrue( group1.GetHashCode() !=  group3.GetHashCode() , "PatternGroup1 should not have same hash code as group2" );      
    }

    [Test]
    public void EqualsComparesAlternateGroupCount() {
      PatternGroup group1 = new PatternGroup();
      PatternGroup group2 = new PatternGroup();
      PatternGroup group3 = new PatternGroup();
      
      PatternGroup group1a = new PatternGroup();
      PatternGroup group2a = new PatternGroup();
      PatternGroup group3a = new PatternGroup();
      PatternGroup group3b = new PatternGroup();
      group1a.AddPattern( new Pattern( new UriRef("http://example.com/subject"), new UriRef("http://example.com/predicate"), new Variable("var") ) );
      group2a.AddPattern( new Pattern( new UriRef("http://example.com/subject"), new UriRef("http://example.com/predicate"), new Variable("var") ) );
      group3a.AddPattern( new Pattern( new UriRef("http://example.com/other"), new UriRef("http://example.com/predicate"), new Variable("var") ) );
      group3b.AddPattern( new Pattern( new UriRef("http://example.com/subject"), new UriRef("http://example.com/predicate"), new Variable("var") ) );

      group1.AddAlternateGroup( group1a );
      group2.AddAlternateGroup( group2a );
      group3.AddAlternateGroup( group3a );
      group3.AddAlternateGroup( group3b );

      Assert.IsTrue( group1.Equals( group2 ), "PatternGroup1 should equal group2" );      
      Assert.IsTrue( ! group1.Equals( group3), "PatternGroup1 should not equal group3" );      
    }


    [Test]
    public void EqualsComparesConstraints() {
      PatternGroup group1 = new PatternGroup();
      PatternGroup group2 = new PatternGroup();
      PatternGroup group3 = new PatternGroup();
      
      group1.AddConstraint( new Constraint( new VariableExpression( new Variable("var") ) ) );
      group2.AddConstraint( new Constraint( new VariableExpression( new Variable("var") ) ) );
      group3.AddConstraint( new Constraint( new VariableExpression( new Variable("other") ) ) );

      Assert.IsTrue( group1.Equals( group2 ), "PatternGroup1 should equal group2" );      
      Assert.IsTrue( ! group1.Equals( group3), "PatternGroup1 should not equal group3" );      
    }

    [Test]
    public void GetHashCodeUsesConstraints() {
      PatternGroup group1 = new PatternGroup();
      PatternGroup group2 = new PatternGroup();
      PatternGroup group3 = new PatternGroup();
      
      group1.AddConstraint( new Constraint( new VariableExpression( new Variable("var") ) ) );
      group2.AddConstraint( new Constraint( new VariableExpression( new Variable("var") ) ) );
      group3.AddConstraint( new Constraint( new VariableExpression( new Variable("other") ) ) );

      Assert.IsTrue( group1.GetHashCode() ==  group2.GetHashCode() , "PatternGroup1 should have same hash code as group2" );      
      Assert.IsTrue( group1.GetHashCode() !=  group3.GetHashCode() , "PatternGroup1 should not have same hash code as group3" );      
    }


     [Test]
    public void EqualsComparesOptionalGroups() {
      PatternGroup group1 = new PatternGroup();
      PatternGroup group2 = new PatternGroup();
      PatternGroup group3 = new PatternGroup();
      
      PatternGroup group1a = new PatternGroup();
      PatternGroup group2a = new PatternGroup();
      PatternGroup group3a = new PatternGroup();
      group1a.AddPattern( new Pattern( new UriRef("http://example.com/subject"), new UriRef("http://example.com/predicate"), new Variable("var") ) );
      group2a.AddPattern( new Pattern( new UriRef("http://example.com/subject"), new UriRef("http://example.com/predicate"), new Variable("var") ) );
      group3a.AddPattern( new Pattern( new UriRef("http://example.com/other"), new UriRef("http://example.com/predicate"), new Variable("var") ) );

      group1.OptionalGroup =  group1a;
      group2.OptionalGroup =  group2a;
      group3.OptionalGroup =  group3a;

      Assert.IsTrue( group1.Equals( group2 ), "PatternGroup1 should equal group2" );      
      Assert.IsTrue( ! group1.Equals( group3), "PatternGroup1 should not equal group3" );      
    }

    [Test]
    public void GetHashCodeUsesOptionalGroups() {
      PatternGroup group1 = new PatternGroup();
      PatternGroup group2 = new PatternGroup();
      PatternGroup group3 = new PatternGroup();
      
      PatternGroup group1a = new PatternGroup();
      PatternGroup group2a = new PatternGroup();
      PatternGroup group3a = new PatternGroup();
      group1a.AddPattern( new Pattern( new UriRef("http://example.com/subject"), new UriRef("http://example.com/predicate"), new Variable("var") ) );
      group2a.AddPattern( new Pattern( new UriRef("http://example.com/subject"), new UriRef("http://example.com/predicate"), new Variable("var") ) );
      group3a.AddPattern( new Pattern( new UriRef("http://example.com/other"), new UriRef("http://example.com/predicate"), new Variable("var") ) );

      group1.OptionalGroup =  group1a;
      group2.OptionalGroup =  group2a;
      group3.OptionalGroup =  group3a;

      Assert.IsTrue( group1.GetHashCode() ==  group2.GetHashCode() , "PatternGroup1 should have same hash code as group2" );      
      Assert.IsTrue( group1.GetHashCode() !=  group3.GetHashCode() , "PatternGroup1 should not have same hash code as group2" );      
    }
    
    [Test]
    public void GetMentionedVariablesExaminesPatternObjects() {
      PatternGroup group1 = new PatternGroup();
      
      group1.AddPattern( new Pattern( new UriRef("http://example.com/subject"), new UriRef("http://example.com/predicate"), new Variable("var") ) );

      IList variables = group1.GetMentionedVariables();
      Assert.AreEqual( 1, variables.Count, "Should be one variable");
      Assert.AreEqual( ((Variable)variables[0]).Name, "var", "Variable is called var");
    }
    
    [Test]
    public void GetMentionedVariablesExaminesPatternPredicates() {
      PatternGroup group1 = new PatternGroup();
      
      group1.AddPattern( new Pattern( new UriRef("http://example.com/subject"), new Variable("var"), new UriRef("http://example.com/object") ) );

      IList variables = group1.GetMentionedVariables();
      Assert.AreEqual( 1, variables.Count, "Should be one variable");
      Assert.AreEqual( ((Variable)variables[0]).Name, "var", "Variable is called var");
    }
   
    [Test]
    public void GetMentionedVariablesExaminesPatternSubjects() {
      PatternGroup group1 = new PatternGroup();
      
      group1.AddPattern( new Pattern( new Variable("var"),  new UriRef("http://example.com/predicate"), new UriRef("http://example.com/object") ) );

      IList variables = group1.GetMentionedVariables();
      Assert.AreEqual( 1, variables.Count, "Should be one variable");
      Assert.AreEqual( ((Variable)variables[0]).Name, "var", "Variable is called var");
    }

    [Test]
    public void GetMentionedVariablesExaminesOptionalPatternGroup() {
      PatternGroup group1 = new PatternGroup();
      
      group1.OptionalGroup.AddPattern( new Pattern( new UriRef("http://example.com/subject"), new Variable("var"), new UriRef("http://example.com/object") ) );

      IList variables = group1.GetMentionedVariables();
      Assert.AreEqual( 1, variables.Count, "Should be one variable");
      Assert.AreEqual( ((Variable)variables[0]).Name, "var", "Variable is called var");
    }

    [Test]
    public void GetMentionedVariablesExaminesAlternatePatternGroups() {
      PatternGroup group1 = new PatternGroup();
      PatternGroup group2 = new PatternGroup();
      
      group2.AddPattern( new Pattern( new UriRef("http://example.com/subject"), new Variable("var"), new UriRef("http://example.com/object") ) );
      group1.AddAlternateGroup( group2 );
      
      IList variables = group1.GetMentionedVariables();
      Assert.AreEqual( 1, variables.Count, "Should be one variable");
      Assert.AreEqual( ((Variable)variables[0]).Name, "var", "Variable is called var");
    }


    [Test]
    public void HasPatternsExaminesAddedPatterns() {
      PatternGroup group1 = new PatternGroup();
      PatternGroup group2 = new PatternGroup();
      
      group1.AddPattern( new Pattern( new UriRef("http://example.com/subject"), new Variable("var"), new UriRef("http://example.com/object") ) );
      
      Assert.AreEqual( true, group1.HasPatterns, "Group should have patterns");
      Assert.AreEqual( false, group2.HasPatterns, "Group should not have patterns");
    }

    [Test]
    public void HasPatternsExaminesOptionalPatternGroup() {
      PatternGroup group1 = new PatternGroup();
      PatternGroup group2 = new PatternGroup();
      
      group1.OptionalGroup.AddPattern( new Pattern( new UriRef("http://example.com/subject"), new Variable("var"), new UriRef("http://example.com/object") ) );
      
      Assert.AreEqual( true, group1.HasPatterns, "Group should have patterns");
      Assert.AreEqual( false, group2.HasPatterns, "Group should not have patterns");
    }

    [Test]
    public void HasPatternsExaminesAlternatePatternGroups() {
      PatternGroup group1 = new PatternGroup();
      PatternGroup group1a = new PatternGroup();
      PatternGroup group2 = new PatternGroup();
      
      group1a.AddPattern( new Pattern( new UriRef("http://example.com/subject"), new Variable("var"), new UriRef("http://example.com/object") ) );
      group1.AddAlternateGroup( group1a );
      
      Assert.AreEqual( true, group1.HasPatterns, "Group should have patterns");
      Assert.AreEqual( false, group2.HasPatterns, "Group should not have patterns");
    }

  }  
}