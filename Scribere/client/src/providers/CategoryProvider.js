import React, { useState, useContext } from "react";
import { UserDataContext } from "./UserDataProvider";
export const CategoryContext = React.createContext();


export const CategoryProvider = (props) => {
    const [category, setCategory] = useState([]);
    const [categories, setCategories] = useState([]);


    const { getToken } = useContext(UserDataContext);

    const apiUrl = "/api/category";

    const addCategory = (category) => {
        return getToken().then((token) =>
            fetch(apiUrl, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${token}`
                },
                body: JSON.stringify(category)
            }));
    };


    const getCategoryById = (id) =>
        getToken().then((token) =>
            fetch(`${apiUrl}/${id}`,
                {
                    method: "GET",
                    headers: {
                        Authorization: `Bearer ${token}`,
                    },
                })
                .then((res) => res.json()));


    const getAllCategories = () => {
        return getToken().then((token) =>

            fetch(apiUrl, {
                method: "GET",
                headers: {
                    Authorization: `Bearer ${token}`
                }
            })
                .then((res) => res.json())
                .then(setCategories))
    };

    const deleteCategory = (id) => {
        return getToken().then((token) =>
            fetch(`${apiUrl}/${id}`, {
                method: "DELETE",
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${token}`
                }
            })
        )
    }

    const updateCategory = (category) => {
        return getToken().then((token) => {
            fetch(`${apiUrl}/${category.id}`, {
                method: "PUT",
                headers: {
                    Authorization: `Bearer ${token}`,
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(category),
            })
        });
    }


    return (
        <CategoryContext.Provider value={{
            category, categories, addCategory, getAllCategories,
            getCategoryById, deleteCategory, updateCategory
        }}>
            {props.children}
        </CategoryContext.Provider>
    )

}

