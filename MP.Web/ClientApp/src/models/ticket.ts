import IShow from "./show";

enum TicketFields {
    CODE = "code",
    QR = "qr",
    SHOW = "show"
}

export default interface ITicket {
    id?: number,
    [TicketFields.CODE]: string,
    [TicketFields.QR]: string,
    [TicketFields.SHOW]: IShow
}