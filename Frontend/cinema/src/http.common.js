import axios from "axios";

export default axios.create({
  baseURL: "http://localhost:5058",
  headers: {
    "Content-type": "application/json",
  },
});
