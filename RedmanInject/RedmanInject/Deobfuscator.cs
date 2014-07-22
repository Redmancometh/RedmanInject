using System;
using System.Collections.Generic;
using System.Reflection;

public class Deobfuscator
{
    public static DeobfuscatedMembers deobfuscate(Assembly assembly)
    {
        DeobfuscatedMembers dm = new DeobfuscatedMembers();
        deobFieldNetworkUserList(dm);
        return dm;
    }

    private static void deobFieldNetworkUserList(DeobfuscatedMembers dm)
    {
        /*try
        {*/
            //dm.fieldNetworkUserList = getFieldsOfType(typeof(NetworkUserList), typeof(List<NetworkUser>))[0];
        Hacks.print("X");
        Type a = typeof(NetworkUserList);
        Hacks.print("Y");
        Type b = typeof(List<NetworkUser>);
        Hacks.print("Z");
        getFieldsOfType(a, b);
        Hacks.print("Z'");
        //Hacks.print("Y");
        /*}
        catch { }*/
    }

    private static FieldInfo[] getFieldsOfType(Type from, Type fieldType)
    {
        List<FieldInfo> fields = new List<FieldInfo>();
        FieldInfo[] fromFields = from.GetFields();
        for (int i = 0; i < fromFields.Length; i++ )
        {
            FieldInfo fi = fromFields[i];
            try
            {
                if (fi.FieldType == fieldType)
                {
                    fields.Add(fi);
                }
            }
            catch
            { }
        }
        return fields.ToArray();
    }

    private static MethodInfo[] getMethodsOfSignature(Type from, Type returnType, Type[] parameterTypes)
    {
        List<MethodInfo> methods = new List<MethodInfo>();
        foreach (MethodInfo mi in from.GetMethods())
        {
            if (mi.ReturnType == returnType)
            {
                ParameterInfo[] pi = mi.GetParameters();
                if (pi.Length != parameterTypes.Length)
                {
                    continue;
                }
                for (int i = 0; i < parameterTypes.Length; i++)
                {
                    if (pi[i].ParameterType != parameterTypes[i])
                    {
                        continue;
                    }
                }
                methods.Add(mi);
            }
        }
        return methods.ToArray();
    }
}
