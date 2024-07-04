using System;
using System.Text;

namespace Devlooped;

static class Extensions
{
    public static StringBuilder AppendLineIndented(this StringBuilder builder, string value, string indent)
    {
        foreach (var line in value.ReplaceLineEndings().Split(Environment.NewLine))
            builder.Append(indent).AppendLine(line);

        return builder;
    }
}
