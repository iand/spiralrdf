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


namespace SemPlan.Spiral.Tests.Utility {
  using NUnit.Framework;
  using SemPlan.Spiral.Utility;
  using SemPlan.Spiral.XsltParser;
  using SemPlan.Spiral.Tests.Core;
  using System;
  using System.IO;
  using System.Xml;
	/// <summary>
	/// Programmer tests for NTripleWriter class
	/// </summary>
  /// <remarks>
  /// $Id: RdfXmlWriterAcidTest.cs,v 1.2 2005/05/26 14:24:31 ian Exp $
  ///</remarks>
  [TestFixture]
  public class NTripleWriterAcidTest {
    [Test]
    public void acidTest() {
      
      
      SimpleModel expectedModel = new SimpleModel(new XsltParserFactory());
      expectedModel.ParseString( COMPLEX_DOCUMENT, "http://example.com/");

/*
      RdfXmlWriter rdfWriterDump = new RdfXmlWriter( new XmlWriterDumper() );
      expectedModel.write( rdfWriterDump );
*/
    
      StringWriter outputWriter = new StringWriter();
      XmlTextWriter xmlWriter = new XmlTextWriter(outputWriter);

      
      RdfXmlWriter rdfWriter = new RdfXmlWriter( xmlWriter );
      
      // Pass the expected triples through the writer
      expectedModel.Write( rdfWriter );
      
      SimpleModel receivedModel = new SimpleModel(new XsltParserFactory());
      receivedModel.ParseString( outputWriter.ToString(), "http://example.com/");

/*
Console.WriteLine();
Console.WriteLine(  outputWriter.ToString() );
Console.WriteLine();
*/
      NTripleListVerifier verifier = new NTripleListVerifier();
      foreach (string ntriple in expectedModel) {
        verifier.Expect(ntriple);
      }
      foreach (string ntriple in receivedModel) {
        verifier.Receive(ntriple);
      }

      // NTripleListVerifier can't do full verify with large numbers of blank nodes
      Assert.IsTrue(  verifier.VerifyCounts() );

    
    }
    private const string COMPLEX_DOCUMENT = @"<?xml version='1.0' encoding='utf-8'?>
<rdf:RDF xmlns:rdf='http://www.w3.org/1999/02/22-rdf-syntax-ns#' xmlns:rdfs='http://www.w3.org/2000/01/rdf-schema#' xmlns:foaf='http://xmlns.com/foaf/0.1/' xmlns:dc='http://purl.org/dc/elements/1.1/' xmlns:contact='http://www.w3.org/2000/10/swap/pim/contact#' xmlns:air='http://www.daml.org/2001/10/html/airport-ont#' xmlns:wn='http://xmlns.com/wordnet/1.6/' xmlns:rss='http://purl.org/rss/1.0/' xmlns:terms='http://purl.org/dc/terms/' xmlns:rel='http://purl.org/vocab/relationship/' xmlns:geo='http://www.w3.org/2003/01/geo/wgs84_pos#' xmlns:aff='http://purl.org/vocab/affiliations/0.1/' xmlns:dcterms='http://purl.org/dc/terms/' xmlns:org='http://purl.org/vocab/organizations/0.1/' xmlns:bio='http://purl.org/vocab/bio/0.1/' xmlns:wot='http://xmlns.com/wot/0.1/' xmlns:psych='http://purl.org/vocab/psychometric-profile/'>
  <rdf:Description rdf:about=''>
    <dc:title>FOAF for Ian Davis</dc:title>
    <dc:description>Friend-of-a-Friend description for Ian Davis</dc:description>
    <dc:creator rdf:nodeID='ian'/>
    <foaf:topic rdf:nodeID='ian'/>
    <foaf:maker rdf:nodeID='ian'/>
    <dcterms:created>2004-02-10T00:29:21Z</dcterms:created>
    <wot:assurance rdf:resource='foaf-rdf.asc'/>
  </rdf:Description>
  <foaf:Person rdf:nodeID='ian'>
    <foaf:name>Ian Davis</foaf:name>
    <foaf:title>Mr</foaf:title>
    <foaf:firstName>Ian</foaf:firstName>
    <foaf:surname>Davis</foaf:surname>
    <foaf:nick>iand</foaf:nick>
    <foaf:mbox rdf:resource='mailto:iand@internetalchemy.org'/>
    <foaf:mbox_sha1sum>69e31bbcf58d432950127593e292a55975bc55fd</foaf:mbox_sha1sum>
    <foaf:weblog>
      <foaf:Document rdf:about='http://internetalchemy.org/'>
        <dc:title>Internet Alchemy Weblog</dc:title>
        <rdfs:seeAlso rdf:resource='http://internetalchemy.org/index.rdf'/>
      </foaf:Document>
    </foaf:weblog>
    <foaf:homepage rdf:resource='http://purl.org/NET/iand'/>
    <foaf:depiction rdf:resource='http://purl.org/NET/iand/depiction'/>
    <wot:pubkeyAddress rdf:resource='http://purl.org/NET/iand/publickey'/>
    <foaf:phone rdf:resource='tel:+44-7966-473-239'/>
    <foaf:workplaceHomepage rdf:resource='http://www.innovateer.com/'/>
    <foaf:schoolHomepage rdf:resource='http://www.rhul.ac.uk/'/>
    <contact:nearestAirport>
      <wn:Airport air:icao='EGGW' air:iata='LTN'/>
    </contact:nearestAirport>
    <geo:location>
      <geo:Position geo:lat='52.422400' geo:lon='-0.803000'/>
    </geo:location>
    <bio:olb>
        British; software developer; former CTO of Calaba; 
        author of OCS; co-author of RSS 1.0; developer of myRSS and Pepys.
    </bio:olb>
    <bio:event>
      <bio:Birth>
        <bio:date>1970-06-15</bio:date>
        <bio:place>Brentwood, Essex, United Kingdom</bio:place>
      </bio:Birth>
    </bio:event>
    <bio:event>
      <bio:Marriage>
        <bio:date>1995-08-04</bio:date>
        <bio:place>
          Northampton Register Office, 
          Northampton, Northamptonshire, United Kingdom
        </bio:place>
      </bio:Marriage>
    </bio:event>
    <psych:profile>
      <psych:PoliticalCompassProfile>
        <psych:economic>-0.38</psych:economic>
        <psych:social>-5.18</psych:social>
      </psych:PoliticalCompassProfile>
    </psych:profile>
    <foaf:interest>
      <rdf:Description rdf:about='http://xmlns.com/foaf/0.1' rdfs:label='Friend-of-a-Friend'/>
    </foaf:interest>
    <foaf:interest>
      <rdf:Description rdf:about='http://c2.com/cgi/wiki?WikiWikiWeb' rdfs:label='Wiki Wiki Webs'/>
    </foaf:interest>
    <foaf:interest>
      <rdf:Description rdf:about='http://www.w3.org/XML/' rdfs:label='Extensible Markup Language (XML)'/>
    </foaf:interest>
    <foaf:interest>
      <rdf:Description rdf:about='http://purl.org/rss' rdfs:label='RDF Site Summary (RSS)'/>
    </foaf:interest>
    <foaf:interest>
      <rdf:Description rdf:about='http://www.w3.org/Metadata/' rdfs:label='Metadata'/>
    </foaf:interest>
    <foaf:interest>
      <rdf:Description rdf:about='http://www.w3.org/2000/01/sw/' rdfs:label='Semantic Web Development'/>
    </foaf:interest>
    <foaf:interest>
      <rdf:Description rdf:about='http://www.w3.org/RDF/' rdfs:label='Resource Description Framework (RDF)'/>
    </foaf:interest>
    <foaf:knows rdf:nodeID='james'/>
    <foaf:knows rdf:nodeID='bill'/>
    <foaf:knows rdf:nodeID='jim'/>
    <foaf:knows rdf:nodeID='julian'/>
    <foaf:knows rdf:nodeID='aaronsw'/>
    <foaf:knows rdf:nodeID='eric'/>
    <foaf:knows rdf:nodeID='danny'/>
    <foaf:knows rdf:nodeID='sue'/>
    <foaf:knows rdf:nodeID='mike'/>
    <foaf:knows rdf:nodeID='steph'/>
    <foaf:knows rdf:nodeID='kier'/>
    <foaf:knows rdf:nodeID='freya'/>
    <foaf:knows rdf:nodeID='danbri'/>
    <rel:friendOf rdf:nodeID='james'/>
    <rel:acquaintanceOf rdf:nodeID='bill'/>
    <rel:acquaintanceOf rdf:nodeID='jim'/>
    <rel:acquaintanceOf rdf:nodeID='julian'/>
    <rel:acquaintanceOf rdf:nodeID='aaronsw'/>
    <rel:acquaintanceOf rdf:nodeID='eric'/>
    <rel:childOf rdf:nodeID='sue'/>
    <rel:childOf rdf:nodeID='mike'/>
    <rel:spouseOf rdf:nodeID='steph'/>
    <rel:parentOf rdf:nodeID='kier'/>
    <rel:parentOf rdf:nodeID='freya'/>
    <foaf:project>
      <rdf:Description>
        <dc:title>myRSS</dc:title>
        <dc:description>A general-purpose, heuristic news scraping engine</dc:description>
        <dc:identifier rdf:resource='http://myrss.com'/>
      </rdf:Description>
    </foaf:project>
    <foaf:project>
      <rdf:Description>
        <dc:title>Pepys</dc:title>
        <dc:description>A desktop, peer-to-peer wiki</dc:description>
        <dc:identifier rdf:resource='http://www.innovateer.com/products/pepys/'/>
      </rdf:Description>
    </foaf:project>
    <foaf:made>
      <rss:channel rdf:about='http://internetalchemy.org/index.rss'>
        <dc:title xml:lang='en'>Internet Alchemy Weblog RSS Feed</dc:title>
        <dc:description xml:lang='en'>An RSS feed of weblog postings to the Internet Alchemy weblog</dc:description>
      </rss:channel>
    </foaf:made>
    <aff:affiliation>
      <aff:Membership>
        <aff:memberOf>
          <org:Organization>
            <org:homepage rdf:resource='http://www.ecademy.com'/>
            <org:name>Ecademy</org:name>
          </org:Organization>
        </aff:memberOf>
        <aff:memberProfile rdf:resource='http://www.ecademy.com/account.php?op=view&amp;id=19774'/>
        <rdfs:seeAlso rdf:resource='http://www.ecademy.com/module.php?mod=network&amp;op=foafrdf&amp;uid=19774'/>
      </aff:Membership>
    </aff:affiliation>
    <aff:affiliation>
      <aff:Membership>
        <aff:memberOf>
          <org:Organization>
            <org:homepage rdf:resource='http://www.ryze.com'/>
            <org:name>Ryze</org:name>
          </org:Organization>
        </aff:memberOf>
        <aff:memberProfile rdf:resource='http://www.ryze.com/view.php?who=iand'/>
      </aff:Membership>
    </aff:affiliation>
    <aff:affiliation>
      <aff:Membership>
        <aff:memberOf>
          <org:Organization>
            <org:homepage rdf:resource='http://www.friendster.com'/>
            <org:name>Friendster</org:name>
          </org:Organization>
        </aff:memberOf>
        <aff:memberProfile rdf:resource='http://www.friendster.com/user.jsp?id=2118'/>
      </aff:Membership>
    </aff:affiliation>
    <aff:affiliation>
      <aff:Membership>
        <aff:memberOf>
          <org:Organization>
            <org:homepage rdf:resource='http://www.friendsreunited.co.uk/'/>
            <org:name>Friends Reunited</org:name>
          </org:Organization>
        </aff:memberOf>
        <aff:memberProfile rdf:resource='http://www.friendsreunited.co.uk/FriendsReunited.asp?wci=membernotes&amp;member_key=1841668'/>
      </aff:Membership>
    </aff:affiliation>
  </foaf:Person>
  <foaf:Person rdf:nodeID='james'>
    <foaf:name>James Carlyle</foaf:name>
    <foaf:mbox_sha1sum>9b7ac29183a3106b9ca8bc436d42f61ee284d147</foaf:mbox_sha1sum>
    <foaf:depiction rdf:resource='http://internetalchemy.org/iand/pics/jamesCarlyle.jpg'/>
    <rdfs:seeAlso rdf:resource='http://www.takepart.com/about/foaf.rdf'/>
  </foaf:Person>
  <foaf:Person rdf:nodeID='bill'>
    <foaf:name>Bill Kearney</foaf:name>
    <foaf:mbox_sha1sum>98783d46199c7733d0e452c93ba0cff0baa4b61b</foaf:mbox_sha1sum>
    <rdfs:seeAlso rdf:resource='http://www.ideaspace.net/users/wkearney/foaf.xrdf'/>
  </foaf:Person>
  <foaf:Person rdf:nodeID='jim'>
    <foaf:name>Jim Ley</foaf:name>
    <foaf:mbox_sha1sum>35022e505e6a64c05837eccf4beb5d8f981a4e5a</foaf:mbox_sha1sum>
    <rdfs:seeAlso rdf:resource='http://jibbering.com/foaf.rdf'/>
  </foaf:Person>
  <foaf:Person rdf:nodeID='julian'>
    <foaf:name>Julian Bond</foaf:name>
    <foaf:mbox_sha1sum>fc521285feac8d1de0a488166aeeb9dbf99070e1</foaf:mbox_sha1sum>
    <rdfs:seeAlso rdf:resource='http://www.ecademy.com/module.php?mod=network&amp;op=foafrdf&amp;uid=1'/>
  </foaf:Person>
  <foaf:Person rdf:nodeID='aaronsw'>
    <foaf:name>Aaron Swartz</foaf:name>
    <foaf:mbox_sha1sum>feb7e9a206907a123e3f66d276b79549f1a7f3de</foaf:mbox_sha1sum>
    <rdfs:seeAlso rdf:resource='http://www.aaronsw.com/about.xrdf'/>
  </foaf:Person>
  <foaf:Person rdf:nodeID='eric'>
    <foaf:name>Eric Vitiello Jr.</foaf:name>
    <foaf:mbox_sha1sum>5f93ed6a7346e6b8a767ebd87c407d4765b4e228</foaf:mbox_sha1sum>
    <rdfs:seeAlso rdf:resource='http://www.perceive.net/xml/foaf.rdf'/>
  </foaf:Person>
  <foaf:Person rdf:nodeID='steph'>
    <foaf:name>Stephanie Davis</foaf:name>
    <foaf:mbox_sha1sum>653f68f291c825fa0c895ac6f1c3418ae161812f</foaf:mbox_sha1sum>
    <foaf:knows rdf:nodeID='ian'/>
    <foaf:knows rdf:nodeID='kier'/>
    <foaf:knows rdf:nodeID='freya'/>
    <rel:spouseOf rdf:nodeID='ian'/>
    <rel:parentOf rdf:nodeID='kier'/>
    <rel:parentOf rdf:nodeID='freya'/>
  </foaf:Person>
  <foaf:Person rdf:nodeID='kier'>
    <foaf:name>Kier Davis</foaf:name>
    <foaf:mbox_sha1sum>b1e91635028f5ee5f79cbd49dbeafc330852d26a</foaf:mbox_sha1sum>
    <foaf:depiction rdf:resource='http://internetalchemy.org/iand/pics/gal20.jpg'/>
    <foaf:knows rdf:nodeID='ian'/>
    <foaf:knows rdf:nodeID='steph'/>
    <foaf:knows rdf:nodeID='freya'/>
    <rel:childOf rdf:nodeID='ian'/>
    <rel:childOf rdf:nodeID='steph'/>
    <rel:siblingOf rdf:nodeID='freya'/>
  </foaf:Person>
  <foaf:Person rdf:nodeID='freya'>
    <foaf:name>Freya Davis</foaf:name>
    <foaf:mbox_sha1sum>87fcd41b2a48a3ef0bee24e520eb48868fa30a56</foaf:mbox_sha1sum>
    <foaf:depiction rdf:resource='http://internetalchemy.org/iand/pics/gal22.jpg'/>
    <foaf:knows rdf:nodeID='ian'/>
    <foaf:knows rdf:nodeID='kier'/>
    <foaf:knows rdf:nodeID='steph'/>
    <rel:childOf rdf:nodeID='ian'/>
    <rel:siblingOf rdf:nodeID='kier'/>
    <rel:childOf rdf:nodeID='steph'/>
  </foaf:Person>
  <foaf:Person rdf:nodeID='sue'>
    <foaf:name>Susan Davis</foaf:name>
    <foaf:mbox_sha1sum>169596780130ef05a6ec79e628176690dcee2e80</foaf:mbox_sha1sum>
    <foaf:knows rdf:nodeID='ian'/>
    <rel:parentOf rdf:nodeID='ian'/>
  </foaf:Person>
  <foaf:Person rdf:nodeID='mike'>
    <foaf:name>Michael Davis</foaf:name>
    <foaf:mbox_sha1sum>9177877c1e05a927a7e800a1469115d3d8d9e86b</foaf:mbox_sha1sum>
    <foaf:knows rdf:nodeID='ian'/>
    <rel:parentOf rdf:nodeID='ian'/>
  </foaf:Person>
  <foaf:Person rdf:nodeID='danbri'>
    <foaf:name>Dan Brickley</foaf:name>
    <rdfs:seeAlso rdf:resource='http://rdfweb.org/people/danbri/rdfweb/webwho.xrdf'/>
    <foaf:mbox_sha1sum>362ce75324396f0aa2d3e5f1246f40bf3bb44401</foaf:mbox_sha1sum>
  </foaf:Person>
  <foaf:Person rdf:nodeID='danny'>
    <foaf:name>Danny Ayers</foaf:name>
    <rdfs:seeAlso rdf:resource='http://dannyayers.com/misc/foaf/foaf.rdf'/>
    <foaf:mbox_sha1sum>ff7a6a9e58db00ecce4aa2daa9667ec8ad64144f</foaf:mbox_sha1sum>
  </foaf:Person>
</rdf:RDF>";
  }
}
