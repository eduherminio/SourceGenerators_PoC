using static Library.Utils;
using Library.Generator;

namespace Library;

public class TestClass
{
    public static void Run()
    {
        var readonlySpan = EmptyClass.GeneratedProperty;

        const int localConst = ClassWithStaticInitializers1.ConstField;
        const int anotherLocalConst = ClassWithStaticInitializers1.ConstField2;
        const int yetAnotherLocalConst = ClassWithStaticInitializers1.ConstField3;

        const int total = ClassWithStaticInitializers2.IsolatedPawnPenalty
            + ClassWithStaticInitializers2.OpenFileRookBonus
            + ClassWithStaticInitializers2.SemiOpenFileRookBonus;
    }
}

[GeneratedNamespace.GeneratePSQT]
public partial class EmptyClass
{
}

/// <summary>
/// Approach 1, take the information from the existing initializers.
/// Can be specially useful when we can't modify the existing source code.
/// </summary>
public static partial class ClassWithStaticInitializers1
{
    [GeneratedNamespace.GeneratePackedConstant]
    public static int _ConstField = Pack(1, 2);

    [GeneratedNamespace.GeneratePackedConstant]
    public static int _ConstField2 = Utils.Pack(-3, -4);

    [GeneratedNamespace.GeneratePackedConstant]
    public static int _ConstField3 = 10;
}

/// <summary>
/// Approach 2, take the information from the attributes.
/// Less error prone, but requires
/// </summary>
public static partial class ClassWithStaticInitializers2
{
    #region What we had before

    private static readonly int Old_IsolatedPawnPenalty = Pack(1, 2);
    private static readonly int Old_OpenFileRookBonus = Pack(-3, -4);
    private static readonly int Old_SemiOpenFileRookBonus = int.MaxValue;

    #endregion

    [GeneratedPack(1, 2)]
    private static readonly int _IsolatedPawnPenalty;

    [GeneratedPack(-3, -4)]
    private static readonly int _OpenFileRookBonus;

    public const int SemiOpenFileRookBonus = int.MaxValue;
}