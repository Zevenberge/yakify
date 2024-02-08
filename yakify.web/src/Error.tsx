import { useEffect } from "react";
import { useNavigate, useRouteError } from "react-router-dom";

export default function Error() {
  const error: any = useRouteError();
  const navigate = useNavigate();
  useEffect(() => {
    if (error && error.status === 404) {
      navigate("/");
    }
  });
  return (
    <>
      <h1>An error has occured.</h1>
      <p>Enjoy the JSON of the error:</p>
      <p className="code">{JSON.stringify(error)}</p>
      <button className="primary" onClick={() => navigate("/")}>
        Go back
      </button>
    </>
  );
}
