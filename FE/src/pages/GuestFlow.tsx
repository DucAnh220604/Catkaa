import React, { useState, useEffect } from "react";
import { motion, AnimatePresence } from "framer-motion";
import { Link } from "react-router-dom";
import {
  ArrowLeft, ArrowRight, CheckCircle2,
  ScanLine, Camera, QrCode, KeyRound, Wifi,
  MessageSquare, Building2, ChevronLeft,
  Smartphone, Check, Phone, ShieldCheck, CreditCard
} from "lucide-react";

/* ──────────────────────────────────────────────
   Components cho từng bước bên trong điện thoại
────────────────────────────────────────────── */

const StepWelcome = ({ onNext }: any) => (
  <div className="phone-screen-content d-flex flex-column h-100 animate__animated animate__fadeIn">
    <div className="px-3 py-2 text-white d-flex align-items-center gap-2" style={{ background: "#1686cb" }}>
      <Building2 size={14} />
      <span className="fw-bold" style={{ fontSize: '11px' }}>Sunrise Homestay</span>
      <div className="ms-auto d-flex gap-2 opacity-50"><Phone size={12}/><MessageSquare size={12}/></div>
    </div>
    <div className="flex-grow-1 p-3" style={{ background: "#F0F4F8" }}>
      <div className="bg-white p-3 rounded-4 shadow-sm mb-3" style={{ fontSize: '12px', borderLeft: '3px solid #16309F' }}>
        Xin chào <strong>Nguyễn Văn An</strong>! 👋
      </div>
      <div className="bg-white p-3 rounded-4 shadow-sm mb-3" style={{ fontSize: '12px' }}>
        🛏️ <strong>Phòng 203</strong> đã sẵn sàng cho bạn. Vui lòng hoàn tất thủ tục nhận phòng nhé!
      </div>
      <div className="rounded-4 overflow-hidden shadow-sm mt-4">
        <div className="p-3 text-white" style={{ background: "linear-gradient(135deg, #16309F, #38BDF8)" }}>
          <div className="fw-bold" style={{ fontSize: '11px' }}>LÀM THỦ TỤC ONLINE</div>
          <div className="small opacity-75" style={{ fontSize: '9px' }}>Xác thực eKYC bảo mật</div>
        </div>
        <div className="bg-white p-2 d-flex justify-content-around border-top" style={{ fontSize: '9px', fontWeight: 'bold' }}>
          <span>✓ CCCD</span><span>✓ eKYC</span><span>✓ PIN</span>
        </div>
      </div>
    </div>
    <div className="p-3 bg-white border-top">
      <button onClick={onNext} className="btn w-100 rounded-pill text-white fw-bold py-2 shadow-sm" style={{ background: "#1686cb", fontSize: '13px' }}>Bắt đầu làm thủ tục</button>
    </div>
  </div>
);

const StepScan = ({ onNext }: any) => {
  const [phase, setPhase] = useState("scanning");
  useEffect(() => { const t = setTimeout(() => setPhase("done"), 2500); return () => clearTimeout(t); }, []);
  return (
    <div className="phone-screen-content bg-dark h-100 d-flex flex-column animate__animated animate__fadeIn">
      <div className="p-3 text-white text-center border-bottom border-secondary small fw-bold">BƯỚC 1: QUÉT CCCD</div>
      <div className="flex-grow-1 d-flex flex-column align-items-center justify-content-center p-4">
        <div className="w-100 aspect-video border border-2 border-dashed border-primary rounded-4 position-relative overflow-hidden bg-primary bg-opacity-10 d-flex align-items-center justify-content-center">
          <AnimatePresence>
            {phase === "scanning" && (
              <motion.div 
                animate={{ top: ["0%", "100%", "0%"] }} 
                transition={{ duration: 2, repeat: Infinity, ease: "linear" }} 
                className="position-absolute start-0 end-0 h-1 bg-info shadow-lg z-3" 
              />
            )}
          </AnimatePresence>
          <CreditCard size={48} className={`transition-all duration-500 ${phase === "done" ? "text-success" : "text-primary opacity-20"}`} />
        </div>
        <p className="mt-4 text-white text-center small fw-bold">
          {phase === "scanning" ? "Vui lòng giữ điện thoại ổn định..." : "Đã nhận diện thành công!"}
        </p>
        {phase === "done" && (
          <div className="bg-success bg-opacity-20 text-success p-2 rounded-3 small mt-2">✓ NGUYỄN VĂN AN - 079...</div>
        )}
      </div>
      <div className="p-3 bg-white">
        <button onClick={onNext} disabled={phase === "scanning"} className="btn w-100 rounded-pill text-white fw-bold py-2 shadow-sm" style={{ background: phase === "done" ? "#16309F" : "#ccc", fontSize: '13px' }}>Tiếp tục</button>
      </div>
    </div>
  );
};

