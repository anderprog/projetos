using System;
using System.Data;
using System.Linq;
using System.Web;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Configuration;
using System.Text;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// Contains my site's global variables.
/// </summary>
public class ClasseUsuario
{

    private static ClasseUsuario instance;
    private static object sync = new Object();

    private ClasseUsuario() { }

    public static ClasseUsuario Instance
    {
        get
        {
            if (instance == null)
            {
                lock (sync)
                {
                    if (instance == null)
                    {
                        instance = new ClasseUsuario();
                    }
                }
            }
            return instance;
        }

    }
    public string Criptografa(string pstring)
    {
        System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
        byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(pstring);
        byte[] hash = md5.ComputeHash(inputBytes);
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        for (int i = 0; i < hash.Length; i++)
        {
            sb.Append(hash[i].ToString("X2"));
        }
        return sb.ToString();

    }
    public string GetADUserGroups(string userName, string psenha)
    {
        string conexaoAD = ConfigurationManager.AppSettings["StringLDAP"];
        DirectoryEntry directoryEntry = new DirectoryEntry(conexaoAD, userName,psenha);
        DirectorySearcher search = new DirectorySearcher(directoryEntry);

        PrincipalContext domain = new PrincipalContext(ContextType.Domain, "REDECORP");
        UserPrincipal user = UserPrincipal.FindByIdentity(domain, IdentityType.SamAccountName, userName); // NGeodakov

        //search.Filter = String.Format("(cn={0})", userName);
        search.Filter = "(&(objectClass=group)(member=" + user.DistinguishedName + "))";
        search.PropertiesToLoad.Add("samaccountname");  //adicionado
        search.PropertiesToLoad.Add("memberOf");
        StringBuilder groupsList = new StringBuilder();

        SearchResult result = search.FindOne();
        if (result != null)
        {
            int groupCount = result.Properties["memberOf"].Count;

            for (int counter = 0; counter < groupCount; counter++)
            {
                groupsList.Append((string)result.Properties["memberOf"][counter]);
                groupsList.Append("@");
            }
        }
        groupsList.Length -= 1;

        return groupsList.ToString();
    }
    public static string ConvertToOctetString(byte[] values, bool
isAddBackslash, bool isUpperCase)
    {
        string slash;
        if (isAddBackslash)
        {
            slash = "\\";
        }
        else
        {
            slash = string.Empty;
        }
        string formatCode;
        if (isUpperCase)
        {
            formatCode = "X2";
        }
        else
        {
            formatCode = "x2";
        }
        StringBuilder builder = new StringBuilder(values.Length * 2);
        for (int iterator = 0; iterator <= values.Length - 1; iterator++)
        {
            builder.Append(slash);
            builder.Append(values[iterator].ToString(formatCode));
        }
        return builder.ToString();
    }
    public string ConvertToOctetString(byte[] values)
    {
        return ConvertToOctetString(values, false, false);
    }
    public string[] GetGroups(DirectoryEntry userEntry, string ldapPath,string senha)
    {
        ArrayList allGroupNames = new ArrayList();
        userEntry.RefreshCache(new string[] { "tokenGroups" });
        PropertyValueCollection groupSids =
userEntry.Properties["tokenGroups"];
        ArrayList binarySids = new ArrayList(groupSids.Count);
        binarySids.AddRange(groupSids);
        for (int i = 0; i <= binarySids.Count - 1; i++)
        {
            byte[] binSid = ((byte[])(binarySids[i]));
            string octetSid = ConvertToOctetString(binSid);
            string groupPath = string.Format(ldapPath + "/<SID={0}>",
octetSid);
            DirectoryEntry groupEntry = new DirectoryEntry(groupPath,
userEntry.Username,senha);
            string groupName =
groupEntry.Properties["samAccountName"].Value.ToString();
            allGroupNames[i] = groupName;
        }
        return (string[])allGroupNames.ToArray(typeof(string));
    }


