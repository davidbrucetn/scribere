using Scribere.Models;
using System.Collections.Generic;

namespace Scribere.Repositories
{
    public interface IUserDataRepository
    {
        void Add(UserData UserData);
        void DeleteUser(int id);
        List<UserData> GetAll();
        UserData GetByFirebaseUserId(string firebaseUserId);
        UserData GetById(int id);
        List<UserData> GetDeactivated();
        void ReactivateUser(int id);
        void UpdateUser(UserData user);
    }
}