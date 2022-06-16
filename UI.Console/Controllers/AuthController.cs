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

            if (saved == null)
                this.FirstPassword();
            else
            {
                while (true)
                {
                    Printer.PrintText("Enter password: ");
                    string input = Printer.Listen();

                    string hashString = HashMachine.CalculateSHA256FromString(input);

                    if (HashMachine.AreHashStringsEqual(saved, hashString))
                        break;
                    else
                        Printer.PrintText("Access denied");
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

                string hashString = HashMachine.CalculateSHA256FromString(input);

                this._authRepo.SavePassword(hashString);
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
