import React, { useState, useEffect, useContext } from "react";
import { useParams, useNavigate } from "react-router-dom";
import MovieService from "../services/MovieService";
import ReviewService from "../services/ReviewService";
import "../App.css";
import StarDisplay from "../components/StarDisplay";
import { jwtDecode } from "jwt-decode";
import { Context } from "../App";

const Movie = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const [signedIn, setSignedIn] = useContext(Context);
  const [movie, setMovie] = useState(null);
  const [rating, setRating] = useState(0);
  const [hover, setHover] = useState(0);
  const [reviews, setReviews] = useState([]);
  const [decodedUser, setDecodedUser] = useState({});
  const [newReview, setNewReview] = useState({
    description: "",
    rating: 0,
    userId: "",
    movieId: id,
    CreatedByUserId: "",
    UpdatedByUserId: "",
  });

  {
    decodedUser.Role == "Admin" && navigate("/admin");
  }

  useEffect(() => {
    const token = localStorage.getItem("token");

    if (token) {
      const decoded = jwtDecode(token);
      setDecodedUser(decoded);

      setNewReview((prev) => ({
        ...prev,
        userId: decoded.UserId,
        CreatedByUserId: decoded.UserId,
        UpdatedByUserId: decoded.UserId,
      }));
    }
  }, [signedIn]);

  useEffect(() => {
    const fetchMovie = async () => {
      try {
        const response = await MovieService.getMovieById(id);
        setMovie(response.data);
      } catch (error) {
        console.error("Error fetching movie:", error);
      }
    };

    fetchMovie();
  }, [id]);

  useEffect(() => {
    const fetchReviews = async () => {
      try {
        const response = await ReviewService.getReviewById(id);
        setReviews(response.data);
      } catch (error) {
        console.error("Error fetching reviews:", error);
      }
    };
    fetchReviews();
  }, [id]);

  const handleReviewChange = (e) => {
    const { name, value } = e.target;
    setNewReview({
      ...newReview,
      [name]: value,
    });
  };

  const handleRatingChange = (newRating) => {
    setNewReview({
      ...newReview,
      rating: newRating,
    });
  };

  const submitReview = async () => {
    try {
      const response = await ReviewService.addReview(newReview);
      const addedReview = response.data;
      setReviews([...reviews, addedReview]);
      setNewReview({
        description: "",
        rating: 0,
        userId: newReview.UserId,
        movieId: id,
        CreatedByUserId: newReview.UserId,
        UpdatedByUserId: newReview.UserId,
      });
    } catch (error) {
      console.error("Error submitting review:", error);
    }
  };

  const handleBuyTicket = () => {
    navigate(`/select-projection/${id}`);
  };

  const calculateAverageRating = () => {
    if (reviews.length === 0) {
      return 0;
    }
    const totalRating = reviews.reduce((acc, curr) => acc + curr.rating, 0);
    return (totalRating / reviews.length).toFixed(1);
  };

  if (!movie) {
    return <div>Loading...</div>;
  }

  return (
    <div className="container mt-4">
      <h1>{signedIn}</h1>
      <h1>{movie[0].title}</h1>
      <div className="row">
        <div className="col-md-8">
          <div className="card">
            <div className="card-body">
              <h5 className="card-title">{movie[0].title}</h5>
              <p className="card-text">{movie[0].description}</p>
              <p className="card-text">
                <strong>Genre:</strong> {movie[0].genre}
              </p>
              <p className="card-text">
                <strong>Language:</strong> {movie[0].language}
              </p>
              <p className="card-text">
                <strong>Duration:</strong> {movie[0].duration} min
              </p>
              <p className="card-text">
                <strong>Actors:</strong> {movie[0].actorNames.join(", ")}
              </p>
              <p className="card-text">
                <strong>Rating:</strong> {calculateAverageRating()}
              </p>
              {signedIn && (
                <button
                  className="btn btn-success mt-3"
                  onClick={handleBuyTicket}
                >
                  Buy Ticket
                </button>
              )}
            </div>
          </div>
        </div>
        <div className="col-md-4">
          <img
            src={movie[0].coverUrl}
            className="img-fluid"
            alt={movie[0].title}
          />
        </div>
      </div>
      {signedIn ? (
        <div className="mt-4">
          <h2>Add a Review</h2>
          <form
            className="form-group"
            onSubmit={(e) => {
              e.preventDefault();
              submitReview();
            }}
          >
            <div className="form-group">
              <label htmlFor="comment">Comment:</label>
              <textarea
                className="form-control"
                id="comment"
                name="description"
                value={newReview.description}
                onChange={handleReviewChange}
              ></textarea>
            </div>

            <label>Rating:</label>
            <div className="star-rating">
              {[...Array(5)].map((star, index) => {
                index += 1;
                return (
                  <button
                    type="button"
                    key={index}
                    className={
                      index <= (hover || newReview.rating) ? "on" : "off"
                    }
                    onClick={() => handleRatingChange(index)}
                    onMouseEnter={() => setHover(index)}
                    onMouseLeave={() => setHover(newReview.rating)}
                  >
                    <span className="star">&#9733;</span>
                  </button>
                );
              })}
            </div>
            <button type="submit" className="btn btn-primary mt-2">
              Submit Review
            </button>
          </form>
        </div>
      ) : (
        <div className="mt-4">
          <h2>Login to Add a Review</h2>
          <p>You need to login to add a review.</p>
        </div>
      )}
      <div className="mt-4">
        <h2>Reviews</h2>
        {reviews.map((review) => (
          <div className="card mb-2" key={review.id}>
            <div className="card-body">
              <h5 className="card-title">{review.firstName}</h5>
              <p className="card-text">{review.description}</p>
              <p className="card-text">
                <strong>Rating:</strong> <StarDisplay rating={review.rating} />
              </p>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default Movie;
