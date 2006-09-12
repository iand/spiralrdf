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

namespace SemPlan.Spiral.Tests.Core {
  using NUnit.Framework;
  using SemPlan.Spiral.Core;
  using SemPlan.Spiral.Expressions;
  using SemPlan.Spiral.Utility;
  using System;  
  using System.Collections;
  
	/// <summary>
	/// Programmer tests for Bindings class
	/// </summary>
  /// <remarks>
  /// $Id: BindingsTest.cs,v 1.1 2006/02/13 23:44:11 ian Exp $
  ///</remarks>
	[TestFixture]
  public class BindingsTest {
    
    [Test]
    public void EqualsComparesBindingVariableNames() {
      Bindings bindings1 = new Bindings();
      Bindings bindings2 = new Bindings();
      Bindings bindings3 = new Bindings();
      
      
      bindings1.Bind( new Variable("foo"), new UriRef("foo:foo") );
      bindings2.Bind( new Variable("foo"), new UriRef("foo:foo") );
      bindings3.Bind( new Variable("foo"), new UriRef("foo:bar") );

      Assert.IsTrue( bindings1.Equals( bindings2 ), "bindings1 should equal bindings2" );      
      Assert.IsTrue( ! bindings1.Equals( bindings3), "bindings1 should not equal bindings3" );      
    }

    [Test]
    public void EqualsComparesNodeBindingVariableNames() {
      Bindings bindings1 = new Bindings();
      Bindings bindings2 = new Bindings();
      Bindings bindings3 = new Bindings();
      
      
      bindings1.BindNode( new Variable("foo"), new UriRef("foo:foo") );
      bindings2.BindNode( new Variable("foo"), new UriRef("foo:foo") );
      bindings3.BindNode( new Variable("foo"), new UriRef("foo:bar") );

      Assert.IsTrue( bindings1.Equals( bindings2 ), "bindings1 should equal bindings2" );      
      Assert.IsTrue( ! bindings1.Equals( bindings3), "bindings1 should not equal bindings3" );      
    }


  }  
}