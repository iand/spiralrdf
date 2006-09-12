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


namespace SemPlan.Spiral.Utility {
  using SemPlan.Spiral.Core;
  using System;
  using System.Collections;
  using System.Xml;
	/// <summary>
	/// Represents a writer that outputs triples in the RDF/XML format
	/// </summary>
  /// <remarks>
  /// $Id: RdfXmlWriter.cs,v 1.2 2005/05/26 14:24:31 ian Exp $
  ///</remarks>
  public class RdfXmlWriter : RdfWriter {
    private const string RDF_NAMESPACE_URI = "http://www.w3.org/1999/02/22-rdf-syntax-ns#";

    private XmlWriter itsXmlWriter;
    private QualifiedName itsCurrentPredicateQualifiedName;
    
    
    private Hashtable itsNamespacePrefixes;
    private Hashtable itsUsedNamespaces;
    private int itsNextNamespaceIndex;

    private Hashtable itsMappedNodeIds;
    private int itsNextNodeIdIndex;

    private Hashtable itsBufferedSubjects;
    private Subject itsCurrentSubject;
    
    
    private enum WriterState {
      Waiting, InSubject, InPredicate, InObject
    }
    private WriterState itsState;

    public RdfXmlWriter(XmlWriter writer) {
      itsXmlWriter = writer;
      itsState = WriterState.Waiting;
      itsNamespacePrefixes = new Hashtable();

      itsNamespacePrefixes[RDF_NAMESPACE_URI] = "rdf";
      itsNamespacePrefixes["http://xmlns.com/foaf/0.1/"] = "foaf";
      itsNamespacePrefixes["http://xmlns.com/wot/0.1/"] = "wot";
      itsNamespacePrefixes["http://www.w3.org/2000/01/rdf-schema#"] = "rdfs";
      itsNamespacePrefixes["http://www.w3.org/2002/07/owl#"] = "owl";
      itsNamespacePrefixes["http://purl.org/vocab/bio/0.1/"] = "bio";
      itsNamespacePrefixes["http://purl.org/dc/elements/1.1/"] = "dc";
      itsNamespacePrefixes["http://purl.org/dc/terms/"] = "dct";
      itsNamespacePrefixes["http://web.resource.org/cc/"] = "cc";
      itsNamespacePrefixes["http://purl.org/vocab/relationship/"] = "rel";
      itsNamespacePrefixes["http://www.w3.org/2003/01/geo/wgs84_pos#"] = "geo";
      itsNamespacePrefixes["http://purl.org/rss/1.0/"] = "rss";
    
      itsUsedNamespaces = new Hashtable();

      itsNextNamespaceIndex = 1;

      itsMappedNodeIds = new Hashtable();
      itsNextNodeIdIndex = 1;
      
      itsBufferedSubjects = new Hashtable();
      itsCurrentSubject = null;
      itsCurrentPredicateQualifiedName = null;
    }

    public void StartOutput() {
      itsXmlWriter.WriteStartDocument(true);
    }
    
    public void EndOutput() {
      itsXmlWriter.WriteStartElement( (string)itsNamespacePrefixes[RDF_NAMESPACE_URI], "RDF", RDF_NAMESPACE_URI);
      foreach ( string ns in itsUsedNamespaces.Keys) {
        itsXmlWriter.WriteAttributeString("xmlns",(string)itsNamespacePrefixes[ns], null, ns);

      }
      foreach (Subject subject in itsBufferedSubjects.Values) {
        subject.Write(this, itsXmlWriter);
      }
      itsXmlWriter.WriteEndElement(); // rdf:RDF
      itsXmlWriter.WriteEndDocument();
    }

    public void StartSubject() {
      itsState = WriterState.InSubject;
    }
    
    public void EndSubject() {
      itsState = WriterState.Waiting;
    }

    public void StartPredicate() {
      itsState = WriterState.InPredicate;
    }
    
    public void EndPredicate() {
      itsState = WriterState.InSubject;
    }

    public void StartObject() {
      itsState = WriterState.InObject; 
    }
    
    public void EndObject() {
      itsState = WriterState.InPredicate;
    }

    public void WriteUriRef(string uriRef) {
      if ( itsState == WriterState.InSubject ) {

        if ( itsBufferedSubjects.Contains( "uriref:" + uriRef ) ) {
          itsCurrentSubject =(Subject) itsBufferedSubjects["uriref:" + uriRef];
        }
        else {
          UriRefSubject subject = new UriRefSubject(uriRef);
          itsBufferedSubjects["uriref:" + uriRef] = subject;
          itsCurrentSubject = subject;
        }
      }
      else  if (itsState == WriterState.InPredicate) {
        itsCurrentPredicateQualifiedName = ParseQualifiedName(uriRef);
      }
      else if ( itsState == WriterState.InObject ) {
        UriRefProperty property = new UriRefProperty(itsCurrentPredicateQualifiedName, uriRef);
        itsCurrentSubject.Add(property);
      }
    }
    
