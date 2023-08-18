using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodingStandard
{
    public class NamingRule
    {
        public void PascalCase(string viewModelFilePath)
        {
            string warningFilePath = @"D:\RoslynTest\namingRule_warnig_message.txt";
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
    }
}
