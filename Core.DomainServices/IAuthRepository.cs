using System;

namespace Core.DomainServices
{
    public interface IAuthRepository
    {
        public void SavePassword(String password);

        public String GetPassword();
    }
}
