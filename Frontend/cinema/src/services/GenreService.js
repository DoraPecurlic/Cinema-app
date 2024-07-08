import http from "../http.common";

const getAllGenres = () => {
  return http.get("/genre");
};

const GenreService = {
  getAllGenres,
};

export default GenreService;
