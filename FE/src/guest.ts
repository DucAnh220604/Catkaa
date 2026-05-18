const showStep = (stepNumber: number) => {
  const steps = document.querySelectorAll<HTMLElement>(".step-content");
  steps.forEach((step, index) => {
    step.classList.toggle("active", index === stepNumber);
    step.style.display = index === stepNumber ? "block" : "none";
  });
};

const bindGuestFlow = () => {
  const buttons = [
    ["btn-checkin", 1],
    ["btn-upload-id", 2],
    ["btn-verify", 3],
    ["btn-paid", 4],
    ["btn-skip", 4],
    ["btn-save", 4],
  ] as const;

  buttons.forEach(([id, nextStep]) => {
    const button = document.getElementById(id);
    button?.addEventListener("click", () => showStep(nextStep));
  });

  showStep(0);
};

if (document.readyState === "loading") {
  document.addEventListener("DOMContentLoaded", bindGuestFlow);
} else {
  bindGuestFlow();
}
