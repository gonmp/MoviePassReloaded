import ICinema from "./cinema";

enum RoomFields {
    NAME = "name",
    CAPACITY = "capacity",
    TICKET_VALUE = "ticketValue",
    CINEMA = "cinema"
}

export default interface IRoom {
    id?: number,
    [RoomFields.NAME]: string,
    [RoomFields.CAPACITY]: number,
    [RoomFields.TICKET_VALUE]: number,
    [RoomFields.CINEMA]: ICinema
}