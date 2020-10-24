import React, { useState, useEffect, createContext } from "react";
import { Spinner } from "reactstrap";
import * as firebase from "firebase/app";
import "firebase/auth";

export const UserDataContext = createContext();

export function UserDataProvider(props) {
  const apiUrl = "/api/UserData";

  const UserData = sessionStorage.getItem("UserData");
  const [isLoggedIn, setIsLoggedIn] = useState(UserData != null);
  const [users, setUsers] = useState([]);

  const [isFirebaseReady, setIsFirebaseReady] = useState(false);
  useEffect(() => {
    firebase.auth().onAuthStateChanged((u) => {
      setIsFirebaseReady(true);
    });
  }, []);

  const login = (email, pw) => {
    
    return firebase
      .auth()
      .signInWithEmailAndPassword(email, pw)
      .then((signInResponse) => getUserData(signInResponse.user.uid))
      .then((UserData) => {
        sessionStorage.setItem("UserData", JSON.stringify(UserData));
        setIsLoggedIn(true);
      });
  };

  const logout = () => {
    return firebase
      .auth()
      .signOut()
      .then(() => {
        sessionStorage.clear();
        setIsLoggedIn(false);
      });
  };

  const register = (UserData, password) => {
    return firebase
      .auth()
      .createUserWithEmailAndPassword(UserData.email, password)
      .then((createResponse) =>
        saveRegisterUser({ ...UserData, firebaseUserId: createResponse.user.uid })
      )
      .then((savedUserData) => {

        sessionStorage.setItem("UserData", JSON.stringify(savedUserData));
        setIsLoggedIn(true);
      });
  };

  const getToken = () => firebase.auth().currentUser.getIdToken();



  const getUserData = (firebaseUserId) => {
    return getToken().then((token) =>
      fetch(`${apiUrl}/${firebaseUserId}`, {
        method: "GET",
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }).then((resp) => resp.json())
    );
  };

  const saveRegisterUser = (UserData) => {
    return getToken().then((token) =>
      fetch(apiUrl, {
        method: "POST",
        headers: {
          Authorization: `Bearer ${token}`,
          "Content-Type": "application/json"
        },
        body: JSON.stringify(UserData)
      }).then(resp => resp.json()));
  };

  const saveUser = (UserData) => {
    return getToken().then((token) =>
      fetch(`${apiUrl}/edittype`, {
        method: "PUT",
        headers: {
          Authorization: `Bearer ${token}`,
          "Content-Type": "application/json",
        },
        body: JSON.stringify(UserData),
      }).then(function (response) {
        if (!response.ok) {
          return false;
        }

        return response.ok;

      })
    );
  };

  const deactivateUserData = (id) => {
    return getToken().then((token) =>
      fetch(`${apiUrl}/${id}`, {
        method: "DELETE",
        headers: {
          Authorization: `Bearer ${token}`,
          "Content-Type": "application/json",
        }
      }).then(function (response) {
        if (!response.ok) {
          return false;
        }

        return response.ok;

      })
    );
  };

  const reactivateUserData = (id) => {
    return getToken().then((token) =>
      fetch(`${apiUrl}/reactivate/${id}`, {
        method: "PUT",
        headers: {
          Authorization: `Bearer ${token}`,
          "Content-Type": "application/json",
        }
      }).then(function (response) {
        if (!response.ok) {
          return false;
        }

        return response.ok;

      })
    );
  };

  const getAllUsers = () =>
    getToken().then((token) =>
      fetch(apiUrl, {
        method: "GET",
        headers: {
          Authorization: `Bearer ${token}`,
        },
      })
        .then((resp) => resp.json())
        .then(setUsers)
    );


  const getDeactivatedUsers = () =>
    getToken().then((token) =>
      fetch(`${apiUrl}/deactivated`, {
        method: "GET",
        headers: {
          Authorization: `Bearer ${token}`,
        },
      })
        .then((resp) => resp.json())
        .then(setUsers)
    );

  const getUserById = (id) =>
    getToken().then((token) =>
      fetch(`${apiUrl}/details/${id}`, {
        method: "GET",
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }).then((resp) => resp.json())
    );

  return (
    <UserDataContext.Provider
      value={{
        isLoggedIn,
        login,
        logout,
        deactivateUserData,
        register,
        getToken,
        users,
        getAllUsers,
        getUserById,
        getDeactivatedUsers,
        reactivateUserData,
        saveUser,
        saveRegisterUser
      }}
    >
      {isFirebaseReady ? (
        props.children
      ) : (
          <Spinner className="app-spinner dark" />
        )}
    </UserDataContext.Provider>
  );
}
