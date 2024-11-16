namespace Library;

/// <summary>
/// Operations we want to move to compile time
/// Orignal source: https://minuskelvin.net/chesswiki/content/packed-eval.html
/// </summary>
internal static class Utils
{
    public static int Pack(short mg, short eg) =>
        (eg << 16) + mg;

    public static short UnpackMG(int packed) =>
        (short)packed;

    public static short UnpackEG(int packed) =>
        (short)((packed + 0x8000) >> 16);
}
