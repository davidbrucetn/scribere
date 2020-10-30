import React, { useState, useContext, createContext } from "react";
import { UserDataContext } from "./UserDataProvider";

export const CommentContext = createContext();

export const CommentProvider = (props) => {
    const [comments, setComments] = useState([]);

    const { getToken } = useContext(UserDataContext);

    const apiUrl = "/api/comment"

    const GetAllCommentsByArticle = (articleId) => {
        return getToken().then((token) =>
            fetch(`${apiUrl}/article/${articleId}`, {
                method: "GET",
                headers: {
                    Authorization: `Bearer ${token}`,
                }
            }).then((res) => res.json())
                .then(setComments)
        );
    };

    const getCommentById = (id) => {
        return getToken().then((token) =>
            fetch(`${apiUrl}/${id}`,
                {
                    method: "GET",
                    headers: {
                        Authorization: `Bearer ${token}`,
                    },
                })
                .then((res) => res.json()));
    }


    const addComment = (comment) => {
        return getToken().then((token) =>
            fetch(apiUrl, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${token}`
                },
                body: JSON.stringify(comment)
            }));
    };

    const deleteComment = (id) => {
        return getToken().then((token) => {
            fetch(`${apiUrl}/${id}`, {
                method: "DELETE",
                headers: {
                    Authorization: `Bearer ${token}`,
                    "Content-Type": "application/json"
                }
            })
        })
    };

    const updateComment = (comment) => {
        return getToken().then((token) => {
            fetch(`${apiUrl}/${comment.id}`, {
                method: "PUT",
                headers: {
                    Authorization: `Bearer ${token}`,
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(comment),
            })
        });
    };

    return (
        <CommentContext.Provider value={{
            comments, addComment, getCommentById,
            GetAllCommentsByArticle, updateComment, deleteComment
        }}>
            {props.children}
        </CommentContext.Provider>
    );
}
