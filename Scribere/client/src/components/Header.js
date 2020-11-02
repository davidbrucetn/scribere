import React, { useState, useContext, useEffect } from 'react';
import { NavLink as RRNavLink } from "react-router-dom";
import {
  Collapse,
  Navbar,
  NavbarToggler,
  NavbarBrand,
  Nav,
  NavItem,
  NavLink,
  UncontrolledDropdown,
  DropdownToggle,
  DropdownMenu,
  DropdownItem,
} from 'reactstrap';
import { UserDataContext } from "../providers/UserDataProvider";
import { CategoryContext } from "../providers/CategoryProvider";

import "./Header.css";


export default function Header() {
  const { isLoggedIn, logout } = useContext(UserDataContext);
  const { categories, getAllCategories } = useContext(CategoryContext);
  const [isOpen, setIsOpen] = useState(false);
  const [isLoading, setIsLoading] = useState(true)
  const toggle = () => setIsOpen(!isOpen);

  const generateNav = () => {
    if (isLoggedIn) {
      Promise.all([getAllCategories()]);
    }


    setIsLoading(false)
  }

  useEffect(() => {
    generateNav()



  }, [isLoggedIn])

  return (
    (!isLoading) ? <div>
      <Navbar color="dark" dark expand="md">
        <NavbarBrand tag={RRNavLink} to="/">Scribere</NavbarBrand>
        <NavbarToggler onClick={toggle} />
        <Collapse isOpen={isOpen} navbar>
          <Nav className="mr-auto" navbar>
            {isLoggedIn &&
              <>
                <NavItem>
                  <NavLink tag={RRNavLink} to="/articles/compose">Compose</NavLink>
                </NavItem>
                <NavItem>
                  <NavLink tag={RRNavLink} to="/articles">Articles</NavLink>
                </NavItem>
                <NavItem>
                  <NavLink tag={RRNavLink} to="/articles/favorites/favarticles">Faves</NavLink>
                </NavItem>
                <NavItem>
                  <NavLink tag={RRNavLink} to="/articles/self/mywork">My Work</NavLink>
                </NavItem>
                <UncontrolledDropdown nav inNavbar>
                  <DropdownToggle nav caret>
                    Categories
              </DropdownToggle>

                  <DropdownMenu style={{ backgroundColor: '#443d3d' }} right>
                    {categories.map((category) =>
                      <DropdownItem key={`DropdownItem=${category.type}`} tag={RRNavLink} className="nav-link" activeClassName="selected" exact to={`/articles/category/${category.type}`}>
                        {category.type}
                      </DropdownItem>
                    )}

                  </DropdownMenu>
                </UncontrolledDropdown>
                <NavItem>
                  <a aria-current="page" className="nav-link" href="/"
                    style={{ cursor: "pointer" }} onClick={logout}>Logout</a>
                </NavItem>
              </>
            }
            {!isLoggedIn &&
              <>
                <NavItem>
                  <NavLink tag={RRNavLink} to="/login">Login</NavLink>
                </NavItem>
                <NavItem>
                  <NavLink tag={RRNavLink} to="/register">Register</NavLink>
                </NavItem>
              </>
            }
          </Nav>
          <Nav navbar>
            {isLoggedIn &&
              <>
                <NavItem>
                  <NavLink tag={RRNavLink} to="/users">Users</NavLink>
                </NavItem>
              </>
            }
          </Nav>
        </Collapse>
      </Navbar>

    </div> : null


  );
}
