import React, { useState, createContext, useContext } from "react";
import { UserDataContext } from "./UserDataProvider";

export const BookContext = createContext();

export const BookProvider = (props) => {
    const [book, setBook] = useState([]);

    const { getToken } = useContext(UserDataContext);

    const apiUrl = "/api/book";

    const addBook = (book) => {
        return getToken().then((token) =>
            fetch(apiUrl, {
                method: "POST",
                headers: {
                    Authorization: `Bearer ${token}`,
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(book),
            }));
    };

    const getBookByArticleId = (articleId) =>
        getToken().then((token) =>
            fetch(`${apiUrl}/article/${articleId}`,
                {
                    method: "GET",
                    headers: {
                        Authorization: `Bearer ${token}`,
                    },
                })
                .then((res) => res.json())
                .then(setBook)
        );

    const updateBook = (book) => {
        return getToken().then((token) => {
            fetch(`${apiUrl}/${book.id}`, {
                method: "PUT",
                headers: {
                    Authorization: `Bearer ${token}`,
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(book),
            })
        });
    }

    const deleteBook = (bookId) => {
        return getToken().then((token) => {
            fetch(`${apiUrl}/${bookId}`, {
                method: "DELETE",
                headers: {
                    Authorization: `Bearer ${token}`,
                    "Content-Type": "application/json",
                },
            })
        })
    };

    return (
        <BookContext.Provider value={{
            book, addBook, getBookByArticleId,
            updateBook, deleteBook
        }}>
            {props.children}
        </BookContext.Provider>
    );
};
