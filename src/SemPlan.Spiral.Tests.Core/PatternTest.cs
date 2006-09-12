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
  using SemPlan.Spiral.Utility;
  using System;  
  using System.Collections;
  
	/// <summary>
	/// Programmer tests for Pattern class
	/// </summary>
  /// <remarks>
  /// $Id: PatternTest.cs,v 1.1 2006/01/20 10:37:44 ian Exp $
  ///</remarks>
	[TestFixture]
  public class PatternTest {
    
    [Test]
    public void EqualsComparesGraphMembers() {      
      Pattern pattern1 = new Pattern( new UriRef("http://example.com/subject"), new UriRef("http://example.com/predicate"), new Variable("var") );
      Pattern pattern2 = new Pattern( new UriRef("http://example.com/subject"), new UriRef("http://example.com/predicate"), new Variable("var") ) ;
      Pattern pattern3 = new Pattern( new UriRef("http://example.com/other"), new UriRef("http://example.com/predicate"), new Variable("var") ) ;

      Assert.IsTrue( pattern1.Equals( pattern2 ), "pattern1 should equal pattern2" );      
      Assert.IsTrue( ! pattern1.Equals( pattern3 ), "pattern1 should not equal pattern3" );      
    }

    [Test]
    public void GetHashCodeUsesGraphMembers() {
      Pattern pattern1 = new Pattern( new UriRef("http://example.com/subject"), new UriRef("http://example.com/predicate"), new Variable("var") );
      Pattern pattern2 = new Pattern( new UriRef("http://example.com/subject"), new UriRef("http://example.com/predicate"), new Variable("var") ) ;
      Pattern pattern3 = new Pattern( new UriRef("http://example.com/other"), new UriRef("http://example.com/predicate"), new Variable("var") ) ;

      Assert.IsTrue( pattern1.GetHashCode() ==  pattern2.GetHashCode() , "pattern1 should have same hash code as pattern2" );      
      Assert.IsTrue( pattern1.GetHashCode() !=  pattern3.GetHashCode() , "pattern1 should not have same hash code as pattern3" );      
    }

    [Test]
    public void EqualsTreatsAllBlankNodesAsEquivilent() {      
      Pattern pattern1 = new Pattern( new BlankNode(), new UriRef("http://example.com/predicate"), new Variable("var") );
      Pattern pattern2 = new Pattern( new BlankNode(), new UriRef("http://example.com/predicate"), new Variable("var") ) ;

      Assert.IsTrue( pattern1.Equals( pattern2 ), "pattern1 should equal pattern2" );      
    }


  }  
}