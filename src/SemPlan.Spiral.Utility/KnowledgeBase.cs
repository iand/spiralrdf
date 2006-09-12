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

namespace SemPlan.Spiral.Utility {
  using SemPlan.Spiral.Core;
  using SemPlan.Spiral.Utility;
  using System;
  using System.IO; 
  using System.Text;
  using System.Xml;
  using System.Collections;
    
  
	/// <summary>
	/// Represents a body of knowledge about any number of resources.
	/// </summary>
  /// <remarks> 
  /// $Id: KnowledgeBase.cs,v 1.3 2006/02/10 13:26:28 ian Exp $
  ///</remarks>
  public class KnowledgeBase {
    private TripleStoreFactory itsTripleStoreFactory;
    private ParserFactory itsParserFactory;

    private TripleStore itsInferences;
    private TripleStore itsAssertions; 
    private TripleStore itsSchemas;

    private ResourceFactory itsResourceFactory;
    private StatementFactory itsStatementFactory;

    private Dereferencer itsDereferencer;

    /// <summary>Creates an empty KnowledgeBase using in memory storage of resource descriptions.</summary>
    /// <param name="parserFactory">A ParserFactory that the KnowledgeBase should use to create new RDF parsers</param>
    public KnowledgeBase(ParserFactory parserFactory) : this(new MemoryTripleStoreFactory(), parserFactory) { }

    /// <summary>Creates an empty KnowledgeBase.</summary>
    /// <param name="tripleStoreFactory">A TripleStoreFactory that the KnowledgeBase should use to create any new TripleStores</param>
    /// <param name="parserFactory">A ParserFactory that the KnowledgeBase should use to create new RDF parsers</param>
    public KnowledgeBase(TripleStoreFactory tripleStoreFactory, ParserFactory parserFactory) {
      itsTripleStoreFactory = tripleStoreFactory;
      itsParserFactory = parserFactory;

      itsAssertions = itsTripleStoreFactory.MakeTripleStore();
      itsInferences =itsTripleStoreFactory.MakeTripleStore();
      itsSchemas = itsTripleStoreFactory.MakeTripleStore();


      itsResourceFactory = tripleStoreFactory.MakeResourceFactory();
      itsStatementFactory = new StatementFactory();
      itsDereferencer = itsParserFactory.GetDereferencer();
      
    }

    public virtual void SetDereferencer( Dereferencer dereferencer ) {
      itsDereferencer = dereferencer;
      itsParserFactory.SetDereferencer( itsDereferencer );
    }

    public int Count {
      get {
        return TripleStore.StatementCount;
      }
    }     
      
    public TripleStore Assertions {
      get {
        return itsAssertions;
      }
    }

    
    public TripleStore Inferences {
      get {
        return itsInferences;
      }
    }

    public TripleStore Schemas {
      get {
        return itsSchemas;
      }
    }
    
    public TripleStore TripleStore {
      get {
        if (itsInferences.IsEmpty()) {
          return itsAssertions;
        }
        else {
          return itsInferences;
        }
      }
    }


    public virtual void Add(Statement statement) {
      itsAssertions.Add( statement );
    }
    
    public virtual void Add(ResourceDescription description) {
      itsAssertions.Add( description );
    }
    
    
    public virtual Resource GetResourceDescribedBy(ConciseBoundedDescription externalDescription) {
      if ( TripleStore.HasNodeDenoting( externalDescription.Resource ) ) {
        return externalDescription.Resource;
      }
      else {
        foreach (Node node in externalDescription.DenotingNodes) {
          if ( TripleStore.HasResourceDenotedBy(node) ) {
            return TripleStore.GetResourceDenotedBy(node);
          }
        }
        return externalDescription.Resource;
      }
    }



    public virtual void Replace(ConciseBoundedDescription description) {
      Resource internalSubjectResource = GetResourceDescribedBy(description);
      ArrayList statementsToRemove = new ArrayList();

      IEnumerator statementEnumerator = itsAssertions.GetStatementEnumerator();
      while (statementEnumerator.MoveNext()) {
        ResourceStatement statement = (ResourceStatement)statementEnumerator.Current;
        if (statement.GetSubject().Equals( internalSubjectResource )  ) {
          statementsToRemove.Add( statement );
        }          
      }
  
      foreach (ResourceStatement statement in statementsToRemove) {
        itsAssertions.Remove( statement );
      }

      Add( description );
    }


