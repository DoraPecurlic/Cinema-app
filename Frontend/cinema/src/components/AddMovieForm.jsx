import React, { useState, useEffect } from "react";

const AddMovieForm = ({
  initialData = {},
  actors,
  genres,
  languages,
  onAddMovie,
  onAddNewActor,
  isUpdate = false,
}) => {
  const [title, setTitle] = useState(initialData.title || "");
  const [description, setDescription] = useState(initialData.description || "");
  const [duration, setDuration] = useState(initialData.duration || "");
  const [language, setLanguage] = useState(initialData.languageId || "");
  const [coverUrl, setCoverUrl] = useState(initialData.coverUrl || "");
  const [trailerUrl, setTrailerUrl] = useState(initialData.trailerUrl || "");
  const [genre, setGenre] = useState(initialData.genreId || "");
  const [newActor, setNewActor] = useState("");
  const [selectedActors, setSelectedActors] = useState(initialData.actorIds || []);

  useEffect(() => {
    if (initialData.actorIds) {
      setSelectedActors(initialData.actorIds);
    }
  }, [initialData.actorIds]);

  const handleSubmit = (event) => {
    event.preventDefault();

    if (
      !title ||
      !description ||
      !duration ||
      !language ||
      !coverUrl ||
      !trailerUrl ||
      !genre ||
      selectedActors.length === 0
    ) {
      alert("Please fill out all fields.");
      return;
    }

    const movieData = {
      title,
      description,
      duration: parseInt(duration, 10),
      languageId: language,
      coverUrl,
      trailerUrl,
      genreId: genre,
      actorIds: selectedActors,
    };

    console.log("Sending movie data:", movieData);

    onAddMovie(movieData);
  };

  const handleAddActor = async () => {
    if (newActor.trim()) {
      const newActorId = await onAddNewActor(newActor);
      if (newActorId) {
        setSelectedActors([...selectedActors, newActorId]);
        setNewActor("");
      }
    }
  };

  return (
    <div className="container mt-5">
      <h2>{isUpdate ? "Update Movie" : "Add Movie"}</h2>
      <form onSubmit={handleSubmit}>
        <div className="form-group">
          <label htmlFor="title">Title</label>
          <input
            type="text"
            className="form-control"
            id="title"
            value={title}
            onChange={(e) => setTitle(e.target.value)}
            required
          />
        </div>
        <div className="form-group">
          <label htmlFor="description">Description</label>
          <textarea
            className="form-control"
            id="description"
            value={description}
            onChange={(e) => setDescription(e.target.value)}
            required
          ></textarea>
        </div>
        <div className="form-group">
          <label htmlFor="duration">Duration (in minutes)</label>
          <input
            type="number"
            className="form-control"
            id="duration"
            value={duration}
            onChange={(e) => setDuration(e.target.value)}
            required
          />
        </div>
        <div className="form-group">
          <label htmlFor="language">Language</label>
          <select
            className="form-control"
            id="language"
            value={language}
            onChange={(e) => setLanguage(e.target.value)}
            required
          >
            <option value="">Select Language</option>
            {languages.map((language) => (
              <option key={language.id} value={language.id}>
                {language.name}
              </option>
            ))}
          </select>
        </div>
        <div className="form-group">
          <label htmlFor="genre">Genre</label>
          <select
            className="form-control"
            id="genre"
            value={genre}
            onChange={(e) => setGenre(e.target.value)}
            required
          >
            <option value="">Select Genre</option>
            {genres.map((genre) => (
              <option key={genre.id} value={genre.id}>
                {genre.name}
              </option>
            ))}
          </select>
        </div>
        <div className="form-group">
          <label htmlFor="coverUrl">Cover URL</label>
          <input
            type="url"
            className="form-control"
            id="coverUrl"
            value={coverUrl}
            onChange={(e) => setCoverUrl(e.target.value)}
            required
          />
        </div>
        <div className="form-group">
          <label htmlFor="trailerUrl">Trailer URL</label>
          <input
            type="url"
            className="form-control"
            id="trailerUrl"
            value={trailerUrl}
            onChange={(e) => setTrailerUrl(e.target.value)}
            required
          />
        </div>
        <div className="form-group">
          <label htmlFor="actors">Actors</label>
          <div className="form-check">
            {actors.map((actor) => (
              <div key={actor.id} className="form-check">
                <input
                  className="form-check-input"
                  type="checkbox"
                  value={actor.id}
                  checked={selectedActors.includes(actor.id)}
                  onChange={(e) => {
                    if (e.target.checked) {
                      setSelectedActors([...selectedActors, actor.id]);
                    } else {
                      setSelectedActors(
                        selectedActors.filter((id) => id !== actor.id)
                      );
                    }
                  }}
                />
                <label className="form-check-label">{actor.name}</label>
              </div>
            ))}
          </div>
          <small className="form-text text-muted">
            If an actor is not on the list, add them below.
          </small>
          <input
            type="text"
            className="form-control mt-2"
            id="new-actor"
            value={newActor}
            onChange={(e) => setNewActor(e.target.value)}
            placeholder="Add new actor"
          />
          <button
            type="button"
            className="btn btn-secondary mt-2"
            onClick={handleAddActor}
          >
            Add Actor
          </button>
        </div>
        <button type="submit" className="btn btn-primary">
          {isUpdate ? "Update Movie" : "Add Movie"}
        </button>
      </form>
    </div>
  );
};

export default AddMovieForm;