    public List<String> GetUserGroupsOld(string puserName, string psenha)
    {
        List<String> groups = new List<String>();
        string conexaoAD = ConfigurationManager.AppSettings["StringLDAP"];

        String userName = puserName;
        int pos = userName.IndexOf(@"\");
        if (pos > 0) userName = userName.Substring(pos + 1);

        PrincipalContext domain = new PrincipalContext(ContextType.Domain, null);
        UserPrincipal user = UserPrincipal.FindByIdentity(domain, IdentityType.SamAccountName, userName); // NGeodakov

        DirectoryEntry de = new DirectoryEntry(conexaoAD);
        DirectorySearcher search = new DirectorySearcher(de);
        search.Filter = "(&(objectClass=group)(member=" + user.DistinguishedName + "))";
        search.PropertiesToLoad.Add("cn");
        search.PropertiesToLoad.Add("samaccountname");
        search.PropertiesToLoad.Add("memberOf");

        SearchResultCollection results = search.FindAll();
        foreach (SearchResult sr in results)
        {
            GetUserGroupsRecursive(groups, sr, de);
        }

        return groups;
    }

    public void GetUserGroupsRecursive(List<String> groups, SearchResult sr, DirectoryEntry de)
    {
        if (sr == null) return;

        String group = (String)sr.Properties["cn"][0];
        if (String.IsNullOrEmpty(group))
        {
            group = (String)sr.Properties["samaccountname"][0];
        }
        if (!groups.Contains(group))
        {
            groups.Add(group);
        }

        DirectorySearcher search;
        SearchResult sr1;
        String name;
        int equalsIndex, commaIndex;
        foreach (String dn in sr.Properties["memberof"])
        {
            equalsIndex = dn.IndexOf("=", 1);
            if (equalsIndex > 0)
            {
                commaIndex = dn.IndexOf(",", equalsIndex + 1);
                name = dn.Substring(equalsIndex + 1, commaIndex - equalsIndex - 1);

                search = new DirectorySearcher(de);
                search.Filter = "(&(objectClass=group)(|(cn=" + name + ")(samaccountname=" + name + ")))";
                search.PropertiesToLoad.Add("cn");
                search.PropertiesToLoad.Add("samaccountname");
                search.PropertiesToLoad.Add("memberOf");
                sr1 = search.FindOne();
                GetUserGroupsRecursive(groups, sr1, de);
            }
        }
    }

    public string GetDepartment(string username)
    {
        string result = string.Empty;

        // if you do repeated domain access, you might want to do this *once* outside this method, 
        // and pass it in as a second parameter!
        PrincipalContext domain = new PrincipalContext(ContextType.Domain, null);
        PrincipalContext yourDomain = new PrincipalContext(ContextType.Domain);

        // find the user
        UserPrincipal user = UserPrincipal.FindByIdentity(yourDomain, username);

        // if user is found
        if (user != null)
        {
            // get DirectoryEntry underlying it
            DirectoryEntry de = (user.GetUnderlyingObject() as DirectoryEntry);

            if (de != null)
            {
                if (de.Properties.Contains("department"))
                {
                    result = de.Properties["department"][0].ToString();
                }
            }
        }

        return result;
    }
    public  List<string> GetNestedGroupMembershipsByTokenGroup(string userDN, string psenha)
    {
        List<string> nestedGroups = new List<string>();
        string conexaoAD = ConfigurationManager.AppSettings["StringLDAP"];

        DirectoryEntry userEnrty = new DirectoryEntry(conexaoAD, userDN,psenha);
        // Use RefreshCach para obter os tokenGroups do atributo construído.
        userEnrty.RefreshCache(new string[] { "tokenGroups" });

        foreach (byte[] sid in userEnrty.Properties["tokenGroups"])
    {
            string groupSID = new System.Security.Principal.SecurityIdentifier(sid, 0).ToString();
            DirectoryEntry grpuEnrty = new DirectoryEntry(conexaoAD ,userDN,psenha);
            nestedGroups.Add(grpuEnrty.Properties["samAccountName"][0].ToString());
        }

        return nestedGroups;
    }
    /// <summary>
    /// Propriedade para o ID do usuario
    /// </summary>
    public string UserText { get; set; }

}