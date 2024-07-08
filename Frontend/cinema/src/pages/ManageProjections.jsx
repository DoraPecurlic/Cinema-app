import React, { useState, useEffect } from "react";
import { Modal, Button, Form } from "react-bootstrap";
import ProjectionService from "../services/ProjectionService";
import MovieService from "../services/MovieService";
import HallService from "../services/HallService";
import { Link } from "react-router-dom";
import "../App.css";

const ManageProjections = () => {
  const [projections, setProjections] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [showEditModal, setShowEditModal] = useState(false);
  const [selectedProjection, setSelectedProjection] = useState(null);
  const [movies, setMovies] = useState([]);
  const [loadingMovies, setLoadingMovies] = useState(true);
  const [errorMovies, setErrorMovies] = useState(null);
  const [availableHalls, setAvailableHalls] = useState([]);
  const [loadingHalls, setLoadingHalls] = useState(false);
  const [errorHalls, setErrorHalls] = useState(null);
  const [successMessage, setSuccessMessage] = useState("");
  const [filters, setFilters] = useState({
    genreId: "",
    languageId: "",
    sortBy: "Duration",
    sortOrder: "DESC",
    pageNumber: 1,
    pageSize: 4,
  });

  useEffect(() => {
    const fetchProjections = async () => {
      try {
        const response = await ProjectionService.getAllProjections();
        setProjections(response.data);
        setLoading(false);
      } catch (error) {
        console.error("Error fetching projections:", error);
        setError("Error fetching projections. Please try again later.");
        setLoading(false);
      }
    };

    fetchProjections();
  }, []);

  useEffect(() => {
    const fetchFilteredMovies = async () => {
      try {
        const response = await MovieService.getFilteredMovies(filters);
        setMovies(response.data);
        setLoadingMovies(false);
      } catch (error) {
        console.error("Error fetching filtered movies:", error);
        setErrorMovies("Error fetching movies. Please try again later.");
        setLoadingMovies(false);
      }
    };

    fetchFilteredMovies();
  }, [filters]);

  const formatDate = (dateString) => {
    const options = { year: "numeric", month: "2-digit", day: "2-digit" };
    const date = new Date(dateString);
    return date.toLocaleDateString("en-GB", options);
  };

  const formatDateForInput = (dateString) => {
    const date = new Date(dateString);
    date.setMinutes(date.getMinutes() - date.getTimezoneOffset());
    return date.toISOString().split("T")[0];
  };

  const formatTime = (timeString) => {
    const time = new Date(`1970-01-01T${timeString}`);
    const options = { hour: "2-digit", minute: "2-digit" };
    return time.toLocaleTimeString("en-GB", options);
  };

  const handleDeleteProjection = async (projectionId) => {
    try {
      await ProjectionService.deleteProjection(projectionId);
      setProjections(
        projections.filter((projection) => projection.id !== projectionId)
      );
    } catch (error) {
      console.error(
        `Error deleting projection with ID ${projectionId}:`,
        error
      );
      setError(
        `Error deleting projection with ID ${projectionId}. Please try again later.`
      );
    }
  };

  const handleEditProjection = async (projection) => {
    setSelectedProjection({
      ...projection,
      date: formatDateForInput(projection.date),
      time: projection.time.slice(0, 5),
    });

    try {
      setLoadingHalls(true);
      const response = await HallService.getAvailableHalls(
        projection.date,
        projection.time,
        projection.movieId
      );
      setAvailableHalls(response.data);
      setLoadingHalls(false);
      setShowEditModal(true);
    } catch (error) {
      console.error("Error fetching available halls:", error);
      setErrorHalls("Error fetching available halls. Please try again later.");
      setLoadingHalls(false);
    }
  };

  const handleUpdateProjection = async () => {
    try {
      const updatedProjection = {
        id: selectedProjection.id,
        date: selectedProjection.date,
        time: selectedProjection.time,
        movieId: selectedProjection.movieId,
        hall: selectedProjection.hall,
        userId: selectedProjection.userId,
      };

      await ProjectionService.updateProjection(
        selectedProjection.id,
        updatedProjection
      );
      setProjections(
        projections.map((proj) =>
          proj.id === selectedProjection.id ? updatedProjection : proj
        )
      );
      setShowEditModal(false);

      setSuccessMessage("Projection updated successfully!");
      setTimeout(() => {
        setSuccessMessage("");
      }, 2500);
    } catch (error) {
      console.error(
        `Error updating projection with ID ${selectedProjection.id}:`,
        error
      );
      if (error.response) {
        console.error("Server error:", error.response.data);
      }
      setError(
        `Error updating projection with ID ${selectedProjection.id}. Please try again later.`
      );
    }
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    setSelectedProjection((prevState) => ({
      ...prevState,
      [name]: name === "time" ? value + ":00" : value,
    }));
    console.log(movies);
  };

  if (loading || loadingMovies) {
    return <p>Loading projections...</p>;
  }

  if (error || errorMovies) {
    return <div className="alert alert-danger">{error || errorMovies}</div>;
  }

  return (
    <>
      <div className="container mt-4">
        <h2 className="text-center mb-4">Manage Projections</h2>
        {successMessage && (
          <div className="alert alert-success mt-3">{successMessage}</div>
        )}
        {projections.map((projection) => (
          <div key={projection.id} className="projection-item">
            <div>
              <strong>Title:</strong> {projection.title}
            </div>
            <div>
              <strong>Date:</strong> {formatDate(projection.date)}
            </div>
            <div>
              <strong>Time:</strong> {formatTime(projection.time)}
            </div>

            <button
              className="btn btn-primary"
              onClick={() => handleEditProjection(projection)}
            >
              Edit
            </button>
            <button
              className="btn btn-danger"
              onClick={() => handleDeleteProjection(projection.id)}
            >
              Delete
            </button>
            <hr />
          </div>
        ))}

        {selectedProjection && (
          <Modal show={showEditModal} onHide={() => setShowEditModal(false)}>
            <Modal.Header closeButton>
              <Modal.Title>Edit Projection</Modal.Title>
            </Modal.Header>
            <Modal.Body>
              <Form>
                <Form.Group>
                  <Form.Label>Title</Form.Label>
                  <Form.Control
                    as="select"
                    name="movieId"
                    value={selectedProjection.movieId}
                    onChange={handleChange}
                  >
                    {movies.map((movie) => (
                      <option key={movie.movieId} value={movie.movieId}>
                        {movie.title}
                      </option>
                    ))}
                  </Form.Control>
                </Form.Group>
                <Form.Group>
                  <Form.Label>Date</Form.Label>
                  <Form.Control
                    type="date"
                    name="date"
                    value={selectedProjection.date}
                    onChange={handleChange}
                  />
                </Form.Group>
                <Form.Group>
                  <Form.Label>Time</Form.Label>
                  <Form.Control
                    type="time"
                    name="time"
                    value={selectedProjection.time}
                    onChange={handleChange}
                  />
                </Form.Group>
                <Form.Group>
                  <Form.Label>Hall</Form.Label>
                  {selectedProjection.date &&
                    selectedProjection.time &&
                    selectedProjection.movieId &&
                    (loadingHalls ? (
                      <p>Loading halls...</p>
                    ) : availableHalls.length === 0 ? (
                      <p>
                        No available halls for selected date, time, and movie.
                      </p>
                    ) : (
                      <Form.Control
                        as="select"
                        name="hall"
                        value={selectedProjection.hall}
                        onChange={handleChange}
                      >
                        {availableHalls.map((hall) => (
                          <option key={hall.id} value={hall.id}>
                            Hall {hall.hallNumber}
                          </option>
                        ))}
                      </Form.Control>
                    ))}
                </Form.Group>
              </Form>
            </Modal.Body>
            <Modal.Footer>
              <Button
                variant="secondary"
                onClick={() => setShowEditModal(false)}
              >
                Close
              </Button>
              <Button variant="primary" onClick={handleUpdateProjection}>
                Save Changes
              </Button>
            </Modal.Footer>
          </Modal>
        )}
      </div>
      <div className="container mt-3">
        <Link to="/add-projection" className="btn btn-primary">
          Add new projection
        </Link>
      </div>
    </>
  );
};

export default ManageProjections;
