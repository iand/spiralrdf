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
  using System;
  
	/// <summary>
	/// Programmer tests for Constraint class
	/// </summary>
  /// <remarks>
  /// $Id: ConstraintTest.cs,v 1.1 2006/02/10 13:26:28 ian Exp $
  ///</remarks>
	[TestFixture]
  public class ConstraintTest {
    
     [Test]
     public void EffectiveBooleanValueOfFalse() {
      Constraint constraint = new Constraint( new ExpressionStub() );
      Assert.IsFalse( constraint.EffectiveBooleanValue( false ) );
     }

     [Test]
     public void EffectiveBooleanValueOfTrue() {
      Constraint constraint = new Constraint( new ExpressionStub() );
      Assert.IsTrue( constraint.EffectiveBooleanValue( true ) );
     }

     [Test]
     public void EffectiveBooleanValueOfBooleanLiteralFalse() {
      Constraint constraint = new Constraint( new ExpressionStub() );
      Assert.IsFalse( constraint.EffectiveBooleanValue( new TypedLiteral("false", "http://www.w3.org/2001/XMLSchema#boolean") ) );
     }

     [Test]
     public void EffectiveBooleanValueOfBooleanLiteralTrue() {
      Constraint constraint = new Constraint( new ExpressionStub() );
      Assert.IsTrue( constraint.EffectiveBooleanValue( new TypedLiteral("true", "http://www.w3.org/2001/XMLSchema#boolean") ) );
     }

     [Test]
     public void EffectiveBooleanValueOfBooleanLiteralTrueCaseInsensitive() {
      Constraint constraint = new Constraint( new ExpressionStub() );
      Assert.IsTrue( constraint.EffectiveBooleanValue( new TypedLiteral("trUe", "http://www.w3.org/2001/XMLSchema#boolean") ) );
     }
     
     [Test]
     public void EffectiveBooleanValueOfPlainLiteralEmpty() {
      Constraint constraint = new Constraint( new ExpressionStub() );
      Assert.IsFalse( constraint.EffectiveBooleanValue( new PlainLiteral("") ) );
     }
     
     [Test]
     public void EffectiveBooleanValueOfPlainLiteralNotEmpty() {
      Constraint constraint = new Constraint( new ExpressionStub() );
      Assert.IsTrue( constraint.EffectiveBooleanValue( new PlainLiteral("scooby") ) );
     }

     [Test]
     public void EffectiveBooleanValueOfStringLiteralEmpty() {
      Constraint constraint = new Constraint( new ExpressionStub() );
      Assert.IsFalse( constraint.EffectiveBooleanValue( new TypedLiteral("", "http://www.w3.org/2001/XMLSchema#string") ) );
     }
     
    [Test]
    public void EffectiveBooleanValueOfStringLiteralNotEmpty() {
     Constraint constraint = new Constraint( new ExpressionStub() );
     Assert.IsTrue( constraint.EffectiveBooleanValue( new TypedLiteral("scooby", "http://www.w3.org/2001/XMLSchema#string") ) );
    }

     [Test]
     public void EffectiveBooleanValueOfIntegerLiteralZero() {
      Constraint constraint = new Constraint( new ExpressionStub() );
      Assert.IsFalse( constraint.EffectiveBooleanValue( new TypedLiteral("0", "http://www.w3.org/2001/XMLSchema#integer") ) );
     }
     
    [Test]
    public void EffectiveBooleanValueOfIntegerLiteralNonZero() {
     Constraint constraint = new Constraint( new ExpressionStub() );
     Assert.IsTrue( constraint.EffectiveBooleanValue( new TypedLiteral("1", "http://www.w3.org/2001/XMLSchema#integer") ) );
    }

     [Test]
     public void EffectiveBooleanValueOfDecimalLiteralZero() {
      Constraint constraint = new Constraint( new ExpressionStub() );
      Assert.IsFalse( constraint.EffectiveBooleanValue( new TypedLiteral("0", "http://www.w3.org/2001/XMLSchema#decimal") ) );
     }
     
    [Test]
    public void EffectiveBooleanValueOfDecimalLiteralNonZero() {
     Constraint constraint = new Constraint( new ExpressionStub() );
     Assert.IsTrue( constraint.EffectiveBooleanValue( new TypedLiteral("1", "http://www.w3.org/2001/XMLSchema#decimal") ) );
    }

     [Test]
     public void EffectiveBooleanValueOfDoubleLiteralZero() {
      Constraint constraint = new Constraint( new ExpressionStub() );
      Assert.IsFalse( constraint.EffectiveBooleanValue( new TypedLiteral("0", "http://www.w3.org/2001/XMLSchema#double") ) );
     }
     
    [Test]
    public void EffectiveBooleanValueOfDoubleLiteralNonZero() {
     Constraint constraint = new Constraint( new ExpressionStub() );
     Assert.IsTrue( constraint.EffectiveBooleanValue( new TypedLiteral("1", "http://www.w3.org/2001/XMLSchema#double") ) );
    }

     [Test]
     public void EffectiveBooleanValueOfFloatLiteralZero() {
      Constraint constraint = new Constraint( new ExpressionStub() );
      Assert.IsFalse( constraint.EffectiveBooleanValue( new TypedLiteral("0", "http://www.w3.org/2001/XMLSchema#float") ) );
     }
     
    [Test]
    public void EffectiveBooleanValueOfFloatLiteralNonZero() {
     Constraint constraint = new Constraint( new ExpressionStub() );
     Assert.IsTrue( constraint.EffectiveBooleanValue( new TypedLiteral("1", "http://www.w3.org/2001/XMLSchema#float") ) );
    }

  }
  
  
  
}