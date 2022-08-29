using System;

namespace ForumGames.Utils.Exceptions
{
    public class ThereIsntCategoryException : ApplicationException
    {
        public ThereIsntCategoryException(string message) : base(message)
        {

        }
    }
}
