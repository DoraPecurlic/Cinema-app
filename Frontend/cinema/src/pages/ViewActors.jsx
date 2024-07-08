import React, { useState, useEffect } from "react";
import { Link } from "react-router-dom";
import ActorService from "../services/ActorService";

const ViewActors = () => {
  const [actors, setActors] = useState([]);

  useEffect(() => {
    const fetchActors = async () => {
      try {
        const response = await ActorService.getAllActors();
        setActors(response.data);
      } catch (error) {
        console.error("Error fetching actors:", error);
      }
    };

    fetchActors();
  }, []);

  const handleDelete = async (id) => {
    try {
      await ActorService.deleteActor(id);
      setActors(actors.filter((actor) => actor.id !== id));
      alert("Actor deleted successfully");
    } catch (error) {
      console.error("Error deleting actor:", error);
      alert("Failed to delete actor: " + error.message);
    }
  };

  return (
    <div className="container mt-5">
      <h2>View Actors</h2>
      {actors.length === 0 ? (
        <p>No actors found...</p>
      ) : (
        <table className="table">
          <thead>
            <tr>
              <th>Name</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {actors.map((actor) => (
              <tr key={actor.id}>
                <td>{actor.name}</td>
                <td>
                  <Link to={`/update-actor/${actor.id}`} className="btn btn-primary me-2">
                    Update
                  </Link>
                  <button
                    className="btn btn-danger"
                    onClick={() => handleDelete(actor.id)}
                  >
                    Delete
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
};

export default ViewActors;
