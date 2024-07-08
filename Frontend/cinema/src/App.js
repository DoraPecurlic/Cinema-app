import React, { useState, useEffect } from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Home from "./pages/Home";
import Navbar from "./components/Navbar";
import Movie from "./pages/Movie";
import AddProjection from "./pages/AddProjection";
import ManageProjections from "./pages/ManageProjections";
import ProjectionSelection from "./pages/ProjectionSelection";
import SeatSelection from "./pages/SeatSelection";
import AdminDashboard from "./pages/AdminDashboard";
import AddMovie from "./pages/AddMovie";
import AddActor from "./pages/AddActor";
import ViewActors from "./pages/ViewActors";
import UpdateActor from "./pages/UpdateActor";
import Payment from "./pages/Payment";
import ViewMovies from "./pages/ViewMovies";
import UpdateMovie from "./pages/UpdateMovie";
import ProtectedRoutes from "./routes/ProtectedRoutes";
import AdminPayments from "./pages/AdminPayments";
import UserPayments from "./pages/UserPayments";

export const Context = React.createContext();
function App() {
  const [signedIn, setSignedIn] = useState(false);
  return (
    <Router>
      <Context.Provider value={[signedIn, setSignedIn]}>
        <Navbar />
      </Context.Provider>
      <Context.Provider value={[signedIn, setSignedIn]}>
        <Routes>
          <Route exact path="/" element={<Home />} />
          <Route exact path="/movie/:id" element={<Movie />} />

          <Route element={<ProtectedRoutes allowedRoles={["Admin"]} />}>
            <Route path="/admin" element={<AdminDashboard />} />
            <Route exact path="/add-projection" element={<AddProjection />} />
            <Route
              exact
              path="/manage-projections"
              element={<ManageProjections />}
            />
            <Route exact path="/add-movie" element={<AddMovie />} />
            <Route exact path="/add-actor" element={<AddActor />} />
            <Route exact path="/view-actors" element={<ViewActors />} />
            <Route exact path="/update-actor/:id" element={<UpdateActor />} />
            <Route exact path="/view-movies" element={<ViewMovies />} />
            <Route exact path="/update-movie/:id" element={<UpdateMovie />} />
            <Route exact path="/view-payments" element={<AdminPayments />} />
          </Route>

          <Route element={<ProtectedRoutes allowedRoles={["User"]} />}>
            <Route
              path="/select-projection/:movieId"
              element={<ProjectionSelection />}
            />
            <Route
              path="/select-seat/:projectionId"
              element={<SeatSelection />}
            />
            <Route path="/payment/:projectionId" element={<Payment />} />
            <Route path="/user-payments" element={<UserPayments />} />
          </Route>
        </Routes>
      </Context.Provider>
    </Router>
  );
}

export default App;
