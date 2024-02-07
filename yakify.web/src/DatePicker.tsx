import { useState } from "react";
import { useDate } from "./DateContext";
import "./DatePicker.css";
import Calendar from "./icons/Calendar";
import { useNumberInput } from "./hooks/useInput";

export default function DatePicker() {
  const [date, setDate] = useDate();
  const [open, setOpen] = useState(false);
  const [input, setInput] = useNumberInput(date);

  function confirm() {
    setOpen(!open);
    setDate(input);
  }

  return (
    <div className="DatePicker">
      <Calendar />
      <button className="DatePicker-button" onClick={() => setOpen(!open)}>
        {date}
      </button>
      {open && (
        <div className="DatePicker-popup card">
          <span>Enter the day to time travel to...</span>
          <input
            className="DatePicker-input"
            type="number"
            value={input}
            min={0}
            onChange={setInput}
          ></input>
          <button className="secondary" onClick={confirm}>
            Confirm!
          </button>
        </div>
      )}
    </div>
  );
}
