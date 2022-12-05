using API.Core.Models;

namespace API.Core.Models
{
    public class UserConstants
    {
        public static List<UserModel> Users = new()
        {
            new UserModel() { Username="mert", Password="merdo", Role="Admin" }
        };
    }
}
