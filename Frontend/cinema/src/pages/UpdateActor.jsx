import React, { useState, useEffect, useContext } from "react";
import { useParams, useNavigate } from "react-router-dom";
import ActorService from "../services/ActorService";
import { AuthenticationContext } from "../context/AuthenticationContextProvider";
import { jwtDecode } from "jwt-decode";

const UpdateActor = () => {
  const { token } = useContext(AuthenticationContext);
  const decodedToken = jwtDecode(token);
  const { id } = useParams();
  const [name, setName] = useState("");
  const navigate = useNavigate();

  useEffect(() => {
    const fetchActor = async () => {
      try {
        const response = await ActorService.getActorById(id);
        setName(response.data.name);
      } catch (error) {
        console.error("Error fetching actor:", error);
      }
    };

    fetchActor();
  }, [id]);

  const handleSubmit = async (event) => {
    event.preventDefault();

    try {
      const actorData = {
        id,
        name,
        isActive: true,
        createdByUserId: decodedToken.UserId,
        updatedByUserId: decodedToken.UserId,
      };
      await ActorService.updateActor(id, actorData);
      alert("Actor updated successfully");
      navigate("/view-actors");
    } catch (error) {
      console.error("Error updating actor:", error);
      alert("Failed to update actor: " + error.message);
    }
  };

  return (
    <div className="container mt-5">
      <h2>Update Actor</h2>
      <form onSubmit={handleSubmit}>
        <div className="form-group">
          <label htmlFor="name">Name</label>
          <input
            type="text"
            className="form-control"
            id="name"
            value={name}
            onChange={(e) => setName(e.target.value)}
            required
          />
        </div>
        <button type="submit" className="btn btn-primary mt-2">
          Update Actor
        </button>
      </form>
    </div>
  );
};

export default UpdateActor;
