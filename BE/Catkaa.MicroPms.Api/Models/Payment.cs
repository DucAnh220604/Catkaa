using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Catkaa.MicroPms.Api.Models
{
    public enum PaymentType
    {
        RoomBooking,
        PlanSubscription
    }

    public class Payment
    {
        [Key]
        public int Id { get; set; }

        public PaymentType Type { get; set; } = PaymentType.RoomBooking;

        public int? BookingId { get; set; }

        [ForeignKey(nameof(BookingId))]
        public Booking? Booking { get; set; }

        public int? PricingPlanId { get; set; }

        [ForeignKey(nameof(PricingPlanId))]
        public PricingPlan? PricingPlan { get; set; }

        public int? UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }

        [MaxLength(255)]
        public string TransactionId { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [MaxLength(50)]
        public string Status { get; set; } = "Pending";

        public DateTime PaymentDate { get; set; }

        [MaxLength(50)]
        public string PaymentMethod { get; set; } = "VNPay";
    }
}
