import React, { useState, useEffect } from "react";
import MovieService from "../services/MovieService";
import { Link } from "react-router-dom";

const ViewMovies = () => {
  const [movies, setMovies] = useState([]);

  useEffect(() => {
    const fetchMovies = async () => {
      try {
        const filters = {
          sortBy: "Title", 
          sortOrder: "ASC",
          pageNumber: 1,
          pageSize: 10,
        };
        const response = await MovieService.getFilteredMovies(filters);
        setMovies(response.data);
      } catch (error) {
        console.error("Error fetching movies:", error.response || error.message || error);
      }
    };

    fetchMovies();
  }, []);

  const handleDeleteMovie = async (id) => {
    try {
      await MovieService.deleteMovie(id);
      setMovies(movies.filter((movie) => movie.movieId !== id));
    } catch (error) {
      console.error("Error deleting movie:", error);
    }
  };

  return (
    <div className="container mt-4">
      <h1>Movies</h1>
      <table className="table">
        <thead>
          <tr>
            <th>Title</th>
            <th>Description</th>
            <th>Duration</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {movies.map((movie) => (
            <tr key={movie.movieId}>
              <td>{movie.title}</td>
              <td>{movie.description}</td>
              <td>{movie.duration}</td>
              <td>
                <Link to={`/update-movie/${movie.movieId}`} className="btn btn-primary btn-sm me-2">
                  Update
                </Link>
                <button
                  className="btn btn-danger btn-sm"
                  onClick={() => handleDeleteMovie(movie.movieId)}
                >
                  Delete
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default ViewMovies;
