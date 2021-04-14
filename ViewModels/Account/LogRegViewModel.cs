using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.ViewModels.Account
{
    public class LogRegViewModel
    {
        public LoginViewModel loginView { get; set; }
        public RegisterViewModel registerView { get; set; }
        public LogRegViewModel(LoginViewModel lg)
        {
            loginView = lg;
            registerView = null;
        }
        public LogRegViewModel(RegisterViewModel rg)
        {
            registerView = rg;
            loginView = null;
        }
        public LogRegViewModel()
        {

        }
    }
}
