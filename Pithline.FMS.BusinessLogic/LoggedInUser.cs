using Microsoft.Practices.Prism.StoreApps;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.BusinessLogic
{
    public class LoggedInUser : ValidatableBindableBase
    {
        private int id;

        [PrimaryKey, AutoIncrement]
        public int Id
        {
            get { return id; }
            set { SetProperty(ref id, value); }
        }

        private string userId;
        [RestorableState]
        public string UserId
        {
            get { return userId; }
            set { SetProperty(ref userId, value); }
        }

        private string password;
        [RestorableState]
        public string Password
        {
            get { return password; }
            set { SetProperty(ref password, value); }
        }


    }
}
