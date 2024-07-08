import http from "../http.common";

const addReview = (newReview) => {
  const token = localStorage.getItem("token");
  return http.post("/review", newReview, {
    headers: {
      Authorization: `Bearer ${token}`,
    },
  });
};

const getAllReviews = () => {
  return http.get("/review");
};

const getReviewById = (id) => {
  return http.get(`/review/movie/${id}`);
};

const ReviewService = {
  addReview,
  getAllReviews,
  getReviewById,
};

export default ReviewService;
