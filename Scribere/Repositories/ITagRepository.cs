using Scribere.Models;
using System.Collections.Generic;

namespace Scribere.Repositories
{
    public interface ITagRepository
    {
        void AddTag(Tag tag);
        void DeleteTag(int tagId);
        List<Tag> GetAll();
        Tag GetTagById(int tagId);
        void UpdateTag(Tag tag);
    }
}