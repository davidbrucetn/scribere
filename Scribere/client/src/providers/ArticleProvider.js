import React, { useState, createContext, useContext } from "react";
import { UserDataContext } from "./UserDataProvider";

export const ArticleContext = createContext();
let orgArray = [];
export const ArticleProvider = (props) => {

    const [articles, setArticles] = useState([]);

    const { getToken } = useContext(UserDataContext);

    const apiUrl = "/api/article";

    const getAllArticles = (ALFilter) => {
        const thisUser = JSON.parse(sessionStorage.UserData);
        return getToken().then((token) =>
            fetch(apiUrl, {
                method: "GET",
                headers: {
                    Authorization: `Bearer ${token}`
                }
            }).then(resp => resp.json())
                .then(articleArray => {
                    if (ALFilter !== undefined || ALFilter !== null) {
                        if (ALFilter === "Self") {
                            orgArray = articleArray.filter(article => article.userId === thisUser.id);
                        } else if (ALFilter === "All") {
                            orgArray = articleArray;
                        } else {

                            orgArray = articleArray.filter(article => article.category.type === ALFilter);
                        }
                    } else {
                        orgArray = articleArray;
                    }
                    return orgArray;

                }));

    };




    const getFavoriteArticles = () => {
        return getToken().then((token) =>
            fetch(`${apiUrl}/favorite`, {
                method: "GET",
                headers: {
                    Authorization: `Bearer ${token}`
                }
            }).then(resp => resp.json())
                .then(faveArticleArray => {
                    return faveArticleArray;
                }));
    };

    const getMyArticles = () => {
        return getToken().then((token) =>
            fetch(`${apiUrl}/mywriting`, {
                method: "GET",
                headers: {
                    Authorization: `Bearer ${token}`
                }
            }).then(resp => resp.json())
                .then(setArticles));
    };

    const getArticlebyId = (id) => {
        return getToken().then((token) =>
            fetch(`${apiUrl}/${id}`, {
                method: "GET",
                headers: {
                    Authorization: `Bearer ${token}`
                }
            }).then(resp => resp.json()));
    };

    const addArticle = (article) => {
        return getToken().then((token) =>
            fetch(apiUrl, {
                method: "POST",
                headers: {
                    Authorization: `Bearer ${token}`,
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(article)
            }).then(resp => resp.json()));
    };

    const updateArticle = (article) => {
        return getToken().then((token) =>
            fetch(`${apiUrl}/${article.id}`, {
                method: "PUT",
                headers: {
                    Authorization: `Bearer ${token}`,
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(article),
            })
        );
    };

    const deleteArticle = (id) => {
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
        <ArticleContext.Provider value={{
            articles, addArticle, getAllArticles,
            getMyArticles, getFavoriteArticles, getArticlebyId, updateArticle,
            deleteArticle
        }}>
            {props.children}
        </ArticleContext.Provider>
    );
};