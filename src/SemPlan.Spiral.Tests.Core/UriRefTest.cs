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
	/// Programmer tests for UriRef class
	/// </summary>
  /// <remarks>
  /// $Id: UriRefTest.cs,v 1.2 2005/05/26 14:24:30 ian Exp $
  ///</remarks>
	[TestFixture]
  public class UriRefTest {
    
    [Test]
    public void labelIsUriRefStringValue() {
      UriRef obj = new UriRef("http://example.com/subject");
      
      Assert.AreEqual("http://example.com/subject", obj.GetLabel());
    }      

     
     [Test]
     public void neverEqualsNull() {
      UriRef instance = new UriRef("http://example.com/subject");
      
      Assert.IsFalse( instance.Equals(null) );
     }

     [Test]
     public void neverEqualsInstanceOfDifferentClass() {
      UriRef instance = new UriRef("http://example.com/subject");
      Object obj = new Object();
      
      Assert.IsFalse( instance.Equals(obj) );
     }

     [Test]
     public void alwaysEqualsSelf() {
      UriRef instance = new UriRef("http://example.com/subject");
      
      Assert.IsTrue( instance.Equals(instance) );
     }


     [Test]
     public void neverEqualsInstanceOfSameClassButDifferentUriRef() {
      UriRef instance = new UriRef("http://example.com/subject");
      UriRef other = new UriRef("http://example.com/other");
      
      Assert.IsFalse( instance.Equals(other) );
     }

     [Test]
     public void alwaysEqualsInstanceOfSameClassWithSameUriRef() {
      UriRef instance = new UriRef("http://example.com/subject");
      UriRef other = new UriRef("http://example.com/subject");
      
      Assert.IsTrue( instance.Equals(other) );
     }




    [Test]
    public void getHashCodeReturnsDifferentCodeForDifferentInstancesWithDifferentUriRef() {
      UriRef instance = new UriRef("http://example.com/subject");
      UriRef other = new UriRef("http://example.com/other");
      
      Assert.IsFalse( instance.GetHashCode() == other.GetHashCode() );
    }


    [Test]
    public void getHashCodeReturnsSameCodeForDifferentInstancesWithSameUriRef() {
      UriRef instance = new UriRef("http://example.com/subject");
      UriRef other = new UriRef("http://example.com/subject");
      
      Assert.IsTrue( instance.GetHashCode() == other.GetHashCode() );
    }

    [Test]
     public void writeCallsWriterWriteUriRefOnce() {
      RdfWriterCounter writer = new RdfWriterCounter();
      UriRef instance = new UriRef("http://example.com/subject");
        
      instance.Write(writer);
      
      Assert.AreEqual( 1, writer.WriteUriRefCalled );
    }

    [Test]
    public void writeCallsWriterWriteUriRefWithCorrectArgument() {
      RdfWriterStore writer = new RdfWriterStore();
      UriRef instance = new UriRef("http://example.com/subject");
      
      instance.Write(writer);
      
      Assert.IsTrue( writer.WasWriteUriRefCalledWith("http://example.com/subject") );
    }

    [Test]
    public void matchesCallsMatchesUriRefOnSpecifier() {
      ResourceSpecifierStore specifier = new ResourceSpecifierStore();
      UriRef instance = new UriRef("http://example.com/subject");
      
      instance.Matches( specifier );
      
      Assert.IsTrue( specifier.WasMatchesUriRefCalledWith("http://example.com/subject") );
    }

    [Test]
    public void matchesReturnsResponseFromMatchesUriRefCall() {
      ResourceSpecifierResponder specifierTrue = new ResourceSpecifierResponder(true);
      ResourceSpecifierResponder specifierFalse = new ResourceSpecifierResponder(false);
      UriRef instance = new UriRef("http://example.com/subject");
      
      Assert.IsTrue( instance.Matches( specifierTrue ));
      Assert.IsFalse( instance.Matches( specifierFalse ));
    }

  }
  
  
  
}