using Scribere.Models;
using System.Collections.Generic;

namespace Scribere.Repositories
{
    public interface IVisibilityRepository
    {
        void AddVisibility(Visibility visibility);
        void DeleteVisibility(int visibilityId);
        List<Visibility> GetAllVisibilities();
        Visibility GetVisibilityById(int visibilityId);
        void UpdateVisibility(Visibility visibility);
    }
}