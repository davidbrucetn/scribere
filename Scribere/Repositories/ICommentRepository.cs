using Scribere.Models;
using System.Collections.Generic;

namespace Scribere.Repositories
{
    public interface ICommentRepository
    {
        void AddComment(Comment comment);
        void DeleteComment(int commentId);
        List<Comment> GetAllCommentsByArticleId(int articleId);
        Comment GetCommentByCommentId(int commentId);
        void UpdateComment(Comment comment);
    }
}