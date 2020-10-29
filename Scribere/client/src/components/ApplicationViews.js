import React, { useContext } from "react";
import { Switch, Route, Redirect } from "react-router-dom";
import { UserDataContext } from "../providers/UserDataProvider";
import Home from "../components/Home/Home";
import Login from "./LoginReg/Login";
import Register from "./LoginReg/Register";
import ArticleList from "./Article/ArticleList";
import ArticleDetail from "./Article/ArticleDetail";
import ArticleEdit from "./Article/ArticleEdit";
import ArticleForm from "./Article/ArticleForm";


export default function ApplicationViews() {
  const { isLoggedIn } = useContext(UserDataContext);

  return (
    <main>
      <Switch>
        <Route path="/" exact>
          {isLoggedIn ? <Home /> : <Redirect to="/login" />}
        </Route>

        <Route path="/articles/compose" exact>
          {isLoggedIn ? <ArticleForm /> : <Redirect to="/login" />}
        </Route>

        <Route path="/articles" exact>
          {isLoggedIn ? <ArticleList /> : <Redirect to="/login" />}
        </Route>

        <Route path="/articles/:id" exact>
          {isLoggedIn ? <ArticleDetail /> : <Redirect to="/login" />}
        </Route>

        <Route path="/articles/edit/:id" exact >
          {isLoggedIn ? <ArticleEdit /> : <Redirect to="/login" />}
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