    public void WritePlainLiteral(string lexicalValue) {
        LiteralProperty property = new LiteralProperty(itsCurrentPredicateQualifiedName, lexicalValue, null, null);
        itsCurrentSubject.Add(property);
    }
    
    public void WritePlainLiteral(string lexicalValue, string language) {
        LiteralProperty property = new LiteralProperty(itsCurrentPredicateQualifiedName, lexicalValue, language, null);
        itsCurrentSubject.Add(property);
    }
    
    public void WriteTypedLiteral(string lexicalValue, string uriRef) {
        LiteralProperty property = new LiteralProperty(itsCurrentPredicateQualifiedName, lexicalValue, null, uriRef);
        itsCurrentSubject.Add(property);
    }
    
    public void WriteBlankNode(string nodeId) {
      string mappedNodeId;
      
      if ( itsMappedNodeIds.Contains( nodeId )) {
        mappedNodeId = (string)itsMappedNodeIds[ nodeId ];
      }
      else {
        mappedNodeId = "genid" + itsNextNodeIdIndex++;
        itsMappedNodeIds[ nodeId ] = mappedNodeId;
      }
      
    
      if ( itsState == WriterState.InSubject ) {
        if ( itsBufferedSubjects.Contains( "blank:" + mappedNodeId ) ) {
          itsCurrentSubject = (Subject)itsBufferedSubjects["blank:" + mappedNodeId];
        }
        else {
          BlankNodeSubject subject = new BlankNodeSubject(mappedNodeId);
          itsBufferedSubjects["blank:" + mappedNodeId] = subject;
          itsCurrentSubject = subject;
        }
      }
      else if ( itsState == WriterState.InObject ) {
        BlankNodeProperty property = new BlankNodeProperty( itsCurrentPredicateQualifiedName, mappedNodeId);
        itsCurrentSubject.Add(property);
      }
      
    }

    public void RegisterNamespacePrefix( string ns, string prefix) {
      itsNamespacePrefixes[ns] = prefix;
    }

    public QualifiedName ParseQualifiedName(string uriRef) {
        int namespaceDelimiterIndex = uriRef.LastIndexOf('#');
        if (namespaceDelimiterIndex == -1) {
          namespaceDelimiterIndex = uriRef.LastIndexOf('/');
          if (namespaceDelimiterIndex == -1) {
            throw new ArgumentException("Predicate uriref must contain / or #");
          }
        }
         
        string localName = uriRef.Substring(namespaceDelimiterIndex + 1);
        string ns = uriRef.Substring(0, namespaceDelimiterIndex + 1);
        
        
        
        if (! itsNamespacePrefixes.Contains( ns ) ) {
          string prefix = "ns" + itsNextNamespaceIndex++;
          RegisterNamespacePrefix( ns, prefix);
        }
  
        itsUsedNamespaces[ns] = 1;
  
        return new QualifiedName( localName, ns );
    }


    public class QualifiedName {
      private string itsLocalName;
      private string itsNamespace;
      public QualifiedName( string localName,  string ns ) {
        itsLocalName = localName;
        itsNamespace = ns;
      }
      
      public string GetNamespace() {
        return itsNamespace;
      }
      
      public string GetLocalName() {
        return itsLocalName;
      }
    }

    private abstract class Subject {
      private ArrayList itsProperties;
      private string itsTypeUriRef;
      
      public Subject() {
        itsProperties = new ArrayList();
        itsTypeUriRef = null;
      }

      public void Write(RdfXmlWriter rdfWriter, XmlWriter xmlWriter) {
        if (itsTypeUriRef == null) {
          xmlWriter.WriteStartElement( (string)rdfWriter.itsNamespacePrefixes[RDF_NAMESPACE_URI], "Description", RDF_NAMESPACE_URI);
        }
        else {
          QualifiedName typeQualifiedName = rdfWriter.ParseQualifiedName(itsTypeUriRef);
          xmlWriter.WriteStartElement((string)rdfWriter.itsNamespacePrefixes[typeQualifiedName.GetNamespace()], typeQualifiedName.GetLocalName(), typeQualifiedName.GetNamespace());
        }
        
        WriteIdentifingAttribute( rdfWriter, xmlWriter);
        foreach (Property property in itsProperties) {
          property.Write(rdfWriter, xmlWriter);
        }
        xmlWriter.WriteEndElement();
      }
      
      public abstract void WriteIdentifingAttribute(RdfXmlWriter rdfWriter, XmlWriter xmlWriter);
      
      public void Add(Property property) {
        itsProperties.Add(property);
      }

