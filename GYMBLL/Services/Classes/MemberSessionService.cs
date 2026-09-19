using GYMBLL.Services.Interfaces;
using GYMBLL.ViewModels.MemberSessionViewModels;
using GYMDAL.Entities;
using GYMDAL.Repositories.Interfaces;

namespace GYMBLL.Services.Classes;

public class MemberSessionService(IUnitOfWork unitOfWork) : IMemberSessionService
{
    public bool CancelBooking(int bookingId)
    {

        var bookingRequist = unitOfWork.GetRepository<Booking>().GetById(bookingId);
        if (bookingRequist is null)
            return false;

        var session = unitOfWork.GetRepository<Session>().GetById(bookingRequist.SessionId);
        if (session is not null && session.StartDate <= DateTime.Now)
            return false;

        unitOfWork.GetRepository<Booking>().Delete(bookingRequist);
        return unitOfWork.SaveChanges() > 0;
    }

    public bool CreateBooking(CreateBookingViewModel model)
    {
        try
        {
            var session = unitOfWork.GetRepository<Session>().GetById(model.SessionId);
            if (session is null)
                return false;
            if (session.EndDate <= DateTime.Now || session.StartDate <= DateTime.Now)
                return false;

            var existingBooking = unitOfWork.GetRepository<Booking>()
                .GetAll(b => b.SessionId == model.SessionId && b.MemberId == model.MemberId);

            if (existingBooking.Any())
                return false;

            var bookedSlots = unitOfWork.GetRepository<Booking>()
                   .GetAll(b => b.SessionId == model.SessionId)
                   .Count();

            if (bookedSlots >= session.Capacity)
                return false;

            var booking = new Booking
            {
                MemberId = model.MemberId,
                SessionId = model.SessionId,
                CreatedAt = DateTime.Now,

            };
            unitOfWork.GetRepository<Booking>().Add(booking);
            return unitOfWork.SaveChanges() > 0;

        }
        catch (Exception)
        {

            return false;
        }
    }

    public IEnumerable<MemberSelectViewModel> GetAvailableMembers(int sessionId)
    {
        var members = unitOfWork.GetRepository<Member>().GetAll();
        if (members is null)
            return [];
        var bookedMemberIds = unitOfWork.GetRepository<Booking>()
            .GetAll(m => m.SessionId == sessionId).Select(m => m.MemberId);
        var availableMembers = members
               .Where(m => !bookedMemberIds.Contains(m.Id))
               .Select(m => new MemberSelectViewModel
               {
                   Id = m.Id,
                   Name = m.Name
               })
               .ToList();

        return availableMembers;
    }

    public IEnumerable<BookingViewModel> GetBookingsForSession(int sessionId)
    {
        var bookings = unitOfWork.GetRepository<Booking>().GetAll(b => b.SessionId == sessionId);
        if (bookings is null)
            return [];
        var result = bookings.Select(b =>
        {
            var member = unitOfWork.GetRepository<Member>().GetById(b.MemberId);
            return new BookingViewModel
            {
                Id = b.Id,
                BookingDate = b.CreatedAt,
                MemberId = b.MemberId,
                MemberName = member.Name,
                MemberPhoto = member?.Photo,
                MemberPhone = member.PhoneNumber,
                MemberEmail = member.Email,
                SessionId = b.SessionId,
                IsAttended = b.IsAttended,
            };
        }).ToList();
        return result;
    }

   public bool MarkAttendance(int bookingId)
   {

        var booking = unitOfWork.GetRepository<Booking>().GetById(bookingId);
        if (booking is null)
            return false;
        var session = unitOfWork.GetRepository<Session>().GetById(booking.SessionId);
        if (session is null)
            return false;
        if (session.StartDate > DateTime.Now || session.EndDate < DateTime.Now)
            return false;

        booking.IsAttended = true;
        booking.UpdatedAt = DateTime.Now;

        unitOfWork.GetRepository<Booking>().Update(booking);
        return unitOfWork.SaveChanges() > 0;

    }

}
