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
	/// Programmer tests for Node class
	/// </summary>
  /// <remarks>
  /// $Id: TypedLiteralNodeTest.cs,v 1.2 2005/05/26 14:24:30 ian Exp $
  ///</remarks>
	[TestFixture]
  public class TypedLiteralTest {
    
    [Test]
    public void labelIsLexicalValue() {
      TypedLiteral node = new TypedLiteral("whizz", "http://example.com/datatype");
      
      Assert.AreEqual("whizz", node.GetLabel());
    }      


    [Test]
    public void dataTypeIsStored() {
      TypedLiteral node = new TypedLiteral("whizz", "http://example.com/datatype");
      
      Assert.AreEqual("http://example.com/datatype", node.GetDataType());
    }      


     [Test]
     public void neverEqualsNull() {
      TypedLiteral instance = new TypedLiteral("whizz", "http://example.com/datatype");
      
      Assert.IsFalse( instance.Equals(null) );
     }

     [Test]
     public void neverEqualsInstanceOfDifferentClass() {
      TypedLiteral instance = new TypedLiteral("whizz", "http://example.com/datatype");
      Object obj = new Object();
      
      Assert.IsFalse( instance.Equals(obj) );
     }

     [Test]
     public void alwaysEqualsSelf() {
      TypedLiteral instance = new TypedLiteral("whizz", "http://example.com/datatype");
      
      Assert.IsTrue( instance.Equals(instance) );
     }

     [Test]
     public void neverEqualsInstanceOfSameClassWithSameLexicalValueAndDifferentDataType() {
      TypedLiteral instance = new TypedLiteral("whizz", "http://example.com/datatype");
      TypedLiteral other = new TypedLiteral("whizz", "http://example.com/other");
      
      Assert.IsFalse( instance.Equals(other) );
     }

     [Test]
     public void neverEqualsInstanceOfSameClassWithDifferentLexicalValueAndSameDataType() {
      TypedLiteral instance = new TypedLiteral("whizz", "http://example.com/datatype");
      TypedLiteral other = new TypedLiteral("bang", "http://example.com/datatype");
      
      Assert.IsFalse( instance.Equals(other) );
     }

     [Test]
     public void alwaysEqualsInstanceOfSameClassWithSameLexicalValueAndSameDataType() {
      TypedLiteral instance = new TypedLiteral("whizz", "http://example.com/datatype");
      TypedLiteral other = new TypedLiteral("whizz", "http://example.com/datatype");
      
      Assert.IsTrue( instance.Equals(other) );
     }

     [Test]
     public void getHashCodeReturnsDifferentCodeForDifferentInstancesWithDifferentLexicalValues() {
      TypedLiteral instance = new TypedLiteral("whizz", "http://example.com/datatype");
      TypedLiteral other = new TypedLiteral("bang", "http://example.com/datatype");
      
      Assert.IsFalse( instance.GetHashCode() == other.GetHashCode() );
     }

     [Test]
     public void getHashCodeReturnsDifferentCodeForDifferentInstancesWithDifferentDataTypes() {
      TypedLiteral instance = new TypedLiteral("whizz", "http://example.com/datatype");
      TypedLiteral other = new TypedLiteral("whizz", "http://example.com/other");
      
      Assert.IsFalse( instance.GetHashCode() == other.GetHashCode() );
     }

     [Test]
     public void getHashCodeReturnsSameCodeForDifferentInstancesWithSameLexicalValuesAndDataTypes() {
      TypedLiteral instance = new TypedLiteral("whizz", "http://example.com/datatype");
      TypedLiteral other = new TypedLiteral("whizz", "http://example.com/datatype");
      
      Assert.IsTrue( instance.GetHashCode() == other.GetHashCode() );
     }

      [Test]
      public void writeCallsWriterWriteTypedLiteralOnce() {
        RdfWriterCounter writer = new RdfWriterCounter();
        TypedLiteral instance = new TypedLiteral("whizz", "http://example.com/datatype");
        
        instance.Write(writer);
        
        Assert.AreEqual( 1, writer.WriteTypedLiteralCalled );
      }

      [Test]
      public void writeCallsWriterWritePlainLiteralWithCorrectArgumentsWithLanguage() {
        RdfWriterStore writer = new RdfWriterStore();
        TypedLiteral instance = new TypedLiteral("whizz", "http://example.com/datatype");
        
        instance.Write(writer);
        
        Assert.IsTrue( writer.WasWriteTypedLiteralCalledWith("whizz", "http://example.com/datatype") );
      }

    [Test]
    public void matchesCallsMatchesTypedLiteralWithoutLanguageOnSpecifier() {
      ResourceSpecifierStore specifier = new ResourceSpecifierStore();
      TypedLiteral instance = new TypedLiteral("whizz", "http://example.com/type");
      
      instance.Matches( specifier );
      
      Assert.IsTrue( specifier.WasMatchesTypedLiteralCalledWith("whizz", "http://example.com/type") );
    }


    [Test]
    public void matchesReturnsResponseFromMatchesTypedLiterallWithoutLanguageCall() {
      ResourceSpecifierResponder specifierTrue = new ResourceSpecifierResponder(true);
      ResourceSpecifierResponder specifierFalse = new ResourceSpecifierResponder(false);
      TypedLiteral instance = new TypedLiteral("whizz", "http://example.com/type");
      
      Assert.IsTrue( instance.Matches( specifierTrue ));
      Assert.IsFalse( instance.Matches( specifierFalse ));
    }

  }
  
  
  
}