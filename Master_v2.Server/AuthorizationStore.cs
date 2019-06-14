using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Master_v2.Server
{
    public static class AuthorizationStore
    {
        private static int roleId = 0;
        private static Dictionary<string, List<int>> readPermissions = new Dictionary<string, List<int>>()
        {
            { "person", new List<int>(){ 2,1 } },
            { "store", new List<int>(){ 1,2 } },
            { "bill", new List<int>(){ 1,2 } },
            { "article", new List<int>(){ 1 } }
        };
        private static Dictionary<string, List<int>> writePermissions = new Dictionary<string, List<int>>()
        {
            { "person", new List<int>(){ 1 } },
            { "store", new List<int>(){ 1,2 } },
            { "bill", new List<int>(){ 1 } },
            { "article", new List<int>(){ 1 } }
        };

        public static int getRoleId()
        {
            return roleId;
        }
        public static void setRoleId(int id)
        {
            roleId = id;
        }

        public static bool checkReadPermission(string controller)
        {
            if (readPermissions[controller].Contains(roleId))
            {
                return true;
            }
            return false;
        }

        public static bool checkWritePermissions(string controller)
        {
            if (writePermissions[controller].Contains(roleId))
            {
                return true;
            }
            return false;
        }
    }
}
