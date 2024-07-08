import React, { useState } from "react";
import "../App.css";

const seats = [
  ["A1", "A2", "A3", "A4", "A5"],
  ["B1", "B2", "B3", "B4", "B5"],
  ["C1", "C2", "C3", "C4", "C5"],
  ["D1", "D2", "D3", "D4", "D5"],
];

const SeatSelector = ({ selectedSeat, onSeatSelect }) => {
  return (
    <div className="seat-selector">
      <h2>Select a Seat</h2>
      <div className="seats">
        {seats.map((row, rowIndex) => (
          <div key={rowIndex} className="seat-row">
            {row.map((seat) => (
              <button
                key={seat}
                className={`seat ${selectedSeat === seat ? "selected" : ""}`}
                onClick={() => onSeatSelect(seat)}
              >
                {seat}
              </button>
            ))}
          </div>
        ))}
      </div>
    </div>
  );
};

export default SeatSelector;
