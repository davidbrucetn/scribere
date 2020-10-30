import React, { useState, createContext, useContext } from "react";
import { UserDataContext } from "./UserDataProvider";

export const VisibilityContext = createContext();

export const VisibilityProvider = (props) => {
    const [visibilities, setVisibilities] = useState([]);

    const { getToken } = useContext(UserDataContext);

    const apiUrl = "/api/visibility";


    const addVisibility = (visibility) => {
        return getToken().then((token) =>
            fetch(apiUrl, {
                method: "POST",
                headers: {
                    Authorization: `Bearer ${token}`,
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(visibility),
            }));
    };

    const getAllVisibilities = () =>
        getToken().then((token) =>
            fetch(apiUrl, {
                method: "GET",
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            })
                .then((resp) => resp.json())
                .then(setVisibilities)
        );

    const getVisibilityByVisibilityId = (id) =>
        getToken().then((token) =>
            fetch(`${apiUrl}/${id}`,
                {
                    method: "GET",
                    headers: {
                        Authorization: `Bearer ${token}`,
                    },
                })
                .then((res) => res.json()));

    const updateVisibility = (visibility) => {
        return getToken().then((token) => {
            fetch(`${apiUrl}/${visibility.id}`, {
                method: "PUT",
                headers: {
                    Authorization: `Bearer ${token}`,
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(visibility),
            })
        });
    }

    const deleteVisibility = (id) => {
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
        <VisibilityContext.Provider value={{
            visibilities, addVisibility, getAllVisibilities, getVisibilityByVisibilityId,
            updateVisibility, deleteVisibility
        }}>
            {props.children}
        </VisibilityContext.Provider>
    );
};
