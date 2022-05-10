using System;
using System.Collections.Generic;

namespace Kiota.Builder.Refiners;
public class CSharpReservedTypesProvider : IReservedNamesProvider
{
    private readonly Lazy<HashSet<string>> _reservedNames = new(static () => new(StringComparer.OrdinalIgnoreCase)
    {
        "environment",
        "file",
        "task",
        "thread",
    });
    public HashSet<string> ReservedNames => _reservedNames.Value;
}
