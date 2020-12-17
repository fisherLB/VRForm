using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphicsEvaluatePlatform.Infrastructure;
using GraphicsEvaluatePlatform.Client.Models;
using System.Data;
using GraphicsEvaluatePlatform.Client.ViewModels;

namespace GraphicsEvaluatePlatform.Client.Repository
{
    interface IUserService
    {
        OperationResult AddUser(UserModel model);
        OperationResult EditUser(UserModel model);
        //OperationResult GetUserList();
        OperationResult DeleteUser(string ids);
        OperationResult GetList(int pageNum, int currentlyPage, Dictionary<string, object> dic);

        List<UserItemViewModel> GetAllUserList(DataTable dt);
    }
}
