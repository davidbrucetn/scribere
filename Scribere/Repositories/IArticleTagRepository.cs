﻿using Scribere.Models;
using System.Collections.Generic;

namespace Scribere.Repositories
{
    public interface IArticleTagRepository
    {
        void AddArticleTag(ArticleTag articleTag);
        void DeleteArticleTag(int articleTagId);
        List<ArticleTag> GetAll(int articleId);
    }
}