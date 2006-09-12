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
  using SemPlan.Spiral.Core;
  using SemPlan.Spiral.Utility;
  using SemPlan.Spiral.Tests.Core;
  using System;
  
	/// <summary>
	/// Programmer tests for SimpleRuleProcessor class
	/// </summary>
  /// <remarks>
  /// $Id: SimpleRuleProcessorTest.cs,v 1.2 2005/05/26 14:24:31 ian Exp $
  ///</remarks>
	[TestFixture]
  public class SimpleRuleProcessorTest {
    [Test]
    public void emptyRuleMakesNoChangesToSourceOrDestinationTripleStores() {
      TripleStore store = new MemoryTripleStore();
      
      SimpleRuleProcessor ruleProcessor = new SimpleRuleProcessor();
      
      Rule rule = new Rule();

      ruleProcessor.Process( rule, store );
      
      Assert.IsTrue( store.IsEmpty() );
    }


    [Test]
    public void noAntecedentsAutomaticallyAddsConsequents() {
      TripleStore store = new MemoryTripleStore();
      
      SimpleRuleProcessor ruleProcessor = new SimpleRuleProcessor();
      
      Rule rule = new Rule();
      rule.AddConsequent( new Pattern( new UriRef("http://example.com/subj"), new UriRef("http://example.com/pred"), new UriRef("http://example.com/obj") ) );

      ruleProcessor.Process( rule, store );
      
      Assert.IsFalse( store.IsEmpty() , "Store is non-empty");
      
      Assert.IsTrue( store.Contains( new Statement( new UriRef("http://example.com/subj"), new UriRef("http://example.com/pred"), new UriRef("http://example.com/obj") ) ), "Destination contains consequent");
      
    }

    [Test]
    public void nonMatchingAntecedentsAddsNoConsequents() {
      TripleStore store = new MemoryTripleStore();
      
      SimpleRuleProcessor ruleProcessor = new SimpleRuleProcessor();
      
      Rule rule = new Rule();
      rule.AddAntecedent( new Pattern( new UriRef("http://example.com/subj"), new UriRef("http://example.com/pred"), new UriRef("http://example.com/obj") ) );
      rule.AddConsequent( new Pattern( new UriRef("http://example.com/subj"), new UriRef("http://example.com/pred"), new UriRef("http://example.com/other") ) );

      ruleProcessor.Process( rule, store );
      Assert.IsTrue( store.IsEmpty() , "Store is still empty");
    }


    [Test]
    public void matchingAntecedentsAddsConsequents() {
      TripleStore store = new MemoryTripleStore();
      
      SimpleRuleProcessor ruleProcessor = new SimpleRuleProcessor();
      
      store.Add( new Statement( new UriRef("http://example.com/subj"), new UriRef("http://example.com/pred"), new UriRef("http://example.com/obj"))  );
      
      Rule rule = new Rule();
      rule.AddAntecedent( new Pattern( new UriRef("http://example.com/subj"), new UriRef("http://example.com/pred"), new UriRef("http://example.com/obj") ) );
      rule.AddConsequent( new Pattern( new UriRef("http://example.com/subj"), new UriRef("http://example.com/pred"), new UriRef("http://example.com/other") ) );

      ruleProcessor.Process( rule, store );
      
      Assert.IsFalse( store.IsEmpty() , "Store is non-empty");
      
      Assert.IsTrue( store.Contains( new Statement( new UriRef("http://example.com/subj"), new UriRef("http://example.com/pred"), new UriRef("http://example.com/other") ) ), "Destination contains consequent");
      
    }


    [Test]
    public void consequentsAreAddedUsingAntecedentSolutionBindings() {
      TripleStore store = new MemoryTripleStore();
      
      SimpleRuleProcessor ruleProcessor = new SimpleRuleProcessor();
      
      store.Add( new Statement(new UriRef("http://example.com/subj"), new UriRef("http://example.com/pred"), new UriRef("http://example.com/obj")) );
      
      Rule rule = new Rule();
      rule.AddAntecedent( new Pattern( new Variable("var1"), new UriRef("http://example.com/pred"), new Variable("var2")) );
      rule.AddConsequent( new Pattern( new Variable("var1"), new UriRef("http://example.com/newPred"), new Variable("var2") ) );

      ruleProcessor.Process( rule, store );
      
      Assert.IsFalse( store.IsEmpty() , "Destination is non-empty");

      Assert.IsTrue( store.Contains( new Statement( new UriRef("http://example.com/subj"), new UriRef("http://example.com/newPred"), new UriRef("http://example.com/obj") ) ), "Destination contains consequent");
      
    }
    
    [Test]
    public void allAntecedentMatchesAreProcessed() {
      TripleStore store = new MemoryTripleStore();
      
      SimpleRuleProcessor ruleProcessor = new SimpleRuleProcessor();
      
      store.Add( new Statement(new UriRef("http://example.com/subj1"), new UriRef("http://example.com/pred"), new UriRef("http://example.com/obj1")) );
      store.Add( new Statement(new UriRef("http://example.com/subj2"), new UriRef("http://example.com/pred"), new UriRef("http://example.com/obj2")) );
      
      Rule rule = new Rule();
      rule.AddAntecedent( new Pattern( new Variable("var1"), new UriRef("http://example.com/pred"), new Variable("var2")) );
      rule.AddConsequent( new Pattern( new Variable("var1"), new UriRef("http://example.com/newPred"), new Variable("var2") ) );

      ruleProcessor.Process( rule, store );
      
      Assert.IsFalse( store.IsEmpty() , "Destination is non-empty");

      Assert.IsTrue( store.Contains( new Statement( new UriRef("http://example.com/subj1"), new UriRef("http://example.com/newPred"), new UriRef("http://example.com/obj1") ) ), "Destination contains first match onsequent");
      Assert.IsTrue( store.Contains( new Statement( new UriRef("http://example.com/subj2"), new UriRef("http://example.com/newPred"), new UriRef("http://example.com/obj2") ) ), "Destination contains second match consequent");
      
    }    

  }
  
  
}