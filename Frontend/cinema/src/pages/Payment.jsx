import React, { useState, useEffect } from "react";
import { useLocation, useHistory } from "react-router-dom";
import { jwtDecode } from "jwt-decode";
import axios from "axios";
import { useNavigate } from "react-router-dom";

const Payment = () => {
  const location = useLocation();
  const navigate = useNavigate();
  const { detailedSelectedSeats, projectionId } = location.state;
  const [paymentId, setPaymentId] = useState(null);
  const [ticketIds, setTicketIds] = useState([]);
  const [decodedUser, setDecodedUser] = useState({});
  const [cardName, setCardName] = useState("");
  const [cardNumber, setCardNumber] = useState("");
  const [expDate, setExpDate] = useState("");
  const [cvv, setCvv] = useState("");
  const [successMessage, setSuccessMessage] = useState("");
  let totalPrice = detailedSelectedSeats.length * 5;

  useEffect(() => {
    const token = localStorage.getItem("token");
    if (token) {
      const decoded = jwtDecode(token);
      setDecodedUser(decoded);
    }
  }, []);

  const handlePayment = async () => {
    if (!cardName || !cardNumber || !expDate || !cvv) {
      alert("Please fill in all card details");
      return;
    }

    try {
      const createPaymentData = {
        TotalPrice: totalPrice,
        UserId: decodedUser.UserId,
      };

      const responsePayment = await fetch("http://localhost:5058/payment", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(createPaymentData),
      });

      if (!responsePayment.ok) {
        throw new Error("Error creating payment");
      }

      const responseDataPayment = await responsePayment.json();
      const paymentId = responseDataPayment.paymentId;

      const ticketPromises = detailedSelectedSeats.map(async (seat) => {
        const createTicketData = {
          Price: 5.0,
          PaymentId: paymentId,
          ProjectionId: projectionId,
          UserId: decodedUser.UserId,
        };

        const responseTicket = await fetch("http://localhost:5058/ticket", {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify(createTicketData),
        });

        if (!responseTicket.ok) {
          throw new Error("Error creating ticket");
        }

        const responseDataTicket = await responseTicket.json();
        return responseDataTicket.ticketId;
      });

      const createdTicketIds = await Promise.all(ticketPromises);
      console.log("Created ticket IDs:", createdTicketIds);

      const reservedSeatsPromises = detailedSelectedSeats.map(
        async (seat, index) => {
          const createReservedSeatData = {
            TicketId: createdTicketIds[index],
            ProjectionId: projectionId,
            SeatId: seat.id,
            UserId: decodedUser.UserId,
          };

          const responseReservedSeat = await fetch(
            "http://localhost:5058/seat/reserved",
            {
              method: "POST",
              headers: {
                "Content-Type": "application/json",
              },
              body: JSON.stringify(createReservedSeatData),
            }
          );

          if (!responseReservedSeat.ok) {
            throw new Error("Error creating reserved seats");
          }
        }
      );

      await Promise.all(reservedSeatsPromises);

      const emailRequest = {
        UserEmail: decodedUser.Email,
        TicketIds: createdTicketIds,
        Seats: detailedSelectedSeats,
      };
      console.log(emailRequest);

      const responseEmail = await axios.post(
        "http://localhost:5058/ticket/send-email",
        emailRequest
      );

      setSuccessMessage(
        "Payment successful. Please check your email for ticket details."
      );
      setTimeout(() => {
        setSuccessMessage("");
        navigate("/user-payments");
      }, 3000);
    } catch (error) {
      console.error("Error:", error.message);
    }
  };

  return (
    <div className="container mt-4">
      {successMessage && (
        <div className="alert alert-success mt-3">{successMessage}</div>
      )}
      <div className="row">
        <div className="col-md-8 offset-md-2">
          <div className="card">
            <div className="card-body">
              <h2 className="card-title">Payment Details</h2>
              <div className="mb-3">
                <label className="form-label">Cardholder Name</label>
                <input
                  type="text"
                  className="form-control"
                  value={cardName}
                  onChange={(e) => setCardName(e.target.value)}
                />
              </div>
              <div className="mb-3">
                <label className="form-label">Card Number</label>
                <input
                  type="text"
                  className="form-control"
                  value={cardNumber}
                  onChange={(e) => setCardNumber(e.target.value)}
                />
              </div>
              <div className="mb-3">
                <label className="form-label">Expiry Date</label>
                <input
                  type="text"
                  className="form-control"
                  value={expDate}
                  onChange={(e) => setExpDate(e.target.value)}
                />
              </div>
              <div className="mb-3">
                <label className="form-label">CVV</label>
                <input
                  type="password"
                  className="form-control"
                  value={cvv}
                  onChange={(e) => setCvv(e.target.value)}
                />
              </div>
              <button className="btn btn-primary" onClick={handlePayment}>
                Pay
              </button>
              <hr />
              <h5 className="card-title">Selected Seats:</h5>
              <ul className="list-group mb-3">
                {detailedSelectedSeats.map((seat) => (
                  <li
                    key={seat.id}
                    className="list-group-item d-flex justify-content-between align-items-center"
                  >
                    <div>
                      <strong>Row:</strong> {seat.rowLetter}
                      <br />
                      <strong>Seat Number:</strong> {seat.seatNumber}
                    </div>
                  </li>
                ))}
              </ul>
              <p>
                <strong>Total Price:</strong> €{totalPrice}
              </p>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Payment;
