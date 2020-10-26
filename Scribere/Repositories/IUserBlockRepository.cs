using Scribere.Models;
using System.Collections.Generic;

namespace Scribere.Repositories
{
    public interface IUserBlockRepository
    {
        void AddUserBlock(UserBlock userBlock);
        void DeleteUserBlock(int SourceUserId, int userBlockId);
        List<UserBlock> GetAll(int sourceUserId);
    }
}