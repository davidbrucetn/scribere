import React, { useState, createContext, useContext } from "react";
import { UserDataContext } from "./UserDataProvider";

export const CountryContext = createContext();

export const CountryProvider = (props) => {
    const [countries, setCountries] = useState([]);

    const { getToken } = useContext(UserDataContext);

    const apiUrl = "/api/country";

    const addCountry = (country) => {
        return getToken().then((token) =>
            fetch(apiUrl, {
                method: "POST",
                headers: {
                    Authorization: `Bearer ${token}`,
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(country),
            }));
    };

    const getAllCountries = () =>
        getToken().then((token) =>
            fetch(apiUrl, {
                method: "GET",
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            })
                .then((resp) => resp.json())
                .then(setCountries)
        );

    const getCountryById = (countryId) =>
        getToken().then((token) =>
            fetch(`${apiUrl}/article/${countryId}`,
                {
                    method: "GET",
                    headers: {
                        Authorization: `Bearer ${token}`,
                    },
                })
                .then((res) => res.json()));

    const updateCountry = (country) => {
        return getToken().then((token) => {
            fetch(`${apiUrl}/${country.id}`, {
                method: "PUT",
                headers: {
                    Authorization: `Bearer ${token}`,
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(country),
            })
        });
    }

    const deleteCountry = (countryId) => {
        return getToken().then((token) => {
            fetch(`${apiUrl}/${countryId}`, {
                method: "DELETE",
                headers: {
                    Authorization: `Bearer ${token}`,
                    "Content-Type": "application/json",
                },
            })
        })
    };

    return (
        <CountryContext.Provider value={{
            countries, addCountry, getCountryById,
            getAllCountries, updateCountry, deleteCountry
        }}>
            {props.children}
        </CountryContext.Provider>
    );
};
