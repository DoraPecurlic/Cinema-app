import React, { useState, useEffect, useContext } from "react";
import { useNavigate } from "react-router-dom";
import MovieService from "../services/MovieService";
import GenreService from "../services/GenreService";
import LanguageService from "../services/LanguageService";
import { AuthenticationContext } from "../context/AuthenticationContextProvider";
import { jwtDecode } from "jwt-decode";

function Home() {
  const [movies, setMovies] = useState([]);
  const [allMovies, setAllMovies] = useState([]);
  const [searchTerm, setSearchTerm] = useState("");
  const [decodedUser, setDecodedUser] = useState({});
  const [filters, setFilters] = useState({
    genreId: "",
    languageId: "",
    sortBy: "Duration",
    sortOrder: "DESC",
    searchTerm: "",
    pageNumber: 1,
    pageSize: 3,
  });

  const [filter, setFilter] = useState({
    genreId: "",
    languageId: "",
    sortBy: "Duration",
    sortOrder: "DESC",
    searchTerm: "",
    pageNumber: 1,
    pageSize: 0,
  });

  const navigate = useNavigate();

  const [genres, setGenres] = useState([]);
  const [languages, setLanguages] = useState([]);
  {
    decodedUser.Role == "Admin" && navigate("/admin");
  }

  useEffect(() => {
    const token = localStorage.getItem("token");
    if (token) {
      try {
        const decodedToken = jwtDecode(token);
        setDecodedUser(decodedToken);
      } catch (error) {
        console.error("Invalid token specified:", error);
      }
    } else {
      console.error("No token found in localStorage.");
    }
  }, []);

  useEffect(() => {
    const fetchGenresAndLanguages = async () => {
      try {
        const genreResponse = await GenreService.getAllGenres();
        setGenres(genreResponse.data);

        const languageResponse = await LanguageService.getAllLanguages();
        setLanguages(languageResponse.data);
      } catch (error) {
        console.error("Error fetching genres and languages:", error);
      }
    };

    fetchGenresAndLanguages();
  }, []);

  useEffect(() => {
    fetchMovies();
  }, [filters]);

  useEffect(() => {
    fetchAllMovies();
  }, [filter]);

  const fetchMovies = () => {
    MovieService.getFilteredMovies(filters)
      .then((response) => {
        setMovies(response.data);
      })
      .catch((error) => {
        console.error("Error fetching movies:", error);
      });
  };
  const fetchAllMovies = () => {
    MovieService.getFilteredMovies(filter)
      .then((response) => {
        setAllMovies(response.data);
      })
      .catch((error) => {
        console.error("Error fetching all movies:", error);
      });
  };

  const handleSearchChange = (e) => {
    const value = e.target.value;
    setSearchTerm(value);
    setFilters({
      ...filters,
      searchTerm: value,
      pageNumber: 1,
    });

    setFilter({
      ...filters,
      searchTerm: value,
    });
  };

  const handleFilterChange = (e) => {
    const { name, value } = e.target;
    setFilters({
      ...filters,
      [name]: value,
      pageNumber: 1,
    });
    setFilter({
      ...filters,
      [name]: value,
      pageSize: 0,
    });
  };

  const handleMovieClick = (movieId) => {
    navigate(`/movie/${movieId}`);
  };

  const totalPages = Math.ceil(allMovies.length / filters.pageSize);

  const handlePageChange = (page) => {
    setFilters({
      ...filters,
      pageNumber: page,
    });
  };

  return (
    <div className="container mt-5">
      <h1>Movies</h1>
      <div className="row mb-4">
        <div className="col-md-3">
          <select
            className="form-control"
            name="genreId"
            value={filters.genreId}
            onChange={handleFilterChange}
          >
            <option value="">Genre</option>
            {genres.map((genre) => (
              <option key={genre.id} value={genre.id}>
                {genre.name}
              </option>
            ))}
          </select>
        </div>
        <div className="col-md-3">
          <select
            className="form-control"
            name="languageId"
            value={filters.languageId}
            onChange={handleFilterChange}
          >
            <option value="">Language</option>
            {languages.map((language) => (
              <option key={language.id} value={language.id}>
                {language.name}
              </option>
            ))}
          </select>
        </div>
      </div>
      <div className="row">
        {movies.length === 0 ? (
          <div className="col-12">
            <p>No movies found...</p>
          </div>
        ) : (
          movies.map((movie) => (
            <div className="col-md-4 mb-4" key={movie.movieId}>
              <div className="card">
                <img
                  src={movie.coverUrl}
                  className="card-img-top"
                  alt={movie.title}
                />
                <div className="card-body">
                  <h5 className="card-title">{movie.title}</h5>
                  <p className="card-text">{movie.description}</p>
                  <button
                    onClick={() => handleMovieClick(movie.movieId)}
                    className="btn btn-primary"
                  >
                    More →
                  </button>
                </div>
              </div>
            </div>
          ))
        )}
      </div>
      <div className="row mt-4">
        <div className="col">
          <nav>
            <ul className="pagination justify-content-center">
              {Array.from({ length: totalPages }, (_, index) => (
                <li
                  className={`page-item ${
                    index + 1 === filters.pageNumber ? "active" : ""
                  }`}
                  key={index}
                >
                  <button
                    className="page-link"
                    onClick={() => handlePageChange(index + 1)}
                  >
                    {index + 1}
                  </button>
                </li>
              ))}
            </ul>
          </nav>
        </div>
      </div>
    </div>
  );
}

export default Home;
