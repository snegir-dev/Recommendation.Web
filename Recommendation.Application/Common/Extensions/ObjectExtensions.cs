using System.Reflection;

namespace Recommendation.Application.Common.Extensions;

public static class ObjectExtensions
{
    public static object? GetValueProperty(this object obj, string name)
    {
        var type = obj.GetType();
        return type.GetProperty(name)?.GetValue(obj);
    }
}