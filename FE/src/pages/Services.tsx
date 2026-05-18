import React from "react";
import { Link } from "react-router-dom";

const Services: React.FC = () => {
  const pricingData = [
    {
      title: "GÓI TRẢI NGHIỆM",
      subtitle: "(Free Trial)",
      price: "0 VNĐ",
      features: [
        { name: "Micro PMS", value: "Có" },
        {
          name: "PA72 Excel Export",
          value: "Có (Chuẩn 100%)",
          highlight: true,
        },
        { name: "Hạn mức OCR", value: "30 lượt/tháng" },
        { name: "Nhắc nhở chủ nhà", value: "Có" },
        { name: "Smart Lock", value: "Không", disabled: true },
      ],
      btnText: "Dùng Thử",
      popular: false,
    },
    {
      title: "Gói SỞ HỮU",
      subtitle: "(Basic - Mua đứt)",
      price: "Giá khóa + Phí setup",
      features: [
        { name: "Micro PMS", value: "Sở hữu vĩnh viễn" },
        { name: "PA72 Excel Export", value: "Có" },
        { name: "Hạn mức OCR", value: "Trả phí theo lượt dùng" },
        { name: "Nhắc nhở chủ nhà", value: "Có" },
        { name: "Smart Lock", value: "Mua đứt phần cứng" },
      ],
      btnText: "Đăng Ký",
      popular: true,
    },
    {
      title: "GÓI TOÀN DIỆN",
      subtitle: "(Pro - Thuê bao)",
      price: "899.000 VNĐ / tháng",
      features: [
        { name: "Micro PMS", value: "Thuê bao hàng tháng" },
        { name: "PA72 Excel Export", value: "Có" },
        { name: "Hạn mức OCR", value: "Không giới hạn*", highlight: true },
        { name: "Nhắc nhở chủ nhà", value: "Có" },
        { name: "Smart Lock", value: "Thuê phần cứng (HaaS)" },
      ],
      btnText: "Đăng Ký",
      popular: false,
    },
  ];

  return (
    <div className="services-page">
      {/* Page Title */}
      <section
        className="page-title"
        style={{
          backgroundImage:
            'linear-gradient(rgba(26, 30, 46, 0.7), rgba(26, 30, 46, 0.7)), url("https://images.unsplash.com/photo-1451187580459-43490279c0fa?q=80&w=2000&auto=format&fit=crop")',
          backgroundSize: "cover",
          backgroundPosition: "center",
          padding: "120px 0",
        }}
      >
        <div className="auto-container text-center">
          <h1 className="text-white fw-900 display-4 mb-2">Gói & Dịch Vụ</h1>
          <p
            className="text-white opacity-75 mx-auto"
            style={{ maxWidth: "600px" }}
          >
            Giải pháp công nghệ toàn diện giúp bạn vận hành nhàn hạ và chuyên
            nghiệp hơn.
          </p>
        </div>
      </section>

      {/* Intro Icons Section */}
      {/* <section className="services-intro py-5 bg-white border-bottom">
        <div className="auto-container">
          <div className="row g-4 text-center">
            <div className="col-md-4">
              <div className="p-4">
                <div
                  className="icon mb-3"
                  style={{ fontSize: "40px", color: "#1686cb" }}
                >
                  <i className="fa-solid fa-shield-halved"></i>
                </div>
                <h5 className="fw-bold">Bảo Mật Pháp Lý</h5>
                <p className="small text-muted">
                  Đảm bảo 100% quy trình eKYC và báo cáo lưu trú PA72.
                </p>
              </div>
            </div>
            <div className="col-md-4">
              <div className="p-4 border-start border-end">
                <div
                  className="icon mb-3"
                  style={{ fontSize: "40px", color: "#1686cb" }}
                >
                  <i className="fa-solid fa-microchip"></i>
                </div>
                <h5 className="fw-bold">Vận Hành Tự Động</h5>
                <p className="small text-muted">
                  Hệ thống Micro PMS và Smart Lock quản lý từ xa 24/7.
                </p>
              </div>
            </div>
            <div className="col-md-4">
              <div className="p-4">
                <div
                  className="icon mb-3"
                  style={{ fontSize: "40px", color: "#1686cb" }}
                >
                  <i className="fa-solid fa-chart-line"></i>
                </div>
                <h5 className="fw-bold">Tối Ưu Doanh Thu</h5>
                <p className="small text-muted">
                  Báo cáo minh bạch, quản lý booking tập trung, hiệu quả.
                </p>
              </div>
            </div>
          </div>
        </div>
      </section> */}

      {/* Pricing Cards Section */}
      <section className="pricing-section pt-40 pb-120 bg-light">
        <div className="auto-container">
          <div className="sec-title text-center mb-5">
            <div className="subtitle" style={{ color: "#1686cb" }}>
              <span
                className="dot"
                style={{ backgroundColor: "#1686cb" }}
              ></span>{" "}
              BẢNG GIÁ DỊCH VỤ
            </div>
            {/* <h2 className="title">Lựa Chọn Giải Pháp Tối Ưu Cho Homestay</h2> */}
          </div>

          <div className="row g-4 justify-content-center align-items-stretch">
            {pricingData.map((pkg, idx) => (
              <div
                key={idx}
                className="col-xl-4 col-md-6 wow fadeInUp"
                data-wow-delay={`${idx * 100}ms`}
              >
                <div
                  className={`pricing-card h-100 shadow-sm bg-white p-5 rounded-5 border-0 text-center transition-all position-relative overflow-hidden ${pkg.popular ? "border-2 scale-105" : ""}`}
                  style={
                    pkg.popular
                      ? {
                          borderColor: "#1686cb",
                          transform: "scale(1.05)",
                          zIndex: 2,
                        }
                      : {}
                  }
                >
                  {pkg.popular && (
                    <div
                      className="popular-badge"
                      style={{
                        backgroundColor: "#1686cb",
                        color: "#fff",
                        position: "absolute",
                        top: "20px",
                        right: "-35px",
                        padding: "5px 40px",
                        transform: "rotate(45deg)",
                        fontSize: "12px",
                        fontWeight: "bold",
                      }}
                    >
                      PHỔ BIẾN
                    </div>
                  )}

                  <div className="pricing-header mb-4">
                    <h4 className="fw-bold text-dark mb-1">{pkg.title}</h4>
                    <p className="text-muted small">{pkg.subtitle}</p>
                    <div className="price-box my-4">
                      <h3
                        className="fw-900 mb-0"
                        style={{
                          color: "#1686cb",
                          fontSize: pkg.price.length > 15 ? "24px" : "32px",
                        }}
                      >
                        {pkg.price}
                      </h3>
                    </div>
                  </div>

                  <div className="pricing-features-list text-start mb-5">
                    {pkg.features.map((feature, fIdx) => (
                      <div
                        key={fIdx}
                        className={`d-flex justify-content-between align-items-center py-2 border-bottom ${feature.disabled ? "opacity-50" : ""}`}
                        style={{ borderColor: "#f0f0f0" }}
                      >
                        <span className="text-muted small fw-bold">
                          {feature.name}
                        </span>
                        <span
                          className={`small ${feature.highlight ? "text-success fw-bold" : feature.disabled ? "text-muted" : "text-dark fw-bold"}`}
                        >
                          {feature.value}
                        </span>
                      </div>
                    ))}
                  </div>

                  <Link
                    to="/contact"
                    className="theme-btn w-100 rounded-pill py-3 d-flex align-items-center justify-content-center transition-all"
                    style={{
                      backgroundColor: pkg.popular ? "#1686cb" : "#f8f9fa",
                      color: pkg.popular ? "#fff" : "#333",
                      border: pkg.popular ? "none" : "1px solid #ddd",
                      fontWeight: "bold",
                      textDecoration: "none",
                    }}
                  >
                    <span className="btn-title">{pkg.btnText}</span>
                  </Link>
                </div>
              </div>
            ))}
          </div>

          <div className="text-center mt-5">
            <p className="text-muted small">
              * Áp dụng chính sách sử dụng hợp lý (Fair Usage Policy) cho lượt
              OCR không giới hạn.
            </p>
          </div>
        </div>
      </section>

      <style>{`
        .pricing-card { transition: all 0.4s ease; }
        .pricing-card:hover { transform: translateY(-10px); box-shadow: 0 20px 40px rgba(22, 134, 203, 0.15) !important; }
        .scale-105 { transform: scale(1.05); }
        .fw-900 { font-weight: 900; }
      `}</style>
    </div>
  );
};

export default Services;
