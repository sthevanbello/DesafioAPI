using System;

namespace ForumGames.Utils.Exceptions
{
    public class CannotDeleteException : ApplicationException
    {
        public CannotDeleteException(string message) : base(message)
        {

        }
    }
}
