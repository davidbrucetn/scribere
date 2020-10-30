import React, { useContext, useEffect, useState } from "react";
import { withRouter } from "react-router-dom";
import { ArticleContext } from "../../providers/ArticleProvider";
import Article from "./Article";



const ArticleList = () => {

    const { articles, getAllArticles } = useContext(ArticleContext);
    const [IsLoading, setIsLoading] = useState(true)


    const generateArticleList = () => {
        getAllArticles();
        setIsLoading(false)
    }



    useEffect(() => {
        generateArticleList();
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    return (
        (!IsLoading) ?

            <div >

                {articles.map((article) => (
                    <Article key={article.id} article={article} generateArticleList={generateArticleList} />
                ))}


            </div> : null
    )

}

export default withRouter(ArticleList);