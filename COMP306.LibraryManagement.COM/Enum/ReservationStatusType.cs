using System;
namespace COMP306.LibraryManagement.COM.Enum
{
	public enum ReservationStatusType
	{
        WaitingApproval = 1,
        Approved = 2,
        Rejected = 3,
        NotEnoughQuota = 4,
        PastTime = 5,
        Deleted = 6
    }
}

