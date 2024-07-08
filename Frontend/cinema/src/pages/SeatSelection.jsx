import React, { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import SeatService from "../services/SeatService";

const SeatSelection = () => {
  const navigate = useNavigate();

  const { projectionId } = useParams();
  const [selectedSeats, setSelectedSeats] = useState([]);
  const [detailedSelectedSeats, setDetailedSelectedSeats] = useState([]);
  const [seats, setSeats] = useState([]);
  const [reservedSeats, setReservedSeats] = useState([]);
  const [hallNumber, setHallNumber] = useState();

  useEffect(() => {
    const fetchSeats = async () => {
      try {
        const response = await SeatService.getSeatsByProjection(projectionId);
        setSeats(response.data);
      } catch (error) {
        console.error("Error fetching seats:", error);
      }
    };

    const fetchReservedSeats = async () => {
      try {
        const response = await SeatService.getReservedSeatsByProjection(
          projectionId
        );
        setReservedSeats(
          response.data.map((seat) => `${seat.rowLetter}${seat.seatNumber}`)
        );
      } catch (error) {
        console.error("Error fetching reserved seats:", error);
      }
    };

    fetchSeats();
    fetchReservedSeats();
  }, [projectionId]);

  const handleSeatSelect = (seatId) => {
    const seat = seats.find((s) => `${s.rowLetter}${s.seatNumber}` === seatId);

    const isSelected = selectedSeats.includes(seatId);

    if (isSelected) {
      setSelectedSeats(selectedSeats.filter((selected) => selected !== seatId));
      setDetailedSelectedSeats(
        detailedSelectedSeats.filter((selected) => selected.id !== seat.id)
      );
    } else {
      setSelectedSeats([...selectedSeats, seatId]);
      setDetailedSelectedSeats([
        ...detailedSelectedSeats,
        {
          id: seat.id,
          rowLetter: seat.rowLetter,
          seatNumber: seat.seatNumber,
          hallNumber: seat.hallNumber,
        },
      ]);
    }
  };
  const handleConfirm = () => {
    if (selectedSeats.length > 0) {
      navigate(`/payment/${projectionId}}`, {
        state: { detailedSelectedSeats, projectionId },
      });
    } else {
      alert("Please select at least one seat.");
    }
  };

  const getMaxColumns = () => {
    let max = 0;
    seats.forEach((seat) => {
      if (seat.seatNumber > max) {
        max = seat.seatNumber;
      }
    });
    return max;
  };

  const rows = [...new Set(seats.map((seat) => seat.rowLetter))];
  const columns = getMaxColumns();

  return (
    <div className="container mt-4">
      <div className="row">
        <div className="col-md-7">
          <h2>Select a Seat</h2>
          <div className="d-flex justify-content-center mb-3">
            <div
              className="screen"
              style={{
                width: `calc(40px * ${columns} + 10px * ${columns - 1})`,
                height: "20px",
                backgroundColor: "#ccc",
                textAlign: "center",
                lineHeight: "20px",
                margin: "0 auto",
              }}
            >
              Screen
            </div>
          </div>
          <div
            className="seat-grid"
            style={{
              display: "grid",
              gridTemplateColumns: `repeat(${columns + 1}, 40px)`,
              gap: "10px",
            }}
          >
            <div></div>
            {[...Array(columns).keys()].map((column) => (
              <div
                key={column}
                style={{
                  width: "40px",
                  height: "40px",
                  display: "flex",
                  alignItems: "center",
                  justifyContent: "center",
                }}
              >
                {column + 1}
              </div>
            ))}
            {rows.map((row) => (
              <React.Fragment key={row}>
                <div
                  style={{
                    width: "40px",
                    height: "40px",
                    display: "flex",
                    alignItems: "center",
                    justifyContent: "center",
                  }}
                >
                  {row}
                </div>
                {[...Array(columns).keys()].map((column) => {
                  const seatNumber = column + 1;
                  const seatId = `${row}${seatNumber}`;
                  const isReserved = reservedSeats.includes(seatId);
                  const isSelected = selectedSeats.includes(seatId);

                  return (
                    <button
                      key={seatId}
                      className={`btn ${
                        isSelected ? "btn-success" : "btn-secondary"
                      }`}
                      onClick={() => handleSeatSelect(seatId)}
                      disabled={isReserved}
                      style={{
                        width: "40px",
                        height: "40px",
                        display: "flex",
                        alignItems: "center",
                        justifyContent: "center",
                      }}
                    >
                      {seatNumber}
                    </button>
                  );
                })}
              </React.Fragment>
            ))}
          </div>
        </div>
        {selectedSeats.length > 0 && (
          <div className="col-md-5">
            <div className="selected-seats">
              <h2>Selected Seats</h2>
              <ul className="list-group">
                {selectedSeats.map((seat) => (
                  <li key={seat} className="list-group-item">
                    Seat: {seat}, Row: {seat.charAt(0)}, Number:{" "}
                    {seat.substring(1)}, Price: €5
                  </li>
                ))}
              </ul>
              <div className="text-center mt-4">
                <button className="btn btn-primary" onClick={handleConfirm}>
                  Confirm Selection
                </button>
              </div>
            </div>
          </div>
        )}
      </div>
    </div>
  );
};

export default SeatSelection;
