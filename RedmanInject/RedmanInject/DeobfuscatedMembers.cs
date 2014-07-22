using System;
using System.Collections.Generic;
using System.Reflection;

public class DeobfuscatedMembers
{
    public FieldInfo fieldNetworkUserList;
    public List<NetworkUser> getNetworkUserList()
    {
        return (List<NetworkUser>)fieldNetworkUserList.GetValue(null);
    }
    public void setNetworkUserList(List<NetworkUser> networkUsers)
    {
        fieldNetworkUserList.SetValue(null, networkUsers);
    }
}