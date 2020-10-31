import React from "react";
import { BrowserRouter as Router } from "react-router-dom";
import Header from "./components/Header";
import ApplicationViews from "./components/ApplicationViews";
import { UserDataProvider } from "./providers/UserDataProvider";
import { UserBlockProvider } from "./providers/UserBlockProvider";
import { ArticleProvider } from "./providers/ArticleProvider";
import { FavoriteProvider } from "./providers/FavoriteProvider";
import { ArticleTagProvider } from "./providers/ArticleTagProvider";
import { CategoryProvider } from "./providers/CategoryProvider";
import { VisibilityProvider } from "./providers/VisibilityProvider";
import { CommentProvider } from "./providers/CommentProvider";
import { CountryProvider } from "./providers/CountryProvider";
import { TagProvider } from "./providers/TagProvider";
import './App.css';

function App() {
  return (
    <Router>
      <UserDataProvider>
        <UserBlockProvider>
          <CountryProvider>
            <ArticleProvider>
              <FavoriteProvider>
                <ArticleTagProvider>
                  <CategoryProvider>
                    <VisibilityProvider>
                      <CommentProvider>
                        <TagProvider>
                          <Header />
                          <ApplicationViews />
                        </TagProvider>
                      </CommentProvider>
                    </VisibilityProvider>
                  </CategoryProvider>
                </ArticleTagProvider>
              </FavoriteProvider>
            </ArticleProvider>
          </CountryProvider>
        </UserBlockProvider>
      </UserDataProvider>
    </Router>
  );
}

export default App;