    /// <summary>Adds a statement of knowledge about a resource to the KnowledgeBase</summary>
    public virtual void Add(Node theSubject, Arc thePredicate, Node theObject) {
      itsAssertions.Add( new Statement(theSubject, thePredicate, theObject) );
    }


		public virtual void Add(KnowledgeBase moreKnowledge) {
      itsAssertions.Add( moreKnowledge.Assertions );
    }

    /// <returns>A concise bounded description containing all properties the KnowledgeBase knows about the resource denoted by the subject if the subject is known, otherwise an empty description.</returns>
    public virtual ResourceDescription GetDescriptionOf(Resource theResource) {
      CbdBoundingStrategy strategy = new CbdBoundingStrategy();
      return strategy.GetDescriptionOf(  theResource, TripleStore );
    }        

    /// <returns>A concise bounded description containing all properties the KnowledgeBase knows about the resource denoted by the subject if the subject is known, otherwise an empty description.</returns>
    public virtual ResourceDescription GetDescriptionOf(Node theNode) {
      return GetDescriptionOf(  TripleStore.GetResourceDenotedBy(theNode) );
    }        


    /// <summary>Removes all statements from the KnowledgeBase.</summary>
    public virtual void Clear() {
      itsAssertions.Clear();
      itsInferences.Clear();
      itsSchemas.Clear();
    }

    /// <summary>Gets a value indicating whether the KnowledgeBase is empty or not.</summary>
    /// <returns>true if the KnowledgeBase contains any resource descriptions, false otherwise</returns>
    public virtual bool IsEmpty() {
      return itsAssertions.IsEmpty();
    }

    public virtual bool Contains(Node theSubject, Arc thePredicate, Node theObject) {
      return TripleStore.Contains( new Statement( theSubject, thePredicate, theObject ) );
    }

    /// <returns>True if the knowledge base has some description about the supplied node</returns>
    public virtual bool HasDescriptionOf(Node theNode) {
      return TripleStore.HasResourceDenotedBy(theNode);
    }
    
    /// <summary>Examines the supplied ConciseBoundedDescription and attempts to locate more information about the described resource.</summary>
    /// <param name="resourceDescription">A ConciseBoundedDescription about which more information is required.</param>
    /// <param name="investigator">An investigator that encapsulates the algorithm to be used to locate more information.</param>
		public virtual void Investigate(ConciseBoundedDescription resourceDescription, Investigator investigator) {
      SemPlan.Spiral.Core.ResourceFactory resourceFactory = itsTripleStoreFactory.MakeResourceFactory();

      investigator.NewStatement +=itsAssertions.GetStatementHandler();
      investigator.investigate( resourceDescription, this, itsParserFactory, resourceFactory, itsStatementFactory, itsDereferencer);
      investigator.NewStatement -=itsAssertions.GetStatementHandler();
    }
    
