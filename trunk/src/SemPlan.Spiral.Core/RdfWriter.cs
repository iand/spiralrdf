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


namespace SemPlan.Spiral.Core {
  using System;
	/** 
  <summary>
	  <para>
      Represents a writer that can output RDF triples into some format. 
      It is designed to be generic  and have no dependencies on other classes.
    </para>
    <para>
      The writing process is modelled as a sequence of method calls. 
      To write a statements the client should
    </para>
    <para>
      1. call startOutput
      2. call startSubject followed by one of writeUriRef or writeBlankNode.
      3. call startPredicate followed by writeUriRef
      4. call startObject followed by writeUriRef, writeBlankNode, writePlainLiteral or writeTypedLiteral
      5. call endObject
      6. either write another object that will apply to the same subject, predicate pair or call endPredicate
      7. either write another predicate that will apply to the same subject or call endSubject      
      8. write any more statements
      9. call endOutput
    </para>
    <para>
      The client and the writer are expected to adhere to the following contract:
    </para>
    <para>
      The first call by the client of the writer SHOULD be startOutput. 
      The writer MUST ignore any other RdfWriter method calls before startOutput has been called.
    </para>
    <para>   
      The last call by the client SHOULD be endOutput. 
      The writer MUST ignore any other RdfWriter method calls after endOutput has been called.
     </para>
    <para>   
      The client SHOULD not call endSubject without first calling startSubject. 
      The client SHOULD ensure that the number of endSubject calls equals the number of startSubject calls
      The writer MUST ignore any call to endSubject that does not have a corresponding startSubject
     </para>

    <para>   
      The client SHOULD not call startPredicate without first calling startSubject. 
      The writer MUST ignore any call to startPredicate if startSubject has not be called.
     </para>

    <para>   
      The client SHOULD not call endPredicate without first calling startPredicate. 
      The writer MUST ignore any call to endPredicate if startPredicate has not be called.
     </para>

    <para>   
      The client SHOULD not call startPredicate without ending any previous predicate 
      The writer MUST execute an endPredicate call if startPredicate is called without ending any  previous predicate
     </para>

    <para>   
      The client SHOULD not call  startObject without first calling startPredicate. 
      The writer MUST ignore any call to startObject if startPredicate has not be called.
     </para>

    <para>   
      The client SHOULD not call  endObject without first calling startObject. 
      The writer MUST ignore any call to endObject if startObject has not be called.
     </para>

    <para>   
      The client SHOULD not call startObject without ending any previous object 
      The writer MUST execute an endObject call if startObject is called without ending any  previous object
     </para>

	 </summary>
   <remarks>$Id: RdfWriter.cs,v 1.2 2005/05/26 14:24:30 ian Exp $</remarks>
  */
  public interface RdfWriter {
    void StartOutput();
    void EndOutput();

    void StartSubject();
    void EndSubject();

    void StartPredicate();
    void EndPredicate();

    void StartObject();
    void EndObject();

    void WriteUriRef(string uriRef);
    void WritePlainLiteral(string lexicalValue);
    void WritePlainLiteral(string lexicalValue, string language);
    void WriteTypedLiteral(string lexicalValue, string uriRef);
    void WriteBlankNode(string nodeId);
  }
}