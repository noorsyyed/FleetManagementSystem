using Pithline.FMS.BusinessLogic.Portable.SSModels;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Pithline.FMS.ServiceScheduling.UILogic.Portable.Services
{
    public interface ITaskService
    {
         Task<ObservableCollection<Pithline.FMS.BusinessLogic.Portable.SSModels.Task>> GetTasksAsync(UserInfo userInfo);

         Task<CaseStatus> UpdateStatusListAsync(Pithline.FMS.BusinessLogic.Portable.SSModels.Task task, UserInfo userInfo);
    }
}
