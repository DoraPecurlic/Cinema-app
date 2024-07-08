import React from "react";
import { Link } from "react-router-dom";

const AdminDashboard = () => {
  return (
    <div className="container mt-4">
      <h1>Admin Dashboard</h1>
      <div className="row mt-4">
        <div className="col-md-6">
          <div className="card mb-3">
            <div className="card-body">
              <h5 className="card-title">Manage Movies</h5>
              <p className="card-text">Add, delete, and manage movies.</p>
              <Link to="/add-movie" className="btn btn-primary">
                Add Movie
              </Link>
              <Link to="/view-movies" className="btn btn-success ms-2">
                View Movies
              </Link>
            </div>
          </div>
        </div>
        <div className="col-md-6">
          <div className="card mb-3">
            <div className="card-body">
              <h5 className="card-title">Manage Projections</h5>
              <p className="card-text">Add, delete and manage projections.</p>
              <Link to="/add-projection" className="btn btn-primary">
                Add Projection
              </Link>
              <Link to="/manage-projections" className="btn btn-success ms-2">
                Manage Projections
              </Link>
            </div>
          </div>
        </div>
      </div>
      <div className="row mt-4">
        <div className="col-md-6">
          <div className="card mb-3">
            <div className="card-body">
              <h5 className="card-title">View payments</h5>
              <p className="card-text">View succesful payments.</p>
              <Link to="/view-payments" className="btn btn-primary">
                View Payments
              </Link>
            </div>
          </div>
        </div>
        <div className="col-md-6">
          <div className="card mb-3">
            <div className="card-body">
              <h5 className="card-title">Manage Actors</h5>
              <p className="card-text">Add, delete, and manage movie actors.</p>
              <Link to="/add-actor" className="btn btn-primary">
                Add Actor
              </Link>
              <Link to="/view-actors" className="btn btn-success ms-2">
                View Actors
              </Link>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default AdminDashboard;
