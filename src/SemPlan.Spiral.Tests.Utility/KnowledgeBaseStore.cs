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

namespace SemPlan.Spiral.Tests.Utility {
  using SemPlan.Spiral.Core;
  using SemPlan.Spiral.Utility;
  using SemPlan.Spiral.Tests.Core;
  using System;
  using System.Collections;
  using System.IO;
  using System.Xml;
  
	/// <summary>
	/// An instrumented version of KnowledgeBase that stores arguments to method calls
	/// </summary>
  /// <remarks>
  /// $Id: KnowledgeBaseStore.cs,v 1.2 2006/02/10 13:26:28 ian Exp $
  ///</remarks>
  public class KnowledgeBaseStore : KnowledgeBase {

    private MethodCallStore itsMethodCalls;
    
    public KnowledgeBaseStore() : base(new ParserFactoryStub()) {
      itsMethodCalls = new MethodCallStore();
    }



    public override void SetDereferencer( Dereferencer dereferencer ) {
      itsMethodCalls.RecordMethodCall("SetDereferencer", dereferencer);
    }
    public bool WasSetDereferencerCalledWith(Dereferencer dereferencer)  {
      return itsMethodCalls.WasMethodCalledWith("SetDereferencer", dereferencer);
    }

    
    public override void Add(Node theSubject, Arc thePredicate, Node theObject) {
      itsMethodCalls.RecordMethodCall("Add", theSubject, thePredicate, theObject);
    }
    public bool WasAddCalledWith(Node theSubject, Arc thePredicate, Node theObject) {
      return itsMethodCalls.WasMethodCalledWith("Add", theSubject, thePredicate, theObject);
    }

    public override void Add(ResourceDescription description) {
      itsMethodCalls.RecordMethodCall("Add", description);
    }
    public bool WasAddCalledWith(ResourceDescription description) {
      return itsMethodCalls.WasMethodCalledWith("Add", description);
    }

    public override void Replace(ConciseBoundedDescription description) {
      itsMethodCalls.RecordMethodCall("Replace", description);
    }
    public bool WasReplaceCalledWith(ConciseBoundedDescription description) {
      return itsMethodCalls.WasMethodCalledWith("Replace", description);
    }


    public override void Clear() {
        // NOOP
    }

    public override bool IsEmpty() {
      return true;
    }

    

    //~ public override ResourceDescription GetDescriptionOf(Resource theResource) {
      //~ itsMethodCalls.RecordMethodCall("GetDescriptionOf", theResource);
      //~ return new ConciseBoundedDescription( theResource );
    //~ }
    //~ public bool WasGetDescriptionOfCalledWith(Resource theResource) {
      //~ return itsMethodCalls.WasMethodCalledWith("GetDescriptionOf", theResource);
    //~ }

		public override void Investigate(ConciseBoundedDescription resourceDescription, Investigator investigator) {
      itsMethodCalls.RecordMethodCall("Investigate", resourceDescription, investigator);
      itsMethodCalls.RecordMethodCall("Investigate", resourceDescription);
    }
    public bool WasInvestigateCalledWith(ConciseBoundedDescription resourceDescription, Investigator investigator) {
      return itsMethodCalls.WasMethodCalledWith("Investigate", resourceDescription, investigator);
    }
    public bool WasInvestigateCalledFor(ConciseBoundedDescription resourceDescription) {
      return itsMethodCalls.WasMethodCalledWith("Onvestigate", resourceDescription);
    }
  
    public override void Think() {
        // NOOP
    }

		public override void Include(TextReader reader, string baseUri) {
      itsMethodCalls.RecordMethodCall("Include", reader, baseUri);
    }
    public bool WasIncludeCalledWith(TextReader reader, string baseUri) {
      return itsMethodCalls.WasMethodCalledWith("Include", reader, baseUri);
    }

    public bool WasIncludeCalledWith(XmlReader reader, string baseUri) {
      return itsMethodCalls.WasMethodCalledWith("Include", reader, baseUri);
    }

		public override void Include(Uri uri) {
      itsMethodCalls.RecordMethodCall("Include", uri);
    }
    public bool WasIncludeCalledWith(Uri uri) {
      return itsMethodCalls.WasMethodCalledWith("Include", uri);
    }
    
		public override void Include(Stream stream, string baseUri) {
      itsMethodCalls.RecordMethodCall("Include", stream, baseUri);
    }
    public bool WasIncludeCalledWith(Stream stream, string baseUri) {
      return itsMethodCalls.WasMethodCalledWith("Include", stream, baseUri);
    }

		public override void Include(string uri) {
      itsMethodCalls.RecordMethodCall("Include", uri);
    }
    public bool WasIncludeCalledWith(string uri) {
      return itsMethodCalls.WasMethodCalledWith("Include", uri);
    }


		public override void IncludeSchema(TextReader reader, string baseUri) {
      itsMethodCalls.RecordMethodCall("IncludeSchema", reader, baseUri);
    }
    public bool WasIncludeSchemaCalledWith(TextReader reader, string baseUri) {
      return itsMethodCalls.WasMethodCalledWith("IncludeSchema", reader, baseUri);
    }
    
		public override void IncludeSchema(Uri uri) {
      itsMethodCalls.RecordMethodCall("IncludeSchema", uri);
    }
    public bool WasIncludeSchemaCalledWith(Uri uri) {
      return itsMethodCalls.WasMethodCalledWith("IncludeSchema", uri);
    }
    
		public override void IncludeSchema(Stream stream, string baseUri) {
      itsMethodCalls.RecordMethodCall("IncludeSchema", stream, baseUri);
    }
    public bool WasIncludeSchemaCalledWith(Stream stream, string baseUri) {
      return itsMethodCalls.WasMethodCalledWith("IncludeSchema", stream, baseUri);
    }

		public override void IncludeSchema(string uri) {
      itsMethodCalls.RecordMethodCall("IncludeSchema", uri);
    }
    public bool WasIncludeSchemaCalledWith(string uri) {
      return itsMethodCalls.WasMethodCalledWith("IncludeSchema", uri);
    }


    public override void Write(RdfWriter writer) {
      itsMethodCalls.RecordMethodCall("Write", writer);
    }
    public bool WasWriteCalledWith(RdfWriter writer) {
      return itsMethodCalls.WasMethodCalledWith("Write", writer);
    }
  
    public override string ToString() {
      return "KnowledgeBaseStore";
    }
  
    public string GetMethodCalls() {
      return itsMethodCalls.getMethodCalls();
    }
 

  }
}