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
  using System;
  using System.Collections;
  using System.Collections.Specialized;
  using System.Text.RegularExpressions;
  
	/// <summary>
	/// Compares a list of NTriples against an expected list to see if the two lists are equivilent
	/// </summary>
  /// <remarks>
  /// $Id: NTripleListVerifier.cs,v 1.2 2005/05/26 14:24:30 ian Exp $
  ///</remarks>
  public class NTripleListVerifier {
    private StringCollection itsExpectedTriples;
    private StringCollection itsReceivedTriples;

    private StringCollection itsExpectedBlankNodes;
    private StringCollection itsReceivedBlankNodes;
    
    private StringCollection itsExpectedNonBlankNodes;
    private StringCollection itsReceivedNonBlankNodes;

    private string itsLastFailureDescription = "";

    private static Regex tripleRegex = new Regex(@"^(\S+)\s+(\S+)\s+(\S.*\S)\s*\.\s*$", RegexOptions.Compiled); 
	  private static Regex blankNode = new Regex(@"^_:(\S+)*$", RegexOptions.Compiled);

    public NTripleListVerifier() {
      itsExpectedTriples = new StringCollection();
      itsReceivedTriples = new StringCollection();
      
      itsExpectedBlankNodes = new StringCollection();
      itsReceivedBlankNodes = new StringCollection();
      
      itsExpectedNonBlankNodes = new StringCollection();
      itsReceivedNonBlankNodes = new StringCollection();
    }

    public void Expect(string nTriple) {
    	Match matchTriple = tripleRegex.Match(nTriple);
			if (matchTriple.Success) {
				string subjectPart = matchTriple.Groups[1].Value;
        Match matchSubjectBlankNode = blankNode.Match(subjectPart);
        if (matchSubjectBlankNode.Success) {
				  string nodeId = matchSubjectBlankNode.Groups[1].Value;
          if (! itsExpectedBlankNodes.Contains(nodeId) ) {
            itsExpectedBlankNodes.Add(nodeId);
          }       
        }
        else {
          itsExpectedNonBlankNodes.Add(subjectPart);
        }

				string objectPart = matchTriple.Groups[3].Value;
        Match matchObjectBlankNode = blankNode.Match(objectPart);
        if (matchObjectBlankNode.Success) {
				  string nodeId = matchObjectBlankNode.Groups[1].Value;
          if (! itsExpectedBlankNodes.Contains(nodeId) ) {
            itsExpectedBlankNodes.Add(nodeId);
          }       
        }
        else {
          itsExpectedNonBlankNodes.Add(objectPart);
        }

        itsExpectedTriples.Add(matchTriple.Groups[1].Value + " " + matchTriple.Groups[2].Value + " " + matchTriple.Groups[3].Value + " .");
      }
    }
    

    public void Receive(string nTriple) {
    	Match matchTriple = tripleRegex.Match(nTriple);
			if (matchTriple.Success) {
				string subjectPart = matchTriple.Groups[1].Value;
        Match matchSubjectBlankNode = blankNode.Match(subjectPart);
        if (matchSubjectBlankNode.Success) {
				  string nodeId = matchSubjectBlankNode.Groups[1].Value;
          if (! itsReceivedBlankNodes.Contains(nodeId) ) {
            itsReceivedBlankNodes.Add(nodeId);
          }       
        }
        else {
          itsReceivedNonBlankNodes.Add(subjectPart);
        }

				string objectPart = matchTriple.Groups[3].Value;
        Match matchObjectBlankNode = blankNode.Match(objectPart);
        if (matchObjectBlankNode.Success) {
				  string nodeId = matchObjectBlankNode.Groups[1].Value;
          if (! itsReceivedBlankNodes.Contains(nodeId) ) {
            itsReceivedBlankNodes.Add(nodeId);
          }       
        }
        else {
          itsReceivedNonBlankNodes.Add(objectPart);
        }
        
      }
      itsReceivedTriples.Add(nTriple);
    }


    public bool VerifyCounts() {
     
      if (itsExpectedTriples.Count != itsReceivedTriples.Count) {
        itsLastFailureDescription = "Received " + itsReceivedTriples.Count +" triples but was expecting " +  itsExpectedTriples.Count;
        return false;
      }

      if (itsExpectedBlankNodes.Count != itsReceivedBlankNodes.Count) {
        itsLastFailureDescription = "Received " + itsReceivedBlankNodes.Count +" blank nodes but was expecting " +  itsExpectedBlankNodes.Count;
        return false;
      }

      if (itsExpectedNonBlankNodes.Count != itsReceivedNonBlankNodes.Count) {
        itsLastFailureDescription = "Received " + itsReceivedNonBlankNodes.Count +" non-blank nodes but was expecting " +  itsExpectedNonBlankNodes.Count;
        return false;
      }

      return true;

    }

    public bool Verify() {
      if ( ! VerifyCounts() ) {
        return false;
      }      

      bool verified = false;

      StringCollection mappedTriples = new StringCollection();
      if (itsReceivedBlankNodes.Count > 0 && itsExpectedBlankNodes.Count > 0) {
        ArrayList mappingPermutations = new ArrayList();
        
        
        if (itsReceivedBlankNodes.Count== 1) {
          Hashtable nodeIdMapping = new Hashtable();
          nodeIdMapping[itsReceivedBlankNodes[0]] = itsExpectedBlankNodes[0];
          mappingPermutations.Add(nodeIdMapping);
        }
       else if (itsReceivedBlankNodes.Count== 2) {
          Hashtable nodeIdMapping1= new Hashtable();
          nodeIdMapping1[itsReceivedBlankNodes[0]] = itsExpectedBlankNodes[0];
          nodeIdMapping1[itsReceivedBlankNodes[1]] = itsExpectedBlankNodes[1];
          mappingPermutations.Add(nodeIdMapping1);

          Hashtable nodeIdMapping2= new Hashtable();
          nodeIdMapping2[itsReceivedBlankNodes[0]] = itsExpectedBlankNodes[1];
          nodeIdMapping2[itsReceivedBlankNodes[1]] = itsExpectedBlankNodes[0];
          mappingPermutations.Add(nodeIdMapping2);
        }
       else if (itsReceivedBlankNodes.Count== 3) {
          Hashtable nodeIdMapping1= new Hashtable();
          nodeIdMapping1[itsReceivedBlankNodes[0]] = itsExpectedBlankNodes[0];
          nodeIdMapping1[itsReceivedBlankNodes[1]] = itsExpectedBlankNodes[1];
          nodeIdMapping1[itsReceivedBlankNodes[2]] = itsExpectedBlankNodes[2];
          mappingPermutations.Add(nodeIdMapping1);

          Hashtable nodeIdMapping2= new Hashtable();
          nodeIdMapping2[itsReceivedBlankNodes[0]] = itsExpectedBlankNodes[0];
          nodeIdMapping2[itsReceivedBlankNodes[1]] = itsExpectedBlankNodes[2];
          nodeIdMapping2[itsReceivedBlankNodes[2]] = itsExpectedBlankNodes[1];
          mappingPermutations.Add(nodeIdMapping2);

          Hashtable nodeIdMapping3= new Hashtable();
          nodeIdMapping3[itsReceivedBlankNodes[0]] = itsExpectedBlankNodes[1];
          nodeIdMapping3[itsReceivedBlankNodes[1]] = itsExpectedBlankNodes[0];
          nodeIdMapping3[itsReceivedBlankNodes[2]] = itsExpectedBlankNodes[2];
          mappingPermutations.Add(nodeIdMapping3);

          Hashtable nodeIdMapping4= new Hashtable();
          nodeIdMapping4[itsReceivedBlankNodes[0]] = itsExpectedBlankNodes[2];
          nodeIdMapping4[itsReceivedBlankNodes[1]] = itsExpectedBlankNodes[0];
          nodeIdMapping4[itsReceivedBlankNodes[2]] = itsExpectedBlankNodes[1];
          mappingPermutations.Add(nodeIdMapping4);

          Hashtable nodeIdMapping5= new Hashtable();
          nodeIdMapping5[itsReceivedBlankNodes[0]] = itsExpectedBlankNodes[2];
          nodeIdMapping5[itsReceivedBlankNodes[1]] = itsExpectedBlankNodes[1];
          nodeIdMapping5[itsReceivedBlankNodes[2]] = itsExpectedBlankNodes[0];
          mappingPermutations.Add(nodeIdMapping5);

          Hashtable nodeIdMapping6= new Hashtable();
          nodeIdMapping6[itsReceivedBlankNodes[0]] = itsExpectedBlankNodes[1];
          nodeIdMapping6[itsReceivedBlankNodes[1]] = itsExpectedBlankNodes[2];
          nodeIdMapping6[itsReceivedBlankNodes[2]] = itsExpectedBlankNodes[0];
          mappingPermutations.Add(nodeIdMapping6);
        }
        else {
          throw new NotImplementedException("Cannot compare ntriples with more than three different blank nodes");
/*
  Permutations of 4:
    1   2   3   4
    1   2   4   3
    1   3   2   4
    1   3   4   2
    1   4   3   2
    1   4   2   3 
    2   1   3   4
    2   1   4   3
    2   3   1   4
    2   3   4   1
    2   4   3   1
    2   4   1   3 
    3   2   1   4
    3   2   4   1
    3   1   2   4
    3   1   4   2
    3   4   1   2
    3   4   2   1
    4   2   3   1
    4   2   1   3 
    4   3   2   1
    4   3   1   2 
    4   1   3   2
    4   1   2   3 
*/ 
          
        }
        
        foreach (object mappingPermutation in mappingPermutations) {
          verified = VerifyAgainstExpected( ApplyNodeIdMapping( (Hashtable) mappingPermutation) );
          if (verified) {
            break;
          }
        }
        
        
      }
      else {
        verified = VerifyAgainstExpected(itsReceivedTriples);
      }


      return verified;
    }
  
    public bool VerifyAgainstExpected(StringCollection triplesToVerify) {
      foreach (string expected in itsExpectedTriples) {
        if (! triplesToVerify.Contains(expected)) {
          itsLastFailureDescription = "Did not receive expected triple: " + expected;
          return false;
        }
      } 
      return true;
    }
  
    public StringCollection ApplyNodeIdMapping(Hashtable nodeIdMapping) {
      StringCollection mappedTriples = new StringCollection();

      foreach (string received in itsReceivedTriples) {
        
        Match matchTriple = tripleRegex.Match(received);
        if (matchTriple.Success) {
          string subjectPart = matchTriple.Groups[1].Value;
          string predicatePart = matchTriple.Groups[2].Value;
          string objectPart = matchTriple.Groups[3].Value;
          
          Match matchSubjectBlankNode = blankNode.Match(subjectPart);
          Match matchObjectBlankNode = blankNode.Match(objectPart);
          
          if (matchSubjectBlankNode.Success) {
            string subjectNodeId = matchSubjectBlankNode.Groups[1].Value;
           
            if (matchObjectBlankNode.Success) {
              string objectNodeId = matchObjectBlankNode.Groups[1].Value;
              mappedTriples.Add("_:" + nodeIdMapping[subjectNodeId] + " " + predicatePart + " _:" + nodeIdMapping[objectNodeId] + " .");
            }
            else {
              mappedTriples.Add("_:" + nodeIdMapping[subjectNodeId] + " " + predicatePart + " " + objectPart+ " .");
            }
          }
          else {
            if (matchObjectBlankNode.Success) {
              string objectNodeId = matchObjectBlankNode.Groups[1].Value;
              mappedTriples.Add(subjectPart + " " + predicatePart + " _:" + nodeIdMapping[objectNodeId] + " .");
            }
            else {
              mappedTriples.Add(received);
            }
          }
        }
        else {
          mappedTriples.Add(received);
        }
      }
      
      return mappedTriples;
    }
    
    public string GetLastFailureDescription() {
      return itsLastFailureDescription;
    }


  }
}