      public void Add(UriRefProperty property) {
        if (itsTypeUriRef == null && property.GetPredicateQualifiedName().GetNamespace().Equals(RDF_NAMESPACE_URI) && property.GetPredicateQualifiedName().GetLocalName().Equals("type") ) {
          itsTypeUriRef = property.GetUriRef();
        }
        else {
          itsProperties.Add(property);
        }
      }

    }

    

    private interface Property {
      void Write(RdfXmlWriter rdfWriter, XmlWriter xmlWriter);
    }

    private class UriRefSubject : Subject {
      private string itsUriRef;
      
      public UriRefSubject(string uriRef) {
        itsUriRef = uriRef;
      }

      public override void WriteIdentifingAttribute(RdfXmlWriter rdfWriter, XmlWriter xmlWriter) {
        xmlWriter.WriteAttributeString( (string)rdfWriter.itsNamespacePrefixes[RDF_NAMESPACE_URI], "about", RDF_NAMESPACE_URI, itsUriRef);
      }
    }

    private class BlankNodeSubject : Subject {
      private string itsNodeId;
      
      public BlankNodeSubject(string nodeId) {
        itsNodeId = nodeId;
      }

      public override void WriteIdentifingAttribute(RdfXmlWriter rdfWriter, XmlWriter xmlWriter) {
        xmlWriter.WriteAttributeString( (string)rdfWriter.itsNamespacePrefixes[RDF_NAMESPACE_URI], "nodeID", RDF_NAMESPACE_URI, itsNodeId);
      }
      
    }


    private class UriRefProperty : Property {
      private QualifiedName itsPredicateQualifiedName;
      private string itsUriRef;
      
      public UriRefProperty(QualifiedName predicateQualifiedName, string uriRef) {
        itsPredicateQualifiedName = predicateQualifiedName;
        itsUriRef = uriRef;
      }
      
      public void Write(RdfXmlWriter rdfWriter, XmlWriter xmlWriter) {
        xmlWriter.WriteStartElement((string)rdfWriter.itsNamespacePrefixes[itsPredicateQualifiedName.GetNamespace()], itsPredicateQualifiedName.GetLocalName(), itsPredicateQualifiedName.GetNamespace());
        xmlWriter.WriteAttributeString( (string)rdfWriter.itsNamespacePrefixes[RDF_NAMESPACE_URI], "resource", RDF_NAMESPACE_URI, itsUriRef);
        xmlWriter.WriteEndElement();
      }
      
      public QualifiedName GetPredicateQualifiedName() {
        return itsPredicateQualifiedName;
      }
      
      public string GetUriRef() {
        return itsUriRef;
      }

    }


    private class BlankNodeProperty : Property {
      private QualifiedName itsPredicateQualifiedName;
      private string itsNodeId;
      
      public BlankNodeProperty(QualifiedName predicateQualifiedName, string nodeId) {
        itsPredicateQualifiedName = predicateQualifiedName;
        itsNodeId = nodeId;
      }
      
      public void Write(RdfXmlWriter rdfWriter, XmlWriter xmlWriter) {
        xmlWriter.WriteStartElement((string)rdfWriter.itsNamespacePrefixes[itsPredicateQualifiedName.GetNamespace()], itsPredicateQualifiedName.GetLocalName(), itsPredicateQualifiedName.GetNamespace());
        xmlWriter.WriteAttributeString( (string)rdfWriter.itsNamespacePrefixes[RDF_NAMESPACE_URI], "nodeID", RDF_NAMESPACE_URI, itsNodeId);
        xmlWriter.WriteEndElement();
      }
    }

    private class LiteralProperty : Property{
      private QualifiedName itsPredicateQualifiedName;
      private string itsLexicalValue;
      private string itsLanguage;
      private string itsDataType;
      
      public LiteralProperty(QualifiedName predicateQualifiedName, string lexicalValue, string language, string dataType) {
        itsPredicateQualifiedName = predicateQualifiedName;
        itsLexicalValue = lexicalValue;
        itsLanguage = language;
        itsDataType = dataType;
      }
      
      public void Write(RdfXmlWriter rdfWriter, XmlWriter xmlWriter) {
        xmlWriter.WriteStartElement((string)rdfWriter.itsNamespacePrefixes[itsPredicateQualifiedName.GetNamespace()], itsPredicateQualifiedName.GetLocalName(), itsPredicateQualifiedName.GetNamespace());
        if (itsLanguage != null) {
          xmlWriter.WriteAttributeString( "xml", "lang", null, itsLanguage);
        }  
        else if (itsDataType != null) {
          xmlWriter.WriteAttributeString( (string)rdfWriter.itsNamespacePrefixes[RDF_NAMESPACE_URI], "datatype", RDF_NAMESPACE_URI, itsDataType);
        }      
        
        xmlWriter.WriteString( itsLexicalValue );
        xmlWriter.WriteEndElement();
      }

    
    }


  }
}