    /// <summary>Applies inference rules to assertions in the KnowledgeBase.</summary>
    public virtual void Think() {
      // Implementation of this method has not been optimised for efficiency. Focus is on correctness for now

      itsInferences = (TripleStore)itsAssertions.Clone();
      itsInferences.Add( itsSchemas );
      
            
      Rule axioms = MakeAxioms();
      itsInferences.Evaluate( axioms );

      int previousStatementCount = 0;
      
      ArrayList rules = new ArrayList();

      Rule rdf1 = new Rule();
      rdf1.AddAntecedent( new Pattern( new Variable("uuu"), new Variable("aaa"), new Variable("yyy") ) );
      rdf1.AddConsequent( new Pattern( new Variable("aaa"), Schema.rdf.type,  Schema.rdf.Property) );
      rules.Add( rdf1 );

      Rule rdfs2 = new Rule();
      rdfs2.AddAntecedent( new Pattern( new Variable("aaa"), Schema.rdfs.domain, new Variable("xxx") ) );
      rdfs2.AddAntecedent( new Pattern( new Variable("uuu"), new Variable("aaa"), new Variable("yyy") ) );
      rdfs2.AddConsequent( new Pattern( new Variable("uuu"), Schema.rdf.type,  new Variable("xxx") ) );
      rules.Add( rdfs2 );

      Rule rdfs3 = new Rule();
      rdfs3.AddAntecedent( new Pattern( new Variable("aaa"), Schema.rdfs.range, new Variable("xxx") ) );
      rdfs3.AddAntecedent( new Pattern( new Variable("uuu"), new Variable("aaa"), new Variable("vvv") ) );
      rdfs3.AddConsequent( new Pattern( new Variable("vvv"), Schema.rdf.type,  new Variable("xxx") ) );
      rules.Add( rdfs3 );


      Rule rdfs4 = new Rule();
      rdfs4.AddAntecedent( new Pattern( new Variable("uuu"), new Variable("aaa"), new Variable("xxx") ) );
      rdfs4.AddConsequent( new Pattern( new Variable("uuu"), Schema.rdf.type,  Schema.rdfs.Resource ) );
      rdfs4.AddConsequent( new Pattern( new Variable("xxx"), Schema.rdf.type,  Schema.rdfs.Resource ) );
      rules.Add( rdfs4 );

      Rule rdfs5 = new Rule();
      rdfs5.AddAntecedent( new Pattern( new Variable("uuu"), Schema.rdf.type,  Schema.rdf.Property) );
      rdfs5.AddConsequent( new Pattern( new Variable("uuu"), Schema.rdfs.subPropertyOf,  new Variable("uuu")) );
      rules.Add( rdfs5 );


      Rule rdfs6 = new Rule();
      rdfs6.AddAntecedent( new Pattern( new Variable("uuu"), Schema.rdfs.subPropertyOf, new Variable("vvv") ) );
      rdfs6.AddAntecedent( new Pattern( new Variable("vvv"), Schema.rdfs.subPropertyOf, new Variable("xxx") ) );
      rdfs6.AddConsequent( new Pattern( new Variable("uuu"), Schema.rdfs.subPropertyOf, new Variable("xxx") ) );
      rules.Add( rdfs6 );

      Rule rdfs7 = new Rule();
      rdfs7.AddAntecedent( new Pattern( new Variable("aaa"), Schema.rdfs.subPropertyOf, new Variable("bbb") ) );
      rdfs7.AddAntecedent( new Pattern( new Variable("uuu"), new Variable("aaa"), new Variable("yyy") ) );
      rdfs7.AddConsequent( new Pattern( new Variable("uuu"), new Variable("bbb"), new Variable("yyy") ) );
      rules.Add( rdfs7 );

      Rule rdfs8and10 = new Rule();
      rdfs8and10.AddAntecedent( new Pattern( new Variable("uuu"), Schema.rdf.type,  Schema.rdfs.Class) );
      rdfs8and10.AddConsequent( new Pattern( new Variable("uuu"), Schema.rdfs.subClassOf, Schema.rdfs.Resource ) );
      rdfs8and10.AddConsequent( new Pattern( new Variable("uuu"), Schema.rdfs.subClassOf,  new Variable("uuu")) );
      rules.Add( rdfs8and10 );


      Rule rdfs9 = new Rule();
      rdfs9.AddAntecedent( new Pattern( new Variable("uuu"), Schema.rdfs.subClassOf, new Variable("xxx") ) );
      rdfs9.AddAntecedent( new Pattern( new Variable("vvv"), Schema.rdf.type, new Variable("uuu") ) );
      rdfs9.AddConsequent( new Pattern( new Variable("vvv"), Schema.rdf.type, new Variable("xxx") ) );
      rules.Add( rdfs9 );

      Rule rdfs11 = new Rule();
      rdfs11.AddAntecedent( new Pattern( new Variable("uuu"), Schema.rdfs.subClassOf, new Variable("vvv") ) );
      rdfs11.AddAntecedent( new Pattern( new Variable("vvv"), Schema.rdfs.subClassOf, new Variable("xxx") ) );
      rdfs11.AddConsequent( new Pattern( new Variable("uuu"), Schema.rdfs.subClassOf, new Variable("xxx") ) );
      rules.Add( rdfs11 );

      Rule rdfs12 = new Rule();
      rdfs12.AddAntecedent( new Pattern( new Variable("uuu"), Schema.rdf.type, Schema.rdfs.ContainerMembershipProperty) );
      rdfs12.AddConsequent( new Pattern( new Variable("uuu"), Schema.rdfs.subPropertyOf, Schema.rdfs.member ) );
      rules.Add( rdfs12 );
      
      Rule rdfs13 = new Rule();
      rdfs13.AddAntecedent( new Pattern( new Variable("uuu"), Schema.rdf.type, Schema.rdfs.Datatype) );
      rdfs13.AddConsequent( new Pattern( new Variable("uuu"), Schema.rdfs.subClassOf, Schema.rdfs.Literal ) );
      rules.Add( rdfs13 );

/*
Can also add following OWL entailment rules 
(after Herman J. ter Horst, Extending the RDFS Entailment Lemma, Proceedings of 3rd International Semantic Web Conference)

rdfp1: (?p rdf:type owl:FunctionalProperty) (?u ?p ?v) (?u ?p ?w) where v is URI or BlankNode then add (?v owl:sameAs ?w)
rdfp2: (?p rdf:type owl:InverseFunctionalProperty) (?u ?p ?w) (?v ?p ?w) then add (?u owl:sameAs ?v)
rdfp3: (?p rdf:type owl:SymmetricProperty) (?v ?p ?w) where w is URI or BlankNode then add (?w ?p ?v)
rdfp4: (?p rdf:type owl:TransitiveProperty) (?u ?p ?v) (?v ?p ?w) then add (?u ?p ?w)
rdfp5a: (?v ?p ?w) then add (?v owl:sameAs ?v)
rdfp5b: (?v ?p ?w) where w is URI or BlankNode then add (?w owl:sameAs ?w)
rdfp6: (?v owl:sameAs ?w) where w is URI or BlankNode then add (?w owl:sameAs ?v)
rdfp7: (?u owl:sameAs ?v) (?v owl:sameAs ?w) then add (?u owl:sameAs ?w)
rdfp8a: (?p owl:inverseOf ?q) (?v ?p ?w) where w is URI or BlankNode and q is URI then add (?w ?q ?v)
rdfp8b: (?p owl:inverseOf ?q) (?v ?q ?w) where w is URI or BlankNode and q is URI then add (?w ?p ?v)
rdfp9: (?v rdf:type rdfs:Class) (?v owl:sameAs ?w) then add (?v rdfs:subClassOf ?w)
rdfp10: (?p rdf:type rdf:Property) (?p owl:sameAs ?q) then add (?p rdfs:subPropertyOf ?q)


*/
      while (previousStatementCount != itsInferences.StatementCount) {
        previousStatementCount = itsInferences.StatementCount;
        foreach (Rule rule in rules) {
          itsInferences.Evaluate( rule );
        }
      }

    }