const StepEKYC = ({ onNext }: any) => {
  const [phase, setPhase] = useState("scanning");
  useEffect(() => { const t = setTimeout(() => setPhase("done"), 3000); return () => clearTimeout(t); }, []);
  return (
    <div className="phone-screen-content bg-dark h-100 d-flex flex-column animate__animated animate__fadeIn">
      <div className="p-3 text-white text-center border-bottom border-secondary small fw-bold">BƯỚC 2: XÁC THỰC KHUÔN MẶT</div>
      <div className="flex-grow-1 d-flex flex-column align-items-center justify-content-center p-4">
        <div className={`rounded-circle border border-4 transition-all duration-700 ${phase === 'done' ? 'border-success' : 'border-primary'} d-flex align-items-center justify-content-center bg-secondary bg-opacity-10`} style={{ width: '180px', height: '180px' }}>
          {phase === 'scanning' ? (
            <motion.div animate={{ opacity: [0.3, 1, 0.3] }} transition={{ duration: 1.5, repeat: Infinity }}>
              <Camera size={48} className="text-info"/>
            </motion.div>
          ) : (
            <motion.div initial={{ scale: 0 }} animate={{ scale: 1 }}>
              <Check size={64} className="text-success"/>
            </motion.div>
          )}
        </div>
        <p className="mt-4 text-white fw-bold text-center small">{phase === 'scanning' ? "Đang so khớp với ảnh CCCD..." : "Xác thực eKYC thành công!"}</p>
      </div>
      <div className="p-3 bg-white">
        <button onClick={onNext} disabled={phase === "scanning"} className="btn w-100 rounded-pill text-white fw-bold py-2 shadow-sm" style={{ background: phase === "done" ? "#16309F" : "#ccc", fontSize: '13px' }}>Tiếp tục</button>
      </div>
    </div>
  );
};

const StepPayment = ({ onNext }: any) => {
  const [paid, setPaid] = useState(false);
  return (
    <div className="phone-screen-content bg-light h-100 d-flex flex-column animate__animated animate__fadeIn">
      <div className="p-3 bg-white text-center border-bottom small fw-bold">BƯỚC 3: THANH TOÁN PHÒNG</div>
      <div className="flex-grow-1 p-4">
        {!paid ? (
          <div className="bg-white rounded-4 p-4 shadow-sm text-center border">
            <p className="fw-bold text-dark small mb-3">VIETQR - NGÂN HÀNG BIDV</p>
            <div className="p-3 bg-light rounded-3 mb-4 border"><QrCode size={120} color="#16309F" className="mx-auto"/></div>
            <div className="d-flex justify-content-between mb-2 small"><span className="text-muted">Tổng cộng:</span><span className="fw-bold text-primary">500.000 ₫</span></div>
            <div className="d-flex justify-content-between small text-start"><span className="text-muted">Nội dung:</span><span className="fw-bold">CATKAA P203 AN</span></div>
          </div>
        ) : (
          <div className="h-100 d-flex flex-column align-items-center justify-content-center text-center">
            <div className="bg-success text-white rounded-circle p-3 mb-3 shadow-lg animate__animated animate__bounceIn"><Check size={32}/></div>
            <p className="fw-bold text-dark">Thanh toán hoàn tất!</p>
            <p className="small text-muted">Hệ thống đã nhận được tiền</p>
          </div>
        )}
      </div>
      {!paid && (
        <div className="p-3 bg-white border-top">
          <button onClick={() => { setPaid(true); setTimeout(onNext, 1500); }} className="btn w-100 rounded-pill text-white fw-bold py-2 shadow-sm" style={{ background: "#1686cb", fontSize: '13px' }}>Tôi đã chuyển khoản</button>
        </div>
      )}
    </div>
  );
};

const StepSuccess = () => (
  <div className="phone-screen-content bg-white h-100 animate__animated animate__fadeIn">
    <div className="p-4 text-center flex-grow-1 d-flex flex-column justify-content-center">
      <div className="bg-success text-white rounded-circle d-inline-flex p-3 mb-4 shadow-lg mx-auto animate__animated animate__tada">
        <Check size={40} />
      </div>
      <h4 className="fw-bold text-dark">Check-in thành công!</h4>
      <p className="text-muted small">Thông tin phòng của bạn tại Sunrise</p>
      
      <div className="bg-white rounded-4 p-4 text-start shadow-sm mt-4 border border-primary border-2">
        <div className="small text-muted fw-bold mb-1 uppercase tracking-wider" style={{ fontSize: '10px' }}>MÃ PIN MỞ CỬA PHÒNG 203</div>
        <div className="h1 fw-bold text-primary mb-3 text-center" style={{ letterSpacing: '8px' }}>4812</div>
        <hr className="my-3"/>
        <div className="d-flex gap-2 align-items-center mb-2">
          <Wifi size={16} className="text-success"/>
          <span className="small fw-bold">Sunrise_5G</span>
        </div>
        <div className="d-flex gap-2 align-items-center">
          <ShieldCheck size={16} className="text-primary"/>
          <span className="small">An tâm pháp lý với CATKAA</span>
        </div>
      </div>
    </div>
    <div className="p-3 border-top">
      <Link to="/" className="btn btn-outline-secondary w-100 rounded-pill fw-bold text-decoration-none small">Về trang chủ</Link>
    </div>
  </div>
);

