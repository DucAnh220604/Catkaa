import React, { useEffect, useState } from 'react';
import { Outlet, useLocation } from 'react-router-dom';
import Header from '../common/Header';
import Footer from '../common/Footer';
import Preloader from '../common/Preloader';

const MainLayout: React.FC = () => {
  const { pathname } = useLocation();
  const [isVisible, setIsVisible] = useState(false);

  const toggleVisibility = () => {
    if (window.scrollY > 300) {
      setIsVisible(true);
    } else {
      setIsVisible(false);
    }
  };

  // Hàm cuộn mượt mà tự tùy chỉnh để tránh bị giật hoặc nhảy thẳng
  const scrollToTop = () => {
    const duration = 600; // Thời gian cuộn (ms)
    const start = window.scrollY;
    const startTime = performance.now();

    const animateScroll = (currentTime: number) => {
      const timeElapsed = currentTime - startTime;
      const progress = Math.min(timeElapsed / duration, 1);
      
      // Công thức Easing (Cubic Out) để cuộn chậm dần về phía đầu trang
      const ease = 1 - Math.pow(1 - progress, 3);
      
      window.scrollTo(0, start * (1 - ease));

      if (timeElapsed < duration) {
        requestAnimationFrame(animateScroll);
      }
    };

    requestAnimationFrame(animateScroll);
  };

  useEffect(() => {
    window.addEventListener("scroll", toggleVisibility);
    return () => window.removeEventListener("scroll", toggleVisibility);
  }, []);

  useEffect(() => {
    window.scrollTo(0, 0);
  }, [pathname]);

  return (
    <div className="page-wrapper">
      <style>{`
        .scroll-to-top {
          position: fixed;
          bottom: 30px;
          right: 30px;
          width: 45px;
          height: 45px;
          background-color: #1686cb;
          color: #fff;
          display: flex;
          align-items: center;
          justify-content: center;
          border-radius: 50%;
          cursor: pointer;
          z-index: 999;
          transition: transform 0.3s ease, background-color 0.3s ease, opacity 0.3s ease;
          box-shadow: 0 4px 15px rgba(22, 134, 203, 0.3);
          border: none;
          outline: none;
        }
        .scroll-to-top:hover {
          background-color: #333;
          transform: translateY(-5px);
        }
        .scroll-to-top.fade-in {
          animation: fadeIn 0.3s ease-in-out;
        }
        @keyframes fadeIn {
          from { opacity: 0; transform: scale(0.8); }
          to { opacity: 1; transform: scale(1); }
        }
      `}</style>
      <Preloader />
      <Header />
      <main style={{ marginBottom: '0', paddingBottom: '0' }}>
        <Outlet />
      </main>
      <Footer />
      {/* Scroll to Top */}
      {isVisible && (
        <button 
          className="scroll-to-top shadow-lg fade-in" 
          onClick={scrollToTop}
          aria-label="Scroll to top"
        >
          <span className="fa fa-angle-up"></span>
        </button>
      )}
    </div>
  );
};

export default MainLayout;
