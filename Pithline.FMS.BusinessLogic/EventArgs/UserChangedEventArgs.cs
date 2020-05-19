using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.BusinessLogic.EventArgs
{
    public class UserChangedEventArgs : System.EventArgs
    {
        public UserChangedEventArgs()
        {

        }
        public UserChangedEventArgs(UserInfo newUserInfo, UserInfo oldUserInfo)
        {
            NewUserInfo = newUserInfo;
            OldUserInfo = oldUserInfo;
        }
        public UserInfo NewUserInfo { get; set; }
        public UserInfo OldUserInfo { get; set; }
    }
}
