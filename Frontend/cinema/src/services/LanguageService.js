import http from "../http.common";

const getAllLanguages = () => {
  return http.get("/language");
};

const LanguageService = {
  getAllLanguages,
};

export default LanguageService;
