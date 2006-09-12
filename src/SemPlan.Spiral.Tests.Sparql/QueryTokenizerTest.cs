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

namespace SemPlan.Spiral.Tests.Sparql {
  using NUnit.Framework;
  using SemPlan.Spiral.Core;
  using SemPlan.Spiral.Sparql;
  using System;  
  using System.Collections;
  
	/// <summary>
	/// Programmer tests for QueryTokenizer class
	/// </summary>
  /// <remarks>
  /// $Id: QueryTokenizerTest.cs,v 1.6 2006/02/10 13:26:28 ian Exp $
  ///</remarks>
	[TestFixture]
  public class QueryTokenizerTest {

    private void AssertTokenIsRecognized(string tokenText, QueryTokenizer.TokenType expectedToken) {
      QueryTokenizer tokenizer = new QueryTokenizer(tokenText);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( expectedToken, tokenizer.Type );      
      Assert.AreEqual( tokenText, tokenizer.TokenText );      
    }

    
    [Test]
    public void recognizeVariable() {
      String input = "?var";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.Variable, tokenizer.Type );      
      Assert.AreEqual( "var", tokenizer.TokenText );      
      Assert.AreEqual( false, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.EOF, tokenizer.Type );      
    }

    [Test]
    public void recognizeQuotedIRIRef() {
      String input = "<http://example.com/subj>";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.QuotedIRIRef, tokenizer.Type );      
      Assert.AreEqual( "http://example.com/subj", tokenizer.TokenText );      
    }


    [Test]
    public void recognizeTwoVariables() {
      String input = "?var ?var2";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.Variable, tokenizer.Type );      
      Assert.AreEqual( "var", tokenizer.TokenText );      
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.Variable, tokenizer.Type );      
      Assert.AreEqual( "var2", tokenizer.TokenText );      
      
      Assert.AreEqual( false, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.EOF, tokenizer.Type );      
    }
    [Test]
    public void skipWhitespaceBetweenTokens() {
      String input = "?var1 ?var2  ?var3 \t?var4 \r?var5 \n?var6";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      
      Assert.AreEqual( false, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.EOF, tokenizer.Type );      
    }

    [Test]
    public void recognizeStringLiteral1() {
      String input = "'hello'";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.RDFLiteral, tokenizer.Type );      
      Assert.AreEqual( "hello", tokenizer.TokenText );      
    }

    [Test]
    public void recognizeStringLiteral2() {
      String input = "\"hello\"";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.RDFLiteral, tokenizer.Type );      
      Assert.AreEqual( "hello", tokenizer.TokenText );      
    }

    [Test]
    public void recognizeStringLiteral1EmbeddedQuote() {
      String input = @"'hell\'o'";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.RDFLiteral, tokenizer.Type );      
      Assert.AreEqual(@"hell'o", tokenizer.TokenText );      
    }
    [Test]
    public void recognizeStringLiteral1EmbeddedSlash() {
      String input = @"'hell\\o'";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.RDFLiteral, tokenizer.Type );      
      Assert.AreEqual( @"hell\o", tokenizer.TokenText );      
    }

    [Test]
    public void recognizeStringLiteral2EmbeddedQuote() {
      String input = @"""hell\'o""";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.RDFLiteral, tokenizer.Type );      
      Assert.AreEqual(@"hell'o", tokenizer.TokenText );      
    }
    [Test]
    public void recognizeStringLiteral2EmbeddedSlash() {
      String input = @"""hell\\o""";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.RDFLiteral, tokenizer.Type );      
      Assert.AreEqual( @"hell\o", tokenizer.TokenText );      
    }

    [Test]
    public void recognizeStringLiteral1EmbeddedNewlineReturnsError() {
      String input = "'hell\no'";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( false, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.ERROR, tokenizer.Type );      
    }

    [Test]
    public void recognizeStringLiteral1EmbeddedCarriageReturnReturnsError() {
      String input = "'hell\ro'";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( false, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.ERROR, tokenizer.Type );      
    }

    [Test]
    public void recognizeStringLiteral2EmbeddedNewlineReturnsError() {
      String input = "\"hell\no\"";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( false, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.ERROR, tokenizer.Type );      
    }

    [Test]
    public void recognizeStringLiteral2EmbeddedCarriageReturnReturnsError() {
      String input = "\"hell\ro\"";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( false, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.ERROR, tokenizer.Type );      
    }

    [Test]
    public void recognizeStringLiteralLong1() {
      String input = "'''hello'''";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.RDFLiteral, tokenizer.Type );      
      Assert.AreEqual( "hello", tokenizer.TokenText );      
    }

    [Test]
    public void recognizeTwoStringLiteralLong1() {
      String input = "'''hello''' '''goodbye'''";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.RDFLiteral, tokenizer.Type );      
      Assert.AreEqual( "hello", tokenizer.TokenText );      

      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.RDFLiteral, tokenizer.Type );      
      Assert.AreEqual( "goodbye", tokenizer.TokenText );      

    }
    
    
    [Test]
    public void recognizeStringLiteralLong1MalformedStart() {
      String input = "''hello'''";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( false, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.ERROR, tokenizer.Type );      
    }

    [Test]
    public void recognizeStringLiteralLong1MalformedEndTwoQuotes() {
      String input = "'''recognizeStringLiteralLong1MalformedEnd''";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( false, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.ERROR, tokenizer.Type );      
    }

    [Test]
    public void recognizeStringLiteralLong1MalformedEndOneQuote() {
      String input = "'''recognizeStringLiteralLong1MalformedEnd'";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( false, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.ERROR, tokenizer.Type );      
    }

    [Test]
    public void recognizeStringLiteralLong2() {
      String input = "\"\"\"hello\"\"\"";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.RDFLiteral, tokenizer.Type );      
      Assert.AreEqual( "hello", tokenizer.TokenText );      
    }

    [Test]
    public void recognizeStringLiteralLong1EmbeddedNewlineAllowed() {
      String input = "'''hell\no'''";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.RDFLiteral, tokenizer.Type );      
      Assert.AreEqual( "hell\no", tokenizer.TokenText );      
    }

    [Test]
    public void recognizeStringLiteralLong1EmbeddedCarriageReturnAllowed() {
      String input = "'''hell\ro'''";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.RDFLiteral, tokenizer.Type );      
      Assert.AreEqual( "hell\ro", tokenizer.TokenText );      
    }

    [Test]
    public void recognizeStringLiteralLong2EmbeddedNewlineAllowed() {
      String input = "\"\"\"hell\no\"\"\"";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.RDFLiteral, tokenizer.Type );      
      Assert.AreEqual( "hell\no", tokenizer.TokenText );      
    }

    [Test]
    public void recognizeStringLiteralLong2EmbeddedCarriageReturnAllowed() {
      String input = "\"\"\"hell\ro\"\"\"";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.RDFLiteral, tokenizer.Type );      
      Assert.AreEqual( "hell\ro", tokenizer.TokenText );      
    }

    [Test]
    public void recognizeStringLiteralLong1EmbeddedSingleQuoteAllowed() {
      String input = "'''hell'o'''";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.RDFLiteral, tokenizer.Type );      
      Assert.AreEqual( "hell'o", tokenizer.TokenText );      
    }

    [Test]
    public void recognizeStringLiteralLong1TwoEmbeddedSingleQuotesAllowed() {
      String input = "'''hell''o'''";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.RDFLiteral, tokenizer.Type );      
      Assert.AreEqual( "hell''o", tokenizer.TokenText );      
    }

    [Test]
    public void recognizeStringLiteralLong2EmbeddedDoubleQuoteAllowed() {
      String input = "\"\"\"hell\"o\"\"\"";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.RDFLiteral, tokenizer.Type );      
      Assert.AreEqual( "hell\"o", tokenizer.TokenText );      
    }

    [Test]
    public void recognizeStringLiteralLong2TwoEmbeddedDoubleQuotesAllowed() {
      String input = "\"\"\"hell\"\"o\"\"\"";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.RDFLiteral, tokenizer.Type );      
      Assert.AreEqual( "hell\"\"o", tokenizer.TokenText );      
    }

    [Test]
    public void recognizeIntegerSingleDigit() {
      String input = "1";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.NumericInteger, tokenizer.Type );      
      Assert.AreEqual( "1", tokenizer.TokenText );      
    }
    [Test]
    public void recognizeIntegerMultipleDigit() {
      String input = "123456789";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.NumericInteger, tokenizer.Type );      
      Assert.AreEqual( "123456789", tokenizer.TokenText );      
    }

    [Test]
    public void recognizeNumericDecimal() {
      String input = "1.1";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.NumericDecimal, tokenizer.Type );      
      Assert.AreEqual( "1.1", tokenizer.TokenText );      
    }
    [Test]
    public void recognizeNumericDecimalLeadingPoint() {
      String input = ".1";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.NumericDecimal, tokenizer.Type );      
      Assert.AreEqual( ".1", tokenizer.TokenText );      
    }
    [Test]
    public void recognizeNumericDoubleExponentNoPoint() {
      String input = "1e1";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.NumericDouble, tokenizer.Type );      
      Assert.AreEqual( "1e1", tokenizer.TokenText );      
    }


    [Test]
    public void recognizeNumericDoubleExponentWithPoint() {
      String input = "1.1e1";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.NumericDouble, tokenizer.Type );      
      Assert.AreEqual( "1.1e1", tokenizer.TokenText );      
    }

    [Test]
    public void recognizeNumericDoubleExponentWithLeadingPoint() {
      String input = ".1e1";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.NumericDouble, tokenizer.Type );      
      Assert.AreEqual( ".1e1", tokenizer.TokenText );      
    }

    [Test]
    public void recognizeNumericDoublePositiveExponentNoPoint() {
      String input = "1e+1";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.NumericDouble, tokenizer.Type );      
      Assert.AreEqual( "1e+1", tokenizer.TokenText );      
    }

    [Test]
    public void recognizeNumericDoubleNegativeExponentNoPoint() {
      String input = "1e-1";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.NumericDouble, tokenizer.Type );      
      Assert.AreEqual( "1e-1", tokenizer.TokenText );      
    }

    [Test]
    public void recognizBooleanLiteralTrue() {
      String input = "true";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.Boolean, tokenizer.Type );      
      Assert.AreEqual( "true", tokenizer.TokenText );      
    }

    [Test]
    public void recognizBooleanLiteralTrueCaseInsensitive() {
      String input = "trUe";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.Boolean, tokenizer.Type );      
      Assert.AreEqual( "trUe", tokenizer.TokenText );      
    }

    [Test]
    public void recognizBooleanLiteralFalse() {
      String input = "false";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.Boolean, tokenizer.Type );      
      Assert.AreEqual( "false", tokenizer.TokenText );      
    }

    [Test]
    public void recognizBooleanLiteralFalseCaseInsensitive() {
      String input = "FAlse";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.Boolean, tokenizer.Type );      
      Assert.AreEqual( "FAlse", tokenizer.TokenText );      
    }

