import http from '../http.common';

const getAvailableHalls = (date, time, movieId) => {
  return http.get(`/hall/halls?date=${date}&time=${time}&movieId=${movieId}`);
};

const getHallById = (id) => {
  return http.get(`/hall/${id}`);
};

const HallService = {
  getAvailableHalls,
  getHallById,
};

export default HallService;
