using System.Runtime.CompilerServices;

namespace Library.Generator.Test;

public static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Init() =>
        VerifySourceGenerators.Initialize();
}