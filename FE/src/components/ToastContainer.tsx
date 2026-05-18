import React from "react";
import { useNotification } from "./NotificationContext";
import { Toast } from "./Toast";

export const ToastContainer: React.FC = () => {
  const { notifications, removeNotification } = useNotification();

  return (
    <div
      style={{
        position: "fixed",
        top: "20px",
        right: "20px",
        zIndex: 9999,
        display: "flex",
        flexDirection: "column",
        gap: "12px",
        pointerEvents: "none",
      }}
    >
      <style>{`
        @keyframes slideIn {
          from {
            transform: translateX(400px);
            opacity: 0;
          }
          to {
            transform: translateX(0);
            opacity: 1;
          }
        }
        
        @keyframes slideOut {
          from {
            transform: translateX(0);
            opacity: 1;
          }
          to {
            transform: translateX(400px);
            opacity: 0;
          }
        }
      `}</style>
      {notifications.map((notif) => (
        <div
          key={notif.id}
          style={{
            pointerEvents: "auto",
          }}
        >
          <Toast notification={notif} onClose={removeNotification} />
        </div>
      ))}
    </div>
  );
};
