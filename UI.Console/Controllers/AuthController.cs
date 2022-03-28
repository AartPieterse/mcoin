using Core.DomainServices;
using System;

namespace UI.Console.Controllers
{
    public class AuthController
    {
        private readonly IAuthRepository _authRepo;

        public AuthController(IAuthRepository repo)
        {
            this._authRepo = repo;
        }

        public void Authenticate()
        {
            String saved = this._authRepo.GetPassword();
            String input;

            if (saved == null)
                this.FirstPassword();
            else
            {
                while (true)
                {
                    Printer.PrintText("Enter password: ");
                    input = Printer.Listen();
                    if (!saved.Trim().Equals(input.Trim()))
                        Printer.PrintText("Access denied");
                    else
                        break;
                }
            }
        }

        private void FirstPassword()
        {
            Printer.PrintText("Welcome newbie!");
            Printer.PrintText("There is not authentication yet, let's set it up");

            Boolean x = true;
            while (x)
            {
                x = this.PasswordCycle();
            }
        }

        public void SetNewPassword()
        {
            this.Authenticate();
            this.PasswordCycle();
        }

        private Boolean PasswordCycle()
        {
            Printer.PrintText("\nType a password: ");
            String input = Printer.Listen();
            Printer.PrintText("Repeat password: ");
            String input2 = Printer.Listen();

            if (input == input2)
            {
                Printer.PrintText("Password set!\n");
                this._authRepo.SavePassword(input);
                return false;
            }
            else
            {
                Printer.PrintText("Passwords don't match, start over");
                return true;
            }
        }
    }
}
