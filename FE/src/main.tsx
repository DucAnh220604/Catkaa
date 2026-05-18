import React from 'react';
import ReactDOM from 'react-dom/client';
import { BrowserRouter } from 'react-router-dom';
import App from './App';

// Global Styles from the template (loaded from public/css via index.html)
// but we also import them here for better Vite integration if needed, 
// OR we rely on index.html. Given the user reported "square boxes", 
// let's ensure all CSS is properly loaded.

import './custom.css';

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <BrowserRouter>
      <App />
    </BrowserRouter>
  </React.StrictMode>
);
