import React from "react";
import AppRouter from "./router/AppRouter";
import { NotificationProvider } from "./components/NotificationContext";
import { MessageProvider } from "./components/MessageContext";
import { ToastContainer } from "./components/ToastContainer";

const App: React.FC = () => {
  return (
    <NotificationProvider>
      <MessageProvider>
        <ToastContainer />
        <AppRouter />
      </MessageProvider>
    </NotificationProvider>
  );
};

export default App;
