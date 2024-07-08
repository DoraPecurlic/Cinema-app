import React, { useState, useEffect } from "react";
import PaymentService from "../services/PaymentService";

const AdminPayments = () => {
  const [payments, setPayments] = useState([]);

  useEffect(() => {
    const fetchPayments = async () => {
      try {
        const response = await PaymentService.getAllPayments();
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
      <table className="table table-striped">
        <thead>
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
              <td>{payment.totalPrice}</td>
              <td>{new Date(payment.paymentDate).toLocaleDateString()}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default AdminPayments;
