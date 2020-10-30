import React, { createContext, useContext } from "react";
import { UserDataContext } from "./UserDataProvider";


export const ArticleTagContext = createContext();

export const ArticleTagProvider = (props) => {
    const { getToken } = useContext(UserDataContext);

    const apiUrl = "/api/articletag";

    const addArticleTag = (articleTag) => {

        return getToken().then((token) =>
            fetch(`${apiUrl}`, {
                method: "POST",
                headers: {
                    Authorization: `Bearer ${token}`,
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(articleTag),
            }));
    };

    const getTagsByArticleId = (articleId) => {
        return getToken().then((token) =>
            fetch(`${apiUrl}/article/${articleId}`,
                {
                    method: "GET",
                    headers: {
                        Authorization: `Bearer ${token}`,
                    },
                })
                .then((res) => res.json()));
    }

    const deleteTagsByArticleId = (id) => {
        return getToken().then((token) => {
            fetch(`${apiUrl}/${id}`, {
                method: "DELETE",
                headers: {
                    Authorization: `Bearer ${token}`,
                    "Content-Type": "application/json",
                },
            })
        })
    };


    return (
        <ArticleTagContext.Provider value={{
            getTagsByArticleId,
            addArticleTag, deleteTagsByArticleId
        }}>
            {props.children}
        </ArticleTagContext.Provider>
    );
};
