import http from "../http.common";

const getSeatsByProjection = (projectionId) => {
  return http.get(`/seat/ByProjection/${projectionId}`);
};

const getReservedSeatsByProjection = (projectionId) => {
  return http.get(`/seat/ReservedByProjection/${projectionId}`);
};

const SeatService = {
  getSeatsByProjection,
  getReservedSeatsByProjection,
};

export default SeatService;
