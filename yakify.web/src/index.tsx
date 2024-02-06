import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import reportWebVitals from './reportWebVitals';
import { RouterProvider } from 'react-router-dom';
import router from './router';
import { DateContextProvider } from './DateContext';

const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement
);
const appRouter = router;
root.render(
  <React.StrictMode>
    <div className='app'>
      <DateContextProvider value={1}>
        <RouterProvider router={appRouter} />
      </DateContextProvider>
    </div>
  </React.StrictMode>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
