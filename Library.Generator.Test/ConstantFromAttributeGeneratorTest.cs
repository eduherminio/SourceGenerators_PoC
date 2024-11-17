using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;

namespace Library.Generator.Test;

public class ConstantFromAttributeGeneratorTest
{
    [Test]
    public Task StaticClass()
    {
        // The source code to test
        const string source = @"
using namespace Library.Generator;

public static partial class TestClass
{
    [GeneratedPack(1, 2)]
    public static readonly int _TestConstant;
}";

        // Pass the source code to our helper and snapshot test the output
        return Verify(source);
    }

    [Test]
    public Task NonStaticClass()
    {
        // The source code to test
        const string source = @"
using namespace Library.Generator;

public partial class TestClass
{
    [GeneratedPack(-1, 2)]
    public static readonly int _TestConstant;
}";

        // Pass the source code to our helper and snapshot test the output
        return Verify(source);
    }

    [Test]
    public Task PrivateStaticReadonlyField()
    {
        // The source code to test
        const string source = @"
using namespace Library.Generator;

public partial class TestClass
{
    [GeneratedPack(-1, -2)]
    private static readonly int _TestConstant;
}";

        // Pass the source code to our helper and snapshot test the output
        return Verify(source);
    }

    [Test]
    public Task NoNamespaceImport_ShouldNotGenerateConstant()
    {
        // The source code to test
        const string source = @"
public partial class TestClass
{
    [GeneratedPack(1, 2)]
    private static readonly int _TestConstant;
}";

        // Pass the source code to our helper and snapshot test the output
        return Verify(source);
    }

    [Test]
    public Task NoAttributes_ShouldNotGenerateConstant()
    {
        // The source code to test
        const string source = @"
using namespace Library.Generator;

public partial class TestClass
{
    private static readonly int _TestConstant;
}";

        // Pass the source code to our helper and snapshot test the output
        return Verify(source);
    }

    /// <summary>
    /// Based on: https://andrewlock.net/creating-a-source-generator-part-2-testing-an-incremental-generator-with-snapshot-testing/
    /// https://github.com/andrewlock/blog-examples/blob/c35edf1c1f0e1f9adf84c215e2ce7ab644b374f5/NetEscapades.EnumGenerators2/tests/NetEscapades.EnumGenerators.Tests/cs
    /// </summary>
    private static Task Verify(string source)
    {
        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(source);

        // Create references for assemblies we require
        // We could add multiple references if required
        IEnumerable<PortableExecutableReference> references =
        [
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location)
        ];

        CSharpCompilation compilation = CSharpCompilation.Create(
            assemblyName: "Tests",
            syntaxTrees: [syntaxTree],
            references: references);

        ConstantFromAttributeGenerator generator = new ConstantFromAttributeGenerator();

        GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

        driver = driver.RunGenerators(compilation);

        return Verifier
            .Verify(driver)
            .UseDirectory("Snapshots");
    }
}