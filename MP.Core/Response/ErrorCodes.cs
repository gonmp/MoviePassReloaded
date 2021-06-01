using System;
using System.Collections.Generic;
using System.Text;

namespace MP.Core.Response
{
    public static class ErrorCodes
    {
        //Movies errors
        public const int MovieNotExists = 1;

        //Genres errors
        public const int GenreNotExists = 10;

        //Cinemas errors
        public const int CinemaNotExist = 20;

        //Rooms errors
        public const int RoomNotExists = 30;

        //Shows errors
        public const int ShowNotExists = 40;

        //Purchases errors
        public const int PurchaseNotExists = 50;

        //Tickets errors
        public const int TicketNotExists = 60;

        //Users errors
        public const int UserNotExists = 70;
        public const int InvalidCredentials = 71;
        public const int UserAlreadyExists = 72;

        //Profiles errors
        public const int ProfileNotExists = 80;

        //Stripe errors
        public const int InvalidPayment = 90;
    }
}
