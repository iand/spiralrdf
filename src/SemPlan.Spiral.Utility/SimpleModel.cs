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
  using System.IO;
  using System.Collections;
  using System.Xml;
	/// <summary>
	/// A simple RDF model implementation 
	/// </summary>
  /// <remarks>
  /// $Id: SimpleModel.cs,v 1.2 2005/05/26 14:24:31 ian Exp $
  ///</remarks>
    public class SimpleModel {
      private Dereferencer itsDereferencer;
      private ArrayList itsStatements;
      private ParserFactory itsParserFactory;
      private TripleStore itsTripleStore;
      
      public SimpleModel(ParserFactory parserFactory) {
        itsParserFactory = parserFactory;
        itsStatements = new ArrayList();
        itsTripleStore = new MemoryTripleStore();
        itsDereferencer = new SimpleDereferencer();
      }
      

      public void SetParserFactory(ParserFactory parserFactory) {
        itsParserFactory = parserFactory;
      }
      
      public void ParseString(string content, string baseUri) {
        StringReader reader = new StringReader(content);
        Parse( reader, baseUri);
      }
      
     /// <summary>
      /// Parse the RDF at the given URI
      /// </summary>
      public void Parse(Uri uri, string baseUri) {
        DereferencerResponse response = itsDereferencer.Dereference( uri );
        if ( response.HasContent ) {
          Parse( response.Stream, uri.ToString() );
        }
        response.Stream.Close();
      }
      
      /// <summary>
      /// Parse the RDF using the string paramater as a URI
      /// </summary>
      public void Parse(string uri, string baseUri) {
        DereferencerResponse response = itsDereferencer.Dereference( uri );
        if ( response.HasContent ) {
          Parse( response.Stream, uri.ToString() );
        }
        response.Stream.Close();
      }
  
      /// <summary>
      /// Parse the RDF using supplied TextReader and base URI
      /// </summary>
      public void Parse(TextReader reader, string baseUri) {
        Parser parser = itsParserFactory.MakeParser(new ResourceFactory(), new StatementFactory());
        StatementHandler handler = new StatementHandler(Add);
        StatementHandler tripleStoreHandler = itsTripleStore.GetStatementHandler();

        parser.NewStatement += handler;
        parser.NewStatement += tripleStoreHandler;
        parser.Parse(reader, baseUri);
        parser.NewStatement -= handler;
        parser.NewStatement -= tripleStoreHandler;
      }
  
      /// <summary>
      /// Parse the RDF using supplied stream and base URI
      /// </summary>
      public void Parse(Stream stream, string baseUri) {
        Parser parser = itsParserFactory.MakeParser(new ResourceFactory(), new StatementFactory());
        StatementHandler handler = new StatementHandler(Add);
        StatementHandler tripleStoreHandler = itsTripleStore.GetStatementHandler();

        parser.NewStatement += handler;
        parser.NewStatement += tripleStoreHandler;
        parser.Parse(stream, baseUri);
        parser.NewStatement -= handler;
        parser.NewStatement -= tripleStoreHandler;
      }
      
      
      
      public void Add(Statement statement) {
        itsStatements.Add(statement);
      }
    
      public int Count {
        get {
          return itsStatements.Count;
        }
      }

      public IEnumerator GetEnumerator() {
        return new NTripleEnumerator(this);
      }

  
      public void Write(RdfWriter writer) {
        writer.StartOutput();
        foreach (Statement statement in itsStatements) {
          statement.Write(writer);
        }
        
        writer.EndOutput();
      }
  
      public override string ToString() {
        StringWriter output = new StringWriter();
        NTripleWriter writer = new NTripleWriter(output);
        Write(writer);
        return output.ToString();
      }

      
      private class NTripleEnumerator : IEnumerator {
        private ArrayList itsNTriples;
        private int itsIndex;
        public NTripleEnumerator(SimpleModel model) {
          itsIndex = -1;
          itsNTriples = new ArrayList();
  
          StringWriter received = new StringWriter();
          NTripleWriter writer = new NTripleWriter(received);
          model.Write(writer);
          StringReader receivedReader = new StringReader(received.ToString());
          string receivedLine = receivedReader.ReadLine();
          while (receivedLine != null) {
  
            string trimmed = receivedLine.Trim();
            if (trimmed.Length > 0 &&  ! trimmed.StartsWith("#") ) {
              itsNTriples.Add( trimmed );
             }
            receivedLine = receivedReader.ReadLine();
          }
  
        }
        
        public object Current {
          get {
            if (itsIndex < itsNTriples.Count) {
              return itsNTriples[itsIndex];
            }
            else {
              return null;
            }
          }
        }
        
        public bool MoveNext() {
          if ( ++itsIndex < itsNTriples.Count ) {
            return true;
          }
          else {
            return false;
          }
        }
  
        public void Reset() {
          itsIndex = -1;
        }
  
      }
 
 
 }
 


 
}