import React, { useState, createContext, useContext } from "react";
import { UserDataContext } from "./UserDataProvider";

export const FavoriteContext = createContext();

export const FavoriteProvider = (props) => {
    const [favoriteArticles, setFavoriteArticles] = useState([]);
    const [favoriteAuthors, setFavoriteAuthors] = useState([]);
    const { getToken } = useContext(UserDataContext);

    const apiFavAuthorUrl = "/api/favorite/favauthor";
    const apiFavArticleUrl = "/api/favorite/favarticle";

    const getAllFavoriteAuthorIds = () => {
        return getToken().then((token) =>
            fetch(apiFavAuthorUrl, {
                method: "GET",
                headers: {
                    Authorization: `Bearer ${token}`
                }
            }).then(resp => resp.json())
                .then(setFavoriteAuthors));
    }

    const getAllFavoriteArticleIds = () => {
        return getToken().then((token) =>
            fetch(apiFavArticleUrl, {
                method: "GET",
                headers: {
                    Authorization: `Bearer ${token}`
                }
            }).then(resp => resp.json())
                .then(setFavoriteArticles));
    }

    const addFavoriteAuthor = (favorite) => {
        return getToken().then((token) =>
            fetch(apiFavAuthorUrl, {
                method: "POST",
                headers: {
                    Authorization: `Bearer ${token}`,
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(favorite)
            }).then(function (response) {
                if (!response.ok) {
                    return false;
                }

                return response.json;

            })
        )
    };

    const addFavoriteArticle = (favorite) => {
        return getToken().then((token) =>
            fetch(apiFavArticleUrl, {
                method: "POST",
                headers: {
                    Authorization: `Bearer ${token}`,
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(favorite)
            }).then(function (response) {
                if (!response.ok) {
                    return false;
                }

                return response.json;

            })
        )
    };

    const removeFavoriteAuthorId = (favoriteUserId) => {
        return getToken().then((token) => {
            fetch(`${apiFavAuthorUrl}/${favoriteUserId}`, {
                method: "DELETE",
                headers: {
                    Authorization: `Bearer ${token}`,
                    "Content-Type": "application/json",
                },
            })
        })
    };

    const removeFavoriteArticleId = (favoriteArticleId) => {
        return getToken().then((token) => {
            fetch(`${apiFavArticleUrl}/${favoriteArticleId}`, {
                method: "DELETE",
                headers: {
                    Authorization: `Bearer ${token}`,
                    "Content-Type": "application/json",
                },
            })
        })
    };

    return (
        <FavoriteContext.Provider value={{
            favoriteAuthors, favoriteArticles, addFavoriteArticle, addFavoriteAuthor,
            getAllFavoriteArticleIds, getAllFavoriteAuthorIds, removeFavoriteArticleId, removeFavoriteAuthorId
        }}>
            {props.children}
        </FavoriteContext.Provider>
    );
};