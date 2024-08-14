using System;
using AOT.Framework.Mvc;

namespace HotUpdate.Model
{
    public sealed class LoginModel:IModel
    {
        public string token;
        public string userName;
        public Int32 userId;
        public void Init()
        {
            
        }
    }
}