/*
    [Test]
    public void recognizeQName() {
      String input = "false";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.QName, tokenizer.Type );      
      Assert.AreEqual( "false", tokenizer.TokenText );      
    }
*/

    [Test]
    public void recognizeSelect() {
      String input = "SELECT";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.KeywordSelect, tokenizer.Type );      
      Assert.AreEqual( "SELECT", tokenizer.TokenText );      
      Assert.AreEqual( false, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.EOF, tokenizer.Type );      
    }

    [Test]
    public void recognizeSelectCaseInsensitive() {
      String input = "sEleCt";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.KeywordSelect, tokenizer.Type );      
      Assert.AreEqual( "sEleCt", tokenizer.TokenText );      
      Assert.AreEqual( false, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.EOF, tokenizer.Type );      
    }


    [Test]
    public void recognizeWhere() {
      String input = "WHERE";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.KeywordWhere, tokenizer.Type );      
      Assert.AreEqual( "WHERE", tokenizer.TokenText );      
      Assert.AreEqual( false, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.EOF, tokenizer.Type );      
    }

    [Test]
    public void recognizeWhereCaseInsensitive() {
      String input = "Where";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.KeywordWhere, tokenizer.Type );      
      Assert.AreEqual( "Where", tokenizer.TokenText );      
      Assert.AreEqual( false, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.EOF, tokenizer.Type );      
    }

    [Test]
    public void recognizePrefix() {
      String input = "PREFIX";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.KeywordPrefix, tokenizer.Type );      
      Assert.AreEqual( "PREFIX", tokenizer.TokenText );      
      Assert.AreEqual( false, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.EOF, tokenizer.Type );      
    }

    [Test]
    public void recognizePrefixCaseInsensitive() {
      String input = "Prefix";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.KeywordPrefix, tokenizer.Type );      
      Assert.AreEqual( "Prefix", tokenizer.TokenText );      
      Assert.AreEqual( false, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.EOF, tokenizer.Type );      
    }


    [Test]
    public void recognizeWildcard() {
      String input = "*";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.Wildcard, tokenizer.Type );      
      Assert.AreEqual( "*", tokenizer.TokenText );      
    }

    [Test]
    public void recognizeBeginGroup() {
      String input = "{";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.BeginGroup, tokenizer.Type );      
      Assert.AreEqual( "{", tokenizer.TokenText );      
    }

    [Test]
    public void recognizeEndGroup() {
      String input = "}";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.EndGroup, tokenizer.Type );      
      Assert.AreEqual( "}", tokenizer.TokenText );      
    }

    [Test]
    public void recognizeStatementTerminator() {
      String input = ".";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.StatementTerminator, tokenizer.Type );      
      Assert.AreEqual( ".", tokenizer.TokenText );      
    }

    [Test]
    public void recognizeValueDelimiter() {
      String input = ";";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.ValueDelimiter, tokenizer.Type );      
      Assert.AreEqual( ";", tokenizer.TokenText );      
    }

    [Test]
    public void recognizePredicateDelimiter() {
      String input = ",";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.PredicateDelimiter, tokenizer.Type );      
      Assert.AreEqual( ",", tokenizer.TokenText );      
    }

 
    [Test]
    public void recognizeLiteralDatatype() {
      String input = "^^<http://example.com/type>";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.LiteralDatatype, tokenizer.Type );      
      Assert.AreEqual( "http://example.com/type", tokenizer.TokenText );      
    }

    [Test]
    public void recognizeKeywordA() {
      String input = "a";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.KeywordA, tokenizer.Type );      
      Assert.AreEqual( "a", tokenizer.TokenText );      
    }

    [Test]
    public void recognizeKeywordACaseInsensitive() {
      String input = "A";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.KeywordA, tokenizer.Type );      
      Assert.AreEqual( "A", tokenizer.TokenText );      
    }


    [Test]
    public void recognizeBlankNode() {
      String input = "_:a";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.BlankNode, tokenizer.Type );      
      Assert.AreEqual( "a", tokenizer.TokenText );      
    }

    [Test]
    public void TokenAbsolutePosition() {
      String input = "?var";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( 0, tokenizer.TokenAbsolutePosition );      
    }

    [Test]
    public void TokenAbsolutePositionAfterWhitespace() {
      String input = "    ?var";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( 4, tokenizer.TokenAbsolutePosition );      
    }

    [Test]
    public void TokenAbsolutePositionForLiteral() {
      String input = "    \"\"\"foo\"\"\"";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( 4, tokenizer.TokenAbsolutePosition );      
    }
    
    [Test]
    public void TokenLineAfterWhitespace() {
      String input = "    ?var";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( 1, tokenizer.TokenLine );      
    }

    [Test]
    public void TokenLineIncrementsForEachNewline() {
      String input = "SELECT\n*\nWHERE";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( 3, tokenizer.TokenLine );      
    }
    
    [Test]
    public void TokenPositions() {
      String input = "SELECT\n  * \nWHERE {?x ?y ?z}";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( 1, tokenizer.TokenLine, "SELECT line number" );      
      Assert.AreEqual( 0, tokenizer.TokenAbsolutePosition, "SELECT absolute character position" );      
      Assert.AreEqual( 0, tokenizer.TokenLinePosition, "SELECT line character position" );      

      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( 2, tokenizer.TokenLine, "* line number" );      
      Assert.AreEqual( 9, tokenizer.TokenAbsolutePosition, "* absolute character position" );      
      Assert.AreEqual( 2, tokenizer.TokenLinePosition, "* line character position" );      

      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( 3, tokenizer.TokenLine, "WHERE line number" );      
      Assert.AreEqual( 12, tokenizer.TokenAbsolutePosition, "WHERE absolute character position" );      
      Assert.AreEqual( 0, tokenizer.TokenLinePosition, "WHERE line character position" );      

      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( 3, tokenizer.TokenLine, "{ line number" );      
      Assert.AreEqual( 18, tokenizer.TokenAbsolutePosition, "{ absolute character position" );      
      Assert.AreEqual( 6, tokenizer.TokenLinePosition, "{ line character position" );      

    }

    [Test]
    public void recognizeStringLiteralWithLanguage1() {
      String input = "'hello'@foo";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.LiteralLanguage, tokenizer.Type );      
      Assert.AreEqual( "foo", tokenizer.TokenText );      
      Assert.AreEqual( false, tokenizer.MoveNext() );      
    }

     [Test]
    public void RecognizeVariableWithDollar() {
      String input = "$var";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.Variable, tokenizer.Type );      
      Assert.AreEqual( "var", tokenizer.TokenText );      
      Assert.AreEqual( false, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.EOF, tokenizer.Type );      
    }

    [Test]
    public void recognizeBeginGroupWithRoundParenthesis() {
      String input = "(";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.BeginGroup, tokenizer.Type );      
      Assert.AreEqual( "(", tokenizer.TokenText );      
    }

    [Test]
    public void recognizeEndGroupWithRoundParenthesis() {
      AssertTokenIsRecognized(")", QueryTokenizer.TokenType.EndGroup);
    }

 
     [Test]
    public void recognizeKeywordFilter() {
      AssertTokenIsRecognized("FILTER", QueryTokenizer.TokenType.KeywordFilter);
      AssertTokenIsRecognized("filTER", QueryTokenizer.TokenType.KeywordFilter);
    }

     [Test]
    public void recognizeKeywordStr() {
      AssertTokenIsRecognized("STR", QueryTokenizer.TokenType.KeywordStr);
      AssertTokenIsRecognized("sTr", QueryTokenizer.TokenType.KeywordStr);
    }

     [Test]
    public void recognizeKeywordLang() {
      AssertTokenIsRecognized("LANG", QueryTokenizer.TokenType.KeywordLang);
      AssertTokenIsRecognized("LanG", QueryTokenizer.TokenType.KeywordLang);
    }
    
    
    [Test]
    public void recognizeKeywordDataType() {
      AssertTokenIsRecognized("DATATYPE", QueryTokenizer.TokenType.KeywordDataType);
      AssertTokenIsRecognized("DaTaTypE", QueryTokenizer.TokenType.KeywordDataType);
    }    

    [Test]
    public void recognizeKeywordLangMatches() {
      AssertTokenIsRecognized("LANGMATCHES", QueryTokenizer.TokenType.KeywordLangMatches);
      AssertTokenIsRecognized("LAngMAtcHES", QueryTokenizer.TokenType.KeywordLangMatches);
    }    

    [Test]
    public void recognizeKeywordBound() {
      AssertTokenIsRecognized("BOUND", QueryTokenizer.TokenType.KeywordBound);
      AssertTokenIsRecognized("bouND", QueryTokenizer.TokenType.KeywordBound);
    }    

    [Test]
    public void recognizeKeywordIsIRI() {
      AssertTokenIsRecognized("isIRI", QueryTokenizer.TokenType.KeywordIsIri);
      AssertTokenIsRecognized("ISiri", QueryTokenizer.TokenType.KeywordIsIri);
    }    

    [Test]
    public void recognizeKeywordIsURI() {
      AssertTokenIsRecognized("isURI", QueryTokenizer.TokenType.KeywordIsUri);
      AssertTokenIsRecognized("ISuri", QueryTokenizer.TokenType.KeywordIsUri);
    }    
    
    [Test]
    public void recognizeKeywordIsBlank() {
      AssertTokenIsRecognized("isBLANK", QueryTokenizer.TokenType.KeywordIsBlank);
      AssertTokenIsRecognized("ISblank", QueryTokenizer.TokenType.KeywordIsBlank);
    }    
    
    [Test]
    public void recognizeKeywordIsLiteral() {
      AssertTokenIsRecognized("isLITERAL", QueryTokenizer.TokenType.KeywordIsLiteral);
      AssertTokenIsRecognized("ISliteral", QueryTokenizer.TokenType.KeywordIsLiteral);
    }    
    
    [Test]
    public void recognizeKeywordRegex() {
      AssertTokenIsRecognized("REGEX", QueryTokenizer.TokenType.KeywordRegex);
      AssertTokenIsRecognized("regeX", QueryTokenizer.TokenType.KeywordRegex);
    }        

    [Test]
    public void recognizeKeywordUnion() {
      AssertTokenIsRecognized("UNION", QueryTokenizer.TokenType.KeywordUnion);
      AssertTokenIsRecognized("UnIon", QueryTokenizer.TokenType.KeywordUnion);
    }        


    [Test]
    public void recognizeKeywordLimit() {
      AssertTokenIsRecognized("LIMIT", QueryTokenizer.TokenType.KeywordLimit);
      AssertTokenIsRecognized("Limit", QueryTokenizer.TokenType.KeywordLimit);
    }        

    [Test]
    public void recognizeKeywordOffset() {
      AssertTokenIsRecognized("OFFSET", QueryTokenizer.TokenType.KeywordOffset);
      AssertTokenIsRecognized("Offset", QueryTokenizer.TokenType.KeywordOffset);
    }        

    [Test]
    public void recognizeKeywordOrder() {
      AssertTokenIsRecognized("ORDER", QueryTokenizer.TokenType.KeywordOrder);
      AssertTokenIsRecognized("Order", QueryTokenizer.TokenType.KeywordOrder);
    }        

    [Test]
    public void recognizeKeywordBy() {
      AssertTokenIsRecognized("BY", QueryTokenizer.TokenType.KeywordBy);
      AssertTokenIsRecognized("By", QueryTokenizer.TokenType.KeywordBy);
    }        

    [Test]
    public void recognizeKeywordAsc() {
      AssertTokenIsRecognized("ASC", QueryTokenizer.TokenType.KeywordAsc);
      AssertTokenIsRecognized("AsC", QueryTokenizer.TokenType.KeywordAsc);
    }      

    [Test]
    public void recognizeKeywordDesc() {
      AssertTokenIsRecognized("DESC", QueryTokenizer.TokenType.KeywordDesc);
      AssertTokenIsRecognized("desc", QueryTokenizer.TokenType.KeywordDesc);
    }      

    [Test]
    public void recognizeKeywordFrom() {
      AssertTokenIsRecognized("FROM", QueryTokenizer.TokenType.KeywordFrom);
      AssertTokenIsRecognized("From", QueryTokenizer.TokenType.KeywordFrom);
    }      

    [Test]
    public void recognizeKeywordNamed() {
      AssertTokenIsRecognized("NAMED", QueryTokenizer.TokenType.KeywordNamed);
      AssertTokenIsRecognized("Named", QueryTokenizer.TokenType.KeywordNamed);
    }      

    [Test]
    public void recognizeKeywordBase() {
      AssertTokenIsRecognized("BASE", QueryTokenizer.TokenType.KeywordBase);
      AssertTokenIsRecognized("Base", QueryTokenizer.TokenType.KeywordBase);
    }      

    [Test]
    public void recognizeKeywordDistinct() {
      AssertTokenIsRecognized("DISTINCT", QueryTokenizer.TokenType.KeywordDistinct);
      AssertTokenIsRecognized("Distinct", QueryTokenizer.TokenType.KeywordDistinct);
    }     

    [Test]
    public void recognizeKeywordConstruct() {
      AssertTokenIsRecognized("CONSTRUCT", QueryTokenizer.TokenType.KeywordConstruct);
      AssertTokenIsRecognized("Construct", QueryTokenizer.TokenType.KeywordConstruct);
    }     

    [Test]
    public void recognizeKeywordDescribe() {
      AssertTokenIsRecognized("DESCRIBE", QueryTokenizer.TokenType.KeywordDescribe);
      AssertTokenIsRecognized("Describe", QueryTokenizer.TokenType.KeywordDescribe);
    }   

    [Test]
    public void recognizeKeywordAsk() {
      AssertTokenIsRecognized("ASK", QueryTokenizer.TokenType.KeywordAsk);
      AssertTokenIsRecognized("Ask", QueryTokenizer.TokenType.KeywordAsk);
    }   

    [Test]
    public void recognizeKeywordOptional() {
      AssertTokenIsRecognized("OPTIONAL", QueryTokenizer.TokenType.KeywordOptional);
      AssertTokenIsRecognized("Optional", QueryTokenizer.TokenType.KeywordOptional);
    }   

    [Test]
    public void recognizeKeywordGraph() {
      AssertTokenIsRecognized("GRAPH", QueryTokenizer.TokenType.KeywordGraph);
      AssertTokenIsRecognized("GraPH", QueryTokenizer.TokenType.KeywordGraph);
    }   

    [Test]
    public void RecognizeBeginGeneratedBlankNode() {
      AssertTokenIsRecognized("[", QueryTokenizer.TokenType.BeginGeneratedBlankNode);
    }   

    [Test]
    public void RecognizeEndGeneratedBlankNode() {
      AssertTokenIsRecognized("]", QueryTokenizer.TokenType.EndGeneratedBlankNode);
    }   

    [Test]
    public void recognizeNumericIntegerWithSign() {
      AssertTokenIsRecognized("+1", QueryTokenizer.TokenType.NumericInteger);
      AssertTokenIsRecognized("-1", QueryTokenizer.TokenType.NumericInteger);
    }   



    [Test]
    public void RecognizeOptionalSequence() {
       String input = "OPTIONAL{:x :y";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.KeywordOptional, tokenizer.Type );      

      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.BeginGroup, tokenizer.Type );      

      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.QName, tokenizer.Type );      

      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.QName, tokenizer.Type );      

      Assert.AreEqual( false, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.EOF, tokenizer.Type );      
   }   

    [Test]
    public void RecognizeBrackettedVariableSequence() {
       String input = "(?o)";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.BeginGroup, tokenizer.Type );      

      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.Variable, tokenizer.Type );      

      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.EndGroup, tokenizer.Type );      

      Assert.AreEqual( false, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.EOF, tokenizer.Type );      

   }   

    [Test]
    public void RecognizeAscSequence() {
       String input = "ASC(?o)";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.KeywordAsc, tokenizer.Type );      

      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.BeginGroup, tokenizer.Type );      

      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.Variable, tokenizer.Type );      

      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.EndGroup, tokenizer.Type );      

      Assert.AreEqual( false, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.EOF, tokenizer.Type );      

   }   


    [Test]
    public void recognizeVariableFollowedByBrace() {
      String input = "?var)";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.Variable, tokenizer.Type );      
      Assert.AreEqual( "var", tokenizer.TokenText );      

      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.EndGroup, tokenizer.Type );      

      Assert.AreEqual( false, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.EOF, tokenizer.Type );      
    }

    [Test]
    public void recognizeComment() {
      String input = "# this is a comment";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.Comment, tokenizer.Type );      
      Assert.AreEqual( "this is a comment", tokenizer.TokenText );      
      Assert.AreEqual( false, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.EOF, tokenizer.Type );      
    }

    [Test]
    public void recognizeQNameFollowedByBrace() {
      String input = ":x}";

      QueryTokenizer tokenizer = new QueryTokenizer(input);
      
      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.QName, tokenizer.Type );      
      Assert.AreEqual( ":x", tokenizer.TokenText );      

      Assert.AreEqual( true, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.EndGroup, tokenizer.Type );      

      Assert.AreEqual( false, tokenizer.MoveNext() );      
      Assert.AreEqual( QueryTokenizer.TokenType.EOF, tokenizer.Type );      
    }


 }  

}