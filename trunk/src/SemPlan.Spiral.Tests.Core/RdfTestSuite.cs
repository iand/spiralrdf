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

namespace SemPlan.Spiral.Tests.Core {
  using SemPlan.Spiral.Core;
  using SemPlan.Spiral.Utility;
  using System;
  using System.IO;
  using System.Collections;
  using System.Text;
  using System.Xml;
	/// <summary>
	/// Runs the supplied parser against all the tests in the W3C RDF Test Suite
	/// </summary>
  /// <remarks>
  /// $Id: RdfTestSuite.cs,v 1.2 2005/05/26 14:24:30 ian Exp $
  ///</remarks>
  public class RdfTestSuite {
    private ParserFactory itsParserFactory;
    private ArrayList itsParserTests;
    private StringBuilder itsFailureDescription;
    
    public RdfTestSuite(ParserFactory parserFactory) {
      itsParserTests = new ArrayList();
      itsFailureDescription = new StringBuilder();
      itsParserFactory = parserFactory;
      
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/amp-in-url/test001.rdf", "rdfcore-test-cases/amp-in-url/test001.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/amp-in-url/test001.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/datatypes/test001.rdf", "rdfcore-test-cases/datatypes/test001.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/datatypes/test001.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/datatypes/test002.rdf", "rdfcore-test-cases/datatypes/test002.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/datatypes/test002.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdf-charmod-literals/test001.rdf", "rdfcore-test-cases/rdf-charmod-literals/test001.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdf-charmod-literals/test001.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdf-charmod-uris/test001.rdf", "rdfcore-test-cases/rdf-charmod-uris/test001.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdf-charmod-uris/test001.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdf-charmod-uris/test002.rdf", "rdfcore-test-cases/rdf-charmod-uris/test002.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdf-charmod-uris/test002.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdf-containers-syntax-vs-schema/test001.rdf", "rdfcore-test-cases/rdf-containers-syntax-vs-schema/test001.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdf-containers-syntax-vs-schema/test001.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdf-containers-syntax-vs-schema/test002.rdf", "rdfcore-test-cases/rdf-containers-syntax-vs-schema/test002.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdf-containers-syntax-vs-schema/test002.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdf-containers-syntax-vs-schema/test003.rdf", "rdfcore-test-cases/rdf-containers-syntax-vs-schema/test003.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdf-containers-syntax-vs-schema/test003.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdf-containers-syntax-vs-schema/test004.rdf", "rdfcore-test-cases/rdf-containers-syntax-vs-schema/test004.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdf-containers-syntax-vs-schema/test004.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdf-containers-syntax-vs-schema/test006.rdf", "rdfcore-test-cases/rdf-containers-syntax-vs-schema/test006.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdf-containers-syntax-vs-schema/test006.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdf-containers-syntax-vs-schema/test007.rdf", "rdfcore-test-cases/rdf-containers-syntax-vs-schema/test007.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdf-containers-syntax-vs-schema/test007.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdf-containers-syntax-vs-schema/test008.rdf", "rdfcore-test-cases/rdf-containers-syntax-vs-schema/test008.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdf-containers-syntax-vs-schema/test008.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdf-element-not-mandatory/test001.rdf", "rdfcore-test-cases/rdf-element-not-mandatory/test001.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdf-element-not-mandatory/test001.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdf-ns-prefix-confusion/test0001.rdf", "rdfcore-test-cases/rdf-ns-prefix-confusion/test0001.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdf-ns-prefix-confusion/test0001.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdf-ns-prefix-confusion/test0003.rdf", "rdfcore-test-cases/rdf-ns-prefix-confusion/test0003.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdf-ns-prefix-confusion/test0003.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdf-ns-prefix-confusion/test0004.rdf", "rdfcore-test-cases/rdf-ns-prefix-confusion/test0004.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdf-ns-prefix-confusion/test0004.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdf-ns-prefix-confusion/test0005.rdf", "rdfcore-test-cases/rdf-ns-prefix-confusion/test0005.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdf-ns-prefix-confusion/test0005.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdf-ns-prefix-confusion/test0006.rdf", "rdfcore-test-cases/rdf-ns-prefix-confusion/test0006.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdf-ns-prefix-confusion/test0006.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdf-ns-prefix-confusion/test0009.rdf", "rdfcore-test-cases/rdf-ns-prefix-confusion/test0009.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdf-ns-prefix-confusion/test0009.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdf-ns-prefix-confusion/test0010.rdf", "rdfcore-test-cases/rdf-ns-prefix-confusion/test0010.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdf-ns-prefix-confusion/test0010.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdf-ns-prefix-confusion/test0011.rdf", "rdfcore-test-cases/rdf-ns-prefix-confusion/test0011.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdf-ns-prefix-confusion/test0011.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdf-ns-prefix-confusion/test0012.rdf", "rdfcore-test-cases/rdf-ns-prefix-confusion/test0012.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdf-ns-prefix-confusion/test0012.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdf-ns-prefix-confusion/test0013.rdf", "rdfcore-test-cases/rdf-ns-prefix-confusion/test0013.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdf-ns-prefix-confusion/test0013.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdf-ns-prefix-confusion/test0014.rdf", "rdfcore-test-cases/rdf-ns-prefix-confusion/test0014.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdf-ns-prefix-confusion/test0014.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-difference-between-ID-and-about/test1.rdf", "rdfcore-test-cases/rdfms-difference-between-ID-and-about/test1.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-difference-between-ID-and-about/test1.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-difference-between-ID-and-about/test2.rdf", "rdfcore-test-cases/rdfms-difference-between-ID-and-about/test2.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-difference-between-ID-and-about/test2.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-difference-between-ID-and-about/test3.rdf", "rdfcore-test-cases/rdfms-difference-between-ID-and-about/test3.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-difference-between-ID-and-about/test3.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-duplicate-member-props/test001.rdf", "rdfcore-test-cases/rdfms-duplicate-member-props/test001.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-duplicate-member-props/test001.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-empty-property-elements/test001.rdf", "rdfcore-test-cases/rdfms-empty-property-elements/test001.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-empty-property-elements/test001.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-empty-property-elements/test002.rdf", "rdfcore-test-cases/rdfms-empty-property-elements/test002.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-empty-property-elements/test002.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-empty-property-elements/test003.rdf", "rdfcore-test-cases/rdfms-empty-property-elements/test003.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-empty-property-elements/test003.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-empty-property-elements/test004.rdf", "rdfcore-test-cases/rdfms-empty-property-elements/test004.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-empty-property-elements/test004.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-empty-property-elements/test005.rdf", "rdfcore-test-cases/rdfms-empty-property-elements/test005.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-empty-property-elements/test005.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-empty-property-elements/test006.rdf", "rdfcore-test-cases/rdfms-empty-property-elements/test006.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-empty-property-elements/test006.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-empty-property-elements/test007.rdf", "rdfcore-test-cases/rdfms-empty-property-elements/test007.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-empty-property-elements/test007.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-empty-property-elements/test008.rdf", "rdfcore-test-cases/rdfms-empty-property-elements/test008.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-empty-property-elements/test008.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-empty-property-elements/test009.rdf", "rdfcore-test-cases/rdfms-empty-property-elements/test009.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-empty-property-elements/test009.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-empty-property-elements/test010.rdf", "rdfcore-test-cases/rdfms-empty-property-elements/test010.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-empty-property-elements/test010.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-empty-property-elements/test011.rdf", "rdfcore-test-cases/rdfms-empty-property-elements/test011.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-empty-property-elements/test011.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-empty-property-elements/test012.rdf", "rdfcore-test-cases/rdfms-empty-property-elements/test012.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-empty-property-elements/test012.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-empty-property-elements/test013.rdf", "rdfcore-test-cases/rdfms-empty-property-elements/test013.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-empty-property-elements/test013.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-empty-property-elements/test014.rdf", "rdfcore-test-cases/rdfms-empty-property-elements/test014.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-empty-property-elements/test014.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-empty-property-elements/test015.rdf", "rdfcore-test-cases/rdfms-empty-property-elements/test015.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-empty-property-elements/test015.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-empty-property-elements/test016.rdf", "rdfcore-test-cases/rdfms-empty-property-elements/test016.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-empty-property-elements/test016.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-empty-property-elements/test017.rdf", "rdfcore-test-cases/rdfms-empty-property-elements/test017.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-empty-property-elements/test017.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-identity-anon-resources/test001.rdf", "rdfcore-test-cases/rdfms-identity-anon-resources/test001.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-identity-anon-resources/test001.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-identity-anon-resources/test002.rdf", "rdfcore-test-cases/rdfms-identity-anon-resources/test002.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-identity-anon-resources/test002.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-identity-anon-resources/test003.rdf", "rdfcore-test-cases/rdfms-identity-anon-resources/test003.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-identity-anon-resources/test003.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-identity-anon-resources/test004.rdf", "rdfcore-test-cases/rdfms-identity-anon-resources/test004.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-identity-anon-resources/test004.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-identity-anon-resources/test005.rdf", "rdfcore-test-cases/rdfms-identity-anon-resources/test005.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-identity-anon-resources/test005.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-not-id-and-resource-attr/test001.rdf", "rdfcore-test-cases/rdfms-not-id-and-resource-attr/test001.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-not-id-and-resource-attr/test001.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-not-id-and-resource-attr/test002.rdf", "rdfcore-test-cases/rdfms-not-id-and-resource-attr/test002.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-not-id-and-resource-attr/test002.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-not-id-and-resource-attr/test004.rdf", "rdfcore-test-cases/rdfms-not-id-and-resource-attr/test004.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-not-id-and-resource-attr/test004.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-not-id-and-resource-attr/test005.rdf", "rdfcore-test-cases/rdfms-not-id-and-resource-attr/test005.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-not-id-and-resource-attr/test005.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-para196/test001.rdf", "rdfcore-test-cases/rdfms-para196/test001.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-para196/test001.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-rdf-names-use/test-001.rdf", "rdfcore-test-cases/rdfms-rdf-names-use/test-001.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-names-use/test-001.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-rdf-names-use/test-002.rdf", "rdfcore-test-cases/rdfms-rdf-names-use/test-002.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-names-use/test-002.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-rdf-names-use/test-003.rdf", "rdfcore-test-cases/rdfms-rdf-names-use/test-003.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-names-use/test-003.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-rdf-names-use/test-004.rdf", "rdfcore-test-cases/rdfms-rdf-names-use/test-004.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-names-use/test-004.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-rdf-names-use/test-005.rdf", "rdfcore-test-cases/rdfms-rdf-names-use/test-005.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-names-use/test-005.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-rdf-names-use/test-006.rdf", "rdfcore-test-cases/rdfms-rdf-names-use/test-006.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-names-use/test-006.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-rdf-names-use/test-007.rdf", "rdfcore-test-cases/rdfms-rdf-names-use/test-007.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-names-use/test-007.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-rdf-names-use/test-008.rdf", "rdfcore-test-cases/rdfms-rdf-names-use/test-008.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-names-use/test-008.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-rdf-names-use/test-009.rdf", "rdfcore-test-cases/rdfms-rdf-names-use/test-009.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-names-use/test-009.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-rdf-names-use/test-010.rdf", "rdfcore-test-cases/rdfms-rdf-names-use/test-010.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-names-use/test-010.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-rdf-names-use/test-011.rdf", "rdfcore-test-cases/rdfms-rdf-names-use/test-011.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-names-use/test-011.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-rdf-names-use/test-012.rdf", "rdfcore-test-cases/rdfms-rdf-names-use/test-012.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-names-use/test-012.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-rdf-names-use/test-013.rdf", "rdfcore-test-cases/rdfms-rdf-names-use/test-013.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-names-use/test-013.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-rdf-names-use/test-014.rdf", "rdfcore-test-cases/rdfms-rdf-names-use/test-014.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-names-use/test-014.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-rdf-names-use/test-015.rdf", "rdfcore-test-cases/rdfms-rdf-names-use/test-015.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-names-use/test-015.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-rdf-names-use/test-016.rdf", "rdfcore-test-cases/rdfms-rdf-names-use/test-016.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-names-use/test-016.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-rdf-names-use/test-017.rdf", "rdfcore-test-cases/rdfms-rdf-names-use/test-017.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-names-use/test-017.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-rdf-names-use/test-018.rdf", "rdfcore-test-cases/rdfms-rdf-names-use/test-018.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-names-use/test-018.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-rdf-names-use/test-019.rdf", "rdfcore-test-cases/rdfms-rdf-names-use/test-019.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-names-use/test-019.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-rdf-names-use/test-020.rdf", "rdfcore-test-cases/rdfms-rdf-names-use/test-020.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-names-use/test-020.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-rdf-names-use/test-021.rdf", "rdfcore-test-cases/rdfms-rdf-names-use/test-021.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-names-use/test-021.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-rdf-names-use/test-022.rdf", "rdfcore-test-cases/rdfms-rdf-names-use/test-022.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-names-use/test-022.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-rdf-names-use/test-023.rdf", "rdfcore-test-cases/rdfms-rdf-names-use/test-023.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-names-use/test-023.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-rdf-names-use/test-024.rdf", "rdfcore-test-cases/rdfms-rdf-names-use/test-024.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-names-use/test-024.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-rdf-names-use/test-025.rdf", "rdfcore-test-cases/rdfms-rdf-names-use/test-025.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-names-use/test-025.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-rdf-names-use/test-026.rdf", "rdfcore-test-cases/rdfms-rdf-names-use/test-026.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-names-use/test-026.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-rdf-names-use/test-027.rdf", "rdfcore-test-cases/rdfms-rdf-names-use/test-027.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-names-use/test-027.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-rdf-names-use/test-028.rdf", "rdfcore-test-cases/rdfms-rdf-names-use/test-028.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-names-use/test-028.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-rdf-names-use/test-029.rdf", "rdfcore-test-cases/rdfms-rdf-names-use/test-029.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-names-use/test-029.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-rdf-names-use/test-030.rdf", "rdfcore-test-cases/rdfms-rdf-names-use/test-030.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-names-use/test-030.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-rdf-names-use/test-031.rdf", "rdfcore-test-cases/rdfms-rdf-names-use/test-031.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-names-use/test-031.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-rdf-names-use/test-032.rdf", "rdfcore-test-cases/rdfms-rdf-names-use/test-032.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-names-use/test-032.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-rdf-names-use/test-033.rdf", "rdfcore-test-cases/rdfms-rdf-names-use/test-033.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-names-use/test-033.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-rdf-names-use/test-034.rdf", "rdfcore-test-cases/rdfms-rdf-names-use/test-034.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-names-use/test-034.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-rdf-names-use/test-035.rdf", "rdfcore-test-cases/rdfms-rdf-names-use/test-035.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-names-use/test-035.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-rdf-names-use/test-036.rdf", "rdfcore-test-cases/rdfms-rdf-names-use/test-036.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-names-use/test-036.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-rdf-names-use/test-037.rdf", "rdfcore-test-cases/rdfms-rdf-names-use/test-037.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-names-use/test-037.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-rdf-names-use/warn-001.rdf", "rdfcore-test-cases/rdfms-rdf-names-use/warn-001.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-names-use/warn-001.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-rdf-names-use/warn-002.rdf", "rdfcore-test-cases/rdfms-rdf-names-use/warn-002.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-names-use/warn-002.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-rdf-names-use/warn-003.rdf", "rdfcore-test-cases/rdfms-rdf-names-use/warn-003.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-names-use/warn-003.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-reification-required/test001.rdf", "rdfcore-test-cases/rdfms-reification-required/test001.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-reification-required/test001.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-seq-representation/test001.rdf", "rdfcore-test-cases/rdfms-seq-representation/test001.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-seq-representation/test001.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-syntax-incomplete/test001.rdf", "rdfcore-test-cases/rdfms-syntax-incomplete/test001.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-syntax-incomplete/test001.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-syntax-incomplete/test002.rdf", "rdfcore-test-cases/rdfms-syntax-incomplete/test002.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-syntax-incomplete/test002.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-syntax-incomplete/test003.rdf", "rdfcore-test-cases/rdfms-syntax-incomplete/test003.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-syntax-incomplete/test003.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-syntax-incomplete/test004.rdf", "rdfcore-test-cases/rdfms-syntax-incomplete/test004.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-syntax-incomplete/test004.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-uri-substructure/test001.rdf", "rdfcore-test-cases/rdfms-uri-substructure/test001.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-uri-substructure/test001.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-xml-literal-namespaces/test001.rdf", "rdfcore-test-cases/rdfms-xml-literal-namespaces/test001.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-xml-literal-namespaces/test001.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-xml-literal-namespaces/test002.rdf", "rdfcore-test-cases/rdfms-xml-literal-namespaces/test002.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-xml-literal-namespaces/test002.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-xmllang/test001.rdf", "rdfcore-test-cases/rdfms-xmllang/test001.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-xmllang/test001.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-xmllang/test002.rdf", "rdfcore-test-cases/rdfms-xmllang/test002.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-xmllang/test002.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-xmllang/test003.rdf", "rdfcore-test-cases/rdfms-xmllang/test003.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-xmllang/test003.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-xmllang/test004.rdf", "rdfcore-test-cases/rdfms-xmllang/test004.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-xmllang/test004.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-xmllang/test005.rdf", "rdfcore-test-cases/rdfms-xmllang/test005.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-xmllang/test005.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfms-xmllang/test006.rdf", "rdfcore-test-cases/rdfms-xmllang/test006.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-xmllang/test006.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfs-domain-and-range/test001.rdf", "rdfcore-test-cases/rdfs-domain-and-range/test001.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfs-domain-and-range/test001.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/rdfs-domain-and-range/test002.rdf", "rdfcore-test-cases/rdfs-domain-and-range/test002.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfs-domain-and-range/test002.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/unrecognised-xml-attributes/test001.rdf", "rdfcore-test-cases/unrecognised-xml-attributes/test001.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/unrecognised-xml-attributes/test001.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/unrecognised-xml-attributes/test002.rdf", "rdfcore-test-cases/unrecognised-xml-attributes/test002.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/unrecognised-xml-attributes/test002.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/xml-canon/test001.rdf", "rdfcore-test-cases/xml-canon/test001.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/xml-canon/test001.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/xmlbase/test001.rdf", "rdfcore-test-cases/xmlbase/test001.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/xmlbase/test001.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/xmlbase/test002.rdf", "rdfcore-test-cases/xmlbase/test002.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/xmlbase/test002.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/xmlbase/test003.rdf", "rdfcore-test-cases/xmlbase/test003.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/xmlbase/test003.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/xmlbase/test004.rdf", "rdfcore-test-cases/xmlbase/test004.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/xmlbase/test004.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/xmlbase/test006.rdf", "rdfcore-test-cases/xmlbase/test006.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/xmlbase/test006.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/xmlbase/test007.rdf", "rdfcore-test-cases/xmlbase/test007.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/xmlbase/test007.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/xmlbase/test008.rdf", "rdfcore-test-cases/xmlbase/test008.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/xmlbase/test008.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/xmlbase/test009.rdf", "rdfcore-test-cases/xmlbase/test009.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/xmlbase/test009.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/xmlbase/test010.rdf", "rdfcore-test-cases/xmlbase/test010.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/xmlbase/test010.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/xmlbase/test011.rdf", "rdfcore-test-cases/xmlbase/test011.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/xmlbase/test011.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/xmlbase/test013.rdf", "rdfcore-test-cases/xmlbase/test013.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/xmlbase/test013.rdf", itsParserFactory) );
      itsParserTests.Add( new PositiveParserTest("rdfcore-test-cases/xmlbase/test014.rdf", "rdfcore-test-cases/xmlbase/test014.nt", "http://www.w3.org/2000/10/rdf-tests/rdfcore/xmlbase/test014.rdf", itsParserFactory) );
      itsParserTests.Add( new NegativeParserTest("rdfcore-test-cases/rdf-containers-syntax-vs-schema/error001.rdf", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdf-containers-syntax-vs-schema/error001.rdf", itsParserFactory) );
      itsParserTests.Add( new NegativeParserTest("rdfcore-test-cases/rdf-containers-syntax-vs-schema/error002.rdf", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdf-containers-syntax-vs-schema/error002.rdf", itsParserFactory) );
      itsParserTests.Add( new NegativeParserTest("rdfcore-test-cases/rdfms-abouteach/error001.rdf", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-abouteach/error001.rdf", itsParserFactory) );
      itsParserTests.Add( new NegativeParserTest("rdfcore-test-cases/rdfms-abouteach/error002.rdf", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-abouteach/error002.rdf", itsParserFactory) );
      itsParserTests.Add( new NegativeParserTest("rdfcore-test-cases/rdfms-difference-between-ID-and-about/error1.rdf", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-difference-between-ID-and-about/error1.rdf", itsParserFactory) );
      itsParserTests.Add( new NegativeParserTest("rdfcore-test-cases/rdfms-empty-property-elements/error001.rdf", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-empty-property-elements/error001.rdf", itsParserFactory) );
      itsParserTests.Add( new NegativeParserTest("rdfcore-test-cases/rdfms-empty-property-elements/error002.rdf", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-empty-property-elements/error002.rdf", itsParserFactory) );
      itsParserTests.Add( new NegativeParserTest("rdfcore-test-cases/rdfms-empty-property-elements/error003.rdf", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-empty-property-elements/error003.rdf", itsParserFactory) );
      itsParserTests.Add( new NegativeParserTest("rdfcore-test-cases/rdfms-rdf-id/error001.rdf", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-id/error001.rdf", itsParserFactory) );
      itsParserTests.Add( new NegativeParserTest("rdfcore-test-cases/rdfms-rdf-id/error002.rdf", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-id/error002.rdf", itsParserFactory) );
      itsParserTests.Add( new NegativeParserTest("rdfcore-test-cases/rdfms-rdf-id/error003.rdf", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-id/error003.rdf", itsParserFactory) );
      itsParserTests.Add( new NegativeParserTest("rdfcore-test-cases/rdfms-rdf-id/error004.rdf", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-id/error004.rdf", itsParserFactory) );
      itsParserTests.Add( new NegativeParserTest("rdfcore-test-cases/rdfms-rdf-id/error005.rdf", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-id/error005.rdf", itsParserFactory) );
      itsParserTests.Add( new NegativeParserTest("rdfcore-test-cases/rdfms-rdf-id/error006.rdf", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-id/error006.rdf", itsParserFactory) );
      itsParserTests.Add( new NegativeParserTest("rdfcore-test-cases/rdfms-rdf-id/error007.rdf", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-id/error007.rdf", itsParserFactory) );
      itsParserTests.Add( new NegativeParserTest("rdfcore-test-cases/rdfms-rdf-names-use/error-001.rdf", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-names-use/error-001.rdf", itsParserFactory) );
      itsParserTests.Add( new NegativeParserTest("rdfcore-test-cases/rdfms-rdf-names-use/error-002.rdf", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-names-use/error-002.rdf", itsParserFactory) );
      itsParserTests.Add( new NegativeParserTest("rdfcore-test-cases/rdfms-rdf-names-use/error-003.rdf", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-names-use/error-003.rdf", itsParserFactory) );
      itsParserTests.Add( new NegativeParserTest("rdfcore-test-cases/rdfms-rdf-names-use/error-004.rdf", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-names-use/error-004.rdf", itsParserFactory) );
      itsParserTests.Add( new NegativeParserTest("rdfcore-test-cases/rdfms-rdf-names-use/error-005.rdf", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-names-use/error-005.rdf", itsParserFactory) );
      itsParserTests.Add( new NegativeParserTest("rdfcore-test-cases/rdfms-rdf-names-use/error-006.rdf", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-names-use/error-006.rdf", itsParserFactory) );
      itsParserTests.Add( new NegativeParserTest("rdfcore-test-cases/rdfms-rdf-names-use/error-007.rdf", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-names-use/error-007.rdf", itsParserFactory) );
      itsParserTests.Add( new NegativeParserTest("rdfcore-test-cases/rdfms-rdf-names-use/error-008.rdf", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-names-use/error-008.rdf", itsParserFactory) );
      itsParserTests.Add( new NegativeParserTest("rdfcore-test-cases/rdfms-rdf-names-use/error-009.rdf", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-names-use/error-009.rdf", itsParserFactory) );
      itsParserTests.Add( new NegativeParserTest("rdfcore-test-cases/rdfms-rdf-names-use/error-010.rdf", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-names-use/error-010.rdf", itsParserFactory) );
      itsParserTests.Add( new NegativeParserTest("rdfcore-test-cases/rdfms-rdf-names-use/error-011.rdf", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-names-use/error-011.rdf", itsParserFactory) );
      itsParserTests.Add( new NegativeParserTest("rdfcore-test-cases/rdfms-rdf-names-use/error-012.rdf", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-names-use/error-012.rdf", itsParserFactory) );
      itsParserTests.Add( new NegativeParserTest("rdfcore-test-cases/rdfms-rdf-names-use/error-013.rdf", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-names-use/error-013.rdf", itsParserFactory) );
      itsParserTests.Add( new NegativeParserTest("rdfcore-test-cases/rdfms-rdf-names-use/error-014.rdf", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-names-use/error-014.rdf", itsParserFactory) );
      itsParserTests.Add( new NegativeParserTest("rdfcore-test-cases/rdfms-rdf-names-use/error-015.rdf", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-names-use/error-015.rdf", itsParserFactory) );
      itsParserTests.Add( new NegativeParserTest("rdfcore-test-cases/rdfms-rdf-names-use/error-016.rdf", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-names-use/error-016.rdf", itsParserFactory) );
      itsParserTests.Add( new NegativeParserTest("rdfcore-test-cases/rdfms-rdf-names-use/error-017.rdf", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-names-use/error-017.rdf", itsParserFactory) );
      itsParserTests.Add( new NegativeParserTest("rdfcore-test-cases/rdfms-rdf-names-use/error-018.rdf", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-names-use/error-018.rdf", itsParserFactory) );
      itsParserTests.Add( new NegativeParserTest("rdfcore-test-cases/rdfms-rdf-names-use/error-019.rdf", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-names-use/error-019.rdf", itsParserFactory) );
      itsParserTests.Add( new NegativeParserTest("rdfcore-test-cases/rdfms-rdf-names-use/error-020.rdf", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-rdf-names-use/error-020.rdf", itsParserFactory) );
      itsParserTests.Add( new NegativeParserTest("rdfcore-test-cases/rdfms-syntax-incomplete/error001.rdf", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-syntax-incomplete/error001.rdf", itsParserFactory) );
      itsParserTests.Add( new NegativeParserTest("rdfcore-test-cases/rdfms-syntax-incomplete/error002.rdf", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-syntax-incomplete/error002.rdf", itsParserFactory) );
      itsParserTests.Add( new NegativeParserTest("rdfcore-test-cases/rdfms-syntax-incomplete/error003.rdf", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-syntax-incomplete/error003.rdf", itsParserFactory) );
      itsParserTests.Add( new NegativeParserTest("rdfcore-test-cases/rdfms-syntax-incomplete/error004.rdf", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-syntax-incomplete/error004.rdf", itsParserFactory) );
      itsParserTests.Add( new NegativeParserTest("rdfcore-test-cases/rdfms-syntax-incomplete/error005.rdf", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-syntax-incomplete/error005.rdf", itsParserFactory) );
      itsParserTests.Add( new NegativeParserTest("rdfcore-test-cases/rdfms-syntax-incomplete/error006.rdf", "http://www.w3.org/2000/10/rdf-tests/rdfcore/rdfms-syntax-incomplete/error006.rdf", itsParserFactory) );

    }
    
    
    public int RunTests() {
      int passCount = 0;
      
      foreach (object parserTest in itsParserTests) {
        if ( ((ParserTest)parserTest).Run()) {
          ++passCount;
        }
        else {
          itsFailureDescription.Append( ((ParserTest)parserTest).GetFailureDescription() );
        }
      }

      return passCount;
    }
    
    public int Count {
      get {
        return itsParserTests.Count;
      }
    }
    
    public string GetFailureDescription() {
      return itsFailureDescription.ToString();
    }
    
    public interface ParserTest {
      bool Run();
      string GetFailureDescription();
    }
    
    public class PositiveParserTest : ParserTest {
      private string itsInputFileName;
      private string itsOutputFileName;
      private string itsBaseUri;
      private ParserFactory itsParserFactory;
      private string itsFailureDescription;
      
      public PositiveParserTest(string inputFileName, string outputFileName, string baseUri, ParserFactory parserFactory) {
         itsInputFileName = inputFileName;
         itsOutputFileName = outputFileName;
         itsBaseUri= baseUri;
         itsParserFactory = parserFactory;
      }
      
      public bool Run() {
        string failureDescription = "";
        
        NTripleListVerifier verifier = new NTripleListVerifier();
        FileStream expectedStream = new FileStream(itsOutputFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
        StreamReader expectedStreamReader = new StreamReader(expectedStream);
        
        string expectedLine = expectedStreamReader.ReadLine();
        while (expectedLine != null) {
          string trimmed = expectedLine.Trim();
          if (trimmed.Length > 0 &&  ! trimmed.StartsWith("#") ) {
            verifier.Expect( trimmed );
           }
          expectedLine = expectedStreamReader.ReadLine();
        }
        
        
        FileStream inputStream = new FileStream(itsInputFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
        
        bool testPassed = false;
        
        try {
          SimpleModel model = new SimpleModel(itsParserFactory);
          model.Parse( inputStream, itsBaseUri);
          
          foreach (string ntriple in model) {
            verifier.Receive(ntriple);
          }
    
          testPassed = verifier.Verify();
          failureDescription = verifier.GetLastFailureDescription() + "\r\nTriples received:\r\n" + model.ToString();
    
        }
        catch (Exception e) {
          failureDescription = "Parse exception: " + e;
        }
        
        if ( ! testPassed) {
          itsFailureDescription ="FAILED " + itsBaseUri + "\r\nReason: " + failureDescription + "\r\n";
        }
        return testPassed;  
      }
      
      public string GetFailureDescription() {
        return itsFailureDescription;
      }
      
    }


    public class NegativeParserTest : ParserTest {
      private string itsInputFileName;
      private string itsBaseUri;
      private ParserFactory itsParserFactory;
      private string itsFailureDescription;
      
      public NegativeParserTest(string inputFileName, string baseUri, ParserFactory parserFactory) {
         itsInputFileName = inputFileName;
         itsBaseUri= baseUri;
         itsParserFactory = parserFactory;
      }
      
      public bool Run() {
        SimpleModel model = new SimpleModel(itsParserFactory);
        Parser parser = itsParserFactory.MakeParser( new ResourceFactory(), new StatementFactory() );
  
        parser.NewStatement += new StatementHandler(model.Add);
        FileStream inputStream = new FileStream(itsInputFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
        
      
        try {
          parser.Parse( inputStream, itsBaseUri );

          if ( model.Count > 0) {
            itsFailureDescription = "FAILED " + itsBaseUri + "\r\nReason: No triples should be returned by parser but " + model.Count + " were received.\r\n";
            return false;
          }

        }
        catch (Exception) {
          // Ignore
        }

          return true;

      }
      
      public string GetFailureDescription() {
        return itsFailureDescription;
      }
      
    }

  }

}