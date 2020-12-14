using Newtonsoft.Json;
using Olwen_2._0._0.App_Data;
using Olwen_2._0._0.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Olwen_2._0._0.DependencyInjection
{
    public static class StateLogin
    {
        private static AccLogin _acc;
        public static AccLogin AccountLogin
        {
            get
            {
                if (_acc == null)
                {
                    _acc = new AccLogin();
                    return _acc;
                }
                else
                    return _acc;
            }
            set
            {
                _acc = value;
            }
        }

        public static bool IsNull()
        {
            string jsonFromFile;
            using (var reader = new StreamReader("../../App_Data/AccountLogin.json"))
            {
                jsonFromFile = reader.ReadToEnd();
            }
            var account = JsonConvert.DeserializeObject<AccLogin>(jsonFromFile);
            if (string.IsNullOrEmpty(account.NameLogin))
                return true;
            return false;
        }

        public static void WrireJson()
        {
            string strResult = JsonConvert.SerializeObject(AccountLogin,Formatting.Indented);
            using (var writer = new StreamWriter("../../App_Data/AccountLogin.json"))
            {
                writer.Write(strResult);
            }

        }

        public static void ReadJson()
        {
            string jsonFromFile;
            using (var reader = new StreamReader("../../App_Data/AccountLogin.json"))
            {
                jsonFromFile = reader.ReadToEnd();
            }

            AccountLogin = JsonConvert.DeserializeObject<AccLogin>(jsonFromFile);
        }
    }
}
