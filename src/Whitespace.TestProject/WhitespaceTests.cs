using DotNot.TestUtilities;
using DotNot.Whitespace;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Text;
using TestUtilities;
using Xunit;

namespace Whitespace.TestProject
{
    public class WhitespaceTests
    {
        [Fact]
        public void TestHelloWorld()
        {
            // Do not touch anything in this string
            var helloWorld = @"   	      	 
   			 		   		  	 
    
		    	  	   
	
     		  	 	
	
     		 		  
 
 	
  	
     		 				
	
     	     
	
     	 	 			
	
     		 				
	
     			  	 
	
     		 		  
	
     		  	  
	
     	    	
	
     	 	 
	
   



";
            var generator = new WhitespaceSourceGenerator();
            var generatorDriver = SourceGeneratorUtilities.CreateDriver(new[] { generator }, new[] { new AdditionalTextImpl("test.ws", helloWorld) });

            var compilation = CSharpCompilation.Create("whitespacetestassembly.dll");
            generatorDriver.RunFullGeneration(compilation, out var outputCompilation, out var diagnostics);
        }
    }
}
