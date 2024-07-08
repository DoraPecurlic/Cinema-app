import React, { useState } from 'react';

const AddActorForm = ({ onAddNewActor }) => {
  const [newActorName, setNewActorName] = useState('');

  const handleAddActor = async () => {
    if (newActorName.trim()) {
      const newActorId = await onAddNewActor(newActorName);
      if (newActorId) {
        alert('Actor added successfully!');
        setNewActorName('');
      } else {
        alert('Failed to add actor.');
      }
    } else {
      alert('Please enter actor\'s name.');
    }
  };

  return (
    <div className="container mt-5">
      <h2>Add Actor</h2>
      <div className="form-group">
        <input
          type="text"
          className="form-control"
          placeholder="Enter actor's name"
          value={newActorName}
          onChange={(e) => setNewActorName(e.target.value)}
        />
      </div>
      <button
        type="button"
        className="btn btn-secondary"
        onClick={handleAddActor}
      >
        Add Actor
      </button>
    </div>
  );
};

export default AddActorForm;
