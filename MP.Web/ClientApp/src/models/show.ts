import IMovie from "./movie";
import IRoom from "./room";

enum ShowFields {
    DATE_TIME = "dateTime",
    MOVIE = "movie",
    ROOM = "room"
}

export default interface IShow {
    id?: number,
    [ShowFields.DATE_TIME]: Date,
    [ShowFields.MOVIE]: IMovie,
    [ShowFields.ROOM]: IRoom
}