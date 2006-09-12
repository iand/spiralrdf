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


namespace SemPlan.Spiral.DriveParser {
  using Drive.Rdf;
  using SemPlan.Spiral.Core;
  using SemPlan.Spiral.Utility;
  using System;
  using System.IO; 
  using System.Xml;

	/// <summary>
	/// Represents a parser using the Drive RDF parsing library
	/// </summary>
  /// <remarks>
  /// $Id: DriveParser.cs,v 1.2 2005/05/26 14:24:30 ian Exp $
  ///</remarks>
  public class DriveParser : Parser {
    private ResourceFactory itsResourceFactory;
    private StatementFactory itsStatementFactory;
    private Dereferencer itsDereferencer;
    
    public event SemPlan.Spiral.Core.StatementHandler NewStatement;
    
    private enum NodeType {
      URIREF, 
      BLANK_NODE, 
      PLAIN_LITERAL,
      TYPED_LITERAL
    };
    
	public void OnNewStatement(SemPlan.Spiral.Core.Statement s) {
		if (NewStatement != null)
			NewStatement(s);
	}

    public DriveParser(ResourceFactory resourceFactory, StatementFactory statementFactory) {
      itsResourceFactory = resourceFactory;
      itsStatementFactory = statementFactory;
      itsDereferencer = new SimpleDereferencer();
    }


    /// <summary>
    /// Assign the factory the parser must use to create resources
    /// </summary>
    public void SetResourceFactory(ResourceFactory resourceFactory) {
      itsResourceFactory = resourceFactory;
    }

    /// <summary>
    /// Assign the factory the parser must use to create statements from resources
    /// </summary>
    public void SetStatementFactory(StatementFactory statementFactory) {
      itsStatementFactory = statementFactory;
    }
    
   /// <summary>
    /// Parse the RDF at the given URI
    /// </summary>
		public void Parse(Uri uri, string baseUri) {
      DereferencerResponse response = itsDereferencer.Dereference(uri);
      if ( response.HasContent ) {
        Stream contentStream = response.Stream;
        this.Parse(contentStream, baseUri);
        contentStream.Close();
      }
		}
    
    /// <summary>
    /// Parse the RDF using the string paramater as a URI
    /// </summary>
		public void Parse(string uri, string baseUri) {
      DereferencerResponse response = itsDereferencer.Dereference(uri);
      if ( response.HasContent ) {
        Stream contentStream = response.Stream;
        this.Parse(contentStream, baseUri);
        contentStream.Close();
      }
    }

    /// <summary>
    /// Parse the RDF using supplied TextReader and base URI
    /// </summary>
		public void Parse(TextReader reader, string baseUri) {
      RdfXmlParser parser = new RdfXmlParser();
      ReadGraph( parser.ParseRdf(  reader, baseUri) );
    }

    /// <summary>
    /// Parse the RDF using supplied XmlReader and base URI
    /// </summary>
		public void Parse(XmlReader reader, string baseUri) {
      RdfXmlParser parser = new RdfXmlParser();
      ReadGraph( parser.ParseRdf(  reader, baseUri) );
    }

    /// <summary>
    /// Parse the RDF using supplied stream and base URI
    /// </summary>
		public void Parse(Stream stream, string baseUri) {
      RdfXmlParser parser = new RdfXmlParser();
      ReadGraph( parser.ParseRdf(  stream, baseUri) );
    }

    public void ReadGraph( IRdfGraph graph ) {
      IRdfEdgeCollection edges = graph.Edges;
      
      for (int index=0; index < edges.Count; ++index) {
        IRdfEdge edge = edges[index];
        
 				OnNewStatement( ParseEdge(edge) );
         
      }
    }

