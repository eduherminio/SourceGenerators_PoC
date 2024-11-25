﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace Library.Generator;

[Generator(LanguageNames.CSharp)]
public class PropertyGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(static postInitializationContext =>
        {
            postInitializationContext.AddSource("GeneratePSQTAttribute.g.cs", SourceText.From("""
                using System;

                namespace GeneratedNamespace
                {
                    internal sealed class GeneratePSQTAttribute : Attribute
                    {
                    }
                }
                """, Encoding.UTF8));
        });

        var pipeline = context.SyntaxProvider.ForAttributeWithMetadataName(
            fullyQualifiedMetadataName: "GeneratedNamespace.GeneratePSQTAttribute",
            predicate: static (syntaxNode, _) => syntaxNode is ClassDeclarationSyntax,
            transform: static (context, _) =>
            {
                return new Model(
                    // Note: this is a simplified example. You will also need to handle the case where the type is in a global namespace, nested, etc.
                    Namespace: context.TargetSymbol.ContainingNamespace?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat.WithGlobalNamespaceStyle(SymbolDisplayGlobalNamespaceStyle.Omitted)) ?? string.Empty,
                    ClassName: context.TargetSymbol.Name,
                    PropertyName: "GeneratedProperty");
            }
        );

        context.RegisterSourceOutput(pipeline, static (context, model) =>
        {
            var sourceText = SourceText.From($$"""
                namespace {{model.Namespace}};
                static partial class {{model.ClassName}}
                {
                    public static ReadOnlySpan<int> {{model.PropertyName}} => [];
                }
                """, Encoding.UTF8);

            context.AddSource($"{model.ClassName}_{model.PropertyName}.g.cs", sourceText);
        });
    }

    private sealed record Model(string Namespace, string ClassName, string PropertyName);
}
