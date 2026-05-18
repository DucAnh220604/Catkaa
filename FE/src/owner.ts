type TableRow = [string, string, string, string?];

const waitingRows: TableRow[] = [
  ["Nguyễn Văn An", "Chờ xác minh", "Khu A"],
  ["Trần Thị Bình", "Chờ xác minh", "Khu B"],
  ["Lê Văn Cường", "Chờ xác minh", "Khu C"],
];

const ekycRows: TableRow[] = [
  ["Phạm Minh Tuấn", "Đã eKYC", "Khu A"],
  ["Hoàng Thu Trang", "Đã eKYC", "Khu B"],
  ["Vũ Hải Nam", "Đã eKYC", "Khu C"],
  ["Đặng Quỳnh Anh", "Đã eKYC", "Khu D"],
  ["Bùi Minh Khoa", "Đã eKYC", "Khu E"],
];

const legalRows: TableRow[] = [
  ["Nguyễn Văn An", "079123456789", "Hà Nội", "Có"],
  ["Trần Thị Bình", "079987654321", "Hải Phòng", "Có"],
  ["Lê Văn Cường", "079456789123", "Đà Nẵng", "Có"],
];

const renderRows = (tbodyId: string, rows: TableRow[]) => {
  const tbody = document.getElementById(tbodyId);
  if (!tbody) return;

  tbody.innerHTML = rows
    .map((row) => {
      const cells = row.map((cell) => `<td>${cell}</td>`).join("");
      return `<tr>${cells}</tr>`;
    })
    .join("");
};

const bindOwnerDashboard = () => {
  renderRows("table-waiting-body", waitingRows);
  renderRows("table-ekyc-body", ekycRows);
  renderRows("table-legal-body", legalRows);

  const overviewSection = document.getElementById("overview-section");
  const legalSection = document.getElementById("legal-section");
  const pageTitle = document.getElementById("page-title");

  const showOverview = () => {
    overviewSection?.classList.remove("d-none");
    legalSection?.classList.add("d-none");
    if (pageTitle) pageTitle.textContent = "Dashboard Chủ Nhà";
  };

  const showLegal = () => {
    overviewSection?.classList.add("d-none");
    legalSection?.classList.remove("d-none");
    if (pageTitle) pageTitle.textContent = "Báo cáo pháp lý";
  };

  document.getElementById("btn-report")?.addEventListener("click", showLegal);
  document.getElementById("btn-back")?.addEventListener("click", showOverview);
  document
    .getElementById("btn-export")
    ?.addEventListener("click", () => window.alert("Đã xuất file mô phỏng."));
  document
    .getElementById("btn-submit-report")
    ?.addEventListener("click", () => window.alert("Đã gửi báo cáo mô phỏng."));
};

if (document.readyState === "loading") {
  document.addEventListener("DOMContentLoaded", bindOwnerDashboard);
} else {
  bindOwnerDashboard();
}
