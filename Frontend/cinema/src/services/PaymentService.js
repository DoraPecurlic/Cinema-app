import http from "../http.common";

const getAllPayments = () => {
  return http.get("/payment");
};

const getUserPayments = (id) => {
  return http.get(`/payment/${id}`);
};

const PaymentService = {
  getAllPayments,
  getUserPayments,
};

export default PaymentService;
