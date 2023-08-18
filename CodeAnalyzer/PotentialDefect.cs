using System;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;

namespace PotentialFault
{
    public class PotentialDefect
    {
        public void UnusedVariable()
        {
            // 솔루션 경로 설정
            string solutionPath = @"D:\RoslynTest\Calc\Calc.sln";

            // 결과를 저장할 텍스트 파일 경로
            string resultFilePath = @"D:\RoslynTest\UnusedVariables.txt";

            MSBuildWorkspace workspace = MSBuildWorkspace.Create();
            Solution solution = workspace.OpenSolutionAsync(solutionPath).Result;

            using (var writer = new StreamWriter(resultFilePath))
            {
                foreach (var project in solution.Projects)
                {
                    foreach (var document in project.Documents)
                    {
                        var syntaxTree = document.GetSyntaxTreeAsync().Result;
                        var root = syntaxTree.GetRoot();

                        // 변수 선언 찾기
                        var variableDeclarations = root.DescendantNodes().OfType<VariableDeclarationSyntax>();

                        foreach (var declaration in variableDeclarations)
                        {
                            foreach (var variable in declaration.Variables)
                            {
                                // 변수 선언이 사용되지 않은 경우, 결과 파일에 기록
                                if (!IsVariableUsed(variable, root))
                                {
                                    string variableName = variable.Identifier.Text;
                                    string projectName = project.Name;
                                    string fileName = document.Name;
                                    int line = syntaxTree.GetLineSpan(variable.Span).StartLinePosition.Line + 1;

                                    writer.WriteLine("Unused Variable");
                                    writer.WriteLine($"Variable name : {variableName}");
                                    writer.WriteLine($"Project name : {projectName}");
                                    writer.WriteLine($"File name : {fileName}");
                                    writer.WriteLine($"Line number : {line}");
                                    writer.WriteLine();
                                }
                            }
                        }
                    }
                }
            }
        }

        // 변수 사용 확인 메서드
        static bool IsVariableUsed(VariableDeclaratorSyntax variable, SyntaxNode root)
        {
            return root.DescendantNodes().OfType<IdentifierNameSyntax>()
                .Any(node => node.Identifier.Text == variable.Identifier.Text);
        }
    }
}
