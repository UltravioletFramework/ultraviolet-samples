using System;

namespace Sample13_UPFAdvanced.Shared
{
    [Flags]
    public enum GameFlags
    {
        None = 0x00,
        CompileExpressions = 0x01,
    }
}
