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
	/// <summary>
	/// A naieve implementation of a rule processor
	/// </summary>
  /// <remarks>
  /// $Id: SimpleRuleProcessor.cs,v 1.4 2006/02/07 01:11:14 ian Exp $
  ///</remarks>
  public class SimpleRuleProcessor {

    public bool Explain = false;

    public void Process(Rule rule, TripleStore store) {
      Query query = new Query();
      
      if (rule.Antecedents.Count == 0) {
        ApplyConsequentStatements(rule, store);
        return;
      }
      
      
      foreach( Pattern pattern in rule.Antecedents) {
        query.AddPattern( pattern );
      }

      foreach (Pattern pattern in rule.Consequents) {         
        if (pattern.GetSubject() is Variable) {
          query.AddVariable( ((Variable)pattern.GetSubject() ) );
        }            
        if (pattern.GetPredicate() is Variable) {
          query.AddVariable( ((Variable)pattern.GetPredicate() ) );
        }
        if (pattern.GetObject() is Variable) {
          query.AddVariable( ((Variable)pattern.GetObject() ) );
        }            
      }
      
      IEnumerator solutions = store.Solve( query );

      if (Explain) ((BacktrackingQuerySolver)solutions).Explain = true;

      ArrayList consequentStatements = new ArrayList();

      while ( solutions.MoveNext() ) {
        QuerySolution solution = (QuerySolution)solutions.Current;

        foreach (Pattern pattern in rule.Consequents) {
            Resource subjectResource;
            Resource predicateResource;
            Resource objectResource;
            
            if (pattern.GetSubject() is Variable) {
              subjectResource = solution[ ((Variable)pattern.GetSubject() ).Name ];
            }
            else {
              subjectResource = store.GetResourceDenotedBy((GraphMember)pattern.GetSubject());
            }
            
            if (pattern.GetPredicate() is Variable) {
              predicateResource = solution[ ((Variable)pattern.GetPredicate() ).Name ];
            }
            else {
              predicateResource = store.GetResourceDenotedBy((GraphMember)pattern.GetPredicate());
            }

            if (pattern.GetObject() is Variable) {
              objectResource = solution[ ((Variable)pattern.GetObject() ).Name ];
            }
            else {
              objectResource = store.GetResourceDenotedBy((GraphMember)pattern.GetObject());
            }
  
            consequentStatements.Add( new ResourceStatement(subjectResource, predicateResource, objectResource)  );

        }
      }
      
      foreach (ResourceStatement statement in consequentStatements) {
        store.Add( statement );
      }
      
    }
    
    public void ApplyConsequentStatements(Rule rule, TripleStore store) {
      ArrayList consequentStatements = new ArrayList();

      foreach (Pattern pattern in rule.Consequents) {
        Resource subjectResource;
        Resource predicateResource;
        Resource objectResource;
        
        if (pattern.GetSubject() is Variable) {
          continue;
        }
        else {
          subjectResource = store.GetResourceDenotedBy((GraphMember)pattern.GetSubject());
        }
        
        if (pattern.GetPredicate() is Variable) {
          continue;
        }
        else {
          predicateResource = store.GetResourceDenotedBy((GraphMember)pattern.GetPredicate());
        }

        if (pattern.GetObject() is Variable) {
          continue;
        }
        else {
          objectResource = store.GetResourceDenotedBy((GraphMember)pattern.GetObject());
        }

        consequentStatements.Add( new ResourceStatement(subjectResource, predicateResource, objectResource)  );
    }
    
      foreach (ResourceStatement statement in consequentStatements) {
        store.Add( statement );
      }
        
    }
  }
 
}