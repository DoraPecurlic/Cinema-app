import React from "react";
import { format } from "date-fns";

const ProjectionSelector = ({ projection, isSelected, onSelect }) => {
  const formattedDate = format(new Date(projection.date), "dd/MM/yyyy");

  const handleSelect = () => {
    onSelect(projection.id);
  };

  return (
    <button
      className={`list-group-item list-group-item-action ${
        isSelected ? "active" : ""
      }`}
      onClick={handleSelect}
    >
      {formattedDate} - {projection.time}
    </button>
  );
};

export default ProjectionSelector;
