using System;
namespace Pithline.FMS.DataProvider.AX.SSModels
{
    public class UserInfo
    {

        public String UserId { get; set; }
        public String Name { get; set; }
        public String EmailId { get; set; }
        public String CompanyId { get; set; }
        public String CompanyName { get; set; }
        public Boolean IsValidationEnabled { get; set; }

    }

}

