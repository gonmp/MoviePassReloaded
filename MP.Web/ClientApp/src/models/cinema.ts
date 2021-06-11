enum CinemaFields {
    NAME = "name",
    ADDRESS = "address"
}

export default interface ICinema {
    id?: number,
    [CinemaFields.NAME]: string,
    [CinemaFields.ADDRESS]: string
}