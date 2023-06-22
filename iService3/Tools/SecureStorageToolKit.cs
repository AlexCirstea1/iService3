using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iService3.Tools
{
    internal class SecureStorageToolKit
    {
        public async void SaveUserID(string userID)
        {
            await SecureStorage.Default.SetAsync("UserID", userID.ToString());
        }
        public async void SaveUsername(string username)
        {
            await SecureStorage.Default.SetAsync("Username", username.ToString());
        }
        public async void SaveToken(string SaveToken)
        {
            await SecureStorage.Default.SetAsync("Token", SaveToken.ToString());
        }
        public async Task<string> GetUserID()
        {
            return await SecureStorage.Default.GetAsync("UserID");
        }
        public async Task<string> GetUsername()
        {
            return await SecureStorage.Default.GetAsync("Username");
        }
        public async Task<string> GetToken()
        {
            return await SecureStorage.Default.GetAsync("Token");
        }
        public async Task<string> IsLogged()
        {
            return await SecureStorage.Default.GetAsync("isLogged");
        }
    }
}
