using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using CarWash.Interface;
using CarWash.Models;
using Microsoft.Extensions.Configuration;

namespace CarWashBookingSystem.Repositories
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _senderEmail;
        private readonly string _senderPassword;

        public EmailService(IConfiguration config)
        {
            _smtpServer = config["EmailSettings:SmtpServer"];
            _smtpPort = int.Parse(config["EmailSettings:SmtpPort"]);
            _senderEmail = config["EmailSettings:SenderEmail"];
            _senderPassword = config["EmailSettings:SenderPassword"];
        }

        public async Task SendBookingRequestAsync(string washerEmail, Bookings booking)
        {
            try
            {
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_senderEmail),
                    Subject = $"New Car Wash Request - Booking ID {booking.BookingId}",
                    Body = GenerateBookingEmail(booking),
                    IsBodyHtml = true
                };
                mailMessage.To.Add(washerEmail);

                using (var smtpClient = new SmtpClient(_smtpServer, _smtpPort))
                {
                    smtpClient.Credentials = new NetworkCredential(_senderEmail, _senderPassword);
                    smtpClient.EnableSsl = true;
                    await smtpClient.SendMailAsync(mailMessage);
                }

                Console.WriteLine($"Booking request email sent to {washerEmail}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send booking request email: {ex.Message}");
            }
        }

        public string GenerateBookingEmail(Bookings booking)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<h2>New Car Wash Booking Request</h2>");
            sb.AppendLine($"<p><strong>Booking ID:</strong> {booking.BookingId}</p>");
            sb.AppendLine($"<p><strong>Customer Name:</strong> {booking.Customer.Name}</p>");
            sb.AppendLine($"<p><strong>Phone Number:</strong> {booking.Customer.Phone}</p>");
            sb.AppendLine($"<p><strong>Car Name:</strong> {booking.Car.Brand}, {booking.Car.Model}</p>");
            sb.AppendLine($"<p><strong>Service Package:</strong> {booking.WashPackage.PackageName}</p>");
            sb.AppendLine($"<p><strong>Notes:</strong>{booking.Notes}");
            sb.AppendLine($"<p><strong>Service Package:</strong> {booking.TotalPrice}</p>");
            sb.AppendLine($"<p><strong>Scheduled Date:</strong> {booking.ServiceDate:dd-MMM-yyyy}</p>");
            sb.AppendLine($"<p><strong>Location:</strong> {booking.Address.Street}, {booking.Address.City}, {booking.Address.Pincode}</p>");
            
            sb.AppendLine("<p>Thank you!</p>");

            return sb.ToString();
        }
    }
}