    private Rule MakeAxioms() {
      Rule axioms = new Rule();
      
      axioms.AddConsequent( new Pattern(Schema.rdfs.comment, Schema.rdfs.domain, Schema.rdfs.Resource  ) );
      axioms.AddConsequent( new Pattern(Schema.rdfs.comment, Schema.rdfs.range, Schema.rdfs.Literal  ) );

      axioms.AddConsequent( new Pattern(Schema.rdfs.domain, Schema.rdfs.domain, Schema.rdf.Property  ) );
      axioms.AddConsequent( new Pattern(Schema.rdfs.domain, Schema.rdfs.range, Schema.rdfs.Class  ) );

      axioms.AddConsequent( new Pattern(Schema.rdf.first, Schema.rdf.type, Schema.rdf.Property) );
      axioms.AddConsequent( new Pattern(Schema.rdf.first, Schema.rdfs.domain, Schema.rdf.List  ) );
      axioms.AddConsequent( new Pattern(Schema.rdf.first, Schema.rdfs.range, Schema.rdfs.Resource  ) );

      axioms.AddConsequent( new Pattern(Schema.rdfs.isDefinedBy, Schema.rdfs.domain, Schema.rdfs.Resource  ) );
      axioms.AddConsequent( new Pattern(Schema.rdfs.isDefinedBy, Schema.rdfs.range, Schema.rdfs.Resource  ) );

      axioms.AddConsequent( new Pattern(Schema.rdfs.label, Schema.rdfs.domain, Schema.rdfs.Resource  ) );
      axioms.AddConsequent( new Pattern(Schema.rdfs.label, Schema.rdfs.range, Schema.rdfs.Literal  ) );

      axioms.AddConsequent( new Pattern(Schema.rdfs.member, Schema.rdfs.domain, Schema.rdfs.Resource  ) );
      axioms.AddConsequent( new Pattern(Schema.rdfs.member, Schema.rdfs.range, Schema.rdfs.Resource  ) );

      axioms.AddConsequent( new Pattern(Schema.rdf.nil, Schema.rdf.type, Schema.rdf.Property) );

      axioms.AddConsequent( new Pattern(Schema.rdf.object_, Schema.rdf.type, Schema.rdf.Property) );
      axioms.AddConsequent( new Pattern(Schema.rdf.object_, Schema.rdfs.domain, Schema.rdf.Statement  ) );
      axioms.AddConsequent( new Pattern(Schema.rdf.object_, Schema.rdfs.range, Schema.rdfs.Resource  ) );

      axioms.AddConsequent( new Pattern(Schema.rdf.predicate, Schema.rdf.type, Schema.rdf.Property) );
      axioms.AddConsequent( new Pattern(Schema.rdf.predicate, Schema.rdfs.domain, Schema.rdf.Statement  ) );
      axioms.AddConsequent( new Pattern(Schema.rdf.predicate, Schema.rdfs.range, Schema.rdfs.Resource  ) );

      axioms.AddConsequent( new Pattern(Schema.rdfs.range, Schema.rdfs.domain, Schema.rdf.Property  ) );
      axioms.AddConsequent( new Pattern(Schema.rdfs.range, Schema.rdfs.range, Schema.rdfs.Class  ) );

      axioms.AddConsequent( new Pattern(Schema.rdf.rest, Schema.rdf.type, Schema.rdf.Property) );
      axioms.AddConsequent( new Pattern(Schema.rdf.rest, Schema.rdfs.domain, Schema.rdf.List  ) );
      axioms.AddConsequent( new Pattern(Schema.rdf.rest, Schema.rdfs.range, Schema.rdf.List  ) );

      axioms.AddConsequent( new Pattern(Schema.rdfs.seeAlso, Schema.rdfs.domain, Schema.rdfs.Resource  ) );
      axioms.AddConsequent( new Pattern(Schema.rdfs.seeAlso, Schema.rdfs.range, Schema.rdfs.Resource  ) );

      axioms.AddConsequent( new Pattern(Schema.rdf.subject, Schema.rdf.type, Schema.rdf.Property) );
      axioms.AddConsequent( new Pattern(Schema.rdf.subject, Schema.rdfs.domain, Schema.rdf.Statement  ) );
      axioms.AddConsequent( new Pattern(Schema.rdf.subject, Schema.rdfs.range, Schema.rdfs.Resource  ) );

      axioms.AddConsequent( new Pattern(Schema.rdfs.subClassOf, Schema.rdfs.domain, Schema.rdfs.Class  ) );
      axioms.AddConsequent( new Pattern(Schema.rdfs.subClassOf, Schema.rdfs.range, Schema.rdfs.Class  ) );
      
      axioms.AddConsequent( new Pattern(Schema.rdfs.subPropertyOf, Schema.rdfs.domain, Schema.rdf.Property  ) );
      axioms.AddConsequent( new Pattern(Schema.rdfs.subPropertyOf, Schema.rdfs.range, Schema.rdf.Property  ) );

      axioms.AddConsequent( new Pattern(Schema.rdf.type, Schema.rdf.type, Schema.rdf.Property) );
      axioms.AddConsequent( new Pattern(Schema.rdf.type, Schema.rdfs.domain, Schema.rdfs.Resource  ) );
      axioms.AddConsequent( new Pattern(Schema.rdf.type, Schema.rdfs.range, Schema.rdfs.Class  ) );

      axioms.AddConsequent( new Pattern(Schema.rdf.value, Schema.rdf.type, Schema.rdf.Property) );
      axioms.AddConsequent( new Pattern(Schema.rdf.value, Schema.rdfs.domain, Schema.rdfs.Resource  ) );
      axioms.AddConsequent( new Pattern(Schema.rdf.value, Schema.rdfs.range, Schema.rdfs.Resource  ) );

      axioms.AddConsequent( new Pattern(Schema.rdf.Alt, Schema.rdfs.subClassOf, Schema.rdfs.Container  ) );
      axioms.AddConsequent( new Pattern(Schema.rdf.Bag, Schema.rdfs.subClassOf, Schema.rdfs.Container  ) );
      axioms.AddConsequent( new Pattern(Schema.rdf.Seq, Schema.rdfs.subClassOf, Schema.rdfs.Container  ) );
      axioms.AddConsequent( new Pattern(Schema.rdfs.ContainerMembershipProperty, Schema.rdfs.subClassOf, Schema.rdf.Property  ) );

      axioms.AddConsequent( new Pattern(Schema.rdfs.isDefinedBy, Schema.rdfs.subPropertyOf, Schema.rdfs.seeAlso  ) );

      axioms.AddConsequent( new Pattern(Schema.rdf.XMLLiteral, Schema.rdf.type, Schema.rdfs.Datatype ) );
      axioms.AddConsequent( new Pattern(Schema.rdf.XMLLiteral, Schema.rdfs.subClassOf, Schema.rdfs.Literal  ) );
      axioms.AddConsequent( new Pattern(Schema.rdfs.Datatype, Schema.rdfs.subClassOf, Schema.rdfs.Class  ) );

      for (int listIndex = 1; listIndex < 16; ++listIndex) {
        UriRef listIndexProperty = new UriRef(Schema.rdf._nsprefix + "_" + listIndex);
        axioms.AddConsequent( new Pattern(listIndexProperty, Schema.rdf.type, Schema.rdf.Property) );
        axioms.AddConsequent( new Pattern(listIndexProperty, Schema.rdf.type, Schema.rdfs.ContainerMembershipProperty ) );
        axioms.AddConsequent( new Pattern(listIndexProperty, Schema.rdfs.domain, Schema.rdfs.Resource  ) );
        axioms.AddConsequent( new Pattern(listIndexProperty, Schema.rdfs.range, Schema.rdfs.Resource  ) );
      }

      return axioms;
    }


