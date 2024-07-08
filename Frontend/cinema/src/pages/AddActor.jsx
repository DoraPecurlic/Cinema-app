import React, { useState, useContext } from "react";
import { useNavigate } from "react-router-dom";
import ActorService from "../services/ActorService";
import { AuthenticationContext } from "../context/AuthenticationContextProvider";
import { jwtDecode } from "jwt-decode";

const AddActor = () => {
  const { token } = useContext(AuthenticationContext);
  const decodedToken = jwtDecode(token);
  const [name, setName] = useState("");
  const navigate = useNavigate();

  const handleSubmit = async (event) => {
    event.preventDefault();

    try {
      const actorData = {
        name,
        isActive: true,
        createdByUserId: decodedToken.UserId,
        updatedByUserId: decodedToken.UserId,
      };
      await ActorService.addActor(actorData);
      alert("Actor added successfully");
      navigate("/view-actors");
    } catch (error) {
      console.error("Error adding actor:", error);
      alert("Failed to add actor: " + error.message);
    }
  };

  return (
    <div className="container mt-5">
      <h2>Add Actor</h2>
      <form onSubmit={handleSubmit}>
        <div className="input-group mb-3">
          <input
            type="text"
            className="form-control"
            placeholder="Actor Name"
            aria-label="Actor Name"
            aria-describedby="button-addon2"
            value={name}
            onChange={(e) => setName(e.target.value)}
            required
          />
          <button className="btn btn-primary" type="submit" id="button-addon2">
            Add Actor
          </button>
        </div>
      </form>
    </div>
  );
};

export default AddActor;
