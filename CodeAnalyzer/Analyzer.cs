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
using System.Runtime.InteropServices;

class Program 
{
    static void Main(string[] args)
    {
        //MSBuildLocator.RegisterDefaults();

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            MSBuildLocator.RegisterDefaults();
        }

        PascalRule pascalRule = new PascalRule();
        //PotentialDefect potentialDefect = new PotentialDefect();

        //string folderPath = "D:\\Calculator_Roslyn\\Calculator\\ViewModel";
        //string solutionPath = @"D:\Calculator_Roslyn\Calculator\Calculator.sln";

        //var csFiles = Directory.GetFiles(folderPath, "*.cs", SearchOption.AllDirectories);

        pascalRule.PascalCase(@"CodeAnalyzer\CodeAnalyzer.csproj");


        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            MSBuildLocator.Unregister();
        }
        //foreach(var csFile in csFiles)
        //{
        //    pascalRule.PascalCase(csFile);
        //}


        //potentialDefect.UnusedVariable();

        //MSBuildLocator.Unregister();

    }

    public class PascalRule
    {
        public void PascalCase(string viewModelFilePath)
        {
            string warningFilePath = "namingRule_warnig_message.txt";
            string viewModelCode = File.ReadAllText(viewModelFilePath);
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(viewModelCode);
            Compilation compilation = CSharpCompilation.Create("ViewModelCompilation",
                syntaxTrees: new[] { syntaxTree },
                references: new[] { MetadataReference.CreateFromFile(typeof(object).Assembly.Location) });

            var root = syntaxTree.GetRoot();
            var methods = root.DescendantNodes().OfType<MethodDeclarationSyntax>();

            List<string> warningMessages = new List<string>();

            foreach (var method in methods)
            {
                string methodName = method.Identifier.ValueText;
                if (!IsPascalCase(methodName))
                {
                    warningMessages.Add($"Method name '{methodName}' in ViewModel is not in PascalCase");
                }
            }

            if (warningMessages.Count > 0)
            {
                File.WriteAllLines(warningFilePath, warningMessages);
                Console.WriteLine($"Warning messages saved to {warningFilePath}");

                // Commit the warning file to the repository
            }
            else
            {
                Console.WriteLine("All method names in ViewModel are in PascalCase");
            }
        }

        static bool IsPascalCase(string s)
        {
            return !string.IsNullOrEmpty(s) && char.IsUpper(s[0]);
        }



        static void RunShellCommand(string command)
        {
            var escapedArgs = command.Replace("\"", "\\\"");
            var process = new System.Diagnostics.Process()
            {
                StartInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
        }
    }
}