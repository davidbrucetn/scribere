import React from "react";
import { BrowserRouter as Router } from "react-router-dom";
import Header from "./components/Header";
import ApplicationViews from "./components/ApplicationViews";
import {UserDataProvider} from "./providers/UserDataProvider";
import './App.css';

function App() {
  return (
    <Router>
        <UserDataProvider>
                      <Header />
                      <ApplicationViews />

        </UserDataProvider>
    </Router>
  );
}

export default App;
