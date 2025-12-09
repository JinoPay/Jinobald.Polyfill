#if NET35

namespace System.Reflection;

public static class RuntimeReflectionExtensions
{
    public static EventInfo GetRuntimeEvent(this Type? type, string name)
    {
        if (type is null) throw new ArgumentNullException(nameof(type));

        return type.GetEvent(name);
    }

    public static FieldInfo GetRuntimeField(this Type? type, string name)
    {
        if (type is null) throw new ArgumentNullException(nameof(type));

        return type.GetField(name);
    }

    public static IEnumerable<EventInfo> GetRuntimeEvents(this Type? type)
    {
        if (type is null) throw new ArgumentNullException(nameof(type));

        return type.GetEvents();
    }

    public static IEnumerable<FieldInfo> GetRuntimeFields(this Type? type)
    {
        if (type is null) throw new ArgumentNullException(nameof(type));

        return type.GetFields();
    }

    public static IEnumerable<MethodInfo> GetRuntimeMethods(this Type? type)
    {
        if (type is null) throw new ArgumentNullException(nameof(type));

        return type.GetMethods();
    }

    public static IEnumerable<PropertyInfo> GetRuntimeProperties(this Type? type)
    {
        if (type is null) throw new ArgumentNullException(nameof(type));

        return type.GetProperties();
    }

    public static InterfaceMapping GetRuntimeInterfaceMap(this Type? typeInfo, Type interfaceType)
    {
        if (typeInfo is null) throw new ArgumentNullException(nameof(typeInfo));

        return typeInfo.GetInterfaceMap(interfaceType);
    }

    public static MethodInfo GetMethodInfo(this Delegate? del)
    {
        if (del is null) throw new ArgumentNullException(nameof(del));

        return del.Method;
    }

    public static MethodInfo GetRuntimeBaseDefinition(this MethodInfo? method)
    {
        if (method is null) throw new ArgumentNullException(nameof(method));

        return method.GetBaseDefinition();
    }

    public static MethodInfo GetRuntimeMethod(this Type? type, string name, Type[] parameters)
    {
        if (type is null) throw new ArgumentNullException(nameof(type));

        return type.GetMethod(name, parameters);
    }

    public static PropertyInfo GetRuntimeProperty(this Type? type, string name)
    {
        if (type is null) throw new ArgumentNullException(nameof(type));

        return type.GetProperty(name);
    }
}

#endif