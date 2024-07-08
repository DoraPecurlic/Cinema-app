import http from "../http.common";

const getAllProjections = () => {
  return http.get("/projection");
};

const getProjectionById = (id) => {
  return http.get(`/projection/${id}`);
};

const getProjectionByMovieId = (id) => {
  return http.get(`/projection/movie/${id}`);
};

const updateProjection = (id, projection) => {
  return http.put(`/projection/${id}`, projection);
};

const addProjection = (projection) => {
  return http.post("/projection", projection);
};

const deleteProjection = (id) => {
  return http.delete(`/projection/${id}`);
};

const ProjectionService = {
  getAllProjections,
  getProjectionById,
  getProjectionByMovieId,
  updateProjection,
  addProjection,
  deleteProjection,
};

export default ProjectionService;