    public Statement ParseEdge(IRdfEdge edge) {
      
      // Nasty hack follows
      UriRef thePredicate = itsResourceFactory.MakeUriRef(edge.ID);
      
      NodeType subjectNodeType;
      NodeType objectNodeType;

      if (edge.ParentNode.ID.StartsWith("blankID:")) {
        subjectNodeType = NodeType.BLANK_NODE;  
      }
      else {
        subjectNodeType = NodeType.URIREF;  
      }

      if (edge.ChildNode is IRdfLiteral) {
        if (((IRdfLiteral)edge.ChildNode).Datatype == "") {
          objectNodeType = NodeType.PLAIN_LITERAL;
        }
        else {
          objectNodeType = NodeType.TYPED_LITERAL;
        }
      }
      else if (edge.ChildNode.ID.StartsWith("blankID:")) {
        objectNodeType = NodeType.BLANK_NODE;  
      }
      else {
        objectNodeType = NodeType.URIREF;  
      }


      if (subjectNodeType == NodeType.BLANK_NODE) {
        BlankNode theSubject = itsResourceFactory.MakeBlankNode("drive" + edge.ParentNode.ID.Substring(8));

        if (objectNodeType == NodeType.BLANK_NODE) {
          BlankNode theObject = itsResourceFactory.MakeBlankNode("drive" + edge.ChildNode.ID.Substring(8));
          return itsStatementFactory.MakeStatement(theSubject, thePredicate, theObject);
        }
        else if  (objectNodeType == NodeType.URIREF) {
          UriRef theObject = itsResourceFactory.MakeUriRef(edge.ChildNode.ID);
          return itsStatementFactory.MakeStatement(theSubject, thePredicate, theObject);
        } 
        else if  (objectNodeType == NodeType.PLAIN_LITERAL) {
          PlainLiteral theObject;
          if ( ((IRdfLiteral)edge.ChildNode).LangID == null ) {
            theObject = itsResourceFactory.MakePlainLiteral( ((IRdfLiteral)edge.ChildNode).Value);
          }
          else {
            theObject = itsResourceFactory.MakePlainLiteral( ((IRdfLiteral)edge.ChildNode).Value, ((IRdfLiteral)edge.ChildNode).LangID);
          }
          return itsStatementFactory.MakeStatement(theSubject, thePredicate, theObject);
        } 
        else if  (objectNodeType == NodeType.TYPED_LITERAL) {
          TypedLiteral theObject = itsResourceFactory.MakeTypedLiteral( ((IRdfLiteral)edge.ChildNode).Value, ((IRdfLiteral)edge.ChildNode).Datatype);
          return itsStatementFactory.MakeStatement(theSubject, thePredicate, theObject);
        } 
       
      }
      else {
        UriRef theSubject = itsResourceFactory.MakeUriRef(edge.ParentNode.ID);        

        if (objectNodeType == NodeType.BLANK_NODE) {
          BlankNode theObject = itsResourceFactory.MakeBlankNode("drive" + edge.ChildNode.ID.Substring(8));
          return itsStatementFactory.MakeStatement(theSubject, thePredicate, theObject);
        }
        else if  (objectNodeType == NodeType.URIREF) {
          UriRef theObject = itsResourceFactory.MakeUriRef(edge.ChildNode.ID);
          return itsStatementFactory.MakeStatement(theSubject, thePredicate, theObject);
        } 
        else if  (objectNodeType == NodeType.PLAIN_LITERAL) {
          PlainLiteral theObject;
          if ( ((IRdfLiteral)edge.ChildNode).LangID == null ) {
            theObject = itsResourceFactory.MakePlainLiteral( ((IRdfLiteral)edge.ChildNode).Value);
          }
          else {
            theObject = itsResourceFactory.MakePlainLiteral( ((IRdfLiteral)edge.ChildNode).Value, ((IRdfLiteral)edge.ChildNode).LangID);
          }
          return itsStatementFactory.MakeStatement(theSubject, thePredicate, theObject);
        } 
        else if  (objectNodeType == NodeType.TYPED_LITERAL) {
          TypedLiteral theObject = itsResourceFactory.MakeTypedLiteral( ((IRdfLiteral)edge.ChildNode).Value, ((IRdfLiteral)edge.ChildNode).Datatype);
          return itsStatementFactory.MakeStatement(theSubject, thePredicate, theObject);
        } 
      }

      return null;
    }

    /// <summary>
    /// Set the Dereferencer to be used to dereference URIs
    /// </summary>
    public void SetDereferencer(Dereferencer dereferencer) {
      itsDereferencer = dereferencer;
    }

  }
}
