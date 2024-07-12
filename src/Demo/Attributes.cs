using System;
using System.Runtime.InteropServices;

namespace Demo;

public class PlatformFactAttribute : FactAttribute
{
    public PlatformFactAttribute(PlatformID platform)
    {
        if (Environment.OSVersion.Platform != platform)
            Skip = $"Test only runs on {platform}";
    }
}