    private Parser GetParser() {
      SemPlan.Spiral.Core.ResourceFactory resourceFactory = itsTripleStoreFactory.MakeResourceFactory();
      Parser parser = itsParserFactory.MakeParser( resourceFactory , itsStatementFactory);
      return parser;
    }

    /// <summary>Parses the supplied RDF and adds any resulting statements into the KnowledgeBase</summary>
    /// <param name="reader">The TextReader containing the RDF data to read.</param>
    /// <param name="baseUri">A uri to be used by the KnowledgeBase to resolve relative uris in the RDF data.</param>
		public virtual void Include(TextReader reader, string baseUri) {
      try {
        Parser parser = GetParser();
        parser.NewStatement += itsAssertions.GetStatementHandler();
        parser.Parse(  reader, baseUri );
        parser.NewStatement -= itsAssertions.GetStatementHandler();
      }
      catch (ParserException e) {
        throw new KnowledgeBaseException("Problem including RDF from reader", e);
      }
    }
    
    /// <summary>Fetches RDF from the supplied uri, parses the content and adds any resulting statements into the KnowledgeBase</summary>
    /// <param name="uri">The URI of the RDF data.</param>
		public virtual void Include(Uri uri) {
      try {

        DereferencerResponse response = itsDereferencer.Dereference( uri );
        if ( response.HasContent ) {
          Parser parser = GetParser();
          parser.NewStatement += itsAssertions.GetStatementHandler();
          parser.Parse(  response.Stream, uri.ToString() );
          response.Stream.Close();
          parser.NewStatement -= itsAssertions.GetStatementHandler();
       }

      }
      catch (ParserException e) {
        throw new KnowledgeBaseException("Problem including RDF from " + uri, e);
      }
    }
    
