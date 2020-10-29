import React from "react";
import { FaStar } from "react-icons/fa";

export default function Home() {
  return (
    <>
      <div className="div__home">
        <header className="masthead text-white text-center header__home">
          <div className="container d-flex align-items-center flex-column">
            <div className="div__container__img">
              <img
                className="image__home"
                src="./assets/logo_transparent.png"
                alt=""
              ></img>
            </div>
            <div className="divider-custom divider-light"></div>
            <div className="divider__custom__line--light"></div>

            <FaStar className="star__home" />

            <div className="divider__custom__line--light"></div>

            <h2 className="pre-wrap masthead-subheading font-weight-light mb-0">
              BLOGS & JOURNALS
            </h2>
          </div>
        </header>
        <div className="container d-flex align-items-center flex-column">
          <h2 className=" d-inline-block mb-0">GET STARTED</h2>

          <div className="divider__custom__line--dark"></div>
          <div className="divider-custom-icon">
            <FaStar className="star__home" />
          </div>
          <div className="divider__custom__line--dark"></div>
        </div>
        {/* @if (!User.Identity.IsAuthenticated)
    {
        <div className="row justify-content-center">
            <a className="btn btn-primary btn-lg" asp-controller="account" asp-action="login">LOGIN</a>
        </div>
    } */}
      </div>
    </>
  );
}