// --- Page Main ---

export default function GuestFlow() {
  const [step, setStep] = useState(0);
  const STEPS_DATA = [
    { label: "Chào mừng", icon: MessageSquare },
    { label: "Quét CCCD", icon: ScanLine },
    { label: "eKYC Selfie", icon: Camera },
    { label: "Thanh toán", icon: QrCode },
    { label: "Nhận phòng", icon: KeyRound },
  ];

  const next = () => step < 4 && setStep(step + 1);
  const back = () => step > 0 && setStep(step - 1);

  return (
    <div className="demo-wrapper">
      <div className="auto-container">
        <div className="row align-items-center g-5">
          
          {/* Cột 1: Thông tin và điều hướng */}
          <div className="col-lg-5">
            <div className="sec-title mb-4">
              <div className="subtitle"><span className="dot"></span> CATKAA SECURITY</div>
              <h2 className="fw-900 text-dark" style={{ fontSize: '42px', lineHeight: '1.1' }}>Quy trình<br/><span className="text-catkaa">Check-in Số</span></h2>
            </div>
            <p className="text-muted mb-5">Hệ thống tự động hóa toàn bộ quy trình từ eKYC đến nhận mã PIN phòng, giúp tiết kiệm 90% thời gian vận hành.</p>
            
            <div className="d-flex flex-column gap-3 mb-5">
              {STEPS_DATA.map((s, i) => (
                <button key={i} onClick={() => setStep(i)} className={`btn d-flex align-items-center gap-3 p-3 rounded-4 border-0 text-start transition-all ${step === i ? 'bg-white shadow border border-primary' : 'opacity-50'}`}>
                  <div className={`w-10 h-10 rounded-3 d-flex align-items-center justify-content-center flex-shrink-0 ${step >= i ? 'bg-catkaa text-white' : 'bg-light text-muted'}`}>
                    {step > i ? <Check size={18}/> : <s.icon size={18}/>}
                  </div>
                  <div>
                    <p className={`mb-0 small fw-bold ${step === i ? 'text-dark' : 'text-muted'}`}>{s.label}</p>
                    <p className="mb-0 text-muted" style={{ fontSize: '10px' }}>BƯỚC {i+1}</p>
                  </div>
                </button>
              ))}
            </div>

            <div className="d-flex gap-3 justify-content-center mt-2">
              <button 
                onClick={back} 
                disabled={step === 0} 
                className="theme-btn btn-style-three btn-small flex-grow-1 d-flex align-items-center justify-content-center shadow-sm"
                style={{ opacity: step === 0 ? 0.5 : 1, maxWidth: '160px' }}
              >
                <span className="btn-title d-flex align-items-center">
                  <ArrowLeft size={16} className="me-2"/>Quay lại
                </span>
              </button>
              <button 
                onClick={next} 
                disabled={step === 4} 
                className="theme-btn btn-style-three btn-small flex-grow-1 d-flex align-items-center justify-content-center shadow-sm"
                style={{ opacity: step === 4 ? 0.5 : 1, maxWidth: '160px' }}
              >
                <span className="btn-title d-flex align-items-center">
                  Tiếp theo<ArrowRight size={16} className="ms-2"/>
                </span>
              </button>
            </div>
          </div>

          {/* Cột 2: Điện thoại Mockup */}
          <div className="col-lg-7 d-flex justify-content-center">
            <div className="phone-mockup">
              <div className="phone-notch-top"></div>
              <div className="h-100 bg-white overflow-hidden">
                <AnimatePresence mode="wait">
                  <motion.div 
                    key={step} 
                    initial={{ opacity: 0, x: 30 }} 
                    animate={{ opacity: 1, x: 0 }} 
                    exit={{ opacity: 0, x: -30 }} 
                    transition={{ duration: 0.4 }}
                    className="h-100"
                  >
                    {step === 0 && <StepWelcome onNext={next} />}
                    {step === 1 && <StepScan onNext={next} />}
                    {step === 2 && <StepEKYC onNext={next} />}
                    {step === 3 && <StepPayment onNext={next} />}
                    {step === 4 && <StepSuccess />}
                  </motion.div>
                </AnimatePresence>
              </div>
            </div>
          </div>

        </div>
      </div>
    </div>
  );
}
