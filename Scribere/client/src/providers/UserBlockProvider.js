import React, { useState, createContext, useContext } from "react";
import { UserDataContext } from "./UserDataProvider";

export const UserBlockContext = createContext();

export const UserBlockProvider = (props) => {
    const [userBlocks, setUserBlocks] = useState([]);

    const { getToken } = useContext(UserDataContext);

    const apiUrl = "/api/userblock";

    const addUserBlock = (userBlock) => {
        return getToken().then((token) =>
            fetch(apiUrl, {
                method: "POST",
                headers: {
                    Authorization: `Bearer ${token}`,
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(userBlock),
            }));
    };

    const getAllUserBlocks = () =>
        getToken().then((token) =>
            fetch(apiUrl, {
                method: "GET",
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            })
                .then((resp) => resp.json())
                .then(setUserBlocks)
        );


    const deleteUserBlock = (userBlockId) => {
        return getToken().then((token) => {
            fetch(`${apiUrl}/${userBlockId}`, {
                method: "DELETE",
                headers: {
                    Authorization: `Bearer ${token}`,
                    "Content-Type": "application/json",
                },
            })
        })
    };

    return (
        <UserBlockContext.Provider value={{
            userBlocks, addUserBlock, getAllUserBlocks,
            deleteUserBlock
        }}>
            {props.children}
        </UserBlockContext.Provider>
    );
};
