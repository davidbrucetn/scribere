import React, { useContext, useEffect } from "react";
import { ArticleContext } from "../../providers/ArticleProvider";
import Article from "./Article";



const ArticleList = () => {

    const { articles, getAllArticles } = useContext(ArticleContext);

    useEffect(() => {
        getAllArticles();
    }, []);

    return (
        <div >

            {articles.map((article) => (
                <Article key={article.id} article={article} />
            ))}


        </div>
    )

}

export default ArticleList;