#region Copyright (c) 2006 Ian Davis and James Carlyle
/*-------------------s-----------------------------------------------------------
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

namespace SemPlan.Spiral.Sparql {
  using SemPlan.Spiral.Core;
  using System;
  using System.Collections;
  using System.Text;
  using System.Xml;

	/// <summary>
	/// Represents query formed from the Sparql syntax
	/// </summary>
  /// <remarks> 
  /// $Id: SparqlResultsFormatBuilder.cs,v 1.1 2006/01/26 14:42:00 ian Exp $
  ///</remarks>
  public class SparqlResultsFormatBuilder {

    public XmlDocument Build(IEnumerator solutions, SparqlQuery query) {
      
      XmlDocument doc = new XmlDocument();
      XmlElement rootElement = doc.CreateElement("", "sparql", "http://www.w3.org/2005/sparql-results#");
      doc.AppendChild( rootElement );
     
      XmlElement headElement = doc.CreateElement("", "head", "http://www.w3.org/2005/sparql-results#");
      rootElement.AppendChild( headElement );
      foreach (Variable variable in query.Variables) {
        XmlElement variableElement = doc.CreateElement("", "variable", "http://www.w3.org/2005/sparql-results#");
        XmlAttribute variableNameAttribute = doc.CreateAttribute("", "name", "");
        variableNameAttribute.Value = variable.Name;
        variableElement.SetAttributeNode(variableNameAttribute);
        headElement.AppendChild( variableElement );
      }
      
      XmlElement resultsElement = doc.CreateElement("", "results", "http://www.w3.org/2005/sparql-results#");
      XmlAttribute orderedAttribute = doc.CreateAttribute("", "ordered", "");
      if (query.IsOrdered) {
        orderedAttribute.Value="true";
      }
      else {
        orderedAttribute.Value="false";
      }
      resultsElement.SetAttributeNode( orderedAttribute );

      XmlAttribute distinctAttribute = doc.CreateAttribute("", "distinct", "");
      if (query.IsDistinct) {
        distinctAttribute.Value="true";
      }
      else {
        distinctAttribute.Value="false";
      }
      resultsElement.SetAttributeNode( distinctAttribute );

      rootElement.AppendChild( resultsElement );
     
      while (solutions.MoveNext() ) {
        QuerySolution solution = (QuerySolution)solutions.Current;
        XmlElement resultElement = doc.CreateElement("", "result", "http://www.w3.org/2005/sparql-results#");

        resultsElement.AppendChild( resultElement );
        foreach (Variable variable in query.Variables) {
          XmlElement bindingElement = doc.CreateElement("", "binding", "http://www.w3.org/2005/sparql-results#");
          XmlAttribute bindingNameAttribute = doc.CreateAttribute("", "name", "");
          bindingNameAttribute.Value = variable.Name;
          bindingElement.SetAttributeNode( bindingNameAttribute );
          resultElement.AppendChild( bindingElement );
          
          XmlElement elementNodeType;

            if ( solution.IsBound( variable.Name ) ) {
            GraphMember member = solution.GetNode( variable.Name );
            
            if ( member is UriRef) {
              elementNodeType = doc.CreateElement("", "uri", "http://www.w3.org/2005/sparql-results#");
            }
            else if ( member is BlankNode) {
              elementNodeType = doc.CreateElement("", "bnode", "http://www.w3.org/2005/sparql-results#");
            }
            else if ( member is PlainLiteral) {
              elementNodeType = doc.CreateElement("", "literal", "http://www.w3.org/2005/sparql-results#");
              if (null != ((PlainLiteral)member).GetLanguage()) {
                XmlAttribute languageAttribute = doc.CreateAttribute("xml", "lang", "http://www.w3.org/XML/1998/namespace");
                languageAttribute.Value = ((PlainLiteral)member).GetLanguage();
                elementNodeType.SetAttributeNode( languageAttribute );
              }
            }
            else if ( member is TypedLiteral) {
              elementNodeType = doc.CreateElement("", "literal", "http://www.w3.org/2005/sparql-results#");
              XmlAttribute datatypeAttribute = doc.CreateAttribute("", "datatype", "");
              datatypeAttribute.Value = ((TypedLiteral)member).GetDataType();
              elementNodeType.SetAttributeNode( datatypeAttribute );
            }
            else {
              throw new ApplicationException("Unsupported graph member type");
            }
            XmlText valueText = doc.CreateTextNode(  solution.GetNode( variable.Name ).GetLabel() );         
            elementNodeType.AppendChild( valueText );
          }
          else {
            elementNodeType = doc.CreateElement("", "unbound", "http://www.w3.org/2005/sparql-results#");
          }          
          bindingElement.AppendChild( elementNodeType );
        }
      }
      
      
      return doc;
    }
  }
  
}