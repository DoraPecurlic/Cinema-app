import React, { useState, useEffect } from "react";
import { jwtDecode } from "jwt-decode";
import PaymentService from "../services/PaymentService";

const UserPayments = ({}) => {
  const [payments, setPayments] = useState([]);
  const [decodedUser, setDecodedUser] = useState({});

  const token = localStorage.getItem("token");
  const decoded = jwtDecode(token);

  useEffect(() => {
    const fetchPayments = async () => {
      try {
        const response = await PaymentService.getUserPayments(decoded.UserId);
        if (!response.status === 200) {
          throw new Error("Failed to fetch payments");
        }
        setPayments(response.data);
      } catch (error) {
        console.error("Error fetching payments:", error);
      }
    };

    fetchPayments();
  }, []);

  return (
    <div className="container mt-4">
      <h2 className="mb-4">Admin Payments</h2>
      <div className="table-responsive">
        <table className="table table-striped table-bordered">
          <thead className="thead-light">
            <tr>
              <th>ID</th>
              <th>Total Price</th>
              <th>Payment Date</th>
            </tr>
          </thead>
          <tbody>
            {payments.map((payment) => (
              <tr key={payment.id}>
                <td>{payment.id}</td>
                <td>${payment.totalPrice.toFixed(2)}</td>
                <td>{new Date(payment.paymentDate).toLocaleDateString()}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
};

export default UserPayments;
