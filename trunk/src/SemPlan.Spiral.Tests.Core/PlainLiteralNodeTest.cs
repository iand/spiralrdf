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
  /// $Id: PlainLiteralNodeTest.cs,v 1.2 2005/05/26 14:24:30 ian Exp $
  ///</remarks>
	[TestFixture]
  public class PlainLiteralTest {
    
    [Test]
    public void labelIsLexicalValue() {
      PlainLiteral node = new PlainLiteral("whizz");
      
      Assert.AreEqual("whizz", node.GetLabel());
    }      

    [Test]
    public void languageSpecifiedInConstructor() {
      PlainLiteral node = new PlainLiteral("whizz", "fr");
      
      Assert.AreEqual("fr", node.GetLanguage());
    }      
    
    [Test]
    public void languageDefaultsToNull() {
      PlainLiteral node = new PlainLiteral("whizz");
      
      Assert.AreEqual(null, node.GetLanguage());
    }      


     [Test]
     public void neverEqualsNull() {
      PlainLiteral instance = new PlainLiteral("whizz");
      
      Assert.IsFalse( instance.Equals(null) );
     }

     [Test]
     public void neverEqualsInstanceOfDifferentClass() {
      PlainLiteral instance = new PlainLiteral("whizz");
      Object obj = new Object();
      
      Assert.IsFalse( instance.Equals(obj) );
     }

     [Test]
     public void alwaysEqualsSelf() {
      PlainLiteral instance = new PlainLiteral("whizz");
      
      Assert.IsTrue( instance.Equals(instance) );
     }


     [Test]
     public void neverEqualsInstanceOfSameClassButDifferentLexicalValueAndNoLanguage() {
      PlainLiteral instance = new PlainLiteral("whizz");
      PlainLiteral other = new PlainLiteral("bang");
      
      Assert.IsFalse( instance.Equals(other) );
     }

     [Test]
     public void alwaysEqualsInstanceOfSameClassWithSameLexicalValueAndNoLanguage() {
      PlainLiteral instance = new PlainLiteral("whizz");
      PlainLiteral other = new PlainLiteral("whizz");
      
      Assert.IsTrue( instance.Equals(other) );
     }

     [Test]
     public void neverEqualsInstanceOfSameClassWithSameLexicalValueAndDifferentLanguage() {
      PlainLiteral instance = new PlainLiteral("whizz", "en");
      PlainLiteral other = new PlainLiteral("whizz", "fr");
      
      Assert.IsFalse( instance.Equals(other) );
     }


     [Test]
     public void alwaysEqualsInstanceOfSameClassWithSameLexicalValueAndSameLanguage() {
      PlainLiteral instance = new PlainLiteral("whizz", "en");
      PlainLiteral other = new PlainLiteral("whizz", "en");
      
      Assert.IsTrue( instance.Equals(other) );
     }


     [Test]
     public void getHashCodeReturnsDifferentCodeForDifferentInstancesWithDifferentLexicalValues() {
      PlainLiteral instance = new PlainLiteral("whizz");
      PlainLiteral other = new PlainLiteral("bang");
      
      Assert.IsFalse( instance.GetHashCode() == other.GetHashCode() );
     }


   [Test]
   public void getHashCodeReturnsSameCodeForDifferentInstancesWithSameLexicalValuesAndNoLanguage() {
    PlainLiteral instance = new PlainLiteral("whizz");
    PlainLiteral other = new PlainLiteral("whizz");
    
    Assert.IsTrue( instance.GetHashCode() == other.GetHashCode() );
   }

   [Test]
   public void getHashCodeReturnsDifferentCodeForDifferentInstancesWithSameLexicalValuesButDifferentLanguages() {
    PlainLiteral instance = new PlainLiteral("whizz", "en");
    PlainLiteral other = new PlainLiteral("whizz", "fr");
    
    Assert.IsFalse( instance.GetHashCode() == other.GetHashCode() );
   }

   [Test]
   public void getHashCodeReturnsSameCodeForDifferentInstancesWithSameLexicalValuesAndSameLanguage() {
    PlainLiteral instance = new PlainLiteral("whizz", "en");
    PlainLiteral other = new PlainLiteral("whizz", "en");
    
    Assert.IsTrue( instance.GetHashCode() == other.GetHashCode() );
   }
   
   
    [Test]
    public void writeCallsWriterWritePlainLiteralOnceWithNoLanguage() {
      RdfWriterCounter writer = new RdfWriterCounter();
       PlainLiteral instance = new PlainLiteral("whizz");
      
      instance.Write(writer);
      
      Assert.AreEqual( 1, writer.WritePlainLiteralCalled );
    }
   
    [Test]
    public void writeCallsWriterWritePlainLiteralOnceWithLanguage() {
      RdfWriterCounter writer = new RdfWriterCounter();
      PlainLiteral instance = new PlainLiteral("whizz", "en");
      
      instance.Write(writer);
      
      Assert.AreEqual( 1, writer.WritePlainLiteralLanguageCalled );
    }

    [Test]
    public void writeCallsWriterWritePlainLiteralWithCorrectArgumentsWithNoLanguage() {
      RdfWriterStore writer = new RdfWriterStore();
      PlainLiteral instance = new PlainLiteral("whizz");
      
      instance.Write(writer);
      
      Assert.IsTrue( writer.WasWritePlainLiteralCalledWith("whizz") );
    }


    [Test]
    public void writeCallsWriterWritePlainLiteralWithCorrectArgumentsWithLanguage() {
      RdfWriterStore writer = new RdfWriterStore();
      PlainLiteral instance = new PlainLiteral("whizz", "gr");
      
      instance.Write(writer);
      
      Assert.IsTrue( writer.WasWritePlainLiteralCalledWith("whizz", "gr") );
    }



    [Test]
    public void matchesCallsMatchesPlainLiteralWithoutLanguageOnSpecifier() {
      ResourceSpecifierStore specifier = new ResourceSpecifierStore();
      PlainLiteral instance = new PlainLiteral("whizz");
      
      instance.Matches( specifier );
      
      Assert.IsTrue( specifier.WasMatchesPlainLiteralCalledWith("whizz") );
    }

    [Test]
    public void matchesCallsMatchesPlainLiteralWithLanguageOnSpecifier() {
      ResourceSpecifierStore specifier = new ResourceSpecifierStore();
      PlainLiteral instance = new PlainLiteral("whizz", "de");
      
      instance.Matches( specifier );
      
      Assert.IsTrue( specifier.WasMatchesPlainLiteralCalledWith("whizz", "de") );
    }

    [Test]
    public void matchesReturnsResponseFromMatchesPlainLiterallWithoutLanguageCall() {
      ResourceSpecifierResponder specifierTrue = new ResourceSpecifierResponder(true);
      ResourceSpecifierResponder specifierFalse = new ResourceSpecifierResponder(false);
      PlainLiteral instance = new PlainLiteral("whizz");
      
      Assert.IsTrue( instance.Matches( specifierTrue ));
      Assert.IsFalse( instance.Matches( specifierFalse ));
    }

    [Test]
    public void matchesReturnsResponseFromMatchesPlainLiterallWithLanguageCall() {
      ResourceSpecifierResponder specifierTrue = new ResourceSpecifierResponder(true);
      ResourceSpecifierResponder specifierFalse = new ResourceSpecifierResponder(false);
      PlainLiteral instance = new PlainLiteral("whizz", "de");
      
      Assert.IsTrue( instance.Matches( specifierTrue ));
      Assert.IsFalse( instance.Matches( specifierFalse ));
    }
  }
  
  
}