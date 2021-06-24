using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;

    public class ActiveDirectoryExplorer
    {
        private static string domain;
        private static string ScopeDN;
        DirectoryEntry de;
        DirectorySearcher ds;

        //Constructor
        public ActiveDirectoryExplorer(string d, string scope)
        {
            domain = d;
            ScopeDN = scope;
            de = GetDirectoryEntry("LDAP://" + ScopeDN);
            ds = new DirectorySearcher();
            ds.SearchRoot = de;
        }

        private DirectoryEntry GetDirectoryEntry(string ldapURL)
        {
            DirectoryEntry dentry = new DirectoryEntry();
            dentry.Path = ldapURL;
            dentry.AuthenticationType = AuthenticationTypes.Secure;

            return dentry;
        }

        private PropertyCollection getDirectoryEntryProperties(string identifier, string fieldName)
        {
            if ((String.Equals(identifier, "")) || (String.Equals(fieldName, "")))
                return null;

            string dn = this.GetAttribute(identifier, fieldName, "distinguishedname");
            DirectoryEntry objRootDSE = new DirectoryEntry("LDAP://" + dn);
            return objRootDSE.Properties;
        }

        private PropertyCollection getDirectoryEntryProperties(string dn)
        {
            string encoded_dn = dn.Replace("/", @"\/");
            DirectoryEntry objRootDSE = new DirectoryEntry("LDAP://" + encoded_dn);
            return objRootDSE.Properties;
        }

        public List<String> GetADSecurityGroupUsers(String sgAlias, string DNofGroupOU)
        {
            //Get all the AD directory entries contained in this security group.
            PropertyCollection properties = this.getDirectoryEntryProperties(sgAlias, "name");

            List<string> groupMembers = new List<string>();

            if (properties != null)
                //Look up all the members of this directory entry and look for person data
                //to add to the collection.
                groupMembers = GetUsersInGroup(properties, groupMembers); //GetUsersInGroup do not return anything. 

            return groupMembers;
        }

        private List<string> GetUsersInGroup(PropertyCollection properties, List<String> groupMembers)
        {
            //Properties index identifiers
            string memberIdx = "member";
            string sAMAccountNameIdx = "sAMAccountName";
            string sAMAccountTypeIdx = "sAMAccountType";

            //Property value identifiers
            int normalUserObject = 805306368; //User object type is 0x30000000 = 805306368 decimal
            int groupObject = 268435456;      //Group object type is 0x10000000 = 268435456 decimal

            if (properties[memberIdx] != null)
            {
                foreach (object property in properties[memberIdx])
                {
                    string distinguishedName = property.ToString();

                    //Get the directory entry for this member record.  Filters for only the sAMAccountTypes
                    //security group and user.
                    PropertyCollection childProperties = this.getDirectoryEntryProperties(distinguishedName);

                    if (childProperties != null)
                    {
                        //If the member's sAMAccountType is User.
                        if ((int)childProperties[sAMAccountTypeIdx].Value == normalUserObject)
                        {
                            //Add the user's sAMAccountName to the list
                            groupMembers.Add(childProperties[sAMAccountNameIdx].Value.ToString());
                        }
                        else
                        {
                            //If the member's sAMAccountType is Group
                            if ((int)childProperties[sAMAccountTypeIdx].Value == groupObject)
                            {
                                //RECURSE - Look up all the members of the security group just found.
                                return GetUsersInGroup(childProperties, groupMembers);
                            }
                        }
                        //Otherwise, just ignore this entry and move on.
                    }
                }
            }
            return groupMembers;
        }

    public bool DoesUserExist(string userName)
    {
        using (var domainContext = new PrincipalContext(ContextType.Domain, "DOMAIN"))
        {
            using (var foundUser = UserPrincipal.FindByIdentity(domainContext, IdentityType.SamAccountName, userName))
            {
                return foundUser != null;
            }
        }
    }

    public bool UserExists(string username, bool returnFalseIfDisabled)
        {
            if (String.Equals(username, ""))
                return false;
            else
            {
                ds.Filter = "(&(objectClass=user) (sAMAccountName=" + username + "))";

                SearchResult result = ds.FindOne();

                if (result == null)
                {
                    ds.Filter = "(&(objectClass=user) (mailNickname=" + username + "))";
                    result = ds.FindOne();
                    if (result == null)
                        return false;
                } 

                if (!returnFalseIfDisabled) return true;

                bool disabled = ((int)result.Properties["userAccountControl"][0] == 514 ||
                            (int)result.Properties["userAccountControl"][0] == 546 ||
                            (int)result.Properties["userAccountControl"][0] == 522 ||
                            (int)result.Properties["userAccountControl"][0] == 66050 ||
                            (int)result.Properties["userAccountControl"][0] == 66082);
                return (!disabled);
            }
        }

        public UserStatus status(string username)
        {
            if (String.Equals(username, ""))
                return new UserStatus(false, false);
            else
            {
                ds.Filter = "(&(objectClass=user) (sAMAccountName=" + username + "))";

                SearchResult result = ds.FindOne();

                if (result == null)
                {
                    ds.Filter = "(&(objectClass=user) (mailNickname=" + username + "))";
                    result = ds.FindOne();
                    if (result == null)
                        return new UserStatus(false, false);
                }

                bool disabled = ((int)result.Properties["userAccountControl"][0] == 514 ||
                            (int)result.Properties["userAccountControl"][0] == 546 ||
                            (int)result.Properties["userAccountControl"][0] == 522 ||
                            (int)result.Properties["userAccountControl"][0] == 66050 ||
                            (int)result.Properties["userAccountControl"][0] == 66082);
                return new UserStatus(true, !disabled);
            }
        }

        public bool UserEnabled(string username)
        {
            bool disabled;
            if (String.Equals(username, ""))
                return false;
            else
            {
                ds.Filter = "(&(objectClass=user) (sAMAccountName=" + username + "))";
                SearchResult result = ds.FindOne();

                if (result == null) return false;
                disabled = ((int)result.Properties["userAccountControl"][0] == 514 ||
                            (int)result.Properties["userAccountControl"][0] == 546 ||
                            (int)result.Properties["userAccountControl"][0] == 522 ||
                            (int)result.Properties["userAccountControl"][0] == 66050 ||
                            (int)result.Properties["userAccountControl"][0] == 66082);
                return (!disabled);
            }
        }

        public string GetEmail(string user)
        {
            try
            {
                if (UserExists(user, false))
                {
                    ds.Filter = ("(userprincipalname=" + user + "@" + domain + ")");

                    SearchResult result = ds.FindOne();
                    if (result != null)
                        return result.Properties["mail"][0].ToString();
                    else
                        return null;
                }
                else
                    return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string GetCN(string user)
        {
            if (UserExists(user, false))
            {
                ds.Filter = ("(userprincipalname=" + user + "@" + domain + ")");

                SearchResult result = ds.FindOne();
                if (result != null)
                    return result.Properties["CN"][0].ToString();
                else
                    return null;
            }
            else
                return null;
        }

        public string GetAttribute(string user, string ADAttribute)
        {
            if (UserExists(user, false))
            {
                ds.Filter = ("(userprincipalname=" + user + "@" + domain + ")");
                SearchResult result = ds.FindOne();
                if (result != null)
                    return result.Properties[ADAttribute][0].ToString();
                else
                    return null;
            }
            else
                return null;
        }

        public string GetAttribute(string identifier, string fieldName, string desiredAttribute)
        {
            try
            {
                ds.Filter = "(" + fieldName + "=" + identifier + ")";
                SearchResult result = ds.FindOne();

                return result.Properties[desiredAttribute][0].ToString();
            }

            catch (Exception)
            {
                return null;
            }
        }

        public List<string> GetMultiValueAttribute(string user, string ADAttribute)
        {
            if (UserExists(user, false))
            {
                ds.Filter = "(userprincipalname=" + user + "@" + domain + ")";

                SearchResult result = ds.FindOne();
                if (result != null)
                {
                    List<string> results = new List<string>();
                    ResultPropertyValueCollection values = result.Properties[ADAttribute];
                    foreach (Object value in values)
                    {
                        results.Add(value.ToString());
                    }
                    return results;
                }
                else
                    return null;
            }
            else
                return null;
        }

        public string GetFirstName(string user)
        {
            if (UserExists(user, false))
            {
                ds.Filter = "(userprincipalname=" + user + "@" + domain + ")";
                SearchResult result = ds.FindOne();
                if (result != null)
                    return result.Properties["givenName"][0].ToString();
                else
                    return null;
            }
            else
                return null;
        }

        public bool isUserInGroup(string username, string group)
        {
            string GROUP = group.ToUpper();
            var principleContext = new PrincipalContext(ContextType.Domain, domain);
            var source = UserPrincipal.FindByIdentity(principleContext, username).GetGroups(principleContext);
            List<string> groupList = new List<string>();
            source.ToList().ForEach(element => groupList.Add(element.SamAccountName.ToUpper()));

            foreach (string groupName in groupList)
                if (groupName == GROUP)
                    return true;

            return false;
        }
    }

    public class UserStatus 
    {
        public bool exists,
                    enabled;

        public string sAMAccountName;
        public string commonName;

        public UserStatus(bool doesExist, bool isEnabled)
        {
            exists = doesExist;
            enabled = isEnabled;
        }

        public UserStatus(string sAMAccountName, bool exists, bool enabled)
        {
            this.sAMAccountName = sAMAccountName;
            this.exists = exists;
            this.enabled = enabled;
        }

        public UserStatus(Principal principal)
        {
            sAMAccountName = principal.SamAccountName;
            exists = true;

            var de = (DirectoryEntry)principal.GetUnderlyingObject();
            int flags = (int)de.Properties["userAccountControl"].Value;
            enabled = Convert.ToBoolean(flags & 0x0002);

            commonName = (string)de.Properties[@"cn"][0]; 
        }
    }

