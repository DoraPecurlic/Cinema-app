import React, { useState } from "react";

export const AuthenticationContext = React.createContext();

const AuthenticationContextProvider = ({ children }) => {
  const [token, setToken] = useState(sessionStorage.getItem("token"));

  const saveToken = (newState) => {
    if (newState) {
      sessionStorage.setItem("token", newState);
      setToken(newState);
    } else {
      sessionStorage.removeItem("token");
      setToken(undefined);
    }
  };

  return (
    <AuthenticationContext.Provider value={{ token, saveToken }}>
      {children}
    </AuthenticationContext.Provider>
  );
};

export default AuthenticationContextProvider;
