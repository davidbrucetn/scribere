using Scribere.Models;
using System.Collections.Generic;

namespace Scribere.Repositories
{
    public interface ICircleRepository
    {
        void AddCircle(Circle circle);
        void DeleteCircle(int userId, int friendId);
        List<Circle> GetAllCircles(int userId);
    }
}