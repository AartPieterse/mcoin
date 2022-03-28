using Core.DomainServices;
using System;

namespace Infrastructure
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AuthFileContext _context;

        public AuthRepository(AuthFileContext context)
        {
            this._context = context;
        }

        public void SavePassword(String password)
        {
            this._context.AddPassword(password);
        }

        public String GetPassword()
        {
            return this._context.GetPassword();
        }
    }
}
