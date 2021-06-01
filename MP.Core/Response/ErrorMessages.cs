using System;
using System.Collections.Generic;
using System.Text;

namespace MP.Core.Response
{
    public static class ErrorMessages
    {
        //Movies messages
        public static string MovieNotExists(int id)
        {
            return $"There is no movie with id = {id}";
        }

        //Genres messages
        public static string GenreNotExists(int id)
        {
            return $"There is no genre with id = {id}";
        }

        //Cinemas messages
        public static string CinemaNotExists(int id)
        {
            return $"There is no cinema with id = {id}";
        }

        //Rooms messages
        public static string RoomNotExists(int id)
        {
            return $"There is no room with id = {id}";
        }

        //Shows messages
        public static string ShowNotExists(int id)
        {
            return $"There is no show with id = {id}";
        }

        //Purchases messages
        public static string PurchaseNotExists(int id)
        {
            return $"There is no purchase with id = {id}";
        }

        //Tickets messages
        public static string TicketNotExists(int id)
        {
            return $"There is no ticket with id = {id}";
        }

        //Users messages
        public static string UserNotExists(int id)
        {
            return $"There is no user with id = {id}";
        }

        public const string InvalidCredentials = "Email or password invalid";
        public const string UserAlreadyExists = "Email already registered";

        //Profiles messages
        public static string ProfileNotExists(int id)
        {
            return $"There is no profile with id = {id}";
        }

        //Stripe errors
        public const string InvalidPayment = "Is not posible to validate payment";
    }
}
