using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using CodingStandard;
using PotentialFault ;
using Microsoft.Build.Locator;

class Program 
{
    static void Main(string[] args)
    {
        MSBuildLocator.RegisterDefaults();

        PascalRule pascalRule = new PascalRule();
        //PotentialDefect potentialDefect = new PotentialDefect();

        //string folderPath = "D:\\Calculator_Roslyn\\Calculator\\ViewModel";
        //string solutionPath = @"D:\Calculator_Roslyn\Calculator\Calculator.sln";

        //var csFiles = Directory.GetFiles(folderPath, "*.cs", SearchOption.AllDirectories);

        pascalRule.PascalCase(@"CodeAnalyzer\CodeAnalyzer.csproj");

        //foreach(var csFile in csFiles)
        //{
        //    pascalRule.PascalCase(csFile);
        //}


        //potentialDefect.UnusedVariable();

        MSBuildLocator.Unregister();

    }

   
}