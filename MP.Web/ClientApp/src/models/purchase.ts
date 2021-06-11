enum PurchaseFields {
    TOTAL_NO_DISCOUNT = "totalNoDiscount",
    TOTAL_WITH_DISCOUNT = "totalWithDiscount",
    DISCOUNT = "discount",
    PURCHASE_DATE = "purchaseDate",
    NUMBER_OF_TICKETS = "numberOfTickets",
    USER = "user",
    TICKETS = "tickets"
}

export default interface IPurchase {
    id?: number,
    [PurchaseFields.TOTAL_NO_DISCOUNT]: number,
    [PurchaseFields.TOTAL_WITH_DISCOUNT]: number,
    [PurchaseFields.DISCOUNT]: number,
    [PurchaseFields.PURCHASE_DATE]: Date,
    [PurchaseFields.NUMBER_OF_TICKETS]: number
}