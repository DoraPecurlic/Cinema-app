import React, { useContext, useState } from "react";
import { AuthenticationContext } from "../context/AuthenticationContextProvider";
import { jwtDecode } from "jwt-decode";
import { Navigate, Outlet } from "react-router-dom";

const ProtectedRoutes = ({ allowedRoles }) => {
  const { token } = useContext(AuthenticationContext);

  let claims = null;

  if (token) {
    claims = jwtDecode(token);

    console.log(jwtDecode(token), "CLAIMS");
  }

  return claims?.Role ? (
    allowedRoles.includes(claims?.Role) ? (
      <Outlet />
    ) : (
      <Navigate to={"/"} />
    )
  ) : (
    <Navigate to={"/"} />
  );
};

export default ProtectedRoutes;
