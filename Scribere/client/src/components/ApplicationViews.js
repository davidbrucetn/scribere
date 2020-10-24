import React, { useContext } from "react";
import { Switch, Route, Redirect } from "react-router-dom";
import { UserDataContext } from "../providers/UserDataProvider";
import Home from "./Home";
import Login from "./Login";
import Register from "./Register";


export default function ApplicationViews() {
  const { isLoggedIn } = useContext(UserDataContext);

  return (
    <main>
      <Switch>
        <Route path="/" exact>
          {isLoggedIn ? <Home /> : <Redirect to="/login" />}
        </Route>

        <Route path="/login">
          <Login />
        </Route>

        <Route path="/register">
          <Register />
        </Route>
      </Switch>
    </main>
  );
};
