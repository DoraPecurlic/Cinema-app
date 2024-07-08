import http from "../http.common";

const getAllActors = () => {
  return http.get("/actor");
};

const getActorById = (id) => {
  return http.get(`/actor/${id}`);
};

const addActor = (actor) => {
  return http.post("/actor", actor);
};

const updateActor = (id, actorData) => {
  return http.put(`/actor/${id}`, actorData);
};

const deleteActor = (actorId) => {
  return http.delete(`/actor/${actorId}`);
};

const ActorService = {
  getAllActors,
  getActorById,
  addActor,
  updateActor,
  deleteActor,
};

export default ActorService;
