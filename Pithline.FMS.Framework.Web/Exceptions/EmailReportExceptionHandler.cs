using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pithline.FMS.Framework.Web.Exceptions
{
    [ExceptionHandler(Type="Email")]
    public class EmailReportExceptionHandler:IExceptionHandler
    {
        public void Handle(Exception ex)
        {
            //write code to send email of the details of the exception
        }
    }
}
