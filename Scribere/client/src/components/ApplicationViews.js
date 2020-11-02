import React, { useContext } from "react";
import { Switch, Route, Redirect, withRouter } from "react-router-dom";
import { UserDataContext } from "../providers/UserDataProvider";
import Login from "./LoginReg/Login";
import Register from "./LoginReg/Register";
import ArticleList from "./Article/ArticleList";
import ArticleDetail from "./Article/ArticleDetail";
import ArticleEdit from "./Article/ArticleEdit";
import ArticleForm from "./Article/ArticleForm";
import UserDetail from "./Users/UserDetail";
import UserList from "./Users/UserList";
import UserEdit from "./Users/UserEdit";


const ApplicationViews = (props) => {
  const { isLoggedIn } = useContext(UserDataContext);

  return (
    <main>
      <Switch>
        <Route path="/" exact>
          {isLoggedIn ? <ArticleList {...props} /> : <Redirect to="/login" />}
        </Route>

        <Route path="/articles/compose" exact>
          {isLoggedIn ? <ArticleForm /> : <Redirect to="/login" />}
        </Route>

        <Route path="/articles" exact>
          {isLoggedIn ? <ArticleList {...props} /> : <Redirect to="/login" />}
        </Route>

        <Route path="/articles/self/:mywork" exact>
          {isLoggedIn ? <ArticleList {...props} /> : <Redirect to="/login" />}
        </Route>

        <Route path="/articles/category/:categoryType" exact>
          {isLoggedIn ? <ArticleList  {...props} /> : <Redirect to="/login" />}
        </Route>

        <Route path="/articles/:id" exact>
          {isLoggedIn ? <ArticleDetail /> : <Redirect to="/login" />}
        </Route>

        <Route path="/articles/edit/:id" exact >
          {isLoggedIn ? <ArticleEdit /> : <Redirect to="/login" />}
        </Route>

        <Route path="/users" exact>
          {isLoggedIn ? <UserList /> : <Redirect to="/login" />}
        </Route>

        <Route path="/users/:id" exact>
          {isLoggedIn ? <UserDetail /> : <Redirect to="/login" />}
        </Route>

        <Route path="/users/edit/:id" exact>
          {isLoggedIn ? <UserEdit /> : <Redirect to="/login" />}
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
export default withRouter(ApplicationViews);