import React, { useState } from "react";
import axios from "axios";
import { Link } from "react-router-dom";

const Register = ({ openLoginModal }) => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");

  const handleRegister = async (e) => {
    e.preventDefault();
    try {
      const response = await axios.post("http://localhost:5058/user/register", {
        email,
        password,
        firstName,
        lastName,
      });
      alert("Registration successful!");

      // setRegisterModalIsOpen(false);
      // //       setLoginModalIsOpen(true);
    } catch (error) {
      alert("Error during registration: " + error.message);
    }
  };

  return (
    <div className="container mt-5">
      <h2>Register</h2>
      <form
        onSubmit={handleRegister}
        style={{ maxWidth: "400px", margin: "0 auto" }}
      >
        <div className="form-group">
          <label htmlFor="firstName">First Name</label>
          <input
            type="text"
            className="form-control"
            id="firstName"
            value={firstName}
            onChange={(e) => setFirstName(e.target.value)}
            required
          />
        </div>
        <div className="form-group">
          <label htmlFor="lastName">Last Name</label>
          <input
            type="text"
            className="form-control"
            id="lastName"
            value={lastName}
            onChange={(e) => setLastName(e.target.value)}
            required
          />
        </div>
        <div className="form-group">
          <label htmlFor="email">Email address</label>
          <input
            type="email"
            className="form-control"
            id="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
          />
        </div>
        <div className="form-group">
          <label htmlFor="password">Password</label>
          <input
            type="password"
            className="form-control"
            id="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />
        </div>
        <button type="submit" className="btn btn-primary">
          Register
        </button>
        <p className="mt-3">
          Already have an account?{" "}
          <Link to="#" onClick={openLoginModal}>
            Login here
          </Link>
        </p>
      </form>
    </div>
  );
};

export default Register;
