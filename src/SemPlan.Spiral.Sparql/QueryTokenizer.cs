#region Copyright (c) 2005 Ian Davis and James Carlyle
/*------------------------------------------------------------------------------
COPYRIGHT AND PERMISSION NOTICE

Copyright (c) 2005 Ian Davis and James Carlyle

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

namespace SemPlan.Spiral.Sparql {
  using SemPlan.Spiral.Core;
  using System;
  using System.Collections;
  using System.Text.RegularExpressions;
  using System.Globalization;

	/// <summary>
	/// Represents a tokenizer for Sparql graph patterns
	/// </summary>
  /// <remarks> 
  /// $Id: QueryTokenizer.cs,v 1.7 2006/02/10 13:26:28 ian Exp $
  ///</remarks>
  public class QueryTokenizer {
    private string itsInput;
    private TokenType itsTokenType;
    private int itsPosition;
    private int itsLinePosition;
    private string itsTokenText;
    private int itsTokenAbsolutePosition;
    private int itsTokenLinePosition;
    private int itsTokenLine;
    private static CaseInsensitiveComparer theComparer;
    private static Hashtable theKeywords;
   
    public enum TokenType {
      BOF,
      EOF,
      ERROR,
      Comment,
      Variable,
      QuotedIRIRef,
      BlankNode,
      RDFLiteral,
      LiteralLanguage,
      LiteralDatatype,
      NumericInteger,
      NumericDecimal,
      NumericDouble,
      Boolean,
      QName,
      Wildcard,
      BeginGroup,
      EndGroup,
      StatementTerminator,
      ValueDelimiter,
      PredicateDelimiter,
      PrefixName,
      KeywordSelect,
      KeywordPrefix,
      KeywordWhere,
      KeywordA,
      KeywordFilter,
      KeywordStr,
      KeywordLang,
      KeywordDataType,
      KeywordLangMatches,
      KeywordBound,
      KeywordIsIri,
      KeywordIsUri,
      KeywordIsBlank,
      KeywordIsLiteral,
      KeywordRegex,
      KeywordUnion,
      KeywordLimit,
      KeywordOffset,
      KeywordOrder,
      KeywordBy,
      KeywordAsc,
      KeywordDesc,
      KeywordFrom,
      KeywordNamed,
      KeywordBase,
      KeywordDistinct,
      KeywordConstruct,
      KeywordDescribe,
      KeywordAsk,
      KeywordOptional,
      KeywordGraph,
      BeginGeneratedBlankNode,
      EndGeneratedBlankNode,
      Unknown,
    }   

    static QueryTokenizer() {
      CultureInfo usCulture = new CultureInfo("en-US");
      theComparer  = new CaseInsensitiveComparer( usCulture );
      
      CaseInsensitiveHashCodeProvider hashCodeProvider = new CaseInsensitiveHashCodeProvider( usCulture );
      theKeywords = new Hashtable( hashCodeProvider, theComparer );
      
      theKeywords["SELECT"] = TokenType.KeywordSelect;
      theKeywords["TRUE"] = TokenType.Boolean;
      theKeywords["FALSE"] = TokenType.Boolean;
      theKeywords["PREFIX"] = TokenType.KeywordPrefix;
      theKeywords["WHERE"] = TokenType.KeywordWhere;
      theKeywords["FILTER"] = TokenType.KeywordFilter;
      theKeywords["STR"] = TokenType.KeywordStr;
      theKeywords["LANG"] = TokenType.KeywordLang;
      theKeywords["DATATYPE"] = TokenType.KeywordDataType;
      theKeywords["LANGMATCHES"] = TokenType.KeywordLangMatches;
      theKeywords["BOUND"] = TokenType.KeywordBound;
      theKeywords["ISIRI"] = TokenType.KeywordIsIri;
      theKeywords["ISURI"] = TokenType.KeywordIsUri;
      theKeywords["ISBLANK"] = TokenType.KeywordIsBlank;
      theKeywords["ISLITERAL"] = TokenType.KeywordIsLiteral;
      theKeywords["REGEX"] = TokenType.KeywordRegex;
      theKeywords["UNION"] = TokenType.KeywordUnion;
      theKeywords["LIMIT"] = TokenType.KeywordLimit;
      theKeywords["OFFSET"] = TokenType.KeywordOffset;
      theKeywords["ORDER"] = TokenType.KeywordOrder;
      theKeywords["BY"] = TokenType.KeywordBy;
      theKeywords["ASC"] = TokenType.KeywordAsc;
      theKeywords["DESC"] = TokenType.KeywordDesc;
      theKeywords["FROM"] = TokenType.KeywordFrom;
      theKeywords["NAMED"] = TokenType.KeywordNamed;
      theKeywords["BASE"] = TokenType.KeywordBase;
      theKeywords["DISTINCT"] = TokenType.KeywordDistinct;
      theKeywords["CONSTRUCT"] = TokenType.KeywordConstruct;
      theKeywords["DESCRIBE"] = TokenType.KeywordDescribe;
      theKeywords["ASK"] = TokenType.KeywordAsk;
      theKeywords["OPTIONAL"] = TokenType.KeywordOptional;
      theKeywords["GRAPH"] = TokenType.KeywordGraph;
      theKeywords["A"] = TokenType.KeywordA;
      
    }

    public QueryTokenizer(String input) {
      itsInput = input;
      itsTokenType = TokenType.BOF;
      itsPosition = 0;
      itsLinePosition = 0;
      itsTokenLine = 1;
      itsTokenLinePosition = 0;
      itsTokenAbsolutePosition = 0;
    }
      
    ///<summary>Move to the next token</summary>
    ///<returns>True if there is another token, false if there are no more tokens</returns>
    public bool MoveNext() {
      SkipWhitespace();
      itsTokenAbsolutePosition = itsPosition;
      itsTokenLinePosition = itsLinePosition;
      
      int i = PeekChar();

      if (i == -1) {
          itsTokenType = TokenType.EOF;
          return false;
      }


      char ch = (char)i;
      if (ch == '?' || ch == '$') {
        itsTokenType = TokenType.Variable;
        ReadChar();
        itsTokenText = CollectVariableName();
        return true;
      }
      else if (ch == '<') {
        itsTokenType = TokenType.QuotedIRIRef;
        string s = "";

        ReadChar();
        while ((i = ReadChar()) != -1) {
          ch = (char)i;
          if ( ch == '>' ) {
            break;
          }
          s += ch;
        };

        itsTokenText = s;
        return true;

      }
      else if (ch == '@') {
        itsTokenType = TokenType.LiteralLanguage;
        ReadChar();
        string s = CollectUntilWhitespace();
        itsTokenText = s;
        return true;

      }
      else if (ch == '#') {
        itsTokenType = TokenType.Comment;
        ReadChar();
        SkipWhitespace();
        string s = CollectUntilEndOfLine();
        itsTokenText = s;
        return true;
      }
      else if (ch == '^') {
        itsTokenType = TokenType.LiteralDatatype;
        ReadChar();
        i = PeekChar();
        if ( i != -1) {
          if ( (char)i == '^') {
            ReadChar();
            i = PeekChar();
            if ( i != -1) {
              if ( (char)i == '<') {
                string s = "";
        
                ReadChar();
                while ((i = ReadChar()) != -1) {
                  ch = (char)i;
                  if ( ch == '>' ) {
                    break;
                  }
                  s += ch;
                };
        
                itsTokenText = s;
                return true;
              }
            }
          } 
        }          

        itsTokenType = TokenType.ERROR;
        return false;
      }
      else if (ch == '*') {
        return AssignTokenFromCurrentCharacter(TokenType.Wildcard);
      }
      else if (ch == '{' || ch == '(') {
        return AssignTokenFromCurrentCharacter(TokenType.BeginGroup);
      }
      else if (ch == '}' || ch == ')') {
        return AssignTokenFromCurrentCharacter(TokenType.EndGroup);
      }
      else if (ch == '[') {
        return AssignTokenFromCurrentCharacter(TokenType.BeginGeneratedBlankNode);
      }
      else if (ch == ']') {
        return AssignTokenFromCurrentCharacter(TokenType.EndGeneratedBlankNode);
      }
      else if (ch == ';') {
        return AssignTokenFromCurrentCharacter(TokenType.ValueDelimiter);
      }
      else if (ch == ',') {
        return AssignTokenFromCurrentCharacter(TokenType.PredicateDelimiter);
      }
      else if (Char.IsDigit(ch) || ch == '.' || ch == '+' || ch == '-') {
        bool noExponent = true;
        string s = "";
      
        if ( ch == '.') {
          itsTokenType = TokenType.StatementTerminator;
        }
        else {
          itsTokenType = TokenType.NumericInteger;
        }
        
        s += ch;
        ReadChar();
        
        while ((i = ReadChar()) != -1) {
          ch = (char)i;
          if (Char.IsDigit(ch)) {
            if ( itsTokenType == TokenType.StatementTerminator) {
              itsTokenType = TokenType.NumericDecimal;
            }
            s += ch;
          }
          else if ( ch == '.') {
            if ( itsTokenType == TokenType.NumericInteger) {
              itsTokenType = TokenType.NumericDecimal;
            }
            s += ch;
          }
          else if ( noExponent && ch == 'e') {
            noExponent = false;
            itsTokenType = TokenType.NumericDouble;
            s += ch;

            i = PeekChar();
            if ( i != -1) {
              if ( (char)i == '+' || (char)i == '-') {
                s += (char)i;
                ReadChar();
              }
            }

          }
          else {
            break;
          }
        };

        itsTokenText = s;
        return true;

      }
      else if (ch == '\'' || ch == '\"') {
        Char quoteType = ch;
        bool longString = false;
        
        itsTokenType = TokenType.RDFLiteral;
        string s = "";

        ReadChar();
        i = PeekChar();
        if ( i != -1) {
          if ( (char)i == quoteType) {
            ReadChar();
            i = PeekChar();
            if ( i != -1) {
              if ( (char)i == quoteType) {
                ReadChar();
                longString = true;
              }
              else {
                itsTokenType = TokenType.ERROR;
                return false;
              }
            }
          }
        }
        
        
        
        while ((i = ReadChar()) != -1) {
          ch = (char)i;
          if (ch == '\\') {
            i = ReadChar();
            if ( i == -1) {
              break;
            }
            s += (char)i;
          }
          else if ( !longString &&( ch == '\n' || ch == '\r') ) {
            itsTokenType = TokenType.ERROR;
            return false;
          }
          else {
            if ( ch == quoteType ) {
              if (longString) {
                i = PeekChar();
                if ( i != -1) {
                  if ( (char)i == quoteType) {
                    ReadChar();
                    i = PeekChar();
                    if ( i != -1) {
                      if ( (char)i == quoteType) {
                        ReadChar();
                        break;
                      }
                      else {
                        s += quoteType;
                      }
                    }
                    else {
                      itsTokenType = TokenType.ERROR;
                      return false;
                    }
                  }
                }
                else {
                  itsTokenType = TokenType.ERROR;
                  return false;
                }
              }
              else {
                break;
              }
            }
            s += ch;
          }
        };

        itsTokenText = s;
        return true;
      }
      else if (ch == '_') {
        itsTokenType = TokenType.BlankNode;
        ReadChar();
        i = PeekChar();
        if ( i != -1) {
          if ( (char)i == ':') {
            ReadChar();
            string s = CollectUntilWhitespace();
            itsTokenText = s;
            return true;
          } 
        }          

        itsTokenType = TokenType.ERROR;
        return false;

      }
      else {
        string s = CollectUntilWhitespaceOrBrace();

        if ( theKeywords.Contains( s ) ) {
          return AssignTokenFromString(s, (TokenType)theKeywords[s]);
        }
        else if ( s.IndexOf(":") > -1 && s.IndexOf(":") != s.Length - 1) {
          return AssignTokenFromString(s, TokenType.QName);
        }
        else if ( s.IndexOf(":") > -1 ) {
          return AssignTokenFromString(s, TokenType.PrefixName);
        }
        else {
          return AssignTokenFromString(s, TokenType.Unknown);
        }
        
      }
    }    

    ///<returns>The current token type</returns>
    public TokenType Type {
      get { return itsTokenType; }
    }
    
    ///<returns>The current token text</returns>
    public string TokenText {
      get { return itsTokenText; }
    }

    ///<returns>The current token character position in the entire query</returns>
    public int TokenAbsolutePosition {
      get { return itsTokenAbsolutePosition; }
    }

    ///<returns>The current token character position in the current line</returns>
    public int TokenLinePosition {
      get { return itsTokenLinePosition; }
    }
    ///<returns>The line of the query that the current token is on</returns>
    public int TokenLine {
      get { return itsTokenLine; }
    }
    private int PeekChar() {
      if (itsPosition < itsInput.Length) {
        return itsInput[itsPosition];
      } 
      else {
        return -1;
      }
    }

    private int ReadChar() {
      if (itsPosition < itsInput.Length) {
        ++itsLinePosition;
        return itsInput[itsPosition++];
      } 
      else {
        return -1;
      }
    }
  
    private void SkipWhitespace() {
      int ch;

      while ((ch = PeekChar()) != -1) {
        if (!Char.IsWhiteSpace((char)ch)) {
          break;
        }

        ReadChar();

        if ( ch == '\n') {
          itsLinePosition = 0;          
          ++itsTokenLine;
        }
      }
    }

    private string CollectUntilWhitespace() {
      int ch;
      string s = "";

      while ((ch = PeekChar()) != -1) {
        if (Char.IsWhiteSpace((char)ch)) {
          break;
        }
        s += (char)ReadChar();
      }
      
      return s;
      
    }    


    private string CollectVariableName() {
      int ch;
      string s = "";

      while ((ch = PeekChar()) != -1) {
        if (  // TODO: rest of character range
              (char)ch == '_' 
           || ( (char)ch >= 'A' && (char)ch <= 'Z') 
           || ( (char)ch >= 'a' && (char)ch <= 'z' ) 
           || ( (char)ch >= '0' && (char)ch <= '9' ) 
           
           ) {
          s += (char)ReadChar();
        }
        else {
          break;
        }
      }
      
      return s;
      
    }    

    
    private string CollectUntilWhitespaceOrBrace() {
      int ch;
      string s = "";

      while ((ch = PeekChar()) != -1) {
        if (Char.IsWhiteSpace((char)ch) || (char)ch == '{' || (char)ch == '(' || (char)ch == '}' || (char)ch == ')') {
          break;
        }
        s += (char)ReadChar();
      }
      
      return s;
      
    }    

    private string CollectUntilEndOfLine() {
      int ch;
      string s = "";

      while ((ch = PeekChar()) != -1) {
        if ((char)ch == '\n' || (char)ch == '\r') {
          break;
        }
        s += (char)ReadChar();
      }
      
      return s;
      
    }    


    private bool AssignTokenFromCurrentCharacter(TokenType tokenType) {
      itsTokenType = tokenType;
      itsTokenText = "" + (char)ReadChar();
      return true;
    }
    
    private bool AssignTokenFromString(string data, TokenType tokenType) {
      itsTokenType = tokenType;
      itsTokenText = data;
      return true;
    }
 

 
  }
}