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
  using System;
  
	/// <summary>
	/// Programmer tests for BlankNode class
	/// </summary>
  /// <remarks>
  /// $Id: BlankNodeTest.cs,v 1.3 2006/01/10 12:26:52 ian Exp $
  ///</remarks>
	[TestFixture]
  public class BlankNodeTest {
    
     [Test]
     public void blankNodeNeverEqualsNull() {
      BlankNode instance = new BlankNode();
      Assert.IsFalse( instance.Equals(null) );
     }


     [Test]
     public void  blankNodeNeverEqualsInstanceOfDifferentClass() {
      BlankNode instance = new BlankNode();
      Object obj = new Object();
      
      Assert.IsFalse( instance.Equals(obj) );
     }

     [Test]
     public void blankNodeAlwaysEqualsSelf() {
      BlankNode instance = new BlankNode();
      
      Assert.IsTrue( instance.Equals(instance) );
     }

     [Test]
     public void blankNodeNeverEqualsInstanceOfSameClassWithoutNodeId() {
      BlankNode instance = new BlankNode();
      BlankNode other = new BlankNode();
      
      Assert.IsFalse( instance.Equals(other) );
     }

     [Test]
     public void getHashCodeReturnsDifferentCodeForDifferentInstances() {
      BlankNode instance = new BlankNode();
      BlankNode other = new BlankNode();
      
      Assert.IsFalse( instance.GetHashCode() == other.GetHashCode() );
     }

      [Test]
      public void writeCallsWriterWriteBlankNodeWithSyntheticNodeLabel() {
        RdfWriterStore writer = new RdfWriterStore();
        BlankNode instance = new BlankNode();
        
        instance.Write(writer);
        
        Assert.IsTrue( writer.WasWriteBlankNodeCalledWith( "spiral" + instance.GetHashCode()) );
      }

    [Test]
    public void matchesCallsMatchesBlankNodeOnSpecifier() {
      ResourceSpecifierStore specifier = new ResourceSpecifierStore();
      BlankNode instance = new BlankNode();
      
      instance.Matches( specifier );
      
      Assert.IsTrue( specifier.WasMatchesBlankNodeCalled() );
    }

    [Test]
    public void matchesReturnsResponseFromMatchesUriRefCall() {
      ResourceSpecifierResponder specifierTrue = new ResourceSpecifierResponder(true);
      ResourceSpecifierResponder specifierFalse = new ResourceSpecifierResponder(false);
      BlankNode instance = new BlankNode();
      
      Assert.IsTrue( instance.Matches( specifierTrue ));
      Assert.IsFalse( instance.Matches( specifierFalse ));
    }


  }
  
  
  
}