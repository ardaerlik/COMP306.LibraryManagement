using System;
using COMP306.LibraryManagement.BUS.Model;
using COMP306.LibraryManagement.COM.Enum;
using COMP306.LibraryManagement.COM.Model;
using COMP306.LibraryManagement.DAL.Context;
using COMP306.LibraryManagement.DAL.Entity;

namespace COMP306.LibraryManagement.BUS.Service
{
    /// <summary>
    /// 
    /// </summary>
	public class ReservationService : IReservationService
    {
        private readonly ApplicationContext _context;
        private readonly IUserService _userService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userService"></param>
        public ReservationService(
            ApplicationContext context,
            IUserService userService)
		{
            _context = context;
            _userService = userService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
		public async Task<ResponseModel> GetAsync(ReservationListModel model)
        {
            try
            {
                var isAuthorizedUser = await _userService.IsAuthorized(
                    model.email,
                    (int)SectionType.Room_Table_Reservations,
                    (int)ActionType.View);

                if (!isAuthorizedUser)
                {
                    return new()
                    {
                        HasError = true,
                        ExceptionMessage = $"{model.email} is not authorized to do this action."
                    };
                }

                var resources = (from location in _context.TftLocations
                                 join type in _context.TluLocationtypes on location.LocationTypeId equals type.Id
                                 where Convert.ToBoolean(type.CanBeReserved) == true
                                 group location by location.LocationTypeId into tmp
                                 select new CalendarResource
                                 {
                                     name = (from type1 in _context.TluLocationtypes where type1.Id == tmp.Key select type1.Name).First(),
                                     id = (from type1 in _context.TluLocationtypes where type1.Id == tmp.Key select type1.Name).First(),
                                     expanded = true,
                                     children = (from child in tmp
                                                 select new Location
                                                 {
                                                     name = child.Name,
                                                     id = child.Id.ToString()
                                                 }).ToList()
                                 }).ToList();

                var events = (from reservation in _context.TftLocationreservations
                              join location in _context.TftLocations on reservation.LocationId equals location.Id
                              join locationtype in _context.TluLocationtypes on location.LocationTypeId equals locationtype.Id
                              join user in _context.AppUsers on reservation.UserId equals user.Id
                              join status in _context.TluReservationstatuses on reservation.ReservationStatus equals status.Id
                              where Convert.ToBoolean(locationtype.CanBeReserved) == true
                                && (status.Id == (int)ReservationStatusType.WaitingApproval || status.Id == (int)ReservationStatusType.Approved)
                                && reservation.ReservationStartDate >= DateTime.Now.Date
                              select new Event
                              {
                                  start = reservation.ReservationStartDate,
                                  end = reservation.ReservationEndDate,
                                  id = reservation.Id.ToString(),
                                  resource = location.Id.ToString(),
                                  text = $"{location.Name} : {user.Name.ToUpper()} {user.Surname.ToUpper()}",
                                  bubbleHtml = $"{location.Name} : {user.Name.ToUpper()} {user.Surname.ToUpper()} : {status.StatusName}"
                              }).ToList();

                var data = new ResponseModel
                {
                    Data = new CalendarModel
                    {
                        resources = resources,
                        events = events
                    },
                    HasError = false
                };

                return data;
            }
            catch (Exception ex)
            {
                return new()
                {
                    HasError = true,
                    ExceptionMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResponseModel> AddAsync(ReservationAddModel model)
        {
            try
            {
                var isAuthorizedUser = await _userService.IsAuthorized(
                    model.email,
                    (int)SectionType.Room_Table_Reservations,
                    (int)ActionType.Add);

                if (!isAuthorizedUser)
                {
                    return new()
                    {
                        HasError = true,
                        ExceptionMessage = $"{model.email} is not authorized to do this action."
                    };
                }

                var canMakeReservation = !(from reservation in _context.TftLocationreservations
                                           join location in _context.TftLocations on reservation.LocationId equals location.Id
                                           join locationType in _context.TluLocationtypes on location.LocationTypeId equals locationType.Id
                                           join status in _context.TluReservationstatuses on reservation.ReservationStatus equals status.Id
                                           where Convert.ToBoolean(locationType.CanBeReserved) == true
                                            && location.Id == Convert.ToInt32(model.resource)
                                            && (status.Id != (int)ReservationStatusType.Rejected && status.Id != (int)ReservationStatusType.Deleted)
                                            && (
                                                (model.end > reservation.ReservationStartDate && model.start < reservation.ReservationStartDate)
                                             || (model.start < reservation.ReservationEndDate && model.end > reservation.ReservationEndDate)
                                             || (model.start >= reservation.ReservationStartDate && model.end <= reservation.ReservationEndDate)
                                            )
                                           select reservation).Any();

                if (!canMakeReservation)
                {
                    return new()
                    {
                        HasError = true,
                        ExceptionMessage = $"{model.resource} : {model.start} : {model.end}, is not valid a reservation."
                    };
                }

                var newReservation = new TftLocationreservation()
                {
                    LocationId = Convert.ToInt32(model.resource),
                    UserId = (from user in _context.AppUsers where user.Email == model.email select user.Id).FirstOrDefault(),
                    ReservationStartDate = model.start,
                    ReservationEndDate = model.end,
                    ReservationStatus = 2,
                    CreatedDate = DateTime.Now
                };

                await _context.TftLocationreservations.AddAsync(newReservation);
                await _context.SaveChangesAsync();

                var responseData = (from reservation in _context.TftLocationreservations
                                    join location in _context.TftLocations on reservation.LocationId equals location.Id
                                    join locationtype in _context.TluLocationtypes on location.LocationTypeId equals locationtype.Id
                                    join user in _context.AppUsers on reservation.UserId equals user.Id
                                    join status in _context.TluReservationstatuses on reservation.ReservationStatus equals status.Id
                                    where reservation.Id == newReservation.Id
                                    select new Event
                                    {
                                        start = reservation.ReservationStartDate,
                                        end = reservation.ReservationEndDate,
                                        id = reservation.Id.ToString(),
                                        resource = location.Id.ToString(),
                                        text = $"{location.Name} : {user.Name.ToUpper()} {user.Surname.ToUpper()}",
                                        bubbleHtml = $"{location.Name} : {user.Name.ToUpper()} {user.Surname.ToUpper()} : {status.StatusName}"
                                    }).FirstOrDefault();

                return new()
                {
                    Data = responseData,
                    HasError = false
                };
            }
            catch (Exception ex)
            {
                return new()
                {
                    HasError = true,
                    ExceptionMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResponseModel> UpdateAsync(ReservationUpdateModel model)
        {
            try
            {
                var isAuthorizedUser = await _userService.IsAuthorized(
                    model.email,
                    (int)SectionType.Room_Table_Reservations,
                    (int)ActionType.Update);

                if (!isAuthorizedUser)
                {
                    return new()
                    {
                        HasError = true,
                        ExceptionMessage = $"{model.email} is not authorized to do this action."
                    };
                }

                model.resource = string.IsNullOrEmpty(model.resource)
                    ? (from reservation in _context.TftLocationreservations
                       where reservation.Id == Convert.ToInt32(model.reservationId)
                       select reservation.LocationId.ToString()).First()
                    : model.resource;

                var canUpdateReservation = !(from reservation in _context.TftLocationreservations
                                             join location in _context.TftLocations on reservation.LocationId equals location.Id
                                             join locationType in _context.TluLocationtypes on location.LocationTypeId equals locationType.Id
                                             join status in _context.TluReservationstatuses on reservation.ReservationStatus equals status.Id
                                             where Convert.ToBoolean(locationType.CanBeReserved) == true
                                              && location.Id == Convert.ToInt32(model.resource)
                                              && (status.Id != (int)ReservationStatusType.Rejected && status.Id != (int)ReservationStatusType.Deleted)
                                              && (
                                                    (model.end > reservation.ReservationStartDate && model.start < reservation.ReservationStartDate)
                                                 || (model.start < reservation.ReservationEndDate && model.end > reservation.ReservationEndDate)
                                                 || (model.start >= reservation.ReservationStartDate && model.end <= reservation.ReservationEndDate)
                                             )
                                             && reservation.Id != Convert.ToInt32(model.reservationId)
                                             select reservation).Any();

                if (!canUpdateReservation)
                {
                    return new()
                    {
                        HasError = true,
                        ExceptionMessage = $"{model.resource} : {model.start} : {model.end}, is not valid a reservation."
                    };
                }

                var oldReservation = (from reservation in _context.TftLocationreservations
                                      where reservation.Id == Convert.ToInt32(model.reservationId)
                                      select reservation).First();

                oldReservation.LocationId = Convert.ToInt32(model.resource);
                oldReservation.ReservationStartDate = model.start;
                oldReservation.ReservationEndDate = model.end;
                oldReservation.ModifiedDate = DateTime.Now;

                _context.TftLocationreservations.Update(oldReservation);
                await _context.SaveChangesAsync();

                var responseData = (from reservation in _context.TftLocationreservations
                                    join location in _context.TftLocations on reservation.LocationId equals location.Id
                                    join locationtype in _context.TluLocationtypes on location.LocationTypeId equals locationtype.Id
                                    join user in _context.AppUsers on reservation.UserId equals user.Id
                                    join status in _context.TluReservationstatuses on reservation.ReservationStatus equals status.Id
                                    where reservation.Id == oldReservation.Id
                                    select new Event
                                    {
                                        start = reservation.ReservationStartDate,
                                        end = reservation.ReservationEndDate,
                                        id = reservation.Id.ToString(),
                                        resource = location.Id.ToString(),
                                        text = $"{location.Name} : {user.Name.ToUpper()} {user.Surname.ToUpper()}",
                                        bubbleHtml = $"{location.Name} : {user.Name.ToUpper()} {user.Surname.ToUpper()} : {status.StatusName}"
                                    }).FirstOrDefault();

                return new()
                {
                    Data = responseData,
                    HasError = false
                };
            }
            catch (Exception ex)
            {
                return new()
                {
                    HasError = true,
                    ExceptionMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResponseModel> DeleteAsync(ReservationDeleteModel model)
        {
            try
            {
                var isAuthorizedUser = await _userService.IsAuthorized(
                    model.email,
                    (int)SectionType.Room_Table_Reservations,
                    (int)ActionType.Delete);

                if (!isAuthorizedUser)
                {
                    return new()
                    {
                        HasError = true,
                        ExceptionMessage = $"{model.email} is not authorized to do this action."
                    };
                }

                var canDeleteReservation = (from reservation in _context.TftLocationreservations
                                            where reservation.Id == Convert.ToInt32(model.reservationId)
                                                && reservation.ReservationStartDate >= DateTime.Now
                                                && reservation.ReservationStatus != (int)ReservationStatusType.Deleted
                                            select reservation).Any();

                if (!canDeleteReservation)
                {
                    return new()
                    {
                        HasError = true,
                        ExceptionMessage = $"{model.reservationId}, can not be deleted."
                    };
                }

                var oldReservation = (from reservation in _context.TftLocationreservations
                                      where reservation.Id == Convert.ToInt32(model.reservationId)
                                      select reservation).First();

                oldReservation.ModifiedDate = DateTime.Now;
                oldReservation.ReservationStatus = (int)ReservationStatusType.Deleted;

                _context.TftLocationreservations.Update(oldReservation);
                await _context.SaveChangesAsync();

                return new()
                {
                    Data = oldReservation,
                    HasError = false
                };
            }
            catch (Exception ex)
            {
                return new()
                {
                    HasError = true,
                    ExceptionMessage = ex.Message
                };
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
	public interface IReservationService
	{
        Task<ResponseModel> GetAsync(ReservationListModel model);
        Task<ResponseModel> AddAsync(ReservationAddModel model);
        Task<ResponseModel> UpdateAsync(ReservationUpdateModel model);
        Task<ResponseModel> DeleteAsync(ReservationDeleteModel model);
    }
}

