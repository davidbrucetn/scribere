import React, { useState, createContext, useContext } from "react";
import { UserDataContext } from "./UserDataProvider";

export const TagContext = createContext();

export const TagProvider = (props) => {
    const [tags, setTags] = useState([]);

    const { getToken } = useContext(UserDataContext);

    const apiUrl = "/api/tag";

    const addTag = (tag) => {
        return getToken().then((token) =>
            fetch(apiUrl, {
                method: "POST",
                headers: {
                    Authorization: `Bearer ${token}`,
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(tag),
            }));
    };

    const getAllTags = () =>
        getToken().then((token) =>
            fetch(apiUrl, {
                method: "GET",
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            })
                .then((resp) => resp.json())
                .then(setTags)
        );

    const getTagByTagId = (id) =>
        getToken().then((token) =>
            fetch(`${apiUrl}/${id}`,
                {
                    method: "GET",
                    headers: {
                        Authorization: `Bearer ${token}`,
                    },
                })
                .then((res) => res.json()));

    const updateTag = (tag) => {
        return getToken().then((token) => {
            fetch(`${apiUrl}/${tag.id}`, {
                method: "PUT",
                headers: {
                    Authorization: `Bearer ${token}`,
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(tag),
            })
        });
    }

    const deleteTag = (id) => {
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
        <TagContext.Provider value={{
            tags, addTag, getAllTags, getTagByTagId,
            updateTag, deleteTag
        }}>
            {props.children}
        </TagContext.Provider>
    );
};
