using System;

namespace UI.Console.HTTP
{
    public class HashRequest
    {
        public HashRequest()
        {

        }

        public String GetHashRequirements()
        {
            return "012x";
        }

        public Int32 SendHash(String hash)
        {
            return 100;
        }
    }
}
