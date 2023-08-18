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
                string commitMessage = "Add naming rule warning messages";
                GitCommit(commitMessage, warningFilePath);
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

        static void GitCommit(string commitMessage, string filePath)
        {
            string addCommand = $"git add {filePath}";
            string commitCommand = $"git commit -m \"{commitMessage}\"";
            string pushCommand = "git push origin main";

            RunShellCommand(addCommand);
            RunShellCommand(commitCommand);
            RunShellCommand(pushCommand);
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
