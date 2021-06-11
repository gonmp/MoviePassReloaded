enum GenreFields {
    NAME = "name"
}

export default interface IGenre {
    id?: number,
    [GenreFields.NAME]: string
}