    /// <summary>Parses the supplied RDF and adds any resulting statements into the KnowledgeBase</summary>
    /// <param name="stream">The Stream containing the RDF data to read.</param>
    /// <param name="baseUri">A uri to be used by the KnowledgeBase to resolve relative uris in the RDF data.</param>
		public virtual void Include(Stream stream, string baseUri) {
      try {
        Parser parser = GetParser();
        parser.NewStatement += itsAssertions.GetStatementHandler();
        parser.Parse(  stream, baseUri );
        parser.NewStatement -= itsAssertions.GetStatementHandler();
      }
      catch (ParserException e) {
        throw new KnowledgeBaseException("Problem including RDF from stream", e);
      }
    }

    /// <summary>Fetches RDF from the supplied uri, parses the content and adds any resulting statements into the KnowledgeBase</summary>
    /// <param name="uri">A string verson of the URI of RDF data.</param>
		public virtual void Include(string uri) {
      try {
        DereferencerResponse response = itsDereferencer.Dereference( uri );
        if ( response.HasContent ) {
          Parser parser = GetParser();
          parser.NewStatement += itsAssertions.GetStatementHandler();
          parser.Parse(  response.Stream, uri.ToString() );
          response.Stream.Close();
          parser.NewStatement -= itsAssertions.GetStatementHandler();
       }
      }
      catch (ParserException e) {
        throw new KnowledgeBaseException("Problem including RDF from " + uri, e);
      }
    }


