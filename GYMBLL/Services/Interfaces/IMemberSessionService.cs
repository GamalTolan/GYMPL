using GYMBLL.ViewModels.MemberSessionViewModels;

namespace GYMBLL.Services.Interfaces;

public interface IMemberSessionService
{
    IEnumerable<BookingViewModel> GetBookingsForSession(int sessionId);
    IEnumerable<MemberSelectViewModel> GetAvailableMembers(int sessionId);
    bool CreateBooking(CreateBookingViewModel model);
    bool CancelBooking(int bookingId);
    bool MarkAttendance(int bookingId);
}
