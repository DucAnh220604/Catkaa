import React, { useEffect } from 'react';

const Preloader: React.FC = () => {
  useEffect(() => {
    // Handle preloader disappearance
    const preloader = document.querySelector('.preloader');
    if (preloader) {
      setTimeout(() => {
        (preloader as HTMLElement).style.display = 'none';
      }, 500);
    }
  }, []);

  return <div className="preloader"></div>;
};

export default Preloader;