    /// <summary>Parses the supplied RDF and adds any resulting statements into the KnowledgeBase</summary>
    /// <param name="reader">The TextReader containing the RDF data to read.</param>
    /// <param name="baseUri">A uri to be used by the KnowledgeBase to resolve relative uris in the RDF data.</param>
		public virtual void IncludeSchema(TextReader reader, string baseUri) {
      try {
        Parser parser = GetParser();
        parser.NewStatement += itsSchemas.GetStatementHandler();
        parser.Parse(  reader, baseUri );
        parser.NewStatement -= itsSchemas.GetStatementHandler();
      }
      catch (ParserException e) {
        throw new KnowledgeBaseException("Problem including RDF from reader", e);
      }
    }
    

    /// <summary>Fetches RDF from the supplied uri, parses the content and adds any resulting statements into the KnowledgeBase</summary>
    /// <param name="uri">The URI of the RDF data.</param>
		public virtual void IncludeSchema(Uri uri) {
      try {
        DereferencerResponse response = itsDereferencer.Dereference( uri );
        if ( response.HasContent ) {
          Parser parser = GetParser();
          parser.NewStatement += itsSchemas.GetStatementHandler();
          parser.Parse(  response.Stream, uri.ToString() );
          response.Stream.Close();
          parser.NewStatement -= itsSchemas.GetStatementHandler();
       }
      }
      catch (ParserException e) {
        throw new KnowledgeBaseException("Problem including RDF from " + uri, e);
      }
    }
    
    /// <summary>Parses the supplied RDF and adds any resulting statements into the KnowledgeBase</summary>
    /// <param name="stream">The Stream containing the RDF data to read.</param>
    /// <param name="baseUri">A uri to be used by the KnowledgeBase to resolve relative uris in the RDF data.</param>
		public virtual void IncludeSchema(Stream stream, string baseUri) {
      try {
        Parser parser = GetParser();
        parser.NewStatement += itsSchemas.GetStatementHandler();
        parser.Parse(  stream, baseUri );
        parser.NewStatement -= itsSchemas.GetStatementHandler();
      }
      catch (ParserException e) {
        throw new KnowledgeBaseException("Problem including RDF from stream", e);
      }
    }

    /// <summary>Fetches RDF from the supplied uri, parses the content and adds any resulting statements into the KnowledgeBase</summary>
    /// <param name="uri">A string verson of the URI of RDF data.</param>
		public virtual void IncludeSchema(string uri) {
      try {
        DereferencerResponse response = itsDereferencer.Dereference( uri );
        if ( response.HasContent ) {
          Parser parser = GetParser();
          parser.NewStatement += itsSchemas.GetStatementHandler();
          parser.Parse(  response.Stream, uri.ToString() );
          response.Stream.Close();
          parser.NewStatement -= itsSchemas.GetStatementHandler();
       }
      }
      catch (ParserException e) {
        throw new KnowledgeBaseException("Problem including RDF from " + uri, e);
      }
    }


    public virtual void Write(RdfWriter writer) {
      itsAssertions.Write( writer );
    }

    public override string ToString() {
      StringBuilder stringValue = new StringBuilder();
      
      stringValue.Append( "KnowledgeBase\n");
      stringValue.Append( "assertions:\n");
      stringValue.Append( itsAssertions.ToString() );
      stringValue.Append( "inferences:\n");
      stringValue.Append( itsInferences.ToString() );
      return stringValue.ToString();
    }
  
    public virtual StatementHandler GetStatementHandler() {
      return itsAssertions.GetStatementHandler();
    }

    
    public void Dump() {
     Console.WriteLine("[KnowledgeBase " + GetHashCode() + "]");
     Console.WriteLine("  itsAssertions:");
     Console.WriteLine( itsAssertions.ToString());
     Console.WriteLine("  itsInferences:");
     Console.WriteLine( itsInferences.ToString());
    }

    public IEnumerator Solve(Query query) {
      return TripleStore.Solve( query );
    }
    
  } 
  
}