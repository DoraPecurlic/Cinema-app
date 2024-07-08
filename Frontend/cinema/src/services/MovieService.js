import http from "../http.common";

const getFilteredMovies = (filters = {}) => {
  const {
    genreId,
    languageId,
    searchTerm,
    sortBy = "title",
    sortOrder = "ASC",
    pageNumber = 1,
    pageSize = 10,
  } = filters;

  let url = `/movie?sortBy=${sortBy}&sortOrder=${sortOrder}&pageNumber=${pageNumber}&pageSize=${pageSize}`;

  if (genreId) {
    url += `&genreId=${genreId}`;
  }
  if (languageId) {
    url += `&languageId=${languageId}`;
  }
  if (searchTerm) {
    url += `&searchTerm=${searchTerm}`;
  }

  return http.get(url);
};

const getMovieById = (id) => http.get(`/movie?movieId=${id}`);

const addMovie = (movieData) => http.post('/movie', movieData);

const updateMovie = (id, movieData) => http.put(`/movie/${id}`, movieData);

const deleteMovie = (id) => http.delete(`/movie/${id}`);

const MovieService = {
  getFilteredMovies,
  getMovieById,
  addMovie,
  updateMovie,
  deleteMovie,
};

export default MovieService;
